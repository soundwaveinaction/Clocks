using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Clocks
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string currentTime;
        private string userTime = "00:00:00";
        private TimeOnly userTimeOnly = TimeOnly.MinValue;
        private double secondAngle;
        private double minuteAngle;
        private double hourAngle;
        readonly string timeFormat;

        public RelayCommand SetTimeCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                    {
                        userTimeOnly = TimeOnly.FromTimeSpan(TimeOnly.FromDateTime(DateTime.Now) - TimeOnly.FromDateTime(DateTime.Parse(UserTime)));
                    },
                    (obj) => Regex.IsMatch(UserTime, @"^(?:[01]\d|2[0-3]):[0-5]\d:[0-5]\d$")
                    );
            }
        }
        public RelayCommand SetCurrentTimeCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    userTimeOnly = TimeOnly.MinValue;
                },
                    (obj) => userTimeOnly!=TimeOnly.MinValue
                );
            }
        }
        public ViewModel()
        {

            DateTimeFormatInfo dateTimeFormatInfo = new CultureInfo(CultureInfo.CurrentCulture.Name, false).DateTimeFormat;
            timeFormat = dateTimeFormatInfo.LongTimePattern;
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Normal);
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.IsEnabled = true;
            timer.Tick += (s, e) =>
            {
                TimeOnly final = TimeOnly.FromTimeSpan((TimeOnly.FromDateTime(DateTime.Now) - userTimeOnly));
                CurrentTime = final.ToLongTimeString();
                SecondAngle = final.Second * 360 / 60;
                MinuteAngle = final.Minute * 360 / 60;
                HourAngle = final.Hour * 360 / 12;
            };
        }

        public double SecondAngle
        {
            get => secondAngle;
            set
            {
                secondAngle = value;
                OnPropertyChanged(nameof(SecondAngle));
            }
        }
        public double MinuteAngle
        {
            get => minuteAngle;
            set
            {
                minuteAngle = value;
                OnPropertyChanged(nameof(MinuteAngle));
            }
        }
        public double HourAngle
        {
            get => hourAngle;
            set
            {
                hourAngle = value;
                OnPropertyChanged(nameof(HourAngle));
            }
        }
        public string CurrentTime
        {
            get => currentTime;
            set
            {
                currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }
        public string UserTime
        {
            get => userTime;
            set
            {
                userTime = value;
                OnPropertyChanged(nameof(UserTime));
            }
        }

        public string TimeFormat => timeFormat;

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
