using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stoca.DataAccess
{
    /// <summary>
    /// Manejador de acceso a base de datos
    /// </summary>
    public static class Manager
    {
        /// <summary>
        /// Manager de Acceso a datos, se referencias todos los Metodos que acceden a la base de datos y devuleven objetos como Command, DataSet
        /// ExcuteNonQuery, Procedimientos SQL entre otros.
        /// </summary>
        private static Connection m_Connection = new Connection();

        public static Connection DataAccesManager()
        {
            return m_Connection;
        }
    }

    
}
