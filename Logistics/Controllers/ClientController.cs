using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logistics.Models;
using System.Web.Security;
using System.Data.Entity;
using Newtonsoft.Json;

namespace Logistics.Controllers
{
    
    [Authorize(Roles="Клиент")]
    public class ClientController : Controller
    {
        private LogistContext db = new LogistContext();
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowAllOrders()
        {
            User user = db.Users.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                Client client = db.Clients.Where(c => c.UserId == user.Id).FirstOrDefault();
                var orders = db.Orders.Where(o => o.ClientId == client.Id).Include(o => o.Tarif);
                return View(orders);
            }
            return RedirectToAction("LogOff", "Account");
        }

        [HttpGet]
        public ActionResult PlaceOrder(int? id)
        {
            User user = db.Users.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                Client client = db.Clients.Where(c => c.UserId == user.Id).FirstOrDefault();
                List<Firm> firms = db.Firms.Include(f => f.Tarifs).ToList();
                ViewBag.Firms = new SelectList(firms, "Id", "Name");
                Order order = new Order();
                order.ClientId = client.Id;
                ViewBag.Tarifs = new SelectList(db.Tarifs.Where(t => t.FirmId == id), "Id", "Destination");
                Firm firm = db.Firms.Find(id);
                if(firm != null)
                     ViewBag.FirmId = firm.Id;
                return View(order);
            }
            return RedirectToAction("LogOff", "Account");
        }

        [HttpPost]
        public ActionResult PlaceOrder(Order order, int? FirmId)
        {
            User user = db.Users.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (order.Date < DateTime.Now)
            {
                ModelState.AddModelError("Date", "Некорректная дата"); // не выводит ошибку
            }
            if (ModelState.IsValid)
            {
                Firm firm = db.Firms.Where(f => f.Id == FirmId).Include(f => f.Clients).FirstOrDefault();
                Client client = db.Clients.Where(c => c.UserId == user.Id).Include(f => f.Firms).FirstOrDefault();
                client.Orders.Add(order);
                if (!client.Firms.Contains(firm))
                {
                    client.Firms.Add(firm);
                }
                if (!firm.Clients.Contains(client))
                {
                    firm.Clients.Add(client);
                }
                db.Orders.Add(order);
                db.SaveChanges();
                return View("Completed", firm);
            }
            return RedirectToAction("PlaceOrder");
        }
 
        public ActionResult DeleteOrder(int id)
        {
            User user = db.Users.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if(user != null)
            {
                Order order = db.Orders.Find(id);
                db.Orders.Remove(order);
                db.SaveChanges();
                return RedirectToAction("ShowAllOrders");
            }
            return RedirectToAction("ShowAllOrders");
        }

        [HttpGet]
        public ActionResult ShowAllTarifs(int? id, string destination = null)
        {
            ViewBag.Firms = new SelectList(db.Firms.Include(f => f.Tarifs),"Id","Name");
            ViewBag.Firm = db.Firms.Where(f => f.Id == id).Include(f => f.Tarifs).FirstOrDefault();
            ViewBag.tarifs = null;
            if (destination != null)
            {
                IEnumerable<Tarif> tarifs = db.Tarifs.Where(t => t.Destination == destination.Trim()).ToList();
                ViewBag.tarifs = tarifs;
            }
            return View();
        }

        public ActionResult ViewTarrifs()
        {
            
            Firm firm = db.Firms.Include(f => f.Tarifs).FirstOrDefault();
            ViewBag.tarif = null;
            return PartialView("_Tarrifs", firm);
        }

        [HttpPost]
        public ActionResult ViewTarrifs(string destination)
        {
            Firm firm = db.Firms.Include(f => f.Tarifs).FirstOrDefault();
            IEnumerable<Tarif> tarif = db.Tarifs.Where(t => t.FirmId == db.Firms.FirstOrDefault().Id).Where(t => t.Destination == destination);
            ViewBag.tarifs = tarif;
            return PartialView("_Tarrifs", firm);
        }
    }
}