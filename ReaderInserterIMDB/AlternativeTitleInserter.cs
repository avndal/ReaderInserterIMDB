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

        public AlternativeTitleInserter()
        {
        }
        public void Insert(List<AlternativeTitle> alternativeTitle, SqlConnection sqlConn)
        {
            DataTable titleTable = new DataTable("Alternative_Titles");

            titleTable.Columns.Add("tconst", typeof(string));
            titleTable.Columns.Add("ordering", typeof(int));
            titleTable.Columns.Add("title", typeof(string));
            titleTable.Columns.Add("region", typeof(string));
            titleTable.Columns.Add("language", typeof(string));
            titleTable.Columns.Add("attributes", typeof(string));
            titleTable.Columns.Add("type", typeof(string));
            titleTable.Columns.Add("isOriginalTitle", typeof(bool));

            foreach (AlternativeTitle altTitle in alternativeTitle)
            {
                DataRow titleRow = titleTable.NewRow();
                FillParameter(titleRow, "tconst", altTitle.Tconst);
                FillParameter(titleRow, "ordering", altTitle.Ordering);
                FillParameter(titleRow, "title", altTitle.Title);
                FillParameter(titleRow, "region", altTitle.Region);
                FillParameter(titleRow, "language", altTitle.Language);
                FillParameter(titleRow, "attributes", altTitle.Attributes);
                FillParameter(titleRow, "type", altTitle.Type);
                FillParameter(titleRow, "isOriginalTitle", altTitle.IsOriginalTitle);
                titleTable.Rows.Add(titleRow);
            }
            SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConn,
                SqlBulkCopyOptions.KeepNulls, null);
            bulkCopy.DestinationTableName = "Alternative_Titles";
            bulkCopy.BulkCopyTimeout = 0;
            bulkCopy.WriteToServer(titleTable);
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
