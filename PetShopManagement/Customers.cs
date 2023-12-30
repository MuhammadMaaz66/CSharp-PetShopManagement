﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace PetShopManagement
{
    public partial class Customers : Form
    {
        public Customers()
        {
            InitializeComponent();
            DisplayCustomers();
        }
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\maazh\OneDrive\Documents\PetShopDb.mdf;Integrated Security=True;Connect Timeout=30");
        int key = 0;
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void DisplayCustomers()
        {
            try
            {
                conn.Open();
                string Query = "Select * from CustomerTable";
                SqlDataAdapter sda = new SqlDataAdapter(Query, conn);
                SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                CustomerDGV.DataSource = ds.Tables[0];
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Clear()
        {
            try
            {
                CustomerNameTb.Text = "";
                CustomerAddressTb.Text = "";
                CustomerPhoneTb.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (CustomerNameTb.Text == "" || CustomerAddressTb.Text == "" || CustomerPhoneTb.Text == "")
            {
                MessageBox.Show("Missing Information!");
            }
            else
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("insert into CustomerTable (CustName, CustAdd, CustPhone) values(@CN, @CA, @CP)", conn);
                    cmd.Parameters.AddWithValue("@CN", CustomerNameTb.Text);
                    cmd.Parameters.AddWithValue("@CA", CustomerAddressTb.Text);
                    cmd.Parameters.AddWithValue("@CP", CustomerPhoneTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Added!");
                    conn.Close();
                    DisplayCustomers();
                    Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void CustomerDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (CustomerDGV.SelectedRows.Count > 0)
            {
                key = Convert.ToInt32(CustomerDGV.SelectedRows[0].Cells[0].Value.ToString());
                CustomerNameTb.Text = CustomerDGV.SelectedRows[0].Cells[1].Value.ToString();
                CustomerAddressTb.Text = CustomerDGV.SelectedRows[0].Cells[3].Value.ToString();
                CustomerPhoneTb.Text = CustomerDGV.SelectedRows[0].Cells[2].Value.ToString();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select A Customer!");
            }
            else
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("delete from CustomerTable where CustId = @CKey", conn);
                    cmd.Parameters.AddWithValue("@CKey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Deleted!");
                    conn.Close();
                    DisplayCustomers();
                    Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Employees obj = new Employees();
            obj.Show();
            this.Hide();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (CustomerNameTb.Text == "" || CustomerAddressTb.Text == "" || CustomerPhoneTb.Text == "")
            {
                MessageBox.Show("Missing Information!");
            }
            else
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Update CustomerTable set CustName=@CN, CustAdd=@CA, CustPhone=@CP where CustId=@CKey", conn);
                    cmd.Parameters.AddWithValue("@CN", CustomerNameTb.Text);
                    cmd.Parameters.AddWithValue("@CA", CustomerAddressTb.Text);
                    cmd.Parameters.AddWithValue("@CP", CustomerPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@CKey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Updated!");
                    conn.Close();
                    DisplayCustomers();
                    Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Products obj = new Products();
            obj.Show();
            this.Hide();
        }
    }
}
