<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

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
       <%-- <span class="tagline">
            <%=fn_Param.WebUrl %>
        </span>--%>
    </h1>
    <div class="ui hidden divider"></div>
    <div class="ui segments">
        <div class="ui attached segment">
            <asp:PlaceHolder ID="ph_Data" runat="server">
                <div class="ui two cards">
                    <a class="red card" href="<%=FuncPath() %>/Sign/12345">
                        <div class="content">
                            <div class="header">2020-7月月會</div>
                            <div class="meta">
                                <span class="category">08:30~09:30</span>
                            </div>
                            <div class="description">
                                <p>台北六樓教育訓練中心</p>
                            </div>
                        </div>
                    </a>
                    <a class="orange card" href="#">
                        <div class="content">
                            <div class="header">2020主管會議</div>
                            <div class="meta">
                                <span class="category">08:30~19:30</span>
                            </div>
                            <div class="description">
                                <p>台北六樓教育訓練中心</p>
                            </div>
                        </div>
                    </a>
                    <a class="red card" href="#">
                        <div class="content">
                            <div class="header">2020主管會議</div>
                            <div class="meta">
                                <span class="category">08:30~19:30</span>
                            </div>
                            <div class="description">
                                <p>台北六樓教育訓練中心</p>
                            </div>
                        </div>
                    </a>
                    <a class="orange card" href="#">
                        <div class="content">
                            <div class="header">2020主管會議</div>
                            <div class="meta">
                                <span class="category">08:30~19:30</span>
                            </div>
                            <div class="description">
                                <p>台北六樓教育訓練中心</p>
                            </div>
                        </div>
                    </a>
                    <a class="red card" href="#">
                        <div class="content">
                            <div class="header">2020主管會議</div>
                            <div class="meta">
                                <span class="category">08:30~19:30</span>
                            </div>
                            <div class="description">
                                <p>台北六樓教育訓練中心</p>
                            </div>
                        </div>
                    </a>
                </div>

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

