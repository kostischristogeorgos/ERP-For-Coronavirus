using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Thema3
{
    
    public partial class Form1 : Form
    {
        int id;
        Employee employee;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loginPanel.Show();
            buttonPanel.Hide();
            insert_panel.Hide();
            dataGrid.Hide();
            return_button.Hide();
            edit_panel.Hide();
            // THE USERNAME AND PASSWORD IS n n.
            // Creates a new employee object .
            employee = new Employee("n", "n");
        }

        private void loginButton_Click(object sender, EventArgs e)
        {   // Calls the checklogin function that checks if the  username and password are correct.
            if (employee.CheckLogin(usernameTextBox.Text, passwordTextBox.Text))
            {
                loginPanel.Hide();
                buttonPanel.Show();
            }
            else
            {
                MessageBox.Show("Wrong username or Passsword.");
            }
        }



        private void insertButton_Click(object sender, EventArgs e)
        {
            buttonPanel.Hide();
            insert_panel.Show();
            return_button.Show();
            delete_button.Hide();
        }
        
        private void insert_button_Click(object sender, EventArgs e)
        {
            // If this textbox is null it means that the user wants to insert and not to update a covidcase.
            if (string.IsNullOrEmpty(search_name_textbox.Text))
            {
                if (String.IsNullOrEmpty(name_textbox.Text) || String.IsNullOrEmpty(email_textbox.Text) || String.IsNullOrEmpty(age_textbox.Text) || String.IsNullOrEmpty(tel_textbox.Text) || String.IsNullOrEmpty(gender_textbox.Text) || String.IsNullOrEmpty(address_textbox.Text))
                {
                    MessageBox.Show("Fill all the textboxes !");
                }
                else
                {
                    // If all of the textboxes are not empty it creates a covidcase object with the attributes given.
                    // It also calls the WriteDb function to enter the case into the database.
                    string date = DateTime.Now.ToString();
                    CovidCase covidCase = new CovidCase(name_textbox.Text, email_textbox.Text, tel_textbox.Text, gender_textbox.Text, age_textbox.Text, symptoms_textbox.Text, address_textbox.Text, date);
                    WriteDB(covidCase);
                    insert_panel.Hide();
                    buttonPanel.Show();
                    name_textbox.Text = "";
                    email_textbox.Text = "";
                    tel_textbox.Text = "";
                    gender_textbox.Text = "";
                    age_textbox.Text = "";
                    symptoms_textbox.Text = "";
                    address_textbox.Text = "";
                }
            }
            else
            {   // This means that the user wants to change a covidcase's attributes.
                // Calls the UpdateDb function to update the Database.
                UpdateDb();
                buttonPanel.Show();
                insert_panel.Hide();
                search_name_textbox.Text = "";
                name_textbox.Text = "";
                email_textbox.Text = "";
                tel_textbox.Text = "";
                gender_textbox.Text = "";
                age_textbox.Text = "";
                symptoms_textbox.Text = "";
                address_textbox.Text = "";
            }
            return_button.Hide();
        }
        private void WriteDB(CovidCase covidCase)
        {
            try
            {   // This function inserts the covid case into the database.
                string connectionString = "Data Source=Database.db;Version=3;";
                SQLiteConnection conn = new SQLiteConnection(connectionString);
                conn.Open();
                string query = "Insert into Cases (Name,Email,Phone,Gender,Age,SubSymptoms,Address,RecordedDate) Values (@Name,@Email,@Phone,@Gender,@Age,@SubSymptoms,@Address,@RecordedDate)";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("Name", covidCase.GetName());
                command.Parameters.AddWithValue("Email", covidCase.GetEmail());
                command.Parameters.AddWithValue("Phone", covidCase.GetPhone());
                command.Parameters.AddWithValue("Gender", covidCase.GetGender());
                command.Parameters.AddWithValue("Age", covidCase.GetAge());
                command.Parameters.AddWithValue("SubSymptoms", covidCase.GetSubsymptoms());
                command.Parameters.AddWithValue("Address", covidCase.GetAddress());
                command.Parameters.AddWithValue("RecordedDate", covidCase.GetDate());
                int return_value = command.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Case recorded successfully.");
            }
            catch
            {
                MessageBox.Show("This name already exists in the Database.");
            }
        }

        private void UpdateDb()
        {
            try
            {   // This function sets the covidcase's new attributes into the database and updates it.
                string connectionString = "Data Source=Database.db;Version=3;";
                SQLiteConnection conn = new SQLiteConnection(connectionString);
                conn.Open();
                string query = "Update Cases set Name= @Name, Email= @Email, Phone = @Phone, Gender = @Gender, Age = @Age, SubSymptoms = @SubSymptoms, Address = @Address where Id=@Id";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("Name", name_textbox.Text);
                command.Parameters.AddWithValue("Email", email_textbox.Text);
                command.Parameters.AddWithValue("Phone", tel_textbox.Text);
                command.Parameters.AddWithValue("Gender",gender_textbox.Text);
                command.Parameters.AddWithValue("Age", age_textbox.Text);
                command.Parameters.AddWithValue("SubSymptoms", symptoms_textbox.Text);
                command.Parameters.AddWithValue("Address", address_textbox.Text);
                command.Parameters.AddWithValue("Id", id);
                int return_value = command.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Case updated successfully.");
            }
            catch
            {
                MessageBox.Show("This name exists already.");
            }
        }
      
        private void viewButton_Click(object sender, EventArgs e)
        {
            buttonPanel.Hide();
            return_button.Show();
            try
            {   // This creates a dataset that's filled by all the database with all of the attributes displayed into the datagrid.
                DataSet dataSet = new DataSet();
                string connectionString = "Data Source=Database.db;Version=3;";
                SQLiteConnection conn = new SQLiteConnection(connectionString);
                conn.Open();
                string query = "Select * from Cases";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                SQLiteCommandBuilder command = new SQLiteCommandBuilder(adapter);
                adapter.Fill(dataSet);
                dataGrid.DataSource = dataSet.Tables[0].DefaultView;
                dataGrid.Show();
            }
            catch
            {
                MessageBox.Show("Error loading the Database.");
            }
        }
        private void search_button_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(search_name_textbox.Text))
            {   
                try
                {   // It searches the Database based on the username given from the user and fills all the textboxes with their values.
                    string connectionString = "Data Source=Database.db;Version=3;";
                    SQLiteConnection conn = new SQLiteConnection(connectionString);
                    conn.Open();
                    string query = "Select * from Cases where Name = @Name";
                    SQLiteCommand command = new SQLiteCommand(query, conn);
                    command.Parameters.AddWithValue("Name", search_name_textbox.Text);
                    SQLiteDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        edit_panel.Hide();
                        insert_panel.Show();
                        id = reader.GetInt32(0);
                        name_textbox.Text = reader.GetString(1).ToString();
                        email_textbox.Text = reader.GetString(2).ToString();
                        tel_textbox.Text = reader.GetString(3).ToString();
                        gender_textbox.Text = reader.GetString(4).ToString();
                        age_textbox.Text = reader.GetString(5).ToString();
                        symptoms_textbox.Text = reader.GetString(6).ToString();
                        address_textbox.Text = reader.GetString(7).ToString();
                    }
                    else
                    {
                        MessageBox.Show("This name doesn't exist.");
                    }
                    reader.Close();
                    conn.Close();
                }
                catch
                {
                    MessageBox.Show("Error finding the name.");
                }
            }
            else
            {
                MessageBox.Show("Fill the name.");
            }
        }
        private void DeleteDb()
        {
            try
            {   // It deletes the searched user from the database.
                string connectionString = "Data Source=Database.db;Version=3;";
                SQLiteConnection conn = new SQLiteConnection(connectionString);
                conn.Open();
                string query = "Delete from Cases where Id=@Id";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                command.Parameters.AddWithValue("Id", id);
                int return_value = command.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Case deleted successfully.");
            }
            catch
            {
                MessageBox.Show("Error deleting Case from the Database");
            }
        }
        private void return_button_Click(object sender, EventArgs e)
        {
            return_button.Hide();
            buttonPanel.Show();
            dataGrid.Hide();
            insert_panel.Hide();
            edit_panel.Hide();
            search_name_textbox.Text = "";
            delete_button.Hide();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void casesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (buttonPanel.Visible == true)
            {
                viewButton.PerformClick();
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
          
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            edit_panel.Show();
            return_button.Show();
            buttonPanel.Hide();
            delete_button.Show();
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            DeleteDb();
            return_button.PerformClick();
        }
    }
}
