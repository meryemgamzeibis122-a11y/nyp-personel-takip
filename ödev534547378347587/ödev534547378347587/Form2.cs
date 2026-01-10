using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
//regex kütüphanesi parola işlemleri için
using System.Text.RegularExpressions;
//giriş çıkış işlemleri kütüphanesi
using System.IO;
using System.Data.SqlTypes;
using System.Data.OleDb;
using System.Diagnostics.Eventing.Reader;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace ödev534547378347587
{
    public partial class Form2 : Form
    {   
        public int secilenIzinId;
        PersonelBLL PersonelIslem = new PersonelBLL();
        PersonelBLL bll=new PersonelBLL();

        public Form2()
        {
            InitializeComponent();
        }
        string connectionString = "Server=172.21.54.253;Database=PersonelTakip;Uid=26_132430005;Pwd=İnif123.;";

        private void kullanicilari_göster()


        {
            try
            {
                using (MySqlConnection baglantim = new MySqlConnection(connectionString))
                {
                    baglantim.Open();


                    string sorgu = "SELECT tcno AS 'TC KİMLİK NO', ad AS 'ADI', soyad AS 'SOYADI', " +
                                   "yetki AS 'YETKİ', kullaniciadi AS 'KULLANICI ADI', parola AS 'PAROLA' " +
                                   "FROM kullanici ORDER BY ad ASC";

                    MySqlDataAdapter kullanicilari_listele = new MySqlDataAdapter(sorgu, baglantim);


                    DataSet dshafiza = new DataSet();
                    kullanicilari_listele.Fill(dshafiza, "kullanici");


                    dataGridView1.DataSource = dshafiza.Tables["kullanici"];

                    baglantim.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri gösterilirken hata oluştu: " + ex.Message);
            }
        }
        private void personellerigöster_göster()
        {
            try
            {
                using (MySqlConnection baglantim = new MySqlConnection(connectionString))
                {
                    baglantim.Open();


                    string sorgu = "SELECT tcno AS 'TC KİMLİK NO', ad AS 'ADI', soyad AS 'SOYADI', " +
                                   "cinsiyet AS'CİNSİYETİ',mezuniyet AS'MEZUNİYETİ',dogum_tarihi AS 'DOĞUM TARİHİ',gorevi AS 'GÖREVİ',gorev_yeri AS'GÖREV YERİ'" +
                                   "FROM personellerim ORDER BY ad ASC";

                    MySqlDataAdapter personelleri_listele = new MySqlDataAdapter(sorgu, baglantim);


                    DataSet dshafiza = new DataSet();
                    personelleri_listele.Fill(dshafiza, "personellerim");


                    dataGridView2.DataSource = dshafiza.Tables["personellerim"];

                    baglantim.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri gösterilirken hata oluştu: " + ex.Message);
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            //kullanıcı işlemleri ayarları
            this.Text = "YÖNETİCİ İŞLEMLERİ";
            textBox1.MaxLength = 11;
            textBox4.MaxLength = 8;
            toolTip1.SetToolTip(this.textBox1, "Tc Kimlik No 11 haneli OLmalı!");
            textBox2.CharacterCasing = CharacterCasing.Upper;
            textBox3.CharacterCasing = CharacterCasing.Upper;
            textBox5.MaxLength = 10;
            textBox6.MaxLength = 10;

            kullanicilari_göster();
            //personel işlemleri ayarları
            maskedTextBox1.Mask = "00000000000";
            maskedTextBox2.Mask = "LLL????????????????????";
            maskedTextBox3.Mask = "LLL????????????????????";
            maskedTextBox4.Mask = "000000";
            maskedTextBox2.Text.ToUpper();
            maskedTextBox3.Text.ToUpper();

            comboBox1.Items.Add("ilköğretim");
            comboBox1.Items.Add("ortaöğretim");
            comboBox1.Items.Add("lise");
            comboBox1.Items.Add("yükseköğretim");

            comboBox2.Items.Add("Yönetici");
            comboBox2.Items.Add("Memur");
            comboBox2.Items.Add("Şöför");
            comboBox2.Items.Add("İşçi");

            comboBox3.Items.Add("Arge");
            comboBox3.Items.Add("Bilgi İşlem");
            comboBox3.Items.Add("Muhasebe");
            comboBox3.Items.Add("Üretim");
            comboBox3.Items.Add("Paketleme");
            comboBox3.Items.Add("Nakliye");

            DateTime zaman = DateTime.Now;
            int yil = int.Parse(zaman.ToString("yyyy"));
            int ay = int.Parse(zaman.ToString("MM"));
            int gün = int.Parse(zaman.ToString("dd"));

            dateTimePicker1.MinDate = new DateTime(1975, 1, 1);
            dateTimePicker1.MaxDate = new DateTime(yil - 18, ay, gün);

            // Personelleri ComboBox'a dolduruyoruz

            DataTable dt = bll.ListeleBLL(); // Veriyi çek

            if (dt.Rows.Count > 0) // Eğer veri geldiyse
            {
                cmbPersonel.DataSource = dt;
                cmbPersonel.DisplayMember = "ad"; // Sütun adının 'ad' olduğundan emin ol
                cmbPersonel.ValueMember = "tcno"; // Sütun adının 'tcno' olduğundan emin ol
            }
            else
            {
                MessageBox.Show("Veritabanında personel bulunamadı!"); // Bu mesaj çıkarsa tablo boştur
            }
            // Form açıldığında ComboBox'ı ad ve soyad birleşik şekilde doldurur
            comboBox4.DataSource = bll.PersonelGetirBLL();
            comboBox4.DisplayMember = "AdSoyad"; // DAL'da verdiğimiz takma isim
            comboBox4.ValueMember = "tcno";








        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string duzeltilmisSifre = PersonelIslem.SifreFormatla(textBox5.Text);
            if (textBox5.Text != duzeltilmisSifre)
            {
                textBox5.Text = duzeltilmisSifre;
                textBox5.SelectionStart = textBox5.Text.Length;
            }
        }
        private void topPage1_temizle()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }
        private void topPage2_temizle()
        {
            maskedTextBox1.Clear();
            maskedTextBox2.Clear();
            maskedTextBox3.Clear();
            maskedTextBox4.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                string secilenYetki = radioButton1.Checked ? "Yönetici" : "Kullanıcı";


                PersonelIslem.YeniPersonelKaydet(
                    textBox1.Text, // TC
                    textBox2.Text, // Ad
                    textBox3.Text, // Soyad
                    textBox4.Text, // Kullanıcı Adı
                    textBox5.Text, // Parola
                    secilenYetki   // Yetki
                );


                MessageBox.Show("Kayıt Başarıyla Tamamlandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);


                personelleri_goster();


                topPage1_temizle();
            }
            catch (Exception ex)
            {
                // Bağlantı veya SQL hatası olursa burada göreceğiz
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void personelleri_goster()
        {
            using (MySqlConnection baglanti = new VeriDeposu().GetConnection())
            {
                baglanti.Open();
                string sorgu = "SELECT * FROM kullanici";
                MySqlDataAdapter da = new MySqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Lütfen silinecek personelin TC numarasını giriniz!");
                return;
            }


            DialogResult onay = MessageBox.Show(textBox1.Text + " TC'li personeli silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (onay == DialogResult.Yes)
            {
                try
                {

                    PersonelIslem.PersonelSil(textBox1.Text);

                    MessageBox.Show("Personel başarıyla silindi.");


                    personelleri_goster();
                    topPage1_temizle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Silme işlemi sırasında bir hata oluştu: " + ex.Message);
                }
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {

                string tc = maskedTextBox1.Text.Replace("_", "").Replace(" ", "").Trim();
                string ad = maskedTextBox2.Text.Replace("_", "").Replace(" ", "").Trim();
                string soyad = maskedTextBox3.Text.Replace("_", "").Replace(" ", "").Trim();
                string hamMaas = maskedTextBox4.Text.Replace("_", "").Replace(" ", "").Trim();


                int maasDegeri;
                if (!int.TryParse(hamMaas, out maasDegeri))
                {
                    MessageBox.Show("Lütfen maaş alanına sadece sayı giriniz!", "Hata");
                    return;
                }


                string cinsiyet = radioButton1.Checked ? "KADIN" : "ERKEK";
                string mezuniyet = comboBox1.Text;
                string gorev = comboBox2.Text;
                string yer = comboBox3.Text;
                DateTime dogum = dateTimePicker1.Value;


                PersonelBLL PersonelIslem = new PersonelBLL();
                PersonelIslem.PersonelKayit(tc, ad, soyad, cinsiyet, mezuniyet, dogum, gorev, yer, maasDegeri);

                MessageBox.Show("Personel başarıyla kaydedildi!");


                personel_listele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kaydetme sırasında bir hata oluştu: " + ex.Message);
            }
        }
        public void personel_listele()
        {
            using (MySqlConnection baglanti = new VeriDeposu().GetConnection())
            {
                baglanti.Open();
                string sorgu = "SELECT * FROM personellerim";
                MySqlDataAdapter da = new MySqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView2.DataSource = dt; // Personel tablosu dataGridView2 ise
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {

                string temizTC = maskedTextBox1.Text.Replace("_", "").Replace(" ", "").Trim();

                if (string.IsNullOrEmpty(temizTC))
                {
                    MessageBox.Show("Lütfen önce silinecek personelin TC numarasını girin!");
                    return;
                }


                PersonelBLL PersonelIslem = new PersonelBLL();
                PersonelIslem.PersonelTabloSilmeBLL(temizTC);


                MessageBox.Show("Personel başarıyla silindi.");


                personel_listele();


                maskedTextBox1.Clear();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void btnOnayla_Click(object sender, EventArgs e)
        {
            // secilenIzinId'nin Form 3'ten doğru geldiğinden emin ol
            if (secilenIzinId != 0)
            {
                bll.IzinOnaylaBLL(secilenIzinId);
                MessageBox.Show("İzin Başarıyla Onaylandı!");

                // Tasarımdaki etiketi güncelle
                lblSeciliDurum.Text = "DURUM: Onaylandı";
            }
            else
            {
                MessageBox.Show("lütfen listeden bir kayıt seçin");
            }

        }
        private void btnReddet_Click(object sender, EventArgs e)
        {
            if (secilenIzinId != 0) // Form 3'ten bir ID gelmiş mi kontrol et
            {
                // BLL katmanına reddetme emrini gönder
                bll.IzinReddetBLL(secilenIzinId);

                MessageBox.Show("İzin talebi reddedildi.");

                // Formdaki durumu güncelle
                lblSeciliDurum.Text = "DURUM: Reddedildi";
                secilenIzinId = 0;
            }
            else
            {
                MessageBox.Show("Lütfen önce bir kayıt seçin!");
            }
        }
        private void btnIzinListele_Click(object sender, EventArgs e)
        {
            // Form 3'ten bir nesne oluşturuyoruz
            Form3 f3 = new Form3();

            // KRİTİK SATIR: Form 3'teki anaForm değişkenine "bu formu (Form2)" aktarıyoruz
            f3.anaForm = this;

            // Form 3'ü ekranda gösteriyoruz
            f3.Show();
            f3.Listele();
        }
        private void btnRaporuAc_Click(object sender, EventArgs e)
        {
            // Form 4 nesnesini oluşturuyoruz
            Form4 raporFormu = new Form4();

            // Formu ekranda gösteriyoruz
            raporFormu.Show();
        }

        private void btnPerformansKaydet_Click(object sender, EventArgs e)
        {
            
            try
            {
                string tc = cmbPersonel.SelectedValue.ToString();
                int puan = (int)numPuan.Value;
                string not = txtNotlar.Text;

                // BLL'deki kırmızı çizgi hatasını gidermek için PersonelBLL'e metodu eklemiş olmalısın
                if (bll.PerformansKaydetBLL(tc, puan, not))
                {
                    MessageBox.Show("Performans başarıyla kaydedildi!");
                    txtNotlar.Clear();
                    numPuan.Value = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        
        }
        private void btnMaasHesapla_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox4.SelectedItem != null)
                {
                    DataRowView satir = (DataRowView)comboBox4.SelectedItem;
                    string gorev = satir["gorevi"].ToString(); // Veritabanındaki sütun adın 'gorevi'

                    // TextBox ismini kontrol et (Örn: textBox1), gün sayısını oradan alır
                    int gun = int.Parse(txtCalisilanGun.Text);

                    double toplamMaas = bll.MaasHesaplaBLL(gorev, gun);

                    // label12 isminin tasarımda doğru olduğundan emin ol!
                    label12.Text = $"{satir["AdSoyad"]} ({gorev}) Maaşı: {toplamMaas.ToString("C2")}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lütfen geçerli bir gün sayısı girin! Hata: " + ex.Message);
            }
        }
    }
        
}












