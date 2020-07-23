<%@ Page Title="登入系統" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="myPage_Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CssContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!-- 工具列 Start -->
    <div class="myContentHeader">
        <div class="ui small menu toolbar">
            <div class="item">
                <div class="ui small breadcrumb">
                    <div class="section">線上簽到</div>
                    <i class="right angle icon divider"></i>
                    <h5 class="active section red-text text-darken-2">登入系統
                    </h5>
                </div>
            </div>
            <div class="right menu">
            </div>
        </div>
    </div>
    <!-- 工具列 End -->

    <!-- 內容 Start -->
    <div class="myContentBody">
        <!-- Search Start -->
        <div class="ui orange attached segment">
            <div class="ui message">
                <h1 class="ui header">請使用員工工號與開機密碼進行登入</h1>
                <ul>
                    <li>若忘記密碼，請洽 #372</li>
                    <li>「記住我」可記憶 12 個月</li>
                    <li>請<span class="red-text">不要使用</span>「無痕模式」或「私密瀏覽」，否則無法記憶登入資訊。</li>
                </ul>
            </div>
            <div class="ui form">
                <div class="ui segment">
                    <div class="field">
                        <div class="ui left icon input">
                            <i class="user icon"></i>
                            <asp:TextBox ID="tb_Account" runat="server" MaxLength="5" placeholder="輸入員工工號(ex:10123)" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="field">
                        <div class="ui left icon input">
                            <i class="lock icon"></i>
                            <asp:TextBox ID="tb_Password" runat="server" MaxLength="30" TextMode="Password" placeholder="輸入密碼" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="field">
                        <asp:CheckBox ID="cb_Remember" runat="server" Checked="true" />&nbsp;記住我 (12個月)
                    </div>
                    <asp:Button ID="btn_Login" runat="server" Text="登入" CssClass="ui fluid large teal button" OnClick="btn_Login_Click" />
                </div>
            </div>

        </div>
        <!-- Search End -->


    </div>
    <!-- 內容 End -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BottomContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="Server">
</asp:Content>

