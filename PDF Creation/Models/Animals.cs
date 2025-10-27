using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDF_Creation.Models
{
    public class Animal
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public DateTime BirthDate { get; set; }
        public double Weight { get; set; }
        public string Owner { get; set; }
        public string PassNr { get; set; }

        public string Impfungen { get; set; }
        public string Symptome { get; set; }    
    }
}