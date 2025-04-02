using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.Models
{
    internal class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public int? BorrowerId { get; set; }
        public int Status { get; set; }
        public int? DueDate { get; set; }
    }
}
