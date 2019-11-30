using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WINTEST
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
          
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BTmemb_Click(object sender, EventArgs e)
        {
            Memb frm1 = new Memb(); // 새 폼 생성

            frm1.Show(); // 새폼 보여 주 기
        }

        private void BTbook_Click(object sender, EventArgs e)
        {
            Stock frm2 = new Stock(); // 새 폼 생성

            frm2.Show(); // 새폼 보여 주 기
        }

        private void BTorder_Click(object sender, EventArgs e)
        {
            Order frm3 = new Order(); // 새 폼 생성

            frm3.Show(); // 새폼 보여 주 기
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {
            pictureBox1.Load(@"C:\WinFormDB\c#image.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}
