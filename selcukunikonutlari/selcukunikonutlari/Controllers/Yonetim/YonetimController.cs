using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using selcukunikonutlari.Models;
using selcukunikonutlari.Controllers.Araclar;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Data;

namespace selcukunikonutlari.Controllers.Yonetim
{

    public class YonetimController : Controller
    {
        [HttpGet]
        public ActionResult YonetimAnasayfa()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YonetimAnasayfa(string mesaj, string email, string sifre)
        {
            if (mesaj.Length > 0 && Session["giris"] != null)
            {
                IEnumerable<Yonetici> daire = context.Yonetici.Where(x => x.email != null).ToList();
                Yonetici giris = (Yonetici)Session["giris"];
                foreach (var item in daire)
                {
                    WebMail.SmtpServer = "smtp.gmail.com";
                    WebMail.UserName = email;
                    WebMail.Password = sifre;
                    WebMail.EnableSsl = true;
                    WebMail.SmtpPort = 587;
                    string mesajicerigi = mesaj;
                    WebMail.Send(item.email, "Site Yoneticii tarafından bir mesaj var", mesajicerigi, email);

                }
            }
            return View();
        }

        Model1 context = new Model1();

        [HttpGet]
        public ActionResult Giris()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Giris(string kullaniciadi, string sifre)
        {
            if (kullaniciadi != null && sifre != null)
            {
                Yonetici giris = null;
                string sifremd5 = AracController.MD5eDonustur(sifre);
                giris=context.Yonetici.Where(x => ((x.yetki == 1 || x.yetki == 2) && x.kulaniciadi == kullaniciadi && x.sifre == sifremd5)).Take(1).FirstOrDefault();
                if (giris == null)
                {
                    ViewBag.hata = "Şifre veya kullanıcı adınız yanlış";
                    return View();
                }
                    
                switch (giris.yetki)
                {
                    case 1:
                        Session["giris"] = giris;
                        return RedirectToAction("YonetimAnasayfa", "Yonetim");
                    case 2:
                        Session["muhasebegiris"] = giris;
                        return RedirectToAction("AidatEkle", "Yonetim");
                }
               

            }
            return View();
        }
        [HttpGet]
        [KontrolController]
        public ActionResult BilgilerimiDegis()
        {
            Yonetici y = (Yonetici)Session["giris"];
            return View(y);
        }
        [HttpPost]
        [KontrolController]
        public ActionResult BilgilerimiDegis(Yonetici giris)
        {
            Yonetici y = null;
            try
            {
                if ((giris.sifre != null || giris.sifre != "") && (giris.kulaniciadi != null || giris.kulaniciadi != ""))
                {
                    y = context.Yonetici.Where(x => x.id == giris.id).Take(1).FirstOrDefault();
                    y.email = giris.email;
                    y.kulaniciadi = giris.kulaniciadi;
                    y.sifre = AracController.MD5eDonustur(giris.sifre);
                    y.tel = giris.tel;
                    context.SaveChanges();
                    Session["giris"] = y;
                }
                else
                {
                    ViewBag.hata = "Eksik Bilgi Girdiniz";
                }
            }
            catch
            {
                ViewBag.hata = "Bilgiler Değiştirilemedi";
            }
            return View(y);
        }
        [KontrolController]
        [HttpGet]
        public ActionResult OnemliTelefonlar()
        {
            return View(context.Onemlitelefonlar);
        }
        [KontrolController]
        [HttpPost]
        public ActionResult OnemliTelefonlar(Onemlitelefonlar bilgiler)
        {
            if (bilgiler.adsoyad != null && bilgiler.baslik != null && bilgiler.tel != null)
            {
                try
                {
                    context.Onemlitelefonlar.Add(bilgiler);
                    context.SaveChanges();
                }
                catch
                {

                }
            }
            else
                ViewBag.hata = "Eksik Bilgi Girdiniz";
            return View(context.Onemlitelefonlar);
        }
        [KontrolController]
        public ActionResult OnemliTelefonlarSil(int id)
        {
            try
            {
                context.Onemlitelefonlar.Remove(context.Onemlitelefonlar.Where(x => x.id == id).Take(1).FirstOrDefault());
                context.SaveChanges();
            }
            catch
            {
                ViewBag.hata = "İşlem gercekleştirilemedi";
            }
            return RedirectToAction("OnemliTelefonlar", "Yonetim");
        }
        [KontrolController]
        [HttpGet]
        public ActionResult SifreSifirla()
        {
            List<Yonetici> liste = context.Yonetici.Where(x => x.yetki == (int)EnumlarController.girisyetki.dairesakinleri).ToList();

            return View(liste);
        }
        [KontrolController]
        [HttpPost]
        public ActionResult SifreSifirla(int[] id)
        {
            if (id!=null && id.Count()>0)
            {
                List<Yonetici> liste = context.Yonetici.Where(x => id.Contains(x.id)).ToList();
                for (int i = 0; i < liste.Count(); i++)
                {
                    liste[i].kulaniciadi = liste[i].adi;
                    liste[i].sifre = AracController.MD5eDonustur(liste[i].soyadi);
                    liste[i].sifre_durum = false;
                }
                context.SaveChanges();
            }else
                ViewBag.hata = "Herhangi bir kullanıcı secilmedi";

            return RedirectToAction("SifreSifirla");
        }

