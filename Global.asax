<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>


<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // 在應用程式啟動時執行的程式碼
        // 載入Routing設定
        RegisterRoutes(RouteTable.Routes);

    }

    void Application_End(object sender, EventArgs e)
    {
        //  在應用程式關閉時執行的程式碼

    }

    void Application_Error(object sender, EventArgs e)
    {
        // 在發生未處理的錯誤時執行的程式碼

    }

    protected void Application_BeginRequest(Object sender, EventArgs e)
    {
        #region -- 語系判斷 --

        System.Globalization.CultureInfo currentInfo;

        //[判斷參數] - 判斷Cookie是否存在
        HttpCookie cLang = Request.Cookies["PKHome_Lang"];
        if ((cLang != null))
        {
            //依Cookie選擇，變換語言別
            switch (cLang.Value.ToString().ToUpper())
            {
                //case "EN-US":
                //    System.Globalization.CultureInfo currentInfo = new System.Globalization.CultureInfo("en-US");
                //    System.Threading.Thread.CurrentThread.CurrentCulture = currentInfo;
                //    System.Threading.Thread.CurrentThread.CurrentUICulture = currentInfo;
                //    break;

                case "ZH-CN":
                    currentInfo = new System.Globalization.CultureInfo("zh-CN");
                    System.Threading.Thread.CurrentThread.CurrentCulture = currentInfo;
                    System.Threading.Thread.CurrentThread.CurrentUICulture = currentInfo;
                    break;

                default:
                    currentInfo = new System.Globalization.CultureInfo("zh-TW");
                    System.Threading.Thread.CurrentThread.CurrentCulture = currentInfo;
                    System.Threading.Thread.CurrentThread.CurrentUICulture = currentInfo;
                    break;
            }

        }
        else
        {
            //Cookie不存在, 新增預設語系(依瀏覽器預設)
            string defCName = System.Globalization.CultureInfo.CurrentCulture.Name;

            //判斷瀏覽器預設的語系, 除了繁中簡中，其他國家語系都帶英文
            switch (defCName.ToUpper())
            {
                case "ZH-TW":
                case "ZH-CN":
                    break;

                default:
                    defCName = "en-US";
                    break;
            }

            Response.Cookies.Add(new HttpCookie("PKHome_Lang", defCName));
            Response.Cookies["PKHome_Lang"].Expires = DateTime.Now.AddYears(1);
            currentInfo = new System.Globalization.CultureInfo(defCName);
            System.Threading.Thread.CurrentThread.CurrentCulture = currentInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = currentInfo;

        }

        #endregion

    }

    /// <summary>
    /// Routing設定
    /// </summary>
    /// <param name="routes">URL路徑</param>
    public static void RegisterRoutes(RouteCollection routes)
    {
        #region -- 定義不處理UrlRouting的規則 --
        routes.Ignore("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
        routes.Ignore("{*allcss}", new { allcss = @".*\.css(/.*)?" });
        routes.Ignore("{*alljpg}", new { alljpg = @".*\.jpg(/.*)?" });
        routes.Ignore("{*alljs}", new { alljs = @".*\.js(/.*)?" });
        routes.Add(new Route("{resource}.css/{*pathInfo}", new StopRoutingHandler()));
        routes.Add(new Route("{resource}.js/{*pathInfo}", new StopRoutingHandler()));
        #endregion

        //[首頁]
        routes.MapPageRoute("HomeRoute", "{lang}", "~/Default.aspx", false,
             new RouteValueDictionary {
                    { "lang", "auto" }});


        //--- 行政管理(2000) --- 
        //製物工單
        //routes.MapPageRoute("MKHelpSearch", "{lang}/{rootID}/MarketingHelp/{CompID}", "~/myMarketingHelp/Search.aspx", false,
        //    new RouteValueDictionary {
        //        { "lang", "auto" }
        //        , { "rootID", "2000" }
        //        , { "CompID", "TW" }});
        //routes.MapPageRoute("MKHelpEdit", "{lang}/{rootID}/MarketingHelp/{CompID}/Edit/{id}", "~/myMarketingHelp/Edit.aspx", false,
        // new RouteValueDictionary {
        //        { "lang", "auto" }
        //        , { "rootID", "2000" }
        //        , { "CompID", "TW" }
        //        , { "id", "new" }});
        //routes.MapPageRoute("MKHelpView", "{lang}/{rootID}/MarketingHelp/{CompID}/View/{id}", "~/myMarketingHelp/View.aspx", false,
        // new RouteValueDictionary {
        //        { "lang", "auto" }
        //        , { "rootID", "2000" }
        //        , { "CompID", "TW" }
        //        , { "id", "new" }});

        //簽到管理
        routes.MapPageRoute("myMSignSearch", "{lang}/SignManage", "~/myManage/Search.aspx", false,
            new RouteValueDictionary {
                { "lang", "auto" }});
        routes.MapPageRoute("myMSignEdit", "{lang}/SignManage/Edit/{id}", "~/myManage/Edit.aspx", false,
         new RouteValueDictionary {
                { "lang", "auto" }
                , { "id", "new" }});

        //簽到頁
        routes.MapPageRoute("myConfirm", "{lang}/Sign/{id}", "~/mySign/Confirm.aspx", false);

        //Login
        routes.MapPageRoute("myLogin", "{lang}/Login", "~/myPage/Login.aspx", false);
        //Error
        routes.MapPageRoute("ErrPage", "Error/{msg}", "~/myPage/ErrorPage.aspx", false);

    }


</script>
