<%@ Page Title="" Language="C#" MasterPageFile="Site.Master" Inherits="System.Web.Mvc.ViewPage<Maverick.Web.Models.PageViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	View
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Model.Page.Title %></h2>
    
    <% foreach(ModuleRequestResult moduleResult in Model["Content"].ModuleResults) { %>
        <% Html.RenderPartial("Module", moduleResult); %>
    <% } %>

</asp:Content>
