using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StationOPCAccess
{
    public class Log
    {
        private TextWriter logWriter;


        public Log(string path) {

             logWriter = new StreamWriter(path, true);

        }

        public void Info(string message)
        {
            logWriter.WriteLineAsync($"{DateTime.Now} {message}");
        }

        public void Error(Exception e)
        {
            logWriter.WriteLineAsync($"{DateTime.Now} Error: {e.Message}");
        }

        public void Error(string message)
        {
            logWriter.WriteLineAsync($"{DateTime.Now} Error:{message}");
        }

        internal void Close()
        {
            logWriter.Close();
        }
    }
}
