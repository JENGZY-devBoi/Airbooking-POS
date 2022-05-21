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

namespace POS_App {
    public partial class formPayment : Form {
        private double amount = 0;
        private double totalPrice = flightData.flightPrice + seatData.seatPrice;
        private string paymentMethod = "Pay by cash";
        private string paymentStatus = "YES";
        private string passenID;
        private string bookingID;
        private string dateNow;
        private string timeNow;

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
            dateNow = labelDateNow.Text;
            timeNow = labelTimeNow.Text;

            postPassengerDB();
            postBookingDB();
            postPaymentDB();
            putSeatDB();

            // Interaction to emp: say "SUCCESS!"
            MessageBox.Show(
                "Booking success!",
                "Notification"
            );

            // GO TO SEARCH FORM
            var formSearch = new formSearch();
            formSearch.Show();
            this.Hide();
        }

        private void postPassengerDB() {
            try {
                dbConfig.connection.Open();
                var adapter = new SqlDataAdapter();
                string sql = 
                    $"INSERT INTO passengers " +
                    $"(passengersTitle,passengersFname,passengersLname," +
                    $"passengersDOB,passengersEmail) " +
                    $"VALUES(" +
                    $"'{passengerData.passengerTitle}'," +
                    $"'{passengerData.passengerFname}'," +
                    $"'{passengerData.passengerLname}'," +
                    $"'{passengerData.passengerDOB}'," +
                    $"'{passengerData.passengerEmail}');";
                adapter.InsertCommand = dbConfig.connection.CreateCommand();
                adapter.InsertCommand.CommandText = sql;
                adapter.InsertCommand.ExecuteNonQuery();
            } catch(Exception ex) {
                MessageBox.Show
                (
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            dbConfig.connection.Close();
        }

        private void postBookingDB() {
            try {
                dbConfig.connection.Open();
                string sql;
                // SELECT PASSENGER ID
                var adapterPass = new SqlDataAdapter();
                var passTB = new DataTable();
                sql = "SELECT * FROM passengers";
                adapterPass.SelectCommand = new SqlCommand(sql, dbConfig.connection);
                adapterPass.Fill(passTB);
                sql =
                    $"passengersFname='{passengerData.passengerFname}' AND " +
                    $"passengersLname='{passengerData.passengerLname}' AND " +
                    $"passengersEmail='{passengerData.passengerEmail}'";                        
                DataRow[] dr = passTB.Select(sql);
                passenID = dr[0]["passengerID"].ToString();

                // INSERT BOOKING
                var adapterBook = new SqlDataAdapter();
                sql =
                    $"INSERT INTO bookings " +
                    $"(bookingDate,bookingTime,passengerID,seatID) " +
                    $"VALUES(" +
                    $"'{dateNow}'," +
                    $"'{timeNow}'," +
                    $"'{passenID}'," +
                    $"'{seatData.seatID}');";
                adapterBook.InsertCommand = dbConfig.connection.CreateCommand();
                adapterBook.InsertCommand.CommandText = sql;
                adapterBook.InsertCommand.ExecuteNonQuery();
            } catch (Exception ex) {
                MessageBox.Show
                (
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            dbConfig.connection.Close();
        }

        private void postPaymentDB() {
            try {
                dbConfig.connection.Open();
                string sql;

                // SELECT BOOKING ID
                var adapterBook = new SqlDataAdapter();
                var bookTB = new DataTable();
                sql = $"SELECT * FROM bookings";
                adapterBook.SelectCommand = new SqlCommand(sql, dbConfig.connection);
                adapterBook.Fill(bookTB);
                sql =
                    $"passengerID='{passenID}' AND " +
                    $"seatID='{seatData.seatID}'";
                DataRow[] dr = bookTB.Select(sql);
                bookingID = dr[0]["bookingID"].ToString();

                // INSERT PAYMENT
                var adapterPay = new SqlDataAdapter();
                sql =
                    $"INSERT INTO payments " +
                    $"(paymentTotalPrice,paymentStatus,paymentNotifyDate,paymentNotifyTime," +
                    $"paymentDate,paymentTime,paymentMethod,bookingID) " +
                    $"VALUES(" +
                    $"'{flightData.flightPrice + seatData.seatPrice}'," +
                    $"'{paymentStatus}'," +
                    $"'{dateNow}'," +
                    $"'{timeNow}'," +
                    $"'{dateNow}'," +
                    $"'{timeNow}'," +
                    $"'{paymentMethod}'," +
                    $"'{bookingID}');";
                adapterBook.InsertCommand = dbConfig.connection.CreateCommand();
                adapterBook.InsertCommand.CommandText = sql;
                adapterBook.InsertCommand.ExecuteNonQuery();
            } catch (Exception ex) {
                MessageBox.Show
                (
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            dbConfig.connection.Close();
        }

        private void putSeatDB() {
            try {
                dbConfig.connection.Open();
                string sql;

                // UPDATE Seat
                var adapterSeat = new SqlDataAdapter();
                sql =
                    $"UPDATE seats " +
                    $"SET seatStatus='{seatData.seatStatus}' " +
                    $"WHERE seatID='{seatData.seatID}'";
                adapterSeat.UpdateCommand = dbConfig.connection.CreateCommand();
                adapterSeat.UpdateCommand.CommandText = sql;
                adapterSeat.UpdateCommand.ExecuteNonQuery();
            } catch (Exception ex) {
                MessageBox.Show
                (
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            dbConfig.connection.Close();
        }
    }
}