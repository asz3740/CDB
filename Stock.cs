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
    public partial class Stock : Form
    {
        MySqlConnection conn;
        MySqlDataAdapter dataAdapter;
        DataSet dataSet;
        int selectedRowIndex;

        public Stock()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string connStr = "server=localhost;port=3307;database=book;uid=root;pwd=1234";
            conn = new MySqlConnection(connStr);
            dataAdapter = new MySqlDataAdapter("SELECT * FROM book", conn);
            dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "book");
            dataGridView1.DataSource = dataSet.Tables["book"];

            SetSearchComboBox();
        }

        private void SetSearchComboBox()
        {
            string sql = "SELECT distinct publisher FROM book";
            MySqlCommand cmd = new MySqlCommand(sql, conn);

            try
            {
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())  
                {
                    cbPublisher.Items.Add(reader.GetString("publisher"));
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
            conditions[0] = (textBoxID.Text != "") ? "bookid=@bookid" : null;
            conditions[1] = (textBoxName.Text != "") ? "bookname=@bookname" : null;
            conditions[2] = (textBoxAuthor.Text != "") ? "author=@author" : null;
            conditions[3] = (cbPublisher.Text != "") ? "publisher=@publisher" : null;       
            conditions[5] = (textBoxStock.Text != "") ? "stock=@stock" : null;
            string condition_price;
            if (textBoxMin.Text != "" && textBoxMax.Text != "")
            {
                condition_price = "price>=@min and price<=@max";
            }
            else if (textBoxMin.Text != "" || textBoxMax.Text != "")
            {
                if (textBoxMin.Text != "")
                    condition_price = "price >= @min";
                else
                    condition_price = "price <= @max";
            }
            else
            {
                condition_price = null;
            }
            conditions[4] = condition_price;

            if (conditions[0] != null || conditions[1] != null || conditions[2] != null || conditions[3] != null || conditions[4] != null || conditions[5] != null)
            {
                queryStr = $"SELECT * FROM book WHERE ";
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
                queryStr = "SELECT * FROM book";
            }

            dataAdapter.SelectCommand = new MySqlCommand(queryStr, conn);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@bookid", textBoxID.Text);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@bookname", textBoxName.Text);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@author", textBoxAuthor.Text);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@publisher", cbPublisher.Text);        
            dataAdapter.SelectCommand.Parameters.AddWithValue("@min", textBoxMin.Text);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@max", textBoxMax.Text);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@stock", textBoxStock.Text);

            try
            {
                conn.Open();
                dataSet.Clear();
                if (dataAdapter.Fill(dataSet, "book") > 0)
                    dataGridView1.DataSource = dataSet.Tables["book"];
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

            StockAs Dig = new StockAs(
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
            string queryStr = "INSERT INTO book (bookname, author, publisher, price, stock)"   +         
                "VALUES(@bookname, @author, @publisher, @price, @stock)";
            dataAdapter.InsertCommand = new MySqlCommand(queryStr, conn);
            dataAdapter.InsertCommand.Parameters.Add("@bookname", MySqlDbType.VarChar);
            dataAdapter.InsertCommand.Parameters.Add("@author", MySqlDbType.VarChar);
            dataAdapter.InsertCommand.Parameters.Add("@publisher", MySqlDbType.VarChar);
            dataAdapter.InsertCommand.Parameters.Add("@price", MySqlDbType.Int32);
            dataAdapter.InsertCommand.Parameters.Add("@stock", MySqlDbType.Int32);

            dataAdapter.InsertCommand.Parameters["@bookname"].Value = rowDatas[0];
            dataAdapter.InsertCommand.Parameters["@author"].Value = rowDatas[1];
            dataAdapter.InsertCommand.Parameters["@publisher"].Value = rowDatas[2];
            dataAdapter.InsertCommand.Parameters["@price"].Value = rowDatas[3];
            dataAdapter.InsertCommand.Parameters["@stock"].Value = rowDatas[4];

            try
            {
                conn.Open();
                dataAdapter.InsertCommand.ExecuteNonQuery();

                dataSet.Clear();                                        
                dataAdapter.Fill(dataSet, "book");                    
                dataGridView1.DataSource = dataSet.Tables["book"];      
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

        internal void DeleteRow(string id)
        {
            string sql = "DELETE FROM book WHERE bookid=@bookid";
            dataAdapter.DeleteCommand = new MySqlCommand(sql, conn);
            dataAdapter.DeleteCommand.Parameters.AddWithValue("@bookid", id);

            try
            {
                conn.Open();
                dataAdapter.DeleteCommand.ExecuteNonQuery();

                dataSet.Clear();
                dataAdapter.Fill(dataSet, "book");
                dataGridView1.DataSource = dataSet.Tables["book"];
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
            string sql = "UPDATE book SET bookname=@bookname, author=@author, publisher=@publisher, price=@price, stock=@stock WHERE bookid=@bookid";
            dataAdapter.UpdateCommand = new MySqlCommand(sql, conn);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@bookid", rowDatas[0]);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@bookname", rowDatas[1]);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@author", rowDatas[2]);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@publisher", rowDatas[3]);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@price", rowDatas[4]);
            dataAdapter.UpdateCommand.Parameters.AddWithValue("@stock", rowDatas[5]);

            try
            {
                conn.Open();
                dataAdapter.UpdateCommand.ExecuteNonQuery();

                dataSet.Clear(); 
                dataAdapter.Fill(dataSet, "book");
                dataGridView1.DataSource = dataSet.Tables["book"];
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
            StockAs Dig = new StockAs();
            Dig.Owner = this;               
            Dig.ShowDialog();              
            Dig.Dispose();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBoxID.Clear();
            textBoxName.Clear();
            textBoxAuthor.Clear();
            cbPublisher.Text = "";
            textBoxMin.Clear();
            textBoxMax.Clear();
            textBoxStock.Clear();
        }
    }
}

