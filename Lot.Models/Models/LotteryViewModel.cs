﻿using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList.Mvc;


namespace Lot.Models.Models
{
    public class LotteryViewModel
    {
        // Properties
        public string StartDate { get; set; }  // 搜尋條件1

        public string EndDate { get; set; }  // 搜尋條件2

        public IPagedList<LotNumber> LotNumber { get; set; }  // 符合條件資料
        public int Page { get; set; }  // 頁碼

        public List<LotNumber> LotNumberNoPage { get; set; }  // 符合條件資料不用分頁
        public string Submit { get; set; }  // 確定按鈕

        // Constructors
        public LotteryViewModel()
        {
            StartDate = string.Empty;
            EndDate = string.Empty;
            Page = 0;
        }
    }
    public class Lottery5ViewModel
    {
        // Properties
        public string StartDate { get; set; }  // 搜尋條件1

        public string EndDate { get; set; }  // 搜尋條件2

        public IPagedList<LotNumber5> LotNumber { get; set; }  // 符合條件資料
        public int Page { get; set; }  // 頁碼

        public List<LotNumber5> LotNumberNoPage { get; set; }  // 符合條件資料不用分頁
        public string Submit { get; set; }  // 確定按鈕

        // Constructors
        public Lottery5ViewModel()
        {
            StartDate = string.Empty;
            EndDate = string.Empty;
            Page = 0;
        }
    }

}
