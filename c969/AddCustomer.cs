using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace c969
{
    
    public partial class AddCustomer : Form
    {
        
        public AddCustomer()
        {
            
        InitializeComponent();

            
        }
        string Connect = "server=127.0.0.1;Port=3306;Username=sqlUser;Password=Passw0rd!;Database=client_schedule";


        private void button1_Click(object sender, EventArgs e)
        {
          var textBoxesToValidate = this.Controls.OfType<System.Windows.Forms.TextBox>()
        .Where(textBox => textBox != textBoxID);

            MySqlConnection con = new MySqlConnection();
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
            

                string addCustomerCountry = $"Insert into country (country, createDate,createdBy,lastUpdate,lastUpdateBy) Values ('{textBoxCountry.Text.Trim()}',NOW(),'Test',NOW(),'Test')";
                string addCustomerCity = $"Insert into City (city, countryId,createDate,createdBy,lastUpdate,lastUpdateBy) Values ('{textBoxCity.Text.Trim()}',LAST_INSERT_ID(),NOW(),'Test',NOW(),'Test')";
                string addCustomerAddress = $"Insert into address (address,address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy) values('{textBoxAddress.Text.Trim()}','',LAST_INSERT_ID(),'{textBoxZip.Text.Trim()}','{textBoxPhone.Text.Trim()}',NOW(),'Test',NOW(),'Test')";
                string addCustomer = $"Insert into Customer (customerName,addressId,active,createDate,createdBy,lastUpdate,lastUpdateBy) values('{textBoxName.Text.Trim()}',LAST_INSERT_ID(),'1',NOW(),'Test',NOW(),'Test')";
            
            
            


                MySqlCommand cmd = new MySqlCommand(addCustomerCountry, con);
                
                cmd.ExecuteNonQuery();
                cmd.CommandText = addCustomerCity;
                cmd.ExecuteNonQuery();
                cmd.CommandText = addCustomerAddress;
                cmd.ExecuteNonQuery();
                cmd.CommandText = addCustomer;
                cmd.ExecuteNonQuery();




            

        }

        private void AddCustomer_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }
    }
}
