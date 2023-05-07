using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lot.Models.Models;


namespace Lot.DataAccess
{
    public class SqlRepository : BaseRepository
    {
        public bool insertMember(List<member> mm)
        {
            var sql = @"INSERT INTO [dbo].[member]
           ([email]
           ,[password])
             VALUES
           (@email,
           @password)";
            return _dbDapper.NonQuerySQL(sql, mm) > 0;
        }
        public List<LotNumber5> GetSelectLotNumber(string selectnum, string StartDate, string EndDate)
        {
            var sqlTemp = "";
            var sql = @"
select * from [dbo].[TwLot539]
where (Num1=@num or Num2=@num or  Num3=@num or Num4=@num or Num5=@num) {0}
";
            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
            {
                sqlTemp = sqlTemp + "and (DrawDate between @startdate and @enddate) ";
            }
            
            sql = string.Format(sql, sqlTemp);

            var param = new Dictionary<string, object>();
            param.Add("num", selectnum);
            param.Add("startdate", StartDate);
            param.Add("enddate", EndDate);
           

            return _dbDapper.QueryList<LotNumber5>(sql, param);
        }
        public List<LotNumber5> GetLotNumber()
        {
            var sql = @"
SELECT           *
FROM              TwLot539
";
            return _dbDapper.QueryList<LotNumber5>(sql, null);
        }
        public List<LotNumber5> GetLotNumberNewTop(string newCount)
        {
            var sql = "";
            if (newCount == "All")
            {
                sql = @"
SELECT   *
FROM              TwLot539
order by Period desc";
            }
            else
            {
                sql = @"
SELECT   Top " + newCount + @"*
FROM              TwLot539
order by Period desc";
            }


            return _dbDapper.QueryList<LotNumber5>(sql, null);
        }
        public bool InputLotNumber(LotNumber5 data)
        {
            var sql = @"
INSERT INTO [dbo].[TwLot539]
           ([Period]
           ,[DrawDate]
           ,[Week]
           ,[Num1]
           ,[Num2]
           ,[Num3]
           ,[Num4]
           ,[Num5])
     VALUES
           (@Period
           ,@DrawDate
           ,@Week
           ,@Num1
           ,@Num2
           ,@Num3
           ,@Num4
           ,@Num5)
";
            return _dbDapper.NonQuerySQL(sql, data) > 0;
        }

        public string GetMaxNo(string dbName)
        {
            var sql =  $"select max(Period) from [dbo].[{dbName}]";
            
            return _dbDapper.ExecuteScalarSQL<string>(sql, null);
        }
        public bool InsertCopyNumber(string data)
        {
            var sql = @"
            IF(NOT EXISTS(SELECT top 1 * FROM [dbo].[TwLot539_StoredCount] ))
BEGIN
	 insert into [dbo].[TwLot539_StoredCount]
	(DrawDate,Num01,Num02,Num03,Num04,Num05,Num06,Num07,Num08,Num09,Num10
      ,Num11,Num12,Num13,Num14,Num15,Num16,Num17,Num18,Num19,Num20
      ,Num21,Num22,Num23,Num24,Num25,Num26,Num27,Num28,Num29,Num30
      ,Num31,Num32,Num33,Num34,Num35,Num36,Num37,Num38,Num39)
	  VALUES
	  ( @data,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0)
END	
    ELSE
BEGIN
    insert into [dbo].[TwLot539_StoredCount]
SELECT TOP (1) @data
      ,[Num01]+1 
      ,[Num02]+1
      ,[Num03]+1
      ,[Num04]+1 
      ,[Num05]+1
      ,[Num06]+1
      ,[Num07]+1 
      ,[Num08]+1 
      ,[Num09]+1 
      ,[Num10]+1 
      ,[Num11]+1 
      ,[Num12]+1 
      ,[Num13]+1 
      ,[Num14]+1 
      ,[Num15]+1 
      ,[Num16]+1 
      ,[Num17]+1 
      ,[Num18]+1
      ,[Num19]+1 
      ,[Num20]+1 
      ,[Num21]+1 
      ,[Num22]+1 
      ,[Num23]+1 
      ,[Num24]+1 
      ,[Num25]+1 
      ,[Num26]+1
      ,[Num27]+1 
      ,[Num28]+1 
      ,[Num29]+1 
      ,[Num30]+1 
      ,[Num31]+1 
      ,[Num32]+1 
      ,[Num33]+1 
      ,[Num34]+1 
      ,[Num35]+1 
      ,[Num36]+1
      ,[Num37]+1 
      ,[Num38]+1 
      ,[Num39]+1 
       FROM [dbo].[TwLot539_StoredCount] order by DrawDate desc
END";
            var param = new Dictionary<string, object>();
            param.Add("data", data);

            return _dbDapper.NonQuerySQL(sql, param) > 0;
        }

