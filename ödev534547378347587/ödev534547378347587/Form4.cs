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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

      
            PersonelBLL bll = new PersonelBLL();

        private void btnSorgula_Click(object sender, EventArgs e)
        {
            // Veritabanından verileri çekip DataGridView'e aktarır
            dgvRapor.DataSource = bll.RaporGetirBLL(dtpBaslangic.Value, dtpBitis.Value, cmbDurum.SelectedItem?.ToString() ?? "Tümü");
        }

    }
}
