using System;
using System.Drawing;
using System.Reflection.Metadata;
using books;
using books.Models;
using books.Services;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

internal class Program
{
    enum _Actions { DISPLAY, SEARCH, CHECKOUT, OVERDUE, EXIT }
    enum _Status { AVALIBLE = 0, CHECKEDOUT = 1, OVERDUE = 2}

    private static async Task Main(string[] args)
    {
        var db = new Context();

        // used to reset values for testing
        //await CreateDBAsync(db);

        string action = "";

        while (action != _Actions.EXIT.ToString())
        {
            Console.WriteLine("Please enter the action you would like to preform from the list provided: ");
            Console.WriteLine("DISPLAY - Displays all books." + System.Environment.NewLine
                + "SEARCH - Search for a book by the Authors name and the book's status." + System.Environment.NewLine
                + "CHECKOUT - Checkout an avalible book by its name." + System.Environment.NewLine
                + "OVERDUE - Allows the user to see all overdue books" + System.Environment.NewLine
                + "EXIT - Exits program." + System.Environment.NewLine);

            action = LibraryHandlers.HandleInputAction();
            string result = "";

            switch (action)
            { 
                case "DISPLAY":
                    result = await Display(db);
                    break;
                case "SEARCH":
                    result = Search(db);
                    break;
                case "CHECKOUT":
                    result = Checkout(db);
                    break;
                case "OVERDUE":
                    result = Overdue(db);
                    break;
            }

            Console.WriteLine(result);
        }

    }

    private static async Task<string> Display(Context db)
    {
        var books = BookService.GetBooks(db);
        string result = "";

        if (books != null)
        {
            result = LibraryHandlers.HandleBooksNames(books);
        }
        else 
        {
            result = "No Books where found";
        }
        return result;
    }

    private static string Search(Context db) 
    {
        string authorName = string.Empty;
        int bookStatus;
        string result = string.Empty;

        while (string.IsNullOrEmpty(authorName)) 
        {
            Console.WriteLine(System.Environment.NewLine +"Please enter the Author's name.");
            authorName = Console.ReadLine();
        }

        var author = AuthorService.GetAuthorByName(db, authorName);

        if (author.Name == null)
        {
            result = "Author could not be found." + System.Environment.NewLine;
        }
        else 
        {
            var bookEnum = LibraryHandlers.HandleInputStatus();
            bookStatus = (int)Enum.Parse(typeof(_Status), bookEnum);

            var books = BookService.GetBooksByAuthorAndStatus(db, author, bookStatus);
            if (books.Count > 0) 
            {
                result = LibraryHandlers.HandleBooksNames(books);
            }
            else 
            {
                result = $"No books where found with author name {authorName} and status {bookEnum}" + System.Environment.NewLine;
            }
        }
        
        return result;
    }

    private static string Checkout(Context db)
    {
        string bookTitle = string.Empty;
        string result = string.Empty;
        while (string.IsNullOrEmpty(bookTitle))
        {
            Console.WriteLine(System.Environment.NewLine + "Please enter the the title of the book you want to check out.");
            bookTitle = Console.ReadLine();
        }

        var book = BookService.GetBookByTitle(db, bookTitle);

        if (book.Title != null && book.Status == 0)
        {
            BookService.CheckoutBook(db, book);
            result = bookTitle + " was checked out" + System.Environment.NewLine;
        }
        else if (book.Title != null)
        {
            result = bookTitle + " cannot be checked out at this time." + System.Environment.NewLine;
        } 
        else
        {
            result = "No book was found with title: " + bookTitle + System.Environment.NewLine;
        }

        return result;
    }

    private static string Overdue(Context db)
    {
        string result = string.Empty;

        var books = BookService.GetOverDueBooks(db);
        if (books.Count > 0)
        {
            List<Borrower> borrowers = new List<Borrower>();
            foreach (var book in books) 
            {
                if (book.BorrowerId != null) 
                {
                    borrowers.Add(BorrowerService.GetBorrowerById(db, book.BorrowerId.Value));
                }
            }
            result = LibraryHandlers.HandleBooksNames(books, borrowers);
        }
        else
        {
            result = $"No overdue books where found" + System.Environment.NewLine;
        }

        return result;
    }

    // sets default values for testing
    /*private static async Task CreateDBAsync(Context db) 
    {
        // create authors
        db.Update(new Author { Id = 1, Name = "Author 1" });
        db.Update(new Author { Id = 2, Name = "Author 2" });
        db.Update(new Author { Id = 3, Name = "Author 3" });
        db.Update(new Author { Id = 4, Name = "Author 4" });
        db.Update(new Author { Id = 5, Name = "Author 5" });
        db.Update(new Author { Id = 6, Name = "Author 6" });

        // create borrowers
        db.Update(new Borrower { Id = 1, Name = "Borrower 1", Email = "borrower1@gmail.com" });

        var lateDate = int.Parse(DateOnly.FromDateTime(DateTime.Now).AddDays(-1).ToString("yyyyMMdd"));
        var weekOutDueDate = int.Parse(DateOnly.FromDateTime(DateTime.Now).AddDays(7).ToString("yyyyMMdd"));
        // create books
        db.Update(new Book { Id = 1, Title = "Book 1", Status = 0, AuthorId = 1 });
        db.Update(new Book { Id = 2, Title = "Book 2", Status = 0, AuthorId = 1 });
        db.Update(new Book { Id = 3, Title = "Book 3", Status = 1, AuthorId = 1, BorrowerId = 1, DueDate = weekOutDueDate });
        db.Update(new Book { Id = 4, Title = "Book 4", Status = 1, AuthorId = 2, BorrowerId = 1, DueDate = weekOutDueDate });
        db.Update(new Book { Id = 5, Title = "Book 5", Status = 2, AuthorId = 3, BorrowerId = 1, DueDate = lateDate });
        db.Update(new Book { Id = 6, Title = "Book 6", Status = 0, AuthorId = 4 });
        db.Update(new Book { Id = 7, Title = "Book 7", Status = 0, AuthorId = 4 });
        db.Update(new Book { Id = 8, Title = "Book 8", Status = 2, AuthorId = 5, BorrowerId = 1, DueDate = lateDate });
        db.Update(new Book { Id = 9, Title = "Book 9", Status = 0, AuthorId = 5 });
        db.Update(new Book { Id = 10, Title = "Book 10", Status = 0, AuthorId = 6 });

        await db.SaveChangesAsync();
    }*/

}