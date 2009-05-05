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
            <li><a id="cp_page_addpage" href="#">New Page ...</a></li>
            <li><a id="cp_page_addchildpage" href="#">New Child Page ...</a></li>
            <li><a id="cp_page_deletepage" href="#">Delete this Page</a></li>
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
        <%-- TODO: Ajax Render --%>
        <% Html.RenderPartial("ModuleApplicationList", Model.ModuleApplications);%>
    </div>
    
    <div id="cp_page_addpage_dlg" class="cp_page_addpage_dlg" title="Create a Page">
        <%-- TODO: Ajax Render --%>
        <%-- TODO: Find a better way to pass "null"... The MVC helpers keep replacing it with this view's Model --%>
        <% Html.RenderPartial("AddPageDialog", new Maverick.Models.Page() { Id = -1 }); %>
    </div>
    
    <div id="cp_page_addchildpage_dlg" class="cp_page_addpage_dlg" title="Create a Child Page of this Page">
        <%-- TODO: Ajax Render --%>
        <% Html.RenderPartial("AddPageDialog", Model.ActivePage);%>
    </div>
</div>
