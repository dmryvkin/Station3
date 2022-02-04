using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StationOPCAccess
{
    public class MockDataSource :IDataSource
    {
        private Random _rand;


        public MockDataSource() {

            _rand = new Random();

        }

        public void BeginAccess()
        {
            
        }

        public void Close()
        {
         }

        public void EndAccess()
        {
            
        }

        public string GetCode(string name)
        {
            return "M00";
        }

        public DateTime GetDateTime()
        {
            return DateTime.Now;
        }

        public double? GetValue(string name)
        {
            return _rand.NextDouble();
        }

        public void Open()
        {
        }

        public void Read()
        {
        }
    }
}
