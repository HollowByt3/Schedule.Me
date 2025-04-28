using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace c969
{
    public partial class UpdateCustomer : Form
    {
        public UpdateCustomer()
        {
            InitializeComponent();
            
        }

        BindingList<Customer> customerList = new BindingList<Customer>();

        string Connect = "server=127.0.0.1;Port=3306;Username=sqlUser;Password=Passw0rd!;Database=client_schedule";
        MySqlConnection con = new MySqlConnection();
       
        private void button1_Click(object sender, EventArgs e)
        {
           var textBoxesToValidate = this.Controls.OfType<TextBox>()
        .Where(textBox => textBox != textBoxID);

            con.ConnectionString = Connect;
            con.Open();

            string pattern = @"^[\d-]+$"; // Matches only digits and dashes.
            Regex regex = new Regex(pattern);

            if (!regex.IsMatch(textBoxPhone.Text))
            {
                MessageBox.Show("Please enter a valid phone number.");
                return;
            }
            if (!regex.IsMatch(textBoxZip.Text))
            {
                MessageBox.Show("Please enter a valid Zip code.");
                return;
            }
            if (textBoxesToValidate.Any(textBox => string.IsNullOrWhiteSpace(textBox.Text)))
            {
                MessageBox.Show("Please enter customer details!");
                return;
            }

            string UpdateCustomer = $"UPDATE customer INNER JOIN address ON customer.addressId = address.addressId  INNER JOIN city ON address.cityId = city.cityId INNER JOIN country ON city.countryId = country.countryId SET customer.customerName='{textBoxName.Text.Trim()}',address.address='{textBoxAddress.Text.Trim()}',address.postalCode='{textBoxZip.Text.Trim()}',address.phone='{textBoxPhone.Text.Trim()}',city.city='{textBoxCity.Text.Trim()}',country.country='{textBoxCountry.Text.Trim()}' WHERE customer.customerId='{textBoxID.Text.Trim()}';";
            
            MySqlCommand cmd = new MySqlCommand(UpdateCustomer, con);

            cmd.ExecuteNonQuery();
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
