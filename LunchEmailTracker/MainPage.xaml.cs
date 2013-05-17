using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using LunchEmailTracker.Resources;
using Microsoft.Phone.Tasks;

namespace LunchEmailTracker
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;

            BuildLocalizedApplicationBar();
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            blockStartTime.DataContext = this.CurrentLunch;
            boxRestaurant.DataContext = this.CurrentLunch;
            boxWhoAttended.DataContext = this.CurrentLunch;
            boxWhoDrove.DataContext = this.CurrentLunch;
            boxNotes.DataContext = this.CurrentLunch;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            State["CurrentLunch"] = _currentLunch;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Initialize the page state only if it is not already initialized,
            // and not when the application was deactivated but not tombstoned 
            // (returning from being dormant).
            if (DataContext == null)
            {
                if (State.ContainsKey("CurrentLunch"))
                {
                    _currentLunch = (LunchTime)State["CurrentLunch"];
                    MessageBox.Show("Restored old state " + _currentLunch.Restaurant);
                }
            }

            // Delete temporary storage to avoid unnecessary storage costs.
            State.Clear();
        }

        private LunchTime _currentLunch;
        public LunchTime CurrentLunch
        {
            get
            {
                if (_currentLunch == null)
                    _currentLunch = new LunchTime();
                return _currentLunch;
            }
        }
        
        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            this.CurrentLunch.Start = DateTime.Now;
        }

        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarSendButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.check.png", UriKind.Relative));
            appBarSendButton.Text = AppResources.AppBarSendButtonText;
            appBarSendButton.Click += appBarSendButton_Click;
            ApplicationBar.Buttons.Add(appBarSendButton);

            
            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem appBarResetMenuItem = new ApplicationBarMenuItem(AppResources.AppBarResetMenuItemText);
            appBarResetMenuItem.Click += appBarResetMenuItem_Click;
            ApplicationBar.MenuItems.Add(appBarResetMenuItem);
            
        }

        void appBarResetMenuItem_Click(object sender, EventArgs e)
        {
            Reset();
        }

        void Reset()
        {
            this.CurrentLunch.Restaurant = "";
            this.CurrentLunch.Driver = "";
            this.CurrentLunch.Attendance = "";
            this.CurrentLunch.Notes = "";
        }

        void appBarSendButton_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(this.this.CurrentLunch.ToString());

            this.CurrentLunch.End = DateTime.Now;
            int minuteDuration = (this.CurrentLunch.End - this.CurrentLunch.Start).Minutes;

            string formatString = @"Lunch - {0}

Duration: {1}
Start: {2}
Finish: {3}

Location: {4}
Driver(s): {5}
Attendee(s): {6}

Notes:
{7}
";
            string body = string.Format(formatString, this.CurrentLunch.End.ToShortDateString(),
                minuteDuration.ToString(),
                this.CurrentLunch.Start.ToString(),
                this.CurrentLunch.End.ToString(),
                this.CurrentLunch.Restaurant,
                this.CurrentLunch.Driver,
                this.CurrentLunch.Attendance,
                this.CurrentLunch.Notes);

            EmailComposeTask ect = new EmailComposeTask();
            ect.Subject = string.Format("Lunch - {0}", this.CurrentLunch.End.ToShortDateString());
            ect.Body = body;

            ect.Show();

            Reset();
        }

        private void boxRestaurant_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                boxWhoDrove.Focus();
            }
        }

        private void boxWhoDrove_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                boxWhoAttended.Focus();
            }
        }

        private void boxWhoAttended_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                boxNotes.Focus();
            }
        }
    }
}