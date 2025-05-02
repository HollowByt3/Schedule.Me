using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Mysqlx.Expr;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace c969
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string Connect = "server=127.0.0.1;Port=3306;Username=sqlUser;Password=Passw0rd!;Database=client_schedule";


        private void button1_Click(object sender, EventArgs e)
        {
            //check for set language
            bool isCultureJapanese = CultureInfo.CurrentUICulture.Name == "ja";
            bool isCultureEnglish = CultureInfo.CurrentUICulture.Name == "en-US";

            while (isCultureEnglish)
            {

                if (UnametextBox.Text.Length == 0)
                {
                    MessageBox.Show("Please enter Username");
                    return;
                }
                else if (PwdtextBox.Text.Length == 0)
                {
                    MessageBox.Show("Please enter your password");
                    return;
                }



                MySqlConnection con = new MySqlConnection();
                con.ConnectionString = Connect;
                con.Open();
                string sqlcmd = "select userName,password from user where userName='" + UnametextBox.Text + "'and password='" + PwdtextBox.Text + "'";
                MySqlCommand cmd = new MySqlCommand(sqlcmd, con);
                cmd.Parameters.AddWithValue("userName", UnametextBox.Text);
                cmd.Parameters.AddWithValue("password", PwdtextBox.Text);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    MessageBox.Show("The Username and Password do not match.");
                    UnametextBox.Clear();
                    PwdtextBox.Clear();
                    UnametextBox.Focus();
                    PwdtextBox.Focus();
                    return;
                }

                
                Scheduler newForm = new Scheduler();
                newForm.Show();
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                // Format log entry
                string logEntry = $"{timestamp} - {UnametextBox.Text}";
                string logFilePath = "C:\\Users\\LabUser\\source\\repos\\c969\\Login_History.txt";
                // Append to file
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(logEntry);
                }

                MessageBox.Show("Login recorded successfully.");

                
                return;
            }
            while(isCultureJapanese)
            {
                if (UnametextBox.Text.Length == 0)
                {
                    MessageBox.Show("ユーザー名を入力してください");
                    return;
                }
                else if (PwdtextBox.Text.Length == 0)
                {
                    MessageBox.Show("パスワードを入力してください");
                    return;
                }



                MySqlConnection con = new MySqlConnection();
                con.ConnectionString = Connect;
                con.Open();
                string sqlcmd = "select userName,password from user where userName='" + UnametextBox.Text + "'and password='" + PwdtextBox.Text + "'";
                MySqlCommand cmd = new MySqlCommand(sqlcmd, con);
                cmd.Parameters.AddWithValue("userName", UnametextBox.Text);
                cmd.Parameters.AddWithValue("password", PwdtextBox.Text);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    MessageBox.Show("ユーザー名とパスワードが一致しません。");
                    UnametextBox.Clear();
                    PwdtextBox.Clear();
                    UnametextBox.Focus();
                    PwdtextBox.Focus();
                    return;
                }

                
                Scheduler newForm = new Scheduler();
                newForm.Show();

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                // Format log entry
                string logEntry = $"{timestamp} - {UnametextBox.Text}";
                string logFilePath = "C:\\Users\\LabUser\\source\\repos\\c969\\Login_History.txt";
                // Append to file
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(logEntry);
                }

                MessageBox.Show("Login recorded successfully.");

                return;
            }



        }


        private async void Form1_Load(object sender, EventArgs e)
        {
          
            var uri = new Uri("https://api.ipgeolocation.io/timezone?apiKey=b43e4b9790914662a9604b20275d22ff");

            using (var client = new HttpClient())
            {
                   try
                {
                    
                        var getReg = await client.GetAsync(uri);
                        var json = await getReg.Content.ReadAsStringAsync();
                        IPGeolocationTimeZone time;

                        time = SimpleJson.DeserializeObject<IPGeolocationTimeZone>(json);
                        label3.Text = $"Timezone:{time.timezone}";
                    
                }
                catch (Exception ex) { label3.Text="Could not get information"; }
            }
            
        }

        private void UnametextBox_TextChanged(object sender, EventArgs e)
        {
        
        }

        private void comboBoxLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxLang.SelectedItem.ToString() == "English") {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            }
            if (comboBoxLang.SelectedItem.ToString() == "Japanese") {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja");
            }
            this.Controls.Clear();
            InitializeComponent();
            Form1_Load(sender, e);
        }
    }
}
