using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using selcukunikonutlari.Models;

namespace selcukunikonutlari.Controllers.Araclar
{
    public class Utility
    {
        Model1 model = new Model1();

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    if (rows.Length > 1)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }

            }


            return dt;
        }

        public static DataTable ConvertXSLXtoDataTable(string strFilePath, string connString)
        {
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            try
            {
                oledbConn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sayfa1$]", oledbConn);
                OleDbDataAdapter da = new OleDbDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(dt);
            }
            catch
            {
            }
            finally
            {

                oledbConn.Close();
            }

            return dt;

        }

        public bool ConvertXSLXtoDataTable(string connString)
        {
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            oledbConn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sayfa1$]", oledbConn);
            OleDbDataReader dr = cmd.ExecuteReader();
            string ad, soyad,adsoyad;
            List<string> yoneticilerlist = model.Yonetici.Where(y => y.yetki == (int)EnumlarController.girisyetki.dairesakinleri).
                Select(x=> x.hesap_kodu+","+x.adi+","+x.soyadi).ToList();
            while (dr.Read())
            {
                adsoyad = dr[2].ToString().Trim();
                ad = dr[2].ToString().Replace(adsoyad.Split()[adsoyad.Split().Count()-1]," ").Trim();
                soyad = dr[2].ToString().Trim().Split(' ')[adsoyad.Split(' ').Count()-1];
                if (yoneticilerlist!=null && yoneticilerlist.Where(x => (x.IndexOf(dr[1].ToString())>=0 && x.IndexOf(ad)>=0 && x.IndexOf(soyad)>=0 )).Count() > 0)
                    continue;
                model.Yonetici.Add(new Yonetici() {
                    yetki = (int)EnumlarController.girisyetki.dairesakinleri,
                    adi = ad,
                    soyadi = soyad,
                    kulaniciadi = ad.Replace('-',' ').Trim(),
                    sifre = AracController.MD5eDonustur(soyad.Replace('-', ' ').Trim()),
                    hesap_kodu = dr[1].ToString()
                });
                

            }
            model.SaveChanges();
            try
            {
               
            }
            catch
            {
            }
            finally
            {

                oledbConn.Close();
            }
            return false;

        }

        public bool Daire_sahibi_ekle(string connString)
        {
            OleDbConnection baglanti = new OleDbConnection(connString);
            try
            {

                baglanti.Open();

                System.Data.DataTable dataSet = baglanti.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                string[] workSheetNames = new String[dataSet.Rows.Count];

                int a = 0;
                foreach (DataRow row in dataSet.Rows)
                {
                    workSheetNames[a] = row["TABLE_NAME"].ToString().Replace("‘", "");
                    a++;
                }
                //OleDbConnection oledbConn = new OleDbConnection(connString);
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                //oledbConn.Open();

                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + workSheetNames[0] + "]", baglanti);
                OleDbDataReader dr = cmd.ExecuteReader();
                string ad, soyad, adsoyad;
                List<string> yoneticilerlist = model.Yonetici.Where(y => y.yetki == (int)EnumlarController.girisyetki.dairesakinleri).
                    Select(x => x.hesap_kodu + "," + x.adi + "," + x.soyadi).ToList();

                while (dr.Read())
                {
                    if (dr[0].ToString().Length > 0 && dr[1].ToString().Length > 0 && dr[2].ToString().Length > 0 && dr[1].ToString().IndexOf('.') > 0)
                    {
                        adsoyad = dr[2].ToString().Trim();
                        ad = dr[2].ToString().Replace(adsoyad.Split()[adsoyad.Split().Count() - 1], " ").Trim();
                        soyad = dr[2].ToString().Trim().Split(' ')[adsoyad.Split(' ').Count() - 1];
                        if (yoneticilerlist != null && yoneticilerlist.Where(x => (x.IndexOf(dr[1].ToString()) >= 0)).Count() > 0)
                            continue;
                        model.Yonetici.Add(new Yonetici()
                        {
                            yetki = (int)EnumlarController.girisyetki.dairesakinleri,
                            adi = ad.Replace('-', ' ').Trim(),
                            soyadi = soyad.Replace('-', ' ').Trim(),
                            kulaniciadi = dr[0].ToString().Trim().ToLower() + "-" + dr[1].ToString().Split('.')[2],
                            sifre = AracController.MD5eDonustur(dr[0].ToString().Trim().ToLower() + "-" + dr[1].ToString().Split('.')[2]),
                            hesap_kodu = dr[1].ToString(),
                            sifre_durum = false,
                            daire = dr[0].ToString().ToLower()

                        });
                    }
                }
                model.SaveChanges();
            }
            catch
            {
                return false;
            }
            finally
            {

                //oledbConn.Close();
                baglanti.Close();
            }
            return true;
        }

        public bool Aidat_ekle(string connString)
        {

            OleDbConnection baglanti = new OleDbConnection(connString);

            baglanti.Open();

            System.Data.DataTable dataSet = baglanti.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            string[] workSheetNames = new String[dataSet.Rows.Count];

            int a = 0;
            foreach (DataRow row in dataSet.Rows)
            {
                workSheetNames[a] = row["TABLE_NAME"].ToString().Replace("‘", "");
                a++;
            }
            //OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            //oledbConn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM ["+workSheetNames[0]+"]", baglanti);
            OleDbDataReader dr = cmd.ExecuteReader();
           
            string isim = dr.GetName(6).ToString();
            List<AidatBorc> liste = model.AidatBorc.Where(x=>x.ay==isim).ToList();
            
            int id = 0;
            while (dr.Read())
            {

                if (liste.Where(x => x.hesap_kodu == dr[1].ToString()).Count()>0)
                {
                    liste.Where(x => x.hesap_kodu == dr[1].ToString()).FirstOrDefault().yil = DateTime.Now.Year.ToString();
                    liste.Where(x => x.hesap_kodu == dr[1].ToString()).FirstOrDefault().buaykiborc = dr[5].ToString() == "" ? 0 : Convert.ToDecimal(dr[5].ToString());
                    liste.Where(x => x.hesap_kodu == dr[1].ToString()).FirstOrDefault().geneltoplam = dr[7].ToString() == "" ? 0 : Convert.ToDecimal(dr[7].ToString());
                    liste.Where(x => x.hesap_kodu == dr[1].ToString()).FirstOrDefault().gecikmezammi = dr[4].ToString() == "" ? 0 : Convert.ToDecimal(dr[4].ToString());
                    liste.Where(x => x.hesap_kodu == dr[1].ToString()).FirstOrDefault().borc_bakiye = dr[3].ToString() == "" ? 0 : Convert.ToDecimal(dr[3].ToString());
                    liste.Where(x => x.hesap_kodu == dr[1].ToString()).FirstOrDefault().ay = isim;
                    liste.Where(x => x.hesap_kodu == dr[1].ToString()).FirstOrDefault().aidat = dr[6].ToString() == "" ? 0 : Convert.ToDecimal(dr[6].ToString());
                }
                else
                {
                    model.AidatBorc.Add(new AidatBorc()
                    {
                        hesap_kodu = dr[1].ToString(),
                        aidat = dr[6].ToString() == "" ? 0 : Convert.ToDecimal(dr[6].ToString()),
                        ay = isim,
                        borc_bakiye = dr[3].ToString() == "" ? 0 : Convert.ToDecimal(dr[3].ToString()),
                        gecikmezammi = dr[4].ToString() == "" ? 0 : Convert.ToDecimal(dr[4].ToString()),
                        geneltoplam = dr[7].ToString() == "" ? 0 : Convert.ToDecimal(dr[7].ToString()),
                        buaykiborc = dr[5].ToString() == "" ? 0 : Convert.ToDecimal(dr[5].ToString()),
                        yil = DateTime.Now.Year.ToString()

                    });
                }
                
            }
            model.SaveChanges();
            try
            {

            }
            catch
            {
            }
            finally
            {

                //oledbConn.Close();
                baglanti.Close();
            }
            return false;
        }
    }
}