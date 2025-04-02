using books.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.Services
{
    internal class BookService
    {
        public static List<Book> GetBooks(Context db) 
        {
            return db.Books
            .OrderBy(b => b.Id)
            .ToList(); 
        }

        public static List<Book> GetBooksByAuthorAndStatus(Context db, Author author, int bookStatus) 
        {
            return db.Books.Where(b => b.AuthorId == author.Id && b.Status == bookStatus).ToList();
        }

        public static Book GetBookByTitle(Context db, string bookTitle) 
        {
            var books = db.Books.ToList();
            return books.Where(b => b.Title.ToUpper() == bookTitle.ToUpper()).SingleOrDefault(new Book());
             
        }

        public static void CheckoutBook(Context db, Book book) 
        {
            var weekOutDueDate = int.Parse(DateOnly.FromDateTime(DateTime.Now).AddDays(7).ToString("yyyyMMdd"));
            book.Status = 1;
            book.BorrowerId = 1;
            book.DueDate = weekOutDueDate;
            db.Update(book);
        }

        public static List<Book> GetOverDueBooks(Context db) 
        {
            return db.Books.Where(b => b.Status == 2).ToList();
        }
    }
}
