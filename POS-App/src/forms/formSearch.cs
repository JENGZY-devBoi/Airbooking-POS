using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_App {
    public partial class formSearch : Form {
        public formSearch() {
            InitializeComponent();
        }

        private void formSearch_Load(object sender, EventArgs e) {
            labelID.Text = emp.id;
            labelFname.Text = emp.fname;
            labelLname.Text = emp.lname;
        }

        private void buttonSearch_Click(object sender, EventArgs e) {

        }

        private void btnLogout_Click(object sender, EventArgs e) {
            var formLogin = new formLogin();
            formLogin.Show();
            this.Hide();
        }
    }
}