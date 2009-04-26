<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Maverick.Web.ModuleFramework.ModuleRequestResult>" %>
<h3><%= Model.Module.Title %></h3>
<% Html.RenderModule(Model); %>

<% if (Context.User.IsInRole("SuperUser")) { %>
    <% using (Html.BeginForm("Delete", "Module")) {%>
    <%= Html.Hidden("id", Model.Module.Id) %>
    <input type="submit" value="Delete this Module" />
    <% } %>
<% } %>