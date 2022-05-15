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
    public partial class formPayment : Form {
        private double amount = 0;
        private double totalPrice = flightData.flightPrice + seatData.seatPrice;
        private string paymentMethod = "Pay by cash";

        public formPayment() {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e) {
            var formBookingDetail = new formBookingDetail();
            formBookingDetail.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e) {
            var formLogin = new formLogin();
            formLogin.Show();
            this.Hide();
        }

        private void payment_Load(object sender, EventArgs e) {
            init();

            // Timer Now
            timerTimeNow.Start();
            labelDateNow.Text = DateTime.Now.ToString("MM/dd/yyyy");
            labelTimeNow.Text = DateTime.Now.ToString("HH:mm:ss");

            // Show Booking Info
            lblFlightPriceBInfo.Text = flightData.flightPrice.ToString("#,#.00");
            lblSeatPriceBInfo.Text = seatData.seatPrice.ToString("#,#.00");
            lblTotal.Text = (flightData.flightPrice + seatData.seatPrice).ToString("#,#.00");
        }

        private void init() {
            // Show user active
            labelID.Text = emp.id;
            labelFname.Text = emp.fname;
            labelLname.Text = emp.lname;
        }

        private void timerTimeNow_Tick(object sender, EventArgs e) {
            labelTimeNow.Text = DateTime.Now.ToString("HH:mm:ss");
            timerTimeNow.Start();
        }

        private void btnCalc_Click(object sender, EventArgs e) {
            if (!validAmount()) return;

            double res = amount - totalPrice;
            lblChange.Text = res.ToString("#,#.00");
        }

        private bool validAmount() {
            // Check empty fill
            if (textAmount.Text == "") {
                MessageBox.Show
                (
                    "Please fill amount",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }

            // Check number type
            if (!double.TryParse(textAmount.Text, out amount)) {
                MessageBox.Show
                (
                    "Please enter number!",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }

            // Check amount enough for payment
            if (amount < totalPrice) {
                MessageBox.Show
                (
                    "Amount is not enough!",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
            return true;
        }

        private void btnSuccess_Click(object sender, EventArgs e) {
            putBookingDB();
            putPassengerDB();
            putPaymentDB();
            putSeatDB();

            // Interaction to emp: say "SUCCESS!"
        }

        private void putPassengerDB() {
            dbConfig.connection.Open();



            dbConfig.connection.Close();
        }

        private void putBookingDB() {
            dbConfig.connection.Open();

  

            dbConfig.connection.Close();
        }

        private void putPaymentDB() {
            dbConfig.connection.Open();



            dbConfig.connection.Close();
        }

        private void putSeatDB() {
            dbConfig.connection.Open();



            dbConfig.connection.Close();
        }
    }
}