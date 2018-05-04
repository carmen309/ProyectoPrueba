
using IdentitySample.Models;
using Opositae.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackEndGestion.Controllers
{

    public class ImagnesController : Controller
    {
        //esta es mi clase para generar errores
        private ApplicationDbContext db = new ApplicationDbContext();
        public EmptyResult ExecutionError(string message, int codigo)
        {
            Response.StatusCode = codigo;
            Response.Write(message);
            Response.StatusDescription = message;
            return new EmptyResult();
        }



        // GET: Imagnes
        public ActionResult Index()
        {
            return View();
        }

        // GET: Imagnes/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Imagnes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Imagnes/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Imagnes/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Imagnes/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Imagnes/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Imagnes/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
