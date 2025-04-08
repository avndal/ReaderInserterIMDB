using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderInserterIMDB
{
    public class Writer
    {
        public string Tconst { get; set; }
        public List<string> Writers { get; set; }

        public Writer(string tconst, List<string> writers)
        {
            Tconst = tconst;
            Writers = writers;
        }
    }
}