        public bool UpdateCopyNumber(LotNumber5 data)
        {
            var sql = @"
            DECLARE @TSQL NVARCHAR(4000)
SET @TSQL =	'update [dbo].[TwLot539_StoredCount]
set ' + @num1 + '=0,' + @num2 + '=0,' + @num3 +'=0,' + @num4 + '=0,' + @num5 + '=0
where DrawDate =' + @date
EXEC SP_EXECUTESQL @TSQL";
            var param = new Dictionary<string, object>();
            param.Add("num1", "[Num" + data.Num1 + "]");
            param.Add("num2", "[Num" + data.Num2 + "]");
            param.Add("num3", "[Num" + data.Num3 + "]");
            param.Add("num4", "[Num" + data.Num4 + "]");
            param.Add("num5", "[Num" + data.Num5 + "]");
            param.Add("date", data.DrawDate);

            return _dbDapper.NonQuerySQL(sql, param) > 0;
        }

        #region EnLotEveryDay
        public List<LotNumber5> GetLotNumberNewTop_EveryDay(string newCount)
        {
            var sql = "";
            if (newCount == "All")
            {
                sql = @"
SELECT   *
FROM              EnLotEveryDay
order by Period desc";
            }
            else
            {
                sql = @"
SELECT   Top " + newCount + @"*
FROM              EnLotEveryDay
order by Period desc";
            }


            return _dbDapper.QueryList<LotNumber5>(sql, null);
        }
        public dynamic GetHot30_EveryDay(string date)
        {
            var sql = "";
           
                sql = @"
SELECT  DrawDate, StatisticalDate_ST, StatisticalDate_ED, 
Num01,  Num02, Num03, Num04, Num05, Num06, Num07, Num08, Num09,
Num10, Num11, Num12, Num13, Num14, Num15, Num16, Num17, Num18, Num19, 
Num20, Num21, Num22, Num23, Num24, Num25, Num26, Num27, Num28, Num29,
Num30, Num31, Num32, Num33, Num34, Num35, Num36, Num37, Num38, Num39
FROM              EnLotEveryDay_Hot30
WHERE DrawDate=@date ";

            var param = new Dictionary<string, object>();
            param.Add("date", date);

            return _dbDapper.QueryList<dynamic>(sql, param).FirstOrDefault();
        }
        public List<LotNumber5> GetLotNumber_EveryDay()
        {
            var sql = @"
SELECT           *
FROM              EnLotEveryDay
";
            return _dbDapper.QueryList<LotNumber5>(sql, null);
        }
        public List<LotNumber5> GetSelectLotNumber_EveryDay(string selectnum, string StartDate, string EndDate)
        {
            var sqlTemp = "";
            var sql = @"
select * from [dbo].[EnLotEveryDay]
where (Num1=@num or Num2=@num or  Num3=@num or Num4=@num or Num5=@num) {0}
";
            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
            {
                sqlTemp = sqlTemp + "and (DrawDate between @startdate and @enddate) ";
            }

            sql = string.Format(sql, sqlTemp);

            var param = new Dictionary<string, object>();
            param.Add("num", selectnum);
            param.Add("startdate", StartDate);
            param.Add("enddate", EndDate);


            return _dbDapper.QueryList<LotNumber5>(sql, param);
        }

