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
    public partial class ProductDetails : Form
    {
        public ProductDetails()
        {
            InitializeComponent();
            GetIdProduct();
            ShowDetails();
        }
    SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\trung hieu\Documents\DBshopthucung.mdf;Integrated Security=True;Connect Timeout=30");

        // lấy ra tên thú cưng
    private void GetnameProduct()
    {
        Con.Open();
        string Query = "Select * from ProductTbl where PrId ='" + PrIdCb.SelectedValue.ToString() + "'";
        SqlCommand cmd = new SqlCommand(Query, Con);
        DataTable dt = new DataTable();
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        sda.Fill(dt);
        foreach (DataRow dr in dt.Rows)
        {
            PrNameTb.Text = dr["PrName"].ToString();
        }
        Con.Close();
    }

        // lấy ra id product
    private void GetIdProduct()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("select PrId from ProductTbl", Con);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("PrId", typeof(int));
            dt.Load(Rdr);
            PrIdCb.ValueMember = "PrId";
            PrIdCb.DataSource = dt;
            Con.Close();
        }

    private void ShowDetails()
        {
            Con.Open();
            string Query = "Select * from ProductDetails";
            // truy vấn dữ liệu để đổ vào database hoặc dataset
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            // tự động cập nhật dữ liệu, khi có dữ liệu mới đổ vào
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            // dổi dữ liệu được truy vấn sda vào bảng ds 
            sda.Fill(ds);
            DetailsDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void PrIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetnameProduct();
        }
        private void Clear()
        {
            PrNameTb.Text = "";
            DtHealthTb.Text = "";
            DtColorTb.Text = "";
            dtAgeTb.Text = "";
            DtImportTb.Text = "";
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (PrNameTb.Text == "" || DtHealthTb.Text == "" || DtColorTb.Text == "" || dtAgeTb.Text == ""|| DtImportTb.Text == "")
            {
                MessageBox.Show("Thông tin chưa đầy đủ");
            }
            else
            {
                try
                {
                    Con.Open();
                    // thực thi truy vấn sql Sqlcommand
                    SqlCommand cmd = new SqlCommand("insert into ProductDetails(PrName,DtHealth,DtColor,DtAge,DtImport) Values(@PN,@DH,@DC,@DA,@DI)", Con);
                    //truyền tham số  và texbox sau khi có data truyền ngược lên tham số values
                    // tham số 1 : tham số cần lấy
                    // tham số 2 : tham số truyền dữ liệu
                    cmd.Parameters.AddWithValue("@PN", PrNameTb.Text);
                    cmd.Parameters.AddWithValue("@DH", DtHealthTb.Text);
                    cmd.Parameters.AddWithValue("@DC", DtColorTb.Text);
                    cmd.Parameters.AddWithValue("@DA", dtAgeTb.Text);
                    cmd.Parameters.AddWithValue("@DI", DtImportTb.Text);

                    // thuc thi query insert
                    //Hàm này không trả về bất kỳ dữ liệu nào và được sử dụng để thực hiện các thao tác không yêu cầu kết quả trả về từ CSDL.  
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm thành công");
                    Con.Close();
                    // reset lại danh sách
                    ShowDetails();
                    Clear();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Products Obj = new Products();
            Obj.Show();
            // this ở đây chính là tham chiếu của thằng hiện tại... ẩn Employees
            this.Hide();
        }
    }
}
