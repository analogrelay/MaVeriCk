/// <reference name="MicrosoftAjax.js" />
/// <reference src="~/Scripts/jquery-1.3.2-vsdoc.js" />

Type.registerNamespace("Maverick.AdminBar");

Maverick.AdminBar.Main = function() {
    $('#cp_page_menu_trigger').menu({
        content: $('#cp_page_menu').html(),
        crumbDefaultText: ' ',
        chooseItem: function(item) {
        Maverick.AdminBar.MenuHandlers[item.id]();
    }
    });
    $('#cp_site_menu_trigger').menu({
        content: $('#cp_site_menu').html(),
        crumbDefaultText: ' ',
        chooseItem: function(item) {
        Maverick.AdminBar.MenuHandlers[item.id]();
    }
    });
    $('#cp_app_menu_trigger').menu({
        content: $('#cp_app_menu').html(),
        crumbDefaultText: ' ',
        chooseItem: function(item) {
            Maverick.AdminBar.MenuHandlers[item.id]();
        }
    });
    $('#cp_page_addmodule_dlg').dialog({
        autoOpen: false,
        modal: true,
        height: 400,
        width: 600,
        buttons: {
            "Cancel": function() { $(this).dialog('close'); }
        }
    });
    $('.cp_page_addpage_dlg').dialog({
        autoOpen: false,
        modal: true,
        height: 400,
        width: 600,
        buttons: {
            "Add": function() { $(this).find('.cp_dlg_form').submit(); /* TODO: AJAX */ },
            "Cancel": function() { $(this).dialog('close'); }
        }
    });
}

Maverick.AdminBar.MenuHandlers = {
    cp_page_addmodule: function() {
        $('#cp_page_addmodule_dlg').dialog('open');
    },
    cp_page_addpage: function() {
        $('#cp_page_addpage_dlg').dialog('open');
    },
    cp_page_addchildpage: function() {
        $('#cp_page_addchildpage_dlg').dialog('open');
    }
}


Maverick.AdminBar.AddModuleDialog_Add = function() {
    alert('add clicked');
}