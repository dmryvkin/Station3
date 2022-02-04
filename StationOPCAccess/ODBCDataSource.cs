using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StationOPCAccess
{
    public class ODBCDataSource : IDataSource
    {
        private OdbcConnection _connection;
        OdbcDataReader _reader;

        int _stationID;
       

        public ODBCDataSource(int stationId, string connectionString) {

            _connection = new OdbcConnection(connectionString);
            _stationID = stationId;

        }

        public void Open()
        {
            try
            {

                _connection.Open();
            }
            catch (Exception e)
            {

                throw e;
            }


        }

        public void BeginAccess()
        {
            string queryString = $"SELECT * FROM DATA_Work WHERE STATIONID={_stationID}";

            OdbcCommand command = new OdbcCommand(queryString, _connection);

            try
            {
                _reader = command.ExecuteReader();
                _reader.Read();

            }
            catch (Exception e) {

                 throw new Exception($"BeginAccess: {e.Message}", e); ;
            }


        }

        public void Close()
        {
            _connection.Close();
        }

        public void EndAccess()
        {
            if (_reader != null)
                _reader.Close();
        }

        public string GetCode(string name)
        {
            try
            {
                return (string)_reader[name + "_CODE"];
            }
            catch (Exception e)
            {

                throw new Exception($"GetValue: {name} {e.Message}", e);
            }

        }

        public DateTime GetDateTime()
        {
            try
            {
                return _reader.GetDateTime(0);
               
            }
            catch (Exception e)
            {

                throw new Exception("GetDateTime: "+ e.Message, e );
            }


        }

        public double? GetValue(string name)
        {
            try
            {
                if (!(_reader[name] is DBNull))

                    return Convert.ToDouble(_reader[name]);
                else
                    return null;

            }
            catch (Exception e)
            {

                throw new Exception($"GetValue {name} {_reader[name].ToString()} {_reader[name].GetType().ToString()} {e.Message}", e);
            }


        }


    }
}
