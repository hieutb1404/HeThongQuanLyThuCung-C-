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
    public partial class customers : Form
    {
        public customers()
        {
            InitializeComponent();
            ShowCustomer();
            EmpNameLbl.Text = Login.LoggedInUser;
            RolsLB.Text = Login.Rols;


        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\trung hieu\Documents\DBshopthucung.mdf;Integrated Security=True;Connect Timeout=30");
        private void ShowCustomer()
        {
            Con.Open();
            string Query = "Select * from CustomerTbl";
            // truy vấn dữ liệu để đổ vào database vao` dataset
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            // tự động cập nhật dữ liệu, khi có dữ liệu mới đổ vào
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            // dổi dữ liệu được truy vấn sda vào bảng ds 
            sda.Fill(ds);
            CustomerDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void Clear()
        {
            CustNameTb.Text = "";
            CustAddTb.Text = "";
            CustPhoneTb.Text = "";
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if(CustNameTb.Text == "" || CustAddTb.Text == "" || CustPhoneTb.Text == "")
            {
                MessageBox.Show("Thông tin chưa đầy đủ");
            } else
            {
                try
                {
                    Con.Open();
                    // thực thi truy vấn sql Sqlcommand
                    SqlCommand cmd = new SqlCommand("insert into CustomerTbl(CustName,CustAdd,CustPhone) Values(@CN,@CA,@CP)", Con);
                    //truyền tham số  và texbox sau khi có data truyền ngược lên tham số values
                    // tham số 1 : tham số cần lấy
                    // tham số 2 : tham số truyền dữ liệu
                    cmd.Parameters.AddWithValue("@CN", CustNameTb.Text);
                    cmd.Parameters.AddWithValue("@CA", CustAddTb.Text);
                    cmd.Parameters.AddWithValue("@CP", CustPhoneTb.Text);
                

                    // thuc thi query insert
                    //Hàm này không trả về bất kỳ dữ liệu nào và được sử dụng để thực hiện các thao tác không yêu cầu kết quả trả về từ CSDL.  
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm thành công");
                    Con.Close();
                    // reset lại danh sách
                    ShowCustomer();
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
                    SqlCommand cmd = new SqlCommand("delete from CustomerTbl where CustId = @Ckey", Con);
                    //truyền tham số  và texbox sau khi có data truyền ngược lên tham số values
                    cmd.Parameters.AddWithValue("@Ckey", Key);


                    // thuc thi query insert
                    //Hàm này không trả về bất kỳ dữ liệu nào và được sử dụng để thực hiện các thao tác không yêu cầu kết quả trả về từ CSDL.  
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa thành công");
                    Con.Close();
                    // reset lại danh sách
                    ShowCustomer();
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
            if (CustNameTb.Text == "" || CustAddTb.Text == "" || CustPhoneTb.Text == "" )
            {
                MessageBox.Show("Thông tin chưa đầy đủ");
            }
            else
            {
                try
                {
                    Con.Open();
                    // thực thi truy vấn sql Sqlcommand
                    SqlCommand cmd = new SqlCommand("Update CustomerTbl set CustName=@CN,CustAdd=@CA,CustPhone=@CP Where CustId = @Ckey", Con);
                    //truyền tham số  và texbox sau khi có data truyền ngược lên tham số values
                    cmd.Parameters.AddWithValue("@CN", CustNameTb.Text);
                    cmd.Parameters.AddWithValue("@CA", CustAddTb.Text);
                    cmd.Parameters.AddWithValue("@CP", CustPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@CKey", Key);


                    // thuc thi query insert
                    //Hàm này không trả về bất kỳ dữ liệu nào và được sử dụng để thực hiện các thao tác không yêu cầu kết quả trả về từ CSDL.  
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sửa thành công");
                    Con.Close();
                    // reset lại danh sách
                    ShowCustomer();
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
            Products Obj = new Products();
            Obj.Show();
            // this ở đây chính là tham chiếu của thằng hiện tại... ẩn Products
            this.Hide();
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

        private void label6_Click(object sender, EventArgs e)
        {
            Billings Obj = new Billings();
            Obj.Show();
            // this ở đây chính là tham chiếu của thằng hiện tại... ẩn Billings
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            // this ở đây chính là tham chiếu của thằng hiện tại... ẩn Login
            this.Hide();
        }

        int Key = 0;
        private void CustomerDGV_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            // [0] ở đây được hiểu là hiện tại, chỉ lấy duy nhất 1 dòng 
            CustNameTb.Text = CustomerDGV.SelectedRows[0].Cells[1].Value.ToString();
            CustAddTb.Text = CustomerDGV.SelectedRows[0].Cells[2].Value.ToString();
            CustPhoneTb.Text = CustomerDGV.SelectedRows[0].Cells[3].Value.ToString();

            if (CustNameTb.Text == "")
            {
                // lấy id đổ vào key để thực hiện sửa và xóa
                //này có nhiệm vụ lấy giá trị mã số nhân viên được chọn từ DataGridView và chuyển đổi thành một số nguyên.
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(CustomerDGV.SelectedRows[0].Cells[0].Value.ToString());

            }
        }

        private void TextFind_TextChanged(object sender, EventArgs e)
        {
            string value = TextFind.Text;
            if (!string.IsNullOrEmpty(value))
            {
                Con.Open();
                string Query = "SELECT * FROM CustomerTbl WHERE CustName LIKE '%' + @CustName + '%'";
                // truy vấn dữ liệu để đổ vào database vao dataset
                SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
                sda.SelectCommand.Parameters.AddWithValue("@CustName", value);
                // tự động cập nhật dữ liệu, khi có dữ liệu mới đổ vào
                SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                // dổi dữ liệu được truy vấn sda vào bảng ds 
                sda.Fill(ds);
                CustomerDGV.DataSource = ds.Tables[0];
                Con.Close();
            }
            else
            {
                // nguoc lai neu khong tim xoa di thi se tro ve danh sach ban dau (default)
                ShowCustomer();
            }
        }
    }
}
