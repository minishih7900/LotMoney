using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lot.Models.Models
{
    public class LotNumber
    {
        public string 期數 { get; set; }
        [Required]
        public string 開獎日期 { get; set; }
        public string 星期 { get; set; }
        [Required]
        public string 號碼1 { get; set; }
        [Required]
        public string 號碼2 { get; set; }
        [Required]
        public string 號碼3 { get; set; }
        [Required]
        public string 號碼4 { get; set; }
        [Required]
        public string 號碼5 { get; set; }
    }
    public class SelectLotNumber
    {
        public int[] selectNumberCountDarry { get; set; }
        public Dictionary<int?, int?> selectNumberCountList { get; set; }
        public Dictionary<int?, int?> selectNumberCountListOrderBy { get; set; }
        public Dictionary<int?, int?> Hot30CountList { get; set; }
        public bool nowYear { get; set; }
    }
    public class SelectHot30
    {
        public string DrawDate { get; set; }
        public string StatisticalDate_ST { get; set; }
        public string StatisticalDate_ED { get; set; }
        public Dictionary<int?, int?> selectNumberCountList { get; set; }
    }

}
