﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderInserterIMDB
{
    public class TitleInserter : Inserter<Title>
    {
        private SqlCommand sqlCommInsertTitle;

        public TitleInserter(SqlConnection sqlConn, SqlTransaction myTrans): base(sqlConn, myTrans)
        {
            sqlCommInsertTitle = new SqlCommand("INSERT INTO [dbo].[Titles]" +
                       "([Tconst],[TitleTypeId],[PrimaryTitle]" +
                       ",[OriginalTitle],[IsAdult],[StartYear],[EndYear],[RuntimeMinutes]) " +
                       "VALUES (@tconst,@titleTypeId,@primaryTitle,@originalTitle," +
                       "@isAdult,@startYear,@endYear,@runtimeMinutes)", sqlConn, myTrans);

            sqlCommInsertTitle.Parameters.Add(CreateParameter("tconst", SqlDbType.VarChar, 10));
            sqlCommInsertTitle.Parameters.Add(CreateParameter("TitleTypeId", SqlDbType.Int));
            sqlCommInsertTitle.Parameters.Add(CreateParameter("PrimaryTitle", SqlDbType.VarChar, -1));
            sqlCommInsertTitle.Parameters.Add(CreateParameter("originalTitle", SqlDbType.VarChar, -1));
            sqlCommInsertTitle.Parameters.Add(CreateParameter("IsAdult", SqlDbType.Bit));
            sqlCommInsertTitle.Parameters.Add(CreateParameter("StartYear", SqlDbType.SmallInt));
            sqlCommInsertTitle.Parameters.Add(CreateParameter("EndYear", SqlDbType.SmallInt));
            sqlCommInsertTitle.Parameters.Add(CreateParameter("RuntimeMinutes", SqlDbType.SmallInt));
            sqlCommInsertTitle.Prepare();
        }
        public void Insert(Title newTitle)
        {
            sqlCommInsertTitle.Parameters["tconst"].Value = newTitle.tconst;
            sqlCommInsertTitle.Parameters["TitleTypeId"].Value = newTitle.titleTypeId;
            sqlCommInsertTitle.Parameters["PrimaryTitle"].Value = newTitle.primaryTitle;
            sqlCommInsertTitle.Parameters["originalTitle"].Value = newTitle.originalTitle;
            sqlCommInsertTitle.Parameters["IsAdult"].Value = CheckBoolNull(newTitle.isAdult);
            sqlCommInsertTitle.Parameters["StartYear"].Value = CheckIntNull(newTitle.startYear);
            sqlCommInsertTitle.Parameters["EndYear"].Value = CheckIntNull(newTitle.endYear);
            sqlCommInsertTitle.Parameters["RuntimeMinutes"].Value = CheckIntNull(newTitle.runtimeMinutes);
            sqlCommInsertTitle.ExecuteNonQuery();
        }
    }
}
