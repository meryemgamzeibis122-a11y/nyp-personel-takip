using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ödev534547378347587
{
    public partial class Form3 : Form
    {
            
            PersonelBLL bll = new PersonelBLL();
            public Form2 anaForm;
        public Form3()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                string tc = maskedTextBox1.Text.Replace("_", "").Replace(" ", "").Trim();

                if (tc.Length < 11)
                {
                    MessageBox.Show("Lütfen 11 haneli TC giriniz!");
                    return;
                }

                DateTime baslangic = dateTimePicker1.Value;
                DateTime bitis = dateTimePicker2.Value;
                string tur = comboBox1.Text;
                string aciklama = textBox1.Text;

                
                PersonelBLL BLL = new PersonelBLL();
                BLL.IzinKayitBLL(tc, baslangic, bitis, tur, aciklama);

                MessageBox.Show("İzin kaydı başarıyla oluşturuldu! ✅");

               
                textBox1.Clear();
                maskedTextBox1.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            izin_listele();
            string secilenIzin=comboBox1.Text;

        }
         
        void izin_listele()
        {
            PersonelBLL BLL = new PersonelBLL();
            dataGridView1.DataSource = BLL.IzinListeleBLL();
        }
        void gun_hesapla()
        {
            labelGun.BackColor = Color.Yellow;
            DateTime baslangic = dateTimePicker1.Value;
            DateTime bitis = dateTimePicker2.Value;

            TimeSpan fark = bitis - baslangic;
            int gunSayisi = fark.Days;

            if (gunSayisi < 0)
            {
                labelGun.Text = "Hata: Bitiş tarihi küçük!";
            }
            else
            {
                labelGun.Text = "Toplam İzin Günü: " + gunSayisi.ToString();
            }
            gunSayisi.ToString();
        }
        
        private void Form3_Load(object sender, EventArgs e)
        {
            izin_listele();
            gun_hesapla();
            Listele();
        }

        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && anaForm != null)
            {
                DataGridViewRow satir = dataGridView1.Rows[e.RowIndex];

               
                anaForm.lblSeciliTC.Text = "TC: " + satir.Cells["tcno"].Value.ToString();
                anaForm.lblSeciliDurum.Text = "DURUM: " + satir.Cells["onay_durumu"].Value.ToString();

               
                anaForm.secilenIzinId = Convert.ToInt32(satir.Cells["id"].Value);

                anaForm.BringToFront();
            }
        }


        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
           
            gun_hesapla();
        }

       

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateTime baslangic = dateTimePicker1.Value;
            DateTime bitis = dateTimePicker2.Value;

            TimeSpan fark = bitis - baslangic;
            int gunSayisi = fark.Days;

            if (gunSayisi < 0)
            {
                labelGun.Text = "Hata: Bitiş tarihi başlangıçtan önce olamaz!";
            }
            else
            {
                labelGun.Text = "Toplam İzin Günü: " + gunSayisi.ToString();
            }
            gun_hesapla();
        }
        
        private void btnAra_Click(object sender, EventArgs e)
        {
            PersonelBLL bll = new PersonelBLL();
            DataTable dt = bll.TCYeGoreDetayBLL(txtAraTC.Text);

            if (dt.Rows.Count > 0)
            {
                
                string ad = dt.Rows[0]["ad"].ToString();
                string soyad = dt.Rows[0]["soyad"].ToString();
                string tc = dt.Rows[0]["tcno"].ToString();
                string baslangic = Convert.ToDateTime(dt.Rows[0]["izin_baslangic"]).ToShortDateString();
                string bitis = Convert.ToDateTime(dt.Rows[0]["izin_bitis"]).ToShortDateString();
                string tur = dt.Rows[0]["izin_turu"].ToString();

                
                lblDetay.Text = "--- PERSONEL BİLGİSİ ---\n\n" +
                                "AD SOYAD: " + ad.ToUpper() + " " + soyad.ToUpper() + "\n" +
                                "TC KİMLİK: " + tc + "\n" +
                                "BAŞLANGIÇ: " + baslangic + "\n" +
                                "BİTİŞ TARİHİ: " + bitis + "\n" +
                                "İZİN TÜRÜ: " + tur;

                lblDetay.Visible = true;
            }
            else
            {
                MessageBox.Show("Bu TC'ye ait izin veya personel kaydı bulunamadı!");
            }
        }

        
        private void btnSil_Click(object sender, EventArgs e)
        {
            
            if (dataGridView1.CurrentRow != null)
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);

                DialogResult onay = MessageBox.Show("Bu kaydı silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo);
                if (onay == DialogResult.Yes)
                {
                    PersonelBLL bll = new PersonelBLL();
                    bll.IzinSilBLL(id);
                    MessageBox.Show("Kayıt başarıyla silindi.");
                    izin_listele(); 
                }
            }
        }

        
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);
                PersonelBLL bll = new PersonelBLL();

                
                bll.IzinGuncelleBLL(id, txtAraTC.Text, dateTimePicker1.Value, dateTimePicker2.Value, comboBox1.Text, textBox1.Text);

                MessageBox.Show("Kayıt başarıyla güncellendi.");
                izin_listele();
            }
        }
        // Form3.cs dosyasında herhangi bir metodun bittiği parantezden sonra:
        public void Listele()
        {
            dataGridView1.DataSource = bll.IzinleriGetirBLL();
        }
       
    }   
}
