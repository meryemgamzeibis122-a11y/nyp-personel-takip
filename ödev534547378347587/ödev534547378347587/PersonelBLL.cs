using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using static ödev534547378347587.VeriDeposu;

namespace ödev534547378347587
{
    public class PersonelBLL
    {
        // TC Kimlik Kontrolü Metodu
        public bool TCKontrolEt(string tc)
        {
           
            return tc.Length == 11;
        }

        // Şifre Karakter Düzenleme Metodu
        public string SifreFormatla(string sifre)
        {
            if (string.IsNullOrEmpty(sifre)) return "";
            return sifre.Replace('ı', 'i').Replace('İ', 'I').Replace('ş', 's').Replace('Ş', 'S')
                        .Replace('ğ', 'g').Replace('Ğ', 'G').Replace('ü', 'u').Replace('Ü', 'U')
                        .Replace('ö', 'o').Replace('Ö', 'O').Replace('ç', 'c').Replace('Ç', 'C');
        }
        // Parola ve Parola Tekrar uyumlu mu 
        public bool ParolaUyumuKontrolEt(string parola, string parolaTekrar)
        {
            
            return parola == parolaTekrar;
        }
        VeriDeposu veri = new VeriDeposu();

        public void YeniPersonelKaydet(string tc, string ad, string soyad, string kAd, string parola, string yetki)
        {
            
            veri.PersonelEkle(tc, ad, soyad, kAd, parola, yetki);
        }
        public void PersonelSil(string tc)
        {
            veri.PersonelSil(tc);
        }
        public void PersonelKayit(string tc, string ad, string soyad, string cinsiyet, string mezuniyet, DateTime dogum, string gorev, string yer, int maas)
        {
            
            veri.PersonelKaydet(tc, ad, soyad, cinsiyet, mezuniyet, dogum, gorev, yer, maas);
        }
        public void PersonelTabloSilmeBLL(string tc)
        {
            
            veri.PersonelTablosundanSil(tc);
        }
        public void IzinKayitBLL(string tc, DateTime bas, DateTime bit, string tur, string acik)
        {
            // VeriDeposu'ndaki metodu çağırıyoruz
            veri.IzinVerisiKaydet(tc, bas, bit, tur, acik);

        }
        public DataTable IzinListeleBLL()
        {
            return veri.IzinleriGetir();
        }
        public DataTable TCYeGoreDetayBLL(string tc) => veri.TCYeGoreDetayGetir(tc);

        public void IzinSilBLL(int id) => veri.IzinSil(id);

        public void IzinGuncelleBLL(int id, string tc, DateTime bas, DateTime bit, string tur, string aciklama)
            => veri.IzinGuncelle(id, tc, bas, bit, tur, aciklama);
       
        // PersonelBLL.cs içindeki sınıfın içine yapıştır
        public void IzinOnaylaBLL(int id)
        {
            // VeriDeposu'ndaki (DAL) güncelleme metodunu çağırıyoruz
            // Not: DAL'daki metodun isminin 'IzinDurumGuncelleDAL' olduğunu varsayıyoruz
            veri.IzinDurumGuncelleDAL(id, "Onaylandı");
        }

        public void IzinReddetBLL(int id)
        {
            veri.IzinDurumGuncelleDAL(id, "Reddedildi");
        }

        // PersonelBLL.cs içine ekle
        public DataTable IzinleriGetirBLL()
        {
            // DAL katmanındaki (VeriDeposu) listeleme metodunu çağırır
            return veri.IzinleriGetirDAL();
        }
        public DataTable RaporGetirBLL(DateTime bas, DateTime bit, string durum)
        {
            // İş mantığı: Başlangıç tarihi bitişten büyükse boş dön veya hata fırlat
            return veri.RaporGetirDAL(bas, bit, durum);
        }

        public DataTable ListeleBLL()
        {
            // Veritabanındaki 'personellerim' tablosunu çeker
            return veri.ListeleDAL();
        }
        public bool PerformansKaydetBLL(string tc, int puan, string not)
        {
            // Formdan gelen verileri DAL katmanına iletir
            return veri.PerformansKaydetDAL(tc, puan, not);
        }

        public DataTable PersonelGetirBLL()
        {
            return veri.PersonelGetirDAL(); // DAL'daki birleştirilmiş listeyi getirir
        }

        public double MaasHesaplaBLL(string gorev, int gun)
        {
            double gunluk = 0;
            // Görev isimleri veritabanındakilerle birebir aynı olmalı
            if (gorev == "Yönetici") gunluk = 3000;
            else if (gorev == "Memur") gunluk = 1800;
            else if (gorev == "Şöför") gunluk = 1400;
            else if (gorev == "İşçi") gunluk = 1200;
            else gunluk = 1000;

            return gunluk * gun;
        }
    }
}
