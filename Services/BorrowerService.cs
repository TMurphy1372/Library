using books.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.Services
{
    internal class BorrowerService
    {
        public static Borrower GetBorrowerById(Context db, int BorrowerId) 
        {
            return db.Borrowers.Where(bw => bw.Id == BorrowerId).Single();
        }
    }
}
