using System.Web.Mvc;

namespace Project_63130260.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index",Controller = "Home_63130260", id = UrlParameter.Optional, AreaName = "Admin" },
				new[] { "Project_63130260.Areas.Admin.Controllers" }
			);
        }
    }
}