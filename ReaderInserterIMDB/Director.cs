using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderInserterIMDB
{
    public class Director
    {
        public string Tconst { get; set; }
        public List<string> Directors { get; set; }

        public Director(string tconst, List<string> directors)
        {
            Tconst = tconst;
            Directors = directors;
        }
    }
}
