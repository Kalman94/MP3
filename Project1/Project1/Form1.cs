using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using System.Xml;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.OleDb;
using System.Collections.Specialized;

namespace Project1
{
    public partial class Form1 : Form
    {
        public bool IsConnectionValid { get; private set; }
        public string OleConnectionString { get; private set; }
        public string AdoConnectionString { get; private set; }
        public string DatabaseName { get; private set; }
        public static string path;
        private object connectionString;
        public Form1()
        {
            InitializeComponent();

            DataTable dataTable = SmoApplication.EnumAvailableSqlServers(true);
            listBox1.ValueMember = "Name";
            listBox1.DataSource = dataTable;

        }
        public void updateConfigFile(string con)
        {
            //updating config file
            XmlDocument XmlDoc = new XmlDocument();
            //Loading the Config file
            XmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            foreach (XmlElement xElement in XmlDoc.DocumentElement)
            {
                if (xElement.Name == "connectionStrings")
                {
                    //setting the coonection string
                    xElement.FirstChild.Attributes[2].Value = con;
                }
            }
            //writing the connection string in config file
            XmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            // MessageBox.Show("done");
        }
        private string GetConnectionString(bool includeDatabase)
        {
            string connectionString;
            if (this.radioButton1.Checked)
                connectionString = string.Format("Server={0}; Integrated Security=SSPI;", listBox1.GetItemText(listBox1.SelectedItem));
            else
                connectionString = string.Format("Server={0}; User ID={1}; Password={2};", listBox1.GetItemText(listBox1.SelectedItem), textBox1.Text, textBox2.Text);

            if (includeDatabase && !string.IsNullOrEmpty(this.listBox2.GetItemText(listBox1.SelectedItem)))
                connectionString += string.Format(" Database={0};", this.listBox2.GetItemText(listBox2.SelectedItem));
            ////////////////////////////////////////////////////////////////////////////////////////////


            string strCon = connectionString;
            updateConfigFile(strCon);
            //  MessageBox.Show(strCon);
            /////////////////////////////////////////////////////////////////////////////////////////
            return connectionString;
        }
        public void PassValue(string strValue)
        {
            connectionString = strValue;
        }
        private string GetOleConnectionString()
        {
            return string.Format("{0} Provider=SQLOLEDB;", this.GetConnectionString(true));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.IsConnectionValid = false;
            this.AdoConnectionString = null;
            this.AdoConnectionString = null;
            if (string.IsNullOrEmpty(this.listBox1.GetItemText(listBox1.SelectedItem)))
            {
                MessageBox.Show("An SQL server must be specified.");
                return;
            }

            if (radioButton2.Checked && string.IsNullOrEmpty(this.textBox1.Text))
            {
                MessageBox.Show("If SQL server authentication is used, a username must be provided.");
                return;
            }
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(this.GetConnectionString(true)))
                {
                    sqlConnection.Open();
                    sqlConnection.Close();
                    MessageBox.Show("Connection successful!");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Connection failed: " + ex.Message);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            string text1 = listBox1.GetItemText(listBox1.GetItemText(listBox1.SelectedItem));
            string text2 = listBox1.GetItemText(listBox1.GetItemText(listBox1.SelectedItem));
            listBox2.Items.Clear();
            if (listBox1.SelectedIndex != -1)
            {
                string serverName = listBox1.SelectedValue.ToString();
                Server server = new Server(serverName);
                try
                {
                    foreach (Database database in server.Databases)
                    {
                        listBox2.Items.Add(database.Name);
                    }
                }
                catch (Exception ex)
                {
                    string exception = ex.Message;
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled =
           textBox2.Enabled =
           radioButton1.Checked;
            string connectionString;
            if (this.radioButton1.Checked)
                connectionString = string.Format("Server={0}; Integrated Security=SSPI;", listBox1.GetItemText(listBox1.SelectedItem));
            else
                connectionString = string.Format("Server={0}; User ID={1}; Password={2};", listBox1.GetItemText(listBox1.SelectedItem), textBox1.Text, textBox2.Text);

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled =
             textBox2.Enabled =
             radioButton2.Checked;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            DataTable dataTable = SmoApplication.EnumAvailableSqlServers(true);
            listBox1.ValueMember = "Name";
            listBox1.DataSource = dataTable;


        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.IsConnectionValid = false;
            this.AdoConnectionString = null;
            this.AdoConnectionString = null;

            if (string.IsNullOrEmpty(this.listBox1.GetItemText(listBox1.SelectedItem)))
            {
                MessageBox.Show("An SQL server must be specified.");
                return;
            }

            if (radioButton2.Checked && string.IsNullOrEmpty(this.textBox1.Text))
            {
                MessageBox.Show("If SQL server authentication is used, a username must be provided.");
                return;
            }

            if (string.IsNullOrEmpty(listBox2.GetItemText(listBox1.GetItemText(listBox1.SelectedItem))))
            {
                MessageBox.Show("A database name must be provided.");
                return;
            }

            try
            {
                using (OleDbConnection connection = new OleDbConnection(this.GetOleConnectionString()))
                {
                    connection.Open();
                    connection.Close();
                    this.AdoConnectionString = this.GetConnectionString(true);
                    this.OleConnectionString = this.GetOleConnectionString();
                    this.IsConnectionValid = true;
                    this.DatabaseName = this.listBox2.GetItemText(listBox2.SelectedItem);

                    Form3 Form3 = new Form3();

                    this.Hide();
                    Form3.ShowDialog();
                    this.Close();
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Connection failed: " + ex.Message);
            }

            this.Close();
        }
    }
}
