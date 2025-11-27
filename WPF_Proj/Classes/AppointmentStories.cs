using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPF_Proj.Classes
{
    public class AppointmentStories
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("Patient_id")]
        public int Patient_id { get; set; }

        [Required]
        [Column("Doctor_id")]
        public int Doctor_id { get; set; }

        [Required]
        [Column("Date")]
        public DateTime Date { get; set; }

        [Required]
        [Column("Diagnosis")]
        public string Diagnosis { get; set; }

        [Column("Recomendations")]
        public string Recomendations { get; set; }
    }
}