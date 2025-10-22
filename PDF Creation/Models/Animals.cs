using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareApp.Models
{
    public class Animal
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public DateTime BirthDate { get; set; }
        public string Owner { get; set; }
        public string PassNr { get; set; }
    }
}