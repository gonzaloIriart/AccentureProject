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

        public ActionResult Edit(int id)
        {
            Book book = db.Book.Find(id);
            ViewBag.header = $"Editar ${book.Title}";
            return View("AddBook", book);
        }

        [HttpPost]
        public ActionResult Edit(Book book, IEnumerable<int> Authors)
        {
            Book databaseBook = db.Book.Find(book.Id);
            databaseBook.Title = book.Title;
            databaseBook.Id_Genre = book.Id_Genre;
            databaseBook.Cover = book.Cover;
            databaseBook.ISBN = book.ISBN;
            databaseBook.Description = book.Description;
            databaseBook.ReleaseDate = book.ReleaseDate;
            databaseBook.Id_Publisher = book.Id_Publisher;
            databaseBook.Author.Clear();

            foreach (int actualAuthor in Authors)
            {
                Author by = db.Author.Find(actualAuthor);
                databaseBook.Author.Add(by);
            }
            db.SaveChanges();
            return Content("Libro editado satisfactoriamente");
        }

    public ActionResult AddBook()
        {
            ViewBag.header = "Add Book";
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

        public ActionResult Publisher()
        {
            ViewBag.Name = "Publisher";
            return View();
        }

        [HttpPost]
        public ActionResult Publisher(Publisher p)
        {
            db.Publisher.Add(p);
            db.SaveChanges();
            return RedirectToAction("AddBook");
        }

        public ActionResult Author()
        {
            ViewBag.Name="Author";
            return View("Publisher");
        }

        [HttpPost]
        public ActionResult Author(Author Author)
        {
            this.db.Author.Add(Author);
            this.db.SaveChanges();
            return RedirectToAction("AddBook");
        }

        //GENRES

        public ActionResult Genre()
        {
            ViewBag.Name = "Genre";
            return View("Publisher");
        }

        [HttpPost]
        public ActionResult Genre(Genre Genre)
        {
            this.db.Genre.Add(Genre);
            this.db.SaveChanges();
            return RedirectToAction("AddBook");
        }

    }
}