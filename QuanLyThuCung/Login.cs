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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

        }

        public static string LoggedInUser;
        public static string Rols;





        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {

            SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\trung hieu\Documents\DBshopthucung.mdf;Integrated Security=True;Connect Timeout=30");
            string Query = "SELECT COUNT(*) FROM EmployeeTbl WHERE EmpName='" + UserNameTb.Text + "' AND EmpPass='" + PasswordTb.Text + "'";
            SqlDataAdapter sda = new SqlDataAdapter(Query,Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            // nếu có tải khoản thì vào trang
            if(dt.Rows[0][0].ToString() == "1")
            {
            this.Hide();
             // gán giá trị tên vào form trước khi mở form
             // nếu gán giá trị sau khi đóng form thì sẽ ko nhận đc tên user để gán
             // mà phải lấy tên user gán vào trước thì mới ok
            LoggedInUser = UserNameTb.Text;
            SqlCommand cmd = new SqlCommand("SELECT EmpRols FROM EmployeeTbl WHERE EmpName=@EmpName", Con);
            cmd.Parameters.AddWithValue("@EmpName", UserNameTb.Text);
                Con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Gán giá trị EmpRols vào biến public static Rols
                    Rols = reader["EmpRols"].ToString();
                }
                Con.Close();
                Homes emp = new Homes();
            emp.Show();

            }
            else
            {
                MessageBox.Show("Vui lòng kiểm tra lại tải khoản hoặc mật khẩu !");
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            UserNameTb.Text = "";
            PasswordTb.Text = "";
            UserNameTb.Focus();
        }

        private void PasswordTb_TextChanged(object sender, EventArgs e)
        {
            PasswordTb.PasswordChar = '\u25CF';


        }
    }
}
