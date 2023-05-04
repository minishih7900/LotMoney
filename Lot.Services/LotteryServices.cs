﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lot.DataAccess;
using System.Transactions;
using System.Globalization;
using Lot.Models.Models;

namespace Study.Services
{
    public class LotteryServices
    {
        public SqlRepository _sqlRepository = new SqlRepository();

        /// <summary>
        /// 依期數TOP查詢筆數
        /// </summary>
        /// <param name="newCount"></param>
        /// <returns></returns>
        public List<LotNumber> GetNumberTopServices_539(string newCount)
        {
            return _sqlRepository.GetLotNumberNewTop(newCount);
        }
        public List<LotNumber> GetNumberTopServices_EveryDay(string newCount)
        {
            return _sqlRepository.GetLotNumberNewTop_EveryDay(newCount);
        }
        /// <summary>
        /// 取得熱門30期資料
        /// </summary>
        /// <param name="newCount"></param>
        /// <returns></returns>
        public dynamic GetHot30_EveryDay(string newCount)
        {
            return _sqlRepository.GetHot30_EveryDay(newCount);
        }
        /// <summary>
        /// 依開始日期結束日期查詢筆數
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public List<LotNumber> GetNumberServices(string StartDate, string EndDate)
        {
            return _sqlRepository.GetLotNumber(StartDate, EndDate);
        }

        public List<LotNumber> GetNumberServices_539()
        {
            return _sqlRepository.GetLotNumber();
        }
        public List<LotNumber> GetNumberServices_EveryDay()
        {
            return _sqlRepository.GetLotNumber_EveryDay();
        }

        public List<LotNumber> GetNumberListServices_539(string selectnum, string StartDate, string EndDate)
        {
            return _sqlRepository.GetSelectLotNumber(selectnum, StartDate, EndDate);
        }
        public List<LotNumber> GetNumberListServices_EveryDay(string selectnum, string StartDate, string EndDate)
        {
            return _sqlRepository.GetSelectLotNumber_EveryDay(selectnum, StartDate, EndDate);
        }

        public string GetMaxNoServices(string dbName)
        {
            return _sqlRepository.GetMaxNo(dbName);
        }

        /// <summary>
        /// 新增號碼
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool AddNumberServices_539(LotNumber data)
        {
            bool success = false;

            using (var scope = new TransactionScope())
            {
                success = _sqlRepository.InputLotNumber(data) &&
                          _sqlRepository.InsertCopyNumber(data.開獎日期) && 
                          _sqlRepository.UpdateCopyNumber(data);
                if (success)
                    scope.Complete();
            }
            return success;
        }
        public bool AddNumberServices_EveryDay(LotNumber data, SelectHot30 hot30)
        {
            bool success = false;

            using (var scope = new TransactionScope())
            {
                success = _sqlRepository.InputLotNumber_EveryDay(data) &&
                          _sqlRepository.InsertCopyNumber_EveryDay(data.開獎日期) &&
                          _sqlRepository.UpdateCopyNumber_EveryDay(data) &&
                          _sqlRepository.InsertHot30_EveryDay(hot30);
                if (success)
                    scope.Complete();
            }
            return success;
        }
    }
}
