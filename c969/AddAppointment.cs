using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace c969
{
    public partial class AddAppointment : Form
    {
        internal AddAppointment(BindingList<appointment> appointments)
        {
            InitializeComponent();
            this.appointments = appointments;
        }
        BindingList<appointment> appointments;

        string Connect = "server=127.0.0.1;Port=3306;Username=sqlUser;Password=Passw0rd!;Database=client_schedule";
        private bool IsOverlapping(DateTime start, DateTime end)
        {
            
            foreach (var appointment in appointments)
            {
                
                // Check if the new appointment overlaps with any existing appointment
                if (start >= appointment.end && end <= appointment.start)
                {
                    return true;
                }
                
            }
            return false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(dateTimePicker1.Value.Hour < 9 || dateTimePicker1.Value.Hour > 17)
            {
                MessageBox.Show("Our office hours are 9 am - 5pm");
                return;
            }
            if(dateTimePicker2.Value.Hour > 17 || dateTimePicker2.Value.Hour <9)
            {
                MessageBox.Show("Our office hours are 9 am - 5pm");
                return;
            }
            if(dateTimePicker1.Value.DayOfWeek == DayOfWeek.Saturday || dateTimePicker1.Value.DayOfWeek == DayOfWeek.Sunday)
            {
                MessageBox.Show("Our office is Closed on weekends and holidays");
                return;
            }
            if(dateTimePicker2.Value.DayOfWeek == DayOfWeek.Saturday || dateTimePicker2.Value.DayOfWeek == DayOfWeek.Sunday)
            {
                MessageBox.Show("Our office is Closed on weekends and holidays");
                return;
            }
            if (IsOverlapping(dateTimePicker1.Value, dateTimePicker2.Value))
            {
                MessageBox.Show("The selected time overlaps with an existing appointment!");
                return;
            }

            MySqlConnection con = new MySqlConnection();
            con.ConnectionString = Connect;
            con.Open();

            MySqlCommand cmd = new MySqlCommand($"INSERT into appointment(title,url, description, type, location, start, end, customerId, userId, contact,createDate,createdBy,lastUpdate,lastUpdateBy) VALUES('{textBoxName.Text.Trim()}','{textBoxUrl.Text.Trim()}','{textBoxType.Text.Trim()}','{textBoxDes.Text.Trim()}','{textBoxLoca.Text.Trim()}','{dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm")}','{dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm")}','{textBoxCustoID.Text.Trim()}','1','{textBox3.Text.Trim()}',NOW(),NOW(),NOW(),NOW())",con);
            cmd.ExecuteNonQuery();
        }

        private void AddAppointment_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dateTimePicker1_MouseDown(object sender, MouseEventArgs e)
        {
            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm";
        }

        private void dateTimePicker2_MouseDown(object sender, MouseEventArgs e)
        {
            dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm";
        }
    }
}
