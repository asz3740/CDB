using MySql.Data.MySqlClient;
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
    public partial class Memb : Form
    {
        MySqlConnection conn;
        MySqlDataAdapter dataAdapter;
        DataSet dataSet;
        int selectedRowIndex;

        public Memb()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            string connStr = "server=localhost;port=3307;database=book;uid=root;pwd=1234";
            conn = new MySqlConnection(connStr);
            dataAdapter = new MySqlDataAdapter("SELECT * FROM customer", conn);
            dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "customer");
            dataGridView1.DataSource = dataSet.Tables["customer"];

            SetSearchComboBox();
        }

        private void SetSearchComboBox()
        {
            string sql = "SELECT distinct city FROM customer";
            MySqlCommand cmd = new MySqlCommand(sql, conn);

            try
            {
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())  
                {
                    cbCity.Items.Add(reader.GetString("city"));
                }
                reader.Close();

                sql = "SELECT distinct dong FROM customer";
                cmd = new MySqlCommand(sql, conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())  
                {
                    cbDong.Items.Add(reader.GetString("dong"));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void cbCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "SELECT distinct dong FROM customer WHERE city=@city";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@city", cbCity.Text);

            cbDong.Items.Clear();      

            try
            {
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) 
                {
                    cbDong.Items.Add(reader.GetString("dong"));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            string queryStr;
            string[] conditions = new string[6];
            conditions[0] = (textBoxID.Text != "") ? "custid=@custid" : null;
            conditions[1] = (textBoxName.Text != "") ? "name=@name" : null;
            conditions[2] = (cbCity.Text != "") ? "city=@city" : null;
            conditions[3] = (cbDong.Text != "") ? "dong=@dong" : null;
            conditions[4] = (textBoxDetail.Text != "") ? "detail=@detail" : null;
            conditions[5] = (textBoxPhone.Text != "") ? "phone=@phone" : null;

            if (conditions[0] != null || conditions[1] != null || conditions[2] != null || conditions[3] != null || conditions[4] != null || conditions[5] != null)
            {
                queryStr = $"SELECT * FROM customer WHERE ";
                bool firstCondition = true;
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (conditions[i] != null)
                        if (firstCondition)
                        {
                            queryStr += conditions[i];
                            firstCondition = false;
                        }
                        else
                        {
                            queryStr += " and " + conditions[i];
                        }
                }
            }
            else
            {
                queryStr = "SELECT * FROM customer";
            }
   
            dataAdapter.SelectCommand = new MySqlCommand(queryStr, conn);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@custid", textBoxID.Text);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@name", textBoxName.Text);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@city", cbCity.Text);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@dong", cbDong.Text);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@detail", textBoxDetail.Text);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@phone", textBoxPhone.Text);

            try
            {
                conn.Open();
                dataSet.Clear();
                if (dataAdapter.Fill(dataSet, "customer") > 0)
                    dataGridView1.DataSource = dataSet.Tables["customer"];
                else
                    MessageBox.Show("찾는 회원이 없습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRowIndex = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[selectedRowIndex];

            MembAs Dig = new MembAs(
                selectedRowIndex,
                row.Cells[0].Value.ToString(),
                row.Cells[1].Value.ToString(),
                row.Cells[2].Value.ToString(),
                row.Cells[3].Value.ToString(),
                row.Cells[4].Value.ToString(),
                row.Cells[5].Value.ToString()
                );

            Dig.Owner = this;              
            Dig.ShowDialog();
            Dig.Dispose();
        }

        public void InsertRow(string[] rowDatas)
        {
            string queryStr = "INSERT INTO customer (name, city, dong, detail, phone) " +
                "VALUES(@name, @city, @dong, @detail, @phone)";
            dataAdapter.InsertCommand = new MySqlCommand(queryStr, conn);
            dataAdapter.InsertCommand.Parameters.Add("@name", MySqlDbType.VarChar);
            dataAdapter.InsertCommand.Parameters.Add("@city", MySqlDbType.VarChar);
            dataAdapter.InsertCommand.Parameters.Add("@dong", MySqlDbType.VarChar);
            dataAdapter.InsertCommand.Parameters.Add("@detail", MySqlDbType.VarChar);
            dataAdapter.InsertCommand.Parameters.Add("@phone", MySqlDbType.Int32);

            #region Parameter를 이용한 처리
            dataAdapter.InsertCommand.Parameters["@name"].Value = rowDatas[0];
            dataAdapter.InsertCommand.Parameters["@city"].Value = rowDatas[1];
            dataAdapter.InsertCommand.Parameters["@dong"].Value = rowDatas[2];
            dataAdapter.InsertCommand.Parameters["@detail"].Value = rowDatas[3];
            dataAdapter.InsertCommand.Parameters["@phone"].Value = rowDatas[4];

            try
            {
                conn.Open();
                dataAdapter.InsertCommand.ExecuteNonQuery();

                dataSet.Clear();                                       
                dataAdapter.Fill(dataSet, "customer");                      
                dataGridView1.DataSource = dataSet.Tables["customer"];      
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            #endregion
        }

        internal void DeleteRow(string id)
        {
            string sql = "DELETE FROM customer WHERE custid=@custid";
            dataAdapter.DeleteCommand = new MySqlCommand(sql, conn);
            dataAdapter.DeleteCommand.Parameters.AddWithValue("@custid", id);

            try
            {
                conn.Open();
                dataAdapter.DeleteCommand.ExecuteNonQuery();

                dataSet.Clear();
                dataAdapter.Fill(dataSet, "customer");
                dataGridView1.DataSource = dataSet.Tables["customer"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        internal void UpdateRow(string[] rowDatas)
        {
            string sql = "UPDATE customer SET name=@name, city=@city, dong=@dong, detail=@detail, phone=@phone WHERE custid=@custid";
            dataAdapter.UpdateCommand = new MySqlCommand(sql, conn);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@custid", rowDatas[0]);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@name", rowDatas[1]);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@city", rowDatas[2]);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@dong", rowDatas[3]);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@detail", rowDatas[4]);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@phone", rowDatas[5]);

            try
            {
                conn.Open();
                dataAdapter.UpdateCommand.ExecuteNonQuery();

                dataSet.Clear();  // 이전 데이터 지우기
                dataAdapter.Fill(dataSet, "customer");
                dataGridView1.DataSource = dataSet.Tables["customer"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            MembAs Dig = new MembAs();
            Dig.Owner = this;             
            Dig.ShowDialog();              
            Dig.Dispose();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBoxID.Clear();
            textBoxName.Clear();
            cbCity.Text = "";
            cbDong.Text = "";
            textBoxDetail.Clear();
            textBoxPhone.Clear();
        }
    }
}
