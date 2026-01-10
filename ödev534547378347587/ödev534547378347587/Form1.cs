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

namespace ödev534547378347587
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        public static string tcno, ad, soyadi, yetki;

        private void button1_Click(object sender, EventArgs e)
        {
            if (hak != 0)
            {
                
                string secilenYetki = "";
                if (radioButton1.Checked) secilenYetki = "Yönetici";
                else if (radioButton2.Checked) secilenYetki = "Kullanıcı";

                VeriDeposu vt = new VeriDeposu();
                using (MySqlConnection baglanti = vt.GetConnection())
                {
                    try
                    {
                        baglanti.Open();
                        
                        string sorgu = "SELECT * FROM kullanici WHERE kullaniciadi=@user AND parola=@pass AND yetki=@yetki";
                        MySqlCommand komut = new MySqlCommand(sorgu, baglanti);

                        komut.Parameters.AddWithValue("@user", txtkullaniciadi.Text);
                        komut.Parameters.AddWithValue("@pass", txtparola.Text);
                        komut.Parameters.AddWithValue("@yetki", secilenYetki);

                        MySqlDataReader oku = komut.ExecuteReader();

                        if (oku.Read())
                        {
                            MessageBox.Show("Giriş Başarılı! Hoşgeldin " + oku["ad"].ToString());
                            string yetki = oku["yetki"].ToString();
                            if (yetki == "yönetici")
                            {
                                Form2 f2= new Form2();
                                f2.Show();
                            }
                            else if (yetki == "kullanıcı")
                            {
                                Form3 f3 = new Form3();
                                f3.Show();

                            }
                            this.Hide();

                            
                        }
                        else
                        {
                            hak--;
                            label5.ForeColor = Color.Red;
                            label5.Text = "Kalan Hak: " + hak;
                            MessageBox.Show("Kullanıcı adı, şifre veya yetki hatalı!");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata: " + ex.Message);
                    }
                }
            }

        }


        int hak = 3;
        bool durum = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Kullanıcı Girişi...";
            this.AcceptButton = button1;
            this.CancelButton = button2;
            label5.Text=Convert.ToString(hak);
            radioButton1.Checked = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;

        }
    }
}
