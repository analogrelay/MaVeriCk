<%@ Page Language="C#" Inherits="Maverick.Web.ModuleFramework.ModuleViewPage<ControlPanelViewModel>" %>

<div id="cp_root">
    <div id="cp_title" class="ui-widget">
        <img src="<%= Url.Content("~/Modules/AdminBar/Content/Images/Admin.png") %>" /> Administration Menu:
    </div>
    <a id="cp_page_menu_trigger" class="cp_menu ui-widget ui-state-default ui-corner-all" href="#cp_page_menu">
        <span class="cp_icon_right ui-icon ui-icon-triangle-1-s"></span>Page
    </a>
    <div id="cp_page_menu" class="hidden">
        <ul>
            <li><a href="#">New Page ...</a></li>
            <li><a href="#">New Child Page ...</a></li>
            <li><a href="#">Delete this Page</a></li>
            <li><a id="cp_page_addmodule" href="#">Add a Module ...</a></li>
        </ul>
    </div>
    <a id="cp_site_menu_trigger" class="cp_menu ui-widget ui-state-default ui-corner-all" href="#cp_site_menu">
        <span class="cp_icon_right ui-icon ui-icon-triangle-1-s"></span>Site
    </a>
    <div id="cp_site_menu" class="hidden">
        <ul>
            <li><a href="#">Site Settings ...</a></li>
        </ul>
    </div>
    <a id="cp_app_menu_trigger" class="cp_menu ui-widget ui-state-default ui-corner-all" href="#cp_app_menu">
        <span class="cp_icon_right ui-icon ui-icon-triangle-1-s"></span>Application
    </a>
    <div id="cp_app_menu" class="hidden">
        <ul>
            <li><a href="#">New Website ...</a></li>
            <li><a href="#">Delete this Website</a></li>
            <li><a href="#">Application Settings ...</a></li>
        </ul>
    </div>
    
    <div id="cp_page_addmodule_dlg" title="Add a Module">
        <% Html.RenderPartial("ModuleApplicationList", Model.ModuleApplications);%>
    </div>
</div>
