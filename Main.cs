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
            Memb frm1 = new Memb(); 

            frm1.Show(); 
        }

        private void BTbook_Click(object sender, EventArgs e)
        {
            Stock frm2 = new Stock(); 

            frm2.Show(); 
        }

        private void BTorder_Click(object sender, EventArgs e)
        {
            Order frm3 = new Order(); 

            frm3.Show(); 
        }

        private void Main_Load(object sender, EventArgs e)
        {
            pictureBox1.Load(@"C:\WinFormDB\c#image.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}
