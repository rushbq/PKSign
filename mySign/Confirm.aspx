<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Confirm.aspx.cs" Inherits="mySign_Confirm" %>

<%@ Import Namespace="PKLib_Method.Methods" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CssContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="ui hidden divider"></div>
    <div class="ui container">
        <h1 class="ui center aligned header">簽到確認</h1>

        <asp:ListView ID="lvList" runat="server" ItemPlaceholderID="ph_Items" OnItemCommand="lvList_ItemCommand" OnItemDataBound="lvList_ItemDataBound">
            <LayoutTemplate>
                <asp:PlaceHolder ID="ph_Items" runat="server" />
            </LayoutTemplate>
            <ItemTemplate>
                <div class="ui green fluid card">
                    <div class="content">
                        <div class="header"><%#Eval("Subject") %></div>
                    </div>
                    <div class="content">
                        <div class="ui large feed">
                            <div class="event">
                                <div class="content">
                                    <div class="summary grey-text text-darken-3">
                                        <i class="clock icon"></i>&nbsp;
                                                <%#Eval("StartTime").ToString().ToDateString("HH:mm") %> ~ <%#Eval("EndTime").ToString().ToDateString("HH:mm") %>
                                    </div>
                                </div>
                            </div>
                            <div class="event">
                                <div class="content">
                                    <div class="summary grey-text text-darken-3">
                                        <i class="map marker alternate icon"></i>&nbsp;<%#Eval("PlaceName") %>&nbsp;<%#Eval("OtherPlace") %>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="content">
                        <h4 class="ui sub header">簽到人</h4>
                        <div class="center aligned blue-text text-darken-2">
                            <h3><%=fn_Param.MemberName %></h3>
                        </div>
                    </div>
                    <div class="extra content">
                        <div class="ui two column grid">
                            <div class="column">
                                <a href="<%=FuncPath() %>" class="ui button"><i class="undo icon"></i>重選</a>
                            </div>
                            <div class="column right aligned">
                                <asp:PlaceHolder ID="ph_signButton" runat="server">
                                    <asp:LinkButton ID="lbtn_doSign" runat="server" CssClass="ui huge orange button" CommandName="doSign"><i class="pencil icon"></i>簽到</asp:LinkButton>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder ID="ph_signed" runat="server">
                                    <h3 class="teal-text text-darken-2">
                                        <i class="check icon"></i>
                                        已簽到&nbsp; at <%#Eval("SignTime").ToString().ToDateString("HH:mm") %>
                                    </h3>
                                </asp:PlaceHolder>

                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:ListView>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BottomContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="Server">
</asp:Content>

