using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ReaderInserterIMDB
{
    public class AlternativeTitleInserter
    {
        private SqlCommand sqlCommInsertAlternativeTitle;

        public AlternativeTitleInserter(SqlConnection sqlConn, SqlTransaction myTrans)
        {
            /*sqlCommInsertAlternativeTitle = new SqlCommand("INSERT INTO [dbo].[Alternative_Titles] ([Tconst], " +
                "[Ordering], [Title], [Region], [Language], [Attributes], [Type], [IsOriginalTitle])" +
            "VALUES (@Tconst,@Ordering,@Title,@Region,@Language,@Attributes,@Type,@IsOriginalTitle)", sqlConn, myTrans);

            sqlCommInsertAlternativeTitle.Parameters.Add(CreateParameter("Tconst", SqlDbType.VarChar, 10));
            sqlCommInsertAlternativeTitle.Parameters.Add(CreateParameter("Ordering", SqlDbType.Int));
            sqlCommInsertAlternativeTitle.Parameters.Add(CreateParameter("Title", SqlDbType.VarChar, 100));
            sqlCommInsertAlternativeTitle.Parameters.Add(CreateParameter("Region", SqlDbType.Char, 2));
            sqlCommInsertAlternativeTitle.Parameters.Add(CreateParameter("Language", SqlDbType.Char, 2));
            sqlCommInsertAlternativeTitle.Parameters.Add(CreateParameter("Attributes", SqlDbType.VarChar, 100));
            sqlCommInsertAlternativeTitle.Parameters.Add(CreateParameter("Type", SqlDbType.VarChar, 100));
            sqlCommInsertAlternativeTitle.Parameters.Add(CreateParameter("IsOriginalTitle", SqlDbType.Bit));
            sqlCommInsertAlternativeTitle.Prepare();*/

        }
        public void Insert(AlternativeTitle alternativeTitle)
        {
            DataTable titleTable = new DataTable("Titles");

            titleTable.Columns.Add("tconst", typeof(string));
            titleTable.Columns.Add("titleType", typeof(string));
            titleTable.Columns.Add("primaryTitle", typeof(string));
            titleTable.Columns.Add("originalTitle", typeof(string));
            titleTable.Columns.Add("isAdult", typeof(bool));
            titleTable.Columns.Add("startYear", typeof(int));
            titleTable.Columns.Add("endYear", typeof(int));
            titleTable.Columns.Add("runtimeMinutes", typeof(int));

            /*sqlCommInsertAlternativeTitle.Parameters["Tconst"].Value = alternativeTitle.Tconst;
            sqlCommInsertAlternativeTitle.Parameters["Ordering"].Value = CheckIntNull(alternativeTitle.Ordering);
            sqlCommInsertAlternativeTitle.Parameters["Title"].Value = alternativeTitle.Title;
            sqlCommInsertAlternativeTitle.Parameters["Region"].Value = alternativeTitle.Region;
            sqlCommInsertAlternativeTitle.Parameters["Language"].Value = alternativeTitle.Language;
            sqlCommInsertAlternativeTitle.Parameters["Attributes"].Value = alternativeTitle.Attributes;
            sqlCommInsertAlternativeTitle.Parameters["Type"].Value = alternativeTitle.Type;
            sqlCommInsertAlternativeTitle.Parameters["IsOriginalTitle"].Value = CheckBoolNull(alternativeTitle.IsOriginalTitle);
            sqlCommInsertAlternativeTitle.ExecuteNonQuery();*/
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
