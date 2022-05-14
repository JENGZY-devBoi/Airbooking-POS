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
    public partial class formSelectSeat : Form {
        #region Fields
        private List<Label> lsSeat = new List<Label>();
        #endregion

        public formSelectSeat() {
            InitializeComponent();
        }

        private void formSelectSeat_Load(object sender, EventArgs e) {
            init();
            fetchData();

            // Timer Now
            timerTimeNow.Start();
            labelDateNow.Text = DateTime.Now.ToString("MM/dd/yyyy");
            labelTimeNow.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void init() {
            // Set array seat
            Label[] arrSeat = { 
                seatA1,seatA2,seatA3,seatA4,seatA5,seatA6,seatA7,seatA8,seatA9,seatA10,
                seatB1,seatB2,seatB3,seatB4,seatB5,seatB6,seatB7,seatB8,seatB9,seatB10,
                seatC1,seatC2,seatC3,seatC4,seatC5,seatC6,seatC7,seatC8,seatC9,seatC10,
                seatD1,seatD2,seatD3,seatD4,seatD5,seatD6,seatD7,seatD8,seatD9,seatD10 
            };
            lsSeat.AddRange(arrSeat);


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

        private bool fetchData(){
            dbConfig.connection.Open();
            var seatAdapter = new SqlDataAdapter();
            var seatTable = new DataTable();
            string sql;

            sql = $"SELECT * from seats " +
                $"WHERE flightID='{flightData.flightID}'";
            
            dbOperation.createCmdSelect(sql);
            seatAdapter.SelectCommand = dbOperation.commandSelect;
            seatAdapter.Fill(seatTable);

            sql = 
                $"flightID='{flightData.flightID}'";
            try {
                DataRow[] seatDR = seatTable.Select(sql);

                int idx = 0;
                foreach (var itm in seatDR) {
                    Console.WriteLine(itm["seatStatus"].ToString());
                    if (itm["seatStatus"].ToString() == "reserve") {
                        bookedSeatColor(lsSeat[idx]);
                        lsSeat[idx].Enabled = false;
                    }
                    idx++;
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                dbConfig.connection.Close();
            }

            dbConfig.connection.Close();
            return true;
        }

        //---------------------------------------------------------------
        //---------------------------------------------------------------
        #region Fields ButtonWasClicked
        bool[] btnClicked;
        #endregion


        private void selectedClick(Label lbl, bool click, int idx) {
            if (click) {
                defaultAllSeat(idx);
                selectedColor(lbl);
            } else {
                unselectedColor(lbl);
            }
        }

        private void defaultAllSeat(int idx) {
            for(var i = 0; i < lsSeat.Count; i++) {
                if (lsSeat[i].BackColor == Color.LightGreen) {
                    lsSeat[i].BackColor = Color.FromArgb(192, 192, 255);
                    break;
                }
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

        private void bookedSeatColor(Label lbl) {
            lbl.BackColor = Color.FromArgb(255, 192, 192);
        }
        
        private void detectCliked(Label lbl) {
            int idx = convertIdxSeat(lbl.Tag.ToString());

            btnClicked[idx] = checkClicked(btnClicked[idx]);
            selectedClick(lbl, btnClicked[idx], idx);
        }

        private void seatA1_Click(object sender, EventArgs e) {
            detectCliked(seatA1);
        }

        private void seatA2_Click(object sender, EventArgs e) {
            detectCliked(seatA2);
        }

        private void seatA3_Click(object sender, EventArgs e) {
            detectCliked(seatA3);
        }

        private void seatA4_Click(object sender, EventArgs e) {
            detectCliked(seatA4);
        }

        private void seatA5_Click(object sender, EventArgs e) {
            detectCliked(seatA5);
        }

        private void seatA6_Click(object sender, EventArgs e) {
            detectCliked(seatA6);
        }

        private void seatA7_Click(object sender, EventArgs e) {
            detectCliked(seatA7);
        }

        private void seatA8_Click(object sender, EventArgs e) {
            detectCliked(seatA8);
        }

        private void seatA9_Click(object sender, EventArgs e) {
            detectCliked(seatA9);
        }

        private void seatA10_Click(object sender, EventArgs e) {
            detectCliked(seatA10);
        }
    }
}
