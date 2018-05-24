
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using selcukunikonutlari.Controllers.Araclar;
using selcukunikonutlari.Models;
using PagedList;
using PagedList.Mvc;
using System.Web.Configuration;
using System.Data;

namespace selcukunikonutlari.Controllers
{
    [ArkaplanGaleriController]
    public class indexController : Controller
    {
        //
        // GET: /index/
        Model1 context = new Models.Model1();

        public ActionResult index()
        {
            return View();
        }

        public ActionResult AnaSayfa()
        {
            ViewData["slider"] = context.Galeri.Where(x => x.slider == true).ToList();

            ViewData["duyuru"] = context.Duyuru.Take(10).OrderBy(x => x.tarih);
            return View();
        }


        [HttpGet]
        public ActionResult Iletisim()
        {
            DaireSayibi ds = context.DaireSayibi.Where(x => x.yetkiId == 2).Take(1).FirstOrDefault();
            return View(ds);
        }
        [HttpPost]
        public ActionResult Iletisim(Iletisim iletisim)
        {
            if (iletisim.adsoyad != null && (iletisim.email != null || iletisim.tel != null) && iletisim.mesaj != null)
            {
                iletisim.durum = false;
                context.Iletisim.Add(iletisim);
                context.SaveChanges();
            }
            DaireSayibi ds = context.DaireSayibi.Where(x => x.yetkiId == 2).Take(1).FirstOrDefault();
            return View(ds);
        }

        public ActionResult Duyurular(int sayfa)
        {
            return View(context.Duyuru.OrderByDescending(x => x.tarih).ToList().ToPagedList(sayfa, 10));
        }

        public ActionResult DuyuruTamMetin(int id)
        {
            return View(context.Duyuru.Where(x => x.id == id).Take(1).FirstOrDefault());
        }

        public ActionResult Galeri(int sayfa)
        {
            return View(context.Galeri.Where(x => x.daireId == null && x.calisanId == null).ToList().ToPagedList(sayfa, 20));
        }

        public ActionResult DaireTanitim(int sayfa)
        {
            return View(context.Daire.Where(y => y.bosdolu == true).OrderByDescending(x => x.id).ToList().ToPagedList(sayfa, 5));
        }

        public ActionResult SiteVaziyetPlani()
        {
            return View(context.Galeri.Where(x => x.vaziyet == true).Take(4).ToList());
        }

        public ActionResult OnemliTelefonlar()
        {
            ViewBag.onemlitel = context.Onemlitelefonlar.ToList();
            return View(context.DaireSayibi.Where(x => x.yetkiId == 2).ToList());
        }

        public ActionResult SiteCalisanlari()
        {
            return View(context.Calisanlar.ToList());
        }

        public ActionResult TemsilciKurulu()
        {
            return View(context.Daire.Where(x => x.DaireSayibi.yetkiId == 2 && x.bosdolu == false).ToList());
        }

        public ActionResult YonetimKurulu()
        {
            return View(context.Daire.Where(x => x.DaireSayibi.yetkiId == 1).ToList());
        }

        public ActionResult KurulKararlari(int sayfa)
        {
            return View(context.Duyuru.Where(x => x.durum == 1).ToList().ToPagedList(sayfa, 10));
        }