        public bool InputLotNumber_EveryDay(LotNumber5 data)
        {
            var sql = @"
INSERT INTO [dbo].[EnLotEveryDay]
           ([Period]
           ,[DrawDate]
           ,[Week]
           ,[Num1]
           ,[Num2]
           ,[Num3]
           ,[Num4]
           ,[Num5])
     VALUES
           (@Period
           ,@DrawDate
           ,@Week
           ,@Num1
           ,@Num2
           ,@Num3
           ,@Num4
           ,@Num5)
";
            return _dbDapper.NonQuerySQL(sql, data) > 0;
        }
        public bool InsertCopyNumber_EveryDay(string data)
        {
            var sql = @"
            IF(NOT EXISTS(SELECT top 1 * FROM [dbo].[EnLotEveryDay_StoredCount] ))
BEGIN
	 insert into [dbo].[EnLotEveryDay_StoredCount]
	(DrawDate,Num01,Num02,Num03,Num04,Num05,Num06,Num07,Num08,Num09,Num10
      ,Num11,Num12,Num13,Num14,Num15,Num16,Num17,Num18,Num19,Num20
      ,Num21,Num22,Num23,Num24,Num25,Num26,Num27,Num28,Num29,Num30
      ,Num31,Num32,Num33,Num34,Num35,Num36,Num37,Num38,Num39)
	  VALUES
	  ( @data,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0)
END	
    ELSE
BEGIN
    insert into [dbo].[EnLotEveryDay_StoredCount]
SELECT TOP (1) @data
      ,[Num01]+1 
      ,[Num02]+1
      ,[Num03]+1
      ,[Num04]+1 
      ,[Num05]+1
      ,[Num06]+1
      ,[Num07]+1 
      ,[Num08]+1 
      ,[Num09]+1 
      ,[Num10]+1 
      ,[Num11]+1 
      ,[Num12]+1 
      ,[Num13]+1 
      ,[Num14]+1 
      ,[Num15]+1 
      ,[Num16]+1 
      ,[Num17]+1 
      ,[Num18]+1
      ,[Num19]+1 
      ,[Num20]+1 
      ,[Num21]+1 
      ,[Num22]+1 
      ,[Num23]+1 
      ,[Num24]+1 
      ,[Num25]+1 
      ,[Num26]+1
      ,[Num27]+1 
      ,[Num28]+1 
      ,[Num29]+1 
      ,[Num30]+1 
      ,[Num31]+1 
      ,[Num32]+1 
      ,[Num33]+1 
      ,[Num34]+1 
      ,[Num35]+1 
      ,[Num36]+1
      ,[Num37]+1 
      ,[Num38]+1 
      ,[Num39]+1 
       FROM [dbo].[EnLotEveryDay_StoredCount] order by DrawDate desc
END";
            var param = new Dictionary<string, object>();
            param.Add("data", data);

            return _dbDapper.NonQuerySQL(sql, param) > 0;
        }
        public bool UpdateCopyNumber_EveryDay(LotNumber5 data)
        {
            var sql = @"
            DECLARE @TSQL NVARCHAR(4000)
SET @TSQL =	'update [dbo].[EnLotEveryDay_StoredCount]
set ' + @num1 + '=0,' + @num2 + '=0,' + @num3 +'=0,' + @num4 + '=0,' + @num5 + '=0
where DrawDate=' + @date
EXEC SP_EXECUTESQL @TSQL";
            var param = new Dictionary<string, object>();
            param.Add("num1", "[Num" + data.Num1 + "]");
            param.Add("num2", "[Num" + data.Num2 + "]");
            param.Add("num3", "[Num" + data.Num3 + "]");
            param.Add("num4", "[Num" + data.Num4 + "]");
            param.Add("num5", "[Num" + data.Num5 + "]");
            param.Add("date", data.DrawDate);

            return _dbDapper.NonQuerySQL(sql, param) > 0;
        }

