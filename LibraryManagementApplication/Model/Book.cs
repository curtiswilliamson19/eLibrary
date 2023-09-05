using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementApplication.Model
{
    public class Book
    {
        //Make these virtual for lazy loading
        public virtual Guid ID { get; set; }
        public virtual string Title { get; set; }
        public virtual string Author { get; set; }
        public virtual string Description { get; set; }
        public virtual string ISBN { get; set; }

        public Book() { }

        public Book(Guid iD, string title, string author, string description, string iSBN)
        {
            ID = iD;
            Title = title;
            Author = author;
            Description = description;
            ISBN = iSBN;
        }

        public override string ToString()
        {
            return $"{Title} ({Author}, {ID})"; //needed to shorten for display in list
        }

        public override bool Equals(object obj)
        {
            return obj is Book book &&
                Title == book.Title &&
                ISBN == book.ISBN;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Book book1, Book book2)
        {
            if(book1 is null && book2 is null)
            {
                return true;
            }
            return !(book1 is null) && book1.Equals(book2);
        }

        public static bool operator !=(Book book1, Book book2)
        {
            return !(book1 == book2);
        }
    }
}
