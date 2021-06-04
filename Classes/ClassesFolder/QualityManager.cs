﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using static ClassesFolder.Enums;

namespace ClassesFolder
{
    public class QualityManager : Employee
    {

        public QualityManager(string username, 
                              string name, 
                              string surname, 
                              string property, 
                              decimal salary, 
                              int experience, 
                              string hireDate) : base(username, name, surname, property, salary, experience, hireDate)
        {

        }

        public string GetFullName()
        {
            return $"{_name} {_surname}";
        }

        public List<DiscountApplication> GetUncheckedDiscountApplications()
        {
            try
            {
                using var connection = new MySqlConnection(ConnectionInfo.ConnectionString);
                connection.Open();

                var query = @"select applicationID, clientUsername, applicationDatetime, category, taxIdentificationNumber, phoneNumber 
                          from discountapplication
                          where status = @status";

                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@status", "pending");
                using MySqlDataReader reader = cmd.ExecuteReader();
                List<DiscountApplication> applications = new List<DiscountApplication>();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    var cat = reader.GetString(3);

                    Category category = Category.Student;
                    switch (cat)
                    {
                        case "soldier":
                            category = Category.Soldier;
                            break;
                        case "low_income":
                            category = Category.LowIncome;
                            break;
                        case "dissabilities":
                            category = Category.DissabilityIssues;
                            break;
                    }

                    applications.Add(new DiscountApplication(reader.GetString(1),
                                                             reader.GetDateTime(2),
                                                             null,
                                                             category,
                                                             reader.GetString(4),
                                                             reader.GetString(5),
                                                             Status.Pending,
                                                             GetDiscountApplicationFiles(id)));
                }

                return applications;
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

        public List<File> GetDiscountApplicationFiles(int id)
        {
            try
            {
                using var connection = new MySqlConnection(ConnectionInfo.ConnectionString);
                connection.Open();

                var query = @"select fileSize, fileName, file
                          from file 
                          where applicationID = @id;";

                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using MySqlDataReader reader = cmd.ExecuteReader();
                List<File> files = new List<File>();

                while (reader.Read())
                {
                    byte[] buffer = new byte[reader.GetInt64(0)];
                    reader.GetBytes(2, 0, buffer, 0, buffer.Length);
                    files.Add(new File(reader.GetString(1), buffer));
                }

                return files;
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

        public string GetUserFullName(string username)
        {
            try
            {
                using var connection = new MySqlConnection(ConnectionInfo.ConnectionString);
                connection.Open();

                var query = @"select name, surname
                              from User
                              where username = @username;";

                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@username", username);
                using MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                return $"{reader.GetString(0)} {reader.GetString(1)}";
            }
            catch (MySqlException)
            {
                MessageBox.Show("Προκλήθηκε σφάλμα κατά την σύνδεση με τον server. Η εφαρμογή θα τερματιστεί!",
                                "Σφάλμα",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                Application.Exit();
                return "";
            }
        }
    
        public Client GetClient(string username)
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
    
        public Dictionary<string, List<SanitaryComplaint>> GetSanitaryComplaints()
        {
            try
            {
                using var connection = new MySqlConnection(ConnectionInfo.ConnectionString);
                connection.Open();
                var statement = @"select targetUsername, summary, category, busDriverUsername 
                                  from sanitarycomplaint;";

                using var cmd = new MySqlCommand(statement, connection);
                using MySqlDataReader reader = cmd.ExecuteReader();

                Dictionary<string, List<SanitaryComplaint>> dictionary = new Dictionary<string, List<SanitaryComplaint>>();

                while (reader.Read())
                {
                    var username = reader.GetString(0);
                    if (!dictionary.ContainsKey(username))
                        dictionary.Add(username, new List<SanitaryComplaint>());

                    var cat = reader.GetString(2);
                    SanitaryComplaintCategory category = SanitaryComplaintCategory.CloseDistance;
                    switch (cat)
                    {
                        case "wear_mask_refusal":
                            category = SanitaryComplaintCategory.WeakMaskRefusal;
                            break;
                        case "has_illness_symptoms":
                            category = SanitaryComplaintCategory.HasIllnessSymptoms;
                            break;
                    }

                    dictionary[username].Add(new SanitaryComplaint(username, reader.GetString(1), category, reader.GetString(3)));

                }

                return dictionary;
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
    
        public Dictionary<string, List<ClientComplaint>> GetUnckeckedClientComplaints()
        {
            try
            {
                using var connection = new MySqlConnection(ConnectionInfo.ConnectionString);
                connection.Open();
                var statement = @"select targetUsername, summary, category, clientUsername 
                                  from clientcomplaint
                                  where checked = @checked;";

                using var cmd = new MySqlCommand(statement, connection);
                cmd.Parameters.AddWithValue("@checked", false);
                using MySqlDataReader reader = cmd.ExecuteReader();

                Dictionary<string, List<ClientComplaint>> dictionary = new Dictionary<string, List<ClientComplaint>>();

                while (reader.Read())
                {
                    var username = reader.GetString(0);
                    if (!dictionary.ContainsKey(username))
                        dictionary.Add(username, new List<ClientComplaint>());

                    var cat = reader.GetString(2);
                    ClientComplaintCategory category = ClientComplaintCategory.AggresiveBehaviour;
                    switch (cat)
                    {
                        case "rude_bus_driver":
                            category = ClientComplaintCategory.Rude;
                            break;
                        case "late_for_no_reason":
                            category = ClientComplaintCategory.LateForNoReason;
                            break;
                        case "aggresive_driving":
                            category = ClientComplaintCategory.CarelessDriving;
                            break;
                        case "driving_rules_violation":
                            category = ClientComplaintCategory.DrivingRuleViolation;
                            break;
                    }

                    dictionary[username].Add(new ClientComplaint(username, false, reader.GetString(1), category, reader.GetString(3)));
                }

                return dictionary;
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
    
        public void InsertDisciplinaryComplaintInDatabase(DisciplinaryComplaint complaint)
        {
            try
            {
                using var connection = new MySqlConnection(ConnectionInfo.ConnectionString);
                connection.Open();
                var query = @"insert into DisciplinaryComplaint (targetUsername, submittedDatetime, comment, qualityManagerUsername) values
	                      (@targetUsername, @submittedDatetime, @comment, @qualityManagerUsername);";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@targetUsername", complaint.TargetUsername);
                cmd.Parameters.AddWithValue("@submittedDatetime", complaint.Datetime.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@comment", complaint.Summary);
                cmd.Parameters.AddWithValue("@qualityManagerUsername", _username);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                MessageBox.Show("Προκλήθηκε σφάλμα κατά την σύνδεση με τον server. Η εφαρμογή θα τερματιστεί!",
                                "Σφάλμα",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                Application.Exit();
            }
        }
    
        public List<Poll> GetPolls()
        {
            try
            {
                using var connection = new MySqlConnection(ConnectionInfo.ConnectionString);
                connection.Open();
                var statement = @"select title, startingDate, endingDate, question, expired 
                                  from poll;";

                using var cmd = new MySqlCommand(statement, connection);
                using MySqlDataReader reader = cmd.ExecuteReader();

                List<Poll> polls = new List<Poll>();

                while (reader.Read())
                {
                    polls.Add(new Poll(reader.GetString(0),
                                       reader.GetDateTime(1),
                                       reader.GetDateTime(2),
                                       reader.GetString(3),
                                       reader.GetBoolean(4)));
                }

                return polls;
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

        public List<Poll> GetExpiredPolls()
        {
            try
            {
                using var connection = new MySqlConnection(ConnectionInfo.ConnectionString);
                connection.Open();
                var statement = @"select title, startingDate, endingDate, question, expired 
                                  from poll
                                  where expired = @expired;";

                using var cmd = new MySqlCommand(statement, connection);
                cmd.Parameters.AddWithValue("@expired", true);
                using MySqlDataReader reader = cmd.ExecuteReader();

                List<Poll> polls = new List<Poll>();

                while (reader.Read())
                {
                    polls.Add(new Poll(reader.GetString(0),
                                       reader.GetDateTime(1),
                                       reader.GetDateTime(2),
                                       reader.GetString(3),
                                       reader.GetBoolean(4)));
                }

                return polls;
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
    
        public void InsertPollInDatabase(Poll poll)
        {
            try
            {
                using var connection = new MySqlConnection(ConnectionInfo.ConnectionString);
                connection.Open();
                var query = @"insert into Poll values 
                          (@title, @expired, @startingDate, @endingDate, @question, @qualityManagerUsername);";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@title", poll.Title);
                cmd.Parameters.AddWithValue("@expired", false);
                cmd.Parameters.AddWithValue("@startingDate", poll.StartingDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@endingDate", poll.EndingDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@question", poll.Question);
                cmd.Parameters.AddWithValue("@qualityManagerUsername", _username);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                MessageBox.Show("Προκλήθηκε σφάλμα κατά την σύνδεση με τον server. Η εφαρμογή θα τερματιστεί!",
                                "Σφάλμα",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        public void InsertPollChoicesInDatabase(Poll poll, List<string> choices)
        {
            try
            {
                foreach (var choice in choices)
                {
                    using var connection = new MySqlConnection(ConnectionInfo.ConnectionString);
                    connection.Open();
                    var query = @"insert into PollChoice (choice, title) values
                              (@choice, @title);";
                    using var cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@choice", choice);
                    cmd.Parameters.AddWithValue("@title", poll.Title);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException)
            {
                MessageBox.Show("Προκλήθηκε σφάλμα κατά την σύνδεση με τον server. Η εφαρμογή θα τερματιστεί!",
                                "Σφάλμα",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                Application.Exit();
            }
        }
    
        public void InsertFeedbackInDatabase(Feedback feedback)
        {
            try
            {
                using var connection = new MySqlConnection(ConnectionInfo.ConnectionString);
                connection.Open();
                var query = @"insert into Feedback (feedbackText, qualityManagerUsername) values
                          (@feedbackText, @qualityManagerUsername)";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@feedbackText", feedback.FeedbackText);
                cmd.Parameters.AddWithValue("@qualityManagerUsername", _username);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                MessageBox.Show("Προκλήθηκε σφάλμα κατά την σύνδεση με τον server. Η εφαρμογή θα τερματιστεί!",
                                "Σφάλμα",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                Application.Exit();
            }
        }
    }
}
