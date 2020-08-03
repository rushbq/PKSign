<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Import Namespace="PKLib_Method.Methods" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CssContent" runat="Server">
    <style>
        h1 {
            font-size: 3em;
            color: rgba(255, 255, 255, 1);
            line-height: 1.2 !important;
            margin: 0px 0px 0px;
            padding-bottom: 0px;
            -moz-perspective: 500px;
            -webkit-perspective: 500px;
            perspective: 500px;
            text-shadow: 0px 0px 10px rgba(0, 0, 0, 0.2);
            -moz-transform-style: preserve-3d;
            -webkit-transform-style: preserve-3d;
            transform-style: preserve-3d;
        }

        .library {
            display: block;
            font-size: 1.75em;
            font-weight: bold;
        }

        .tagline {
            font-size: 0.75em;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="ui hidden divider"></div>
    <h1 class="ui center aligned header">
        <span class="library">寶工線上簽到
        </span>
        <span class="tagline">
            <%=DateTime.Today.ToString().ToDateString("yyyy 年 M 月 dd 日") %>
        </span>
    </h1>
    <div class="ui hidden divider"></div>
    <div class="ui segments">
        <div class="ui attached segment">
            <asp:PlaceHolder ID="ph_Data" runat="server">
                <asp:ListView ID="lvList" runat="server" ItemPlaceholderID="ph_Items" OnItemDataBound="lvList_ItemDataBound">
                    <LayoutTemplate>
                        <asp:PlaceHolder ID="ph_Items" runat="server" />
                    </LayoutTemplate>
                    <ItemTemplate>
                        <a class="ui red fluid card" href="<%=FuncPath() %>/Sign/<%#Eval("Data_ID") %>">
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
                            <asp:PlaceHolder ID="ph_signed" runat="server">
                                <div class="extra content">
                                    <div class="right floated teal-text text-darken-2">
                                        <strong>
                                            <i class="check icon"></i>
                                            已簽到&nbsp; at <%#Eval("SignTime").ToString().ToDateString("HH:mm") %>
                                        </strong>
                                    </div>
                                </div>
                            </asp:PlaceHolder>
                        </a>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <a class="ui olive fluid card" href="<%=FuncPath() %>/Sign/<%#Eval("Data_ID") %>">
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
                            <asp:PlaceHolder ID="ph_signed" runat="server">
                                <div class="extra content">
                                    <div class="right floated teal-text text-darken-2">
                                        <i class="check icon"></i>
                                        已簽到&nbsp; at <%#Eval("SignTime").ToString().ToDateString("HH:mm") %>
                                    </div>
                                </div>
                            </asp:PlaceHolder>
                        </a>
                    </AlternatingItemTemplate>
                </asp:ListView>
            </asp:PlaceHolder>

            <!-- Empty Content Start -->
            <asp:PlaceHolder ID="ph_EmptyData" runat="server" Visible="false">
                <div class="ui placeholder segment">
                    <div class="ui icon header">
                        <i class="microphone slash icon"></i>
                        目前沒有會議要簽到,若有需求請洽管理部.
                    </div>
                </div>
            </asp:PlaceHolder>
            <!-- Empty Content End -->
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BottomContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="Server">
</asp:Content>

