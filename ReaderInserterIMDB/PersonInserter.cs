using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ReaderInserterIMDB
{
    public class PersonInserter
    {
        //private SqlCommand sqlCommInsertPerson;

        public PersonInserter()
        {

        }

        public void Insert(List<Person> persons, SqlConnection sqlConn)
        {
            DataTable personTable = new DataTable("Persons");
            DataTable knownForTable = new DataTable("Persons_Titles");

            personTable.Columns.Add("Nconst", typeof(string));
            personTable.Columns.Add("PrimaryName", typeof(string));
            personTable.Columns.Add("BirthYear", typeof(int));
            personTable.Columns.Add("DeathYear", typeof(int));
            knownForTable.Columns.Add("Nconst", typeof(string));
            knownForTable.Columns.Add("Tconst", typeof(string));

            foreach (Person person in persons)
            {
                DataRow personRow = personTable.NewRow();
                FillParameter(personRow, "Nconst", person.Nconst);
                FillParameter(personRow, "PrimaryName", person.PrimaryName);
                FillParameter(personRow, "BirthYear", person.BirthYear);
                FillParameter(personRow, "DeathYear", person.DeathYear);
                personTable.Rows.Add(personRow);
                foreach (string knownFor in person.KnownForTitles)
                {
                    DataRow knownForRow = knownForTable.NewRow();
                    FillParameter(knownForRow, "Nconst", person.Nconst);
                    FillParameter(knownForRow, "Tconst", knownFor);
                    knownForTable.Rows.Add(knownForRow);
                }
            }
            SqlBulkCopy bulkPerson = new SqlBulkCopy(sqlConn,
                SqlBulkCopyOptions.KeepNulls, null);
            bulkPerson.DestinationTableName = "Persons";
            bulkPerson.BulkCopyTimeout = 0;
            bulkPerson.WriteToServer(personTable);

            SqlBulkCopy bulkKnownFor = new SqlBulkCopy(sqlConn,
                SqlBulkCopyOptions.KeepNulls, null);
            bulkKnownFor.DestinationTableName = "Persons_Titles";
            bulkKnownFor.BulkCopyTimeout = 0;
            bulkKnownFor.WriteToServer(knownForTable);
        }

        public void FillParameter(DataRow row,
            string columnName,
            object? value)
        {
            if (value != null)
            {
                row[columnName] = value;
            }
            else
            {
                row[columnName] = DBNull.Value;
            }
        }
        public static SqlParameter CreateParameter(string parameterName, SqlDbType type, int? size = null)
        {
            SqlParameter result = new SqlParameter(parameterName, type);
            if (size == null)
            {
                result = new SqlParameter(parameterName, type);
            }
            else
            {
                result = new SqlParameter(parameterName, type, (int)size);
            }
            return result;
        }
        public static Object CheckIntNull(string input)
        {
            if (input == "NULL")
            {
                return DBNull.Value;
            }
            if (int.TryParse(input, out int result))
            {
                return result;
            }
            throw new Exception("Ingen int");
        }
    }
}
