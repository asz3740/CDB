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
    public partial class StockAs : Form
    {
        private string id;
        private string name;
        private string author;
        private string publisher;
        private string price;
        private string stock;

        private int selectedRowIndex;

        public StockAs()
        {
            InitializeComponent();
        }

        public StockAs(int selectedRowIndex, string v1, string v2, string v3, string v4, string v5, string v6)
        {
            InitializeComponent();
            this.selectedRowIndex = selectedRowIndex;
            this.id = v1;
            this.name = v2;
            this.author = v3;
            this.publisher = v4;
            this.price = v5;
            this.stock = v6;
        }

        Stock mainForm;
        private void Form2_Load(object sender, EventArgs e)
        {
            txtId.Text = id;
            txtName.Text = name;
            txtAuthor.Text = author;
            txtPublisher.Text = publisher;
            txtPirce.Text = price;
            txtStock.Text = stock;

            if (Owner != null)
            {
                mainForm = Owner as Stock;
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            string[] rowDatas = {
                txtName.Text,
                txtAuthor.Text,
                txtPublisher.Text,
                txtPirce.Text,
                txtStock.Text };
            mainForm.InsertRow(rowDatas);
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string[] rowDatas = {
                txtId.Text,
                txtName.Text,
                txtAuthor.Text,
                txtPublisher.Text,
                txtPirce.Text,
                txtStock.Text};
            mainForm.UpdateRow(rowDatas);
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("주문 내역에 삭제하려는 도서의 정보가 있을 수 있습니다.. 그래도 삭제하시겠습니까?", "삭제확인창", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                mainForm.DeleteRow(id);
                this.Close();
            }
        }

        private void btnTextBoxClear_Click(object sender, EventArgs e)
        {
            txtId.Clear();
            txtName.Clear();
            txtAuthor.Clear();
            txtPublisher.Clear();
            txtPirce.Clear();
            txtStock.Clear();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
