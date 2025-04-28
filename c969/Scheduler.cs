using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Signers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace c969
{
    public partial class Scheduler : Form
    {
        public Scheduler()
        {
            InitializeComponent();
        }
        BindingList<appointment> appointments = new BindingList<appointment>();


        string Connect = "server=127.0.0.1;Port=3306;Username=sqlUser;Password=Passw0rd!;Database=client_schedule";
        private DataAdapter DataAdapter;
        private DataSet DsCustomer;
        private DataSet DsAppointment;
        public void Scheduler_Load(object sender, EventArgs e)
        {
            dataGridViewappt.CurrentCell = null;
            dataGridViewCustomer.ClearSelection();
            BindingList<Customer> customerList = new BindingList<Customer>();
            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = Connect;
            con.Open();

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
                        start = reader.GetDateTime(7),
                        end = reader.GetDateTime(8),
                        User_ID = reader.GetInt32(9),
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

        private void button2_Click(object sender, EventArgs e)
        {
            UpdateAppointment updateAppointment = new UpdateAppointment(appointments);
            updateAppointment.ShowDialog();
            dataGridViewappt.Rows.Clear();
            Scheduler_Load(sender, e);
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
                else if (result == DialogResult.No) { result.; }

            }
            catch (Exception) { MessageBox.Show("Please select an appointment"); }
        }

        private void dataGridViewappt_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridViewappt.ClearSelection();
            dataGridViewappt.CurrentCell = null;
        }

        private void CustomerBinder(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            
            dataGridViewCustomer.ClearSelection();
            dataGridViewCustomer.CurrentCell = null;
        }
    }
}
