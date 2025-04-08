using Microsoft.Data.SqlClient;
using ReaderInserterIMDB;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;



string ConnString = "server=localhost;Database=IMDB;" +
               "Integrated security=True;TrustServerCertificate=True";

List<List<Title>> titlesList = new List<List<Title>>();
List<Person> persons = new List<Person>();
List<Person> persons2 = new List<Person>();
List<Director> directorList = new List<Director>();
List<Writer> writerList = new List<Writer>();

for (int i = 0; i < 115; i++)
{
    List<Title> titles = new List<Title>();
    foreach (string line in
        System.IO.File.ReadLines
        (@"C:/temp/title.basics.tsv")
        .Skip(1 + (i * 100000)).Take(100000))
    {
        string[] values = line.Split("\t");
        if (values.Length == 9)
        {
            /*string titleType = values[1];
            SqlCommand sqlComm = new SqlCommand("EXEC [dbo].[TitleTypeGetInsertID] @NewTitleType = '"
                        + titleType + "'");*/

            titles.Add(new Title(
                values[0], values[1], values[2], values[3],
                ConvertToBool(values[4]), ConvertToInt(values[5]),
                ConvertToInt(values[6]), ConvertToInt(values[7]),
                values[8]
                ));
        }
    }
    titlesList.Add(titles);
}
List<Title> titlesRest = new List<Title>();
foreach (string line in
        System.IO.File.ReadLines
        (@"C:/temp/title.basics.tsv")
        .Skip(1 + (115 * 100000)))
{
    string[] values = line.Split("\t");
    if (values.Length == 9)
    {
        /*string titleType = values[1];
        SqlCommand sqlComm = new SqlCommand("EXEC [dbo].[TitleTypeGetInsertID] @NewTitleType = '"
                    + titleType + "'");*/

        titlesRest.Add(new Title(
            values[0], values[1], values[2], values[3],
            ConvertToBool(values[4]), ConvertToInt(values[5]),
            ConvertToInt(values[6]), ConvertToInt(values[7]),
            values[8]
            ));
    }
}
titlesList.Add(titlesRest);


for (int i = 0; i < 490; i++)
{
    foreach (string line in
    System.IO.File.ReadLines
    (@"C:/temp/name.basics.tsv")
    .Skip(1 + (100000 * i)).Take(100000))
    {
        string[] values = line.Split("\t");
        if (values.Length == 6)
        {
            List<string> knownForTitles = new List<string>();
            foreach (string knownFor in values[5].Split(","))
            {
                knownForTitles.Add(knownFor);
            }
            persons.Add(new Person(values[0], values[1], ConvertToInt(values[2]),
                ConvertToInt(values[3]), knownForTitles));
        }
    }
}

foreach (string line in
    System.IO.File.ReadLines
    (@"C:/temp/name.basics.tsv")
    .Skip(1 + 490 * 100000))
{
    string[] values = line.Split("\t");
    if (values.Length == 6)
    {
        List<string> knownForTitles = new List<string>();
        foreach (string knownFor in values[5].Split(","))
        {
            knownForTitles.Add(knownFor);
        }
        persons2.Add(new Person(values[0], values[1], ConvertToInt(values[2]),
            ConvertToInt(values[3]), knownForTitles));
    }
}

/*foreach (string line in
    System.IO.File.ReadLines
    (@"C:/temp/title.crew.tsv")
    .Skip(1).Take(1))
{
    string[] values = line.Split("\t");
    if (values.Length == 3)
    {
        List<string?> directors = new List<string?>();
        List<string?> writers = new List<string?>();

        if (values[1] != "\\N")
        {
            foreach (string? director in values[1].Split(","))
            {
                directors.Add(director);
            }
        }
        if (values[2] != "\\N")
        {
            foreach (string? writer in values[2].Split(","))
            { 
                writers.Add(CheckNull(writer));
            }
        }

        directorList.Add(new Director(values[0], directors));
        writerList.Add(new Writer(values[0], writers));
    }
}*/


DateTime before = DateTime.Now;

SqlConnection sqlConn = new SqlConnection(ConnString);
sqlConn.Open();

TitleInserter titleInserter = new TitleInserter();
foreach (List<Title> titles in titlesList)
{
    Console.WriteLine($"Adds {titles.Count}, titles");
    titleInserter.Insert(titles, sqlConn);
}


Console.WriteLine($"Adds {persons.Count}, people");
PersonInserter? personInserter = new PersonInserter();
personInserter.Insert(persons, sqlConn);

