using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebLogin.Models;

namespace WebLogin.Controllers
{
    public class LoginController : Controller
    {
        public static string SHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Autorizar(WebLogin.Models.Usuario userModel)
        {
            using (LoginDBEntities db = new LoginDBEntities())
            {
                userModel.Password = SHA256(userModel.Password);
                var usuarioDet = db.Usuarios.Where(x => x.Usuario1 == userModel.Usuario1 && x.Password == userModel.Password).FirstOrDefault();
                if(usuarioDet == null)
                {
                    userModel.MensajeDeError = "Usuario o PW incorrectos";
                    return View("Index",userModel);
                }
                else
                {
                    Session["IdUsuario"] = userModel.Usuario1;
                    return RedirectToAction("Index", "Usuario");
                }
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}