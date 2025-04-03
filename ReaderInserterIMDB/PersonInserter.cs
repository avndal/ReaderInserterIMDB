using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ReaderInserterIMDB
{
    public class PersonInserter : PreparedInserter<Person>
    {
        private SqlCommand sqlCommInsertPerson;

        public PersonInserter(SqlConnection sqlConn, SqlTransaction myTrans) : base(sqlConn, myTrans)  
        {
            sqlConn = sqlConn;
            myTrans = myTrans;

            sqlCommInsertPerson = new SqlCommand("INSERT INTO [dbo].[Persons]" +
                       "([Nconst],[PrimaryName],[BirthYear]" +
                       ",[DeathYear])" + "VALUES (@Nconst, @PrimaryName, @BirthYear, @DeathYear)", sqlConn, myTrans);

            sqlCommInsertPerson.Parameters.Add(CreateParameter("Nconst", SqlDbType.VarChar, 9));
            sqlCommInsertPerson.Parameters.Add(CreateParameter("PrimaryName", SqlDbType.VarChar, 100));
            sqlCommInsertPerson.Parameters.Add(CreateParameter("BirthYear", SqlDbType.SmallInt));
            sqlCommInsertPerson.Parameters.Add(CreateParameter("DeathYear", SqlDbType.SmallInt));
            sqlCommInsertPerson.Prepare();
        }

        public void Insert(Person newPerson)
        {
            sqlCommInsertPerson.Parameters["Nconst"].Value = newPerson.Nconst;
            sqlCommInsertPerson.Parameters["PrimaryName"].Value = newPerson.PrimaryName;
            sqlCommInsertPerson.Parameters["BirthYear"].Value = CheckIntNull(newPerson.BirthYear);
            sqlCommInsertPerson.Parameters["DeathYear"].Value = CheckIntNull(newPerson.DeathYear);
            sqlCommInsertPerson.ExecuteNonQuery();
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

        public static Object CheckBoolNull(string input)
        {
            if (input == "NULL")
            {
                return DBNull.Value;
            }
            if (input == "1")
            {
                return true;
            }
            else if (input == "0")
            {
                return false;
            }
            throw new Exception("Ingen bool");
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
