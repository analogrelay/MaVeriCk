<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Login.aspx
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Enter your OpenID URL</h2>
    
    <% using (Html.BeginLoginForm()) {%>
    <%= Html.TextBox("openid_identifier")%>
    <%= Html.Hidden("returnUrl", ViewData["ReturnUrl"]) %>
    <input type="submit" value="Login" />
    <% } %>
    
</asp:Content>
