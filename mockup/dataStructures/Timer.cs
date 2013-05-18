using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace mockup
{
    public class Timer
    {
        private static System.Timers.Timer aTimer;

        public static void Main()
        {
            // Create a timer with a ten second interval.
            aTimer = new System.Timers.Timer(10000);

            // Hook up the Elapsed event for the timer.
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            // Set the Interval to 2 seconds (2000 milliseconds).
            aTimer.Interval = 2000;
            aTimer.Enabled = true;
        }

        // Specify what you want to happen when the Elapsed event is 
        // raised.
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //updateModules();
        }
    }
}
