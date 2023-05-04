using Lot.Models.Models;
using PagedList;
using Lot.Services;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lot.Controllers
{
    public class EnLotEveryDayController : BaseController
    {
        // GET: EnLotEveryDay
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
            List<LotNumber> DBdata = _lotteryService.GetNumberServices_EveryDay();
            viewModel.LotNumber = DBdata.OrderByDescending(x => x.開獎日期).ToPagedList(page, PageSize);
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult QueryNumber(LotteryViewModel data)
        {
            // 從資料庫撈資料
            List<LotNumber> DBdata = _lotteryService.GetNumberServices_EveryDay();
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
            numList = _lotteryService.GetNumberListServices_EveryDay(selectnum, StartDate, EndDate);
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
            numList = _lotteryService.GetNumberTopServices_EveryDay(PeriodNum);
            SumNumberCount(model, numList);
            ViewBag.Message = "查詢期間為：" + numList.Select(d => d.開獎日期).Min() + "~" + numList.Select(d => d.開獎日期).Max();
            ViewBag.Data = numList.Count;

            if (PeriodNum =="30")
            {
                dynamic data = _lotteryService.GetHot30_EveryDay(numList.Select(d => d.開獎日期).Max());
                model.Hot30CountList = new Dictionary<int?, int?>();
                foreach (KeyValuePair<string, dynamic> pair in data)
                {
                    if (pair.Key.Contains("num"))
                    {
                        string key = pair.Key.Replace("num","");
                        model.Hot30CountList.Add(int.Parse(key), int.Parse(pair.Value));
                       
                    }
                   
                }
            }
            


            return View(model);
        }

        #endregion

        #region 新增號碼
        [HttpGet]
        public ActionResult AddNumber()
        {
            SetMaxNo("EnLotEveryDay");
            LotNumber model = new LotNumber();
            model.開獎日期 = DateTime.Now.ToString("yyyyMMdd");
            return View(model);
        }

        [HttpPost]
        public ActionResult AddNumber(LotNumber data)
        {
            SetMaxNo("EnLotEveryDay");
            if (ModelState.IsValid)
            {
                AddStartZone(data);
                data.期數 = data.開獎日期;
                data.星期 = YYYYMMDDtoDayOfWeek(data.開獎日期);

                //追加儲存30期熱門資料
                SelectLotNumber model = new SelectLotNumber();
                List<LotNumber> numList = new List<LotNumber>();
                numList = _lotteryService.GetNumberTopServices_EveryDay("30");
                SumNumberCount(model, numList);
                SelectHot30 hot30 = new SelectHot30();
                hot30.DrawDate = data.開獎日期;
                hot30.StatisticalDate_ST = numList.Select(d => d.開獎日期).Min();
                hot30.StatisticalDate_ED = numList.Select(d => d.開獎日期).Max();
                hot30.selectNumberCountList = model.selectNumberCountListOrderBy;

                if (_lotteryService.AddNumberServices_EveryDay(data, hot30))
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