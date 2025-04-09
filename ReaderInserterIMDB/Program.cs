using Microsoft.Data.SqlClient;
using ReaderInserterIMDB;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;



string ConnString = "server=localhost;Database=IMDB;" +
               "Integrated security=True;TrustServerCertificate=True";

DateTime before = DateTime.Now;

SqlConnection sqlConn = new SqlConnection(ConnString);
sqlConn.Open();

TitleInserter titleInserter = new TitleInserter();
GenreInserter genreInserter = new GenreInserter();


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
            titles.Add(new Title(
                values[0], values[1], values[2], values[3],
                ConvertToBool(values[4]), ConvertToInt(values[5]),
                ConvertToInt(values[6]), ConvertToInt(values[7]),
                values[8]
                ));
        }
    }
    Console.WriteLine($"{i} / 115");
    titleInserter.Insert(titles, sqlConn);
    //Console.WriteLine("Adds, genres");
    //genreInserter.InsertGenres(sqlConn, titles);
}

PersonInserter? personInserter = new PersonInserter();
ProfessionInserter professionInserter = new ProfessionInserter();
for (int i = 0; i < 150; i++)
{
    List<Person> persons = new List<Person>();
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
                ConvertToInt(values[3]), values[4], knownForTitles));
        }
    }
    Console.WriteLine($"Adds {persons.Count}, people");
    personInserter.Insert(persons, sqlConn);
    //Console.WriteLine("Adds, professions");
    //professionInserter.Insert(sqlConn, persons);
}

DirectorWriterInserter directorWriterInserter = new DirectorWriterInserter();
for (int i = 0; i < 115; i++)
{
    List<Writer> writers = new List<Writer>();
    List<Director> directors = new List<Director>();
    foreach (string line in
    System.IO.File.ReadLines
    (@"C:/temp/title.crew.tsv")
    .Skip(1 + (100000 * i)).Take(100000))
    {
        string[] values = line.Split("\t");
        if (values.Length == 3)
        {
            writers.Add(new Writer(values[0], values[2]));

            directors.Add(new Director(values[0], values[1]));
        }
    }
    Console.WriteLine($"{i} / 581");

    directorWriterInserter.Insert(directors, writers, sqlConn);
}

//AlternativeTitleInserter altInserter = new AlternativeTitleInserter();

//for (int i = 0; i < 5810*2; i++)
/*for (int i = 0; i < 10; i++)
{
    List<AlternativeTitle> altTitles = new List<AlternativeTitle>();
    foreach (string line in
    System.IO.File.ReadLines
    (@"C:/temp/title.akas.tsv")
    .Skip(1 + (5000 * i)).Take(5000))
    {
        string[] values = line.Split("\t");
        if (values.Length == 8)
        {
            List<string> knownForTitles = new List<string>();
            altTitles.Add(new AlternativeTitle(values[0], int.Parse(values[1]), values[2], values[3], values[4],
                values[5], values[6], ConvertToBool(values[7])));
        }
    }
    Console.WriteLine($"{i} / 581");

    altInserter.Insert(altTitles, sqlConn);
}*/

sqlConn.Close();

DateTime after = DateTime.Now;
Console.WriteLine("Tid: " + (after - before));




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