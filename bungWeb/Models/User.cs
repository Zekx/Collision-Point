using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bungWeb.Models
{
    public class User
    {
        public String firstname { get; set; }
        public String lastname { get; set; }
        public String username { get;}
        public String phone { get; set; }
        public String email { get; set; }
        public int position { get; }

        public User(String fn, String ln, String usr, String ph, String em, int pos) {
            firstname = fn;
            lastname = ln;
            username = usr;
            phone = ph;
            email = em;
            position = pos;
        }
    }
}