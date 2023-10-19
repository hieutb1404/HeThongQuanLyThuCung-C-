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

//manh

namespace QuanLyThuCung
{
    public partial class Employees : Form
    {
        public Employees()
        {
            InitializeComponent();
            ShowEmployees();
            EmpNameLbl.Text = Login.LoggedInUser;
            RolsLB.Text = Login.Rols;

        }




        string conString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\trung hieu\Documents\DBshopthucung.mdf;Integrated Security=True;Connect Timeout=30";
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\trung hieu\Documents\DBshopthucung.mdf;Integrated Security=True;Connect Timeout=30");
        private void ShowEmployees()
        {
            Con.Open();
            string Query = "Select * from EmployeeTbl";
            // truy vấn dữ liệu để đổ vào database hoặc dataset
            SqlDataAdapter sda = new SqlDataAdapter(Query,Con);
            // tự động cập nhật dữ liệu, khi có dữ liệu mới đổ vào
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            // dổi dữ liệu được truy vấn sda vào bảng ds 
            sda.Fill(ds);
            EmployeeDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

       

        private void Clear()
        {
            EmpNameTb.Text = "";
            EmpAddTb.Text = "";
            EmpPhoneTb.Text = "";
            PasswordTb.Text = "";
            EmpDOB.Text = string.Empty;
            RolsTb.Text = "";
        }


        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (EmpNameTb.Text == "" || EmpAddTb.Text == "" || EmpPhoneTb.Text == "" || PasswordTb.Text == "" || RolsTb.Text == "")
            {
                MessageBox.Show("Thông tin chưa đầy đủ");
            } else
            {

                    try
                    {
                    //using, kết nối sẽ tự động được giải phóng sau khi sử dụng và không cần phải gọi Con.Dispose() và Con.Close().
                    using (SqlConnection Con = new SqlConnection(conString))
                        {
                        if (Con.State == ConnectionState.Closed)
                        {
                            Con.Open();
                        }
                        SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM EmployeeTbl WHERE EmpName=@EN", Con);
                        cmd.Parameters.AddWithValue("@EN", EmpNameTb.Text);
                        int count = (int)cmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Tên nhân viên đã tồn tại.");
                            return;
                        }

                        // thực thi truy vấn sql Sqlcommand
                        cmd = new SqlCommand("insert into EmployeeTbl(EmpName,EmpAdd,EmpDOB,EmpPhone,EmpPass,EmpRols) Values(@EN,@EA,@ED,@EP,@EPa,@ER)", Con);
                        //truyền tham số  và texbox sau khi có data truyền ngược lên tham số values
                        // tham số 1 : tham số cần lấy
                        // tham số 2 : tham số truyền dữ liệu
                        cmd.Parameters.AddWithValue("@EN", EmpNameTb.Text);
                        cmd.Parameters.AddWithValue("@EA", EmpAddTb.Text);
                        cmd.Parameters.AddWithValue("@ED", EmpDOB.Value.Date);
                        cmd.Parameters.AddWithValue("@EP", EmpPhoneTb.Text);
                        cmd.Parameters.AddWithValue("@EPa", PasswordTb.Text);
                        cmd.Parameters.AddWithValue("@ER", RolsTb.Text);


                        // thuc thi query insert
                        //Hàm này không trả về bất kỳ dữ liệu nào và được sử dụng để thực hiện các thao tác không yêu cầu kết quả trả về từ CSDL.  
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Thêm thành công");
                        Con.Close();
                        // reset lại danh sách
                        ShowEmployees();
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
        private void EmployeeDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        private bool CheckTenNhanVienTonTai(SqlConnection Con, string TenNV)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM EmployeeTbl WHERE EmpName=@TN", Con);
            cmd.Parameters.AddWithValue("@TN", TenNV);
            int count = (int)cmd.ExecuteScalar();
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (EmpNameTb.Text == "" || EmpAddTb.Text == "" || EmpPhoneTb.Text == "" || PasswordTb.Text == "" || RolsTb.Text == "")
            {
                MessageBox.Show("Thông tin chưa đầy đủ");
            }
            else
            {
                try
                {
                    //using, kết nối sẽ tự động được giải phóng sau khi sử dụng và không cần phải gọi Con.Dispose() và Con.Close().
                    using (SqlConnection Con = new SqlConnection(conString))
                    {
                        if (Con.State == ConnectionState.Closed)
                        {
                            Con.Open();
                        }

                        // Lấy tên nhân viên hiện tại từ database
                        SqlCommand cmd = new SqlCommand("SELECT EmpName FROM EmployeeTbl WHERE EmpNum=@EKey", Con);
                        cmd.Parameters.AddWithValue("@EKey", Key);
                        string tenNhanVienHienTai = (string)cmd.ExecuteScalar();

                        if (EmpNameTb.Text != tenNhanVienHienTai) // Kiểm tra nếu tên nhân viên đã thay đổi
                        {
                            // truyền đối số cho hàm checkTenNhanVienTonTai xử lý khi gặp tên đã trùng và không cho sửa
                            if (CheckTenNhanVienTonTai(Con, EmpNameTb.Text))
                            {
                                MessageBox.Show("Tên nhân viên đã tồn tại.");
                                return;
                            }
                        }
                        // thực thi truy vấn sql Sqlcommand
                        cmd = new SqlCommand("Update EmployeeTbl set EmpName=@EN,EmpAdd=@EA,EmpDOB=@ED,EmpPhone=@EP,EmpPass=@EPa,EmpRols=@ER where EmpNum=@EKey", Con);
                        //truyền tham số  và texbox sau khi có data truyền ngược lên tham số values
                        cmd.Parameters.AddWithValue("@EN", EmpNameTb.Text);
                        cmd.Parameters.AddWithValue("@EA", EmpAddTb.Text);
                        cmd.Parameters.AddWithValue("@ED", EmpDOB.Value.Date);
                        cmd.Parameters.AddWithValue("@EP", EmpPhoneTb.Text);
                        cmd.Parameters.AddWithValue("@EPa", PasswordTb.Text);
                        cmd.Parameters.AddWithValue("@ER", RolsTb.Text);

                        cmd.Parameters.AddWithValue("@EKey", Key);


                        // thuc thi query insert
                        //Hàm này không trả về bất kỳ dữ liệu nào và được sử dụng để thực hiện các thao tác không yêu cầu kết quả trả về từ CSDL.  
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Sửa thành công");
                        Con.Close();
                        // reset lại danh sách
                        ShowEmployees();
                        Clear();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
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
                    SqlCommand cmd = new SqlCommand("delete from EmployeeTbl where EmpNum = @EmpKey", Con);
                    //truyền tham số  và texbox sau khi có data truyền ngược lên tham số values
                    cmd.Parameters.AddWithValue("@EmpKey", Key);


                    // thực thi query insert
                    //Hàm này không trả về bất kỳ dữ liệu nào và được sử dụng để thực hiện các thao tác không yêu cầu kết quả trả về từ CSDL.  
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa thành công");
                    Con.Close();
                    // reset lại danh sách
                    ShowEmployees();
                    Clear();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            customers Obj = new customers();
            Obj.Show();
            // this ở đây chính là tham chiếu của thằng hiện tại... ẩn Employees
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

            Products Obj = new Products();
            Obj.Show();
            // this ở đây chính là tham chiếu của thằng hiện tại... ẩn Products
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Billings Obj = new Billings();
            Obj.Show();
            // this ở đây chính là tham chiếu của thằng hiện tại... ẩn Billings
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Homes Obj = new Homes();
            Obj.Show();
            // this ở đây chính là tham chiếu của thằng hiện tại... ẩn Homes
            this.Hide();
        }

        private void label7_Click_1(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            // this ở đây chính là tham chiếu của thằng hiện tại... ẩn Login
            this.Hide();
        }

        private void EmployeeDGV_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            // [0] ở đây được hiểu là hiện tại, chỉ lấy duy nhất 1 dòng 
            // cells tương ứng với cột
            EmpNameTb.Text = EmployeeDGV.SelectedRows[0].Cells[1].Value.ToString();
            EmpAddTb.Text = EmployeeDGV.SelectedRows[0].Cells[2].Value.ToString();
            EmpDOB.Text = EmployeeDGV.SelectedRows[0].Cells[3].Value.ToString();
            EmpPhoneTb.Text = EmployeeDGV.SelectedRows[0].Cells[4].Value.ToString();
            PasswordTb.Text = EmployeeDGV.SelectedRows[0].Cells[5].Value.ToString();
            RolsTb.Text = EmployeeDGV.SelectedRows[0].Cells[6].Value.ToString();


            if (EmpNameTb.Text == "")
            {
                // lấy id đổ vào key để thực hiện sửa và xóa
                Key = 0;
            }
            else
            {
                //này có nhiệm vụ lấy giá trị mã số nhân viên được chọn từ DataGridView và chuyển đổi thành một số nguyên.
                Key = Convert.ToInt32(EmployeeDGV.SelectedRows[0].Cells[0].Value.ToString());

            }
        }

        private void TextFind_TextChanged(object sender, EventArgs e)
        {
            string value = TextFind.Text;
            if (!string.IsNullOrEmpty(value))
            {
                Con.Open();
                string Query = "SELECT * FROM EmployeeTbl WHERE EmpName LIKE '%' + @EmpName + '%'";
                // truy vấn dữ liệu để đổ vào database vao dataset
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
                sda.SelectCommand.Parameters.AddWithValue("@EmpName", value);
                // tự động cập nhật dữ liệu, khi có dữ liệu mới đổ vào
                SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                // dổi dữ liệu được truy vấn sda vào bảng ds 
                sda.Fill(ds);
                EmployeeDGV.DataSource = ds.Tables[0];
                Con.Close();
            }
            else
            {
                // nguoc lai neu khong tim xoa di thi se tro ve danh sach ban dau (default)
                ShowEmployees();
            }
        }
    }
}
