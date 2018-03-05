using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace selcukunikonutlari.Controllers.Araclar
{
    public class EnumlarController : Controller
    {
        //
        // GET: /Enumlar/

        public ActionResult Index()
        {
            return View();
        }
        enum aylar
        {
            [Description("ocak")]
            ocak,
            [Description("subat")]
            subat,
            [Description("Mart")]
            Mart,
            [Description("Nisan")]
            Nisan,
            [Description("Mayis")]
            Mayis,
            [Description("Haziran")]
            Haziran,
            [Description("Temmuz")]
            Temmuz,
            [Description("Ağustos")]
            Agustos,
            [Description("Eylül")]
            Eylul,
            [Description("Ekim")]
            Ekim,
            [Description("Kasım")]
            Kasim,
            [Description("Aralık")]
            Aralik
        }
        public static string GetDescription(Enum value)
        {
            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                string strvalue = string.Empty;
                if (string.IsNullOrEmpty(strvalue))
                    strvalue = (attributes.Length > 0) ? attributes[0].Description : value.ToString();
                return strvalue;
            }
            catch
            {
                return null;
            }
        }

        public enum girisyetki
        {
            [Description("Yönetici")]
            yonetici=1,
            [Description("Muhasebeci")]
            muhasebe=2,
            [Description("Daire sakinleri")]
            dairesakinleri = 3
        }

        public enum dairedurum
        {
            [Description("Kiralık")]
            kiralik=1,
            [Description("Satlık")]
            satlik=2
        }

        public enum dairesayibdurum
        {
            [Description("Normal")]
            normal=1,
            [Description("Temsilci")]
            temsilci=2
        }

        public enum daireyetki
        {
            [Description("Normal")]
            normal = 1,
            [Description("Y.K.Başkanı")]
            yonetici = 2,
            [Description("Y.K Başkan Yardımcısı")]
            yardimci = 3,
            [Description("Y.K Muasip Üye")]
            uye = 4
        }

        public enum duyurudurum
        {
            [Description("Karar")]
            karar=1,
            [Description("Yapılan Harcamalar")]
            harcama=2
        }

    }
}
