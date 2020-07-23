using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using AuthData.Controllers;
using AuthData.Models;
using PKLib_Method.Methods;

/*
 此版使用Semantic UI
*/
public partial class Site_S_UI : System.Web.UI.MasterPage
{
    private const string AntiXsrfTokenKey = "__AntiXsrfToken";
    private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    private string _antiXsrfTokenValue;


    protected void Page_Init(object sender, EventArgs e)
    {
        // 下面的程式碼有助於防禦 XSRF 攻擊
        var requestCookie = Request.Cookies[AntiXsrfTokenKey];
        Guid requestCookieGuidValue;
        if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
        {
            // 使用 Cookie 中的 Anti-XSRF 權杖
            _antiXsrfTokenValue = requestCookie.Value;
            Page.ViewStateUserKey = _antiXsrfTokenValue;
        }
        else
        {
            // 產生新的防 XSRF 權杖並儲存到 cookie
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
            Page.ViewStateUserKey = _antiXsrfTokenValue;

            var responseCookie = new HttpCookie(AntiXsrfTokenKey)
            {
                HttpOnly = true,
                Value = _antiXsrfTokenValue
            };
            if (System.Web.Security.FormsAuthentication.RequireSSL && Request.IsSecureConnection)
            {
                responseCookie.Secure = true;
            }
            Response.Cookies.Set(responseCookie);
        }

        Page.PreLoad += master_Page_PreLoad;
    }

    protected void master_Page_PreLoad(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // 設定 Anti-XSRF 權杖
            ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
            ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
        }
        else
        {
            // 驗證 Anti-XSRF 權杖
            if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
            {
                throw new InvalidOperationException("Anti-XSRF 權杖驗證失敗。");
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                //判斷 & 轉換語系
                Check_Lang();


                #region -- 取得選單 --

                //----- 宣告:資料參數 -----
                AuthRepository _data = new AuthRepository();

                //----- 原始資料:取得資料 -----
                var queryAllmenu = _data.GetUserMenu(fn_Param.MemberID);

                lt_headerMenu.Text = showMenu(queryAllmenu);

                _data = null;

                #endregion

            }
            catch
            {
                throw;
            }
        }
    }

    /// <summary>
    /// 顯示選單
    /// </summary>
    /// <param name="myMenu"></param>
    /// <returns></returns>
    private string showMenu(IQueryable<Auth> myMenu)
    {
        System.Text.StringBuilder html = new System.Text.StringBuilder();
        //Url前置
        string Url = "{0}{1}/".FormatThis(fn_Param.WebUrl, Req_Lang);

        //--// 第一層 //--
        var menuLv1 = myMenu
            .Where(m => m.Lv.Equals(1));
        foreach (var itemLv1 in menuLv1)
        {
            if (itemLv1.child > 0)
            {
                html.Append("<div class=\"ui dropdown item rootMenu\">{0} <i class=\"dropdown icon\"></i><div class=\"menu\">".FormatThis(
                    itemLv1.MenuName
                    ));
            }
            else
            {

                html.Append("<a href=\"{0}\" class=\"item\">{1}</a>".FormatThis(
                    Url + itemLv1.Url
                    , itemLv1.MenuName
                    ));
            }


            #region >> 第二層選單 <<

            var menuLv2 = myMenu
                .Where(m => m.Lv.Equals(2) && m.ParentID.Equals(itemLv1.MenuID.ToString()));
            foreach (var itemLv2 in menuLv2)
            {
                html.Append(" <a href=\"{0}\" class=\"item\">{1}</a>".FormatThis(
                    Url + itemLv2.Url
                    , itemLv2.MenuName
                    ));


                html.Append("</li>");
            }

            #endregion


            if (itemLv1.child > 0) html.Append("</div></div>");

        }


        return html.ToString();
    }


    #region -- 語系處理 --

    public string LangName(string lang)
    {
        switch (lang.ToUpper())
        {
            case "CN":
                return "简体中文";

            default:
                return "繁體中文";
        }
    }

    /// <summary>
    /// 判斷 & 轉換語系
    /// </summary>
    private void Check_Lang()
    {
        //取得目前語系cookie
        HttpCookie cLang = Request.Cookies["PKHome_Lang"];
        //將傳來的參數,轉換成完整語系參數
        string langCode = fn_Language.Get_LangCode(Req_Lang);

        //判斷傳入語系是否與目前語系相同, 若不同則執行語系變更
        if (cLang != null)
        {
            if (!cLang.Value.ToUpper().Equals(langCode.ToUpper()))
            {
                //重新註冊cookie
                Response.Cookies.Remove("PKHome_Lang");
                Response.Cookies.Add(new HttpCookie("PKHome_Lang", langCode));
                Response.Cookies["PKHome_Lang"].Expires = DateTime.Now.AddYears(1);

                //語系變換
                System.Globalization.CultureInfo currentInfo = new System.Globalization.CultureInfo(langCode);
                System.Threading.Thread.CurrentThread.CurrentCulture = currentInfo;
                System.Threading.Thread.CurrentThread.CurrentUICulture = currentInfo;

                //redirect
                Response.Redirect(Request.Url.AbsoluteUri);
            }
        }

    }


    #endregion


    #region -- 參數設定 --

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


    /// <summary>
    /// 瀏覽器Title
    /// </summary>
    private string _Param_WebTitle;
    public string Param_WebTitle
    {
        get
        {
            if (string.IsNullOrEmpty(Page.Title))
            {
                return fn_Param.WebName;
            }
            else
            {
                return "{0} | {1}".FormatThis(Page.Title, fn_Param.WebName);
            }
        }
        set
        {
            this._Param_WebTitle = value;
        }
    }

    #endregion
}
