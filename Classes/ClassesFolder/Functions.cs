﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ClassesFolder
{
    public class Functions
    {
        public static Client GetClientByUsername(string username)
        {
            try
            {
                using var connection = new MySqlConnection(ConnectionInfo.ConnectionString);
                connection.Open();
                var statement = @"select name, surname
                                  from user
                                  where username = @username;";
                using var cmd = new MySqlCommand(statement, connection);

                cmd.Parameters.AddWithValue("@username", username);
                using MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                return new Client(username,
                                  reader.GetString(0),
                                  reader.GetString(1),
                                  "Client");
            }
            catch (MySqlException)
            {
                MessageBox.Show("Προκλήθηκε σφάλμα κατά την σύνδεση με τον server. Η εφαρμογή θα τερματιστεί!",
                                "Σφάλμα",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                Application.Exit();
                return null;
            }
        }

        public static BusDriver GetBusDriverByUsername(string username)
        {
            try
            {
                using var connection = new MySqlConnection(ConnectionInfo.ConnectionString);
                connection.Open();
                var statement = @"select name, surname, salary, experience, hireDate, complaintsCounter
                                  from user 
                                  inner join Employee on User.username = Employee.username
                                  inner join BusDriver on User.username = BusDriver.username
                                  where User.username = @username;";
                using var cmd = new MySqlCommand(statement, connection);

                cmd.Parameters.AddWithValue("@username", username);
                using MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                return new BusDriver(username,
                                     reader.GetString(0),
                                     reader.GetString(1),
                                     "Bus Driver",
                                     reader.GetDecimal(2),
                                     reader.GetInt32(3),
                                     reader.GetString(4),
                                     reader.GetInt32(5));
            }
            catch (MySqlException)
            {
                MessageBox.Show("Προκλήθηκε σφάλμα κατά την σύνδεση με τον server. Η εφαρμογή θα τερματιστεί!",
                                "Σφάλμα",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                Application.Exit();
                return null;
            }
        }
    }
}