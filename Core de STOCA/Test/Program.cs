using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stoca.Log;
using System.IO;
using System.Data;
namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            /*string sPath = System.AppDomain.CurrentDomain.RelativeSearchPath;
            if (sPath == null || sPath.Length == 0)
            {
                sPath = System.AppDomain.CurrentDomain.BaseDirectory;
            }*/

            string fecha;
            string sLogFileName;
            sLogFileName = "C:\\LogNet" + "\\" + "App_" + DateTime.Today.ToString("ddMMyyyy") + ".txt";


            //Stoca.DataAccess.Connection c_connection = new Stoca.DataAccess.Connection();
            //Stoca.DataAccess.Manager.DataAccesManager().GetConnection().
            string valor = "david";
            int mensaje;
            string mensaje2 = null;
            System.Data.IDbCommand cmd = null;
            try
            {
                cmd = Stoca.DataAccess.Manager.DataAccesManager().CreateCommand(CommandType.StoredProcedure, "EJEMPLO2");
                Stoca.DataAccess.Manager.DataAccesManager().CreateAndAddParameter(cmd, DbType.String, ParameterDirection.Input, "Parametro1", valor);
                Stoca.DataAccess.Manager.DataAccesManager().CreateAndAddParameter(cmd, DbType.String, ParameterDirection.Input, "Parametro2", 5);
                Stoca.DataAccess.Manager.DataAccesManager().CreateAndAddParameter(cmd, DbType.String, ParameterDirection.Input, "Parametro3", mensaje2);

                //DataSet ds = Stoca.DataAccess.Manager.DataAccesManager().GetDataSet(cmd);
                //Stoca.DataAccess.Manager.DataAccesManager().ExecuteNoquery(cmd);
                //Ejemplo cuando se ejecuta DataReader
                //IDataReader dr = Stoca.DataAccess.Manager.DataAccesManager().ExecuteDataReader(cmd);

                //Ejemplo cuando se ejecutar ExecuteScalar
                mensaje2 = Stoca.DataAccess.Manager.DataAccesManager().ExecuteScalar(cmd).ToString();
                //Stoca.DataAccess.Manager.DataAccesManager().ExecuteNoquery(cmd);

            }
            catch (Exception ex)
            {
                Stoca.Log.LogManager.GetLog().Error("Error en ejecucion", ex, cmd);
                throw ex;
            }
            

            //Stoca.Log.BaseLog c_baselog = new BaseLog();
            
            //CreateAddLogMessage(sLogFileName, "DAVI MONTILLA4");
            
            try
            {
                prueba();
            }
            catch (Exception e)
            {

                Stoca.Log.LogManager.GetLog().Error("Error en ejecucion",e);
                throw e;
            }
            //Stoca.Log.LogManager.GetLog().Error("MENSAJE DE ERROR INICIAL");
            //string sPath = Stoca.Log.BaseLog.GetConfigFilePath();
                        
            Console.WriteLine("listo");
            Console.ReadKey();
        }



        public static void prueba()
        {
            try
            {
                int demo;
                demo = Convert.ToInt32("ff");
            }
            catch (Exception e)
            {

                Stoca.Log.LogManager.GetLog().Error("Error en ejecucion", e);
                throw e;
            
            }
        }
        public static void CreateAddLogMessage(string FullPath, string Message)
        {
            System.IO.StreamWriter MyLog = new System.IO.StreamWriter(FullPath);
            MyLog.WriteLine(Message);
            MyLog.Close();
        }

        public static void AddLogMessage(string FullPath, string Message)
        {
            /* using (System.IO.StreamWriter MyLogs = System.IO.File.AppendText(FullPath))
             {
                 //MyLogs.NewLine;
                 MyLogs.WriteLine(Message);
                 MyLogs.Close();
             }*/

            System.IO.StreamWriter MyLog = new System.IO.StreamWriter(FullPath, true);
            MyLog.WriteLine(Message);
            MyLog.Close();
        }
    }
}
