using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Drawing;
using selcukunikonutlari.Models;
using System.IO;
using System.Web.Helpers;

namespace selcukunikonutlari.Controllers.Araclar
{
    public class AracController : Controller
    {
        //
        // GET: /Arac/
        private string _yol;
        public AracController(string yol)
        {
            _yol = yol;
        }
        public ActionResult Index()
        {
            return View();
        }

        Model1 contex = new Model1();

        public static string MD5eDonustur(string metin)
        {
            MD5CryptoServiceProvider pwd = new MD5CryptoServiceProvider();
            return Sifrele(metin, pwd);
        }

        private static string Sifrele(string metin, HashAlgorithm alg)
        {
            byte[] byteDegeri = System.Text.Encoding.UTF8.GetBytes(metin);
            byte[] sifreliByte = alg.ComputeHash(byteDegeri);
            return Convert.ToBase64String(sifreliByte);
        }

        /// <summary>
        /// coklu resim eklemede kullan
        /// </summary>
        /// <param name="resimler"></param>
        /// <param name="resimad"></param>
        /// <param name="neresmi">personelmi yoksa dairemi yoksa galeri içinmi</param>
        /// <param name="id"></param>
        public void ResimKaydet(HttpPostedFileBase[] resimler, string resimad, string neresmi, int id)
        {
            byte sayac = 0;
            int resimadeti = resimadeti = resimler.Count();
            while (resimadeti > sayac)
            {
                //        Image orjres = Image.FromStream(resimler[sayac].InputStream);
                //        Bitmap resim = new Bitmap(orjres);
                //        resim.Save(_yol+"Content/images/daireresimleri/") + resimad + sayac + System.IO.Path.GetExtension(resimler[sayac].FileName));



                //WebImage resimduzenle = new WebImage(resimler[sayac].InputStream);
                //resimduzenle.FileName = resimad + sayac;
                //byte[] resimdizi = resimduzenle.GetBytes();
                //http://selcukunikonutlari.net/Content/images/galeriresimleri/22-01-2018-20-15-53.jpg
                Galeri galeriekle = new Galeri();
                if (neresmi == "daire")
                {
                    resimler[sayac].SaveAs(_yol+ "Content\\images\\daireresimleri\\" + resimad + System.IO.Path.GetExtension(resimler[sayac].FileName));
                    galeriekle.resimyol = "http://selcukunikonutlari.net/Content/images/daireresimleri/" + resimad  + System.IO.Path.GetExtension(resimler[sayac].FileName);
                    galeriekle.daireId = id;
                }
                    
                else if (neresmi == "personel")
                {
                    var yuklemeYeri = _yol + "Content\\images\\daireresimleri\\personelresimleri\\"+ resimad + sayac + System.IO.Path.GetExtension(resimler[sayac].FileName);
                    resimler[sayac].SaveAs(yuklemeYeri);
                    galeriekle.resimyol = "http://selcukunikonutlari.net/Content/images/personelresimleri/" + resimad + sayac + System.IO.Path.GetExtension(resimler[sayac].FileName);
                    galeriekle.calisanId = id;
                }
                    
                contex.Galeri.Add(galeriekle);

                sayac++;
            }
            contex.SaveChanges();
        }
        /// <summary>
        /// tek resim eklemek için kullna
        /// </summary>
        /// <param name="resim"></param>
        /// <param name="resimad"></param>
        /// <param name="neresmi"></param>
        /// <param name="id"></param>
        public void ResimKaydet(HttpPostedFileBase resim, string resimad, string neresmi, int id)
        {
           

            Galeri galeriekle = new Galeri();

            string yolson = resimad + System.IO.Path.GetExtension(resim.FileName);
            if (neresmi == "daire")
            {
                var yuklemeYeri = Path.Combine(_yol+"Content/images/daireresimleri/",yolson );
                resim.SaveAs(yuklemeYeri);
                galeriekle.resimyol ="http://selcukunikonutlari.net/Content/images/daireresimleri/"+yolson ;
                galeriekle.daireId = id;
            }
            else if (neresmi == "personel")
            {
                var yuklemeYeri = Path.Combine(_yol+"Content/images/personelresimleri/", resimad + System.IO.Path.GetExtension(resim.FileName));
                resim.SaveAs(yuklemeYeri);
                galeriekle.resimyol = "http://selcukunikonutlari.net/Content/images/personelresimleri/"+yolson;
                galeriekle.calisanId = id;
            }

            contex.Galeri.Add(galeriekle);
            contex.SaveChanges();
        }
        /// <summary>
        /// sadece galeriye coklu ekler
        /// </summary>
        /// <param name="resimler"></param>
        /// <param name="neredekullanilacak"></param>
        /// <param name="aciklama"></param>
        public void ResimKaydet(HttpPostedFileBase[] resimler, byte neredekullanilacak, string aciklama)
        {
            byte sayac = 0;
            int resimadeti = resimadeti = resimler.Count();
            while (resimadeti > sayac)
            {

                Galeri galeriekle = new Galeri();
                string yolson = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + sayac + System.IO.Path.GetExtension(resimler[sayac].FileName);
                var yuklemeYeri = Path.Combine(_yol+"Content/images/galeriresimleri/", yolson);
                resimler[sayac].SaveAs(yuklemeYeri);
                galeriekle.resimyol = "http://selcukunikonutlari.net/Content/images/galeriresimleri/"+yolson;
                galeriekle.aciklama = aciklama;
                galeriekle.galeri1 = true;
                contex.Galeri.Add(galeriekle);
                contex.SaveChanges();

                sayac++;
            }
        }
        /// <summary>
        /// galeriye coklu resim eklemek için kullanılır
        /// </summary>
        /// <param name="resim"></param>
        /// <param name="resimad"></param>
        /// <param name="neresmi"></param>
        /// <param name="id"></param>
        /// <param name="aciklama"></param>
        /// <param name="gozukecekyer"></param>
        public void ResimKaydet(HttpPostedFileBase resim, string resimad, string aciklama, byte gozukecekyer)
        {
            Galeri galeriekle = new Galeri();
            string yolson = resimad + System.IO.Path.GetExtension(resim.FileName);
            var yuklemeYeri = Path.Combine(_yol+"Content/images/galeriresimleri/", yolson);
            resim.SaveAs(yuklemeYeri);
            galeriekle.resimyol = "http://selcukunikonutlari.net/Content/images/galeriresimleri/"+yolson;
            galeriekle.aciklama = aciklama;
            galeriekle.galeri1 = true;

            contex.Galeri.Add(galeriekle);
            contex.SaveChanges();
        }
        /// <summary>
        /// Galerideki resmi güncelleme
        /// </summary>
        /// <param name="resim"></param>
        /// <param name="resimad"></param>
        /// <param name="id"></param>
        /// <param name="neidsi"></param>
        public void ResimKaydet(HttpPostedFileBase resim, string resimad, int id, string neidsi)
        {

            if (neidsi == "galeri")
            {
                Galeri guncelle = contex.Galeri.Where(x => x.id == id).Take(1).FirstOrDefault();
                if (System.IO.File.Exists(guncelle.resimyol.Replace("http://selcukunikonutlari.net", _yol)))
                    System.IO.File.Delete(guncelle.resimyol.Replace("http://selcukunikonutlari.net", _yol));
                string yolson = resimad + System.IO.Path.GetExtension(resim.FileName);
                var yuklemeYeri = Path.Combine(_yol+"Content/images/galeriresimleri/",yolson);
                resim.SaveAs(yuklemeYeri);
                guncelle.resimyol = "http://selcukunikonutlari.net/Content/images/galeriresimleri/";
                contex.SaveChanges();
            }
            else if (neidsi == "personel")
            {
                Galeri guncelle = contex.Galeri.Where(x => x.calisanId == id).Take(1).FirstOrDefault();
                if (System.IO.File.Exists(guncelle.resimyol.Replace("http://selcukunikonutlari.net", _yol)))
                    System.IO.File.Delete(guncelle.resimyol.Replace("http://selcukunikonutlari.net",_yol));
                string yolson = resimad + System.IO.Path.GetExtension(resim.FileName);
                var yuklemeYeri = Path.Combine(_yol+"Content/images/personelresimleri/",yolson );
                resim.SaveAs(yuklemeYeri);
                guncelle.resimyol = "http://selcukunikonutlari.net/Content/images/personelresimleri/"+yolson;
                contex.SaveChanges();
            }
            else if (neidsi == "daire")
            {
                Galeri guncelle = contex.Galeri.Where(x => x.daireId == id).Take(1).FirstOrDefault();
                if (System.IO.File.Exists(guncelle.resimyol.Replace("http://selcukunikonutlari.net",_yol)))
                    System.IO.File.Delete(guncelle.resimyol.Replace("http://selcukunikonutlari.net",_yol));
                string yolson = resimad + System.IO.Path.GetExtension(resim.FileName);
                var yuklemeYeri = Path.Combine(_yol+"Content/images/daireresimleri/", yolson);
                resim.SaveAs(yuklemeYeri);
                guncelle.resimyol = "http://selcukunikonutlari.net/Content/images/daireresimleri/"+yolson;
                contex.SaveChanges();
            }
        }


