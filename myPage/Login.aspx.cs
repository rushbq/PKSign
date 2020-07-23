using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PKLib_Method.Methods;


public partial class myPage_Login : System.Web.UI.Page
{
    public string ErrMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //已登入
                if (!string.IsNullOrEmpty(fn_Param.MemberID))
                {
                    //導向指定網址
                    Response.Redirect(fn_Param.WebUrl);
                }

            }

        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void btn_Login_Click(object sender, EventArgs e)
    {
        try
        {
            /*
             * 使用外部連AD帳戶驗證, 請確認AD相關設定是否齊全
             */

            //取得輸入參數
            string GetEmail = tb_Account.Text.Left(5);
            string GetPwd = tb_Password.Text.Left(30);
            string errTxt = "";

            if (string.IsNullOrWhiteSpace(GetEmail))
            {
                errTxt += "帳號空白\\n";
            }
            if (string.IsNullOrWhiteSpace(GetPwd))
            {
                errTxt += "密碼空白\\n";
            }

            //alert
            if (!string.IsNullOrEmpty(errTxt))
            {
                CustomExtension.AlertMsg(errTxt, "");
                return;
            }

            //[判斷帳號是否存在]
            if (Check_AD(GetEmail, GetPwd, out ErrMsg))
            {
                //導向指定網址
                Response.Redirect(fn_Param.WebUrl);
            }
            else
            {
                //登入失敗
                CustomExtension.AlertMsg("登入失敗,請確認帳號或密碼是否正確.", "");
                return;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }


    /// <summary>
    /// AD驗證
    /// </summary>
    /// <param name="UserID">帳號</param>
    /// <param name="UserPwd">密碼</param>
    /// <returns></returns>
    private bool Check_AD(string UserID, string UserPwd, out string ErrMsg)
    {
        try
        {
            //呼叫ADService
            ADService ADSrv = new ADService();
            //輸入帳號
            ADSrv.AD_UserName = UserID;
            //輸入密碼
            ADSrv.AD_UserPwd = UserPwd;
            //取得回傳字串集
            StringCollection collectAttr = ADSrv.getADAttributes(out ErrMsg);
            if (collectAttr != null)
            {
                //先清除Cookie
                HttpCookie myCookie = new HttpCookie("PKSign_MemberInfo");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);

                #region -- Cookie處理 --

                //產生Cookie
                HttpCookie cMemberInfo = new HttpCookie("PKSign_MemberInfo");

                //設定多值
                cMemberInfo.Values.Add("MemberID", collectAttr[3]);    //AD Guid
                //cMemberInfo.Values.Add("MemberAcct", collectAttr[2]);   //登入工號
                cMemberInfo.Values.Add("MemberName", Server.UrlEncode(collectAttr[1]));   //名稱


                //判斷是否要記住帳號
                if (this.cb_Remember.Checked)
                {
                    cMemberInfo.Values.Add("RememberMe", "Y");  //記住我
                    //設定到期日(12個月)
                    cMemberInfo.Expires = DateTime.Now.AddMonths(12);
                }
                else
                {
                    cMemberInfo.Values.Add("RememberMe", "N");  //記住我(N)
                    //不設定到期日, 基本上瀏覽器關閉就會消失
                }

                //寫到用戶端
                Response.Cookies.Add(cMemberInfo);

                #endregion

                return true;
            }
            else
            {
                //AD驗證不通過
                return false;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }


}