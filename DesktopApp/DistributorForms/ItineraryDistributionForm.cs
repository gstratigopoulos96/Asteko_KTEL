﻿using ClassesFolder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static ClassesFolder.Enums;

namespace DistributorForms
{
    public partial class ItineraryDistributionForm : Form
    {
        private ItineraryDistributionManager _distributor;
        private List<Reservation> _reservations;
        private Dictionary<string, List<Reservation>> _dictionary;
        private List<BusLine> _busLines;
        private int _busLineIndex;
        private List<Bus> _buses;
        private List<BusDriver> _busDrivers;
        private string _date;
        private string _hour;

        public ItineraryDistributionForm(ItineraryDistributionManager distributor)
        {
            _distributor = distributor;
            InitializeComponent();
        }

        private void ItineraryDistributionForm_Load(object sender, EventArgs e)
        {
            _dictionary = new Dictionary<string, List<Reservation>>();
            _reservations = _distributor.GetReservations();
            _busLines = _distributor.GetBusLines();

            foreach (var reservation in _reservations)
            {
                var key = $"{reservation.TravelDatetime.ToString("yyyy-MM-dd HH:mm:ss")}{reservation.ResBusLine}";
                if (!_dictionary.ContainsKey(key))
                {
                    _dictionary.Add(key, new List<Reservation>());
                    _dictionary[key].Add(reservation);
                }
                else
                {
                    _dictionary[key].Add(reservation);
                }
            }

            foreach (var key in _dictionary.Keys)
            {
                var item = _dictionary[key];
                rereservationsListview.Items.Add(new ListViewItem(new string[]
                {
                    item[0].TravelDatetime.ToString("yyyy-MM-dd HH:mm:ss"),
                    item[0].ResBusLine.ToString(),
                    _dictionary[key].Count.ToString()
                }));
            }

            busLineNumberCombobox.Items.AddRange(_distributor.GetBusLines().Select(x => x.Number.ToString()).ToArray());
            sizeCombobox.Items.AddRange(new string[] { "Μεγάλο", "Μεσαίο", "Μικρό" });
        }

