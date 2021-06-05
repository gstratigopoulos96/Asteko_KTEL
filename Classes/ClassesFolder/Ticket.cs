﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassesFolder
{
    public class Ticket
    {
        private Itinerary _correspondingItinerary;
        private bool _delayedItinerary;
        private bool _used;
        private string _clientUsername;

        public Itinerary CorrespondingItinerary => _correspondingItinerary;
        public bool DelayedItinerary
        {
            get { return _delayedItinerary; }
            set { _delayedItinerary = value; }
        }
        public bool Used
        {
            get { return _used; }
            set { _used = value; }
        }
        public string ClientUsername => _clientUsername;

        public Ticket(Itinerary correspondingItinerary, 
                      bool delayedItinerary, 
                      bool used, 
                      string clientUsername)
        {
            _correspondingItinerary = correspondingItinerary;
            _delayedItinerary = delayedItinerary;
            _used = used;
            _clientUsername = clientUsername;
        }

        public void SetAsUsed()
        {
            try
            {
                _used = true;

                using var connection = new MySqlConnection(ConnectionInfo.ConnectionString);
                connection.Open();

                var query = @"update Ticket
                          set used = @used
                          where itineraryID = @itineraryID and clientUsername = @username;";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@used", _used);
                cmd.Parameters.AddWithValue("@clientUsername", _clientUsername);
                cmd.Parameters.AddWithValue("@itineraryID", _correspondingItinerary.ID);
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

        public void SetAsDelayed()
        {
            try
            {
                _delayedItinerary = true;

                using var connection = new MySqlConnection(ConnectionInfo.ConnectionString);
                connection.Open();

                var query = @"update Ticket 
                          set delayedItinerary = @delayedItinerary
                          where itineraryID = @itineraryID and clientUsername = @username;";

                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@clientUsername", _clientUsername);
                cmd.Parameters.AddWithValue("@itineraryID", _correspondingItinerary.ID);
                cmd.Parameters.AddWithValue("@delayedItinerary", _delayedItinerary);
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

        public void UnsetDelayed()
        {
            try
            {
                _delayedItinerary = false;

                using var connection = new MySqlConnection(ConnectionInfo.ConnectionString);
                connection.Open();

                var query = @"update Ticket 
                          set delayedItinerary = @delayedItinerary
                          where itineraryID = @itineraryID and clientUsername = @username;";

                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@clientUsername", _clientUsername);
                cmd.Parameters.AddWithValue("@itineraryID", _correspondingItinerary.ID);
                cmd.Parameters.AddWithValue("@delayedItinerary", _delayedItinerary);
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
