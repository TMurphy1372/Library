using books.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books.Services
{
    internal class AuthorService
    {
        public static Author GetAuthorByName(Context db, string authorName) 
        {
            var authors = db.Authors.ToList();
            return authors.Where(a => a.Name.ToUpper() == authorName.ToUpper()).SingleOrDefault(new Author());
        }
    }
}
