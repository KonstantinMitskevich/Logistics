using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logistics.Models;
using System.Web.Security;

namespace Logistics.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using(LogistContext db = new LogistContext())
                {
                    user = db.Users.Where(u => u.Login == model.Login && u.Password == model.Password).FirstOrDefault();
                }
                if(user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Login,true);
                    if(user.RoleId == 1)
                    {
                        return RedirectToAction("Index", "Firm");
                    }
                    else if (user.RoleId == 2)
                    {
                        return RedirectToAction("Index", "Client"); 
                    }
                    else if (user.RoleId == 3)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("","Пользователя с таким логином и паролем нет");
            }
            return View(model);
        }


        [HttpGet]
        public ActionResult RegisterClient()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterClient(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (LogistContext db = new LogistContext())
                {
                    user = (from u in db.Users
                            where u.Login == model.Login && u.Password == model.Password
                            select u).FirstOrDefault();
                }
                if (user == null)
                {
                    using (LogistContext db = new LogistContext())
                    {
                        db.Users.Add(new User { Login = model.Login, Password = model.Password, RoleId = 2 });
                        db.Clients.Add(new Client { Name = model.Name, User = user, Orders = null });
                        db.SaveChanges();

                        user = db.Users.Where(u => u.Login == model.Login && u.Password == model.Password).FirstOrDefault();
                    }
                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("Index", "Client");
                }
                else
                {
                    return RedirectToAction("Index", "Firm");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult RegisterFirm()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterFirm(RegisterModelFirm model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (LogistContext db = new LogistContext())
                {
                    user = (from u in db.Users
                            where u.Login == model.Login && u.Password == model.Password
                            select u).FirstOrDefault();
                }
                if (user == null)
                {
                    using (LogistContext db = new LogistContext())
                    {
                        db.Users.Add(new User { Login = model.Login, Password = model.Password, RoleId = 1 });
                        db.Firms.Add(new Firm { Name = model.Name, User = user, Tarifs = null, Clients = null });
                        db.SaveChanges();

                        user = db.Users.Where(u => u.Login == model.Login && u.Password == model.Password).FirstOrDefault();
                    }
                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("Index", "Firm");
                }
                else
                {
                    return RedirectToAction("Index", "Firm");
                }
            }
            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}