using System.Collections.Generic;
using System.Data;

namespace CVA.AddOn.Common.DAO
{
    public interface IDAO
    {
        object Model { get; set; }
        string TableName { get; set; }

        void BeginTransaction();
        void Close();
        void CommitTransaction();
        void CreateModel();
        void DeleteModel();
        void ExecuteNonQuery(string command);
        object ExecuteScalar(string command);
        bool Exists(string where);
        DataTable FillDataTable(string command);
        List<T> FillListFromCommand<T>(string command);
        T FillModel<T>(string command);
        T FillModelFromCommand<T>(string command);
        List<T> FillModelList<T>(string command);
        List<string> FillStringList(string command);
        string GetConnectedServer();
        string GetNextCode(string tableName);
        string GetNextCode(string tableName, string fieldName);
        string GetNextCode(string tableName, string fieldName, string where);
        int GetRowCount(string command);
        bool HasRows(string command);
        T RetrieveModel<T>(string where = "", string orderBy = "");
        List<T> RetrieveModelList<T>(string where = "", string orderBy = "");
        void RollbackTransaction();
        void UpdateModel();
    }
}
