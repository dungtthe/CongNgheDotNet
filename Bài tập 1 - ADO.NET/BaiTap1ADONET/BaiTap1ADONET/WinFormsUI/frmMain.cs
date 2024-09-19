using BaiTap1ADONET.BusinessLogicLayer;
using BaiTap1ADONET.DataAccessLayer.DataClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BaiTap1ADONET
{
    public partial class frmMain : Form
    {
        private AnimalLogic animalLogic;
        private string textCow;
        private string textSheep;
        private string textGoat;
        private int countCow;
        private int countSheep;
        private int countGoat;
        private bool canStatistical;


        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            animalLogic = AnimalLogic.GI();
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            List<DataThongKe> data;
            ValidateData();
            if (canStatistical)
                data = animalLogic.Statistical(countCow, countSheep, countGoat);
            else
                return;

            dataGrid.DataSource = data;
        }

        private void txtSoLuongBo_TextChanged(object sender, EventArgs e)
        {
            textCow = txtSoLuongBo.Text;
        }

        private void txtSoLuongCuu_TextChanged(object sender, EventArgs e)
        {
            textSheep = txtSoLuongCuu.Text;
        }

        private void txtSoLuongDe_TextChanged(object sender, EventArgs e)
        {
            textGoat = txtSoLuongDe.Text;
        }


        private void ValidateData()
        {
            if (!int.TryParse(textCow, out _))
            {
                MessageBox.Show("Chỉ chấp nhật số");
                canStatistical = false;

            }
            else if (!int.TryParse(textSheep, out _))
            {
                MessageBox.Show("Chỉ chấp nhật số.");
                canStatistical = false;

            }
            else if (!int.TryParse(textGoat, out _))
            {
                MessageBox.Show("Chỉ chấp nhật số.");
                canStatistical = false;

            }
            else
            {
                countCow = int.Parse(txtSoLuongBo.Text);
                countSheep = int.Parse(txtSoLuongDe.Text);
                countGoat = int.Parse(txtSoLuongCuu.Text);
                Console.WriteLine(countCow.ToString() + countSheep.ToString() + countGoat.ToString());
                canStatistical = true;
            }
        }
    }
}
