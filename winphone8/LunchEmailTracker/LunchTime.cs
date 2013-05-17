using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchEmailTracker
{
    public class LunchTime : INotifyPropertyChanged
    {
        private DateTime _start;
        public DateTime Start
        {
            get { return _start; }
            set
            {
                _start = value;
                PropChanged("Start");
            }
        }

        private DateTime _end;
        public DateTime End
        {
            get { return _end; }
            set
            {
                _end = value;
                PropChanged("End");
            }
        }

        private string _restaurant;
        public string Restaurant
        {
            get { return _restaurant; }
            set
            {
                _restaurant = value;
                PropChanged("Restaurant");
            }
        }

        private string _driver;
        public string Driver
        {
            get { return _driver; }
            set
            {
                _driver = value;
                PropChanged("Driver");
            }
        }

        private string _attendance;
        public string Attendance
        {
            get { return _attendance; }
            set
            {
                _attendance = value;
                PropChanged("Attendance");
            }
        }

        private string _notes;
        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                PropChanged("Notes");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void PropChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public override string ToString()
        {
            return String.Format("{0}\n{1}\n{2}\n{3}\n{4}", _start.ToString(),
                _restaurant, _driver, _attendance, _notes);
        }
    }
}
