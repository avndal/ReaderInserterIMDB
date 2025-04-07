using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderInserterIMDB
{
    public class Person
    {
        public string Nconst { get; set; }
        public string PrimaryName { get; set; }
        public string BirthYear { get; set; }
        public string DeathYear { get; set; }
        public List<string> Professions { get; set; }
        public List<string> KnownForTitles { get; set; }

    }
}
