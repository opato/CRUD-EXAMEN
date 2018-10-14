using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLogin.Models;

namespace WebLogin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        private string Encrypt_Password(string password)
        {
            string pswstr = string.Empty;
            byte[] psw_encode = new byte[password.Length];
            psw_encode = System.Text.Encoding.UTF8.GetBytes(password);
            pswstr = Convert.ToBase64String(psw_encode);
            return pswstr;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Autorizar(WebLogin.Models.Usuario userModel)
        {
            using (LoginDBEntities db = new LoginDBEntities())
            {
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