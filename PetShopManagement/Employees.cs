using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;

namespace PetShopManagement
{
    public partial class Employees : Form
    {
        public Employees()
        {
            InitializeComponent();
            DisplayEmployees();
        }
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\maazh\OneDrive\Documents\PetShopDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (EmpNameTb.Text == "" || EmpAddTb.Text == "" || EmpPhoneTb.Text == "" || EmpPassTb.Text == "")
            {
                MessageBox.Show("Missing Information!");
            }
            else
            {
                try
                {
                    conn.Open();
                    // Convert the string to DateTime
                    Console.WriteLine("Date string to parse: " + EmpDOB.Text.Trim());
                    DateTime empDOB = DateTime.ParseExact(EmpDOB.Text.Trim(), "dddd, MMMM dd, yyyy", CultureInfo.InvariantCulture);
                    Console.WriteLine("Date string: " + empDOB);
                    // EmpDOB @ED, ==> insert these both in line below...
                    SqlCommand cmd = new SqlCommand("Update EmployeeTable set EmpName=@EN, EmpAdd=@EA, EmpPhone=@EP, EmpPass=@EPa where EmpNum=@EKey", conn);
                    cmd.Parameters.AddWithValue("@EN", EmpNameTb.Text);
                    cmd.Parameters.AddWithValue("@EA", EmpAddTb.Text);
                    //cmd.Parameters.AddWithValue("@ED", EmpDOB.Text);
                    cmd.Parameters.AddWithValue("@EP", EmpPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@EPa", EmpPassTb.Text);
                    cmd.Parameters.AddWithValue("@EKey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Employee Updated!");
                    conn.Close();
                    DisplayEmployees();
                    Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        
        private void DisplayEmployees()
        {
            try
            {
                conn.Open();
                string Query = "Select * from EmployeeTable";
                SqlDataAdapter sda = new SqlDataAdapter(Query, conn);
                SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                EmployeesDGV.DataSource = ds.Tables[0];
                conn.Close();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Clear()
        {
            try
            {
                EmpNameTb.Text = "";
                EmpPassTb.Text = "";
                EmpPhoneTb.Text = "";
                EmpAddTb.Text = "";
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (EmpNameTb.Text == "" || EmpAddTb.Text == "" || EmpPhoneTb.Text == "" || EmpPassTb.Text == "")
            {
                MessageBox.Show("Missing Information!");
            } else
            {
                try 
                {
                    conn.Open();
                    // Convert the string to DateTime
                    Console.WriteLine("Date string to parse: " + EmpDOB.Text.Trim());
                    DateTime empDOB = DateTime.ParseExact(EmpDOB.Text.Trim(), "dddd, MMMM dd, yyyy", CultureInfo.InvariantCulture);
                    Console.WriteLine("Date string: " + empDOB);
                    // EmpDOB @ED, ==> insert these both in line below...
                    SqlCommand cmd = new SqlCommand("insert into EmployeeTable (EmpName, EmpAdd, EmpPhone, EmpPass ) values(@EN, @EA, @EP, @EPa)", conn);
                    cmd.Parameters.AddWithValue("@EN", EmpNameTb.Text);
                    cmd.Parameters.AddWithValue("@EA", EmpAddTb.Text);
                    //cmd.Parameters.AddWithValue("@ED", EmpDOB.Text);
                    cmd.Parameters.AddWithValue("@EP", EmpPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@EPa", EmpPassTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Employee Added!");
                    conn.Close();
                    DisplayEmployees();
                    Clear();
                } catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        int key = 0;
        private void EmployeesDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (EmployeesDGV.SelectedRows.Count > 0)
            {
                key = Convert.ToInt32(EmployeesDGV.SelectedRows[0].Cells[0].Value.ToString());
                EmpNameTb.Text = EmployeesDGV.SelectedRows[0].Cells[1].Value.ToString();
                EmpAddTb.Text = EmployeesDGV.SelectedRows[0].Cells[2].Value.ToString();
                EmpPhoneTb.Text = EmployeesDGV.SelectedRows[0].Cells[3].Value.ToString();
                EmpPassTb.Text = EmployeesDGV.SelectedRows[0].Cells[4].Value.ToString();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select An Employee!");
            }
            else
            {
                try
                {
                    conn.Open();
                    // Convert the string to DateTime
                    Console.WriteLine("Date string to parse: " + EmpDOB.Text.Trim());
                    DateTime empDOB = DateTime.ParseExact(EmpDOB.Text.Trim(), "dddd, MMMM dd, yyyy", CultureInfo.InvariantCulture);
                    Console.WriteLine("Date string: " + empDOB);
                    // EmpDOB @ED, ==> insert these both in line below...
                    SqlCommand cmd = new SqlCommand("delete from EmployeeTable where EmpNum = @EmpKey", conn);
                    cmd.Parameters.AddWithValue("@EmpKey", key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Employee Deleted!");
                    conn.Close();
                    DisplayEmployees();
                    Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
