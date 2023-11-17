using DATA_63130260;
using QLBH_AnkerShop.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_63130260.Areas.Admin.Controllers
{

	public class Home_63130260Controller : Controller
    {
	    int currentMonth = Convert.ToInt32(DateTime.Now.Month);
	    int currentYear = Convert.ToInt32(DateTime.Now.Year);
        // GET: Admin/Home_63130260

        [RoleAdmin]
        public ActionResult Index(int month = 0)
		{

            if(month == 0) { 
                ViewBag.month = currentMonth;
			}
            else
            {
                ViewBag.month = month;
			}
			ViewBag.year = currentYear;

          
			var map = new mapUser();
            var list = map.GetListUser();
            return View(list);
        }
    }
}