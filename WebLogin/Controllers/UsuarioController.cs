using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLogin.Models;

namespace WebLogin.Controllers
{
    public class UsuarioController : Controller
    {
        private string Encrypt_Password(string password)
        {
            string pswstr = string.Empty;
            byte[] psw_encode = new byte[password.Length];
            psw_encode = System.Text.Encoding.UTF8.GetBytes(password);
            pswstr = Convert.ToBase64String(psw_encode);
            return pswstr;
        }

        // GET: Usuario
        public ActionResult Index()
        {
            try
            {
                using (LoginDBEntities db = new LoginDBEntities())
                {
                    List<Usuario> lista = db.Usuarios.Where(a => a.Estatus.Value).ToList();
                    return View(lista);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Usuario rowUsuario)
        {
            if(!ModelState.IsValid)
                return View();
            try
            {
                using (LoginDBEntities db = new LoginDBEntities())
                {
                    var usuarioExist = db.Usuarios.Where(x => x.Usuario1 == rowUsuario.Usuario1 && x.Correo == rowUsuario.Correo).FirstOrDefault();
                    if (!(usuarioExist == null))
                    {
                        ModelState.AddModelError("", "Ya existe un usuario con la misma información");
                        return View();
                    }
                    rowUsuario.Estatus = true;
                    rowUsuario.Password = Encrypt_Password(rowUsuario.Password);
                    rowUsuario.ConfirmaPassword = Encrypt_Password(rowUsuario.ConfirmaPassword);
                    db.Usuarios.Add(rowUsuario);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("","Error al agregar usuario" + ex.Message);
                return View();
            }
        }

        public ActionResult Editar(int id)
        {
            try
            {
                using (LoginDBEntities db = new LoginDBEntities())
                {
                    Usuario usuario = db.Usuarios.Find(id);
                    return View(usuario);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Usuario rowUsuario)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();
                using (LoginDBEntities db = new LoginDBEntities())
                {
                    Usuario usuarioReg = db.Usuarios.Find(rowUsuario.Id);
                    usuarioReg.Correo = rowUsuario.Correo;
                    usuarioReg.Sexo = rowUsuario.Sexo;
                    usuarioReg.Password = Encrypt_Password(rowUsuario.Password);
                    usuarioReg.ConfirmaPassword = Encrypt_Password(rowUsuario.Password);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al editar usuario" + ex.Message);
                return View();
            }
        }

        public ActionResult Eliminar(int id)
        {
            try
            {
                using (LoginDBEntities db = new LoginDBEntities())
                {
                    Usuario usuario = db.Usuarios.Find(id);
                    usuario.Estatus = false;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}