        #region daire islemleri
        [KontrolController]
        public ActionResult DaireEkle()
        {
            return View();
        }
        [HttpGet]
        [KontrolController]
        public ActionResult BosDaire()
        {
            return View();
        }
        [HttpPost]
        [KontrolController]
        public ActionResult BosDaire(Daire yenidaire, HttpPostedFileBase[] resimler)
        {

            if (yenidaire.blokno != null && yenidaire.aciklama != null && yenidaire.daireno != null && yenidaire.dairedurum > 0 && yenidaire.fiyat > 0 && yenidaire.kat > 0)
            {
                yenidaire.bosdolu = true;
                context.Daire.Add(yenidaire);
                context.SaveChanges();
                yenidaire = context.Daire.Where(x => x.blokno == yenidaire.blokno && x.daireno == yenidaire.daireno).Take(1).FirstOrDefault();
                int id = (int)yenidaire.id;
                AracController resimekle = new AracController(Server.MapPath("~/"));
                resimekle.ResimKaydet(resimler, yenidaire.blokno + yenidaire.daireno, "daire", id);
            }
            else
                ViewBag.hata = "Eksik Bilgi Girdiniz";
            try
            {
            }
            catch
            {

                ViewBag.hata = "İşlem Gercekleştirilemedi";
            }


            return View();
        }

