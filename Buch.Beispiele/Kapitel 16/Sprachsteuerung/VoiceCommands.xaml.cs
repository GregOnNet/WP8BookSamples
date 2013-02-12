using Microsoft.Phone.Controls;
using System;
using System.Globalization;
using System.Windows.Navigation;

namespace Voice
{
    public partial class VoiceCommands : PhoneApplicationPage
    {
        public VoiceCommands()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                if (NavigationContext.QueryString.ContainsKey("voiceCommandName"))
                {
                    string day;
                    string dayParameter = NavigationContext.QueryString["day"].ToLower();
                    DayOfWeek weekDay;

                    switch (dayParameter)
                    {
                        case "morgen":
                            weekDay = DateTime.Now.AddDays(1).DayOfWeek;
                            day = "Morgen";
                            break;
                        case "übermorgen":
                            weekDay = DateTime.Now.AddDays(2).DayOfWeek;
                            day = "Übermorgen";
                            break;
                        default:
                            weekDay = DateTime.Now.DayOfWeek;
                            day = "Heute";
                            break;
                    }

                    string dayName = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(weekDay);
                    string anwer = string.Format("{0} ist {1}", day, dayName);
                    Result.Text = anwer;
                }
            }
        }
    }
}