Console.WriteLine($"Adds {persons2.Count}, people");
personInserter.Insert(persons2, sqlConn);

/*Console.WriteLine($"Adds {directorList.Count}, DirectorsWriters");
DirectorWriterInserter directorWriterInserter = new DirectorWriterInserter();
directorWriterInserter.Insert(directorList, writerList, sqlConn);*/

sqlConn.Close();

DateTime after = DateTime.Now;
Console.WriteLine("Tid: " + (after - before));











/*List<Title> titles = new List<Title>();

foreach (string line in
    System.IO.File.ReadLines
    (@"C:/temp/title.basics.tsv")
    .Skip(1).Take(10000))
{
    string[] values = line.Split("\t");
    if (values.Length == 9)
    {
        titles.Add(new Title(
            values[0], values[1], values[2], values[3],
            ConvertToBool(values[4]), ConvertToInt(values[5]),
            ConvertToInt(values[6]), ConvertToInt(values[7]),
            values[8]
            ));
    }
}

Console.WriteLine(titles.Count);

DateTime before = DateTime.Now;

SqlConnection sqlConn = new SqlConnection(ConnString);
sqlConn.Open();

TitleInserter? titleInserter = new TitleInserter();

titleInserter.Insert(titles, sqlConn);


sqlConn.Close();

DateTime after = DateTime.Now;

Console.WriteLine("Tid: " + (after - before));*/






/*using (SqlConnection sqlConn = new SqlConnection("server=localhost;Database=IMDB;" +
               "Integrated security=True;TrustServerCertificate=True"))
{
    sqlConn.Open();
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();

    SqlTransaction myTrans = sqlConn.BeginTransaction();
    int counter = 0;

    try
    {
        List<Title> titles = new List<Title>();

        foreach (string line in
            System.IO.File.ReadLines
            (@"C:\temp\title.basics.tsv\data.tsv")
            .Skip(1).Take(10000))
        {
            string[] values = line.Split("\t");
            if (values.Length == 9)
            {
                titles.Add(new Title(
                    values[0], values[1], values[2], values[3],
                    ConvertToBool(values[4]), ConvertToInt(values[5]),
                    ConvertToInt(values[6]), ConvertToInt(values[7]),
                    values[8]
                    ));
            }
        }
        TitleInserter titleInserter = new TitleInserter();

        titleInserter.Insert(titles, sqlConn, myTrans);
        /*List<Title> titleList = new List<Title>();
        List<Person> personList = new List<Person>();
        
        //IInserter inserter = new NormalInserter(sqlConn, myTrans);
        TitleInserter insertTitle = new TitleInserter(sqlConn, myTrans);
        //foreach (string line in File.ReadLines("C:/temp/title.basics.tsv").Skip(1).Take(117057))
        foreach (string line in File.ReadLines("C:/temp/title.basics.tsv").Skip(1))
        //foreach (string line in File.ReadLines("C:/temp/title.basics.tsv").Skip(1).Take(10))
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
                inserter.InsertGenres(sqlConn, titleList, myTrans);
            }
        }
        PersonInserter insertPerson = new PersonInserter(sqlConn, myTrans);
        foreach (string line in File.ReadLines("C:/temp/name.basics.tsv").Skip(1))
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
            professionInserter.Insert(sqlConn, personList, myTrans);
        }
        foreach (string line in File.ReadLines("C:/temp/title.crew.tsv").Skip(1))
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
        }
        AlternativeTitleInserter altInserter = new AlternativeTitleInserter(sqlConn, myTrans);

        foreach (string line in File.ReadLines("C:/temp/title.akas.tsv").Skip(1))
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
//        myTrans.Rollback();
    }
    stopwatch.Stop();
    Console.WriteLine("Elapsed milli: " + stopwatch.ElapsedMilliseconds);

    myTrans.Rollback();
}*/

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
string? CheckNull(string input)
{
    if (input.ToLower() == "\\n")
    {
        return null;
    }
    return input;
}
bool ConvertToBool(string input)
{
    if (input == "0")
    {
        return false;
    }
    else if (input == "1")
    {
        return true;
    }
    throw new ArgumentException(
        "Kolonne er ikke 0 eller 1, men " + input);
}

int? ConvertToInt(string input)
{
    if (input.ToLower() == @"\n")
    {
        return null;
    }
    else
    {
        return int.Parse(input);
    }

}