        private void ProgrammingButton_Click(object sender, EventArgs e)
        {
            if (busLineNumberCombobox.SelectedItem != null &&
                availableStartingHoursCombobox.SelectedItem != null &&
                sizeCombobox.SelectedItem != null
                )
            {
                recommendedBusesListview.Items.Clear();
                recommendedDriversListview.Items.Clear();

                if (_distributor.CheckDuplicateItinerary(busLineNumberCombobox.SelectedItem.ToString(),
                    $"{dateTimePicker.Value.ToString("yyyy-MM-dd")} {availableStartingHoursCombobox.SelectedItem}:00"))
                {
                    MessageBox.Show("Υπάρχει ήδη το συγκεκριμένο δρομολόγιο.",
                                    "Σφάλμα",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }


                _busLineIndex = busLineNumberCombobox.SelectedIndex;

                var lastMinuteDates = GetLastMinuteAvailableDates();
                if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                {
                    if (!lastMinuteDates.Contains(dateTimePicker.Value.ToString("dd-MM-yyyy")))
                    {
                        MessageBox.Show($"Μη επιτρεπτή ημερομηνία. Παρακαλώ εισάγετε ημερομηνία στο διάστημα [{lastMinuteDates[0]}, {lastMinuteDates[lastMinuteDates.Count - 1]}]",
                                        "Σφάλμα",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    var reservationDates = GetReservationAvailableDates().Select(x => x).Where(x => !lastMinuteDates.Contains(x)).ToList();
                    if (!reservationDates.Contains(dateTimePicker.Value.ToString("dd-MM-yyyy")))
                    {
                        MessageBox.Show($"Μη επιτρεπτή ημερομηνία. Παρακαλώ εισάγετε ημερομηνία στο διάστημα [{reservationDates[0]}, {reservationDates[reservationDates.Count - 1]}]",
                                        "Σφάλμα",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                }

                var busDrivers = _distributor.GetBusDrivers();
                var duration = _busLines[busLineNumberCombobox.SelectedIndex].Duration;

                busDrivers = busDrivers
                    .Select(x => x)
                    .Where(x => !x.IsOnPaidLeave(dateTimePicker.Value.ToString("yyyy-MM-dd")) && 
                                x.IsAvailableOnHour(dateTimePicker.Value.ToString("yyyy-MM-dd"), 
                                                    availableStartingHoursCombobox.SelectedItem.ToString(),
                                                    duration) &&
                                !x.IsLeadToOverWorking(duration))
                    .ToList();

                var buses = _distributor.GetBuses();

                buses = buses
                    .Select(x => x)
                    .Where(x => x.IsAvailableOnHour(dateTimePicker.Value.ToString("yyyy-MM-dd"),
                                                    availableStartingHoursCombobox.SelectedItem.ToString(),
                                                    duration))
                    .ToList();

                var startStop = _busLines[busLineNumberCombobox.SelectedIndex].Stops[0];
                var endStop = _busLines[busLineNumberCombobox.SelectedIndex].Stops[_busLines[busLineNumberCombobox.SelectedIndex].Stops.Count - 1];


                var recBusDrivers = busDrivers
                    .Select(x => x)
                    .Where(x => x.HasItineraryEndTimeAndNoNextItineraryOnSpecificTime(dateTimePicker.Value.ToString("yyyy-MM-dd"),
                                                                                      availableStartingHoursCombobox.SelectedItem.ToString(),
                                                                                      startStop) &&
                                x.DoesntHaveImmidiatelyItinerary(dateTimePicker.Value.ToString("yyyy-MM-dd"),
                                                                 availableStartingHoursCombobox.SelectedItem.ToString(),
                                                                 duration,
                                                                 endStop))
                    .ToList();

                var recBuses = buses
                    .Select(x => x)
                    .Where(x => x.HasItineraryEndTimeAndNoNextItineraryOnSpecificTime(dateTimePicker.Value.ToString("yyyy-MM-dd"),
                                                                                      availableStartingHoursCombobox.SelectedItem.ToString(),
                                                                                      startStop) &&
                                x.DoesntHaveImmidiatelyItinerary(dateTimePicker.Value.ToString("yyyy-MM-dd"),
                                                                 availableStartingHoursCombobox.SelectedItem.ToString(),
                                                                 duration,
                                                                 endStop))
                    .ToList();

                _hour = availableStartingHoursCombobox.SelectedItem.ToString();
                _date = dateTimePicker.Value.ToString("yyyy-MM-dd");

                if (recBusDrivers.Count > 0 && recBuses.Count > 0)
                {
                    var result = MessageBox.Show("Εντοπίσαμε κάποιες προτάσεις για οδηγούς και λεωφορεία. Θα θέλατε να τις δείτε;",
                                                 "Ερώτηση",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        _busDrivers = recBusDrivers.OrderBy(x => x.AvailableWorkingHours).ToList();
                        foreach (var busDriver in _busDrivers)
                        {
                            recommendedDriversListview.Items.Add(new ListViewItem(new string[]
                            {
                                $"{busDriver.Name} {busDriver.Surname}",
                                (busDriver.AvailableWorkingHours / 60m).ToString("#.##")
                            }));
                        }

                        _buses = recBuses;

                        foreach (var bus in recBuses)
                        {
                            int size = -1;
                            switch (bus.Size)
                            {
                                case Enums.BusSize.SMALL:
                                    size = 2;
                                    break;
                                case Enums.BusSize.MEDIUM:
                                    size = 3;
                                    break;
                                case Enums.BusSize.LARGE:
                                    size = 5;
                                    break;
                            }

                            recommendedBusesListview.Items.Add(new ListViewItem(new string[]
                            {
                                bus.Id.ToString(),
                                size.ToString()
                            }));
                        }

                        return;
                    }
                }

                busDrivers = busDrivers
                    .Select(x => x)
                    .Where(x => x.FindIfCanBeAccepted(dateTimePicker.Value.ToString("yyyy-MM-dd"),
                                                      availableStartingHoursCombobox.SelectedItem.ToString(),
                                                      duration))
                    .ToList();

                if (sizeCombobox.SelectedItem != null)
                {
                    BusSize size = BusSize.SMALL;
                    switch (sizeCombobox.SelectedItem.ToString())
                    {
                        case "Μεγάλο":
                            size = BusSize.LARGE;
                            break;
                        case "Μεσαίο":
                            size = BusSize.MEDIUM;
                            break;
                    }

                    buses = buses
                        .Select(x => x)
                        .Where(x => x.FindIfCanBeAccepted(dateTimePicker.Value.ToString("yyyy-MM-dd"),
                                                          availableStartingHoursCombobox.SelectedItem.ToString(),
                                                          duration) &&
                                    x.Size == size)
                        .ToList();
                }

                _busDrivers = busDrivers.OrderBy(x => x.AvailableWorkingHours).ToList();
                foreach (var busDriver in _busDrivers)
                {
                    recommendedDriversListview.Items.Add(new ListViewItem(new string[]
                    {
                                $"{busDriver.Name} {busDriver.Surname}",
                                (busDriver.AvailableWorkingHours / 60m).ToString("#.##")
                    }));
                }

                _buses = buses;
                foreach (var bus in buses)
                {
                    int size = -1;
                    switch (bus.Size)
                    {
                        case Enums.BusSize.SMALL:
                            size = 2;
                            break;
                        case Enums.BusSize.MEDIUM:
                            size = 3;
                            break;
                        case Enums.BusSize.LARGE:
                            size = 5;
                            break;
                    }

                    recommendedBusesListview.Items.Add(new ListViewItem(new string[]
                    {
                                bus.Id.ToString(),
                                size.ToString()
                    }));
                }

            }
            else
            {
                MessageBox.Show("Παρακαλώ συμπληρώστε όλα τα πεδία.", 
                                "Σφάλμα", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
            }
        }

        private void BusLineNumberCombobox_SelectedValueChanged(object sender, EventArgs e)
        {
            availableStartingHoursCombobox.Items.Clear();

            if (busLineNumberCombobox.SelectedItem != null)
            {
                var busLine = _busLines.Find(x => x.Number == int.Parse(busLineNumberCombobox.SelectedItem.ToString()));

                TimeSpan timeSpan = new TimeSpan(9, 0, 0);
                while (timeSpan.Hours != 23)
                {
                    availableStartingHoursCombobox.Items.Add(timeSpan.ToString("hh':'mm"));
                    timeSpan = timeSpan.Add(new TimeSpan(0, busLine.Duration, 0));
                }
                availableStartingHoursCombobox.Items.Add(timeSpan.ToString("hh':'mm"));
            }
        }

        private void CreateItineraryButton_Click(object sender, EventArgs e)
        {
            if (recommendedBusesListview.CheckedItems.Count == 1 &&
                recommendedDriversListview.CheckedItems.Count == 1)
            {
                DateTime targetDatetime = DateTime.Parse($"{_date} {_hour}:00");
                string busDriverUsername = _busDrivers[recommendedDriversListview.CheckedIndices[0]].Username;
                BusLine busLine = _busLines[_busLineIndex];
                Bus bus = _buses.Find(x => x.Id == int.Parse(recommendedBusesListview.Items[recommendedBusesListview.CheckedIndices[0]].SubItems[0].Text));

                int size = 5;
                switch (bus.Size)
                {
                    case BusSize.SMALL:
                        size = 2;
                        break;
                    case BusSize.MEDIUM:
                        size = 3;
                        break;
                }

                Itinerary itinerary = new Itinerary(-1,
                                                    targetDatetime,
                                                    busDriverUsername,
                                                    busLine,
                                                    bus,
                                                    ItineraryStatus.NoDelayed, 
                                                    size);

                _distributor.InsertItineraryInDatabase(itinerary);

                _busDrivers[recommendedDriversListview.CheckedIndices[0]].DecreaseAvailableWorkingHours(busLine.Duration);

                MessageBox.Show("Επιτυχής καταχώρηση δρομολογίου.",
                                "Επιτυχία",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Παρακαλώ επιλέξτε οδηγό και λεωφορείο.",
                                "Σφάλμα",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        public static List<string> GetLastMinuteAvailableDates()
        {
            List<string> dates = new List<string>();
            if (DateTime.Today.DayOfWeek != DayOfWeek.Sunday)
            {
                DateTime start = DateTime.Today;
                DateTime end = DateTime.Today;
                start = start.AddDays(1);
                end = end.AddDays(7 - (int)DateTime.Today.DayOfWeek);
                while (start <= end)
                {
                    dates.Add(start.ToString("dd-MM-yyyy"));
                    start = start.AddDays(1);
                }
            }

            return dates;
        }

        public static List<string> GetReservationAvailableDates()
        {
            List<string> dates = new List<string>();
            DateTime start = DateTime.Today;
            DateTime end = DateTime.Today;
            start = start.AddDays(1);
            if (DateTime.Today.DayOfWeek != DayOfWeek.Sunday)
            {
                end = end.AddDays(14 - (int)DateTime.Today.DayOfWeek);
                while (start <= end)
                {
                    dates.Add(start.ToString("dd-MM-yyyy"));
                    start = start.AddDays(1);
                }
            }
            else
            {
                end = end.AddDays(7 - (int)DateTime.Today.DayOfWeek);
                while (start <= end)
                {
                    dates.Add(start.ToString("dd-MM-yyyy"));
                    start = start.AddDays(1);
                }
            }

            return dates;
        }
    }
}