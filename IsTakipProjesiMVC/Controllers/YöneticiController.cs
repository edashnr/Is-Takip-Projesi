using IsTakipProjesiMVC.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsTakipProjesiMVC.Controllers
{
    public class YöneticiController : Controller
    {


        IsTakipDBEntities entity = new IsTakipDBEntities();
        // GET: Yönetici

        // LOGİN SYAFASINA GİRİŞ YAPILIYOR
        public ActionResult Index()
        {
            int yetkiTürId =Convert.ToInt32( Session["PersonelYetkiTürId"]);

            if (yetkiTürId == 1) {

                int birimId = Convert.ToInt32(Session["PersonelbirimId"]);

                var birim = (from b in entity.Birimler
                             where b.birimId == birimId
                             select b).FirstOrDefault();

                ViewBag.birimAd = birim.birimAd;

                return View();
            }
            else {
                return RedirectToAction("Index","Login");
            }
            
        }

        //İŞ ATAMA İŞLEMİ YAPILIYOR
        public ActionResult Ata()
        {

            int yetkiTürId = Convert.ToInt32(Session["PersonelYetkiTürId"]);

            if (yetkiTürId == 1)
            {

                int birimId = Convert.ToInt32(Session["PersonelBirimId"]);

                var calisanlar= (from p in entity.Personeller
                                 where p.personelBirimId == birimId && p.personelYetkiTürId == 2
                                 select p).ToList();

                ViewBag.personeller = calisanlar;

               

                var birim = (from b in entity.Birimler
                             where b.birimId == birimId
                             select b).FirstOrDefault();

                ViewBag.birimAd = birim.birimAd;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        //İŞ ATAMA KAYDETME YAPILIYOR
        [HttpPost]
        public ActionResult Ata(FormCollection formCollection)
        {
            string isBaslik = formCollection["isBaslik"];
            string isAciklama = formCollection["isAciklama"];
            int secilenPersonelId = Convert.ToInt32(formCollection ["selectPer"]);

            Isler yeniIs = new Isler();

            yeniIs.isBaslik = isBaslik;
            yeniIs.isAciklama=isAciklama;
            yeniIs.isPersonelId = secilenPersonelId;    
            yeniIs.iletilenTarih = DateTime.Now;
            yeniIs.isDurumId = 1;
            yeniIs.isOkunma = false;

            entity.Isler.Add(yeniIs);
            entity.SaveChanges();

            return RedirectToAction("Takip","Yönetici");
        }

        //İŞ TAKİPİ İÇİN OLUŞTURULUYOR
            public ActionResult Takip()
        {

            int yetkiTürId = Convert.ToInt32(Session["PersonelYetkiTürId"]);

            if (yetkiTürId == 1)
            {

                int birimId = Convert.ToInt32(Session["PersonelBirimId"]);

                var calisanlar = (from p in entity.Personeller
                                  where p.personelBirimId == birimId && p.personelYetkiTürId == 2
                                  select p).ToList();

                ViewBag.personeller = calisanlar;



                var birim = (from b in entity.Birimler
                             where b.birimId == birimId
                             select b).FirstOrDefault();

                ViewBag.birimAd = birim.birimAd;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public ActionResult Takip(int selectPer)
        {
            var secilenPersonel =( from p in entity.Personeller
                                  where p.personelId == selectPer
                                  select p).FirstOrDefault();

            TempData["secilen"] = secilenPersonel;

            return RedirectToAction("Listele", "Yönetici");
        }

        [HttpGet]
        public  ActionResult Listele()
        {
            int yetkiTürId = Convert.ToInt32(Session["PersonelYetkiTürId"]);

            if (yetkiTürId == 1)
            {
                if (TempData["secilen"] != null)
                {
                    Personeller secilenPersonel = (Personeller)TempData["secilen"];

                    try
                    {
                        var isler = (from i in entity.Isler
                                     where i.isPersonelId == secilenPersonel.personelId
                                     select i).ToList().OrderByDescending(i => i.iletilenTarih);

                        ViewBag.isler = isler;
                        ViewBag.personel = secilenPersonel;
                        ViewBag.isSayisi = isler.Count();

                        return View();

                    }
                    catch (Exception)
                    {

                        return RedirectToAction("Takip", "Yönetici");
                    }

                  
                }
                else
                {
                    // Hata mesajı veya yönlendirme
                    return RedirectToAction("Takip", "Yönetici");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}