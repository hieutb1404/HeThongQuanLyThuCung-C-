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

namespace QuanLyThuCung
{
    public partial class Billings : Form
    {
        public Billings()
        {
            InitializeComponent();
            EmpNameLbl.Text = Login.LoggedInUser;
            RolsLB.Text = Login.Rols;
            GetCustomers();
            ShowProduct();
            showhistorybill();


        }
        // kết nối cơ sở dữ liệu
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\trung hieu\Documents\DBshopthucung.mdf;Integrated Security=True;Connect Timeout=30");

        private void GetCustomers()
        {
            // mở kết nối CSDL
            Con.Open();
            // thực hiện truy vấn lấy thông tin từ bảng khách hàng, và được truyền qua tham số đầu tiên của Sqlcommnad
            SqlCommand cmd = new SqlCommand("select CustId from CustomerTbl", Con);
            // khai báo SqlDataReader đọc dữ liệu trả về từ câu truy vấn (dữ liệu là cột CustId được truy vấn ở trên)
            // ở đây nó chưa được đọc vì chưa có câu lệnh sql nào thực thi gán cho nó
            SqlDataReader Rdr;
            // ExecuteReader() thực thi câu lệnh sql và sau đó truyền vào cho thằng Rdr đọc dữ liệu từng dòng 1
            Rdr = cmd.ExecuteReader();
            //Sau khi đọc dữ liệu xong, đoạn mã tạo đối tượng DataTable để lưu trữ thông tin khách hàng.
            // lưu trữ bằng cách add vào dt
            //Đối tượng DataTable này chứa một cột "CustID" kiểu int.
            DataTable dt = new DataTable();
            dt.Columns.Add("CustID", typeof(int));
            //Dữ liệu được lưu trữ trong đối tượng SqlDataReader sẽ được đưa vào đối tượng DataTable
            //bằng cách gọi phương thức Load(Rdr) của đối tượng DataTable.
            dt.Load(Rdr);
            //CustIdCb là một điều khiển ComboBox và thuộc tính ValueMember đang được sử dụng để chỉ định
            //rằng giá trị của mỗi mục trong ComboBox sẽ là giá trị của cột "CustId" trong bảng dữ liệu được liên kết.
            CustIdCb.ValueMember = "CustId";
            // sau đó đổ dữ liệu vào comboboux
            CustIdCb.DataSource = dt;
            Con.Close();
        }



        private void ShowProduct()
        {
            Con.Open();
            string Query = "select * from ProductTbl";
           // đối tượng SqlDataAdapter,  được sử dụng để lấy dữ liệu từ cơ sở dữ liệu và đưa nó vào DataSet.
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            //SqlCommandBuilder để xây dựng các lệnh SQL tự động,
            //giúp thực hiện các thao tác Insert, Update, Delete trong DataSet.
            SqlCommandBuilder Builder = new SqlCommandBuilder();
            //đối tượng DataSet, sẽ lưu trữ dữ liệu lấy được từ bảng ProductTbl.
            var ds = new DataSet();
            // đổ dữ liệu từ sda sau khi thực thi vào dataset (ds)
            sda.Fill(ds);
            // vì sao là[0], vì trong dataset có thể chứa nhiều bảng dataTable
            //Fill() để lấy dữ liệu từ database, đã tự động tạo ra một bảng DataTable đầu tiên trong DataSet.
            //Do đó, ds.Tables[0] là cách để truy xuất đến bảng đầu tiên trong DataSet ds.
            ProductsDGV.DataSource = ds.Tables[0];
            Con.Close();

        }
        // lịch sử mua hàng
        private void showhistorybill()
        {
            Con.Open();
            string Query = "select * from BillTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder();
            var ds = new DataSet();
            sda.Fill(ds);
            HistoryBill.DataSource = ds.Tables[0];
            Con.Close();

        }

