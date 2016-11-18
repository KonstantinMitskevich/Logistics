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
        public ActionResult PlaceOrder()
        {
            User user = db.Users.Where(u => u.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                Client client = db.Clients.Where(c => c.UserId == user.Id).FirstOrDefault();
                int selectedIndex = 1;
                SelectList firms = new SelectList(db.Firms, "Id", "Name", selectedIndex);
                ViewBag.Firms = firms;
                SelectList tarifs = new SelectList(db.Tarifs.Where(t => t.FirmId == selectedIndex), "Id", "Destination",selectedIndex);
                ViewBag.Tarifs = tarifs;
                ViewBag.FirmId = 1;
                Order order = new Order();
                order.ClientId = client.Id;
                return View(order);
            }
            return RedirectToAction("LogOff", "Account");
        }

        public ActionResult GetItems(int id)
        {
            ViewBag.FirmId = id;
            return PartialView(db.Tarifs.Where(c => c.FirmId == id).ToList());
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
                order.Status = 1;
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


        public JsonResult ShowAlllTar(int id = 1)
        {
            var jsondata = db.Tarifs.Where(t => t.FirmId == id);
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ShowAllTarifs(int? id, string destination = null)
        {
            ViewBag.Firms = new SelectList(db.Firms.Include(f => f.Tarifs), "Id", "Name");
            ViewBag.Id = 2;
            return View();
        }

        public ActionResult TarifsSearch(int id, int page = 1)
        {

            int pageSize = 5;
            ViewBag.tarifs = db.Tarifs.Where(t => t.FirmId == id);
            List<Tarif> tarifs = db.Tarifs.Where(t => t.FirmId == id).ToList();
            IEnumerable<Tarif> tarifPerPages = tarifs.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = tarifs.Count };
            IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, Tarifs = tarifPerPages };
            if (Request.IsAjaxRequest())
            {
                return PartialView("TarifsSearch", ivm);
            }
            return PartialView(ivm);
        }

    
        public ActionResult TarifsSearchByDestination(string destination = null)
        {
            ViewBag.tarifs = db.Tarifs.Where(t => t.Destination == destination.Trim()).ToList();
            return PartialView("TarifsSearchByDestination", ViewBag.tarifs);
        }

       
    }
}

