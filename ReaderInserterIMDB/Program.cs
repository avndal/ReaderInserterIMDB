using Microsoft.Data.SqlClient;
using ReaderInserterIMDB;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

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
        TitleInserter insertTitle = new TitleInserter(sqlConn, myTrans);
        //foreach (string line in File.ReadLines("C:/temp/title.basics.tsv").Skip(1).Take(117057))
        //foreach (string line in File.ReadLines("C:/temp/title.basics.tsv").Skip(1))
        foreach (string line in File.ReadLines("C:/temp/title.basics.tsv").Skip(1).Take(10))
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
                SqlCommand sqlComm = new SqlCommand("EXEC [dbo].[TitleTypeGetInsertID] @NewTitleType = '"
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
                    titleList.Add(newTitle);
                }
                else
                {
                    Console.WriteLine("Kunne ikke få id for titletype: " + titleType);
                    reader.Close();
                }
                GenreInserter inserter = new GenreInserter();
                //inserter.InsertGenres(sqlConn, titleList, myTrans);
            }
        }
        /*PersonInserter insertPerson = new PersonInserter(sqlConn, myTrans);
        foreach (string line in File.ReadLines("C:/temp/name.basics.tsv").Skip(1).Take(10))
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
                List<String> knownForTitles = splitLine[5].Replace("'", "''").Split(",").ToList();

                Person newPerson = new Person()
                {
                    Nconst = Nconst,
                    PrimaryName = primaryName,
                    BirthYear = BirthYear,
                    DeathYear = DeathYear,
                    Professions = professions,
                    KnownForTitles = knownForTitles
                };
                insertPerson.Insert(newPerson, sqlConn, myTrans);
                personList.Add(newPerson);
            }
            ProfessionInserter professionInserter = new ProfessionInserter();
            //professionInserter.Insert(sqlConn, personList, myTrans);
        }*/
        /*foreach (string line in File.ReadLines("C:/temp/title.crew.tsv").Skip(1).Take(10))
        {
            string[] splitLine = line.Split("\t");
            if (splitLine.Length != 3)
            {
                Console.WriteLine("Linjen har ikke 6 kolonner: " + line);
            }
            else
            {
                string Tconst = splitLine[0].Replace("'", "''");
                List<String> directors = splitLine[1].Replace("'", "''").Split(",").ToList();
                List<String> writers = splitLine[2].Replace("'", "''").Split(",").ToList();

                foreach (string director in directors)
                {
                    SqlCommand sqlCommInsertDirector = new SqlCommand("INSERT INTO [dbo].[Directors] ([Nconst], [Tconst])" +
                "VALUES ('" + Tconst + "', '" + director + "')", sqlConn, myTrans);
                    sqlCommInsertDirector.ExecuteNonQuery();
                }
                foreach (string writer in writers)
                {
                    SqlCommand sqlCommInsertWriter = new SqlCommand("INSERT INTO [dbo].[Writers] ([Nconst], [Tconst])" +
                "VALUES ('" + Tconst + "', '" + writer + "')", sqlConn, myTrans);
                    sqlCommInsertWriter.ExecuteNonQuery();
                }
            }
        }*/
        AlternativeTitleInserter altInserter = new AlternativeTitleInserter(sqlConn, myTrans);

        foreach (string line in File.ReadLines("C:/temp/title.akas.tsv").Skip(1).Take(100))
        {
            string[] splitLine = line.Split("\t");
            if (splitLine.Length != 8)
            {
                Console.WriteLine("Linjen har ikke 6 kolonner: " + line);
            }
            else
            {
                string Tconst = splitLine[0].Replace("'", "''");
                string ordering = CheckInt(splitLine[1]);
                string title = splitLine[2].Replace("'", "''");
                string Region = splitLine[3].Replace("'", "''");
                string Language = splitLine[4].Replace("'", "''");
                string Attributes = splitLine[5].Replace("'", "''");
                string Type = splitLine[6].Replace("'", "''");
                string IsOriginalTitle = CheckBit(splitLine[7]);

                AlternativeTitle newTitle = new AlternativeTitle()
                {
                    Tconst = Tconst,
                    Ordering = ordering,
                    Title = title,
                    Region = Region,
                    Language = Language,
                    Attributes = Attributes,
                    Type = Type,
                    IsOriginalTitle = IsOriginalTitle
                };
                altInserter.Insert(newTitle);
            }
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