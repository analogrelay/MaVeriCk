<%@ Page Language="C#" Inherits="Maverick.Web.ModuleFramework.ModuleViewPage<IClaimsIdentity>" %>
<h4>Name: <%= Model.Name %></h4>
<h4>Claims:</h4>
<table>
    <thead>
        <tr>
            <td>Claim Type</td>
            <td>Value</td>
            <td>Issuer</td>
            <td>Is Role Claim?</td>
            <td>IsInRole?</td>
        </tr>
    </thead>
    <tbody>
        <% foreach(Claim c in Model.Claims) { %>
        <tr>
            <td><%= c.ClaimType %></td>
            <td><%= c.Value %></td>
            <td><%= c.Issuer %></td>
            <td>
                <% if (Model.RoleClaimTypes.Contains(c.ClaimType)) {%>
                Yes
                <% } else { %>
                No
                <% } %>
            </td>
            <td>
                <% if (User.IsInRole(c.Value)) {%>
                Yes
                <% } else { %>
                No
                <% } %>
            </td>
        </tr>
        <% } %>
    </tbody>
</table>