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
        public ActionResult Calculate(int id)
        {
            //int cost = 400;
            
        var result = db.bills.Where(a => a.id == id).ToArray();
            var chto_to_tam = from a in db.contracts
                              where a.id_client == id
                              select a.id_tariff;
            string chto_to_tam2 = chto_to_tam.ToString();
            
            
            var e = from a in db.tariffs
                    where a.id == Convert.ToInt32(chto_to_tam2)
                    select new 
                    {
                        a
                    };
            var list = new List<Tariff>();
            foreach(var c in e)
            {
                list.Add(new Tariff()
                {
                    day_price = c.a.day_price,
                    night_price = c.a.night_price,
                });
            }
           // var ee = e.Select(s => new { s.day_price, s.night_price}).ToList();
            var used = from a in db.collectors
                       where a.id_client == id
                       select new 
                       {
                          a
                       };
            foreach (var c in used)
            {
                list.Add(new Tariff()
                {
                    day_used = c.a.day_used,
                    night_used = c.a.night_used,
                });
            }
            //     var usedlist = used.Select(s => new { s.day_used, s.night_used }).ToList();
            //decimal day_price = Convert.ToDecimal(ee[0]);
            //decimal night_price = Convert.ToDecimal(ee[1]);
            //decimal day_used = Convert.ToDecimal(usedlist[0]);
            //decimal night_used = Convert.ToDecimal(usedlist[1]);
            //  decimal payment = day_price *day_used + night_price *night_used;
            decimal payment = list[0].day_price * list[1].day_used+ list[0].night_price * list[1].night_used;
            return View(payment);
        
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
