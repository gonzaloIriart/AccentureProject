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
        // Database initialization
        private AccentureAccademyLibraryEntities db;
        public LibraryController()
        {
            this.db = new AccentureAccademyLibraryEntities();
        }
        // Books view
        public ActionResult Index()
        {
            List<Book> books = this.db.Book.ToList();
            return View(books);
        }
        // Send JSON of books.
        // Prevenimos la circularidad de Book y Author con
        // (db.Configuration.ProxyCreationEnabled = false;)
        // Debemos crear la relacion manualmente con el metodo select
        public ActionResult JsonBooks()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var books = this.db.Book.Select(b => new{ 
                b.Author,
                b.Id,
                b.Title,
                b.Cover,
                b.Description,
                b.ReleaseDate,                
                b.Genre,
                b.Publisher

            }).ToList();
            return Json(books, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BookDetails(int id)
        {
            Book book = this.db.Book.Find(id);
            ViewBag.Title = "Book Details";
            return View(book);
        }
        public ActionResult JsonBook(int id)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            Book book = this.db.Book.Find(id);
            return Json(book, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddBook(Book book, IEnumerable<int> Authors)
        {
            foreach (int autorActual in Authors)
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