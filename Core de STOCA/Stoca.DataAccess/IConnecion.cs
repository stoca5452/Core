using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stoca.DataAccess
{
    public interface IConnecion : IDisposable
    {
        #region Miembros de IConnection
        void Open();
        /// <summary>
        /// Retorna la conexión a la base de datos
        /// </summary>
//        IDbConnection GetConnection();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDbCommand CreateCommand();

        IDbCommand CreateCommand(CommandType commandType, string commandText);

        System.Data.DataSet GetDataSet(IDbCommand command);

        System.Data.DataSet GetDataSet(string demo, IDbCommand command);

        int ExecuteNoQuery(System.Data.IDbCommand command);
        
        #endregion Miembros de IConnection

    }
}
