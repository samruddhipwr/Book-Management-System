using BookManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BookManagementSystem.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User objUser)
        {
            if (ModelState.IsValid)
            {
                using (Book_Management db = new Book_Management())
                {
                    var EncryptPass = Encryptdata(objUser.Password);
                    var obj=db.Users.Where(a=>a.UserName.Equals(objUser.UserName) && a.Password.Equals(EncryptPass)).FirstOrDefault();
                    if(obj!=null)
                    {
                        Session["UserId"] = obj.UserId.ToString();
                        Session["UserName"] = obj.UserName.ToString();
                        return RedirectToAction("Profile");
                    }
                    else
                    {
                        ModelState.AddModelError("password", "The username or password is incorrect");
                    }
                }
            }
            
            return View(objUser);
        }
        public ActionResult Profile()
        {
            if (Session["UserId"]!=null)
            {
                return View();
            }
            else
            {
                
                return RedirectToAction("Login");
            }
        }
        public static string Encryptdata(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }
        public static string Decryptdata(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }
    }
}
