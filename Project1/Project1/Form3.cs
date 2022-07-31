using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Sql;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Mp3Lib;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Media;
using System.Text.RegularExpressions;
using System.Diagnostics;
using NAudio.Wave;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
namespace Project1
{
    public partial class Form3 : Form
    {
        MemoryStream mp3file;
        Mp3FileReader mp3reader;
        WaveOut waveOut;
        public string connection = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString.ToString();
        public Form3()
        {
            InitializeComponent();
            string connection = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString.ToString();
            //  MessageBox.Show(connection);
            string sql = "SELECT * FROM Files";
            SqlConnection conn = new SqlConnection(connection);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataTable ds = new DataTable();
            conn.Open();
            dataadapter.Fill(ds);
            conn.Close();
            dataGridView1.DataSource = ds;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            // dataGridView1.DataMember = "Authors_table"; 
           
       
        }

        private void Form3_Load(object sender, System.EventArgs e) {
         
        }



        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Title = "Browse mp3 Files";
            openFileDialog1.Filter = "mp3 files (*.mp3)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            textBox1.Text = openFileDialog1.FileName;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
        }

       

        public bool CheckAll()
        {
            string VarDirectoryPath = textBox1.Text;
            string[] files = Directory.GetFiles(VarDirectoryPath);
            foreach (string filename in files)
            {
                Mp3Lib.Mp3File mp3File = new Mp3Lib.Mp3File(filename);
                string f = mp3File.FileName;
                if (this.dataGridView1.Columns.Contains(f))
                {
                    return true;
                }
            }
            return false;
        }

        public static string SecureReplace(string varValue)
        {

            if (String.IsNullOrWhiteSpace(varValue))
                return "";

            string[] badKeywords = new string[] 
        {
            "]", "[", "--", "=", "/*", "*/", "<", ">", "'", "\"", "`","(",")","_","'"," '","'","`",".","'","'","'","'","'."
        };

            for (int ii = 0; ii < badKeywords.Length; ii++)
            {
                varValue = varValue.Replace(badKeywords[ii], "");
            }
            varValue = varValue.Trim();


            string[] lowerGreek = { "α", "β", "γ", "δ", "ε", "ζ", "η", "θ", "ι", "κ", "λ", "μ", "ν", "ξ", " ο", "π", "ρ", "σ", "τ", "υ", "φ", "χ", "ψ", "ω" };



            string[] capitalGreek = { "Α", "Β", "Γ", "Δ", "Ε", "Ζ", "Η", "Θ", "Ι", "Κ", "Λ", "Μ", "Ν", "Ξ", "Ο", "Π", "Ρ", "Σ", "Τ", "Υ", "Φ", "Χ", "Ψ", "Ω" };
            string[] lowerUsa = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string[] capitalUsa = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "v", "w", "x", "y", "z" };




            varValue = varValue.ToUpper();
            for (int iii = 0; iii < 24; iii++)
            {
                varValue = varValue.Replace(capitalGreek[iii], capitalUsa[iii]);
                //  varValue = varValue.Replace(lowerGreek[iii],lowerUsa[iii]);
            }
            return varValue;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            String columnName = this.dataGridView1.Columns[e.ColumnIndex].Name;
            MessageBox.Show(columnName);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Declare Variable and set value,Provide the folder which contains files
            string VarDirectoryPath = textBox1.Text; ;
            string connection = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString.ToString();
            SqlConnection SQLConnection = new SqlConnection(connection);

            //get all files from directory or folder to get file's informationstring VarDirectoryPath = textBox1.Text; ;
            string[] files = Directory.GetFiles(VarDirectoryPath);

