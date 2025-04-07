using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderInserterIMDB
{
    public class ProfessionInserter
    {
        private SqlCommand sqlCommInsertGenre;

        public void Insert(SqlConnection sqlConn, List<Person> personList, SqlTransaction myTrans)
        {

            HashSet<string> professions = new HashSet<string>();
            Dictionary<string, int> professionDict = new Dictionary<string, int>();
            foreach (Person person in personList)
            {
                foreach (var profession in person.Professions)
                {
                    professions.Add(profession);
                }
            }
            foreach (string profession in professions)
            {
                sqlCommInsertGenre = new SqlCommand("INSERT INTO [dbo].[Professions] ([Professions])" +
                    "OUTPUT INSERTED.ID " +
                    "VALUES (@Professions)", sqlConn, myTrans);

                sqlCommInsertGenre.Parameters.Add(CreateParameter("Professions", SqlDbType.VarChar, 100));
                sqlCommInsertGenre.Prepare();

                sqlCommInsertGenre.Parameters["Professions"].Value = profession;
                sqlCommInsertGenre.ExecuteScalar();

                SqlDataReader reader = sqlCommInsertGenre.ExecuteReader();
                if (reader.Read())
                {
                    int newId = (int)reader["Id"];
                    professionDict.Add(profession, newId);
                }
                reader.Close();
            }
            foreach (Person myPerson in personList)
            {
                foreach (string profession in myPerson.Professions)
                {
                    sqlCommInsertGenre = new SqlCommand("INSERT INTO [dbo].[Persons_Professions] ([Nconst], [ProfessionID])" +
                    "VALUES (@Nconst, @ProfessionID)", sqlConn, myTrans);

                    sqlCommInsertGenre.Parameters.Add(CreateParameter("Nconst", SqlDbType.VarChar, 10));
                    sqlCommInsertGenre.Parameters.Add(CreateParameter("ProfessionID", SqlDbType.Int));
                    sqlCommInsertGenre.Prepare();

                    sqlCommInsertGenre.Parameters["Nconst"].Value = myPerson.Nconst;
                    sqlCommInsertGenre.Parameters["ProfessionID"].Value = professionDict[profession];
                    sqlCommInsertGenre.ExecuteNonQuery();
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
    }
}
