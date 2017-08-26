using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MySql.Data.MySqlClient;
using bungWeb.Models;
using System.Text;

namespace bungWeb.Controllers
{
    public class SqlController
    {
        static string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["gCloudConnection"].ConnectionString;

        public static User retrieveLogin(string username, string password) {
            User resultUsr = null;
            string checkPassword = "abc";

            MySql.Data.MySqlClient.MySqlConnection mySqlConnection;

            try
            {
                mySqlConnection = new MySql.Data.MySqlClient.MySqlConnection(connStr);
                MySqlCommand cmd = new MySqlCommand("SELECT * from users where username = ?USRN", mySqlConnection);
                cmd.Parameters.AddWithValue("?USRN", username);
                mySqlConnection.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read()) {
                    /*
                    System.Diagnostics.Debug.WriteLine(reader.GetString("firstname"));
                    System.Diagnostics.Debug.WriteLine(reader.GetString("lastname"));
                    System.Diagnostics.Debug.WriteLine(reader.GetString("username"));
                    System.Diagnostics.Debug.WriteLine(reader.GetString("pass"));
                    System.Diagnostics.Debug.WriteLine(reader.GetString("email"));
                    System.Diagnostics.Debug.WriteLine(reader.GetString("phone"));
                    System.Diagnostics.Debug.WriteLine(reader.GetString("position"));
                    */

                    resultUsr = new User(reader.GetString("firstname"), reader.GetString("lastname"), 
                        reader.GetString("username"), reader.GetString("phone"), reader.GetString("email"), Int32.Parse(reader.GetString("position")));
                    checkPassword = reader.GetString("pass");
                }

                mySqlConnection.Close();
            }
            catch (Exception e) {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }

            string sha256Password = sha256(password);
            if (!(sha256Password.Equals(checkPassword))) {
                resultUsr = null;
            }

            return resultUsr;
        }

        public static List<Post> retrievePost(string username)
        {
            List<Post> posts = new List<Post>();

            MySql.Data.MySqlClient.MySqlConnection mySqlConnection;

            try
            {
                mySqlConnection = new MySql.Data.MySqlClient.MySqlConnection(connStr);
                MySqlCommand cmd = new MySqlCommand("select p.*, u.firstname, u.lastname from posts p, users u where p.userid = (select id from users where username = ?USRN);", mySqlConnection);
                cmd.Parameters.AddWithValue("?USRN", username);

                mySqlConnection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    /*
                    System.Diagnostics.Debug.WriteLine(reader.GetInt32("id"));
                    System.Diagnostics.Debug.WriteLine(reader.GetString("firstname"));
                    System.Diagnostics.Debug.WriteLine(reader.GetString("lastname"));
                    System.Diagnostics.Debug.WriteLine(reader.GetString("title"));
                    System.Diagnostics.Debug.WriteLine(reader.GetString("body"));
                    System.Diagnostics.Debug.WriteLine(reader.GetDateTime("datePosted"));
                    System.Diagnostics.Debug.WriteLine(reader.GetBoolean("removed"));
                    */

                    posts.Add(new Post(reader.GetInt32("id"), reader.GetString("firstname") + " " + reader.GetString("lastname"), reader.GetString("title"),
                        reader.GetString("body"), reader.GetDateTime("datePosted"), reader.GetBoolean("removed")));
                }

                    mySqlConnection.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }

            return posts;
        }

        public static bool createPost(string username, string title, string body) {
            MySql.Data.MySqlClient.MySqlConnection mySqlConnection;

            try
            {
                mySqlConnection = new MySql.Data.MySqlClient.MySqlConnection(connStr);
                MySqlCommand cmd = new MySqlCommand("INSERT INTO posts(userid, title, body, datePosted, removed) " +
                    "values((SELECT id from users where username = ?USRN), ?TIT, ?BOD, NOW(), false)", mySqlConnection);
                mySqlConnection.Open();

                cmd.Parameters.AddWithValue("?USRN", username);
                cmd.Parameters.AddWithValue("?TIT", title);
                cmd.Parameters.AddWithValue("?BOD", body);

                cmd.ExecuteNonQuery();

                mySqlConnection.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        public static String sha256(String val) {
            StringBuilder sb = new StringBuilder();

            using (System.Security.Cryptography.SHA256 hash = System.Security.Cryptography.SHA256Managed.Create()) {
                Encoding en = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(en.GetBytes(val));

                foreach (Byte b in result) {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }
    }
}