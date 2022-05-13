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
    public partial class formSelectSeat : Form {
        public formSelectSeat() {
            InitializeComponent();
        }

        private void formSelectSeat_Load(object sender, EventArgs e) {
            init();

            // Timer Now
            timerTimeNow.Start();
            labelDateNow.Text = DateTime.Now.ToString("MM/dd/yyyy");
            labelTimeNow.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void init() {
            // Show user active
            labelID.Text = emp.id;
            labelFname.Text = emp.fname;
            labelLname.Text = emp.lname;       

            // Set flight data
            comboFrom.Items.Add(flightData.flightFrom);
            comboTo.Items.Add(flightData.flightTo);
            comboDepart.Items.Add(flightData.flightDateTime);

            comboFrom.SelectedIndex = 0;
            comboTo.SelectedIndex = 0;
            comboDepart.SelectedIndex = 0;

            // Set array to btnCliked
            btnClicked = new bool[40];
            for (int i = 0; i < 40; i++) btnClicked[i] = false;
        }

        private void btnBack_Click(object sender, EventArgs e) {
            var formPassengerInfo = new formPassengerInfo();
            formPassengerInfo.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e) {
            var formLogin = new formLogin();
            formLogin.Show();
            this.Hide();
        }

        private void btnConfirm_Click(object sender, EventArgs e) {
            //
        }

        private void timerTimeNow_Tick_1(object sender, EventArgs e) {
            labelTimeNow.Text = DateTime.Now.ToString("HH:mm:ss");
            timerTimeNow.Start();
        }

        //---------------------------------------------------------------
        //---------------------------------------------------------------
        #region Fields ButtonWasClicked
        bool[] btnClicked;
        #endregion

        private void seatA1_Click(object sender, EventArgs e) {
            int idx = convertIdxSeat(seatA1.Tag.ToString());

            btnClicked[idx] = checkClicked(btnClicked[idx]);
            selectedClick(seatA1, btnClicked[idx]);
        }

        private void seatA2_Click(object sender, EventArgs e) {
            int idx = convertIdxSeat(seatA2.Tag.ToString());

            btnClicked[idx] = checkClicked(btnClicked[idx]);
            selectedClick(seatA2, btnClicked[idx]);
        }

        private void selectedClick(Label lbl, bool click) {
            if (click) {
                selectedColor(lbl);
            } else {
                unselectedColor(lbl);
            }
        }

        private bool checkClicked(bool click) {
            if (click) return false;
            return true;
        }

        private int convertIdxSeat(string str) {
            int idx = Convert.ToInt32(str.Split(' ')[1]) - 1;
            return idx;
        }

        private void selectedColor(Label lbl) {
            lbl.BackColor = Color.LightGreen;
        }

        private void unselectedColor(Label lbl) {
            lbl.BackColor = Color.FromArgb(192, 192, 255);
        }

    }
}
