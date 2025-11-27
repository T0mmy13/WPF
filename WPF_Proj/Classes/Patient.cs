using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPF_Proj.Classes
{
    public class Patient : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _lastName;
        private string _middleName;
        private DateTime _birthday;
        private string _phoneNumber;
        private DateTime? _lastAppointmentDate;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        [Required]
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        [Required]
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        [Required]
        public string MiddleName
        {
            get => _middleName;
            set { _middleName = value; OnPropertyChanged(); }
        }

        [Required]
        public DateTime Birthday
        {
            get => _birthday;
            set { _birthday = value; OnPropertyChanged(); OnPropertyChanged(nameof(Age)); OnPropertyChanged(nameof(IsAdult)); }
        }

        [Required]
        public string PhoneNumber
        {
            get => _phoneNumber;
            set { _phoneNumber = value; OnPropertyChanged(); }
        }

        [NotMapped]
        public DateTime? LastAppointmentDate
        {
            get => _lastAppointmentDate;
            set { _lastAppointmentDate = value; OnPropertyChanged(); }
        }

        [NotMapped]
        public int Age
        {
            get
            {
                if (Birthday == default) return 0;
                var today = DateTime.Today;
                var age = today.Year - Birthday.Year;
                if (Birthday.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        [NotMapped]
        public bool IsAdult => Age >= 18;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public Patient()
        {
            Birthday = DateTime.Now.AddYears(-30);
        }
    }
}