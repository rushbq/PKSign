<%@ Page Title="編輯簽到本" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Edit.aspx.cs" Inherits="myManage_Edit" %>

<%@ Import Namespace="PKLib_Method.Methods" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CssContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!-- 工具列 Start -->
    <div class="myContentHeader">
        <div class="ui small menu toolbar">
            <div class="item">
                <div class="ui small breadcrumb">
                    <div class="section">簽到本管理</div>
                    <i class="right angle icon divider"></i>
                    <h5 class="active section red-text text-darken-2">編輯簽到本
                    </h5>
                </div>
            </div>
            <div class="right menu">
                <a class="anchor" id="top"></a>
            </div>
        </div>
    </div>
    <!-- 工具列 End -->
    <!-- 內容 Start -->
    <div class="myContentBody">
        <div class="ui grid">
            <div class="row">
                <!-- Left Body Content Start -->
                <div id="myStickyBody" class="thirteen wide column">
                    <div class="ui attached segment grey-bg lighten-5">
                        <asp:PlaceHolder ID="ph_ErrMessage" runat="server" Visible="false">
                            <div class="ui negative message">
                                <div class="header">
                                    Oops!!
                                </div>
                                <asp:Literal ID="lt_ShowMsg" runat="server"></asp:Literal>
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="ph_LockMessage" runat="server" Visible="false">
                            <div class="ui positive message">
                                <div class="header">
                                    注意事項
                                </div>
                                <p>
                                    若會議已開始簽到, 以下欄位將會鎖住不可修改。                               
                                </p>
                                <ul>
                                    <li>會議時間</li>
                                    <li>會議室</li>
                                </ul>
                            </div>
                        </asp:PlaceHolder>


                        <!-- Section-基本資料 Start -->
                        <div class="ui segments">
                            <div class="ui green segment">
                                <h5 class="ui header"><a class="anchor" id="baseData"></a>基本資料</h5>
                            </div>
                            <div id="formBase" class="ui small form segment">
                                <div class="fields">
                                    <div class="seven wide field">
                                        <label>追蹤編號</label>
                                        <asp:Label ID="lb_TraceID" runat="server" CssClass="ui red basic large label">系統自動產生</asp:Label>
                                    </div>
                                    <div class="five wide field">
                                        <label>前台顯示</label>
                                        <asp:RadioButtonList ID="rbl_Display" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="Y" Selected="True">顯示&nbsp;</asp:ListItem>
                                            <asp:ListItem Value="N">隱藏</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="four wide field">
                                        <label>自訂排序&nbsp;<small>(數字小的在前面)</small></label>
                                        <asp:TextBox ID="tb_Sort" runat="server" MaxLength="4" type="number" min="1" max="999" placeholder="數字小的在前面" autocomplete="off">999</asp:TextBox>
                                    </div>
                                </div>
                                <div class="fields">
                                    <div class="sixteen wide required field">
                                        <label>會議主題</label>
                                        <asp:TextBox ID="tb_Subject" runat="server" MaxLength="80" placeholder="會議主題" autocomplete="off"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="fields">
                                    <div class="seven wide required field">
                                        <label>會議時間</label>
                                        <div class="two fields">
                                            <div class="field">
                                                <div class="ui left icon input datepicker">
                                                    <asp:TextBox ID="tb_StartTime" runat="server" placeholder="開始日" autocomplete="off"></asp:TextBox>
                                                    <i class="calendar alternate outline icon"></i>
                                                </div>
                                            </div>
                                            <div class="field">
                                                <div class="ui left icon input datepicker">
                                                    <asp:TextBox ID="tb_EndTime" runat="server" placeholder="結束日" autocomplete="off"></asp:TextBox>
                                                    <i class="calendar alternate icon"></i>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="nine wide field">
                                        <label>會議室</label>
                                        <div class="fields">
                                            <div class="ten wide field">
                                                <asp:DropDownList ID="ddl_Place" runat="server" CssClass="fluid">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="six wide field">
                                                <asp:TextBox ID="tb_OtherPlace" runat="server" MaxLength="30" placeholder="其他地點" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="ui right aligned segment">
                                <asp:PlaceHolder ID="ph_tip1" runat="server">
                                    <div class="ui right pointing red basic label">請先建立基本資料,再進行名單設定.</div>
                                </asp:PlaceHolder>
                                <button id="doSaveBase" type="button" class="ui green small button">
                                    <i class="save icon"></i>存檔
                                </button>
                                <asp:HiddenField ID="hf_DataID" runat="server" />
                                <asp:Button ID="btn_doSaveBase" runat="server" Text="Save" OnClick="btn_doSaveBase_Click" Style="display: none;" />
                            </div>
                        </div>
                        <!-- Section-基本資料 End -->

                        <asp:PlaceHolder ID="ph_Details" runat="server" Visible="false">
                            <!-- Section-與會名單設定 Start -->
                            <div class="ui segments" style="min-height: 500px">
                                <div class="ui brown segment">
                                    <h5 class="ui header"><a class="anchor" id="section1"></a>名單設定</h5>
                                </div>
                                <div class="ui segment">
                                    <!-- 名單設定 tab Start -->
                                    <div>
                                        <button type="button" id="treeOpen" class="ui green small button"><i class="plus square icon"></i>展開</button>
                                        <button type="button" id="treeClose" class="ui grey small button"><i class="minus square icon"></i>收合</button>
                                        <button type="button" id="setTree" class="ui blue small button"><i class="clipboard check icon"></i>儲存名單</button>
                                        <div style="display: none" class="serversidecontroller">
                                            <asp:Button ID="btn_Setting" runat="server" Text="Set" OnClick="btn_Setting_Click" />
                                            <asp:TextBox ID="tree_Values" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <!-- zTree js -->
                                    <div id="userList" class="ztree"></div>
                                    <!-- 名單設定 tab End -->

                                </div>
                            </div>
                            <!-- Section-與會名單設定 End -->

                            <!-- Section-簽到名單 Start -->
                            <div class="ui segments">
                                <div class="ui blue segment">
                                    <h5 class="ui header"><a class="anchor" id="section2"></a>簽到名單</h5>
                                </div>
                                <div class="ui segment">
                                    <asp:ListView ID="lvList_NameList" runat="server" ItemPlaceholderID="ph_Items" OnItemCommand="lvList_NameList_ItemCommand" OnItemDataBound="lvList_NameList_ItemDataBound">
                                        <LayoutTemplate>
                                            <table id="listTable" class="ui celled striped small table" style="width: 100%">
                                                <thead>
                                                    <tr>
                                                        <th class="center aligned">區域</th>
                                                        <th>人員</th>
                                                        <th class="no-sort no-search center aligned">簽到時間</th>
                                                        <th class="no-sort no-search center aligned">是否簽到</th>
                                                        <th class="no-sort center aligned">管理者代簽</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:PlaceHolder ID="ph_Items" runat="server" />
                                                </tbody>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="center aligned">
                                                    <%#fn_Param.GetCorpName(Eval("Area").ToString()) %>
                                                </td>
                                                <td>
                                                    <%#Eval("Display_Name") %>&nbsp;(<%#Eval("NickName") %>)
                                                </td>
                                                <td class="center aligned">
                                                    <%#Eval("SignTime").ToString().ToDateString("yyyy/MM/dd HH:mm") %>
                                                </td>
                                                <td class="center aligned">
                                                    <asp:Literal ID="lt_Sign" runat="server"></asp:Literal>
                                                </td>
                                                <td class="center aligned">
                                                    <asp:Literal ID="lt_Agent" runat="server"></asp:Literal>
                                                    <asp:PlaceHolder ID="ph_AdmSign" runat="server">
                                                        <asp:LinkButton ID="lbtn_Add" runat="server" CssClass="ui red icon small button" ValidationGroup="List" CommandName="doInsert" OnClientClick="return confirm('確定要代簽嗎?')"><i class="user plus icon"></i></asp:LinkButton>
                                                        <asp:HiddenField ID="hf_NameID" runat="server" Value='<%#Eval("NameID") %>' />
                                                    </asp:PlaceHolder>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <div class="ui placeholder segment">
                                                <div class="ui icon header">
                                                    <i class="users icon"></i>
                                                    名單未設定
                                                </div>
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </div>
                            </div>
                            <!-- Section-簽到名單 End -->


                            <!-- Section-名單外簽到 Start -->
                            <div class="ui segments">
                                <div class="ui purple segment">
                                    <h5 class="ui header"><a class="anchor" id="section3"></a>名單外簽到</h5>
                                </div>
                                <div class="ui segment">
                                    <asp:ListView ID="lvList_unNamedList" runat="server" ItemPlaceholderID="ph_Items">
                                        <LayoutTemplate>
                                            <table id="unNamedTable" class="ui celled striped small table" style="width: 100%">
                                                <thead>
                                                    <tr>
                                                        <th class="center aligned">區域</th>
                                                        <th>人員</th>
                                                        <th class="no-sort no-search center aligned">簽到時間</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:PlaceHolder ID="ph_Items" runat="server" />
                                                </tbody>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="center aligned">
                                                    <%#fn_Param.GetCorpName(Eval("Area").ToString()) %>
                                                </td>
                                                <td>
                                                    <%#Eval("Display_Name") %>&nbsp;(<%#Eval("NickName") %>)
                                                </td>
                                                <td class="center aligned">
                                                    <%#Eval("SignTime").ToString().ToDateString("yyyy/MM/dd HH:mm") %>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <div class="ui placeholder segment">
                                                <div class="ui icon header">
                                                    <i class="user outline icon"></i>
                                                    無額外簽到者
                                                </div>
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </div>
                            </div>
                            <!-- Section-名單外簽到 End -->
                        </asp:PlaceHolder>

                        <!-- Section-維護資訊 Start -->
                        <div class="ui segments">
                            <div class="ui grey segment">
                                <h5 class="ui header"><a class="anchor" id="infoData"></a>維護資訊</h5>
                            </div>
                            <div class="ui segment">
                                <table class="ui celled small four column table">
                                    <thead>
                                        <tr>
                                            <th colspan="2" class="center aligned">建立</th>
                                            <th colspan="2" class="center aligned">最後更新</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr class="center aligned">
                                            <td>
                                                <asp:Literal ID="info_Creater" runat="server">資料建立中...</asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="info_CreateTime" runat="server">資料建立中...</asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="info_Updater" runat="server"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:Literal ID="info_UpdateTime" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <!-- Section-維護資訊 End -->
                    </div>

                </div>
                <!-- Left Body Content End -->

                <!-- Right Navi Menu Start -->
                <div class="three wide column">
                    <div class="ui sticky">
                        <div id="fastjump" class="ui secondary vertical pointing fluid text menu">
                            <div class="header item">快速跳轉<i class="dropdown icon"></i></div>
                            <a href="#baseData" class="item">基本設定</a>
                            <a href="#section1" class="item">名單設定</a>
                            <a href="#section2" class="item">簽到名單</a>
                            <a href="#section3" class="item">名單外簽到</a>
                            <a href="#top" class="item"><i class="angle double up icon"></i>到頂端</a>
                        </div>

                        <div class="ui vertical text menu">
                            <div class="header item">功能按鈕</div>
                            <div class="item">
                                <a href="<%:Page_SearchUrl %>" class="ui small button"><i class="undo icon"></i>返回列表</a>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Right Navi Menu End -->
            </div>
        </div>

    </div>
    <!-- 內容 End -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BottomContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="Server">
    <script>
        $(function () {
            //Save Click
            $("#doSaveBase").click(function () {
                $("#formBase").addClass("loading");
                $("#MainContent_btn_doSaveBase").trigger("click");
            });

            //init dropdown list
            $('select').dropdown();

            //tab menu
            $('.menu .item').tab();
        });
    </script>

    <%-- 快速選單 --%>
    <script src="<%=fn_Param.WebUrl %>javascript/sticky.js"></script>
    <%-- 日期選擇器 Start --%>
    <link href="<%=fn_Param.CDNUrl %>plugin/Semantic-UI-Calendar0.0.8/calendar.min.css" rel="stylesheet" />
    <script src="<%=fn_Param.CDNUrl %>plugin/Semantic-UI-Calendar0.0.8/calendar.min.js"></script>
    <script src="<%=fn_Param.CDNUrl %>plugin/Semantic-UI-Calendar0.0.8/options.js"></script>
    <script>
        $(function () {
            //載入datepicker
            $('.datepicker').calendar(calendarOptsByTime_Range);
        });
    </script>
    <%-- 日期選擇器 End --%>

    <asp:PlaceHolder ID="ph_DetailJS" runat="server">
        <%-- zTree Start --%>
        <link rel="stylesheet" href="<%=fn_Param.WebUrl %>plugins/zTree/css/style.min.css" />
        <script src="<%=fn_Param.WebUrl %>plugins/zTree/jquery.ztree.core-3.5.min.js"></script>
        <script src="<%=fn_Param.WebUrl %>plugins/zTree/jquery.ztree.excheck-3.5.min.js"></script>
        <script>
            //--- zTree 設定 Start ---
            var setting = {
                view: {
                    dblClickExpand: false   //已使用onclick展開,故將雙擊展開關閉                    
                },
                callback: {
                    onClick: MMonClick
                },
                check: {
                    enable: true
                },
                data: {
                    simpleData: {
                        enable: true
                    }
                }
            };

            //Event - onClick
            function MMonClick(e, treeId, treeNode) {
                var zTree = $.fn.zTree.getZTreeObj(treeId);
                zTree.expandNode(treeNode);
            }
            //--- zTree 設定 End ---
        </script>
        <script>
            $(function () {
                /*
                    取得人員List
                */
                var jqxhr = $.post("<%=fn_Param.WebUrl%>Ajax_Data/GetSignUserList.ashx", {
                    id: '<%=Req_DataID%>',
                })
            .done(function (data) {
                //載入選單
                $.fn.zTree.init($("#userList"), setting, data)
            })
            .fail(function () {
                alert("人員選單載入失敗");
            });

            });

            //全展開
            $("#treeOpen").on("click", function () {
                //選單id
                var myTreeName = "userList";

                var treeObj = $.fn.zTree.getZTreeObj(myTreeName);

                treeObj.expandAll(true);
            });
            //全收合
            $("#treeClose").on("click", function () {
                //選單id
                var myTreeName = "userList";

                var treeObj = $.fn.zTree.getZTreeObj(myTreeName);

                treeObj.expandAll(false);
            });

            /*
               取得已勾選的項目ID
            */
            $("#setTree").on("click", function () {
                //選單id
                var myTreeName = "userList";
                var valAry = [];

                //宣告tree物件
                var treeObj = $.fn.zTree.getZTreeObj(myTreeName);

                //取得節點array
                var nodes = treeObj.getCheckedNodes(true);

                //將id丟入陣列
                for (var row = 0; row < nodes.length; row++) {
                    //只取開頭為'v_'的值
                    var myval = nodes[row].id;
                    if (myval.substring(0, 2) == "v_") {
                        valAry.push(myval.replace("v_", ""));
                    }
                }

                //將陣列組成以','分隔的字串，並填入欄位
                $("#MainContent_tree_Values").val(valAry.join(","));

                //觸發設定 click
                $("#MainContent_btn_Setting").trigger("click");

            });
        </script>
        <%-- zTree End --%>
    </asp:PlaceHolder>


    <%-- DataTable Start --%>
    <link href="https://cdn.datatables.net/1.10.13/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="https://cdn.datatables.net/1.10.11/js/jquery.dataTables.min.js"></script>
    <script>
        $(function () {
            /*
             [使用DataTable]
             注意:標題欄須與內容欄數量相等
           */
            var table = $('#listTable').DataTable({
                "searching": true,  //搜尋
                "ordering": true,   //排序
                "paging": true,     //分頁
                "info": true,      //筆數資訊
                "pageLength": 10,   //每頁筆數
                "language": {
                    //自訂筆數顯示選單
                    "lengthMenu": '',
                    //自訂頁數資訊
                    "info": '共 <b>_TOTAL_</b> 筆 ,目前頁次 <b>_PAGE_</b> / _PAGES_, 每頁 5 筆.'
                },
                //讓不排序的欄位在初始化時不出現排序圖
                "order": [],
                //欄位定義:css=no-sort不排序; css=no-search不搜尋
                "columnDefs": [
                    { "targets": 'no-sort', "orderable": false, },
                    { "targets": 'no-search', "searchable": false }
                ],
                //捲軸設定
                "scrollY": '70vh',
                "scrollCollapse": true,
                "scrollX": false

            });

        });
    </script>
    <%-- DataTable End --%>
</asp:Content>

