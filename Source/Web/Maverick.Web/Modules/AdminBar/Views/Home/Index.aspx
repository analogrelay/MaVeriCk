<%@ Page Language="C#" Inherits="Maverick.Web.ModuleFramework.ModuleViewPage" %>
<div id="cp_menu">
    <!-- TODO: Move Style to CSS after merging with themeing_engine branch -->
    <ul style="display: inline">
        <li><img src="<%= Url.Content("~/Modules/AdminBar/Content/Images/Admin.png") %>" /> Administration</li>
        <li><a href="#">Page <img src="<%= Url.Content("~/Modules/AdminBar/Content/Images/DownArrow.png") %>" /></a></li>
        <li><a href="#">Site <img src="<%= Url.Content("~/Modules/AdminBar/Content/Images/DownArrow.png") %>" /></a></li>
        <li><a href="#">Application <img src="<%= Url.Content("~/Modules/AdminBar/Content/Images/DownArrow.png") %>" /></a></li>
    </ul>
</div>