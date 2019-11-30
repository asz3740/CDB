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
    public partial class MembAs : Form
    {
        private string id;
        private string name;
        private string city;
        private string dong;
        private string detail;
        private string phone;
        private int selectedRowIndex;

        public MembAs()
        {
            InitializeComponent();
        }

        public MembAs(int selectedRowIndex, string v1, string v2, string v3, string v4, string v5, string v6)
        {
            InitializeComponent();
            this.selectedRowIndex = selectedRowIndex;
            this.id = v1;
            this.name = v2;
            this.city = v3;
            this.dong = v4;
            this.detail = v5;
            this.phone = v6;
        }

        Memb mainForm;

        private void Form5_Load(object sender, EventArgs e)
        {
            txtId.Text = id;
            txtName.Text = name;
            txtCity.Text = city;
            txtDong.Text = dong;
            txtDetail.Text = detail;
            txtPhone.Text = phone;
            if (Owner != null)
            {
                mainForm = Owner as Memb;
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            string[] rowDatas = {
                txtName.Text,
                txtCity.Text,
                txtDong.Text,
                txtDetail.Text,
                txtPhone.Text};
            mainForm.InsertRow(rowDatas);
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string[] rowDatas = {
                txtId.Text,
                txtName.Text,
                txtCity.Text,
                txtDong.Text,
                txtDetail.Text,
                txtPhone.Text};
            mainForm.UpdateRow(rowDatas);
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            var result = MessageBox.Show("주문 내역에 삭제하려는 고객의 정보가 있을 수 있습니다.. 그래도 삭제하시겠습니까?", "삭제확인창", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
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
            txtCity.Clear();
            txtDong.Clear();
            txtDetail.Clear();
            txtPhone.Clear();
        }
    }
}
