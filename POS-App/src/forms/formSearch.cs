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
        private Panel[] arrPanel = new Panel[10];
        private List<string> lsFlightTo = new List<string>();

        public formSearch() {
            InitializeComponent();
        }

        private void formSearch_Load(object sender, EventArgs e) {
            init();
        }

        private void btnSearch_Click(object sender, EventArgs e) {
            validFill();
        }

        private void btnLogout_Click(object sender, EventArgs e) {
            var formLogin = new formLogin();
            formLogin.Show();
            this.Hide();
        }

        private void comboFrom_SelectedIndexChanged(object sender, EventArgs e) {
            fetchTo();
        }

        private void init() {
            // initial panel 
            Panel[] arr = { row1, row2, row3, row4, row5, row6, row7, row8, row9, row10 };
            for (int i = 0; i < arr.Length; i++) arrPanel[i] = arr[i];

            //SET Visible panel turn off
            foreach (var itm in arrPanel) itm.Visible = false;

            // Show user active
            labelID.Text = emp.id;
            labelFname.Text = emp.fname;
            labelLname.Text = emp.lname;
        }

        private bool validFill() {
            // Valid fill complete info
            if (comboFrom.SelectedItem == null ||
                comboTo.SelectedItem == null){
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

            // Valid zero or negative number of passenger
            if (numericPerson.Value < 1) {
                MessageBox.Show
                    (
                        "A number of passengers Cannot be 0 or a negative number.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                return false;
            }

            return true;
        }

        private void fetchData() {
            
        }

        private bool fetchTo() {
            // CLEAR
            lsFlightTo.Clear();
            comboTo.Items.Clear();

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

                // Fetch Data
                foreach (var itm in flightDR) {
                    Console.WriteLine(itm["flightEntryStation"]);
                    string item = itm["flightEntryStation"].ToString();

                    // UNIQUE DATA
                    if (!lsFlightTo.Contains(item)) lsFlightTo.Add(item);
                }
                comboTo.Items.AddRange(lsFlightTo.ToArray());
                dbConfig.connection.Close();
                return true;
            } catch (Exception ex) {
                Console.WriteLine("Error");
                dbConfig.connection.Close();
                return false;
            }

            dbConfig.connection.Close();
            return true;
        }
    }
}