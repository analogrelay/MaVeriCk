<%@ Page Language="C#" Inherits="Maverick.Web.ModuleFramework.ModuleViewPage" %>
<ul id="<%= Html.Id("tasks")%>">
    <li>Finish this module</li>
</ul>
<% using (Ajax.BeginForm("Create", new AjaxOptions() { UpdateTargetId = Html.Id("tasks"), InsertionMode = InsertionMode.InsertAfter })) {%>
<input id="<%= Html.Id("task") %>" name="id" />
<input type="submit" value="Add Task" />
<% } %>