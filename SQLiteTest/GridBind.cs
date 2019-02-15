using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLiteTest
{
    struct GridBind
    {
        private string author;
        private string book;

        public GridBind(string author, string book)
        {
            this.author = author;
            this.book = book;
        }

        public string Author
        { get
            {
                return author;
            }
}

        public string Book
        { get
            {
                return book;
            }
        }
    }
}
