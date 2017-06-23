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
    public class collectorsController : Controller
    {
        private EnergyDBEntities3 db = new EnergyDBEntities3();

        // GET: collectors
        public ActionResult Index()
        {
            ViewBag.stick = " | ";
            var collectors = db.collectors.Include(c => c.client);
            return View(collectors.ToList());
        }

        // GET: collectors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            collector collector = db.collectors.Find(id);
            if (collector == null)
            {
                return HttpNotFound();
            }
            return View(collector);
        }

        // GET: collectors/Create
        public ActionResult Create()
        {
            ViewBag.id_client = new SelectList(db.clients, "id", "name");
            return View();
        }

        // POST: collectors/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_client,day_used,night_used")] collector collector)
        {
            if (ModelState.IsValid)
            {
                db.collectors.Add(collector);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_client = new SelectList(db.clients, "id", "name", collector.id_client);
            return View(collector);
        }

        // GET: collectors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            collector collector = db.collectors.Find(id);
            if (collector == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_client = new SelectList(db.clients, "id", "name", collector.id_client);
            return View(collector);
        }

        // POST: collectors/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_client,day_used,night_used")] collector collector)
        {
            if (ModelState.IsValid)
            {
                db.Entry(collector).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_client = new SelectList(db.clients, "id", "name", collector.id_client);
            return View(collector);
        }

        // GET: collectors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            collector collector = db.collectors.Find(id);
            if (collector == null)
            {
                return HttpNotFound();
            }
            return View(collector);
        }

        // POST: collectors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            collector collector = db.collectors.Find(id);
            db.collectors.Remove(collector);
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
