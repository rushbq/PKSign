using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PKLib_Method.Methods;
using SignData.Controllers;
using SignData.Models;

public partial class mySign_Confirm : SecurityCheck
{
    public string ErrMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        /*
          只檢查登入,不檢查MENU權限
        */

        //資料顯示
        LookupDataList();
    }


    #region -- 資料顯示 --

    /// <summary>
    /// 取得資料
    /// </summary>
    private void LookupDataList()
    {
        //----- 宣告:資料參數 -----
        SignDataRepository _data = new SignDataRepository();
        Dictionary<string, string> search = new Dictionary<string, string>();

        try
        {
            #region >> 條件篩選 <<

            search.Add("Who", fn_Param.MemberID);
            search.Add("DataID", Req_DataID);

            #endregion

            //----- 原始資料:取得所有資料 -----
            var query = _data.GetMeetingList(search, out ErrMsg);

            //----- 資料整理:繫結 ----- 
            lvList.DataSource = query;
            lvList.DataBind();

            //No data
            if (query.Rows.Count == 0)
            {
                Response.Redirect(FuncPath());
                return;
            }
        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            _data = null;
        }

    }


    protected void lvList_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            //取得Key值
            string _SignWho = fn_Param.MemberID;
            string _GetIP = CustomExtension.GetIP();

            //----- 宣告:資料參數 -----
            SignDataRepository _data = new SignDataRepository();

            try
            {
                //instance
                var data = new CheckIn
                {
                    SignWho = _SignWho,
                    SignTime = DateTime.Now.ToString().ToDateString("yyyy/MM/dd HH:mm:ss"),
                    FromIP = _GetIP,
                    IsAgent = "N"
                };

                //do Check
                if (_data.CheckSign(Req_DataID, data, out ErrMsg))
                {
                    CustomExtension.AlertMsg("重複簽到", FuncPath());
                    return;
                }

                //do Create
                if (false == _data.CreateSign(Req_DataID, data, out ErrMsg))
                {
                    CustomExtension.AlertMsg("簽到失敗", thisPage);
                    return;
                }

                //導向首頁
                Response.Redirect(FuncPath());
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                //Release
                _data = null;
            }

        }
    }


    protected void lvList_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem dataItem = (ListViewDataItem)e.Item;

                //取得資料:簽到時間
                string _signTime = DataBinder.Eval(dataItem.DataItem, "SignTime").ToString();

                PlaceHolder ph_signed = (PlaceHolder)e.Item.FindControl("ph_signed");
                ph_signed.Visible = !string.IsNullOrEmpty(_signTime);
                PlaceHolder ph_signButton = (PlaceHolder)e.Item.FindControl("ph_signButton");
                ph_signButton.Visible = string.IsNullOrEmpty(_signTime);

            }
        }
        catch (Exception)
        {
            throw;
        }
    }


    #endregion


    #region -- 網址參數 --
    /// <summary>
    /// 取得網址參數 - 語系
    /// </summary>
    public string Req_Lang
    {
        get
        {
            string myLang = Page.RouteData.Values["lang"] == null ? "auto" : Page.RouteData.Values["lang"].ToString();

            //若為auto, 就去抓cookie
            return myLang.Equals("auto") ? fn_Language.Get_Lang(Request.Cookies["PKHome_Lang"].Value) : myLang;
        }
        set
        {
            _Req_Lang = value;
        }
    }
    private string _Req_Lang;


    /// <summary>
    /// 取得此功能的前置路徑
    /// </summary>
    /// <returns></returns>
    public string FuncPath()
    {
        return "{0}{1}".FormatThis(
            fn_Param.WebUrl
            , Req_Lang);
    }


    /// <summary>
    /// 本頁網址
    /// </summary>
    private string _thisPage;
    public string thisPage
    {
        get
        {
            return "{0}Sign/{1}".FormatThis(FuncPath(), Req_DataID);
        }
        set
        {
            this._thisPage = value;
        }
    }

    #endregion


    #region -- 傳遞參數 --
    /// <summary>
    /// 取得傳遞參數 - 資料編號
    /// </summary>
    private string _Req_DataID;
    public string Req_DataID
    {
        get
        {
            String DataID = Page.RouteData.Values["id"].ToString();

            return DataID;
        }
        set
        {
            this._Req_DataID = value;
        }
    }


    #endregion


}