        public void galeriyeekle(HttpPostedFileBase resim, string aciklama, bool slider, bool arkaplan, bool vaziyet)
        {

            Galeri galeri = new Galeri();
            galeri.aciklama = aciklama;
            galeri.arkaplan = arkaplan;
            galeri.slider = slider;
            galeri.galeri1 = true;
            galeri.vaziyet = vaziyet;
            string yolson = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + System.IO.Path.GetExtension(resim.FileName);
            var yuklemeYeri = Path.Combine(_yol+"Content\\images\\galeriresimleri\\",yolson);
            WebImage resimkaydet = new WebImage(resim.InputStream);
            resimkaydet.Save(yuklemeYeri);
            //resim.SaveAs(_yol + "Content\\images\\galeriresimleri\\"+ DateTime.Now.ToString() + System.IO.Path.GetExtension(resim.FileName));
            galeri.resimyol = "http://selcukunikonutlari.net/Content/images/galeriresimleri/"+yolson;


            contex.Galeri.Add(galeri);
            contex.SaveChanges();
        }

        public void galeriyeekle(HttpPostedFileBase[] resim, string aciklama, bool slider, bool arkaplan, bool vaziyet)
        {
            int boyut = resim.Count();
            for (int i = 0; i < boyut; i++)
            {
                Galeri galeri = new Galeri();
                string yolson = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + i + System.IO.Path.GetExtension(resim[i].FileName);
                var yuklemeYeri = Path.Combine(_yol+"Content/images/galeriresimleri/", yolson);
                resim[i].SaveAs(yuklemeYeri);
                galeri.resimyol = "http://selcukunikonutlari.net/Content/images/galeriresimleri/"+yolson;
                galeri.aciklama = aciklama;
                galeri.arkaplan = arkaplan;
                galeri.slider = slider;
                galeri.vaziyet = vaziyet;
                galeri.galeri1 = true;
                contex.Galeri.Add(galeri);
                contex.SaveChanges();
            }
        }