        [HttpGet]
        [KontrolController]
        public ActionResult DoluDaire(int id)
        {
            if (id > 0)
                return View(context.Daire.Where(x => x.id == id).Take(1).FirstOrDefault());

            return View();


        }
        [HttpPost]
        [KontrolController]
        public ActionResult DoluDaire(Daire daire, DaireSayibi DaireSayibi, HttpPostedFileBase resim)
        {
            try
            {
                if (daire.id == 0 && daire.blokno != null && daire.daireno != null && DaireSayibi.ad != null && DaireSayibi.durum > 0 && DaireSayibi.soyad != null && DaireSayibi.tel != null)
                {
                    bool kontrol = true;
                    if (DaireSayibi.durum!=1)
                    {
                        if(context.Daire.Where(x => x.DaireSayibi.durum == DaireSayibi.durum).Count() > 0)
                            kontrol=false;
                    }
                    if (kontrol)
                    {
                        daire.bosdolu = false;
                        context.Daire.Add(daire);
                        context.SaveChanges();
                        daire = context.Daire.Where(x => x.blokno == daire.blokno && x.daireno == daire.daireno).Take(1).FirstOrDefault();
                        DaireSayibi.daireId = daire.id;
                        context.DaireSayibi.Add(DaireSayibi);
                        context.SaveChanges();
                        Daire d = context.Daire.Where(x => x.DaireSayibi.tel == DaireSayibi.tel).Take(1).FirstOrDefault();
                        if (resim != null)
                        {
                            AracController resimekle = new AracController(Server.MapPath("~/"));
                            resimekle.doludaireresimekle(resim, d.id);
                        }
                    }
                    else
                        ViewBag.hata = "İşleminiz Gercekleştirilemdi";
                }
                else if (daire.id > 0 && daire.blokno != null && daire.daireno != null && DaireSayibi.ad != null && DaireSayibi.durum > 0 && DaireSayibi.soyad != null && DaireSayibi.tel != null)
                {
                    Daire d = context.Daire.Where(x => x.id == daire.id).Take(1).FirstOrDefault();
                    d.blokno = daire.blokno;
                    d.dairedurum = daire.dairedurum;
                    d.daireno = daire.daireno;
                    d.DaireSayibi.ad = DaireSayibi.ad;
                    d.DaireSayibi.durum = DaireSayibi.durum;
                    d.DaireSayibi.email = DaireSayibi.email;
                    d.DaireSayibi.soyad = DaireSayibi.soyad;
                    d.DaireSayibi.tel = DaireSayibi.tel;
                    d.DaireSayibi.yetkiId = DaireSayibi.yetkiId;
                    d.kat = daire.kat;
                    context.SaveChanges();
                    if (resim != null)
                    {
                        AracController resimekle = new AracController(Server.MapPath("~/"));
                        resimekle.doludaireresimguncelle(resim, d.id);
                    }
                }
                else
                    ViewBag.hata = "Eksik Bilgi Girdiniz";
            }
            catch
            {
                ViewBag.hata = "İşleminiz Gercekleştirilemdi";
            }

            return RedirectToAction("Tumdaireler", "Yonetim", new { sayfa = 1 });
        }
        [KontrolController]
        public ActionResult Tumdaireler(int sayfa, string filitre = "")
        {
            IEnumerable<Daire> daireler = null;
            try
            {
                if (filitre.ToLower() == "dolu" || filitre.ToLower() == "boş")
                {
                    bool bosdolu = false;
                    if (filitre.ToLower() == "boş")
                        bosdolu = true;
                    daireler = context.Daire.Where(x => x.bosdolu == bosdolu).ToList().ToPagedList(sayfa, 10);
                    return View(daireler);
                }
                else if (filitre != "")
                {
                    daireler = context.Daire.Where(x => x.blokno == filitre).ToList().ToPagedList(sayfa, 10);
                    if (daireler.Count() == 0)
                        daireler = context.Daire.Where(x => x.daireno == filitre).ToList().ToPagedList(sayfa, 10);
                    return View(daireler);
                }


                daireler = context.Daire.ToList().ToPagedList(sayfa, 10);
            }
            catch
            {
                ViewBag.hata = "İşlem Gercekleştirilemedi";
            }
            return View(daireler);
        }
        [KontrolController]
        public ActionResult DaireDetay(int id)
        {
            Daire daire = null;
            try
            {
                daire = context.Daire.Where(x => x.id == id).Take(1).FirstOrDefault();
            }
            catch
            {
                ViewBag.hata = "İşlem Gercekleştirilemedi";
            }
            return View(daire);
        }

        //[KontrolController]
        //public ActionResult Daireil(int id, string islem)
        //{
        //    try
        //    {
        //        if (id > 0 && islem == "sil")
        //        {
        //            Daire sil = context.Daire.Where(x => x.id == id).FirstOrDefault();
        //            if (sil.bosdolu == true)
        //                context.Galeri.Remove(context.Galeri.Where(x => x.daireId == id).Take(1).FirstOrDefault());
        //            else
        //            {
        //                if (sil.Galeri1.Count>0)
        //                    context.Galeri.Remove(context.Galeri.Where(x => x.yk == id).Take(1).FirstOrDefault());
        //                if (sil.Galeri.Count > 0)
        //                    context.Galeri.Remove(context.Galeri.Where(x => x.daireId == id).Take(1).FirstOrDefault());
        //                DaireSayibi ds = context.DaireSayibi.Where(x => x.daireId == id).Take(1).FirstOrDefault();
        //                if(ds!=null)
        //                    context.DaireSayibi.Remove(ds);

        //            }
        //            context.Daire.Remove(context.Daire.Where(x => x.id == id).Take(1).FirstOrDefault());
        //            context.SaveChanges();
        //        }
        //        else
        //        {
        //            ViewBag.hata = "Silme İşlemi Başarısız";
        //        }
        //    }
        //    catch
        //    {
        //        ViewBag.hata = "Silme İşlemi Başarısız";
        //    }
        //    return RedirectToAction("Tumdaireler", "Yonetim", new { sayfa = 1 });
        //}

