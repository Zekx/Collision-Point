using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using bungWeb.Models;

namespace bungWeb.Controllers
{
    public class BlogController : Controller
    {
        [HttpGet]
        public ActionResult Home() {
            ViewData["logged"] = false;
            ViewData["page"] = 1;

            List<Post> posts = SqlController.retrievePost("admin");
            posts = posts.OrderByDescending(e => e.rawDate.Date).ThenByDescending(e => e.rawDate.TimeOfDay).ToList<Post>();
            ViewData["posts"] = posts;

            if (Session["user"] != null)
            {
                ViewData["logged"] = true;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Home(int val) {
            ViewData["logged"] = false;

            List<Post> posts = SqlController.retrievePost("admin");
            posts = posts.OrderByDescending(e => e.rawDate.Date).ThenByDescending(e => e.rawDate.TimeOfDay).ToList<Post>();
            ViewData["posts"] = posts;

            if (val <= 0)
            {
                ViewData["page"] = 1;
            }
            else
            {
                if (val > (posts.Count()/10)+1) {
                    ViewData["page"] = (posts.Count / 10) + 1;
                }
                else
                {
                    ViewData["page"] = val;
                }
            }

            if (Session["user"] != null)
            {
                ViewData["logged"] = true;
            }

            return View();
        }

        public ActionResult Resume() {
            ViewData["logged"] = false;

            if (Session["user"] != null)
            {
                ViewData["logged"] = true;
            }

            return View();
        }

        [HttpGet]
        public ActionResult CreatePost() {
            ViewData["logged"] = false;

            if (Session["user"] != null)
            {
                ViewData["logged"] = true;
            }
            else {
                return Redirect("/Blog/Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreatePost(string topic, string body)
        {
            ViewData["logged"] = false;

            if (Session["user"] != null)
            {
                ViewData["logged"] = true;
            }
            else
            {
                return Redirect("/Blog/Home");
            }

            if (string.IsNullOrEmpty(topic) || string.IsNullOrEmpty(body)){

                return Json(new { success = false, error = "The form must be fully filled!" }, JsonRequestBehavior.AllowGet);
            }
            else {
                if (SqlController.createPost((string)Session["user"], topic, body))
                {
                    return Json(new { success = true, message = "The post was successfully posted!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, error = "There was a problem updating the post!" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        public ActionResult Login() {
            ViewData["logged"] = false;

            if (Session["user"] != null) {
                ViewData["logged"] = true;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(string val){
            string username = "";
            string password = "";

            if (!string.IsNullOrEmpty(Request.Form["username"])) {
                username = Request.Form["username"];
            }
            if (!string.IsNullOrEmpty(Request.Form["password"]))
            {
                password = Request.Form["password"];
            }

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewData["logged"] = false;
                ViewData["error"] = "Login Credentials must be fully filled.";
                return View();
                
            }
            else {
                User usr = SqlController.retrieveLogin(username, password);

                if (usr != null) {
                    Session["user"] = usr.username;
                    ViewData["logged"] = true;
                    return Redirect("/Blog/Home");
                }
                else {
                    ViewData["error"] = "Username or password is incorrect.";
                    ViewData["logged"] = false;
                    return View();
                }
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session["user"] = null;

            return Redirect("/Blog/Login");
        }
    }
}
