using System.Collections.Generic;
using System.Linq;
using System.Web;
using PKLib_Data.Assets;
using PKLib_Data.Controllers;

/// <summary>
/// 常用參數
/// </summary>
public class fn_Param
{
    /// <summary>
    /// 網站名稱
    /// </summary>
    public static string WebName
    {
        get
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings["WebName"];
        }
        set
        {
            _WebName = value;
        }
    }
    private static string _WebName;


    /// <summary>
    /// 網站網址
    /// </summary>
    public static string WebUrl
    {
        get
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings["WebUrl"];
        }
        set
        {
            _WebUrl = value;
        }
    }
    private static string _WebUrl;


    /// <summary>
    /// 編碼用的Key(MD5Encrypt)
    /// </summary>
    public static string Deskey
    {
        get
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings["DesKey"];
        }
        set
        {
            _Deskey = value;
        }
    }
    private static string _Deskey;

    
    /// <summary>
    /// CDN網址
    /// </summary>
    public static string CDNUrl
    {
        get
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings["CDNUrl"];
        }
        set
        {
            _CDNUrl = value;
        }
    }
    private static string _CDNUrl;


    /// <summary>
    /// 系統寄件者
    /// </summary>
    public static string SysMail_Sender
    {
        get
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings["SysMail_Sender"];
        }
        set
        {
            _SysMail_Sender = value;
        }
    }
    private static string _SysMail_Sender;



    private static string GetMemberInfo(string cookieParam)
    {
        //讀取cookie
        HttpCookie myInfo = HttpContext.Current.Request.Cookies["PKSign_MemberInfo"];
        if (myInfo == null)
        {
            return "";
        }
        else
        {
            if (myInfo.Values[cookieParam] == null)
            {
                return "";
            }
            else
            {
                return myInfo.Values[cookieParam];
            }
        }
    }

    /// <summary>
    /// 員工Guid
    /// </summary>
    private static string _MemberID;
    public static string MemberID
    {
        get
        {
            return GetMemberInfo("MemberID");
        }
        private set
        {
            _MemberID = value;
        }
    }


    /// <summary>
    /// 員工DisplayName
    /// </summary>
    private static string _MemberName;
    public static string MemberName
    {
        get
        {
            return HttpUtility.UrlDecode(GetMemberInfo("MemberName"));
        }
        private set
        {
            _MemberName = value;
        }
    }


    /// <summary>
    /// 記住我
    /// </summary>
    private static string _RememberMe;
    public static string RememberMe
    {
        get
        {
            return GetMemberInfo("RememberMe");
        }
        private set
        {
            _RememberMe = value;
        }
    }



}