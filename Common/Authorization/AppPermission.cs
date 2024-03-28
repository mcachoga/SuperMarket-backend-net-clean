using System.Collections.ObjectModel;

namespace SuperMarket.Common.Authorization
{
    public record AppPermission(string Feature, string Action, string Group, string Description, bool IsBasic = false)
    {
        public string Name => NameFor(Feature, Action);

        public static string NameFor(string feature, string action)
        {
            return $"Permissions.{feature}.{action}";
        }
    }

    public class AppPermissions
    {
        private static readonly AppPermission[] _all = new AppPermission[]
        {
            new(AppFeature.Users, AppAction.Create, AppRoleGroup.SystemAccess, "Create Users"),
            new(AppFeature.Users, AppAction.Update, AppRoleGroup.SystemAccess, "Update Users"),
            new(AppFeature.Users, AppAction.Read, AppRoleGroup.SystemAccess, "Read Users"),
            new(AppFeature.Users, AppAction.Delete, AppRoleGroup.SystemAccess, "Delete Users"),

            new(AppFeature.UserRoles, AppAction.Read, AppRoleGroup.SystemAccess, "Read User Roles"),
            new(AppFeature.UserRoles, AppAction.Update, AppRoleGroup.SystemAccess, "Update User Roles"),

            new(AppFeature.Roles, AppAction.Read, AppRoleGroup.SystemAccess, "Read Roles"),
            new(AppFeature.Roles, AppAction.Create, AppRoleGroup.SystemAccess, "Create Roles"),
            new(AppFeature.Roles, AppAction.Update, AppRoleGroup.SystemAccess, "Update Roles"),
            new(AppFeature.Roles, AppAction.Delete, AppRoleGroup.SystemAccess, "Delete Roles"),

            new(AppFeature.RoleClaims, AppAction.Read, AppRoleGroup.SystemAccess, "Read Role Claims/Permissions"),
            new(AppFeature.RoleClaims, AppAction.Update, AppRoleGroup.SystemAccess, "Update Role Claims/Permissions"),

            new(AppFeature.Markets, AppAction.Read, AppRoleGroup.ManagementHierarchy, "Read Markets", IsBasic: true),
            new(AppFeature.Markets, AppAction.Create, AppRoleGroup.ManagementHierarchy, "Create Markets"),
            new(AppFeature.Markets, AppAction.Update, AppRoleGroup.ManagementHierarchy, "Update Markets"),
            new(AppFeature.Markets, AppAction.Delete, AppRoleGroup.ManagementHierarchy, "Delete Markets"),

            new(AppFeature.Products, AppAction.Read, AppRoleGroup.ManagementHierarchy, "Read Products", IsBasic: true),
            new(AppFeature.Products, AppAction.Create, AppRoleGroup.ManagementHierarchy, "Create Products"),
            new(AppFeature.Products, AppAction.Update, AppRoleGroup.ManagementHierarchy, "Update Products"),
            new(AppFeature.Products, AppAction.Delete, AppRoleGroup.ManagementHierarchy, "Delete Products"),

            new(AppFeature.Orders, AppAction.Read, AppRoleGroup.ManagementHierarchy, "Read Orders", IsBasic: true),
            new(AppFeature.Orders, AppAction.Create, AppRoleGroup.ManagementHierarchy, "Create Orders"),
            new(AppFeature.Orders, AppAction.Update, AppRoleGroup.ManagementHierarchy, "Update Orders"),
            new(AppFeature.Orders, AppAction.Delete, AppRoleGroup.ManagementHierarchy, "Delete Orders")
        };

        public static IReadOnlyList<AppPermission> AdminPermissions { get; } = 
            new ReadOnlyCollection<AppPermission>(_all.Where(p => !p.IsBasic).ToArray());

        public static IReadOnlyList<AppPermission> BasicPermissions { get; } =
            new ReadOnlyCollection<AppPermission>(_all.Where(p => p.IsBasic).ToArray());

        public static IReadOnlyList<AppPermission> AllPermissions { get; } =
            new ReadOnlyCollection<AppPermission>(_all);
    }
}