        // lấy tên theo id
        private void GetCustName()
        {
            Con.Open();
            //điều kiện là CustId trùng với giá trị được chọn từ CustIdCb (combobox) , ở đã đã xử lý 
            string Query = "Select * from CustomerTbl where CustId ='" + CustIdCb.SelectedValue.ToString() + "'";
            //SqlCommand để thực thi câu truy vấn Query trên kết nối Con.
            SqlCommand cmd = new SqlCommand(Query,Con);
            //Tạo một bảng dữ liệu mới.
            DataTable dt = new DataTable();
            // thực hiện truy vấn cmd và điền dữ liệu vào bảng dt.
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            // lặp mỗi từng dòng dữ liệu đưa vào dr
            foreach(DataRow dr in dt.Rows)
            {
                // giá trị của cột CustName trong dòng hiện tại vào TextBox CustNameTb.
                CustNameTb.Text = dr["CustName"].ToString();
            }
            Con.Close();
        }

        private void Reset()
        {
            PrNameTb.Text = "";
            QtyTb.Text = "";
            // Stock = 0 nhưng hàm Showproduct vẫn được gọi nên nó vẫn đc gán giá trị của PrQty
            Stock = 0;
            Key = 0;
        }

        // khai báo biến lưu trữ Stock = 0, vì biến Stock giống với kiểu dữ liệu của ProductTbl
        // nên khi thằng showProduct() được gọi nó sẽ tìm đến PrQty(số lượng)
        // vì trong bảng ProductTbl chỉ có mỗi PrQty giống kiểu dữ liệu của biến toàn cục Stock
        // nên khi gọi ShowProduct() nó sẽ gán giá trị của PrQty vào Stock
        // khi đó Stock sẽ ko phải = 0 nữa, mà là = giá trị của PrQty
        int Stock=0 , Key = 0;
        private void UpdateStock()
        {
            try
            {
                // stock - số lượng trong textbox được truyền
                int NewQty = Stock - Convert.ToInt32(QtyTb.Text);
                Con.Open();
                SqlCommand cmd = new SqlCommand("Update ProductTbl set PrQty = @PQ where PrId = @PKey", Con);
                cmd.Parameters.AddWithValue("@PQ",NewQty);
                cmd.Parameters.AddWithValue("@PKey", Key);
                cmd.ExecuteNonQuery();

                Con.Close();
                ShowProduct();

            } catch(Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
        int n = 0, GrdTotal = 0;

        private void addToBill_Click(object sender, EventArgs e)
        {
            if (QtyTb.Text == "" || Convert.ToInt32(QtyTb.Text) > Stock)
            {
                MessageBox.Show("Số lượng thú cưng hiện tại không đủ");
            }
            else if (QtyTb.Text == "" || Key == 0)
            {
                MessageBox.Show("ko thấy");
            }
            else
            {
                int total = Convert.ToInt32(QtyTb.Text) * Convert.ToInt32(PrPriceTb.Text);
                bool productExists = false;
                foreach (DataGridViewRow row in BillDGV.Rows)
                {
                    if (row.Cells[1].Value != null && row.Cells[1].Value.ToString() == PrNameTb.Text)
                    {
                        int newQty = Convert.ToInt32(row.Cells[3].Value) + Convert.ToInt32(QtyTb.Text);
                        int newTotal = newQty * Convert.ToInt32(PrPriceTb.Text);
                        row.Cells[3].Value = newQty;
                        row.Cells[4].Value = newTotal;
                        GrdTotal += newTotal;
                        productExists = true;
                        break;
                    }
                }
                if (!productExists)
                {
                    DataGridViewRow newRow = new DataGridViewRow();
                    newRow.CreateCells(BillDGV);
                    newRow.Cells[0].Value = n + 1;
                    newRow.Cells[1].Value = PrNameTb.Text;
                    newRow.Cells[2].Value = PrPriceTb.Text;
                    newRow.Cells[3].Value = QtyTb.Text;
                    newRow.Cells[4].Value = total;
                    GrdTotal += total;
                    BillDGV.Rows.Add(newRow);
                    n++;
                }
                TotalLbl.Text = "Thành tiền: " + GrdTotal;
                UpdateStock();
                Reset();
            }
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void CustIdCb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CustIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetCustName();

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            InsertBill();
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm",285,600);
            if(printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            Employees Obj = new Employees();
            if (RolsLB.Text == "admin")
            {
                Obj.Show();
                this.Hide();

            }
            else
            {
                MessageBox.Show("ban khong co quyen truy cap");
            }
            // this ở đây chính là tham chiếu của thằng hiện tại... ẩn Employees
        }

        private void label4_Click(object sender, EventArgs e)
        {
            customers Obj = new customers();
            Obj.Show();
            // this ở đây chính là tham chiếu của thằng hiện tại... ẩn Employees
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            // this ở đây chính là tham chiếu của thằng hiện tại... ẩn Employees
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Homes Obj = new Homes();
            Obj.Show();
            // this ở đây chính là tham chiếu của thằng hiện tại... ẩn Employees
            this.Hide();
        }

        private void UpdateDatabase()
        {
            Con.Open();

            foreach (DataGridViewRow row in BillDGV.Rows)
            {
                if (row.Cells[1].Value != null)
                {
                 string productName = row.Cells[1].Value.ToString();
                int quantity = Convert.ToInt32(row.Cells[3].Value);

                SqlCommand cmd = new SqlCommand("UPDATE ProductTbl SET PrQty = @Stock WHERE PrName = @ProductName", Con);
                cmd.Parameters.AddWithValue("@Stock", quantity);
                cmd.Parameters.AddWithValue("@ProductName", productName);

                cmd.ExecuteNonQuery();

                }
            }

            Con.Close();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (BillDGV.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = BillDGV.SelectedRows[0];
                int selectedQty = Convert.ToInt32(selectedRow.Cells[3].Value);
                int selectedTotal = Convert.ToInt32(selectedRow.Cells[4].Value);
                GrdTotal -= selectedTotal;
                BillDGV.Rows.Remove(selectedRow);

                // Cập nhật lại số lượng hàng hóa trong giỏ hàng
                foreach (DataGridViewRow row in ProductsDGV.Rows)
                {
                    if (row.Cells[1].Value != null && row.Cells[1].Value.ToString() == selectedRow.Cells[1].Value.ToString())
                    {
                        int currentQty = Convert.ToInt32(row.Cells[3].Value);
                        int newQty = currentQty + selectedQty;
                        row.Cells[3].Value = newQty;
                        break;
                    }
                }
                // Cập nhật lại tổng cộng và hiển thị
                GrdTotal = 0;
                foreach (DataGridViewRow row in BillDGV.Rows)
                {
                    int currentQty = Convert.ToInt32(row.Cells[3].Value);
                    int currentPrice = Convert.ToInt32(row.Cells[2].Value);
                    int currentTotal = currentQty * currentPrice;
                    row.Cells[4].Value = currentTotal;
                    GrdTotal += currentTotal;
                }
                TotalLbl.Text = "Thành tiền: " + GrdTotal;
                UpdateDatabase();
                Reset();
            }
        }

        string prodname;
        // thiet ke giao dien hoa don
        int prodid, prodqty, prodprice, total, pos = 60;
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Cửa hàng thú cưng", new Font("Times New Roman", 12, FontStyle.Bold), Brushes.Red, new Point(80));
                                                                                                                               // X,Y     
            e.Graphics.DrawString("Hóa đơn thanh toán", new Font("Times New Roman", 10, FontStyle.Bold), Brushes.Red, new Point(26,20));
            e.Graphics.DrawString("ID", new Font("Times New Roman", 8, FontStyle.Bold), Brushes.Black, new PointF(26, pos - 10));
            e.Graphics.DrawString("Product Name", new Font("Times New Roman", 8, FontStyle.Bold), Brushes.Black, new PointF(70, pos - 10));
            e.Graphics.DrawString("Price", new Font("Times New Roman", 8, FontStyle.Bold), Brushes.Black, new PointF(170, pos - 10));
            e.Graphics.DrawString("Quantity", new Font("Times New Roman", 8, FontStyle.Bold), Brushes.Black, new PointF(220, pos - 10));
            e.Graphics.DrawString("Total", new Font("Times New Roman", 8, FontStyle.Bold), Brushes.Black, new PointF(290, pos - 10));
            foreach (DataGridViewRow row in BillDGV.Rows)
            {
                prodid = Convert.ToInt32(row.Cells["idColumn"].Value);
                prodname = "" + row.Cells["nameColumn"].Value;
                prodprice = Convert.ToInt32(row.Cells["priceColumn"].Value);
                prodqty = Convert.ToInt32(row.Cells["quantityColumn"].Value);
                total = Convert.ToInt32(row.Cells["totalColumn"].Value);
                e.Graphics.DrawString("" + prodid, new Font("Times New Roman", 8, FontStyle.Bold), Brushes.Blue, new Point(26, pos+10));
                e.Graphics.DrawString("" + prodname, new Font("Times New Roman", 8, FontStyle.Bold), Brushes.Blue, new Point(70, pos+10));
                e.Graphics.DrawString("" + prodprice, new Font("Times New Roman", 8, FontStyle.Bold), Brushes.Blue, new Point(170, pos+10));
                e.Graphics.DrawString("" + prodqty, new Font("Times New Roman", 8, FontStyle.Bold), Brushes.Blue, new Point(220, pos+10));
                e.Graphics.DrawString("" + total, new Font("Times New Roman", 8, FontStyle.Bold), Brushes.Blue, new Point(290, pos+10));
                pos = pos + 20;
            }
            e.Graphics.DrawString("Tổng tiền: " + GrdTotal, new Font("Times New Roman", 12, FontStyle.Bold), Brushes.Crimson, new Point(50, pos + 50));
            e.Graphics.DrawString("****Shop Thú Cưng****" , new Font("Times New Roman", 12, FontStyle.Bold), Brushes.Crimson, new Point(10, pos + 85));
            BillDGV.Rows.Clear();
            BillDGV.Refresh();
            pos = 100;
            GrdTotal = 0;
            n = 0;
        }

        private void InsertBill()
        {
            try
            {
                Con.Open();
                // thực thi truy vấn sql Sqlcommand
                SqlCommand cmd = new SqlCommand("insert into BillTbl (BDate,CustId,CustName,EmpName,Amt) Values(@BD,@CI,@CN,@EN,@Am)", Con);
                //truyền tham số  và texbox sau khi có data truyền ngược lên tham số values
                // tham số 1 : tham số cần lấy
                // tham số 2 : tham số truyền dữ liệu
                cmd.Parameters.AddWithValue("@BD", DateTime.Today.Date);
                cmd.Parameters.AddWithValue("@CI", CustIdCb.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@CN", CustNameTb.Text);
                cmd.Parameters.AddWithValue("@EN", EmpNameLbl.Text);
                cmd.Parameters.AddWithValue("@Am", GrdTotal);

                // thuc thi query insert
                //Hàm này không trả về bất kỳ dữ liệu nào và được sử dụng để thực hiện các thao tác không yêu cầu kết quả trả về từ CSDL.  
                cmd.ExecuteNonQuery();
                MessageBox.Show("đã lưu hóa đơn");
                Con.Close();
                // reset lại danh sách
                showhistorybill();
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void ProductsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ProductsDGV.SelectedRows.Count > 0)
            {
                PrNameTb.Text = ProductsDGV.SelectedRows[0].Cells[1].Value.ToString();
                Stock = Convert.ToInt32(ProductsDGV.SelectedRows[0].Cells[3].Value.ToString());
                PrPriceTb.Text = ProductsDGV.SelectedRows[0].Cells[4].Value.ToString();
                if (PrNameTb.Text == "")
                {
                    Key = 0;
                }
                else
                {
                    //này có nhiệm vụ lấy giá trị mã số nhân viên được chọn từ DataGridView và chuyển đổi thành một số nguyên.
                    Key = Convert.ToInt32(ProductsDGV.SelectedRows[0].Cells[0].Value.ToString());
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Products Obj = new Products();
            Obj.Show();
            // this ở đây chính là tham chiếu của thằng hiện tại... ẩn Employees
            this.Hide();
        }
    }
}
