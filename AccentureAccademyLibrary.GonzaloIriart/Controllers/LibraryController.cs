using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccentureAccademyLibrary.GonzaloIriart.Models;

namespace AccentureAccademyLibrary.GonzaloIriart.Controllers
{
    public class LibraryController : Controller
    {
        private AccentureAccademyLibraryEntities db;
        public LibraryController()
        {
            this.db = new AccentureAccademyLibraryEntities();
        }
        // GET: Library
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult AddBook()
        {
            return View();
        }
    }
}