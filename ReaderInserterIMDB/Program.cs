using Microsoft.Data.SqlClient;
using ReaderInserterIMDB;
using System.Diagnostics;

using (SqlConnection sqlConn = new SqlConnection("server=localhost;Database=IMDB;" +
               "Integrated security=True;TrustServerCertificate=True"))
{
    sqlConn.Open();
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();

    SqlTransaction myTrans = sqlConn.BeginTransaction();
    int counter = 0;

    try
    {
        List<Title> titleList = new List<Title>();
        List<Person> personList = new List<Person>();
        //IInserter inserter = new NormalInserter(sqlConn, myTrans);
        IInserter<Title> insertTitle = new Inserter<Title>(sqlConn, myTrans);
        foreach (string line in File.ReadLines("C:/temp/title.basics.tsv").Skip(1).Take(10000))
        {
            counter++;
            string[] splitLine = line.Split("\t");
            if (splitLine.Length != 9)
            {
                Console.WriteLine("Linjen har ikke 9 kolonner: " + line);
            }
            else
            {
                string titleType = splitLine[1];
                SqlCommand sqlComm = new SqlCommand("EXEC [dbo].[TitleTypeGetInsertID] @NewTiteType = '"
                    + titleType + "'", sqlConn, myTrans);
                SqlDataReader reader = sqlComm.ExecuteReader();
                if (reader.Read())
                {
                    string titleTypeId = reader["Id"].ToString();
                    //Console.WriteLine("Indlæst Titletypeid: " + titleTypeId);
                    reader.Close();

                    string tconst = splitLine[0];
                    string primaryTitle = splitLine[2].Replace("'", "''");
                    string originalTitle = splitLine[3].Replace("'", "''");
                    string isAdult = CheckBit(splitLine[4]);
                    string startYear = CheckInt(splitLine[5]);
                    string endYear = CheckInt(splitLine[6]);
                    string runtimeMinutes = CheckInt(splitLine[7]);
      //              string genre = splitLine[8].Replace("'", "''");
                    List<String> genres = splitLine[8].Replace("'", "''").Split(",").ToList();

                    Title newTitle = new Title()
                    {
                        tconst = tconst,
                        titleTypeId = titleTypeId,
                        primaryTitle = primaryTitle,
                        originalTitle = originalTitle,
                        isAdult = isAdult,
                        startYear = startYear,
                        endYear = endYear,
                        runtimeMinutes = runtimeMinutes,
                        genres = genres
                    };
                    insertTitle.Insert(newTitle);
                    titleList.Add(newTitle);  //Måske kan man gemme i title_genre uden en list af titles??
                }
                else
                {
                    Console.WriteLine("Kunne ikke få id for titletype: " + titleType);
                    reader.Close();
                }
                GenreInserter inserter = new GenreInserter();
                inserter.InsertGenres(sqlConn, titleList, myTrans);
            }
        }
        IInserter<Person> insertPerson = new PersonInserter(sqlConn, myTrans);
        foreach (string line in File.ReadLines("C:/temp/name.basics.tsv").Skip(1).Take(10000))
        {
            counter++;
            string[] splitLine = line.Split("\t");
            if (splitLine.Length != 6)
            {
                Console.WriteLine("Linjen har ikke 6 kolonner: " + line);
            }
            else
            {
                //Console.WriteLine("Indlæst Titletypeid: " + titleTypeId);

                string Nconst = splitLine[0];
                string primaryName = splitLine[1].Replace("'", "''");
                string BirthYear = CheckInt(splitLine[2]);
                string DeathYear = CheckInt(splitLine[3]);
                List<String> professions = splitLine[4].Replace("'", "''").Split(",").ToList();

                Person newPerson = new Person()
                {
                    Nconst = Nconst,
                    PrimaryName = primaryName,
                    BirthYear = BirthYear,
                    DeathYear = DeathYear,
                    Professions = professions
                };
                insertPerson.Insert(newPerson);
                personList.Add(newPerson);
            }
            ProfessionInserter inserter = new ProfessionInserter();
            inserter.InsertGenres(sqlConn, personList, myTrans);
        }
    }
    catch (SqlException ex)
    {
        Console.WriteLine("linje nummer: " + counter + "\r\n" + ex.ToString());
    }
    stopwatch.Stop();
    Console.WriteLine("Elapsed milli: " + stopwatch.ElapsedMilliseconds);

    myTrans.Rollback();
}

string CheckBit(string bit)
{
    if (bit == "1" || bit == "0")
    {
        return bit;
    }
    else if (bit.ToLower() == "\\n")
    {
        return "NULL";
    }
    throw new Exception("Hvordan fanden ser dit bit ud!!!");
}

string CheckInt(string input)
{
    if (int.TryParse(input, out int result))
    {
        return input;
    }
    else if (input.ToLower() == "\\n")
    {
        return "NULL";
    }
    throw new Exception("Hvordan fanden ser din int ud!!!");
}