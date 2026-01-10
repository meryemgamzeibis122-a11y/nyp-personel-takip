using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ödev534547378347587
{
    public class VeriDeposu
    {
        private static string connectionString = "Server=172.21.54.253;Database=PersonelTakip;Uid=26_132430005;Pwd=İnif123.";
        
        
        public MySqlConnection GetConnection()
        {

            return new MySqlConnection(connectionString);
        }
        public void PersonelEkle(string tc, string ad, string soyad, string kAd, string parola, string yetki)
        {

            using (MySqlConnection baglanti = GetConnection())
            {
                baglanti.Open();

                string sorgu = "INSERT INTO kullanici (ad, soyad, yetki, kullaniciadi, parola, tcno) VALUES (@p1, @p2, @p3, @p4, @p5, @p6)";

                using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
                {
                    komut.Parameters.AddWithValue("@p1", ad);
                    komut.Parameters.AddWithValue("@p2", soyad);
                    komut.Parameters.AddWithValue("@p3", yetki);
                    komut.Parameters.AddWithValue("@p4", kAd);
                    komut.Parameters.AddWithValue("@p5", parola);
                    komut.Parameters.AddWithValue("@p6", tc);

                    komut.ExecuteNonQuery();
                }
            }
        }

        public DataTable TumPersonelleriGetir()
        {
            DataTable dt = new DataTable();
            string conString = "Server=172.21.54.253;Database=PersonelTakip;Uid=26_132430005;Pwd=İnif123";

            using (SqlConnection baglanti = new SqlConnection(conString))
            {

                string sorgu = "SELECT tcno, ad, soyad, kadi, yetki FROM kullanici";
                SqlDataAdapter da = new SqlDataAdapter(sorgu, baglanti);
                da.Fill(dt);
            }
            return dt;
        }
        public void PersonelSil(string tc)
        {
            using (MySqlConnection baglanti = GetConnection())
            {
                baglanti.Open();
                // tcno'ya göre silme sorgusu
                string sorgu = "DELETE FROM kullanici WHERE tcno = @p1";
                using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
                {
                    komut.Parameters.AddWithValue("@p1", tc);
                    komut.ExecuteNonQuery();
                }
            }
        }
        public void PersonelKaydet(string tc, string ad, string soyad, string cinsiyet, string mezuniyet, DateTime dogum, string gorev, string yer, int maas)
        {
            using (MySqlConnection baglanti = GetConnection())
            {
                baglanti.Open();

                string sorgu = "INSERT INTO personellerim (tcno, ad, soyad, cinsiyet, mezuniyet, dogum_tarihi, gorevi, gorev_yeri, maas) " +
                               "VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9)";

                using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
                {
                    komut.Parameters.AddWithValue("@p1", tc);
                    komut.Parameters.AddWithValue("@p2", ad);
                    komut.Parameters.AddWithValue("@p3", soyad);
                    komut.Parameters.AddWithValue("@p4", cinsiyet);
                    komut.Parameters.AddWithValue("@p5", mezuniyet);
                    komut.Parameters.AddWithValue("@p6", dogum); // Doğum Tarihi
                    komut.Parameters.AddWithValue("@p7", gorev);
                    komut.Parameters.AddWithValue("@p8", yer);
                    komut.Parameters.AddWithValue("@p9", maas);
                    komut.ExecuteNonQuery();
                }
            }
        }
        public void PersonelTablosundanSil(string tc)
        {
            using (MySqlConnection baglanti = GetConnection())
            {
                baglanti.Open();

                string sorgu = "DELETE FROM personellerim WHERE tcno = @p1";
                using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
                {
                    komut.Parameters.AddWithValue("@p1", tc);
                    int sonuc = komut.ExecuteNonQuery();

                    if (sonuc == 0)
                    {
                        throw new Exception("Bu TC numarasına ait kayıt bulunamadı!");
                    }
                }
            }
        }
        public void IzinVerisiKaydet(string tc, DateTime baslangic, DateTime bitis, string tur, string aciklama)
        {
            using (MySqlConnection baglanti = GetConnection())
            {
                baglanti.Open();
                // Sorguya onay_durumu sütununu da ekledik ki 'Beklemede' olarak başlasın
                string sorgu = "INSERT INTO izinlerim (tcno, izin_baslangic, izin_bitis, izin_turu, aciklama, onay_durumu) " +
                               "VALUES (@p1, @p2, @p3, @p4, @p5, 'Beklemede')";

                using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti)) // Komutu burada MySqlCommand olarak tanımlıyoruz
                {
                    komut.Parameters.AddWithValue("@p1", tc);
                    komut.Parameters.AddWithValue("@p2", baslangic);
                    komut.Parameters.AddWithValue("@p3", bitis);
                    komut.Parameters.AddWithValue("@p4", tur);
                    komut.Parameters.AddWithValue("@p5", aciklama);

                    // ExecuteNonQuery artık hata vermez çünkü 'komut' artık bir MySqlCommand
                    komut.ExecuteNonQuery();
                } // 'komut' burada otomatik kapanır
            } // 'baglanti' burada otomatik kapanır
        }
        public DataTable IzinleriGetir()
        {
            using (MySqlConnection baglanti = GetConnection())
            {
                string sorgu = "SELECT * FROM izinlerim";
                MySqlDataAdapter da = new MySqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        
        public DataTable TCYeGoreDetayGetir(string tc)
        {
            using (MySqlConnection baglanti = GetConnection())
            {
                baglanti.Open();
                
                string sorgu = "SELECT i.*, p.ad, p.soyad FROM izinlerim i INNER JOIN personellerim p ON i.tcno = p.tcno WHERE i.tcno = @p1 ORDER BY i.id DESC LIMIT 1";

                MySqlDataAdapter da = new MySqlDataAdapter(sorgu, baglanti);
                da.SelectCommand.Parameters.AddWithValue("@p1", tc);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

       
        public void IzinSil(int id)
        {
            using (MySqlConnection baglanti = GetConnection())
            {
                baglanti.Open();
                string sorgu = "DELETE FROM izinlerim WHERE id = @p1";
                MySqlCommand komut = new MySqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@p1", id);
                komut.ExecuteNonQuery();
            }
        }

       
        public void IzinGuncelle(int id, string tc, DateTime bas, DateTime bit, string tur, string aciklama)
        {
            using (MySqlConnection baglanti = GetConnection())
            {
                baglanti.Open();
                string sorgu = "UPDATE izinlerim SET tcno=@p1, izin_baslangic=@p2, izin_bitis=@p3, izin_turu=@p4, aciklama=@p5 WHERE id=@p6";
                MySqlCommand komut = new MySqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@p1", tc);
                komut.Parameters.AddWithValue("@p2", bas);
                komut.Parameters.AddWithValue("@p3", bit);
                komut.Parameters.AddWithValue("@p4", tur);
                komut.Parameters.AddWithValue("@p5", aciklama);
                komut.Parameters.AddWithValue("@p6", id);
                komut.ExecuteNonQuery();
            }
        }

        public void IzinOnaylaDAL(int id)
        {
            // MySQL kullandığın için MySqlCommand kullanıyoruz
            using (MySqlConnection baglanti = GetConnection())
            {
                string sorgu = "UPDATE izinlerim SET onay_durumu = 'Onaylandı' WHERE id = @id";
                MySqlCommand komut = new MySqlCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@id", id);

                if (baglanti.State == ConnectionState.Closed) baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
            }
        }
        // VeriDeposu.cs içine ekle
        public void IzinDurumGuncelleDAL(int id, string yeniDurum)
        {
            using (MySqlConnection baglanti = GetConnection())
            {
                baglanti.Open();
                // Gelen ID'ye göre onay_durumu sütununu güncelliyoruz
                string sorgu = "UPDATE izinlerim SET onay_durumu = @durum WHERE id = @id";

                using (MySqlCommand komut = new MySqlCommand(sorgu, baglanti))
                {
                    komut.Parameters.AddWithValue("@durum", yeniDurum);
                    komut.Parameters.AddWithValue("@id", id);
                    komut.ExecuteNonQuery();
                }
            }
        }
        public DataTable IzinleriGetirDAL()
        {
            using (MySqlConnection baglanti = GetConnection())
            {
                // 'kullanicilar' tablosundaki ad ve soyad bilgilerini 'tcno' üzerinden birleştiriyoruz
                string sorgu = "SELECT i.id, k.ad, k.soyad, i.tcno, i.izin_baslangic, i.izin_bitis, i.onay_durumu " +
                               "FROM izinlerim i " +
                               "INNER JOIN kullanici k ON i.tcno = k.tcno " +
                               "WHERE i.onay_durumu = 'Beklemede'";

                MySqlDataAdapter da = new MySqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public DataTable RaporGetirDAL(DateTime baslangic, DateTime bitis, string durum)
        {
            using (MySqlConnection baglanti = GetConnection())
            {
                baglanti.Open();
                // i.izin_baslangic ve i.izin_bitis görsellerdekiyle aynı
                // p.ad, p.soyad 'personellerim' tablosundan geliyor
                string sorgu = "SELECT p.ad, p.soyad, i.tcno, i.izin_turu, i.izin_baslangic, i.izin_bitis, i.onay_durumu " +
                               "FROM izinlerim i " +
                               "INNER JOIN personellerim p ON i.tcno = p.tcno " +
                               "WHERE i.izin_baslangic >= @bas AND i.izin_bitis <= @bit";

                if (durum != "Tümü")
                {
                    sorgu += " AND i.onay_durumu = @durum";
                }

                MySqlCommand cmd = new MySqlCommand(sorgu, baglanti);
                // MySQL tarih formatı uyuşmazlığını önlemek için:
                cmd.Parameters.AddWithValue("@bas", baslangic.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@bit", bitis.ToString("yyyy-MM-dd"));

                if (durum != "Tümü") { cmd.Parameters.AddWithValue("@durum", durum); }

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public bool PerformansKaydetDAL(string tcno, int puan, string not)
        {
            using (MySqlConnection baglanti = GetConnection())
            {
                baglanti.Open();

                // 1. Adım: personellerim tablosundan ad ve soyadı çek
                string adSoyadSorgu = "SELECT ad, soyad FROM personellerim WHERE tcno = @tc";
                MySqlCommand cmdAdSoyad = new MySqlCommand(adSoyadSorgu, baglanti);
                cmdAdSoyad.Parameters.AddWithValue("@tc", tcno);

                string ad = "";
                string soyad = "";

                using (MySqlDataReader dr = cmdAdSoyad.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        ad = dr["ad"].ToString();
                        soyad = dr["soyad"].ToString();
                    }
                }

                // 2. Adım: Yeni tablo adın 'personelperformans' ile verileri kaydet
                string sorgu = "INSERT INTO personelperformans (tcno, ad, soyad, puan, degerlendirme, tarih) " +
                               "VALUES (@tc, @ad, @soyad, @puan, @not, CURDATE())";

                MySqlCommand cmd = new MySqlCommand(sorgu, baglanti);
                cmd.Parameters.AddWithValue("@tc", tcno);
                cmd.Parameters.AddWithValue("@ad", ad);
                cmd.Parameters.AddWithValue("@soyad", soyad);
                cmd.Parameters.AddWithValue("@puan", puan);
                cmd.Parameters.AddWithValue("@not", not);


                return cmd.ExecuteNonQuery() > 0;
            }
        }
             public DataTable ListeleDAL()
        {
            using (MySqlConnection baglanti = GetConnection())
            {
                baglanti.Open();
                string sorgu = "SELECT * FROM personellerim"; // Görseldeki tablo adın
                MySqlDataAdapter da = new MySqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public DataTable PersonelGetirDAL()
        {
            using (MySqlConnection baglanti = GetConnection())
            {
                baglanti.Open();
                // CONCAT ile ad ve soyadı birleştirip 'AdSoyad' takma adını verdik
                string sorgu = "SELECT *, CONCAT(ad, ' ', soyad) AS AdSoyad FROM personellerim";
                MySqlDataAdapter da = new MySqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }


    
}



