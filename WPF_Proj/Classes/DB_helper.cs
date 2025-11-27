using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using Microsoft.EntityFrameworkCore;


namespace WPF_Proj.Classes
{    
    public class DBContext : DbContext
    {
        public DbSet<Doctor> Doctors { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source=localhost\\SQLEXPRESS;Initial Catalog=db_wpf;Integrated Security=True;Trust Server Certificate=True"
            );
        }

    }
}
