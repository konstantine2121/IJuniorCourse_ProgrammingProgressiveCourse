using System;
using System.Collections.Generic;
using System.Data;
using _27_Task.Model;

namespace _27_Task.DataAccess
{
    using Columns = VotersTable.Columns;

    public class VotersInfoProvider
    {
        private readonly DbManager _dbManager;

        public VotersInfoProvider(DbManager dbManager)
        {
            _dbManager = dbManager;
        }

        public List<VoterInfoDto> FindInfo(string passportHash)
        {
            var infos = new List<VoterInfoDto>();

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

        private VoterInfoDto TranslateRecord(DataRow dataRow)
        {
            var name = dataRow.Field<string>(Columns.Name);
            var passportHash = dataRow.Field<string>(Columns.PassportHash);
            var canVote = Convert.ToBoolean(dataRow.Field<long>(Columns.CanVote));

            return new VoterInfoDto(passportHash, name, canVote);
        }
    }
}
