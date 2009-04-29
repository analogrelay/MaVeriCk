<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Maverick.Web.ModuleFramework.ModuleRequestResult>" %>
<!-- Start Module #<%= Model.Module.Id %> -->
<% if (Context.User.IsInRole("SuperUser")) {%>
<div class="module-container ui-widget-content ui-corner-all">
<div class="module-header ui-widget-header ui-corner-all">
    <% using (Html.BeginForm("Delete", "Module")) {%>
    <%=Html.Hidden("id", Model.Module.Id)%>
    <button class="module-action ui-state-default ui-corner-all" type="submit">
        <span class="ui-icon ui-icon-closethick"></span>
        Delete
    </button>
    <% } %>
    <h3>Module</h3>
</div>
<% } %>
<h3><%= Model.Module.Title %></h3>
<% Html.RenderModule(Model); %>

<% if (Context.User.IsInRole("SuperUser")) { %>
</div>
<% } %>
<!-- End Module #<%= Model.Module.Id %> -->