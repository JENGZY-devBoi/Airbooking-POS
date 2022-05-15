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
    public partial class formBookingDetail : Form {
        public formBookingDetail() {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e) {
            var formSelectSeat = new formSelectSeat();
            formSelectSeat.Show();
            this.Hide();
        }

        private void formBookingDetail_Load(object sender, EventArgs e) {
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

            // Show Flight Info
            // Depart
            lblDepartStation.Text = flightData.flightFrom;
            lblDepartDate.Text = flightData.flightDateTime;
            lblDepartTime.Text = flightData.flightDepart;
            // Arrival
            lblArrivalStation.Text = flightData.flightTo;
            lblArrivalDate.Text = flightData.flightDateTime;
            lblArrivalTime.Text = flightData.flightArrival;
            // Flight
            lblFlightID.Text = flightData.flightID;
            lblAirline.Text = flightData.flightAirline;
            lblFlightDur.Text = flightData.flightInfo;
            lblFlightPrice.Text = flightData.flightPrice.ToString("#,#.00");

            // Show Seat Info
            lblSeatID.Text = seatData.seatID;
            lblSeatAmount.Text = "1"; // improve in the future
            lblSeatPrice.Text = seatData.seatPrice.ToString("#,#.00");

            // Show Passenger Info
            lblTitle.Text = passengerData.passengerTitle;
            lblFname.Text = passengerData.passengerFname;
            lblLname.Text = passengerData.passengerLname;
            lblDOB.Text = passengerData.passengerDOB;
            lblEmail.Text = passengerData.passengerEmail;

            // Show Booking Info
            lblFlightPriceBInfo.Text = flightData.flightPrice.ToString("#,#.00");
            lblSeatPriceBInfo.Text = seatData.seatPrice.ToString("#,#.00");
            lblTotal.Text = (flightData.flightPrice + seatData.seatPrice).ToString("#,#.00");
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

        private void btnPlayment_Click(object sender, EventArgs e) {
            // GO TO payment forom
            var formPayment = new formPayment();
            formPayment.Show();
            this.Hide();
        }
    }
}
