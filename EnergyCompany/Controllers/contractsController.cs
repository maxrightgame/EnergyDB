using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnergyCompany.Models;

namespace EnergyCompany.Controllers
{
    public class contractsController : Controller
    {
        private EnergyDBEntities3 db = new EnergyDBEntities3();

        // GET: contracts
        public ActionResult Index()
        {
            var contracts = db.contracts.Include(c => c.client).Include(c => c.tariff);
            return View(contracts.ToList());
        }

        public ActionResult Search(String searchText)
        {
            var result = db.clients.Where(a => a.name.ToLower().Contains(searchText.ToLower()));
            return View(result);
        }

        // GET: contracts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contract contract = db.contracts.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // GET: contracts/Create
        public ActionResult Create()
        {
            ViewBag.id_client = new SelectList(db.clients, "id", "name");
            ViewBag.id_tariff = new SelectList(db.tariffs, "id", "name");
            return View();
        }

        // POST: contracts/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_client,id_tariff,date_start,date_end,id")] contract contract)
        {
            if (ModelState.IsValid)
            {
                db.contracts.Add(contract);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_client = new SelectList(db.clients, "id", "name", contract.id_client);
            ViewBag.id_tariff = new SelectList(db.tariffs, "id", "name", contract.id_tariff);
            return View(contract);
        }

        // GET: contracts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contract contract = db.contracts.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_client = new SelectList(db.clients, "id", "name", contract.id_client);
            ViewBag.id_tariff = new SelectList(db.tariffs, "id", "name", contract.id_tariff);
            return View(contract);
        }

        // POST: contracts/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_client,id_tariff,date_start,date_end,id")] contract contract)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contract).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_client = new SelectList(db.clients, "id", "name", contract.id_client);
            ViewBag.id_tariff = new SelectList(db.tariffs, "id", "name", contract.id_tariff);
            return View(contract);
        }

        // GET: contracts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contract contract = db.contracts.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // POST: contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            contract contract = db.contracts.Find(id);
            db.contracts.Remove(contract);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
