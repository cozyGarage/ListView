using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace bind_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MySqlConnection connection;

        public MainWindow()
        {
            InitializeComponent();

            // Defining connection string
            string connectionString = "server=mariadb.vamk.fi;port=3306;database=e1500953_window_programming;uid=e1500953;password=3T35WyJfNDB;"; // Modify these

            connection = new MySqlConnection(connectionString);
            List<Person> items = new List<Person>();
            try
            {
                connection.Open();
                // Creating query string
                string sql = "SELECT * FROM person";
                // New command object
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                // New reader object
                MySqlDataReader rdr = cmd.ExecuteReader();

                // While reader has rows
                while (rdr.Read())
                {

                    items.Add(new Person() { Id = rdr.GetString(0), Fname = rdr.GetString(1), Lname = rdr.GetString(2), Email = rdr.GetString(3) });

                }
                rdr.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Some problems -> " + ex.Message);
            }

            person_list.ItemsSource = items;
        }

        public class Person
        {
            public string? Id { get; set; }
            public string Fname { get; set; }
            public string Lname { get; set; }
            public string Email { get; set; }
        }

        private void addPerson_Click(object sender, RoutedEventArgs e)
        {
            // Get the values from the input fields
            string newFname = newFnameTextBox.Text;
            string newLname = newLnameTextBox.Text;
            string newEmail = newEmailTextBox.Text;

            // Insert the new row into the database
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("INSERT INTO person (Fname, Lname, Email) VALUES (@fname, @lname, @email)", connection);
                command.Parameters.AddWithValue("@fname", newFname);
                command.Parameters.AddWithValue("@lname", newLname);
                command.Parameters.AddWithValue("@email", newEmail);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("New person added to the database.");
                    // Refresh the ListView
                    RefreshListView();
                }
                else
                {
                    MessageBox.Show("Error adding new person to the database.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error from main window -> " + ex.Message);
            }
            finally
            {
                connection.Close();


            }
        }
        private void RefreshListView()
        {
            List<Person> items = new List<Person>();
            try
            {
                // Creating connection string
                string connectionString = "server=mariadb.vamk.fi;port=3306;database=e1500953_window_programming;uid=e1500953;password=3T35WyJfNDB;"; // Modify these

                MySqlConnection connection = new MySqlConnection(connectionString);

                // Open the connection
                connection.Open();

                // Creating query string
                string sql = "SELECT * FROM person";
                // New command object
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                // New reader object
                MySqlDataReader rdr = cmd.ExecuteReader();

                // While reader has rows
                while (rdr.Read())
                {
                    items.Add(new Person() { Id = rdr.GetString(0), Fname = rdr.GetString(1), Lname = rdr.GetString(2), Email = rdr.GetString(3) });
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when add person -> " + ex.Message);
            }

            person_list.ItemsSource = items;
        }

        private void removePerson_Click(object sender, RoutedEventArgs e)
        {
            // Get the ID of the person to remove from the input field
            string idToRemove = removeIdTextBox.Text;

            // Delete the row from the database with the given ID
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM person WHERE Id=@id", connection);
                cmd.Parameters.AddWithValue("@id", idToRemove);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Person with ID " + idToRemove + " removed from the database.");
                    // Refresh the ListView
                    RefreshListView();
                }
                else
                {
                    MessageBox.Show("No person found with ID " + idToRemove + ".");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing person from database -> " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }


    }
}