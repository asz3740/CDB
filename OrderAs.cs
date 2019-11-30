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
    public partial class OrderAs : Form
    {
        private string orderid;
        private string custid;
        private string bookid;
        private string saleprice;
        private string year;
        private string month;
        private string day;
        private int selectedRowIndex;
        public OrderAs()
        {
            InitializeComponent();
        }
        public OrderAs(int selectedRowIndex, string v1, string v2, string v3, string v4, string v5, string v6, string v7)
        {
            InitializeComponent();
            this.selectedRowIndex = selectedRowIndex;
            this.orderid = v1;
            this.custid = v2;
            this.bookid = v3;
            this.saleprice = v4;
            this.year = v5;
            this.month = v6;
            this.day = v7;
        }

        Order mainForm;

        private void OrderAs_Load(object sender, EventArgs e)
        {
            txtOrderid.Text = orderid;
            txtCustid.Text = custid;
            txtBookid.Text = bookid;
            txtSaleprice.Text = saleprice;
            txtYear.Text = year;
            txtMonth.Text = month;
            txtDay.Text = day;

            if (Owner != null)
            {
                mainForm = Owner as Order;
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            string[] rowDatas = {
                txtCustid.Text,
                txtBookid.Text,
                txtSaleprice.Text,
                txtYear.Text,
                txtMonth.Text,
                txtDay.Text};
            mainForm.InsertRow(rowDatas);
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string[] rowDatas = {
                txtOrderid.Text,
                txtCustid.Text,
                txtBookid.Text,
                txtSaleprice.Text,
                txtYear.Text,
                txtMonth.Text,
                txtDay.Text};
            mainForm.UpdateRow(rowDatas);
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("정말삭제하시겠습니까?", "삭제확인창", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                mainForm.DeleteRow(orderid);
                this.Close();
            }
        }

        private void btnTextBoxClear_Click(object sender, EventArgs e)
        {
            txtOrderid.Clear();
            txtCustid.Clear();
            txtBookid.Clear();
            txtSaleprice.Clear();
            txtYear.Clear();
            txtMonth.Clear();
            txtDay.Clear();
        }
    }
}
