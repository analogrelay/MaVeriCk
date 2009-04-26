<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% if(Request.IsAuthenticated) {%>
Welcome <%=Html.CurrentUser().DisplayName%>! <%=Html.LogoutLink("Logout")%>
<% } else { %>
<%-- Uncomment this line when you've configured the Azure Access Control Service --%>
<%--<%= Html.LoginLink("Login with your Windows Live ID", "AzureAcsLiveId") %>--%>
<%-- For debug and testing purposes only! --%>
<%= Html.LoginLink("Login as a Developer", "Debug") %>
<% } %>
