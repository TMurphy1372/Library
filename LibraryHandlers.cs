using books.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace books
{
    internal class LibraryHandlers
    {
        enum _Actions { DISPLAY, SEARCH, CHECKOUT, OVERDUE, EXIT }
        enum _Status { AVALIBLE = 0, CHECKEDOUT = 1, OVERDUE = 2 }

        public static string HandleInputAction()
        {
            bool valid = false;
            string action = "";
            while (!valid)
            {
                action = Console.ReadLine().ToUpper();

                // handle nonvalid action
                if (Enum.IsDefined(typeof(_Actions), action))
                {
                    valid = true;
                }
                else
                {
                    Console.WriteLine(action + ": is not a valid action. Please enter a valid action." + System.Environment.NewLine);
                }
            }
            return action;
        }
        public static string HandleInputStatus()
        {
            bool valid = false;
            string status = "";
            while (!valid)
            {
                Console.WriteLine("Please enter the book's status.");
                status = Console.ReadLine().ToUpper();

                // handle nonvalid action
                if (Enum.IsDefined(typeof(_Status), status))
                {
                    valid = true;
                }
                else
                {
                    Console.WriteLine(status + ": is not a valid status. Please enter a valid status." + System.Environment.NewLine);
                }
            }
            return status;
        }

        public static string HandleBooksNames(List<Book> books, List<Borrower> borrowers = null)
        {
            string result = string.Empty;
            foreach (var book in books)
            {
                if (book.BorrowerId != null && borrowers == null)
                {
                    result += $"Id: {book.Id}, Title: {book.Title}, Author Id: {book.AuthorId}," +
                        $" Borrower Id: {book.BorrowerId}, Status: {(_Status)book.Status}, Due Date: {book.DueDate}"
                        + System.Environment.NewLine;
                }
                else if (book.BorrowerId != null && borrowers.Count == books.Count)
                {
                    result += $"Id: {book.Id}, Title: {book.Title}, Author Id: {book.AuthorId}," +
                        $" Borrower Id: {book.BorrowerId}, Status: {(_Status)book.Status}, Due Date: {book.DueDate}"
                        + System.Environment.NewLine;

                    Borrower overDueBorrower;
                    foreach (var borrower in borrowers)
                    {
                        if (book.BorrowerId == borrower.Id)
                        {
                            overDueBorrower = borrower;
                            result += $"Borrower: {overDueBorrower.Name} their email address is {overDueBorrower.Email}"
                                + System.Environment.NewLine + System.Environment.NewLine;
                            break;
                        }
                    }
                }
                else
                {
                    result += $"Id: {book.Id}, Title: {book.Title}, Author Id: {book.AuthorId}, Status: {(_Status)book.Status}"
                        + System.Environment.NewLine;
                }
            }
            return result;
        }
    }
}
