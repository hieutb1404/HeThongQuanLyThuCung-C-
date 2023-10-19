using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyThuCung
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
            timer1.Start();
        }

        int StartP = 0;


        private void timer1_Tick(object sender, EventArgs e)
        {
            StartP += 1;
            Progress.Value = StartP;
            Percentaget.Text = StartP + "%";
            if(Progress.Value == 100)
            {
                Progress.Value = 0;
                timer1.Stop();
                Login Obj = new Login();
                Obj.Show();
                this.Hide();
            }
        }
    }
}