        #endregion

        #region Calisan
        [KontrolController]
        public ActionResult Calisanlar()
        {
            IEnumerable<Calisanlar> calisan = null;
            try
            {
                calisan = context.Calisanlar;
            }
            catch (Exception)
            {
                ViewBag.hata = "İşlem Gercekleştirilemedi";
            }
            return View(calisan);
        }

        [HttpGet]
        [KontrolController]
        public ActionResult CalisanEkleDüzenle(int id)
        {
            try
            {
                if (id > 0)
                    return View(context.Calisanlar.Where(x => x.id == id).Take(1).FirstOrDefault());
            }
            catch
            {
                ViewBag.hata = "İşlem Gercekleştirilemedi";
            }


            return View();
        }
        [HttpPost]
        [KontrolController]
        public ActionResult CalisanEkleDüzenle(Calisanlar calisan, string sef, HttpPostedFileBase fotoraf)
        {
            bool sef1 = false;
            if (sef == "on")
                sef1 = true;

            try
            {
                if (calisan.id > 0 && calisan.adi != null && calisan.sorumlubloklar != null && calisan.soyadi != null && calisan.tel != null)
                {
                    selcukunikonutlari.Models.Calisanlar guncelle = context.Calisanlar.Where(x => x.id == calisan.id).Take(1).FirstOrDefault();
                    guncelle.sorumlubloklar = calisan.sorumlubloklar;
                    guncelle.soyadi = calisan.soyadi;
                    guncelle.tel = calisan.tel;
                    guncelle.adi = calisan.adi;
                    guncelle.sef = sef1;
                    context.SaveChanges();
                    if (fotoraf != null)
                    {
                        AracController resimkaydet = new AracController(Server.MapPath("~/"));
                        resimkaydet.ResimKaydet(fotoraf, calisan.adi + calisan.soyadi, calisan.id, "personel");
                    }

                    return RedirectToAction("Calisanlar", "Yonetim");
                }
                else if (fotoraf != null && calisan.adi != null && calisan.sorumlubloklar != null && calisan.soyadi != null && calisan.tel != null)
                {
                    calisan.sef = sef1;
                    context.Calisanlar.Add(calisan);
                    context.SaveChanges();
                    calisan = context.Calisanlar.Where(x => x.tel == calisan.tel).Take(1).FirstOrDefault();

                    AracController resimkaydet = new AracController(Server.MapPath("~/"));
                    resimkaydet.ResimKaydet(fotoraf, calisan.adi + calisan.soyadi, "personel", calisan.id);
                }
                else
                    ViewBag.hata = "Eksik Bilgi Girdiniz";
            }
            catch
            {
                ViewBag.hata = "İşlem Gercekleştirilemedi";
            }
            return RedirectToAction("Calisanlar", "Yonetim");
        }
        [KontrolController]
        public ActionResult CalisanSil(int id, string islem)
        {
            try
            {
                if (id > 0 && islem == "sil")
                {
                    context.Galeri.Remove(context.Galeri.Where(x => x.calisanId == id).FirstOrDefault());
                    context.Calisanlar.Remove(context.Calisanlar.Where(y => y.id == id).FirstOrDefault());
                    //context.Database.ExecuteSqlCommand("delete from Calisanlar where id={0}", id);
                    context.SaveChanges();
                }
                else
                    ViewBag.hata = "Silme İşlemi Başarısız";
            }
            catch
            {
                ViewBag.hata = "Silme İşlemi Başarısız";
            }
            return RedirectToAction("Calisanlar", "Yonetim");
        }
        #endregion