            //loop through files
            foreach (string filename in files)
            {
                SqlCommand SqlCmd = new SqlCommand();
                SqlCmd.Connection = SQLConnection;
                SQLConnection.Open();
                FileInfo file = new FileInfo(filename);
                Mp3Lib.Mp3File file2;
                
             
                string varValue = file.Name;
                SecureReplace(varValue);
                ElStr el2lat = new ElStr();
                varValue = el2lat.ToLatin(varValue);
                bool x = true;
                string path = file.FullName;
                path = el2lat.ToLatin(path);

             
                varValue = varValue.Replace(" ", "-").Replace("\t", "-");

                varValue = varValue.Replace(" ", "_").Replace("\t", "_");

                varValue = varValue.Replace(" ", "").Replace("\t", "");


                path = path.Replace(" ", "-").Replace("\t", "-");

                path = path.Replace(" ", "_").Replace("\t", "_");

                path = path.Replace(" ", "").Replace("\t", "");
                string c = textBox1.Text + file.Name;
                string c1 = textBox1.Text + varValue;
                // Directory.Move(c, c1);
                // Directory.Move(file.FullName, path);
                string path11 = file.FullName;
                string file1 = Path.GetFileNameWithoutExtension(path11);
                string path1 = Path.GetFileNameWithoutExtension(path);
                string NewPath = path.Replace(file1, path1);
                //string NewPath = path.Replace(file1, path1);
                //  Console.WriteLine(NewPath); //photo\myFolder\image-resize.jpg
                string extension = file.Extension;
                //  bool exists =this.dataGridView1.Name.Contains(varValue);               
               
               
                try
                {

                    for (int rows = 0; rows < dataGridView1.Rows.Count; rows++)
                    {

                        for (int col = 0; col < dataGridView1.Rows[rows].Cells.Count; col++)
                        {
                            string s1;
                            s1 = dataGridView1.Rows[0].Cells[0].Value.ToString();
                            if (varValue == s1)
                            {



                                SQLConnection.Close();
                                x = false;
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("try again" + ex);
                }
                
                if (x == true)
                {

                    SqlCmd.CommandText += "  SET ANSI_WARNINGS OFF ";

                    SqlCmd.CommandText += "Insert into dbo.Files(";
                    SqlCmd.CommandText += "[Name],[Filepath])";
                    SqlCmd.CommandText += " Values( '" + varValue + "',  '" + path + "' )";
                    SqlCmd.ExecuteNonQuery();
                    SQLConnection.Close();

                }


                SQLConnection.Close();
                Openn();
                
            }

        }

    


        public void Openn()
        {

            string connection = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString.ToString();
            string sql = "SELECT * FROM Files";
            SqlConnection conn = new SqlConnection(connection);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataTable ds = new DataTable();
            conn.Open();
            dataadapter.Fill(ds);
            conn.Close();
            dataGridView1.DataSource = ds;
          
        }

        private void button4_Click(object sender, EventArgs e)
        {

            string connection = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString.ToString();
            string sql = "DElete from Files  where (Name !='')  ";

            SqlConnection conn = new SqlConnection(connection);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);

            DataTable ds = new DataTable();
            conn.Open();
            dataadapter.Fill(ds);
            conn.Close();
            SqlConnection sqlConnection1 = new SqlConnection(connection);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = " DBCC CHECKIDENT (Files, RESEED, 0)";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            reader = cmd.ExecuteReader();
            // Data is accessible through the DataReader object here.

            sqlConnection1.Close();
            dataGridView1.DataSource = ds;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

        }
        //   public string tableName="Files";

        private void button1_Click_1(object sender, EventArgs e)
        {
            string connection = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString.ToString();
            string sql = "IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Files' AND xtype='U') ";
            sql += "CREATE TABLE [dbo].[Files](";
            sql += "[id] [int] IDENTITY(1,1) NOT NULL,";
            sql += "[Name] [nvarchar](50) NULL,";
            sql += "[Filepath] [nvarchar](50) NULL,";
            sql += " CONSTRAINT [PK_Files] PRIMARY KEY CLUSTERED (";
            sql +="[id] ASC1";
                sql +=")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]";
                sql +=") ON [PRIMARY]";
              
            SqlConnection conn = new SqlConnection(connection);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);

            conn.Open();

            conn.Close();
            MessageBox.Show("Table created");
        }



        public static string CleanFileName3(string filename)
        {
            string regexSearch = string.Format("{0}{1}",
                new string(System.IO.Path.GetInvalidFileNameChars()),
                new string(System.IO.Path.GetInvalidPathChars()));
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            string file = r.Replace(filename, "");

            return file;
        }

        public static string CleanFileName4(string filename)
        {
            return new String(filename.Except(System.IO.Path.GetInvalidFileNameChars()).ToArray());
        }

        public static string CleanFileName5(string filename)
        {
            string file = filename;

            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                file = file.Replace(c, '_');
            }
            return file;
        }
        private void button5_Click(object sender, System.EventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.Rows[dataGridView1.CurrentRow.Index];

            string a = Convert.ToString(selectedRow.Cells["Name"].Value);
            a = a.Replace(@"\", @"\\");
            a = a.Replace(@"-", @" ");
            MessageBox.Show(textBox1.Text+"\\"+a);
            // a = a.Replace(@".mp3", @".wav");
            //   a = a.Replace(@".Mp3", @".wav");
            //   a = a.Replace(@".MP3", @".wav");
            Mp3FileReader reader = new Mp3FileReader(textBox1.Text+a);
            var waveOut = new WaveOut(); // or WaveOutEvent()
            waveOut.Init(reader);

            label2.Text = reader.TotalTime.Minutes.ToString();
        
            waveOut.Play();
           
       
        }
    
        private void button6_Click(object sender, System.EventArgs e)
        {
            waveOut.Pause();

        }

        private void button2_Click(object sender, System.EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {
            textBox1.Text=textBox1.Text.Replace( @"\",@"\\");
        }

        private void button2_Click_1(object sender, System.EventArgs e)
        {
           
            string VarDirectoryPath = textBox1.Text; ;
            string[] files = Directory.GetFiles(VarDirectoryPath);
            //    SqlCommand SqlCmd = new SqlCommand();
            //loop through files

            Process [] runingProcess= Process.GetProcesses();
            for (int i=0; i<runingProcess.Length; i++)
            {
                // compare equivalent process by their name
                if(runingProcess[i].ProcessName=="mspaint")
                {
                    // kill  running process
                   runingProcess[i].Kill();
                }
     
            }
           foreach (string filename in files)
            {
                FileInfo file = new FileInfo(filename);
                string varValue = file.Name;
                SecureReplace(varValue);
                ElStr el2lat = new ElStr();
                varValue = el2lat.ToLatin(varValue);
                bool x = true;
                string path = file.FullName;
                path = el2lat.ToLatin(path);
                FileInfo fileInfo = new FileInfo(file.FullName);
                   // System.IO.File.Move(file.FullName, path);
                    fileInfo.MoveTo(path);
                  //  MessageBox.Show(file.FullName);
                  //  MessageBox.Show(path);
                    var filee = ShellFile.FromFilePath(filename);
                    //MessageBox.Show(filee.ToString());
                    if (filee.Properties.System.Title.Value != null)
                    {
                        filee.Properties.System.Title.Value = "";
                        filee.Properties.System.Title.Value = filee.Properties.System.Title.Value + varValue.ToString();
                     //   MessageBox.Show(filee.Properties.System.Title.Value);
                    }
                
            }

           MessageBox.Show("ok");
             
         
           button5.Visible = true;
           button6.Visible = true;
        }
      


    }


}


        
        
    
    
