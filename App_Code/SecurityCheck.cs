using System;
using PKLib_Method.Methods;

/// <summary>
///   檢查ID是否過期，重新取得登入資訊
/// </summary>
public class SecurityCheck : System.Web.UI.Page
{
    protected override void OnLoad(System.EventArgs e)
    {
        try
        {
            //[檢查參數] Cookie使用者編號是否存在
            if (string.IsNullOrEmpty(fn_Param.MemberID))
            {
                //清除Session
                Session.Clear();

                //導向登入頁
                Response.Redirect("{0}{1}/Login".FormatThis(
                    fn_Param.WebUrl
                    , Req_Lang));

            }
            else
            {
                base.OnLoad(e);
            }
        }
        catch (Exception)
        {
            throw;
        }

    }


    /// <summary>
    /// 取得傳遞參數 - 語系
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
            this._Req_Lang = value;
        }
    }
    private string _Req_Lang;

}