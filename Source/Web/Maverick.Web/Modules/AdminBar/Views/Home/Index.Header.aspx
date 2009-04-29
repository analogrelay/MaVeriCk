<%@ Page Language="C#" Inherits="Maverick.Web.ModuleFramework.ModuleViewPage" %>
<%= Html.Stylesheet("~/Modules/AdminBar/Content/Styles.css") %>
<%= Html.Script("~/Modules/AdminBar/Scripts/AdminBar.Main.js") %>
<script type="text/javascript">
    $(document).ready(Maverick.AdminBar.Main);
</script>