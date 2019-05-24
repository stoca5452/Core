using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stoca.Log
{
    public interface ILog
    {
       
        /// <summary>
        /// Escribe un log de error como mensaje
        /// </summary>
        /// <param name="message"></param>
        void Error(object message);

        /// <summary>
        /// Escribe un mensaje de error para una excepcion
        /// </summary>
        /// <param name="message">Texto a informar</param>
        /// <param name="exception">Excepcion</param>
        void Error(object message, Exception exception);

        /// <summary>
        /// Escribe un mensaje de error para una excepcion con objeto command
        /// </summary>
        /// <param name="message">Mensaje</param>
        /// <param name="exception">Excepcion</param>
        /// <param name="command">command</param>
        void Error(object message, Exception exception, System.Data.IDbCommand command);
        
    }
}
