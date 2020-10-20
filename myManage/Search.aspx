<%@ Page Title="簽到本清單" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Search.aspx.cs" Inherits="myManage_Search" %>

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
                    <div class="active section red-text text-darken-2">
                        簽到本清單
                    </div>
                </div>
            </div>
            <div class="right menu">
                <a href="<%=FuncPath() %>/Edit/" class="item"><i class="plus icon"></i>建立簽到本</a>
            </div>
        </div>
    </div>
    <!-- 工具列 End -->

    <!-- 內容 Start -->
    <div class="myContentBody">
        <!-- Advance Search Start -->
        <div class="ui orange attached segment">
            <div class="ui small form">
                <div class="fields">
                    <div class="six wide field">
                        <label>會議日期</label>
                        <div class="two fields">
                            <div class="field">
                                <div class="ui left icon input datepicker">
                                    <asp:TextBox ID="filter_sDate" runat="server" placeholder="開始日" autocomplete="off"></asp:TextBox>
                                    <i class="calendar alternate outline icon"></i>
                                </div>
                            </div>
                            <div class="field">
                                <div class="ui left icon input datepicker">
                                    <asp:TextBox ID="filter_eDate" runat="server" placeholder="結束日" autocomplete="off"></asp:TextBox>
                                    <i class="calendar alternate icon"></i>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="four wide field">
                        <label>會議地點</label>
                        <asp:DropDownList ID="filter_place" runat="server" CssClass="fluid"></asp:DropDownList>
                    </div>
                    <div class="six wide field">
                        <label>關鍵字查詢</label>
                        <asp:TextBox ID="filter_Keyword" runat="server" autocomplete="off" placeholder="查詢:追蹤編號, 會議主題" MaxLength="20"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="ui two column grid">
                <div class="column">
                    <a href="<%=FuncPath() %>" class="ui small button"><i class="refresh icon"></i>重置條件</a>
                </div>
                <div class="column right aligned">
                    <button type="button" id="doSearch" class="ui blue small button"><i class="search icon"></i>查詢</button>
                    <asp:Button ID="btn_Search" runat="server" Text="Button" OnClick="btn_Search_Click" Style="display: none" />
                </div>
            </div>

        </div>
        <!-- Advance Search End -->

        <!-- Empty Content Start -->
        <asp:PlaceHolder ID="ph_EmptyData" runat="server">
            <div class="ui placeholder segment">
                <div class="ui icon header">
                    <i class="search icon"></i>
                    目前條件查無資料，請重新查詢。
                </div>
            </div>
        </asp:PlaceHolder>
        <!-- Empty Content End -->

        <!-- List Content Start -->
        <asp:PlaceHolder ID="ph_Data" runat="server">
            <asp:ListView ID="lvDataList" runat="server" ItemPlaceholderID="ph_Items" OnItemCommand="lvDataList_ItemCommand" OnItemDataBound="lvDataList_ItemDataBound">
                <LayoutTemplate>
                    <div class="ui green attached segment">
                        <table class="ui celled selectable compact small table">
                            <thead>
                                <tr>
                                    <th class="grey-bg lighten-3 center aligned">追蹤編號</th>
                                    <th class="grey-bg lighten-3">會議主題</th>
                                    <th class="grey-bg lighten-3 center aligned">地點</th>
                                    <th class="grey-bg lighten-3 center aligned">簽到時間</th>
                                    <th class="grey-bg lighten-3 center aligned collapsing">前台顯示</th>
                                    <th class="grey-bg lighten-3 center aligned collapsing">與會人數</th>
                                    <th class="grey-bg lighten-3 center aligned collapsing">簽到人數</th>
                                    <th class="grey-bg lighten-3"></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder ID="ph_Items" runat="server" />
                            </tbody>
                        </table>
                    </div>

                    <!-- List Pagination Start -->
                    <div class="ui mini bottom attached segment grey-bg lighten-4">
                        <asp:Literal ID="lt_Pager" runat="server"></asp:Literal>
                    </div>
                    <!-- List Pagination End -->
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="left aligned collapsing">
                            <b class="red-text text-darken-2"><%#Eval("TraceID") %></b>
                        </td>
                        <td class="green-text text-darken-2">
                            <h4><%#Eval("Subject") %></h4>
                        </td>
                        <td class="center aligned collapsing">
                            <div class="ui basic fluid label">
                                <%#Eval("PlaceName") %>&nbsp;<%#Eval("OtherPlace") %>
                            </div>
                        </td>
                        <td class="center aligned collapsing">
                            <div>
                                <div class="ui basic fluid label">
                                    開始<div class="detail"><%#Eval("StartTime") %></div>
                                </div>
                            </div>
                            <div>
                                <div class="ui basic fluid label">
                                    結束<div class="detail"><%#Eval("EndTime") %></div>
                                </div>
                            </div>
                        </td>
                        <td class="center aligned positive">
                            <%#Eval("Display") %>
                        </td>
                        <td class="center aligned">
                            <strong><%#Eval("JoinCnt") %></strong>
                        </td>
                        <td class="center aligned error">
                            <strong><%#Eval("SignCnt") %></strong>
                        </td>
                        <td class="center aligned collapsing">
                            <asp:PlaceHolder ID="ph_Edit" runat="server">
                                <a class="ui small teal basic icon button" href="<%#FuncPath() %>/Edit/<%#Eval("Data_ID") %>">
                                    <i class="pencil icon"></i>
                                </a>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="ph_Del" runat="server">
                                <asp:LinkButton ID="lbtn_Close" runat="server" CssClass="ui small orange basic icon button" ValidationGroup="List" CommandName="doClose" OnClientClick="return confirm('確定刪除?')"><i class="trash alternate icon"></i></asp:LinkButton>
                            </asp:PlaceHolder>
                            <asp:HiddenField ID="hf_DataID" runat="server" Value='<%#Eval("Data_ID") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </asp:PlaceHolder>
        <!-- List Content End -->

    </div>
    <!-- 內容 End -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BottomContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="Server">
    <script>
        $(function () {
            //偵測enter
            $(".myContentBody").keypress(function (e) {
                code = (e.keyCode ? e.keyCode : e.which);
                if (code == 13) {
                    $("#doSearch").trigger("click");
                    //避免觸發submit
                    e.preventDefault();
                }
            });

            //[搜尋][查詢鈕] - 觸發查詢
            $("#doSearch").click(function () {
                $("#MainContent_btn_Search").trigger("click");
            });


            //init dropdown list
            $('select').dropdown();

        });
    </script>
    <%-- 日期選擇器 Start --%>
    <link href="<%=fn_Param.CDNUrl %>plugin/Semantic-UI-Calendar0.0.8/calendar.min.css" rel="stylesheet" />
    <script src="<%=fn_Param.CDNUrl %>plugin/Semantic-UI-Calendar0.0.8/calendar.min.js"></script>
    <script src="<%=fn_Param.CDNUrl %>plugin/Semantic-UI-Calendar0.0.8/options.js"></script>
    <script>
        $(function () {
            //載入datepicker
            $('.datepicker').calendar(calendarOpts_Range);
        });
    </script>
    <%-- 日期選擇器 End --%>
</asp:Content>

