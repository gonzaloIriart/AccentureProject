using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public ActionResult JsonListBooks()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Book> books = this.db.Book.ToList();

            return Json(books, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(Book book, IEnumerable<int> autores)
        {
            foreach (int autorActual in autores)
            {
                Author autor = this.db.Author.Find(autorActual);
                book.Author.Add(autor);
            }
            db.Book.Add(book);
            db.SaveChanges();
            return Content("Libro añadido satisfactoriamente");
        }

    }
}