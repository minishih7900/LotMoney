using Lot.Models.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lot.Controllers
{
    public class TwLot539Controller : BaseController
    {
        // GET: TwLot539
        public ActionResult Index()
        {
            return View();
        }

        #region 查詢號碼
        [HttpGet]
        public ActionResult QueryNumber(int page = 1)
        {
            // 進入搜尋頁面 不主動撈取資料
            LotteryViewModel viewModel = new LotteryViewModel();
            // 從資料庫撈資料
            List<LotNumber> DBdata = _lotteryService.GetNumberServices_539();
            viewModel.LotNumber = DBdata.OrderByDescending(x => x.開獎日期).ToPagedList(page, PageSize);
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult QueryNumber(LotteryViewModel data)
        {
            // 從資料庫撈資料
            List<LotNumber> DBdata = _lotteryService.GetNumberServices_539();
            data.Page = data.Page == 0 ? 2 : data.Page + 1;

            // 當日期符合開始與結束日期
            if (!string.IsNullOrWhiteSpace(data.StartDate) && !string.IsNullOrWhiteSpace(data.EndDate))
            {
                data.LotNumber = DBdata.Where(p => p.開獎日期.CompareTo(data.StartDate) >= 0 && p.開獎日期.CompareTo(data.EndDate) <= 0).OrderByDescending(m => m.開獎日期).ToPagedList(data.Page > 0 ? data.Page - 1 : 0, PageSize);
            }
            else
            {
                data.LotNumber = DBdata.OrderByDescending(x => x.開獎日期).ToPagedList(data.Page > 0 ? data.Page - 1 : 0, PageSize);
            }
            return View(data);
        }
        #endregion

        #region 查詢號碼不分頁
        [HttpGet]
        public ActionResult QueryNumberNoPage()
        {
            // 進入搜尋頁面 不主動撈取資料
            LotteryViewModel viewModel = new LotteryViewModel();
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult QueryNumberNoPage(LotteryViewModel data)
        {
            // 從資料庫撈資料
            List<LotNumber> DBdata = _lotteryService.GetNumberServices_539();

            //
            if (!string.IsNullOrEmpty(data.Submit))
            {
                data.LotNumberNoPage = DBdata.Where(p => p.開獎日期.StartsWith(data.Submit)).ToList();
                return View(data);
            }

            // 當日期符合開始與結束日期
            if (!string.IsNullOrWhiteSpace(data.StartDate) && !string.IsNullOrWhiteSpace(data.EndDate))
            {
                data.LotNumberNoPage = DBdata.Where(p => p.開獎日期.CompareTo(data.StartDate) >= 0 && p.開獎日期.CompareTo(data.EndDate) <= 0).OrderBy(m => m.開獎日期).ToList();
            }
            else
            {
                data.LotNumberNoPage = DBdata.OrderBy(x => x.開獎日期).ToList();
            }
            return View(data);
        }
        #endregion

        #region 最合數字
        [HttpGet]
        public ActionResult QuerySelectNumberCount()
        {
            SelectLotNumber model = new SelectLotNumber();
            model.selectNumberCountDarry = new int[40];
            model.selectNumberCountListOrderBy = new Dictionary<int?, int?>();
            return View(model);
        }
        [HttpPost]
        public ActionResult QuerySelectNumberCount(string selectnum, string StartDate, string EndDate, bool nowYear)
        {

            SelectLotNumber model = new SelectLotNumber();
            List<LotNumber> numList = new List<LotNumber>();
            if (string.IsNullOrEmpty(StartDate) && string.IsNullOrEmpty(EndDate) && nowYear)
            {
                StartDate = DateTime.Now.Year.ToString() + "0101";
                EndDate = DateTime.Now.Year.ToString() + "1231";
            }
            model.nowYear = nowYear;
            numList = _lotteryService.GetNumberListServices_539(selectnum, StartDate, EndDate);
            SumNumberCount(model, numList);
            ViewBag.Message = "查詢期間為：" + numList.Select(d => d.開獎日期).Min() + "~" + numList.Select(d => d.開獎日期).Max();
            ViewBag.Data = numList.Count;

            return View(model);
        }

        #endregion

        #region 熱門牌
        [HttpGet]
        public ActionResult HotNumber()
        {
            return View();
        }
        [HttpPost]
        public ActionResult HotNumber(string PeriodNum)
        {
            SelectLotNumber model = new SelectLotNumber();
            List<LotNumber> numList = new List<LotNumber>();
            numList = _lotteryService.GetNumberTopServices_539(PeriodNum);
            SumNumberCount(model, numList);
            ViewBag.Message = "查詢期間為：" + numList.Select(d => d.開獎日期).Min() + "~" + numList.Select(d => d.開獎日期).Max();
            ViewBag.Data = numList.Count;
            return View(model);
        }

        #endregion

        #region 新增號碼
        [HttpGet]
        public ActionResult AddNumber()
        {
            SetMaxNo("TwLot539");
            LotNumber model = new LotNumber();
            model.開獎日期 = DateTime.Now.ToString("yyyyMMdd");
            return View(model);
        }

        [HttpPost]
        public ActionResult AddNumber(LotNumber data)
        {
            SetMaxNo("TwLot539");
            if (ModelState.IsValid)
            {
                AddStartZone(data);
                data.期數 = data.開獎日期;
                data.星期 = YYYYMMDDtoDayOfWeek(data.開獎日期);
                if (_lotteryService.AddNumberServices_539(data))
                {
                    TempData["message"] = "寫入成功";
                    return RedirectToAction("AddNumber");
                }
            }
            TempData["message"] = "寫入失敗";
            return View();
        }
        #endregion
    }
}