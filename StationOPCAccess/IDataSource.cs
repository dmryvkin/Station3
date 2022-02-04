using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StationOPCAccess
{
    public interface IDataSource
    {
        void Open();

        void Close();

        void BeginAccess();

        void EndAccess();


        double? GetValue(string name);

        string GetCode(string name);

        DateTime GetDateTime();


    }
}
