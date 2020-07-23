<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Confirm.aspx.cs" Inherits="mySign_Confirm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CssContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="ui hidden divider"></div>
    <div class="ui container">
        <h1>簽到確認</h1>
        <div class="ui fluid card">
            <div class="content">
                <div class="header">2020-7月月會</div>
            </div>
            <div class="content">
                <div class="meta">
                    <span class="category">08:30~09:30</span>
                </div>
                <div class="description">
                    <p>台北六樓教育訓練中心</p>
                </div>
            </div>
            <div class="content">
                <h4 class="ui sub header">簽到人</h4>
                <div class="center aligned author blue-text text-darken-2">
                    <h3>資訊部-高高高高</h3>
                </div>
            </div>
            <div class="extra content">
                <div class="ui two column grid">
                    <div class="column">
                        <a href="<%=FuncPath() %>" class="ui button"><i class="undo icon"></i>重選</a>
                    </div>
                    <div class="column right aligned">
                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="ui huge orange button"><i class="pencil icon"></i>簽到</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BottomContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="Server">
</asp:Content>

