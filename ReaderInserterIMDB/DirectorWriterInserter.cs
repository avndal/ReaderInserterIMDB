using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderInserterIMDB
{
    public class DirectorWriterInserter
    {
        public DirectorWriterInserter()
        {

        }
        public void Insert(List<Director> directors, List<Writer> writers, SqlConnection sqlConn)
        {
            DataTable directorTable = new DataTable("Directors");
            DataTable writerTable = new DataTable("Writers");

            directorTable.Columns.Add("nconst", typeof(string));
            directorTable.Columns.Add("tconst", typeof(string));
            writerTable.Columns.Add("nconst", typeof(string));
            writerTable.Columns.Add("tconst", typeof(string));

            foreach (Director director in directors)
            {
                foreach (string direct in director.Directors)
                {
                    if (CheckNull(direct) != null) {
                        DataRow directorRow = directorTable.NewRow();
                        FillParameter(directorRow, "nconst", direct);
                        FillParameter(directorRow, "tconst", director.Tconst);
                        directorTable.Rows.Add(directorRow);
                    }
                }
            }
            foreach (Writer writer in writers)
            {
                foreach (string write in writer.Writers)
                {
                    if (CheckNull(write) != null)
                    {
                        DataRow writerRow = writerTable.NewRow();
                        FillParameter(writerRow, "nconst", write);
                        FillParameter(writerRow, "tconst", writer.Tconst);
                        writerTable.Rows.Add(writerRow);
                    }
                }
            }
            SqlBulkCopy directorBulk = new SqlBulkCopy(sqlConn,
                SqlBulkCopyOptions.KeepNulls, null);
            directorBulk.DestinationTableName = "Directors";
            directorBulk.BulkCopyTimeout = 0;
            directorBulk.WriteToServer(directorTable);

            SqlBulkCopy writerBulk = new SqlBulkCopy(sqlConn,
                SqlBulkCopyOptions.KeepNulls, null);
            writerBulk.DestinationTableName = "Writers";
            writerBulk.BulkCopyTimeout = 0;
            writerBulk.WriteToServer(directorTable);
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
        string? CheckNull(string input)
        {
            if (input.ToLower() == "\\n")
            {
                return null;
            }
            return input;
        }
    }
}
