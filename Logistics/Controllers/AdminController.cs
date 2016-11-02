using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logistics.Models;
using System.Web.Security;
using System.Data.Entity;

namespace Logistics.Controllers
{
    [Authorize(Roles="Администратор")]
    public class AdminController : Controller
    {
        LogistContext db = new LogistContext();
        
        public ActionResult Index()
        {
           
            return View();
        }

        [HttpGet]
        public ActionResult GetAllFirms()
        {
            IEnumerable<Firm> firms = db.Firms;
            return View(firms);
        }

        [HttpGet]
        public ActionResult GetAllClients()
        {
            IEnumerable<Client> clients = db.Clients;
            return View(clients);
        }

        public ActionResult DeleteFirm(int id)
        {
            Firm firm = db.Firms.Find(id);
            db.Firms.Remove(firm);
            db.SaveChanges();
            return View();
        }

        public ActionResult DeleteClient(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
            db.SaveChanges();
            return View();
        }

        [HttpGet]
        public ActionResult EditFirm(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            if(id != null)
            {
                Firm firm = db.Firms.Find(id);
                return View(firm);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditF(Firm firm)
        {
            db.Entry(firm).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("GetAllFirms");
        }

        [HttpGet]
        public ActionResult EditClient(int? id)
        {
            if(id == null){
                return HttpNotFound();
             }
            if (id != null)
            {
                Client client = db.Clients.Find(id);
                return View(client);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditC(Client client)
        {
            db.Entry(client).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("GetAllClients");
        }
    }
}