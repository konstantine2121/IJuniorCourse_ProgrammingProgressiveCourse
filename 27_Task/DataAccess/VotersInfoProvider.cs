using System;
using System.Collections.Generic;
using System.Data;
using _27_Task.Model.Data;

namespace _27_Task.DataAccess
{
    using Columns = VotersTable.Columns;

    public class VotersInfoProvider
    {
        private readonly IDbManager _dbManager;

        public VotersInfoProvider(IDbManager dbManager)
        {
            _dbManager = dbManager;
        }

        public List<VoterInfo> FindInfo(string passportHash)
        {
            var infos = new List<VoterInfo>();

            var table = _dbManager.FindRecords(passportHash);
            
            if (table is null || table.Rows.Count < 1)
            {
                return infos;
            }

            foreach (DataRow row in table.Rows) 
            {
                infos.Add(TranslateRecord(row));
            }

            return infos;
        }

        private VoterInfo TranslateRecord(DataRow dataRow)
        {
            var name = dataRow.Field<string>(Columns.Name);
            var passportHash = dataRow.Field<string>(Columns.PassportHash);
            var canVote = Convert.ToBoolean(dataRow.Field<long>(Columns.CanVote));

            return new VoterInfo(passportHash, name, canVote);
        }
    }
}
