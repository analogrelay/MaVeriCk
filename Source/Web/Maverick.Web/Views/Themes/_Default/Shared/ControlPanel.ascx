<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Maverick.Web.Models.ControlPanelViewModel>" %>
<div id="controlPanelBody">
<h3>Super-Snazzy Control Panel</h3>
<% using (Html.BeginForm("Create", "Module", FormMethod.Post, new {id="addModuleForm"})) {%>
<%= Html.DropDownList("Modules") %>
<input type="submit" value="Add Module to Page" />
<% } %>
</div>