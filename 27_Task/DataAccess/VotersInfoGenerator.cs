﻿using System.Collections.Generic;
using _27_Task.Model;
using _27_Task.Utils;

namespace _27_Task.DataAccess
{
    public class VotersInfoGenerator
    {
        private HashCalculator _hashCalculator = HashCalculator.CreateSha256Calculator();

        public IEnumerable<VoterInfoDto> GenerateInfos() 
        {
            yield return new VoterInfoDto(Hash("0000000000"),"И.И.И.", true);
            yield return new VoterInfoDto(Hash("1111111111"),"C.C.C.", true);
            yield return new VoterInfoDto(Hash("2222222222"),"П.П.П.", false);
            yield return new VoterInfoDto(Hash("3333333333"),"К.К.К.", true);
            yield return new VoterInfoDto(Hash("4444444444"),"А.А.А.", false);
        }

        private string Hash(string value) => 
            _hashCalculator.CalculateHash(value);
    }
}
