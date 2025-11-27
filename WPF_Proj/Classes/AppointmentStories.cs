using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Proj.Classes
{
    internal class AppointmentStories
    {
        [Key]
        public int Id { get; set; }
        public int Patient_id { get; set; }
        public int Doctor_id { get; set; }
        public DateTime Date { get; set; }
        public string Diagnosis { get; set; }
        public string Recomendations { get; set; }

        public AppointmentStories() { }
    }
}
