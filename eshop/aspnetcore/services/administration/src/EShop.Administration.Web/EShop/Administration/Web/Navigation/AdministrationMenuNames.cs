using EShop.Shared;

namespace EShop.Administration.Web.Navigation
{
    public class AdministrationMenuNames
    {
        /// <summary>
        /// 管理
        /// </summary>
        public class Administration
        {
            public const string GroupName = ModuleNames.Administration;
            /// <summary>
            /// 身份标识管理
            /// </summary>
            public class Identity
            {
                public const string GroupName = ModuleNames.Identity;

                public const string Roles = GroupName + ".Roles";
                public const string Users = GroupName + ".Users";
                public const string OrganizationUnits = GroupName + ".OrganizationUnits";
                public const string ClaimTypes = GroupName + ".ClaimTypes";
                public const string SecurityLog = GroupName + ".SecurityLog";
            }
        }        

        ///// <summary>
        ///// Saas
        ///// </summary>
        //public class Saas
        //{
        //    public const string GroupName = ModuleNames.Saas;

        //    public const string Tenants = GroupName + ".Tenants";
        //    public const string Editions = GroupName + ".Editions";
        //}

        ///// <summary>
        ///// 设置
        ///// </summary>
        //public class Settings
        //{
        //    public const string GroupName = "Settings";
        //}
    }
}
