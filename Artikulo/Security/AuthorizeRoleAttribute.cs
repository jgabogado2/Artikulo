using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Artikulo.Models.DB;
using Artikulo.Models.EntityManager;
using Microsoft.AspNetCore.Mvc.Filters;


namespace Artikulo.Security
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string[] userAssignedRoles;

        public AuthorizeRolesAttribute(params string[] roles)
        {
            this.userAssignedRoles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool authorize = false;

            using (MyDBContext db = new MyDBContext())
            {
                UserManager um = new UserManager();
                foreach (var role in userAssignedRoles)
                {
                    authorize = um.IsUserInRole(context.HttpContext.User.Identity.Name, role);
                    if (authorize)
                        return;
                }
            }

            context.Result = new RedirectResult("~/Home/Unauthorized"); // Need to create a separate page
        }
    }
}