        #region duyuru islemleri
        [KontrolController]
        public ActionResult TumDuyurular(int sayfa, int durum = 0, string baslik = "", string tarih = "")
        {
            //int sayfa1 = int.TryParse(sayfa, out sayfa1) == true ? sayfa1 : 1;
            try
            {
                DateTime dt = DateTime.TryParse(tarih, out dt) == true ? dt : DateTime.Now.Date;
                if (durum > 0 && baslik == "" && tarih == "")
                {
                    sayfa = 1;
                    return View(context.Duyuru.Where(x => x.durum == durum).ToList().ToPagedList(sayfa, 10));
                }
                else if (baslik != "" && tarih == "" && durum == 0)
                {
                    sayfa = 1;
                    return View(context.Duyuru.Where(x => x.baslik.Contains(baslik) == true).ToList().ToPagedList(sayfa, 10));
                }
                else if (tarih != "" && baslik == "" && durum == 0)
                {
                    sayfa = 1;
                    return View(context.Duyuru.Where(x => x.tarih == dt.Date).ToList().ToPagedList(sayfa, 10));
                }
                return View(context.Duyuru.ToList().ToPagedList(sayfa, 10));
            }
            catch (Exception)
            {
                ViewBag.hata = "İşlem Gercekleştirilemedi";
            }
            return null;
        }
        [KontrolController]
        [HttpGet]
        public ActionResult DuyuruEkle(int id)
        {
            try
            {
                if (id > 0)
                    return View(context.Duyuru.Take(1).FirstOrDefault());
            }
            catch
            {
                ViewBag.hata = "İşlem Gerçekleştirilemedi";
            }

            return View();
        }
        [HttpPost]
        [KontrolController]
        [ValidateInput(false)]
        public ActionResult DuyuruEkle(Duyuru duyuru)
        {
            try
            {
                if (duyuru.id == 0 && duyuru.icerik != null && duyuru.baslik != null && duyuru.durum > 0)
                {
                    duyuru.tarih = DateTime.Now.Date;
                    context.Duyuru.Add(duyuru);
                    context.SaveChanges();
                }
                else if (duyuru.id > 0 && duyuru.icerik != null && duyuru.baslik != null && duyuru.durum > 0)
                {
                    Duyuru duyuru2 = context.Duyuru.Where(x => x.id == duyuru.id).Take(1).FirstOrDefault();
                    duyuru2.baslik = duyuru.baslik;
                    duyuru2.durum = duyuru.durum;
                    duyuru2.icerik = duyuru.icerik;
                    duyuru2.tarih = DateTime.Now.Date;
                    context.SaveChanges();
                }
                else
                    ViewBag.hata = "Eksik İçerik Girdiniz";
            }
            catch (Exception)
            {
                ViewBag.hata = "İşlem Gercekleştirilemdi";
            }
            return RedirectToAction("TumDuyurular", "Yonetim", new { sayfa = 1 });
        }
        [KontrolController]
        public ActionResult Duyuruil(int id)
        {
            try
            {
                if (id > 0)
                {
                    context.Duyuru.Remove(context.Duyuru.Where(x => x.id == id).Take(1).FirstOrDefault());
                    context.SaveChanges();
                }
            }
            catch
            {
                ViewBag.hata = "Silme İşlemi Başarısız";
            }
            return RedirectToAction("TumDuyurular", "Yonetim", new { sayfa = 1 });
        }
        #endregion

        #region galeri islemleri
        [KontrolController]
        public ActionResult TumGaleri()
        {
            try
            {
                return View(context.Galeri.Where(x => x.galeri1 == true).ToList());
            }
            catch (Exception)
            {
                ViewBag.hata = "İşlem Gerçekleştirilemedi";
            }
            return null;
        }
        [HttpGet]
        [KontrolController]
        public ActionResult GaleriEkleDuzenle(int id)
        {
            try
            {
                if (id > 0)
                    return View(context.Galeri.Where(x => x.id == id).Take(1).FirstOrDefault());

            }
            catch
            {
                ViewBag.hata = "İşlem Gerçekleştirilemedi";
            }
            return View();
        }
        [HttpPost]
        [KontrolController]
        public ActionResult GaleriEkleDuzenle(int id, string aciklama, string slider, string arkaplan, string vaziyet, HttpPostedFileBase tekresim = null, HttpPostedFileBase[] cokluresim = null)
        {
            bool slider1 = false, arkaplan1 = false, vaziyet1 = false;
            if (slider == "on")
                slider1 = true;
            if (arkaplan == "on")
                arkaplan1 = true;
            if (vaziyet == "on")
                vaziyet1 = true;
            AracController resimekle = new AracController(Server.MapPath("~/"));
            try
            {
                if (id > 0)
                {
                    if (tekresim != null)
                        resimekle.galeriguncelle(tekresim, aciklama, slider1, arkaplan1, id, vaziyet1);
                    else
                    {
                        Galeri g = context.Galeri.Where(x => x.id == id).Take(1).FirstOrDefault();
                        g.slider = slider1;
                        g.arkaplan = arkaplan1;
                        g.vaziyet = vaziyet1;
                        g.aciklama = aciklama;
                        context.SaveChanges();
                    }

                }
                else if (tekresim != null && cokluresim[0] == null)
                    resimekle.galeriyeekle(tekresim, aciklama, slider1, arkaplan1, vaziyet1);
                else if (cokluresim[0] != null && tekresim == null)
                    resimekle.galeriyeekle(cokluresim, aciklama, slider1, arkaplan1, vaziyet1);
                else
                    ViewBag.hata = "Eksik İçerik Girdiniz";
                    
            }
            catch (Exception)
            {
                ViewBag.hata = "İşlem Gerçekleştirilemedi";
            }

            return RedirectToAction("TumGaleri", "Yonetim");
        }

