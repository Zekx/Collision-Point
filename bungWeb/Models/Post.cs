using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bungWeb.Models
{
    public class Post
    {
        public int id { get; }
        public String author { get; set; }
        public String title { get; set; }
        public String body { get; set; }
        public DateTime rawDate { get; set; }
        public String datePosted { get; set; }
        public Boolean removed { get; }

        public Post(int id, String au, String ti, String bo, DateTime date, Boolean rm) {
            this.id = id;
            author = au;
            title = ti;
            body = bo;
            rawDate = date;
            datePosted = date.Date.ToString("MM/dd/yyy");
            removed = rm;
        }
    }
}