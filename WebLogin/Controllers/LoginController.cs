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