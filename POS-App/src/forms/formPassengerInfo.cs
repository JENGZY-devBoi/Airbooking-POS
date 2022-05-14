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
    public partial class formPassengerInfo : Form {
        public formPassengerInfo() {
            InitializeComponent();
        }

        private void formPassengerInfo_Load(object sender, EventArgs e) {
            init();

            // Timer Now
            timerTimeNow.Start();
            labelDateNow.Text = DateTime.Now.ToString("MM/dd/yyyy");
            labelTimeNow.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void timerTimeNow_Tick(object sender, EventArgs e) {
            labelTimeNow.Text = DateTime.Now.ToString("HH:mm:ss");
            timerTimeNow.Start();
        }

        private void btnLogout_Click(object sender, EventArgs e) {
            var formLogin = new formLogin();
            formLogin.Show();
            this.Hide();
        }

        private void btnSubmit_Click(object sender, EventArgs e) {
            if (validFill()) submit();
        }

        private void btnBack_Click(object sender, EventArgs e) {
            var formSearch = new formSearch();
            formSearch.Show();
            this.Hide();
        }

        private void submit() {
            //Console.WriteLine(dateTimeDOB.Value.ToString().Split(' ')[0]);

            passengerData.passengerTitle = comboTitle.SelectedItem.ToString();
            passengerData.passengerFname = textFname.Text;
            passengerData.passengerLname = textLname.Text;
            passengerData.passengerDOB = dateTimeDOB.Value.ToString().Split(' ')[0];
            passengerData.passengerEmail = textEmail.Text + "@" + comboEmail.SelectedItem.ToString();

            // GO TO formSelectSeat
            var formSelectSeat = new formSelectSeat();
            formSelectSeat.Show();
            this.Hide();
        }

        private bool validFill() {
            // Valid fill complete info
            if (comboTitle.SelectedItem == null ||
                textFname.Text == "" ||
                textLname.Text == "" ||
                textEmail.Text == "" ||
                comboEmail.Text == "") {
                // Check
                // Console.WriteLine("Please fill complete information.");
                MessageBox.Show
                    (
                        "Please fill complete information.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                return false;
            }
            return true;
        }

        private void init() {
            // Set the Format type and the CustomFormat string.
            dateTimeDOB.Format = DateTimePickerFormat.Custom;
            dateTimeDOB.CustomFormat = "MM - dd - yyyy";

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
            comboTitle.SelectedIndex = 0;
            comboEmail.SelectedIndex = 0;

            // Set passenger data (when back)
            textFname.Text = passengerData.passengerFname;
            textLname.Text = passengerData.passengerLname;
            if (passengerData.passengerEmail != null) {
                comboTitle.Text = passengerData.passengerTitle;
                textEmail.Text = passengerData.passengerEmail.Split('@')[0];
                comboEmail.Text = passengerData.passengerEmail.Split('@')[1];
                dateTimeDOB.Value = new DateTime
                    (
                        Convert.ToInt32(passengerData.passengerDOB.Split('/')[2]), // Year
                        Convert.ToInt32(passengerData.passengerDOB.Split('/')[0]), // Month
                        Convert.ToInt32(passengerData.passengerDOB.Split('/')[1]) // Day
                    );
            }
        }

    }
}
