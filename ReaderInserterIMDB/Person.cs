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
        public int? BirthYear { get; set; }
        public int? DeathYear { get; set; }
//        public List<string> Professions { get; set; }
        public List<string> KnownForTitles { get; set; }

        public Person(string nconst, string primaryName,
           int? birthYear, int? deathYear, List<string> knownForTitles)
        {
            Nconst = nconst;
            PrimaryName = primaryName;
            BirthYear = birthYear;
            DeathYear = deathYear;

            KnownForTitles = knownForTitles;
        }
    }
}
