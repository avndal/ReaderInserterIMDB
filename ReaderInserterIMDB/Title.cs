using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderInserterIMDB
{
    public class Title
    {
        public string tconst { get; set; }
        public string titleTypeId { get; set; }
        public string primaryTitle { get; set; }
        public string originalTitle { get; set; }
        public bool isAdult { get; set; }
        public int? startYear { get; set; }
        public int? endYear { get; set; }
        public int? runtimeMinutes { get; set; }
        public List<string> genres { get; set; }

        public Title(string tconst, string titleType,
           string primaryTitle, string originalTitle,
           bool isAdult, int? startYear, int? endYear,
           int? runtimeMinutes, string genresString)
        {
            this.tconst = tconst;
            this.titleTypeId = titleType;
            this.primaryTitle = primaryTitle;
            this.originalTitle = originalTitle;
            this.isAdult = isAdult;
            this.startYear = startYear;
            this.endYear = endYear;
            this.runtimeMinutes = runtimeMinutes;

            genres = genresString.Split(",").ToList();
        }
    }
}
