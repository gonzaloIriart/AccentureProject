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

        public ActionResult Search(string search)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var books = this.db.Book.Where(b => b.Title.Contains(search)).Select(b => new {
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

        public ActionResult Filter(int g, int a, int p)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var books = db.Book.Select(b => new {
                b.Author,
                b.Id,
                b.Title,
                b.Cover,
                b.Description,
                b.ReleaseDate,
                b.Genre,
                b.Publisher
            }).ToList();
            if (g != 0)
            {
                books = books.FindAll(item => item.Genre.Id == g);
            }
            if (p != 0)
            {
                books = books.FindAll(b => b.Publisher.Id == p);
            }
            if(a != 0)
            {
                Author author = this.db.Author.Find(a);
                books = books.FindAll(b => b.Author.Contains(author));
            }
            
            return Json(books, JsonRequestBehavior.AllowGet);
        }
        //Book details View
        public ActionResult BookDetails(int id)
        {
            Book book = this.db.Book.Find(id);
            ViewBag.header = "Book Details";
            return View(book);
        }
        //This function selects a random book and uses the BookDetails view
        public ActionResult Recommend()
        {
            Book book = this.db.Book.OrderBy(b => Guid.NewGuid()).Take(1).First();
            ViewBag.header = $"We recommend {book.Title}";
            return View("BookDetails", book);
        }

        //JSON of book details
        public ActionResult JsonBook(int id)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            Book book = this.db.Book.Find(id);
            return Json(book, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddBook()
        {
            Book book = new Book();
            ViewBag.header = "Add Book";
            return View(book);
        }

        [HttpPost]
        public ActionResult AddBook(Book book, IEnumerable<int> Authors)
        {
            if (Authors.Count()==0||(book.Title.Length<=5)|| (book.Title.Length > 50)||book.Id_Genre==0||book.Id_Publisher==0||book.ISBN==null||book.ReleaseDate==null)
            {
                ViewBag.errorMessage = "Verifique los datos ingresados";
                ViewBag.header = "Add Book";
                return View("AddBook", book);
            }
            else
            {
                foreach (int autorActual in Authors)
                {
                    Author autor = this.db.Author.Find(autorActual);
                    book.Author.Add(autor);
                }
                db.Book.Add(book);
                db.SaveChanges();
                ViewBag.successMessage = "Libro añadido con exito";
                return RedirectToAction("Index", "Home");
            }
            
        }

        //Edit book, reuses the addBook View
        public ActionResult Edit(int id)
        {
            Book book = db.Book.Find(id);
            ViewBag.header = $"Editar {book.Title}";
            return View("AddBook", book);
        }

        [HttpPost]
        public ActionResult Edit(Book book, IEnumerable<int> Authors)
        {
            if (Authors.Count() == 0 || (book.Title.Length <= 5) || (book.Title.Length > 50) || book.Id_Genre == 0 || book.Id_Publisher == 0 || book.ISBN == null || book.ReleaseDate == null)
            {
                ViewBag.errorMessage = "Verifique los datos ingresados";
                ViewBag.header = $"Editar {book.Title}";
                return View("AddBook", book);
            }
            else
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
                ViewBag.successMessage = "Libro editado con exito";
                return RedirectToAction("Index", "Home");
            }
            
        }

        public ActionResult Delete(int id)
        {
            Book book = db.Book.Find(id);
            this.db.Book.Remove(book);
            this.db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        //PUBLISHERS CRUD
        
        public ActionResult AddPublisher()
        {
            ViewBag.Name = "Publisher";
            return View();
        }

        [HttpPost]
        public ActionResult AddPublisher(Publisher p)
        {
            db.Publisher.Add(p);
            db.SaveChanges();
            return RedirectToAction("AddBook");
        }

        public ActionResult EditPublisher(int id)
        {
            Publisher p = db.Publisher.Find(id);
            ViewBag.Name = " Edit Publisher";
            ViewBag.Value = p.Name;
            return View("AddPublisher");
        }

        [HttpPost]
        public ActionResult EditPublisher(Publisher p)
        {
            Publisher publisherDB = db.Publisher.Find(p.Id);
            publisherDB.Name = p.Name;
            db.SaveChanges();
            return RedirectToAction("ListPublisher");
        }

        public ActionResult ListPublisher()
        {
            ViewBag.Name = "Publishers List";
            ViewBag.Route = "EditPublisher";
            ViewBag.RouteAdd = "AddPublisher";
            List<Publisher> publishers = this.db.Publisher.ToList();
            ViewBag.List = publishers;            
            return View();
        }

        //AUTHOR CRUD, reuses publisher views

        public ActionResult AddAuthor()
        {
            ViewBag.Name="Author";
            ViewBag.Title = "Add Author";
            ViewBag.List = "ListAuthor";
            return View("AddPublisher");
        }

        [HttpPost]
        public ActionResult AddAuthor(Author Author)
        {
            this.db.Author.Add(Author);
            this.db.SaveChanges();
            return RedirectToAction("AddBook");
        }


        public ActionResult EditAuthor(int id)
        {
            Author a = this.db.Author.Find(id);
            ViewBag.Name = "Author";
            ViewBag.Title = "Edit Author";
            ViewBag.List = "ListAuthor";
            ViewBag.Value = a.Name;
            return View("AddPublisher", a);
        }

        [HttpPost]
        public ActionResult EditAuthor(Author a)
        {
            Author authorDB = db.Author.Find(a.Id);
            authorDB.Name = a.Name;
            db.SaveChanges();
            return RedirectToAction("ListAuthor");
        }

        public ActionResult ListAuthor()
        {
            ViewBag.Name = "Authors List";
            ViewBag.Route = "EditAuthor";
            ViewBag.RouteAdd = "AddAuthor";
            List<Author> authors = this.db.Author.ToList();
            ViewBag.List = authors;
            return View("ListPublisher");
        }

        //GENRES CRUD, reuses publisher views

        public ActionResult AddGenre()
        {
            ViewBag.Name = "Genre";
            ViewBag.Title = "Add Genre";
            ViewBag.List = "ListGenre";
            return View("Publisher");
        }

        [HttpPost]
        public ActionResult AddGenre(Genre Genre)
        {
            this.db.Genre.Add(Genre);
            this.db.SaveChanges();
            return RedirectToAction("ListGenre");
        }


        public ActionResult EditGenre(int id)
        {
            Genre g = this.db.Genre.Find(id);
            ViewBag.Name = "Genre";
            ViewBag.Title = "Edit Genre";
            ViewBag.List = "ListGenre";
            ViewBag.Value = g.Name;
            return View();
        }

        [HttpPost]
        public ActionResult EditGenre(Genre g)
        {
            Genre genreDB = this.db.Genre.Find(g.Id);
            genreDB.Name = g.Name;
            db.SaveChanges();
            return RedirectToAction("ListGenre");
        }

        public ActionResult ListGenre()
        {
            ViewBag.Name = "Genres List";
            ViewBag.Route = "EditGenre";
            ViewBag.RouteAdd = "AddGenre";
            List<Genre> genres = this.db.Genre.ToList();
            ViewBag.List = genres;
            return View("ListPublisher");
        }
    }
}