        public bool InsertHot30_EveryDay(SelectHot30 data)
        {
            var sql = @"
            
    INSERT INTO [dbo].[EnLotEveryDay_Hot30]
           ([DrawDate]
           ,[StatisticalDate_ST]
           ,[StatisticalDate_ED]
           ,[Num01]
           ,[Num02]
           ,[Num03]
           ,[Num04]
           ,[Num05]
           ,[Num06]
           ,[Num07]
           ,[Num08]
           ,[Num09]
           ,[Num10]
           ,[Num11]
           ,[Num12]
           ,[Num13]
           ,[Num14]
           ,[Num15]
           ,[Num16]
           ,[Num17]
           ,[Num18]
           ,[Num19]
           ,[Num20]
           ,[Num21]
           ,[Num22]
           ,[Num23]
           ,[Num24]
           ,[Num25]
           ,[Num26]
           ,[Num27]
           ,[Num28]
           ,[Num29]
           ,[Num30]
           ,[Num31]
           ,[Num32]
           ,[Num33]
           ,[Num34]
           ,[Num35]
           ,[Num36]
           ,[Num37]
           ,[Num38]
           ,[Num39])
     VALUES
           (@DrawDate
           ,@StatisticalDate_ST
           ,@StatisticalDate_ED
           ,@01
           ,@02
           ,@03
           ,@04
           ,@05
           ,@06
           ,@07
           ,@08
           ,@09
           ,@10
           ,@11
           ,@12
           ,@13
           ,@14
           ,@15
           ,@16
           ,@17
           ,@18
           ,@19
           ,@20
           ,@21
           ,@22
           ,@23
           ,@24
           ,@25
           ,@26
           ,@27
           ,@28
           ,@29
           ,@30
           ,@31
           ,@32
           ,@33
           ,@34
           ,@35
           ,@36
           ,@37
           ,@38
           ,@39)
";
            var param = new Dictionary<string, object>();
            param.Add("DrawDate", data.DrawDate);
            param.Add("StatisticalDate_ST", data.StatisticalDate_ST);
            param.Add("StatisticalDate_ED", data.StatisticalDate_ED);
            for (int i = 1; i < 40; i++)
            {
                param.Add(i.ToString().PadLeft(2,'0'), data.selectNumberCountList[i]);
            }

            return _dbDapper.NonQuerySQL(sql, param) > 0;
        }

        #endregion

