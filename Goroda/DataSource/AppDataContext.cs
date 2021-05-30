using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Configuration;

namespace Goroda.DataSource
{
    public class AppDataContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Street> Streets { get; set; }
        public AppDataContext():base(ConfigurationManager.ConnectionStrings["BD"].ConnectionString)
        {
        }
    }
}
