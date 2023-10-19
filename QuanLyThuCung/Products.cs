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
    // khac hieu
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
            ShowProducts();
            EmpNameLbl.Text = Login.LoggedInUser;
            RolsLB.Text = Login.Rols;

        }
        string conString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\trung hieu\Documents\DBshopthucung.mdf;Integrated Security=True;Connect Timeout=30";

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\trung hieu\Documents\DBshopthucung.mdf;Integrated Security=True;Connect Timeout=30");
        private void ShowProducts()
        {
            Con.Open();
            string Query = "Select * from ProductTbl";
            // truy vấn dữ liệu để đổ vào database vao dataset
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            // tự động cập nhật dữ liệu, khi có dữ liệu mới đổ vào
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            // dổi dữ liệu được truy vấn sda vào bảng ds 
            sda.Fill(ds);
            ProductsDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void Clear()
        {
            // reset textbox về ban đầu
            PrNameTb.Text = "";
            CatTb.SelectedIndex = 0;
            QtyTb.Text = "";
            PriceTb.Text = "";
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (PrNameTb.Text == "" || PriceTb.Text == "" || QtyTb.Text == "" || CatTb.SelectedIndex== -1)
            {
                MessageBox.Show("Thông tin chưa đầy đủ");
            }
            //Trong đó, hàm decimal.TryParse được sử dụng để chuyển đổi giá trị của PriceTb.Text sang kiểu decimal,
            //nếu giá trị này không hợp lệ thì điều kiện price < 0 sẽ trả về true. Nếu giá trị nhập vào không hợp lệ thì sẽ hiển thị thông báo lỗi.
            //Nếu giá trị hợp lệ thì sẽ tiếp tục thực hiện truy vấn SQL như bình thường.
            else if (!decimal.TryParse(PriceTb.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Giá sản phẩm không hợp lệ");
            }
            else if (!decimal.TryParse(QtyTb.Text, out decimal Quantity) || Quantity < 0)
            {
                MessageBox.Show("Số lượng không hợp lệ");
            }
            else
            {
                try
                {
                    using (SqlConnection Con = new SqlConnection(conString))
                    {
                        if (Con.State == ConnectionState.Closed)
                        {
                            Con.Open();
                        }


                        // Kiểm tra sản phẩm có tồn tại trong bảng không
                        SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM ProductTbl WHERE PrName=@PN", Con);
                        cmd.Parameters.AddWithValue("@PN", PrNameTb.Text);
                        int count = (int)cmd.ExecuteScalar();
                        if (count > 0)
                        {
                            // Sản phẩm đã tồn tại, cập nhật số lượng
                            cmd = new SqlCommand("UPDATE ProductTbl SET PrQty=PrQty+@PQ,PrPrice=PrPrice+@PP WHERE PrName=@PN", Con);
                            cmd.Parameters.AddWithValue("@PN", PrNameTb.Text);
                            cmd.Parameters.AddWithValue("@PQ", QtyTb.Text);
                            cmd.Parameters.AddWithValue("@PP", PriceTb.Text);

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Cập nhật số lượng và giá sản phẩm thành công");
                        }
                        else
                        {
                            // Sản phẩm chưa tồn tại, thêm sản phẩm mới
                            cmd = new SqlCommand("INSERT INTO ProductTbl(PrName,PrCat,PrQty,PrPrice) VALUES(@PN,@PC,@PQ,@PP)", Con);
                            cmd.Parameters.AddWithValue("@PN", PrNameTb.Text);
                            cmd.Parameters.AddWithValue("@PC", CatTb.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@PQ", QtyTb.Text);
                            cmd.Parameters.AddWithValue("@PP", PriceTb.Text);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Thêm sản phẩm mới thành công");
                        }

                        Con.Close();
                        ShowProducts();
                        Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }
        }

        int Key = 0;
        private void ProductsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // [0] ở đây được hiểu là hiện tại, chỉ lấy duy nhất 1 dòng 
            // cells = cột 
            PrNameTb.Text = ProductsDGV.SelectedRows[0].Cells[1].Value.ToString();
            CatTb.Text = ProductsDGV.SelectedRows[0].Cells[2].Value.ToString();
            QtyTb.Text = ProductsDGV.SelectedRows[0].Cells[3].Value.ToString();
            PriceTb.Text = ProductsDGV.SelectedRows[0].Cells[4].Value.ToString();


            if (PrNameTb.Text == "")
            {
                // lấy id đổ vào key để thực hiện sửa và xóa
                //này có nhiệm vụ lấy giá trị mã số nhân viên được chọn từ DataGridView và chuyển đổi thành một số nguyên.
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(ProductsDGV.SelectedRows[0].Cells[0].Value.ToString());

            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Vui lòng chọn dòng có dữ liệu để xóa!");
            }
            else
            {
                try
                {
                    Con.Open();
                    // thực thi truy vấn sql Sqlcommand
                    SqlCommand cmd = new SqlCommand("delete from ProductTbl where PrId = @Ckey", Con);
                    //truyền tham số  và texbox sau khi có data truyền ngược lên tham số values
                    cmd.Parameters.AddWithValue("@Ckey", Key);


                    // thuc thi query insert
                    //Hàm này không trả về bất kỳ dữ liệu nào và được sử dụng để thực hiện các thao tác không yêu cầu kết quả trả về từ CSDL.  
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa thành công");
                    Con.Close();
                    // reset lại danh sách
                    ShowProducts();
                    Clear();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (PrNameTb.Text == "" || PriceTb.Text == "" || QtyTb.Text == "" || CatTb.SelectedIndex == -1)
            {
                MessageBox.Show("Thông tin chưa đầy đủ");
            }
            //Trong đó, hàm decimal.TryParse được sử dụng để chuyển đổi giá trị của PriceTb.Text sang kiểu decimal,
            //nếu giá trị này không hợp lệ thì điều kiện price < 0 sẽ trả về true. Nếu giá trị nhập vào không hợp lệ thì sẽ hiển thị thông báo lỗi.
            //Nếu giá trị hợp lệ thì sẽ tiếp tục thực hiện truy vấn SQL như bình thường.
            else if (!decimal.TryParse(PriceTb.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Giá sản phẩm không hợp lệ");
            }
            else if (!decimal.TryParse(QtyTb.Text, out decimal Quantity) || Quantity < 0)
            {
                MessageBox.Show("Số lượng không hợp lệ");
            }
            else
            {
                try
                {
                    Con.Open();
                    // thực thi truy vấn sql Sqlcommand
                    SqlCommand cmd = new SqlCommand("Update ProductTbl set PrName = @PN,PrCat = @PC, PrQty = @PQ , PrPrice=@PP where PrId = @CKey ", Con);
                    //truyền tham số  và texbox sau khi có data truyền ngược lên tham số values
                    // tham số 1 : tham số cần lấy
                    // tham số 2 : tham số truyền dữ liệu
                    cmd.Parameters.AddWithValue("@PN", PrNameTb.Text);
                    cmd.Parameters.AddWithValue("@PC", CatTb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@PQ", QtyTb.Text);
                    cmd.Parameters.AddWithValue("@PP", PriceTb.Text);
                    cmd.Parameters.AddWithValue("@CKey", Key);



                    // thuc thi query insert
                    //Hàm này không trả về bất kỳ dữ liệu nào và được sử dụng để thực hiện các thao tác không yêu cầu kết quả trả về từ CSDL.  
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sửa thành công");
                    Con.Close();
                    // reset lại danh sách
                    ShowProducts();
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
            Homes Obj = new Homes();
            Obj.Show();
            // this ở đây chính là tham chiếu của thằng hiện tại... ẩn Homes
            this.Hide();
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
            // this ở đây chính là tham chiếu của thằng hiện tại... ẩn customers
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Billings Obj = new Billings();
            Obj.Show();
            // this ở đây chính là tham chiếu của thằng hiện tại... ẩn Blilings
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            // this ở đây chính là tham chiếu của thằng hiện tại... ẩn Login
            this.Hide();
        }

        private void DetailsBtn_Click(object sender, EventArgs e)
        {
            ProductDetails Obj = new ProductDetails();
            Obj.Show();
            // this ở đây chính là tham chiếu của thằng hiện tại... ẩn ProductDetails
            this.Hide();
        }

        private void TextFind_TextChanged(object sender, EventArgs e)
        {
            string value = TextFind.Text;
            if (!string.IsNullOrEmpty(value))
            {
                Con.Open();
                string Query = "SELECT * FROM ProductTbl WHERE PrName LIKE '%' + @ProductName + '%'";
                // truy vấn dữ liệu để đổ vào database vao dataset
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
                sda.SelectCommand.Parameters.AddWithValue("@ProductName", value);
                // tự động cập nhật dữ liệu, khi có dữ liệu mới đổ vào
                SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                // dổi dữ liệu được truy vấn sda vào bảng ds 
                sda.Fill(ds);
                ProductsDGV.DataSource = ds.Tables[0];
                Con.Close();
            }
            else
            {
                // nguoc lai neu khong tim xoa di thi se tro ve danh sach ban dau (default)
                ShowProducts();
            }
        }
    }
}
