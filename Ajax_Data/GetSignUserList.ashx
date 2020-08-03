<%@ WebHandler Language="C#" Class="GetAuthList" %>

using System;
using System.Web;
using System.Linq;
using SignData.Controllers;
using Newtonsoft.Json;
using System.Data;

public class GetAuthList : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        //[接收參數] 查詢字串
        string ErrMsg = "";
        string _id = context.Request["id"];

        //----- 宣告:資料參數 -----
        SignDataRepository _data = new SignDataRepository();

        //----- 原始資料:取得所有資料 -----
        using (DataTable _myDT = _data.GetSignUser(_id, out ErrMsg))
        {
            if (_myDT == null)
            {
                context.Response.Write(ErrMsg);
                return;
            }
            //----- 資料整理:顯示欄位 -----
            /*
             * 使用jsTree 樹狀選單元件
             * 將資料整理成 jsTree json格式
             */
            var data = _myDT.AsEnumerable()
                .Select(fld =>
                 new
                 {
                     id = fld.Field<string>("MenuID"),
                     parentID = fld.Field<string>("ParentID"),
                     label = fld.Field<string>("MenuName"),
                     open = fld.Field<string>("IsOpen").Equals("Y") ? true : false,
                     selected = fld.Field<string>("IsChecked").Equals("Y") ? true : false,
                     //chkDisabled = fld.Field<string>("ParentID").Equals("0") ? true : false
                     chkDisabled = false
                 });


            //----- 資料整理:序列化 ----- 
            string jdata = JsonConvert.SerializeObject(data, Formatting.Indented);

            /*
             * [回傳格式] - Json
             * data：資料
             */

            //輸出Json
            context.Response.ContentType = "application/json";
            context.Response.Write(jdata);
        }

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}