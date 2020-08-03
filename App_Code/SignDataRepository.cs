using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using PKLib_Method.Methods;
using SignData.Models;


namespace SignData.Controllers
{
    public class SignDataRepository
    {
        #region -----// Read //-----

        /// <summary>
        /// 指定資料
        /// </summary>
        /// <param name="search">search集合</param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public IQueryable<BaseData> GetOne(Dictionary<string, string> search
            , out string ErrMsg)
        {
            int dataCnt = 0;
            return GetList(search, 0, 1, out dataCnt, out ErrMsg);
        }

        public IQueryable<BaseData> GetAllList(Dictionary<string, string> search
            , out int DataCnt, out string ErrMsg)
        {
            return GetList(search, 0, 9999999, out DataCnt, out ErrMsg);
        }

        /// <summary>
        /// 資料清單
        /// </summary>
        /// <param name="search">search集合</param>
        /// <param name="startRow">StartRow</param>
        /// <param name="endRow">RecordsPerPage</param>
        /// <param name="DataCnt">傳址參數(資料總筆數)</param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public IQueryable<BaseData> GetList(Dictionary<string, string> search
            , int startRow, int endRow
            , out int DataCnt, out string ErrMsg)
        {
            ErrMsg = "";

            try
            {
                /* 開始/結束筆數計算 */
                int cntStartRow = startRow + 1;
                int cntEndRow = startRow + endRow;

                //----- 宣告 -----
                List<BaseData> dataList = new List<BaseData>(); //資料容器
                List<SqlParameter> sqlParamList = new List<SqlParameter>(); //SQL參數容器
                List<SqlParameter> subParamList = new List<SqlParameter>(); //SQL參數取得
                StringBuilder sql = new StringBuilder(); //SQL語法容器
                StringBuilder subSql = new StringBuilder(); //條件SQL取得
                DataCnt = 0;    //資料總數

                //取得SQL語法
                subSql = _ListSQL(search);
                //取得SQL參數集合
                subParamList = _ListParams(search);


                #region >> 資料筆數SQL查詢 <<
                using (SqlCommand cmdCnt = new SqlCommand())
                {
                    //----- SQL 查詢語法 -----
                    sql.Clear();
                    sql.AppendLine(" SELECT COUNT(TbAll.Data_ID) AS TotalCnt FROM (");

                    //子查詢SQL
                    sql.Append(subSql);

                    sql.AppendLine(" ) AS TbAll");

                    //----- SQL 執行 -----
                    cmdCnt.CommandText = sql.ToString();
                    cmdCnt.Parameters.Clear();
                    sqlParamList.Clear();
                    //cmd.CommandTimeout = 60;   //單位:秒

                    //----- SQL 固定參數 -----
                    //sqlParamList.Add(new SqlParameter("@Lang", lang.ToUpper()));

                    //----- SQL 條件參數 -----
                    //加入篩選後的參數
                    sqlParamList.AddRange(subParamList);

                    //加入參數陣列
                    cmdCnt.Parameters.AddRange(sqlParamList.ToArray());

                    //Execute
                    using (DataTable DTCnt = dbConn.LookupDT(cmdCnt, dbConn.DBS.PKEF, out ErrMsg))
                    {
                        //資料總筆數
                        if (DTCnt.Rows.Count > 0)
                        {
                            DataCnt = Convert.ToInt32(DTCnt.Rows[0]["TotalCnt"]);
                        }
                    }

                    //*** 在SqlParameterCollection同個循環內不可有重複的SqlParam,必須清除才能繼續使用. ***
                    cmdCnt.Parameters.Clear();
                }
                #endregion


                #region >> 主要資料SQL查詢 <<

                //----- 資料取得 -----
                using (SqlCommand cmd = new SqlCommand())
                {
                    //----- SQL 查詢語法 -----
                    sql.Clear();
                    sql.AppendLine(" SELECT TbAll.* FROM (");

                    //子查詢SQL
                    sql.Append(subSql);

                    sql.AppendLine(" ) AS TbAll");

                    sql.AppendLine(" WHERE (TbAll.RowIdx >= @startRow) AND (TbAll.RowIdx <= @endRow)");
                    sql.AppendLine(" ORDER BY TbAll.RowIdx");

                    //----- SQL 執行 -----
                    cmd.CommandText = sql.ToString();
                    cmd.Parameters.Clear();
                    sqlParamList.Clear();
                    //cmd.CommandTimeout = 60;   //單位:秒

                    //----- SQL 固定參數 -----
                    sqlParamList.Add(new SqlParameter("@startRow", cntStartRow));
                    sqlParamList.Add(new SqlParameter("@endRow", cntEndRow));

                    //----- SQL 條件參數 -----
                    //加入篩選後的參數
                    sqlParamList.AddRange(subParamList);

                    //加入參數陣列
                    cmd.Parameters.AddRange(sqlParamList.ToArray());

                    //Execute
                    using (DataTable DT = dbConn.LookupDT(cmd, dbConn.DBS.PKEF, out ErrMsg))
                    {
                        //LinQ 查詢
                        var query = DT.AsEnumerable();

                        //資料迴圈
                        foreach (var item in query)
                        {
                            //加入項目
                            var data = new BaseData
                            {
                                Data_ID = item.Field<Guid>("Data_ID"),
                                TraceID = item.Field<string>("TraceID"),
                                Subject = item.Field<string>("Subject"),
                                Display = item.Field<string>("Display"),
                                Sort = item.Field<Int32>("Sort"),
                                StartTime = item.Field<DateTime>("StartTime").ToString().ToDateString("yyyy/MM/dd HH:mm"),
                                EndTime = item.Field<DateTime>("EndTime").ToString().ToDateString("yyyy/MM/dd HH:mm"),
                                Place = item.Field<int>("Place"),
                                PlaceName = item.Field<string>("PlaceName"),
                                OtherPlace = item.Field<string>("OtherPlace"),
                                JoinCnt = item.Field<Int32>("JoinCnt"),
                                SignCnt = item.Field<Int32>("SignCnt"),
                                Create_Time = item.Field<DateTime?>("Create_Time").ToString().ToDateString("yyyy/MM/dd HH:mm"),
                                Update_Time = item.Field<DateTime?>("Update_Time").ToString().ToDateString("yyyy/MM/dd HH:mm"),
                                Create_Name = item.Field<string>("Create_Name"),
                                Update_Name = item.Field<string>("Update_Name")
                            };

                            //將項目加入至集合
                            dataList.Add(data);

                        }
                    }

                    //回傳集合
                    return dataList.AsQueryable();
                }

                #endregion

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString() + "_Error:_" + ErrMsg);
            }
        }


        /// <summary>
        /// 取得SQL查詢
        /// ** TSQL查詢條件寫在此 **
        /// </summary>
        /// <param name="search">search集合</param>
        /// <returns></returns>
        /// <see cref="GetTempList"/>
        private StringBuilder _ListSQL(Dictionary<string, string> search)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(" SELECT Base.Data_ID, Base.TraceID");
            sql.AppendLine(" , Base.Subject, Base.Display, Base.Sort, Base.StartTime, Base.EndTime");
            sql.AppendLine(" , Base.Place, RefPlace.Class_Name_zh_TW AS PlaceName, Base.OtherPlace");
            sql.AppendLine(" , (SELECT Account_Name + ' (' + Display_Name + ')' FROM PKSYS.dbo.User_Profile WHERE (Guid = Base.Create_Who)) AS Create_Name");
            sql.AppendLine(" , (SELECT Account_Name + ' (' + Display_Name + ')' FROM PKSYS.dbo.User_Profile WHERE (Guid = Base.Update_Who)) AS Update_Name");
            sql.AppendLine(" , Base.Create_Who, Base.Create_Time, Base.Update_Who, Base.Update_Time");
            sql.AppendLine(" , ROW_NUMBER() OVER(ORDER BY Base.Create_Time DESC) AS RowIdx");
            sql.AppendLine(" , (SELECT COUNT(*) FROM SignBook_NameList WHERE (Parent_ID = Base.Data_ID)) AS JoinCnt");
            sql.AppendLine(" , (SELECT COUNT(*) FROM SignBook_CheckIn WHERE (Parent_ID = Base.Data_ID)) AS SignCnt");
            sql.AppendLine(" FROM SignBook Base");
            sql.AppendLine("  INNER JOIN SignBook_RefCLass RefPlace ON Base.Place = RefPlace.Class_ID");
            sql.AppendLine(" WHERE (1=1)");

            /* Search */
            #region >> filter <<

            if (search != null)
            {
                //過濾空值
                var thisSearch = search.Where(fld => !string.IsNullOrWhiteSpace(fld.Value));

                //查詢內容
                foreach (var item in thisSearch)
                {
                    switch (item.Key)
                    {
                        case "DataID":
                            sql.Append(" AND (Base.Data_ID = @Data_ID)");

                            break;

                        case "Keyword":
                            //關鍵字
                            sql.Append(" AND (");
                            sql.Append(" (UPPER(Base.TraceID) LIKE '%' + UPPER(@keyword) + '%')");
                            sql.Append(" OR (UPPER(Base.Subject) LIKE '%' + UPPER(@keyword) + '%')");
                            sql.Append(" OR (UPPER(Base.OtherPlace) LIKE '%' + UPPER(@keyword) + '%')");
                            sql.Append(")");

                            break;

                        case "Place":
                            sql.Append(" AND (Base.Place = @Place)");

                            break;

                        case "dateSection":
                            sql.Append(" AND(");
                            sql.Append(" (Base.StartTime BETWEEN @sDate AND @eDate)");
                            sql.Append("  OR(Base.EndTime BETWEEN @sDate AND @eDate)");
                            sql.Append(" )");
                            break;

                            //case "sDate":
                            //    sql.Append(" AND (Base.Create_Time >= @sDate)");
                            //    break;
                            //case "eDate":
                            //    sql.Append(" AND (Base.Create_Time <= @eDate)");
                            //    break;

                    }
                }
            }
            #endregion

            return sql;
        }


        /// <summary>
        /// 取得條件參數
        /// ** SQL參數設定寫在此 **
        /// </summary>
        /// <param name="search">search集合</param>
        /// <returns></returns>
        /// <see cref="GetTempList"/>
        private List<SqlParameter> _ListParams(Dictionary<string, string> search)
        {
            //declare
            List<SqlParameter> sqlParamList = new List<SqlParameter>();

            //get values
            if (search != null)
            {
                //過濾空值
                var thisSearch = search.Where(fld => !string.IsNullOrWhiteSpace(fld.Value));

                //查詢內容
                foreach (var item in thisSearch)
                {
                    switch (item.Key)
                    {
                        case "DataID":
                            sqlParamList.Add(new SqlParameter("@Data_ID", item.Value));

                            break;

                        case "Keyword":
                            sqlParamList.Add(new SqlParameter("@keyword", item.Value));

                            break;

                        case "Place":
                            sqlParamList.Add(new SqlParameter("@Place", item.Value));

                            break;

                        case "sDate":
                            sqlParamList.Add(new SqlParameter("@sDate", item.Value + " 00:00:00"));
                            break;
                        case "eDate":
                            sqlParamList.Add(new SqlParameter("@eDate", item.Value + " 23:59:59"));
                            break;

                    }
                }
            }


            return sqlParamList;
        }


        /// <summary>
        /// 與會人員名單
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public DataTable GetSignUser(string parentID, out string ErrMsg)
        {
            //----- 宣告 -----
            string sql = "";

            //----- 資料取得 -----
            using (SqlCommand cmd = new SqlCommand())
            {
                //----- SQL 查詢語法 -----

                sql = @"
                SELECT Tbl.* FROM
                (
                    SELECT CAST(SID AS NVARCHAR) AS MenuID, '0' AS ParentID, '【' + SName + '】' AS MenuName
                     , CAST(Sort AS NVARCHAR) AS Sort, 'Y' AS IsOpen, 'N' AS IsChecked
                    FROM [PKSYS].dbo.Shipping WITH (NOLOCK)
                    WHERE (Display = 'Y')

                    UNION ALL

                    SELECT CAST(Dept.DeptID AS NVARCHAR) AS MenuID, Dept.Area AS ParentID, Dept.DeptName AS MenuName
                     , CAST(100 + Dept.Sort AS NVARCHAR) AS Sort, 'N' AS IsOpen
	                 , (CASE WHEN (
	                    SELECT COUNT(*) FROM [PKSYS].dbo.User_Profile Prof
	                    INNER JOIN [PKEF].dbo.SignBook_NameList NameList ON Prof.Guid = NameList.Who AND NameList.Parent_ID = @parentID
		                WHERE Prof.DeptID = Dept.DeptID
	                   ) > 0 THEN 'Y' ELSE 'N' END) AS IsChecked
                    FROM [PKSYS].dbo.User_Dept Dept WITH (NOLOCK)
                    WHERE (Dept.Display = 'Y')

                    UNION ALL

                    SELECT 'v_' + Prof.Guid AS MenuID, Prof.DeptID AS ParentID, Prof.Display_Name AS MenuName
                     , Prof.Account_Name AS Sort, 'N' AS IsOpen
                     , (CASE WHEN NameList.Who IS NULL THEN 'N' ELSE 'Y' END) AS IsChecked
                    FROM [PKSYS].dbo.User_Profile Prof WITH (NOLOCK)
                        INNER JOIN [PKSYS].dbo.User_Dept Dept WITH (NOLOCK) ON Prof.DeptID = Dept.DeptID
                        LEFT JOIN [PKEF].dbo.SignBook_NameList NameList ON Prof.Guid = NameList.Who AND NameList.Parent_ID = @parentID
                    WHERE (Dept.Display = 'Y') AND (Prof.Display = 'Y')
                ) AS Tbl
                ORDER BY Tbl.Sort";

                //----- SQL 執行 -----
                cmd.CommandText = sql.ToString();
                cmd.Parameters.AddWithValue("parentID", parentID);

                //回傳
                return dbConn.LookupDT(cmd, out ErrMsg);
            }

        }


        /// <summary>
        /// 與會人員-簽到名單
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public DataTable GetCheckInList(string parentID, out string ErrMsg)
        {
            //----- 宣告 -----
            string sql = "";

            //----- 資料取得 -----
            using (SqlCommand cmd = new SqlCommand())
            {
                //----- SQL 查詢語法 -----

                sql = @"
                    SELECT Prof.Display_Name, Prof.NickName, Dept.Area
                    , ChkWho.SignWho, ChkWho.SignTime, ChkWho.FromIP
                    , Lst.Who AS NameID
                    , ISNULL(ChkWho.IsAgent, 'N') AS IsAgent
                    , (CASE WHEN ChkWho.SignWho IS NULL THEN 'N' ELSE 'Y' END) AS IsSign
                    , (SELECT Display_Name FROM [PKSYS].dbo.User_Profile WHERE (Guid = ChkWho.Create_Who)) AS AgentName
                    FROM SignBook_NameList Lst
                     INNER JOIN [PKSYS].dbo.User_Profile Prof WITH (NOLOCK) ON Lst.Who = Prof.Guid
                     INNER JOIN [PKSYS].dbo.User_Dept Dept WITH (NOLOCK) ON Prof.DeptID = Dept.DeptID
                     LEFT JOIN SignBook_CheckIn ChkWho ON Lst.Who = ChkWho.SignWho AND Lst.Parent_ID = ChkWho.Parent_ID
                    WHERE (Lst.Parent_ID = @parentID)";

                //----- SQL 執行 -----
                cmd.CommandText = sql.ToString();
                cmd.Parameters.AddWithValue("parentID", parentID);

                //回傳
                return dbConn.LookupDT(cmd, dbConn.DBS.PKEF, out ErrMsg);
            }

        }


        /// <summary>
        /// 非與會名單內-簽到名單
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public DataTable GetCheckInList_NoName(string parentID, out string ErrMsg)
        {
            //----- 宣告 -----
            string sql = "";

            //----- 資料取得 -----
            using (SqlCommand cmd = new SqlCommand())
            {
                //----- SQL 查詢語法 -----

                sql = @"
                    SELECT Prof.Display_Name, Prof.NickName, Dept.Area
                    , ChkWho.SignWho, ChkWho.SignTime, ChkWho.FromIP
                    , ISNULL(ChkWho.IsAgent, 'N') AS IsAgent
                    , (CASE WHEN ChkWho.SignWho IS NULL THEN 'N' ELSE 'Y' END) AS IsSign
                    FROM SignBook_CheckIn ChkWho
                     INNER JOIN [PKSYS].dbo.User_Profile Prof WITH (NOLOCK) ON ChkWho.SignWho = Prof.Guid
                     INNER JOIN [PKSYS].dbo.User_Dept Dept WITH (NOLOCK) ON Prof.DeptID = Dept.DeptID
                    WHERE (ChkWho.Parent_ID = @parentID)
                     AND (ChkWho.SignWho NOT IN(
                      SELECT Who FROM SignBook_NameList WHERE Parent_ID = @parentID
                     ))";

                //----- SQL 執行 -----
                cmd.CommandText = sql.ToString();
                cmd.Parameters.AddWithValue("parentID", parentID);

                //回傳
                return dbConn.LookupDT(cmd, dbConn.DBS.PKEF, out ErrMsg);
            }

        }


        /// <summary>
        /// 前台會議清單
        /// </summary>
        /// <param name="search"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public DataTable GetMeetingList(Dictionary<string, string> search, out string ErrMsg)
        {
            //----- 宣告 -----
            string sql = "";

            //----- 資料取得 -----
            using (SqlCommand cmd = new SqlCommand())
            {
                //----- SQL 查詢語法 -----

                sql = @"
                    SELECT Base.Data_ID, Base.TraceID
                     , Base.Subject, RefPlace.Class_Name_zh_TW AS PlaceName, Base.OtherPlace
                     , Base.StartTime, Base.EndTime
                     , (SELECT SignTime FROM SignBook_CheckIn WHERE (Parent_ID = Base.Data_ID) AND (SignWho = @Who)) AS SignTime
                    FROM SignBook Base
                     INNER JOIN SignBook_RefCLass RefPlace ON Base.Place = RefPlace.Class_ID
                    WHERE (Base.StartTime <= GETDATE()) AND (Base.EndTime >= GETDATE()) AND (Base.Display = 'Y')
                    ";

                //get values
                if (search != null)
                {
                    //過濾空值
                    var thisSearch = search.Where(fld => !string.IsNullOrWhiteSpace(fld.Value));

                    //查詢內容
                    foreach (var item in thisSearch)
                    {
                        switch (item.Key)
                        {
                            case "DataID":
                                sql += " AND (Base.Data_ID = @dataID)";
                                cmd.Parameters.AddWithValue("dataID", item.Value);

                                break;

                            case "Who":
                                cmd.Parameters.AddWithValue("Who", item.Value);

                                break;

                        }
                    }
                }

                //order by
                sql += "ORDER BY Base.Sort, Base.StartTime";

                //----- SQL 執行 -----
                cmd.CommandText = sql.ToString();

                //回傳
                return dbConn.LookupDT(cmd, dbConn.DBS.PKEF, out ErrMsg);
            }

        }

        #endregion


        #region -----// Create //-----
        /// <summary>
        /// 建立基本資料
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public bool Create(BaseData instance, out string ErrMsg)
        {
            //----- 宣告 -----
            StringBuilder sql = new StringBuilder();

            //----- 資料查詢 -----
            using (SqlCommand cmd = new SqlCommand())
            {
                //----- SQL 查詢語法 -----
                sql.AppendLine(" INSERT INTO SignBook (");
                sql.AppendLine("  Data_ID, TraceID, Subject, Place, OtherPlace");
                sql.AppendLine("  , StartTime, EndTime, Display, Sort");
                sql.AppendLine("  , Create_Who, Create_Time");
                sql.AppendLine(" ) VALUES (");
                sql.AppendLine("  @Data_ID, @TraceID, @Subject, @Place, @OtherPlace");
                sql.AppendLine("  , @StartTime, @EndTime, @Display, @Sort");
                sql.AppendLine("  , @Create_Who, GETDATE()");
                sql.AppendLine(" )");

                //----- SQL 執行 -----
                cmd.CommandText = sql.ToString();
                cmd.Parameters.AddWithValue("Data_ID", instance.Data_ID);
                cmd.Parameters.AddWithValue("TraceID", instance.TraceID);
                cmd.Parameters.AddWithValue("Subject", instance.Subject);
                cmd.Parameters.AddWithValue("Place", instance.Place);
                cmd.Parameters.AddWithValue("OtherPlace", instance.OtherPlace);
                cmd.Parameters.AddWithValue("StartTime", instance.StartTime.ToString().ToDateString("yyyy/MM/dd HH:mm"));
                cmd.Parameters.AddWithValue("EndTime", instance.EndTime.ToString().ToDateString("yyyy/MM/dd HH:mm"));
                cmd.Parameters.AddWithValue("Display", instance.Display);
                cmd.Parameters.AddWithValue("Sort", instance.Sort);
                cmd.Parameters.AddWithValue("Create_Who", fn_Param.MemberID);

                //return
                return dbConn.ExecuteSql(cmd, dbConn.DBS.PKEF, out ErrMsg);
            }

        }


        /// <summary>
        /// 建立與會名單
        /// </summary>
        /// <param name="parentID">單頭ID</param>
        /// <param name="query">單身資料</param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public bool CreateDetail(string parentID, List<NameList> query, out string ErrMsg)
        {
            //----- 宣告 -----
            StringBuilder sql = new StringBuilder();

            //----- 資料查詢 -----
            using (SqlCommand cmd = new SqlCommand())
            {
                //----- SQL 查詢語法 -----
                sql.AppendLine(" DELETE FROM SignBook_NameList WHERE (Parent_ID = @DataID);");
                sql.AppendLine(" UPDATE SignBook SET Update_Who = @Update_Who, Update_Time = GETDATE() WHERE (Data_ID = @DataID);");
                sql.AppendLine(" DECLARE @NewID AS INT ");

                foreach (var item in query)
                {
                    if (!string.IsNullOrWhiteSpace(item.Who))
                    {
                        sql.AppendLine(" SET @NewID = (");
                        sql.AppendLine("  SELECT ISNULL(MAX(Data_ID), 0) + 1 AS NewID");
                        sql.AppendLine("  FROM SignBook_NameList");
                        sql.AppendLine("  WHERE (Parent_ID = @DataID)");
                        sql.AppendLine(" )");

                        sql.AppendLine(" INSERT INTO SignBook_NameList( ");
                        sql.AppendLine("  Parent_ID, Data_ID, Who");
                        sql.AppendLine(" ) VALUES (");
                        sql.AppendLine("  @DataID, @NewID, '{0}'".FormatThis(item.Who));
                        sql.AppendLine(" );");
                    }
                }

                //----- SQL 執行 -----
                cmd.CommandText = sql.ToString();
                cmd.Parameters.AddWithValue("DataID", parentID);
                cmd.Parameters.AddWithValue("Update_Who", fn_Param.MemberID);

                return dbConn.ExecuteSql(cmd, dbConn.DBS.PKEF, out ErrMsg);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentID">單頭ID</param>
        /// <param name="query">單身資料</param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public bool CreateSign(string parentID, CheckIn query, out string ErrMsg)
        {
            //----- 宣告 -----
            StringBuilder sql = new StringBuilder();

            //----- 資料查詢 -----
            using (SqlCommand cmd = new SqlCommand())
            {
                //----- SQL 查詢語法 -----
                sql.AppendLine(" DECLARE @NewID AS INT ");

                sql.AppendLine(" SET @NewID = (");
                sql.AppendLine("  SELECT ISNULL(MAX(Data_ID), 0) + 1 AS NewID");
                sql.AppendLine("  FROM SignBook_CheckIn");
                sql.AppendLine("  WHERE (Parent_ID = @DataID)");
                sql.AppendLine(" )");

                sql.AppendLine(" INSERT INTO SignBook_CheckIn( ");
                sql.AppendLine("  Parent_ID, Data_ID, SignWho, SignTime");
                sql.AppendLine("  , FromIP, IsAgent, Create_Who, Create_Time");
                sql.AppendLine(" ) VALUES (");
                sql.AppendLine("  @DataID, @NewID, @SignWho, @SignTime");
                sql.AppendLine("  , @FromIP, @IsAgent, @Create_Who, GETDATE()");
                sql.AppendLine(" );");

                //----- SQL 執行 -----
                cmd.CommandText = sql.ToString();
                cmd.Parameters.AddWithValue("DataID", parentID);
                cmd.Parameters.AddWithValue("SignWho", query.SignWho);
                cmd.Parameters.AddWithValue("SignTime", query.SignTime);
                cmd.Parameters.AddWithValue("FromIP", query.FromIP);
                cmd.Parameters.AddWithValue("IsAgent", query.IsAgent);
                cmd.Parameters.AddWithValue("Create_Who", fn_Param.MemberID);

                return dbConn.ExecuteSql(cmd, dbConn.DBS.PKEF, out ErrMsg);
            }

        }


        /// <summary>
        /// 取得類別
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="lang">語系(tw/en/cn)</param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public IQueryable<ClassItem> GetClassItem(string type, string lang, out string ErrMsg)
        {
            //----- 宣告 -----
            List<ClassItem> dataList = new List<ClassItem>();
            StringBuilder sql = new StringBuilder();

            //----- 資料取得 -----
            using (SqlCommand cmd = new SqlCommand())
            {
                //----- SQL 查詢語法 -----
                sql.AppendLine(" SELECT Class_ID AS ID, Class_Name_{0} AS Label"
                    .FormatThis(fn_Language.Get_LangCode(lang).Replace("-", "_")));
                sql.AppendLine(" FROM SignBook_RefClass WITH(NOLOCK)");
                sql.AppendLine(" WHERE (Class_Type = @type) AND (Display = 'Y')");
                sql.AppendLine(" ORDER BY Sort");

                //----- SQL 執行 -----
                cmd.CommandText = sql.ToString();
                cmd.Parameters.AddWithValue("type", Convert.ToInt32(type));

                using (DataTable DT = dbConn.LookupDT(cmd, dbConn.DBS.PKEF, out ErrMsg))
                {
                    //LinQ 查詢
                    var query = DT.AsEnumerable();

                    //資料迴圈
                    foreach (var item in query)
                    {
                        //加入項目
                        var data = new ClassItem
                        {
                            ID = item.Field<int>("ID"),
                            Label = item.Field<string>("Label")
                        };

                        //將項目加入至集合
                        dataList.Add(data);

                    }
                }

                //回傳集合
                return dataList.AsQueryable();

            }
        }

        #endregion



        #region -----// Update //-----

        public bool Update(string dataID, BaseData instance, out string ErrMsg)
        {
            //----- 宣告 -----
            StringBuilder sql = new StringBuilder();

            //----- 資料查詢 -----
            using (SqlCommand cmd = new SqlCommand())
            {
                //----- SQL 查詢語法 -----
                sql.AppendLine(" UPDATE SignBook SET ");
                sql.AppendLine("  Subject = @Subject, Place = @Place, OtherPlace = @OtherPlace");
                sql.AppendLine("  , StartTime = @StartTime, EndTime = @EndTime, Display = @Display, Sort = @Sort");
                sql.AppendLine("  , Update_Who = @Update_Who, Update_Time = GETDATE()");
                sql.AppendLine(" WHERE (Data_ID = @DataID)");
                //----- SQL 執行 -----
                cmd.CommandText = sql.ToString();
                cmd.Parameters.AddWithValue("DataID", dataID);
                cmd.Parameters.AddWithValue("Subject", instance.Subject);
                cmd.Parameters.AddWithValue("Place", instance.Place);
                cmd.Parameters.AddWithValue("OtherPlace", instance.OtherPlace);
                cmd.Parameters.AddWithValue("StartTime", instance.StartTime.ToString().ToDateString("yyyy/MM/dd HH:mm"));
                cmd.Parameters.AddWithValue("EndTime", instance.EndTime.ToString().ToDateString("yyyy/MM/dd HH:mm"));
                cmd.Parameters.AddWithValue("Display", instance.Display);
                cmd.Parameters.AddWithValue("Sort", instance.Sort);
                cmd.Parameters.AddWithValue("Update_Who", fn_Param.MemberID);

                //execute
                return dbConn.ExecuteSql(cmd, dbConn.DBS.PKEF, out ErrMsg);
            }
        }

        #endregion



        #region -----// Delete //-----

        /// <summary>
        /// 刪除所有資料
        /// </summary>
        /// <param name="dataID">資料編號</param>
        /// <returns></returns>
        public bool Delete(string dataID, out string ErrMsg)
        {
            //----- 宣告 -----
            StringBuilder sql = new StringBuilder();

            //----- 資料查詢 -----
            using (SqlCommand cmd = new SqlCommand())
            {
                //----- SQL 查詢語法 -----
                sql.AppendLine(" DELETE FROM SignBook_NameList WHERE (Parent_ID = @DataID);");
                sql.AppendLine(" DELETE FROM SignBook_CheckIn WHERE (Parent_ID = @DataID);");
                sql.AppendLine(" DELETE FROM SignBook WHERE (Data_ID = @DataID);");

                //----- SQL 執行 -----
                cmd.CommandText = sql.ToString();
                cmd.Parameters.AddWithValue("DataID", dataID);

                return dbConn.ExecuteSql(cmd, dbConn.DBS.PKEF, out ErrMsg);
            }
        }

        #endregion

    }
}
