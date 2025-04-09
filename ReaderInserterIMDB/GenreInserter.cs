using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderInserterIMDB
{
    public class GenreInserter
    {
        private SqlCommand sqlCommInsertGenre;

        public void InsertGenres(SqlConnection sqlConn, List<Title> titleList)
        {

            HashSet<string> genres = new HashSet<string>();
            Dictionary<string, int> genreDict = new Dictionary<string, int>();
            foreach (Title title in titleList)
            {
                foreach (var genre in title.genres)
                {
                    genres.Add(genre);
                }
            }
            foreach (string genre in genres)
            {
                sqlCommInsertGenre = new SqlCommand("INSERT INTO [dbo].[Genres] ([Name])" +
                    "OUTPUT INSERTED.ID " +
                    "VALUES (@Name)", sqlConn);

                sqlCommInsertGenre.Parameters.Add(CreateParameter("Name", SqlDbType.VarChar, 100));
                sqlCommInsertGenre.Prepare();

                sqlCommInsertGenre.Parameters["Name"].Value = genre.Replace("'", "''");
                sqlCommInsertGenre.ExecuteScalar();

                SqlDataReader reader = sqlCommInsertGenre.ExecuteReader();
                if (reader.Read())
                {
                    int newId = (int)reader["Id"];
                    genreDict.Add(genre, newId);
                }
                reader.Close();
            }
            foreach (Title myTitle in titleList)
            {
                foreach (string genre in myTitle.genres)
                {
                    sqlCommInsertGenre = new SqlCommand("INSERT INTO [dbo].[Titles_Genres] ([Tconst], [GenreID])" +
                    "VALUES ('" + myTitle.tconst + "', '" + genreDict[genre] + "')", sqlConn);

                    sqlCommInsertGenre.Parameters.Add(CreateParameter("Tconst", SqlDbType.VarChar, 10)); //Vi behøver måske ikke at tjekke for int
                    sqlCommInsertGenre.Parameters.Add(CreateParameter("GenreID", SqlDbType.Int));
                    sqlCommInsertGenre.Prepare();
                    
                    sqlCommInsertGenre.Parameters["Tconst"].Value = myTitle.tconst;
                    sqlCommInsertGenre.Parameters["GenreID"].Value = genreDict[genre];
                    try
                    {
                        sqlCommInsertGenre.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(sqlCommInsertGenre.CommandText, ex);
                    }
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
