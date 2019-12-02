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
    public partial class Order : Form
    {
        MySqlConnection conn;
        MySqlDataAdapter dataAdapter;
        DataSet dataSet;
        int selectedRowIndex;
        public Order()
        {
            InitializeComponent();
        }

        private void Order_Load(object sender, EventArgs e)
        {
            string connStr = "server=localhost;port=3307;database=book;uid=root;pwd=1234";
            conn = new MySqlConnection(connStr);
            dataAdapter = new MySqlDataAdapter("SELECT * FROM orders", conn);
            dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "orders");
            dataGridView1.DataSource = dataSet.Tables["orders"];

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            string queryStr;

            string[] conditions = new string[7];
            conditions[0] = (textBoxOrderID.Text != "") ? "orderid=@orderid" : null;
            conditions[1] = (textBoxCustID.Text != "") ? "custid=@custid" : null;
            conditions[2] = (textBoxBookID.Text != "") ? "bookid=@bookid" : null;
            conditions[4] = (textBoxYear.Text != "") ? "year=@year" : null;
            conditions[5] = (textBoxMonth.Text != "") ? "month=@month" : null;
            conditions[6] = (textBoxDay.Text != "") ? "day=@day" : null;
            string condition_saleprice;
            if (textBoxMin.Text != "" && textBoxMax.Text != "")
            {
                condition_saleprice = "saleprice>=@min and saleprice<=@max";
            }
            else if (textBoxMin.Text != "" || textBoxMax.Text != "")
            {
                if (textBoxMin.Text != "")
                    condition_saleprice = "saleprice >= @min";
                else
                    condition_saleprice = "saleprice <= @max";
            }
            else
            {
                condition_saleprice = null;
            }
            conditions[3] = condition_saleprice;

            if (conditions[0] != null || conditions[1] != null || conditions[2] != null || conditions[3] != null || conditions[4] != null || conditions[5] != null || conditions[6] != null)
            {
                queryStr = $"SELECT * FROM orders WHERE ";
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
                queryStr = "SELECT * FROM orders";
            }   
            dataAdapter.SelectCommand = new MySqlCommand(queryStr, conn);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@orderid", textBoxOrderID.Text);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@custid", textBoxCustID.Text);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@bookid", textBoxBookID.Text);        
            dataAdapter.SelectCommand.Parameters.AddWithValue("@min", textBoxMin.Text);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@max", textBoxMax.Text);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@year", textBoxYear.Text);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@month", textBoxMonth.Text);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@day", textBoxDay.Text);

            try
            {
                conn.Open();
                dataSet.Clear();
                if (dataAdapter.Fill(dataSet, "orders") > 0)
                    dataGridView1.DataSource = dataSet.Tables["orders"];
                else
                    MessageBox.Show("찾는 데이터가 없습니다.");
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

            OrderAs Dig = new OrderAs(
                selectedRowIndex,
                row.Cells[0].Value.ToString(),
                row.Cells[1].Value.ToString(),
                row.Cells[2].Value.ToString(),
                row.Cells[3].Value.ToString(),
                row.Cells[4].Value.ToString(),
                row.Cells[5].Value.ToString(),
                row.Cells[6].Value.ToString()
                );

            Dig.Owner = this;               
            Dig.ShowDialog();               
            Dig.Dispose();
        }

        public void InsertRow(string[] rowDatas)
        {
            string queryStr = "INSERT INTO orders (custid, bookid, saleprice, year, month, day) " +
                "VALUES(@custid, @bookid, @saleprice, @year, @month, @day)";
            dataAdapter.InsertCommand = new MySqlCommand(queryStr, conn);
            dataAdapter.InsertCommand.Parameters.Add("@custid", MySqlDbType.Int32);
            dataAdapter.InsertCommand.Parameters.Add("@bookid", MySqlDbType.Int32);
            dataAdapter.InsertCommand.Parameters.Add("@saleprice", MySqlDbType.Int32);
            dataAdapter.InsertCommand.Parameters.Add("@year", MySqlDbType.Int32);
            dataAdapter.InsertCommand.Parameters.Add("@month", MySqlDbType.Int32);
            dataAdapter.InsertCommand.Parameters.Add("@day", MySqlDbType.Int32);

            dataAdapter.InsertCommand.Parameters["@custid"].Value = rowDatas[0];
            dataAdapter.InsertCommand.Parameters["@bookid"].Value = rowDatas[1];
            dataAdapter.InsertCommand.Parameters["@saleprice"].Value = rowDatas[2];
            dataAdapter.InsertCommand.Parameters["@year"].Value = rowDatas[3];
            dataAdapter.InsertCommand.Parameters["@month"].Value = rowDatas[4];
            dataAdapter.InsertCommand.Parameters["@day"].Value = rowDatas[5];

            try
            {
                conn.Open();
                dataAdapter.InsertCommand.ExecuteNonQuery();

                dataSet.Clear();                                       
                dataAdapter.Fill(dataSet, "orders");                      
                dataGridView1.DataSource = dataSet.Tables["orders"];      
            }
            catch (Exception)
            {
                MessageBox.Show("해당 고객이나 책이 존재하지 않습니다.");
            }
            finally
            {
                conn.Close();
            }
        }

        internal void DeleteRow(string id)
        {
            string sql = "DELETE FROM orders WHERE orderid=@orderid";
            dataAdapter.DeleteCommand = new MySqlCommand(sql, conn);
            dataAdapter.DeleteCommand.Parameters.AddWithValue("@orderid", id);

            try
            {
                conn.Open();
                dataAdapter.DeleteCommand.ExecuteNonQuery();

                dataSet.Clear();
                dataAdapter.Fill(dataSet, "orders");
                dataGridView1.DataSource = dataSet.Tables["orders"];
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
            string sql = "UPDATE orders SET custid=@custid, bookid=@bookid, saleprice=@saleprice, year=@year, month=@month, day=@day WHERE orderid=@orderid";
            dataAdapter.UpdateCommand = new MySqlCommand(sql, conn);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@orderid", rowDatas[0]);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@custid", rowDatas[1]);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@bookid", rowDatas[2]);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@saleprice", rowDatas[3]);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@year", rowDatas[4]);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@month", rowDatas[5]);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@day", rowDatas[6]);


            try
            {
                conn.Open();
                dataAdapter.UpdateCommand.ExecuteNonQuery();

                dataSet.Clear();  
                dataAdapter.Fill(dataSet, "orders");
                dataGridView1.DataSource = dataSet.Tables["orders"];
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
            OrderAs Dig = new OrderAs();
            Dig.Owner = this;               
            Dig.ShowDialog();               
            Dig.Dispose();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBoxOrderID.Clear();
            textBoxCustID.Clear();
            textBoxBookID.Clear();  
            textBoxMin.Clear();
            textBoxMax.Clear();
            textBoxYear.Clear();
            textBoxMonth.Clear();
            textBoxDay.Clear();
        }

    }
}
