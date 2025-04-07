using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderInserterIMDB
{
    public class DirectorWriter
    {
        public string Tconst { get; set; }
        public List<string?> Direcotors { get; set; }
        public List<string?> Writers { get; set; }
        //        public List<string> Professions { get; set; }

        public DirectorWriter(string tconst, List<string?> directors, List<string?>writers)
        {
            Tconst = tconst;
            Direcotors = directors;
            Writers = writers;
        }
    }
}
