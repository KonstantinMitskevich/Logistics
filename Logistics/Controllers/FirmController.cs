using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Logistics.Models;

namespace Logistics.Controllers
{
    [Authorize(Roles="Фирма")]
    public class FirmController : Controller
    {
        LogistContext db = new LogistContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddTarif(int id = 0)
        {
            User user = db.Users.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                Firm firm = db.Firms.Where(f => f.UserId == user.Id).Include(f => f.Tarifs).FirstOrDefault();
                Tarif tarif = db.Tarifs.Find(id);
                if (firm != null && firm.Tarifs.Contains(tarif))
                {
                    return View(tarif);
                }
                else
                {
                    return View();
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddTarif(Tarif tarif)
        {
            User user = db.Users.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                Firm firm = db.Firms.Where(f => f.UserId == user.Id).Include(f => f.Tarifs).FirstOrDefault();
                Tarif nTarif = db.Tarifs.Find(tarif.Id); // проверка, если уже содержится в базе
                if (tarif != null && !firm.Tarifs.Contains(nTarif)) // да
                {
                    tarif.FirmId = firm.Id;
                    db.Tarifs.Add(tarif);
                    db.SaveChanges();
                    return RedirectToAction("ShowTarifs");
                }
                if (ModelState.IsValid)
                {
                    if (tarif != null && firm.Tarifs.Contains(nTarif)) // нет
                    {
                        nTarif.Price = tarif.Price;
                        nTarif.Destination = tarif.Destination;
                        db.Entry(nTarif).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("ShowTarifs");
                    }
                }
               else
                {
                    return View("AddTarif");
                }
                return View(tarif);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        [HttpGet]
        public ActionResult ShowTarifs()
        {
            User user = db.Users.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                Firm firm = db.Firms.Where(f => f.UserId == user.Id).Include(f => f.Tarifs).FirstOrDefault();
                return PartialView("_Tarrifs", firm);
            }
            else
            {
                return RedirectToAction("LogOff", "Account");
            }
        }

        public ActionResult DeleteTarifs(int id)
        {
            User user = db.Users.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                Firm firm = db.Firms.Where(f => f.UserId == user.Id).FirstOrDefault();
                Tarif tarif = db.Tarifs.Find(id);
                if (tarif.FirmId == firm.Id)
                {
                    db.Tarifs.Remove(tarif);
                    db.SaveChanges();
                    return RedirectToAction("ShowTarifs");
                }
            }
            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult ShowAllRegisteredClients()
        {
            User user = db.Users.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            Firm firm = db.Firms.Where(f => f.UserId == user.Id).Include(f => f.Clients).FirstOrDefault();
            return View(firm);
        }

        public ActionResult ShowAllOrders()
        {
            User user = db.Users.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            Firm firm = db.Firms.Where(f => f.UserId == user.Id).Include(f => f.Tarifs).Include(f => f.Clients).FirstOrDefault();
            ViewBag.clients = db.Clients.Include(c => c.Orders);
            ViewBag.tarifs = db.Tarifs;
            return View(firm);
        }

        [HttpPost]
        public ActionResult ShowAllOrders(int orderId, int status)
        {
            User user = db.Users.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("LogOff","Account");
            }

            Order order = db.Orders.Find(orderId);
            if (order != null)
            {
                order.Status = status;
            }
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ShowAllOrders");
        }
    }
}