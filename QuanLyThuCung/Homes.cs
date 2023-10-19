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
    public partial class Homes : Form
    {
        public Homes()
        {
            InitializeComponent();
            EmpNameLbl.Text = Login.LoggedInUser;
            RolsLB.Text = Login.Rols;
            CountDogs();
            CountCats();
            TotalWeek();
            TotalMonth();
            TotalYear();
            TotalDogandCats();

        
        }
        // kết nối cơ sở dữ liệu
        SqlConnection Con = Connection.GetConnection();
        private void CountDogs()
        {
            

            // tạo ra 1 biến string cat ="chó"
            string Cat = "Chó";
            Con.Open();
            // sqlDataAdapter truy vấn dữ liệu từ data Connection con , và đổ vào dataTable
            // truy vấn câu lệnh này để đếm số lượng chó
            SqlDataAdapter sda = new SqlDataAdapter("Select Count(*) from ProductTbl where PrCat = N'" + Cat + "'", Con);
            DataTable dt = new DataTable();
            // sau khi truy vấn dữ liệu xong ta đổ dữ liệu vào dataTable
            sda.Fill(dt);
            // đổ dữ liệu vào label(nhãn) để có được số lượng 
            DogLbl.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }

        // dem so luong meo
        private void CountCats()
        {

            string Cat = "Mèo";
            Con.Open();
            // sqlDataAdapter truy vấn dữ liệu từ data Connection con , và đổ vào dataTable
            // truy vấn câu lệnh này để đếm số lượng chó
            SqlDataAdapter sda = new SqlDataAdapter("Select Count(*) from ProductTbl where PrCat = N'" + Cat + "'", Con);
            DataTable dt = new DataTable();
            // sau khi truy vấn dữ liệu xong ta đổ dữ liệu vào dataTable
            sda.Fill(dt);
            // đổ dữ liệu vào label(nhãn) để có được số lượng 
            CatLbl.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }
        // tinh tong tien ban duoc theo ngay

        private void TotalDogandCats()
        {

            Con.Open();
            // sqlDataAdapter truy vấn dữ liệu từ data Connection con , và đổ vào dataTable
            // truy vấn câu lệnh này để tính tổng tiền hóa đơn đã bán từ bảng BillTbl
            SqlDataAdapter sda = new SqlDataAdapter("SELECT SUM(Amt) FROM BillTbl WHERE BDate = CONVERT(DATE, GETDATE())", Con);
            DataTable dt = new DataTable();
            // sau khi truy vấn dữ liệu xong ta đổ dữ liệu vào dataTable
            sda.Fill(dt);
            // đổ dữ liệu vào label(nhãn) để có được số lượng 
            totalDC.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }
        // tinh tong tien ban duoc theo tuan
        private void TotalWeek()
        {

            Con.Open();
            // sqlDataAdapter truy vấn dữ liệu từ data Connection con , và đổ vào dataTable
            // truy vấn câu lệnh này để tính tổng tiền hóa đơn đã bán từ bảng BillTbl
            SqlDataAdapter sda = new SqlDataAdapter("SELECT SUM(Amt) FROM BillTbl WHERE BDate >= DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0) AND BDate < DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()) + 1, 0)", Con);
            DataTable dt = new DataTable();
            // sau khi truy vấn dữ liệu xong ta đổ dữ liệu vào dataTable
            sda.Fill(dt);
            // đổ dữ liệu vào label(nhãn) để có được số lượng 
            totalweek.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }
        // tinh tong tien theo thang
        private void TotalMonth()
        {
            Con.Open();
            // sqlDataAdapter truy vấn dữ liệu từ data Connection con , và đổ vào dataTable
            // truy vấn câu lệnh này để tính tổng tiền hóa đơn đã bán từ bảng BillTbl
            SqlDataAdapter sda = new SqlDataAdapter("SELECT SUM(Amt) FROM BillTbl WHERE MONTH(BDate) = MONTH(GETDATE()) AND YEAR(BDate) = YEAR(GETDATE())", Con);
            DataTable dt = new DataTable();
            // sau khi truy vấn dữ liệu xong ta đổ dữ liệu vào dataTable
            sda.Fill(dt);
            // đổ dữ liệu vào label(nhãn) để có được số lượng 
            totalmth.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }

        // tinh tong tien theo nam
        private void TotalYear()
        {
            Con.Open();
            // sqlDataAdapter truy vấn dữ liệu từ data Connection con , và đổ vào dataTable
            // truy vấn câu lệnh này để tính tổng tiền hóa đơn đã bán từ bảng BillTbl
            SqlDataAdapter sda = new SqlDataAdapter("SELECT SUM(Amt) FROM BillTbl WHERE YEAR(BDate) = YEAR(GETDATE())", Con);
            DataTable dt = new DataTable();
            // sau khi truy vấn dữ liệu xong ta đổ dữ liệu vào dataTable
            sda.Fill(dt);
            // đổ dữ liệu vào label(nhãn) để có được số lượng 
            totalyrr.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }
        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            Products Obj = new Products();
            Obj.Show();
            // this ở đây chính là tham chiếu của thằng hiện tại... ẩn Employees
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Employees Obj = new Employees();
            if(RolsLB.Text == "admin")
            {
            Obj.Show();
            this.Hide();

            } else
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

        private void label6_Click(object sender, EventArgs e)
        {
            Billings Obj = new Billings();
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

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void DogLbl_Click(object sender, EventArgs e)
        {

        }
    }
}
