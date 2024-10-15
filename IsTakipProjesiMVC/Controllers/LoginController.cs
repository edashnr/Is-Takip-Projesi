using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IsTakipProjesiMVC.Models;


namespace IsTakipProjesiMVC.Controllers
{
    public class LoginController : Controller
    {
        IsTakipDBEntities entity = new IsTakipDBEntities();
        // GET: Login
        public ActionResult Index()
        {
            ViewBag.mesaj = null;
            return View();
        }

        [HttpPost]  
        public  ActionResult Index(string kullaniciAd,string parola)
        {

            Personeller personel  = (from p in entity.Personeller where p.personelKullaniciAd==kullaniciAd && 
                                    p.personelParola==parola
                                    select p).FirstOrDefault();
            if (personel != null)
            {
                Session["PersonelAdSoyad"]= personel.personelAdSoyad;
                Session["PersonelId"]=personel.personelId;
                Session["PersonelBirimId"] = personel.personelBirimId;
                Session["PersonelYetkiTürId"] = personel.personelYetkiTürId;

                switch (personel.personelYetkiTürId)
                {
                    case 1:
                        return RedirectToAction("Index", "Yönetici");

                    case 2:
                        return RedirectToAction("Index", "Calisan");

                    default:
                        return View();
                }
            }
            else
            {
                ViewBag.mesaj = "Kullanıcı Adı ya da Parola Yanlış";
                return View();
            }

            
        }

    }
}