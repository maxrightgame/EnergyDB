using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnergyCompany.Models;
using System.Linq;
namespace EnergyCompany.Controllers
{
    public class billsController : Controller
    {
        private EnergyDBEntities3 db = new EnergyDBEntities3();

        // GET: bills
        public ActionResult Index()
        {
            var bills = db.bills.Include(b => b.client).Include(b => b.collector);
            return View(bills.ToList());
        }
        class Tariff
        {
            public decimal day_price { get; set; }
            public decimal night_price { get; set; }
            public decimal day_used { get; set; }
            public decimal night_used { get; set; }
        }
        public ActionResult Calculate(int id_client, int id)
        {
            //int cost = 400;
            var temp = db.contracts.Include(c => c.client).Include(c => c.tariff).Include(c => c.client.collectors).Where(c => c.client.id == id_client).ToArray();
            var contract = temp[0];
            var collectors = contract.client.collectors.ToArray();
            for (int i = 0; i < collectors.Length; i++)
            {
                var collector = collectors[i];

                var newPayment = new bill()
                {
                    id_client = contract.id_client,
                    id_collector = contract.client.collectors.ToArray()[i].id,
                    payment = collector.day_used * contract.tariff.day_price + collector.night_used * contract.tariff.night_price
                };
                db.bills.Add(newPayment);
            }
            db.SaveChanges();
            return View(0);
        
    }

        // GET: bills/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bill bill = db.bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // GET: bills/Create
        public ActionResult Create()
        {
            ViewBag.id_client = new SelectList(db.clients, "id", "name");
            ViewBag.id_collector = new SelectList(db.collectors, "id", "id");
            return View();
        }

        // POST: bills/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_client,id,payment,id_collector")] bill bill)
        {
            if (ModelState.IsValid)
            {
                db.bills.Add(bill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_client = new SelectList(db.clients, "id", "name", bill.id_client);
            ViewBag.id_collector = new SelectList(db.collectors, "id", "id", bill.id_collector);
            return View(bill);
        }

        // GET: bills/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bill bill = db.bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_client = new SelectList(db.clients, "id", "name", bill.id_client);
            ViewBag.id_collector = new SelectList(db.collectors, "id", "id", bill.id_collector);
            return View(bill);
        }

        // POST: bills/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_client,id,payment,id_collector")] bill bill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_client = new SelectList(db.clients, "id", "name", bill.id_client);
            ViewBag.id_collector = new SelectList(db.collectors, "id", "id", bill.id_collector);
            return View(bill);
        }

        // GET: bills/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bill bill = db.bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // POST: bills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bill bill = db.bills.Find(id);
            db.bills.Remove(bill);
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
