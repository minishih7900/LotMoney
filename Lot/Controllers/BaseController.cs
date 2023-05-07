using Lot.Models.Models;
using Study.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;

namespace Lot.Controllers
{
    public class BaseController : Controller
    {
        protected LotteryServices _lotteryService = new LotteryServices();
        // 單頁可容納之資料筆數(可參數化此數值)
        protected const int PageSize = 10;
        // GET: Base
        #region 通用
        /// <summary>
        /// 計算查詢結果的各號碼總計
        /// </summary>
        /// <param name="model"></param>
        /// <param name="numList"></param>
        /// <returns></returns>
        protected SelectLotNumber SumNumberCount(SelectLotNumber model, List<LotNumber5> numList)
        {
            Dictionary<int?, int?> numDic = new Dictionary<int?, int?>();
            model.selectNumberCountDarry = new int[40];
            foreach (var item in numList)
            {
                model.selectNumberCountDarry[int.Parse(item.Num1)] = model.selectNumberCountDarry[int.Parse(item.Num1)] + 1;
                model.selectNumberCountDarry[int.Parse(item.Num2)] = model.selectNumberCountDarry[int.Parse(item.Num2)] + 1;
                model.selectNumberCountDarry[int.Parse(item.Num3)] = model.selectNumberCountDarry[int.Parse(item.Num3)] + 1;
                model.selectNumberCountDarry[int.Parse(item.Num4)] = model.selectNumberCountDarry[int.Parse(item.Num4)] + 1;
                model.selectNumberCountDarry[int.Parse(item.Num5)] = model.selectNumberCountDarry[int.Parse(item.Num5)] + 1;
            }
            for (int i = 1; i < 40; i++)
            {
                numDic.Add(i, model.selectNumberCountDarry[i]);
            }
            model.selectNumberCountList = numDic;
            model.selectNumberCountListOrderBy = numDic.OrderByDescending(d => d.Value).ToDictionary(dkey => dkey.Key, dvalue => dvalue.Value);
            return model;
        }

        /// <summary>
        /// 取得最大期數
        /// </summary>
        protected void SetMaxNo(string dbName)
        {
            ViewBag.MaxNo = (Convert.ToUInt32(_lotteryService.GetMaxNoServices(dbName))).ToString().PadLeft(8, '0');
        }

        /// <summary>
        /// 判斷號碼是否有重覆
        /// </summary>
        /// <param name="data"></param>
        protected void CheckRepeatFun(LotNumber data)
        {
            List<string> checkRepeat = new List<string> { data.號碼1, data.號碼2, data.號碼3, data.號碼4, data.號碼5 };
            //List<string> RepeatData = new List<string>();
            //for (int i = 0; i < checkRepeat.Count; i++)
            //{
            //    for (int j = i + 1; j < checkRepeat.Count; j++)
            //    {
            //        if (checkRepeat[i] == checkRepeat[j])
            //        {
            //            RepeatData.Add(checkRepeat[i]);
            //        }
            //    }
            //}
            //var dd = from p in checkRepeat
            //         group p by p.ToString() into g
            //         where g.Count() > 1//出現1次以上的數字
            //         select g.Key;
            //TempData["message"] = checkRepeat.GroupBy(p=>p).Where(g=>g.Count()>1).Select(m=>m.Key);
            if (checkRepeat.GroupBy(p => p).Count() > 1)
            {
                TempData["message"] = "重覆號碼：" + checkRepeat.GroupBy(p => p).Where(g => g.Count() > 1).Select(m => m.Key);
            }
            else
            {
                TempData["message"] = "無重覆值";
            }
        }

        /// <summary>
        /// 去掉前面0
        /// </summary>
        /// <param name="data"></param>
        protected string TrimStartZone(string data)
        {
            data = data.TrimStart('0');
            return data;
        }
        protected void AddStartZone(LotNumber5 data)
        {
            data.Num1 = data.Num1.PadLeft(2, '0');
            data.Num2 = data.Num2.PadLeft(2, '0');
            data.Num3 = data.Num3.PadLeft(2, '0');
            data.Num4 = data.Num4.PadLeft(2, '0');
            data.Num5 = data.Num5.PadLeft(2, '0');
        }
        /// <summary>
        /// yyyymmdd轉換星期，且刪除「週」
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected string YYYYMMDDtoDayOfWeek(string data)
        {
            DateTime date = DateTime.ParseExact(data, "yyyyMMdd", null);
            string taiwanDayOfWeek = date.ToString("ddd", new CultureInfo("zh-TW"));
            return taiwanDayOfWeek.Replace("週","");
        }
    }
        #endregion
}