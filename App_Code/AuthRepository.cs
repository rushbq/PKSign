using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using AuthData.Models;
using PKLib_Method.Methods;

namespace AuthData.Controllers
{
    public class AuthRepository
    {
        public string ErrMsg;


        #region -----// Read //-----

        /// <summary>
        /// 取得使用者有權限的選單
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public IQueryable<Auth> GetUserMenu(string userGuid)
        {
            //----- 宣告 -----
            StringBuilder sql = new StringBuilder();
            List<Auth> dataList = new List<Auth>();

            //----- 資料查詢 -----
            using (SqlCommand cmd = new SqlCommand())
            {
                //----- SQL 查詢語法 -----
                sql.AppendLine("SELECT Base.Prog_ID AS MenuID, Base.Up_Id AS ParentID, Base.Prog_Name_{0} AS MenuName".FormatThis(fn_Language.Param_Lang));
                sql.AppendLine("  , Base.Lv AS Lv, Base.Prog_Link AS Url, Base.Child_Cnt AS Child");
                sql.AppendLine(" FROM Program Base WITH (NOLOCK)");
                sql.AppendLine(" WHERE (Base.Display = 'Y') AND (UPPER(Base.MenuDisplay) = 'PKSIGN')");
                /* 20200807 - 管理部說開放ALL */
                //sql.AppendLine(" INNER JOIN User_Profile_Rel_Program Auth WITH (NOLOCK) ON Base.Prog_ID = Auth.Prog_ID");
                //sql.AppendLine(" WHERE (Auth.[Guid] = @userID) AND (Base.Display = 'Y') AND (UPPER(Base.MenuDisplay) = 'PKSIGN')");
                sql.AppendLine(" ORDER BY Base.Sort, Base.Prog_ID");

                //----- SQL 執行 -----
                cmd.CommandText = sql.ToString();
                //cmd.Parameters.AddWithValue("userID", userGuid);

                //----- 資料取得 -----
                using (DataTable DT = dbConn.LookupDT(cmd, dbConn.DBS.PKEF, out ErrMsg))
                {
                    //LinQ 查詢
                    var query = DT.AsEnumerable();

                    //資料迴圈
                    foreach (var item in query)
                    {
                        //加入項目
                        var data = new Auth
                        {
                            MenuID = item.Field<int>("MenuID").ToString(),
                            ParentID = item.Field<int>("ParentID").ToString(),
                            MenuName = item.Field<string>("MenuName"),
                            Lv = item.Field<int>("Lv"),
                            Url = item.Field<string>("Url"),
                            child = item.Field<int>("Child"),
                            Target = "_self"
                        };

                        //將項目加入至集合
                        dataList.Add(data);

                    }
                }
            }


            //回傳集合
            return dataList.AsQueryable();
        }

        #endregion


        #region -----// Check //-----

        /// <summary>
        /// 判斷是否有使用權限(Local)
        /// </summary>
        /// <param name="userID">使用者guid</param>
        /// <param name="menuID">選單編號</param>
        /// <returns></returns>
        public bool Check_Auth(string userID, string menuID)
        {
            //----- 宣告 -----
            StringBuilder sql = new StringBuilder();

            //----- 資料查詢 -----
            using (SqlCommand cmd = new SqlCommand())
            {
                //----- SQL 查詢語法 -----
                sql.AppendLine(" SELECT COUNT(*) AS AuthCnt");
                sql.AppendLine(" FROM User_Profile_Rel_Program Auth WITH (NOLOCK)");
                sql.AppendLine(" WHERE (Auth.[Guid] = @userID) AND (Auth.Prog_ID = @menuID)");


                //----- SQL 執行 -----
                cmd.CommandText = sql.ToString();
                cmd.Parameters.AddWithValue("userID", userID);
                cmd.Parameters.AddWithValue("menuID", menuID);

                //----- 資料取得 -----
                using (DataTable DT = dbConn.LookupDT(cmd, dbConn.DBS.PKEF, out ErrMsg))
                {
                    int myCnt = Convert.ToInt32(DT.Rows[0]["AuthCnt"]);

                    return myCnt.Equals(0) ? false : true;
                }

            }
        }

        #endregion


    }
}
