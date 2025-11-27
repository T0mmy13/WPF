using System;
using System.Linq;

namespace WPF_Proj.Classes
{
    public static class AppointmentHelper
    {
        public static DateTime? GetLastAppointmentDate(int patientId)
        {
            try
            {
                using (var db = new DBContext())
                {
                    return db.AppointmentStories
                        .Where(a => a.Patient_id == patientId)
                        .OrderByDescending(a => a.Date)
                        .Select(a => a.Date)
                        .FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
        }

        public static int GetDaysSinceLastAppointment(int patientId)
        {
            var lastDate = GetLastAppointmentDate(patientId);
            if (lastDate == null) return -1;

            return (DateTime.Today - lastDate.Value).Days;
        }
    }
}