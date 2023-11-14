using System.Data;

namespace _27_Task.DataAccess
{
    public interface IDbManager
    {
        bool CheckDbFileExists();

        void CreateDatabase();

        void FillVotersInfo();

        DataTable FindRecords(string passportHash);
    }
}