        public void galeriguncelle(HttpPostedFileBase resim, string aciklama, bool slider, bool arkaplan, int id, bool vaziyet)
        {
            string yolson = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + System.IO.Path.GetExtension(resim.FileName);
            var yuklemeYeri = Path.Combine(_yol+"Content/images/galeriresimleri/", yolson);
            resim.SaveAs(yuklemeYeri);

            Galeri galeri = contex.Galeri.Where(x => x.id == id).Take(1).FirstOrDefault();
            if (System.IO.File.Exists(galeri.resimyol.Replace("http://selcukunikonutlari.net",_yol)))
            {
                System.IO.File.Delete(galeri.resimyol.Replace("http://selcukunikonutlari.net",_yol));
            }
            galeri.aciklama = aciklama;
            galeri.arkaplan = arkaplan;
            galeri.slider = slider;
            galeri.galeri1 = true;
            galeri.vaziyet = vaziyet;
            galeri.resimyol = "http://selcukunikonutlari.net/Content/images/galeriresimleri/"+yolson;
            contex.SaveChanges();
        }

        
        public void doludaireresimekle(HttpPostedFileBase resim,int id)
        {
            string yolson = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + System.IO.Path.GetExtension(resim.FileName);
            var yuklemeYeri = Path.Combine(_yol + "Content\\images\\galeriresimleri\\",yolson );
            resim.SaveAs(yuklemeYeri);
            Galeri g = new Galeri();
            g.yk = id;
            g.resimyol = "http://selcukunikonutlari.net/Content/images/galeriresimleri/"+yolson;
            g.daireId = id;
            contex.Galeri.Add(g);
            contex.SaveChanges();
        }
        public void doludaireresimguncelle(HttpPostedFileBase resim, int id)
        {
           

            Galeri g = contex.Galeri.Where(x => x.yk == id).Take(1).FirstOrDefault();
            if (System.IO.File.Exists(g.resimyol.Replace("http://selcukunikonutlari.net",_yol)))
            {
                System.IO.File.Delete(g.resimyol.Replace("http://selcukunikonutlari.net",_yol));
            }
            string yolson = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + System.IO.Path.GetExtension(resim.FileName);
            var yuklemeYeri = Path.Combine(_yol + "Content\\images\\galeriresimleri\\",yolson);
            resim.SaveAs(yuklemeYeri);
            g.resimyol = "http://selcukunikonutlari.net/Content/images/galeriresimleri/"+yolson;
            g.yk = id;
            g.daireId = id;
            contex.SaveChanges();
        }
        

    }
}
