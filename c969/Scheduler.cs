using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Signers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace c969
{
    public partial class Scheduler : Form
    {
        public Scheduler()
        {
            InitializeComponent();
        }
        BindingList<appointment> Getappointments = new BindingList<appointment>();
        BindingList<appointment> appointments = new BindingList<appointment>();
        List<user> users = new List<user>();

        string Connect = "server=127.0.0.1;Port=3306;Username=sqlUser;Password=Passw0rd!;Database=client_schedule";
        

        

        

        public void Scheduler_Load(object sender, EventArgs e)
        {

            


            UpcomingAppointment(dataGridViewappt, 7);
            
            
            dataGridViewappt.CurrentCell = null;
            dataGridViewCustomer.ClearSelection();
            BindingList<Customer> customerList = new BindingList<Customer>();
            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = Connect;
            con.Open();

            //add users to list
            MySqlCommand getUsers = new MySqlCommand("SELECT user.userName,appointment.start,appointment.end,appointment.description from user INNER JOIN appointment ON user.userId=appointment.userId", con);
            using (MySqlDataReader reader = getUsers.ExecuteReader()) 
            {
                while (reader.Read())
                {
                    user user = new user
                    {
                        userName = reader.GetString(0),
                         Start = reader.GetDateTime(1), 
                        end = reader.GetDateTime(2), 
                        description = reader.GetString(3)
                        
                    };
                    users.Add(user);
                }
            }
            


            //adds customers from database to list

            MySqlCommand sqlcmd = new MySqlCommand("select Customer.customerId, Customer.customerName,  address.address, address.postalcode, address.phone, city.city, Country.country from customer \r\ninner join address on customer.addressId = address.addressId  inner join city on address.cityId = city.cityId inner join country on city.countryId = country.countryId;",con);
            
            using (MySqlDataReader reader = sqlcmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Customer customer = new Customer
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Address = reader.GetString(2),
                        PostalCode = reader.GetString(3),
                        Phone = reader.GetString(4),
                        City = reader.GetString(5),
                        Country = reader.GetString(6),
                        
                    };
                    customerList.Add(customer);
                    

                }
                
            }
            dataGridViewCustomer.DataSource = customerList;



            //adds appointments from database to list
            
            string sqlcmdAppt = "select appointmentId, title, description, location, contact, type, url, start, end, UserId, customerId from appointment;";
            MySqlCommand getAppt = new MySqlCommand(sqlcmdAppt,con);
            using (MySqlDataReader reader = getAppt.ExecuteReader())
            {
                while (reader.Read())
                {
                    appointment appointment = new appointment
                    {
                        Id = reader.GetInt32(0),
                        title = reader.GetString(1),
                        description = reader.GetString(2),
                        location = reader.GetString(3),
                        contact = reader.GetString(4),
                        type = reader.GetString(5),
                        url = reader.GetString(6),
                        Start = reader.GetDateTime(7),
                        end = reader.GetDateTime(8),
                        Customer_ID = reader.GetInt32(10)
                    };
                    appointments.Add(appointment);
                }
            }
            dataGridViewappt.DataSource = appointments;
            

                //con.Close();

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddCustomer newForm = new AddCustomer();
            newForm.ShowDialog();
            dataGridViewCustomer.Rows.Clear();
            Scheduler_Load(sender, e);



        }

        public void buttonUpdate_Click(object sender, EventArgs e)
        {

            UpdateCustomer newForm = new UpdateCustomer();
            Customer SelectedCustomer = dataGridViewCustomer.CurrentRow.DataBoundItem as Customer;
            
            newForm.textBoxID.Text = SelectedCustomer.Id.ToString();
            newForm.textBoxName.Text = SelectedCustomer.Name.ToString();
            newForm.textBoxAddress.Text = SelectedCustomer.Address.ToString();
            newForm.textBoxPhone.Text = SelectedCustomer.Phone.ToString();
            newForm.textBoxCity.Text = SelectedCustomer.City.ToString();
            newForm.textBoxZip.Text = SelectedCustomer.PostalCode.ToString();
            newForm.textBoxCountry.Text = SelectedCustomer.Country.ToString();
            newForm.ShowDialog();
            dataGridViewCustomer.Rows.Clear();
            Scheduler_Load( sender,  e);

            


        }
        //deletes customer from database when selected from datagridview
        private void buttonDel_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection con = new MySqlConnection();
                con.ConnectionString = Connect;
                con.Open();

                Customer SelectedCustomer = dataGridViewCustomer.CurrentRow.DataBoundItem as Customer;

                DialogResult result = MessageBox.Show($"Are you sure you want to delete {SelectedCustomer.Name}?"+ MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {

                    if (SelectedCustomer == null || !dataGridViewCustomer.CurrentRow.Selected) { MessageBox.Show("Please select a Customer!"); }
                    string DeleteCustomer = $"DELETE customer, address, city, country FROM customer INNER JOIN address ON address.addressId = customer.addressId INNER JOIN city ON address.cityId = city.cityId INNER JOIN country ON city.countryId = country.countryId  WHERE customer.customerId='{SelectedCustomer.Id}';";

                    MySqlCommand cmd = new MySqlCommand(DeleteCustomer, con);
                    cmd.ExecuteNonQuery();
                    dataGridViewCustomer.Rows.Clear();
                    Scheduler_Load(sender, e);
                }if (result == DialogResult.No) { Close(); }
            }
            catch (Exception) { MessageBox.Show("Please Select a Customer"); }
        }

        private void PartBinded(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //dataGridViewCustomer.ClearSelection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddAppointment addAppointment = new AddAppointment(appointments);
            addAppointment.ShowDialog();
            dataGridViewappt.Rows.Clear();
            Scheduler_Load(sender, e);

        }

        //This updates appointments
        private void button2_Click(object sender, EventArgs e)
        {
            appointment selectedAppointment = dataGridViewappt.CurrentRow.DataBoundItem as appointment;

            UpdateAppointment updateAppointment = new UpdateAppointment(appointments);
            updateAppointment.textBoxID.Text = selectedAppointment.Id.ToString();
            updateAppointment.textBoxName.Text = selectedAppointment.title.ToString();
            updateAppointment.textBoxType.Text = selectedAppointment.type.ToString();
            updateAppointment.textBoxDes.Text = selectedAppointment.type.ToString();
            updateAppointment.textBoxLoca.Text = selectedAppointment.location.ToString();
            updateAppointment.textBoxUrl.Text = selectedAppointment.url.ToString();
            updateAppointment.textBoxCustoID.Text = selectedAppointment.Customer_ID.ToString();
            updateAppointment.textBox3.Text = selectedAppointment.contact.ToString();


            updateAppointment.ShowDialog();
            dataGridViewappt.Rows.Clear();
            Scheduler_Load(sender, e);
        }

        //generates alert for upcoming appointmentments
        private void UpcomingAppointment(DataGridView dataGridView, int columnIndex)
        {

            foreach (DataGridViewRow row in dataGridViewappt.Rows)
            {
                var StartTime = Convert.ToDateTime(row.Cells["start"].Value);
                TimeSpan difference = DateTime.Now - StartTime;
                
                if (difference.TotalMinutes<=15 && difference.TotalMinutes >= 0)
                {
                    MessageBox.Show("You have an upcoming appointment in"+difference.TotalMinutes);
                }
            }
        }
        

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                appointment selectedAppointment = dataGridViewappt.CurrentRow.DataBoundItem as appointment;

                DialogResult result = MessageBox.Show("Are you sure you want to delete"+ selectedAppointment.title+ " "+ "?","Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    MySqlConnection con = new MySqlConnection();
                    con.ConnectionString = Connect;
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"DELETE FROM appointment WHERE appointmentId='{selectedAppointment.Id}'", con);
                    cmd.ExecuteNonQuery();

                    dataGridViewappt.Rows.Clear();
                    Scheduler_Load(sender, e);
                }
            }
            catch (Exception) { MessageBox.Show("Please select an appointment"); }
        }

        //converts appointment times to the user's timezone
        private void dataGridViewappt_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            bool isDST = localTimeZone.IsDaylightSavingTime(DateTime.Now);

            foreach (DataGridViewRow row in dataGridViewappt.Rows)
        {
                if (row.Cells[7].Value != null && row.Cells[8].Value != null)
                {
                    DateTime startTime = Convert.ToDateTime(row.Cells["start"].Value);
                    DateTime endTime = Convert.ToDateTime(row.Cells["end"].Value);

                   DateTime NewStart = TimeZoneInfo.ConvertTime(startTime, localTimeZone);
                   DateTime NewEnd = TimeZoneInfo.ConvertTime(endTime, localTimeZone);

                    row.Cells[7].Value = NewStart;
                    row.Cells[8].Value = NewEnd;

                }
            }
        }
        

        private void CustomerBinder(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            
            dataGridViewCustomer.ClearSelection();
            dataGridViewCustomer.CurrentCell = null;
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            DateTime selectedDate = monthCalendar1.SelectionStart.Date;
            ShowAppointmentsForDate(selectedDate);
        }

        private void ShowAppointmentsForDate(DateTime date)
        {
            listBox1.Items.Clear();

            foreach (var appointment in appointments)
            {
                if (appointment.Start.Date == date || appointment.end.Date == date)
                {
                    listBox1.Items.Add(appointment.title);
                    listBox1.Items.Add(appointment.description);
                    listBox1.Items.Add(appointment.location);
                }
            }

            if (listBox1.Items.Count == 0)
            {
                listBox1.Items.Add("No appointments for this day.");
            }
        }


       



        //Generate report on click
        private void button3_Click(object sender, EventArgs e)
        {
           if(comboBox1.SelectedItem.ToString() == "Appointment Types by month")
            {
                var Generatedreport = GenerateReport();

                MessageBox.Show($"{Generatedreport}");
                return;
            }
            if (comboBox1.SelectedItem.ToString() == "User Schedule")  
            {
                MessageBox.Show(GenerateSchedule());
                return;
            }
            if (comboBox1.SelectedItem.ToString() == "Number of appointments each month") { MessageBox.Show(appointmentCount()); return; }

                
        }
        public string GenerateReport()
        {
            // Group appointments by month and type
            var report = appointments
                .GroupBy(a => new
                {
                    Month = (a.Start.ToString("MMMM yyyy")), // Group by month (e.g., "January 2025")
                    a.type
                })
                .OrderBy(g => DateTime.ParseExact(g.Key.Month, "MMMM yyyy", CultureInfo.InvariantCulture)) // Sort by month
                .ToDictionary(
                    g => g.Key.Month,
                    g => g.GroupBy(a => a.type)
                          .ToDictionary(typeGroup => typeGroup.Key, typeGroup => typeGroup.Count())
                );

            StringBuilder reportBuilder = new StringBuilder();
            reportBuilder.AppendLine("Appointment Report:");
            foreach (var monthEntry in report)
            {
                reportBuilder.AppendLine($"{monthEntry.Key}");
                foreach (var typeEntry in monthEntry.Value)
                {
                    reportBuilder.AppendLine($"  {typeEntry.Key}: {typeEntry.Value}");
                }
            }

            return reportBuilder.ToString();
        }

        public string GenerateSchedule()
        {
            var schedule = users.OrderBy(x => x.Start).ToList().GroupBy(g => g.userName);

            StringBuilder report = new StringBuilder();
            foreach (var user in users)
            {
            report.AppendLine($"\n{user.userName}:");
            report.AppendLine($" {user.Start} - {user.end}");
            }

            return report.ToString();
        }

        public string appointmentCount()
        {
            var appointmentsByMonth = appointments.GroupBy(x => x.Start.ToString("MMMM yyyy", CultureInfo.InvariantCulture)) // Group by "January 2025", etc.
            .ToDictionary(g => g.Key, g => g.Count());

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Number of appointments each month");
            foreach (var entry in appointmentsByMonth)
            {
                stringBuilder.AppendLine($"{entry.Key}: {entry.Value} appointments");
            }
            return stringBuilder.ToString();
        }

    }
}
