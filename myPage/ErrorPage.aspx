<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ErrorPage.aspx.cs" Inherits="myPage_ErrorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CssContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="myContentBody">
        <div class="ui red attached segment">
            <div class="ui placeholder segment">
                <div class="ui icon header">
                    <i class="exclamation icon"></i>
                    Oops...
                    <p>
                        <%=Req_Msg %>
                    </p>
                </div>
                <div class="inline">
                    <a href="<%=fn_Param.WebUrl %>" class="ui  button"><i class="home icon"></i>點我回首頁</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BottomContent" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptContent" runat="Server">
</asp:Content>