        [KisiMuhasebegiris]
        public ActionResult AidatBorcDurumu()
        {
            Yonetici giren=(Yonetici)Session["kisimuhasebegiris"];
            if (giren.sifre_durum==false)
                return RedirectToAction("YeniSifre", giren);

            ViewData["mesajlar"] = context.Mesajlar.Where(x => x.yoneticiID == giren.id).Take(1).FirstOrDefault();

            ViewData["adsoyad"] = giren.adi + " " + giren.soyadi;
            List<AidatBorc> liste = context.AidatBorc.Where(x => x.hesap_kodu == giren.hesap_kodu).ToList();
            return View(liste);
        }
        [KisiMuhasebegiris]
        [HttpGet]
        public ActionResult YeniSifre()
        {
            return View();
        }
        [KisiMuhasebegiris]
        [HttpPost]
        public ActionResult YeniSifre(Yonetici yenibilgiler)
        {
            Yonetici giren = (Yonetici)Session["kisimuhasebegiris"];
            Yonetici degistirilecek= context.Yonetici.Where(x=>x.id==giren.id).Take(1).FirstOrDefault();
            degistirilecek.kulaniciadi = yenibilgiler.kulaniciadi;
            degistirilecek.sifre = AracController.MD5eDonustur(yenibilgiler.sifre);
            degistirilecek.tel = yenibilgiler.tel;
            degistirilecek.email = yenibilgiler.email;
            degistirilecek.sifre_durum = true;
            context.SaveChanges();
            Session.RemoveAll();
            return RedirectToAction("KisiMuhasebeGiris");
        }
        public ActionResult BelirsizOdemeler()
        {
            string[] uzantilar = { ".xls", ".xlsx", ".csv" };
            string yol = Server.MapPath("~/Content/buaykiaidat/") + "belirsizodemeler";

            if (System.IO.File.Exists(yol + uzantilar[0]))
                yol = yol + uzantilar[0];
            else if (System.IO.File.Exists(yol + uzantilar[1]))
                yol = yol + uzantilar[1];
            else if (System.IO.File.Exists(yol + uzantilar[2]))
                yol = yol + uzantilar[2];

            string connString;
            if (System.IO.File.Exists(yol))
            {
                DataTable dt=null;
                if (yol.Contains(".csv"))
                {
                    dt = Utility.ConvertCSVtoDataTable(yol);
                    ViewBag.belirsiz = dt;
                }
                else if (yol.Contains(".xlsx"))
                {
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + yol + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //dt = Utility.ConvertXSLXtoDataTable(connString);
                    ViewBag.belirsiz = dt;
                }
                else if (yol.Contains(".xls"))
                {
                    connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + yol + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    //dt = Utility.ConvertXSLXtoDataTable(connString);
                    ViewBag.belirsiz = dt;
                }

            }


            return View();
        }

        public ActionResult BankaBilgileri()
        {
            return View(context.BankaBilgileri.ToList());
        }

        public ActionResult YapilanHarcamaKayitlari()
        {
            string[] uzantilar = { ".xls", ".xlsx", ".csv" };
            string yol = Server.MapPath("~/Content/buaykiaidat/") + "yapilanharcamakaydi";

            if (System.IO.File.Exists(yol + uzantilar[0]))
                yol = yol + uzantilar[0];
            else if (System.IO.File.Exists(yol + uzantilar[1]))
                yol = yol + uzantilar[1];
            else if (System.IO.File.Exists(yol + uzantilar[2]))
                yol = yol + uzantilar[2];

            string connString;
            if (System.IO.File.Exists(yol))
            {
                DataTable dt=null;
                if (yol.Contains(".csv"))
                {
                    dt = Utility.ConvertCSVtoDataTable(yol);
                    ViewBag.yapilanharcama = dt;
                }
                else if (yol.Contains(".xlsx"))
                {
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + yol + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //dt = Utility.ConvertXSLXtoDataTable(connString);
                    ViewBag.yapilanharcama = dt;
                }
                else if (yol.Contains(".xls"))
                {
                    connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + yol + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    //dt = Utility.ConvertXSLXtoDataTable(connString);
                    ViewBag.yapilanharcama = dt;
                }
            }
            return View();
        }

        public ActionResult ErrorSayfasi()
        {
            return View();
        }

        [HttpGet]
        public ActionResult KisiMuhasebeGiris()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KisiMuhasebeGiris(string kullaniciadi, string sifre)
        {
            if (kullaniciadi != null && sifre != null)
            {
                string md5sifre = AracController.MD5eDonustur(sifre);
                Yonetici giris = context.Yonetici.Where(x=> (x.sifre== md5sifre && x.kulaniciadi==kullaniciadi && x.yetki==3 ) ).Take(1).FirstOrDefault();
                
                if (giris!=null && giris.kulaniciadi.Length>0)
                {
                    Session["kisimuhasebegiris"] = giris;
                    return RedirectToAction("AidatBorcDurumu", "index");
                }

            }
            return View();
        }



    }
}
