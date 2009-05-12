<%@ Page Language="C#" Inherits="Maverick.Web.ModuleFramework.ModuleViewPage<Maverick.Models.Page>" %>
<% if (Model.Id == -1) {%>
This page will be at the root level
<% } else {%>
This page will be a child of the page: <%=Model.Title%>
<% } %>