        [KontrolController]
        public ActionResult Galerisil(int id)
        {
            try
            {
                if (id > 0)
                {
                    Galeri silinecek = context.Galeri.Where(x => x.id == id).Take(1).FirstOrDefault();
                    if (System.IO.File.Exists(silinecek.resimyol.Replace("http://selcukunikonutlari.net",Server.MapPath("~"))))
                    {
                        System.IO.File.Delete(silinecek.resimyol.Replace("http://selcukunikonutlari.net", Server.MapPath("~")));
                    }
                    context.Galeri.Remove(silinecek);
                    context.SaveChanges();
                }
            }
            catch
            {
                ViewBag.hata = "Silme İşlemi Gerçekleştirilemedi";
            }
            return RedirectToAction("TumGaleri", "Yonetim");
        }

        #endregion

        #region iletisim

        public ActionResult TumIletisim()
        {
            try
            {
                return View(context.Iletisim.OrderByDescending(x => x.id).ToList());
            }
            catch (Exception)
            {
                ViewBag.hata = "İşlem Gercekleştirilemedi";
            }
            return null;
        }

        public ActionResult IletisimOkundu(int id)
        {

            try
            {

                if (id > 0)
                {
                    Iletisim okundu = context.Iletisim.Where(x => x.id == id).Take(1).FirstOrDefault();
                    okundu.durum = true;
                    context.SaveChanges();

                }
            }
            catch (Exception)
            {
                ViewBag.hata = "İşlem Gercekleştirilemedi";
            }
            return RedirectToAction("TumIletisim", "Yonetim");
        }

