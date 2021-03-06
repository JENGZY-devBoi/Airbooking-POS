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
    public partial class formSearch : Form {
        #region Feilds
        private Panel[] arrPanel = new Panel[10];
        private Label[] arrDeparture = new Label[10];
        private Label[] arrArrival = new Label[10];
        private Label[] arrFlightInfo = new Label[10];
        private Label[] arrPrice = new Label[10];
        private List<string> lsFlightTo = new List<string>();
        private List<string> lsDateDepart = new List<string>();
        private List<DateTime> lsDateDepartDT = new List<DateTime>();
        private List<string> lsFlightID = new List<string>();
        #endregion

        public formSearch() {
            InitializeComponent();
        }

        private void formSearch_Load(object sender, EventArgs e) {
            init();

            // Timer Now
            timerTimeNow.Start();
            labelDateNow.Text = DateTime.Now.ToString("MM/dd/yyyy");
            labelTimeNow.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void btnSearch_Click(object sender, EventArgs e) {
            if (validFill()) fetchDataList();
        }

        private void btnLogout_Click(object sender, EventArgs e) {
            var formLogin = new formLogin();
            formLogin.Show();
            this.Hide();
        }

        private void comboFrom_SelectedIndexChanged(object sender, EventArgs e) {
            fetchTo();
        }

        private void comboTo_SelectedIndexChanged(object sender, EventArgs e) {
            fetchDepart();
        }

        private void init() {
            // initial Controls
            Panel[] panelFlightList = { row1, row2, row3, row4, row5, row6, row7, row8, row9, row10 };
            Label[] labelDepart = { Departure1, Departure2, Departure3, Departure4, Departure5,
                                    Departure6, Departure7, Departure8, Departure9, Departure10};
            Label[] labelArrival = { labelArrival1, labelArrival2, labelArrival3, labelArrival4, labelArrival5, 
                                    labelArrival6, labelArrival7, labelArrival8, labelArrival9, labelArrival10};
            Label[] labelFlightInfo = { labelFlightInfo1, labelFlightInfo2, labelFlightInfo3, labelFlightInfo4, labelFlightInfo5,
                                    labelFlightInfo6, labelFlightInfo7, labelFlightInfo8, labelFlightInfo9, labelFlightInfo10};
            Label[] labelPrice = { labelPrice1, labelPrice2, labelPrice3, labelPrice4, labelPrice5, labelPrice6, labelPrice7,
                                    labelPrice8, labelPrice9, labelPrice10};

            for (int i = 0; i < panelFlightList.Length; i++) {
                arrPanel[i] = panelFlightList[i];
                arrDeparture[i] = labelDepart[i];
                arrArrival[i] = labelArrival[i];
                arrFlightInfo[i] = labelFlightInfo[i];
                arrPrice[i] = labelPrice[i];
            }

            //SET Visible panel turn off
            foreach (var itm in arrPanel) itm.Visible = false;

            // Show user active
            labelID.Text = emp.id;
            labelFname.Text = emp.fname;
            labelLname.Text = emp.lname;
        }

        private void selectFlight(string i) {
            // Minus by 1 because, c# index base on zero
            int idx = Convert.ToInt32(i) - 1;

            flightData.flightID = lsFlightID[idx];
            flightData.flightFrom = comboFrom.SelectedItem.ToString();
            flightData.flightTo = comboTo.SelectedItem.ToString();
            flightData.flightDepart = arrDeparture[idx].Text;
            flightData.flightArrival = arrArrival[idx].Text;
            flightData.flightInfo = arrFlightInfo[idx].Text;
            flightData.flightDateTime = comboDepart.SelectedItem.ToString();
            flightData.flightPrice = Convert.ToDouble(arrPrice[idx].Text);

            // GO TO passengerInfo form
            // Clearß
            passengerData.passengerTitle = null;
            passengerData.passengerEmail = null;
            passengerData.passengerFname = null;
            passengerData.passengerLname = null;
            passengerData.passengerDOB = null;

            var passengerForm = new formPassengerInfo();
            passengerForm.Show();
            this.Hide();
        }

        private bool validFill() {
            // Valid fill complete info
            if (comboFrom.SelectedItem == null ||
                comboTo.SelectedItem == null ||
                comboDepart.SelectedItem == null) {
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

        private void fetchDataList() {
            // CLEAR DATA
            lsFlightID.Clear();

            // SET Visible panel turn off
            foreach (var itm in arrPanel) itm.Visible = false;

            dbConfig.connection.Open();

            var flightAdapter = new SqlDataAdapter();
            var flightTable = new DataTable();
            string sql;

            sql = $"SELECT * FROM flights";

            dbOperation.createCmdSelect(sql);
            flightAdapter.SelectCommand = dbOperation.commandSelect;
            flightAdapter.Fill(flightTable);

            sql =
                $"flightExitStation='{comboFrom.SelectedItem.ToString()}' AND " +
                $"flightEntryStation='{comboTo.SelectedItem.ToString()}' AND " +
                $"flightExitDate='{comboDepart.SelectedItem.ToString()}'";
           
            try {
                DataRow[] flightDR = flightTable.Select(sql);

                int idx = 0;
                foreach (var itm in flightDR) {
                    arrPanel[idx].Visible = true;

                    string fID = itm["flightID"].ToString();
                    string fExitTime = itm["flightExitTime"].ToString();
                    string fEntryTime = itm["flightEntryTime"].ToString();
                    string fExitDate = itm["flightExitDate"].ToString();
                    string fEntryDate = itm["flightEntryDate"].ToString();
                    string fPrice = itm["flightPrice"].ToString();
                  
                    // components
                    arrDeparture[idx].Text = fExitTime;
                    arrArrival[idx].Text = fEntryTime;
                    arrFlightInfo[idx].Text = calcDurFlight(fExitTime, fEntryTime);
                    arrPrice[idx].Text = fPrice;

                    // push flight ID
                    lsFlightID.Add(fID);
            
                    idx++;
                }

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            labelDepart.Text = $"Departure({comboFrom.SelectedItem.ToString()})";
            labelArrival.Text = $"Arrival({comboTo.SelectedItem.ToString()})";
            dbConfig.connection.Close();
        }

        // BAD PRACTICE!!
        private string calcDurFlight(string from, string to) {
            //Console.WriteLine(from + "\n" + to);
            string[] strFrom = from.Split(':');
            string[] strTo = to.Split(':');

            int hrFrom = Convert.ToInt32(strFrom[0]);
            int minFrom = Convert.ToInt32(strFrom[1]);

            int hrTo = Convert.ToInt32(strTo[0]);
            int minTo = Convert.ToInt32(strTo[1]);

            int hr = Math.Abs(hrTo - hrFrom);
            int min = Math.Abs(minTo - minFrom);

            //return $"{((hr > 9) ? "00" : "0")}{hr}:{((min > 9)?"00":"0")}{min}";
            return $"{hr}hr {min}m";
        }

        private bool fetchTo() {
            // CLEAR
            lsFlightTo.Clear();
            comboTo.Items.Clear();
            comboDepart.Items.Clear();

            // DB connnection
            dbConfig.connection.Open();

            var flightAdapter = new SqlDataAdapter();
            var flightTable = new DataTable();
            string sql;

            // Bad practice: because query alway when you select change!!
            sql = "SELECT * FROM flights";
            dbOperation.createCmdSelect(sql);
            flightAdapter.SelectCommand = dbOperation.commandSelect;
            flightAdapter.Fill(flightTable);

            dbOperation.disposeCmdSelect();

            sql = $"flightExitStation='{comboFrom.SelectedItem.ToString()}'";
            try {
                DataRow[] flightDR = flightTable.Select(sql);
                var ls = new List<string>();

                foreach (var itm in flightDR) {
                    //Console.WriteLine(itm["flightEntryStation"]);
                    //Console.WriteLine(itm["flightExitTime"]);
                    string flightTo = itm["flightEntryStation"].ToString();

                    // UNIQUE DATA
                    if (!lsFlightTo.Contains(flightTo)) lsFlightTo.Add(flightTo);
                }

                comboTo.Items.AddRange(lsFlightTo.ToArray());
                //comboDepart.Items.AddRange(lsFlightDepart.ToArray());

                comboTo.Enabled = true;

                dbConfig.connection.Close();
                return true;
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                dbConfig.connection.Close();
                comboTo.Enabled = false;
                return false;
            }
        }

        private bool fetchDepart() {
            // CLEAR
            lsDateDepart.Clear();
            lsDateDepartDT.Clear();
            comboDepart.Items.Clear();

            // DB connnection
            dbConfig.connection.Open();

            var flightAdapter = new SqlDataAdapter();
            var flightTable = new DataTable();
            string sql;

            // Bad practice: because query alway when you select change!!
            sql = "SELECT * FROM flights";
            dbOperation.createCmdSelect(sql);
            flightAdapter.SelectCommand = dbOperation.commandSelect;
            flightAdapter.Fill(flightTable);

            dbOperation.disposeCmdSelect();

            sql = 
                $"flightExitStation='{comboFrom.SelectedItem.ToString()}' " +
                $"AND flightEntryStation='{comboTo.SelectedItem.ToString()}'";

            try {
                DataRow[] departDR = flightTable.Select(sql);
                foreach (var itm in departDR) {
                    //Console.WriteLine(itm["flightExitDate"].ToString().Split(' ')[0]);
                    string date = itm["flightExitDate"].ToString().Split(' ')[0];

                    // Unique data
                    if (!lsDateDepart.Contains(date)) lsDateDepart.Add(date);
                }

                lsDateDepart.Sort();
                comboDepart.Items.AddRange(lsDateDepart.ToArray());

                dbConfig.connection.Close();                
                comboDepart.Enabled = true;
                return true;
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                dbConfig.connection.Close();
                comboDepart.Enabled = false;
                return false;
            }
        }


        private void timerTimeNow_Tick(object sender, EventArgs e) {
            labelTimeNow.Text = DateTime.Now.ToString("HH:mm:ss");
            timerTimeNow.Start();
        }

        private void btnSelect1_Click(object sender, EventArgs e) {
            //selectFlight(Convert.ToInt32(btnSelect1.Tag.ToString()));
            selectFlight(btnSelect1.Tag.ToString());
        }

        private void btnSelect2_Click(object sender, EventArgs e) {
            selectFlight(btnSelect2.Tag.ToString());
        }

        private void btnSelect3_Click(object sender, EventArgs e) {
            selectFlight(btnSelect3.Tag.ToString());
        }

        private void btnSelect4_Click(object sender, EventArgs e) {
            selectFlight(btnSelect4.Tag.ToString());
        }

        private void btnSelect5_Click(object sender, EventArgs e) {
            selectFlight(btnSelect5.Tag.ToString());
        }

        private void btnSelect6_Click(object sender, EventArgs e) {
            selectFlight(btnSelect6.Tag.ToString());
        }

        private void btnSelect7_Click(object sender, EventArgs e) {
            selectFlight(btnSelect7.Tag.ToString());
        }

        private void btnSelect8_Click(object sender, EventArgs e) {
            selectFlight(btnSelect8.Tag.ToString());
        }

        private void btnSelect9_Click(object sender, EventArgs e) {
            selectFlight(btnSelect9.Tag.ToString());
        }

        private void btnSelect10_Click(object sender, EventArgs e) {
            selectFlight(btnSelect10.Tag.ToString());
        }
    }
}