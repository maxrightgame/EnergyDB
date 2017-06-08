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
    public class HomeController : Controller
    {
        private EnergyDBEntities3 db = new EnergyDBEntities3();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SearchComplex(string clientname, string tariffname, DateTime date_start, DateTime date_end)
        {
            var result = db.contracts.Where(a => a.date_start >= date_start && a.date_end <= date_end).ToArray();

            if (clientname.Trim() != "")
            {
                result = result.Where((a => a.client.name.ToString().ToLower().Contains(clientname.ToLower().Trim()))).ToArray();
            }
            if (tariffname.Trim() != "")
            {
                result = result.Where((a => a.tariff.name.ToString().ToLower().Contains(tariffname.ToLower().Trim()))).ToArray();
            }

            return View();
        }
    }
}