using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bungWeb.Controllers.PostEdit
{
    public class EditController : Controller
    {
        [HttpGet]
        public ActionResult EditPost()
        {
            ViewData["logged"] = false;

            if (Session["user"] != null)
            {
                ViewData["logged"] = true;
            }

            return View();
        }

        [HttpGet]
        public ActionResult RemovePost() {
            ViewData["logged"] = false;

            if (Session["user"] != null)
            {
                ViewData["logged"] = true;
            }

            return View();
        }
    }
}