        public ActionResult Iletisimil(int id)
        {

            try
            {
                if (id > 0)
                {
                    context.Iletisim.Remove(context.Iletisim.Where(x => x.id == id).Take(1).FirstOrDefault());
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                ViewBag.hata = "İşlem Gercekleştirilemedi";
            }
            return RedirectToAction("TumIletisim", "Yonetim");
        }
        #endregion

        #region muhasebe islemleri
        [MuhasebeciGirisKontrol]
        [HttpGet]
        public ActionResult AidatEkle()
        {
            return View();
        }
        [MuhasebeciGirisKontrol]
        [HttpPost]
        public ActionResult AidatEkle(HttpPostedFileBase exel)
        {
            string connString;
            string[] uzantilar = { ".xls", ".xlsx", ".csv" };
            string yol = Server.MapPath("~/Content/buaykiaidat/") + "aidat";

            if (System.IO.Path.GetExtension(exel.FileName) == uzantilar[0])
            {
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + yol + uzantilar[0] + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                yol = yol + uzantilar[0];
            }

            else if (System.IO.Path.GetExtension(exel.FileName) == uzantilar[1])
            {
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + yol + uzantilar[1] + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                yol = yol + uzantilar[1];
            }

            else if (System.IO.Path.GetExtension(exel.FileName) == uzantilar[2])
            {
                connString = yol + uzantilar[2];
                yol = yol + uzantilar[2];
            }
            else
                connString = "";
            exel.SaveAs(yol);

            Utility ekle = new Utility();
            ekle.Daire_sahibi_ekle(connString);
            ekle.Aidat_ekle(connString);
            ViewBag.hata = "İşlem Gerçekleştirildi";
            try
            {
                
            }
            catch (Exception)
            {
                ViewBag.hata = "İşlem Gercekleştirilemedi";
            }
            return View();
           
        }
        [MuhasebeciGirisKontrol]
        [HttpGet]
        public ActionResult BelirsizOdemeler()
        {
            return View();
        }
        [MuhasebeciGirisKontrol]
        [HttpPost]
        public ActionResult BelirsizOdemeler(HttpPostedFileBase exel)
        {
            try
            {
                string[] uzantilar = { ".xls", ".xlsx", ".csv" };
                string yol1 = Server.MapPath("~/Content/buaykiaidat/") + "belirsizodemeler" + uzantilar[0];
                string yol2 = Server.MapPath("~/Content/buaykiaidat/") + "belirsizodemeler" + uzantilar[1];
                string yol3 = Server.MapPath("~/Content/buaykiaidat/") + "belirsizodemeler" + uzantilar[2];

                if (System.IO.File.Exists(yol1))
                    System.IO.File.Delete(yol1);
                else if (System.IO.File.Exists(yol2))
                    System.IO.File.Delete(yol2);
                else if (System.IO.File.Exists(yol3))
                    System.IO.File.Delete(yol3);

                string uzanti = Path.GetExtension(exel.FileName);
                exel.SaveAs(Server.MapPath("~/Content/buaykiaidat/") + "belirsizodemeler" + uzanti);
                ViewBag.hata = "İşlem Gerçekleştirildi";
            }
            catch (Exception)
            {
                ViewBag.hata = "İşlem Gercekleştirilemedi";
            }
            return View();
        }
        [MuhasebeciGirisKontrol]
        [HttpGet]
        public ActionResult YapilanHarcamaKaydi()
        {
            return View();
        }
        [MuhasebeciGirisKontrol]
        [HttpPost]
        public ActionResult YapilanHarcamaKaydi(HttpPostedFileBase exel)
        {
            try
            {
                string[] uzantilar = { ".xls", ".xlsx", ".csv" };
                string yol1 = Server.MapPath("~/Content/buaykiaidat/") + "yapilanharcamakaydi" + uzantilar[0];
                string yol2 = Server.MapPath("~/Content/buaykiaidat/") + "yapilanharcamakaydi" + uzantilar[1];
                string yol3 = Server.MapPath("~/Content/buaykiaidat/") + "yapilanharcamakaydi" + uzantilar[2];

                if (System.IO.File.Exists(yol1))
                    System.IO.File.Delete(yol1);
                else if (System.IO.File.Exists(yol2))
                    System.IO.File.Delete(yol2);
                else if (System.IO.File.Exists(yol3))
                    System.IO.File.Delete(yol3);

                string uzanti = Path.GetExtension(exel.FileName);
                exel.SaveAs(Server.MapPath("~/Content/buaykiaidat/") + "yapilanharcamakaydi" + uzanti);
            }
            catch (Exception)
            {
                ViewBag.hata = "İşlem Gercekleştirilemedi";
            }
            return View();
        }
        [MuhasebeciGirisKontrol]
        [HttpGet]
        public ActionResult BankaBilgileri()
        {
            try
            {
                return View(context.BankaBilgileri.ToList());
            }
            catch
            {
                ViewBag.hata = "İşlem Gerçekleştirilemdi";
            }
            return null;
        }
        [MuhasebeciGirisKontrol]
        [HttpPost]
        public ActionResult BankaBilgileri(BankaBilgileri banka)
        {
            try
            {
                context.BankaBilgileri.Add(banka);
                context.SaveChanges();
            }
            catch (Exception)
            {
                ViewBag.hata = "İşlem Gercekleştirilemedi";
            }
            return View();
        }
        [MuhasebeciGirisKontrol]
        public ActionResult BankaSil(int id)
        {
            try
            {
                if (id > 0)
                {
                    context.BankaBilgileri.Remove(context.BankaBilgileri.Where(x => x.id == id).Take(1).FirstOrDefault());
                    context.SaveChanges();
                }
            }
            catch
            {
                ViewBag.hata = "İşlem Gerçekleştirilemdi";
            }
            return RedirectToAction("BankaBilgileri", "Yonetim");
        }
        #endregion


        public ActionResult Cikis()
        {
            Session.RemoveAll();
            return RedirectToAction("AnaSayfa", "index");
        }
    }
}
