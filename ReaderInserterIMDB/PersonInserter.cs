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
        private SqlCommand sqlCommInsertPerson;

        public PersonInserter(SqlConnection sqlConn, SqlTransaction myTrans)
        {
            sqlCommInsertPerson = new SqlCommand("INSERT INTO [dbo].[Persons]" +
                       "([Nconst],[PrimaryName],[BirthYear]" +
                       ",[DeathYear])" + "VALUES (@Nconst, @PrimaryName, @BirthYear, @DeathYear)", sqlConn, myTrans);

            sqlCommInsertPerson.Parameters.Add(CreateParameter("Nconst", SqlDbType.VarChar, 9));
            sqlCommInsertPerson.Parameters.Add(CreateParameter("PrimaryName", SqlDbType.VarChar, 100));
            sqlCommInsertPerson.Parameters.Add(CreateParameter("BirthYear", SqlDbType.SmallInt));
            sqlCommInsertPerson.Parameters.Add(CreateParameter("DeathYear", SqlDbType.SmallInt));
            sqlCommInsertPerson.Prepare();
        }

        public void Insert(Person newPerson, SqlConnection sqlConn, SqlTransaction myTrans)
        {
            sqlCommInsertPerson.Parameters["Nconst"].Value = newPerson.Nconst;
            sqlCommInsertPerson.Parameters["PrimaryName"].Value = newPerson.PrimaryName;
            sqlCommInsertPerson.Parameters["BirthYear"].Value = CheckIntNull(newPerson.BirthYear);
            sqlCommInsertPerson.Parameters["DeathYear"].Value = CheckIntNull(newPerson.DeathYear);

            sqlCommInsertPerson.ExecuteNonQuery();

            foreach (string title in newPerson.KnownForTitles)
            {
                SqlCommand sqlCommInsertKnownFor = new SqlCommand("INSERT INTO [dbo].[Persons_Titles] ([Nconst], [Tconst])" +
                "VALUES ('" + newPerson.Nconst + "', '" + title + "')", sqlConn, myTrans);

                try
                {
                    sqlCommInsertKnownFor.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(sqlCommInsertKnownFor.CommandText, ex);
                }
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
