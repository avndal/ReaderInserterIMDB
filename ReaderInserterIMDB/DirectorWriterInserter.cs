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
            /*DataTable directorTable = new DataTable("Directors");
            DataTable writerTable = new DataTable("Writers");

            directorTable.Columns.Add("nconst", typeof(string));
            directorTable.Columns.Add("tconst", typeof(string));
            writerTable.Columns.Add("nconst", typeof(string));
            writerTable.Columns.Add("tconst", typeof(string));

            
                if (director.Directors != null)
                {
                    foreach (string? newDirector in ei.Directors)
                    {
                        DataRow directorRow = directorTable.NewRow();
                        FillParameter(directorRow, "nconst", director.Directors);
                        FillParameter(directorRow, "tconst", director.Tconst);
                        directorTable.Rows.Add(directorRow);
                    }
                }
                if (writers.Writers != null)
                {
                    foreach (string? writer in writers.Writers)
                    {
                        DataRow writerRow = writerTable.NewRow();
                        FillParameter(writerRow, "nconst", writer);
                        FillParameter(writerRow, "tconst", writers.Tconst);
                        writerTable.Rows.Add(writerRow);
                    }
                }
            if (writers.Writers != null)
            {
                foreach (string? writer in writers.Writers)
                {
                    DataRow writerRow = writerTable.NewRow();
                    FillParameter(writerRow, "nconst", writer);
                    FillParameter(writerRow, "tconst", writers.Tconst);
                    writerTable.Rows.Add(writerRow);
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
            writerBulk.WriteToServer(directorTable);*/
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
    }
}
