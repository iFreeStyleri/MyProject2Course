using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Goroda.DataSource
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PopulationCount { get; set; }
        public Country Country { get; set; }
    }
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PopulationCount { get; set; }
        public string President { get; set; }
    }

    public class CityView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PopulationCount { get; set; }
        public string CountryName { get; set; }
    }
    public class Street
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public City City { get; set; }

    }
}
