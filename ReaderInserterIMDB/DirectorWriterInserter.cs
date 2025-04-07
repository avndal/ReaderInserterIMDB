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
        public void Insert(List<DirectorWriter> directorWriters, SqlConnection sqlConn)
        {
            DataTable directorTable = new DataTable("Directors");
            DataTable writerTable = new DataTable("Writers");

            directorTable.Columns.Add("nconst", typeof(string));
            directorTable.Columns.Add("tconst", typeof(string));
            writerTable.Columns.Add("nconst", typeof(string));
            writerTable.Columns.Add("tconst", typeof(string));

            foreach (DirectorWriter directorWriter in directorWriters)
            {
                foreach (string? director in directorWriter.Direcotors)
                {
                    DataRow directorRow = directorTable.NewRow();
                    FillParameter(directorRow, "nconst", directorWriter.Direcotors);
                    FillParameter(directorRow, "tconst", directorWriter.Tconst);
                    directorTable.Rows.Add(directorRow);
                }
                if (directorWriter.Writers != null)
                {
                    foreach (string? writer in directorWriter.Writers)
                    {
                        DataRow writerRow = writerTable.NewRow();
                        FillParameter(writerRow, "nconst", directorWriter.Writers);
                        FillParameter(writerRow, "tconst", directorWriter.Tconst);
                        writerTable.Rows.Add(writerRow);
                    }
                }
                else
                {
                    DataRow writerRow = writerTable.NewRow();
                    FillParameter(writerRow, "nconst", DBNull.Value);
                    FillParameter(writerRow, "tconst", directorWriter.Tconst);
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
    }
}
