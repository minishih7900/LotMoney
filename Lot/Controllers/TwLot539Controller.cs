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
            Lottery5ViewModel viewModel = new Lottery5ViewModel();
            // 從資料庫撈資料
            List<LotNumber5> DBdata = _lotteryService.GetNumberServices_539();
            viewModel.LotNumber = DBdata.OrderByDescending(x => x.DrawDate).ToPagedList(page, PageSize);
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult QueryNumber(Lottery5ViewModel data)
        {
            // 從資料庫撈資料
            List<LotNumber5> DBdata = _lotteryService.GetNumberServices_539();
            data.Page = data.Page == 0 ? 2 : data.Page + 1;

            // 當日期符合開始與結束日期
            if (!string.IsNullOrWhiteSpace(data.StartDate) && !string.IsNullOrWhiteSpace(data.EndDate))
            {
                data.LotNumber = DBdata.Where(p => p.DrawDate.CompareTo(data.StartDate) >= 0 && p.DrawDate.CompareTo(data.EndDate) <= 0).OrderByDescending(m => m.DrawDate).ToPagedList(data.Page > 0 ? data.Page - 1 : 0, PageSize);
            }
            else
            {
                data.LotNumber = DBdata.OrderByDescending(x => x.DrawDate).ToPagedList(data.Page > 0 ? data.Page - 1 : 0, PageSize);
            }
            return View(data);
        }
        #endregion

        #region 查詢號碼不分頁
        [HttpGet]
        public ActionResult QueryNumberNoPage()
        {
            // 進入搜尋頁面 不主動撈取資料
            Lottery5ViewModel viewModel = new Lottery5ViewModel();
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult QueryNumberNoPage(Lottery5ViewModel data)
        {
            // 從資料庫撈資料
            List<LotNumber5> DBdata = _lotteryService.GetNumberServices_539();

            //
            if (!string.IsNullOrEmpty(data.Submit))
            {
                data.LotNumberNoPage = DBdata.Where(p => p.DrawDate.StartsWith(data.Submit)).ToList();
                return View(data);
            }

            // 當日期符合開始與結束日期
            if (!string.IsNullOrWhiteSpace(data.StartDate) && !string.IsNullOrWhiteSpace(data.EndDate))
            {
                data.LotNumberNoPage = DBdata.Where(p => p.DrawDate.CompareTo(data.StartDate) >= 0 && p.DrawDate.CompareTo(data.EndDate) <= 0).OrderBy(m => m.DrawDate).ToList();
            }
            else
            {
                data.LotNumberNoPage = DBdata.OrderBy(x => x.DrawDate).ToList();
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
            List<LotNumber5> numList = new List<LotNumber5>();
            if (string.IsNullOrEmpty(StartDate) && string.IsNullOrEmpty(EndDate) && nowYear)
            {
                StartDate = DateTime.Now.Year.ToString() + "0101";
                EndDate = DateTime.Now.Year.ToString() + "1231";
            }
            model.nowYear = nowYear;
            numList = _lotteryService.GetNumberListServices_539(selectnum, StartDate, EndDate);
            SumNumberCount(model, numList);
            ViewBag.Message = "查詢期間為：" + numList.Select(d => d.DrawDate).Min() + "~" + numList.Select(d => d.DrawDate).Max();
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
            List<LotNumber5> numList = new List<LotNumber5>();
            numList = _lotteryService.GetNumberTopServices_539(PeriodNum);
            SumNumberCount(model, numList);
            ViewBag.Message = "查詢期間為：" + numList.Select(d => d.DrawDate).Min() + "~" + numList.Select(d => d.DrawDate).Max();
            ViewBag.Data = numList.Count;

            if (PeriodNum == "30")
            {
                dynamic data = _lotteryService.GetHot30_539(numList.Select(d => d.DrawDate).Max());
                model.Hot30CountList = new Dictionary<int?, int?>();
                foreach (KeyValuePair<string, dynamic> pair in data)
                {
                    if (pair.Key.Contains("Num"))
                    {
                        string key = pair.Key.Replace("Num", "");
                        model.Hot30CountList.Add(int.Parse(key), int.Parse(pair.Value));

                    }

                }
            }
            return View(model);
        }

        #endregion

        #region 未開次數
        [HttpGet]
        public ActionResult UnopenedCount()
        {
            SelectLotNumber model = new SelectLotNumber();
            dynamic data = _lotteryService.GetStoredCount_539();
            model.selectNumberCountListOrderBy = new Dictionary<int?, int?>();
            foreach (KeyValuePair<string, dynamic> pair in data)
            {
                if (pair.Key.Contains("Num"))
                {
                    string key = pair.Key.Replace("Num", "");
                    model.selectNumberCountListOrderBy.Add(int.Parse(key), int.Parse(pair.Value));
                }
            }
            model.selectNumberCountListOrderBy = model.selectNumberCountListOrderBy.OrderByDescending(d => d.Value).ToDictionary(dkey => dkey.Key, dvalue => dvalue.Value);
            ViewBag.MaxDate = data.DrawDate;
            return View(model);
        }
        #endregion

        #region 新增號碼
        [HttpGet]
        public ActionResult AddNumber()
        {
            SetMaxNo("TwLot539");
            LotNumber5 model = new LotNumber5();
            model.DrawDate = DateTime.Now.ToString("yyyyMMdd");
            return View(model);
        }

        [HttpPost]
        public ActionResult AddNumber(LotNumber5 data)
        {
            SetMaxNo("TwLot539");
            if (ModelState.IsValid)
            {
                AddStartZone(data);
                data.Period = data.DrawDate;
                data.Week = YYYYMMDDtoDayOfWeek(data.DrawDate);

                //追加儲存30期熱門資料
                SelectLotNumber model = new SelectLotNumber();
                List<LotNumber5> numList = new List<LotNumber5>();
                numList = _lotteryService.GetNumberTopServices_539("30");
                SumNumberCount(model, numList);
                SelectHot30 hot30 = new SelectHot30();
                hot30.DrawDate = data.DrawDate;
                hot30.StatisticalDate_ST = numList.Select(d => d.DrawDate).Min();
                hot30.StatisticalDate_ED = numList.Select(d => d.DrawDate).Max();
                hot30.selectNumberCountList = model.selectNumberCountListOrderBy;

                if (_lotteryService.AddNumberServices_539(data, hot30))
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