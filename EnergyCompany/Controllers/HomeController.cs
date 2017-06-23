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

        public ActionResult Query1(String query1text)  //вывести с определенным тарифом
        {
            var result = db.contracts.Include(c => c.client).Include(c => c.tariff).Where(c => c.tariff.name.ToLower().Contains(query1text.ToLower())).ToArray();
            return View(result);
        }

        public ActionResult Query2(DateTime query1time, DateTime query2time)  //вывести с определенной датой заключения
        {
            var result = db.contracts.Where(c => c.date_start >= query1time && c.date_end <= query2time).ToArray();
            return View(result);
        }

        public ActionResult Query3(decimal query3decimal) //вывести с определенным размером счета
        {
            var result = db.bills.Where(c => c.payment >= query3decimal).ToArray();
            return View(result);
        }

        public ActionResult Query4(decimal query4decimal)  //вывести с определенным значением на дневном счетчике
        {
            var result = db.collectors.Include(c => c.client).Where(c => c.day_used >= query4decimal).ToArray();
            return View(result);
        }

        public ActionResult Query5(decimal query5decimal)  //вывести с определенным значением на ночном счетчике
        {
            var result = db.collectors.Include(c => c.client).Where(c => c.night_used >= query5decimal).ToArray();
            return View(result);
        }

        public ActionResult Query6(String query6text)  //вывести клиентов с таким-то адресом и их тариф
        {
            var result = db.contracts.Include(c => c.client).Where(c => c.client.adress.ToLower().Contains(query6text.ToLower())).ToArray(); 
            return View(result);
        }

    }
}