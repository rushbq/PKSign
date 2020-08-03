using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using PKLib_Method.Methods;
using SignData.Controllers;
using SignData.Models;

public partial class myManage_Edit : SecurityCheck
{
    public string ErrMsg;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                #region --權限--
                //[權限判斷] Start

                bool isPass = false;
                isPass = fn_CheckAuth.Check(fn_Param.MemberID, "11011");

                if (!isPass)
                {
                    Response.Redirect("{0}Error/您無使用權限".FormatThis(fn_Param.WebUrl));
                    return;
                }

                //[權限判斷] End
                #endregion

                //[產生選單]
                Get_ClassList(ddl_Place, "1", "請選擇");


                //[參數判斷] - 資料編號
                if (Req_DataID.Equals("new"))
                {

                    //填入預設值
                    tb_StartTime.Text = DateTime.Today.ToString().ToDateString("yyyy/MM/dd 09:00");
                    tb_EndTime.Text = DateTime.Today.ToString().ToDateString("yyyy/MM/dd 18:00");
                }
                else
                {
                    //資料顯示
                    LookupData();

                }

            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    #region -- 資料顯示:基本資料 --

    /// <summary>
    /// 取得資料
    /// </summary>
    private void LookupData()
    {
        //----- 宣告:資料參數 -----
        SignDataRepository _data = new SignDataRepository();
        Dictionary<string, string> search = new Dictionary<string, string>();

        try
        {
            //----- 原始資料:取得所有資料 -----
            search.Add("DataID", Req_DataID);

            var query = _data.GetOne(search, out ErrMsg).FirstOrDefault();

            //----- 資料整理:繫結 ----- 
            if (query == null)
            {
                CustomExtension.AlertMsg("無法取得資料,即將返回列表頁.", Page_SearchUrl);
                return;
            }


            #region >> 欄位填寫 <<
            hf_DataID.Value = query.Data_ID.ToString();
            lb_TraceID.Text = query.TraceID;
            rbl_Display.SelectedValue = query.Display;
            tb_Sort.Text = query.Sort.ToString();
            tb_Subject.Text = query.Subject;
            tb_StartTime.Text = query.StartTime.ToDateString("yyyy/MM/dd HH:mm");
            tb_EndTime.Text = query.EndTime.ToDateString("yyyy/MM/dd HH:mm");
            ddl_Place.SelectedValue = query.Place.ToString();
            tb_OtherPlace.Text = query.OtherPlace;

            //Details
            ph_Details.Visible = true;  //與會名單tree / list
            ph_DetailJS.Visible = true; //tree js
            ph_tip1.Visible = false;  //提示建立資料

            #endregion


            //維護資訊
            info_Creater.Text = query.Create_Name;
            info_CreateTime.Text = query.Create_Time;
            info_Updater.Text = query.Update_Name;
            info_UpdateTime.Text = query.Update_Time;


            //簽到判斷, 大於0, 鎖住部份欄位
            Int32 _signCnt = query.SignCnt;
            if (_signCnt > 0)
            {
                tb_StartTime.Enabled = false;
                tb_EndTime.Enabled = false;
                ddl_Place.Enabled = false;
                tb_OtherPlace.Enabled = false;

                ph_LockMessage.Visible = true;
            }

            //載入單身
            LookupData_Detail1();
            LookupData_Detail2();

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

    #endregion


    #region -- 資料顯示:與會名單 --

    private void LookupData_Detail1()
    {
        //----- 宣告:資料參數 -----
        SignDataRepository _data = new SignDataRepository();

        try
        {
            //----- 原始資料:取得所有資料 -----
            var query = _data.GetCheckInList(Req_DataID, out ErrMsg);

            //----- 資料整理:繫結 ----- 
            lvList_NameList.DataSource = query;
            lvList_NameList.DataBind();

            query = null;
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


    protected void lvList_NameList_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            /*
              (看實際情況)下一版本改為ajax處理, 不然跳頁會不好用
            */
            //取得Key值
            string _SignWho = ((HiddenField)e.Item.FindControl("hf_NameID")).Value;

            //----- 宣告:資料參數 -----
            SignDataRepository _data = new SignDataRepository();

            try
            {
                //instance
                var data = new CheckIn
                {
                    SignWho = _SignWho,
                    SignTime = DateTime.Now.ToString().ToDateString("yyyy/MM/dd HH:mm:ss"),
                    FromIP = "",
                    IsAgent = "Y"
                };

                if (false == _data.CreateSign(Req_DataID, data, out ErrMsg))
                {
                    CustomExtension.AlertMsg("代簽失敗", "");
                    return;
                }

                //導向本頁
                Response.Redirect(thisPage + "#section2");
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


    protected void lvList_NameList_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem dataItem = (ListViewDataItem)e.Item;

                //取得資料:是否簽到
                string _isSign = DataBinder.Eval(dataItem.DataItem, "IsSign").ToString();
                //取得資料:是否代簽
                string _isAgent = DataBinder.Eval(dataItem.DataItem, "IsAgent").ToString();
                string _agentName = DataBinder.Eval(dataItem.DataItem, "AgentName").ToString();

                //代簽區域
                PlaceHolder ph_AdmSign = (PlaceHolder)e.Item.FindControl("ph_AdmSign");

                //簽到顯示
                Literal lt_Sign = (Literal)e.Item.FindControl("lt_Sign");
                if (_isSign.Equals("Y"))
                {
                    ph_AdmSign.Visible = false;
                    lt_Sign.Text = " <i class=\"check big icon green-text\"></i>";
                }
                else
                {
                    lt_Sign.Text = " <i class=\"close big icon grey-text\"></i>";
                }

                //代簽顯示
                Literal lt_Agent = (Literal)e.Item.FindControl("lt_Agent");
                if (_isAgent.Equals("Y"))
                {
                    lt_Agent.Text = _agentName;
                }

            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion


    #region -- 資料顯示:名單外簽到 --

    private void LookupData_Detail2()
    {
        //----- 宣告:資料參數 -----
        SignDataRepository _data = new SignDataRepository();

        try
        {
            //----- 原始資料:取得所有資料 -----
            var query = _data.GetCheckInList_NoName(Req_DataID, out ErrMsg);

            //----- 資料整理:繫結 ----- 
            lvList_unNamedList.DataSource = query;
            lvList_unNamedList.DataBind();

            query = null;
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


    #endregion


    #region -- 資料編輯 --

    /// <summary>
    /// 資料新增
    /// </summary>
    private void Add_Data()
    {
        //----- 宣告:資料參數 -----
        SignDataRepository _data = new SignDataRepository();

        try
        {
            //----- 設定:資料欄位 -----
            //產生Guid
            string guid = CustomExtension.GetGuid();
            string _traceID = NewTraceID();
            string _subject = tb_Subject.Text;
            string _disp = rbl_Display.SelectedValue;
            Int32 _sort = Convert.ToInt32(tb_Sort.Text);
            string _startTime = tb_StartTime.Text;
            string _endTime = tb_EndTime.Text;
            Int32 _place = Convert.ToInt32(ddl_Place.SelectedValue);
            string _otherPlace = tb_OtherPlace.Text;


            //instance
            var data = new BaseData
            {
                Data_ID = new Guid(guid),
                TraceID = _traceID,
                Subject = _subject,
                Display = _disp,
                Sort = _sort,
                StartTime = _startTime,
                EndTime = _endTime,
                Place = _place,
                OtherPlace = _otherPlace
            };


            //----- 方法:新增資料 -----
            if (!_data.Create(data, out ErrMsg))
            {
                ph_ErrMessage.Visible = true;
                lt_ShowMsg.Text = ErrMsg;
                CustomExtension.AlertMsg("新增失敗", "");
                return;
            }
            else
            {
                //導向本頁
                string url = FuncPath() + "/Edit/" + guid + "#section1";
                Response.Redirect(url);
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


    /// <summary>
    /// 資料修改
    /// </summary>
    private void Edit_Data()
    {
        //----- 宣告:資料參數 -----
        SignDataRepository _data = new SignDataRepository();

        try
        {
            //----- 設定:資料欄位 -----
            string _dataID = hf_DataID.Value;

            string _subject = tb_Subject.Text;
            string _disp = rbl_Display.SelectedValue;
            Int32 _sort = Convert.ToInt32(tb_Sort.Text);
            string _startTime = tb_StartTime.Text;
            string _endTime = tb_EndTime.Text;
            Int32 _place = Convert.ToInt32(ddl_Place.SelectedValue);
            string _otherPlace = tb_OtherPlace.Text;

            //instance
            var data = new BaseData
            {
                Subject = _subject,
                Display = _disp,
                Sort = _sort,
                StartTime = _startTime,
                EndTime = _endTime,
                Place = _place,
                OtherPlace = _otherPlace
            };

            //----- 方法:更新資料 -----
            if (!_data.Update(_dataID, data, out ErrMsg))
            {
                ph_ErrMessage.Visible = true;
                lt_ShowMsg.Text = ErrMsg;
                CustomExtension.AlertMsg("更新失敗", "");
                return;
            }
            else
            {
                //導向本頁
                Response.Redirect(thisPage);

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

    #endregion


    #region -- 按鈕事件 --
    /// <summary>
    /// SAVE
    /// </summary>
    private void doSave()
    {
        string errTxt = "";

        if (string.IsNullOrWhiteSpace(tb_Subject.Text))
        {
            errTxt += "會議主題\\n";
        }
        if (string.IsNullOrWhiteSpace(ddl_Place.SelectedValue))
        {
            errTxt += "會議室\\n";
        }

        string _sTime = tb_StartTime.Text;
        string _eTime = tb_EndTime.Text;
        if (Convert.ToDateTime(_sTime) > Convert.ToDateTime(_eTime))
        {
            errTxt += "會議時間不可逆行\\n";
        }

        //alert
        if (!string.IsNullOrEmpty(errTxt))
        {
            CustomExtension.AlertMsg("=== 請檢查以下欄位 ===\\n" + errTxt, "");
            return;
        }

        /* 執行新增/更新 */
        if (string.IsNullOrEmpty(hf_DataID.Value))
        {
            Add_Data();
        }
        else
        {
            Edit_Data();
        }
    }


    protected void btn_doSaveBase_Click(object sender, EventArgs e)
    {
        doSave();
    }

    /// <summary>
    /// 名單設定
    /// </summary>
    protected void btn_Setting_Click(object sender, EventArgs e)
    {
        //----- 判斷 -----
        //[欄位檢查] - 權限編號
        string inputValue = tree_Values.Text;
        if (string.IsNullOrEmpty(inputValue))
        {
            CustomExtension.AlertMsg("名單未勾選任何人,請確認!", "");
            return;
        }


        //[取得參數值] - 編號組合
        string[] strAry = Regex.Split(inputValue, @"\,{1}");
        var query = from el in strAry
                    select new
                    {
                        Val = el.ToString().Trim()
                    };

        //----- 宣告 -----
        List<NameList> dataList = new List<NameList>();
        foreach (var item in query)
        {
            //加入項目
            var data = new NameList
            {
                Who = item.Val
            };

            //將項目加入至集合
            dataList.Add(data);
        }


        //----- 宣告:資料參數 -----
        SignDataRepository _data = new SignDataRepository();

        //----- 方法:更新資料 -----
        if (false == _data.CreateDetail(Req_DataID, dataList, out ErrMsg))
        {
            CustomExtension.AlertMsg("名單設定失敗", "");
            return;
        }
        else
        {
            //導向本頁
            Response.Redirect(thisPage + "#section1");
        }
    }
    #endregion


    #region -- 附加功能 --

    /// <summary>
    /// 取得類別資料 
    /// </summary>
    /// <param name="ddl">下拉選單object</param>
    /// <param name="type">1:會議室</param>
    /// <param name="rootName">第一選項顯示名稱</param>
    private void Get_ClassList(DropDownList ddl, string type, string rootName)
    {
        //----- 宣告:資料參數 -----
        SignDataRepository _data = new SignDataRepository();

        //----- 原始資料:取得所有資料 -----
        var query = _data.GetClassItem(type, Req_Lang, out ErrMsg);


        //----- 資料整理 -----
        ddl.Items.Clear();

        if (!string.IsNullOrEmpty(rootName))
        {
            ddl.Items.Add(new ListItem(rootName, ""));
        }

        foreach (var item in query)
        {
            string fullLabel = item.Label;
            ddl.Items.Add(new ListItem(fullLabel, item.ID.ToString()));
        }

        query = null;
    }

    /// <summary>
    /// New TraceID
    /// </summary>
    /// <returns></returns>
    private string NewTraceID()
    {
        //產生TraceID
        long ts = Cryptograph.GetCurrentTime();

        Random rnd = new Random();
        int myRnd = rnd.Next(1, 99);

        return "{0}{1}".FormatThis(ts, myRnd);
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
        return "{0}{1}/SignManage".FormatThis(
            fn_Param.WebUrl
            , Req_Lang);
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
            _Req_DataID = value;
        }
    }


    /// <summary>
    /// 本頁網址
    /// </summary>
    private string _thisPage;
    public string thisPage
    {
        get
        {
            return "{0}/Edit/{1}".FormatThis(FuncPath(), Req_DataID);
        }
        set
        {
            _thisPage = value;
        }
    }


    /// <summary>
    /// 設定參數 - 列表頁Url
    /// </summary>
    private string _Page_SearchUrl;
    public string Page_SearchUrl
    {
        get
        {
            string tempUrl = CustomExtension.getCookie("SignManage");

            return string.IsNullOrWhiteSpace(tempUrl) ? FuncPath() : Server.UrlDecode(tempUrl);
        }
        set
        {
            _Page_SearchUrl = value;
        }
    }

    #endregion


}