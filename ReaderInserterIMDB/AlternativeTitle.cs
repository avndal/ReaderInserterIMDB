using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderInserterIMDB
{
    public class AlternativeTitle
    {
        public string Tconst { get; set; }
        public int Ordering { get; set; }
        public string Title { get; set; }
        public string Region { get; set; }
        public string Language { get; set; }
        public string Attributes { get; set; }
        public string Type { get; set; }
        public bool IsOriginalTitle { get; set; }

        public AlternativeTitle(string tconst, int ordering, string title, string region, string language, 
            string attributes, string type, bool isOriginalTitle)
        {
            Tconst = tconst;
            Ordering = ordering;
            Title = title;
            Region = region;
            Language = language;
            Attributes = attributes;
            Type = type;
            IsOriginalTitle = isOriginalTitle;
        }
    }
}
