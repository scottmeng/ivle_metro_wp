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
    public class SemInfo
    {
        public string AcademicYear { get; set; }
        public string Semester { get; set; }

        public SemInfo(string ay, string sem)
        {
            AcademicYear = ay;
            Semester = sem;
        }
    }
}