        #region HkLot49
        public List<LotNumber6> GetLotNumber6()
        {
            var sql = @"
SELECT           *
FROM              HkLot49
";
            return _dbDapper.QueryList<LotNumber6>(sql, null);
        }
        public List<LotNumber6> GetLotNumber6(string startDate, string endDate)
        {
            var sql = @"
SELECT           *
FROM              HkLot49
WHERE          (開獎日期 BETWEEN @StartDate AND @EndDate)
";
            return _dbDapper.QueryList<LotNumber6>(sql, new { StartDate = startDate, EndDate = endDate });
        }
        public bool InputLotNumber6(LotNumber6 data)
        {
            var sql = @"
INSERT INTO [dbo].[HkLot49]
           ([期數]
           ,[開獎日期]
           ,[號碼1]
           ,[號碼2]
           ,[號碼3]
           ,[號碼4]
           ,[號碼5]
           ,[號碼6]
           ,[特別號])
     VALUES
           (@期數
           ,@開獎日期
           ,@號碼1
           ,@號碼2
           ,@號碼3
           ,@號碼4
           ,@號碼5
           ,@號碼6
           ,@特別號)
";
            return _dbDapper.NonQuerySQL(sql, data) > 0;
        }
        public bool InsertCopyNumber6(string data)
        {
            var sql = @"
            IF(NOT EXISTS(SELECT top 1 * FROM [dbo].[HkLot49_StoredCount] ))
BEGIN
	 insert into [dbo].[HkLot49_StoredCount]
	(日期,[1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13]
      ,[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26]
      ,[27],[28],[29],[30],[31],[32],[33],[34],[35],[36],[37],[38],[39],[40],[41],[42],[43],[44],[45],[46],[47],[48],[49])
	  VALUES
	  ( @data,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0)
END	
    ELSE
BEGIN
    insert into [dbo].[HkLot49_StoredCount]
SELECT TOP (1) @data
      ,[1]+1 as [1]
      ,[2]+1 as [2]
      ,[3]+1 as [3]
      ,[4]+1 as [4]
      ,[5]+1 as [5]
      ,[6]+1 as [6]
      ,[7]+1 as [7]
      ,[8]+1 as [8]
      ,[9]+1 as [9]
      ,[10]+1 as [10]
      ,[11]+1 as [11]
      ,[12]+1 as [12]
      ,[13]+1 as [13]
      ,[14]+1 as [14]
      ,[15]+1 as [15]
      ,[16]+1 as [16]
      ,[17]+1 as [17]
      ,[18]+1 as [18]
      ,[19]+1 as [19]
      ,[20]+1 as [20]
      ,[21]+1 as [21]
      ,[22]+1 as [22]
      ,[23]+1 as [23]
      ,[24]+1 as [24]
      ,[25]+1 as [25]
      ,[26]+1 as [26]
      ,[27]+1 as [27]
      ,[28]+1 as [28]
      ,[29]+1 as [29]
      ,[30]+1 as [30]
      ,[31]+1 as [31]
      ,[32]+1 as [32]
      ,[33]+1 as [33]
      ,[34]+1 as [34]
      ,[35]+1 as [35]
      ,[36]+1 as [36]
      ,[37]+1 as [37]
      ,[38]+1 as [38]
      ,[39]+1 as [39]
      ,[40]+1 as [40]
      ,[41]+1 as [41]
      ,[42]+1 as [42]
      ,[43]+1 as [43]
      ,[44]+1 as [44]
      ,[45]+1 as [45]
      ,[46]+1 as [46]
      ,[47]+1 as [47]
      ,[48]+1 as [48]
      ,[49]+1 as [49]
       FROM [MyDB].[dbo].[HkLot49_StoredCount] order by 日期 desc
END";
            var param = new Dictionary<string, object>();
            param.Add("data", data);

            return _dbDapper.NonQuerySQL(sql, param) > 0;
        }
        public bool UpdateCopyNumber6(LotNumber6 data)
        {
            var sql = @"
            DECLARE @TSQL NVARCHAR(4000)
SET @TSQL =	'update [dbo].[HkLot49_StoredCount]
set ' + @num1 + '=0,' + @num2 + '=0,' + @num3 +'=0,' + @num4 + '=0,' + @num5 + '=0,' + @num6 + '=0,' + @num7 + '=0
where 日期=' + @date
EXEC SP_EXECUTESQL @TSQL";
            var param = new Dictionary<string, object>();
            param.Add("num1", "[" + data.號碼1 + "]");
            param.Add("num2", "[" + data.號碼2 + "]");
            param.Add("num3", "[" + data.號碼3 + "]");
            param.Add("num4", "[" + data.號碼4 + "]");
            param.Add("num5", "[" + data.號碼5 + "]");
            param.Add("num6", "[" + data.號碼6 + "]");
            param.Add("num7", "[" + data.特別號 + "]");
            param.Add("date", data.開獎日期);

            return _dbDapper.NonQuerySQL(sql, param) > 0;
        }
        public string GetMaxNo6()
        {
            var sql = @"select max(期數) from [dbo].[HkLot49]";
            return _dbDapper.ExecuteScalarSQL<string>(sql, null);
        }

        public List<LotNumber6> GetLotNumber6NewTop(string newCount)
        {
            var sql = @"
SELECT   Top " + newCount + @"*
FROM              HkLot49
order by 期數 desc";
            return _dbDapper.QueryList<LotNumber6>(sql, null);
        }
        public List<LotNumber6> GetSelectLotNumber6(string selectnum, string StartDate, string EndDate, string StartPeriod, string EndPeriod)
        {
            var sqlTemp = "";
            var sql = @"
select * from [dbo].[HkLot49]
where (號碼1=@num or 號碼2=@num or  號碼3=@num or 號碼4=@num or 號碼5=@num or 號碼6=@num) {0}
";
            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
            {
                sqlTemp = sqlTemp + "and (開獎日期 between @startdate and @enddate) ";
            }
            if (StartPeriod != "00000" && EndPeriod != "00000")
            {
                sqlTemp = sqlTemp + "and 期數 between @startperiod and @endperiod ";
            }
            sql = string.Format(sql, sqlTemp);

            var param = new Dictionary<string, object>();
            param.Add("num", selectnum);
            param.Add("startdate", StartDate);
            param.Add("enddate", EndDate);
            param.Add("startperiod", StartPeriod);
            param.Add("endperiod", EndPeriod);

            return _dbDapper.QueryList<LotNumber6>(sql, param);
        }
        #endregion

    }
}
