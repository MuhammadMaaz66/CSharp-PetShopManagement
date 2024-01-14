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
using System.Windows.Input;
using static PetShopManagement.Login;

namespace PetShopManagement
{
    public partial class Billings : Form
    {
        private User loggedInUser;
        public Billings()
        {
            InitializeComponent();
            //EmpNameLabel.Text = Login.Employee;
            GetCustomers();
            DisplayProduct();
            //GetBillData(); // uncomment when want to show on the startup
            loggedInUser = UserManager.LoggedInUser;
            if (loggedInUser != null)
            {
                // You can use the user details as needed
                int userId = loggedInUser.UserId;
                string username = loggedInUser.Username;

                // Example: Display the username in a label
                label7.Text = username;
            }
            else
            {
                // User is not logged in
                // Handle the case where no user is logged in
            }
        }
        int Key = 0, Stock = 0;
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\maazh\OneDrive\Documents\PetShopDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void label11_Click(object sender, EventArgs e)
        {

        }
        private void GetCustomers()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("Select CustId from CustomerTable", conn);
            SqlDataReader Rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CustId", typeof(int));
            dt.Load(Rdr);
            CategoryCB.ValueMember = "CustId";
            CategoryCB.DataSource = dt;
            conn.Close();
        }
        private void DisplayProduct()
        {
            conn.Open();
            string Query = "Select * from ProductTable";
            SqlDataAdapter sda = new SqlDataAdapter(Query, conn);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ProductsDGV.DataSource = ds.Tables[0];
            conn.Close();
        }
        private void Reset()
        {
            ProdNameTb.Text = "";
            QuantityTb.Text = "";
            Stock = 0;
            Key = 0;
        }
        private void GetCustName()
        {
            conn.Open();
            string Query = "Select * from CustomerTable where CustId="+CategoryCB.SelectedValue.ToString();
            SqlCommand cmd = new SqlCommand(Query, conn);
            DataTable dt = new DataTable();
            SqlDataAdapter sda  = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                CustomerNameTb.Text = dr["CustName"].ToString();
            }
            conn.Close();
        }
        private void UpdateStock()
        {
            try { 
                int NewQty = Stock - Convert.ToInt32(QuantityTb.Text);
                conn.Open();
                SqlCommand cmd = new SqlCommand("Update ProductTable set ProdQty=@PQ where ProdId=@PKey", conn);
                cmd.Parameters.AddWithValue("@PQ", NewQty);
                cmd.Parameters.AddWithValue("@PKey", Key);
                cmd.ExecuteNonQuery();
                conn.Close();
                DisplayProduct();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        int n = 0, GrandTotal = 0;

        private void ProductsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ProductsDGV.SelectedRows.Count > 0)
            {
                Key = Convert.ToInt32(ProductsDGV.SelectedRows[0].Cells[0].Value.ToString());
                ProdNameTb.Text = ProductsDGV.SelectedRows[0].Cells[1].Value.ToString();
                //CategoryCB.Text = ProductsDGV.SelectedRows[0].Cells[2].Value.ToString();
                Stock = Convert.ToInt32(ProductsDGV.SelectedRows[0].Cells[3].Value.ToString());
                PriceTb.Text = ProductsDGV.SelectedRows[0].Cells[4].Value.ToString();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            Products obj = new Products();
            obj.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Customers obj = new Customers();
            obj.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Employees obj = new Employees();
            obj.Show();
            this.Hide();
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            Reset();
        }
        string prodName;
        private void PrintButton_Click(object sender, EventArgs e)
        {
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 600, 1000);
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }
        int prodId, prodQty, prodPrice, total, pos = 60;

        private void label6_Click(object sender, EventArgs e)
        {
            UserManager.Logout();

            // Navigate to the login form or perform any other necessary actions
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Home obj = new Home();
            obj.Show();
            this.Hide();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();
            int fullScreenWidth = e.PageBounds.Width; // Get full screen width
            e.Graphics.DrawString("My Pet Shop", new Font("Cambria", 12, FontStyle.Bold), Brushes.Red, new Point(76, 10));
            e.Graphics.DrawString("ID         PRODUCT               PRICE   QUANTITY TOTAL", new Font("Cambria", 10, FontStyle.Bold), Brushes.Red, new Point(26, 40));
            foreach (DataGridViewRow row in BillDGV.Rows)
            {
                prodId = Convert.ToInt32(row.Cells["BNum"].Value);
                    prodName = "" + row.Cells["ProdName"].Value;
                    prodPrice = Convert.ToInt32(row.Cells["Price"].Value);
                    prodQty = Convert.ToInt32(row.Cells["Quantity"].Value);
                    total = Convert.ToInt32(row.Cells["Total"].Value);
                    e.Graphics.DrawString("" + prodId.ToString(), new Font("Cambria", 8, FontStyle.Bold), Brushes.Blue, new Point(26, pos));
                    e.Graphics.DrawString("" + prodName, new Font("Cambria", 8, FontStyle.Bold), Brushes.Blue, new Point(60, pos));
                    e.Graphics.DrawString("" + prodPrice.ToString(), new Font("Cambria", 8, FontStyle.Bold), Brushes.Blue, new Point(180, pos));
                    e.Graphics.DrawString("" + prodQty.ToString(), new Font("Cambria", 8, FontStyle.Bold), Brushes.Blue, new Point(250, pos));
                    e.Graphics.DrawString("" + total.ToString(), new Font("Cambria", 8, FontStyle.Bold), Brushes.Blue, new Point(300, pos));
                    pos = pos + 20;
                    rowsToRemove.Add(row);
                }
                e.Graphics.DrawString("Grand Total: Rs" + GrandTotal, new Font("Cambria", 12, FontStyle.Bold), Brushes.Crimson, new Point(50, pos + 50));
                e.Graphics.DrawString("***************** PetShop *****************", new Font("Cambria", 12, FontStyle.Bold), Brushes.Crimson, new Point(10, pos + 85));
            foreach (DataGridViewRow rowToRemove in rowsToRemove)
            {
                if (!rowToRemove.IsNewRow)
                {
                    BillDGV.Rows.Remove(rowToRemove);
                }
            }
            //BillDGV.Rows.Clear();
            BillDGV.Refresh();
            pos = 100;
            GrandTotal = 0;
            n = 0;
        }

        private void GetBillData() {
            try
            {
                conn.Open();
                string Query = "Select * from BillTable";
                SqlDataAdapter sda = new SqlDataAdapter(Query, conn);
                SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                BillDGV.DataSource = ds.Tables[0];
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bunifuThinButton23_Click(object sender, EventArgs e)
        {
            if (QuantityTb.Text == "" || Convert.ToInt32(QuantityTb.Text)>Stock)
            {
                MessageBox.Show("Not Enough Items");
            } else if (QuantityTb.Text == "" || Key == 0)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                ProductsDGV.DataSource = null;
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ID", typeof(int));         // Product ID
                dataTable.Columns.Add("ProductName", typeof(string)); // Product name
                dataTable.Columns.Add("Quantity", typeof(int));    // Quantity
                dataTable.Columns.Add("Price", typeof(decimal));   // Price
                dataTable.Columns.Add("Total", typeof(decimal));   // Total

                int total = Convert.ToInt32(QuantityTb.Text)*Convert.ToInt32(PriceTb.Text);
                //DataGridViewRow newRow = new DataGridViewRow();
                DataRow newRow = dataTable.NewRow();
                //newRow.CreateCells(ProductsDGV);
                //newRow.Cells[0].Value = n+1;
                //newRow.Cells[1].Value = ProdNameTb.Text;
                //newRow.Cells[2].Value = QuantityTb.Text;
                //newRow.Cells[3].Value = PriceTb.Text;
                //newRow.Cells[4].Value = total;
                newRow["ID"] = n + 1;                          // Assuming n is the product ID
                newRow["ProductName"] = ProdNameTb.Text;       // Product name from TextBox
                newRow["Quantity"] = Convert.ToInt32(QuantityTb.Text); // Quantity from TextBox
                newRow["Price"] = Convert.ToDecimal(PriceTb.Text);     // Price from TextBox
                newRow["Total"] = total; // Calculated total
                dataTable.Rows.Add(newRow);
                // Rebind the data source to the DataGridView
                ProductsDGV.DataSource = dataTable;
                GrandTotal = GrandTotal + total;
                /// code to store bill to database
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into BillTable (ProdName, Quantity, Price, Total) values(@PN, @BQ, @BP, @BT)", conn);
                //cmd.Parameters.AddWithValue("@BN", n + 1);
                cmd.Parameters.AddWithValue("@PN", ProdNameTb.Text);
                cmd.Parameters.AddWithValue("@BQ", Convert.ToInt32(QuantityTb.Text));
                cmd.Parameters.AddWithValue("@BP", Convert.ToDecimal(PriceTb.Text));
                cmd.Parameters.AddWithValue("@BT", total);
                cmd.ExecuteNonQuery();
                //MessageBox.Show("Bill Added!");
                conn.Close();
                //ProductsDGV.Rows.Add(newRow);
                n++;
                TotalLabel.Text = GrandTotal.ToString();
                GetBillData();
                UpdateStock();
                Reset();
            }
        }
    }
}
