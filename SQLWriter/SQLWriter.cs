using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Station.Device;
using System.Collections;
using System.Data.Odbc;

namespace SQLWriter
{
    [Serializable()]
    public class SQLWriter : IDataWriter
    {
        private Hashtable _parameters;

        private string _connectionString;
        private volatile bool _busy;
        private volatile bool _first;



        private OdbcConnection _connection;

        public SQLWriter()
        {

            _parameters = new Hashtable();

        }


       

        public bool IsBusy
        {
            get
            {
                return _busy;
            }

            set
            {
                _busy = value;
            }
        }


        public void AddParameter(string key, object value)
        {
            _parameters.Add(key, value);  
        }

        public void WriteData(int id, int period,DateTime time, bool emergency, IEnumerable<IChannel> channels)
        {

            if (_connection != null)
            {
                lock (_connection)
                {

                    try
                    {


                        UpdateProbeInfo(id, period, time, _connection);

                        foreach (IChannel c in channels)
                        {
                            if(_first )
                                UpdateColumn(c, _connection);

                            UpdateChannelData(id,c, _connection);
                        }


                        _first = false;


                    }
                    catch (Exception e)
                    {

                        throw new Exception($"SQLWriter: {e.Message}", e);
                    }
                }
            }
           
           
        }

        private static void UpdateProbeInfo(int stationID, int period, DateTime time, OdbcConnection connection)
        {
            OdbcCommand command = new OdbcCommand("UPDATE  DATA_WORK  SET [DateTime] = ?,  [LoopMain]=? where STATIONID=?", connection);

            command.Parameters.AddWithValue("@dateTime", time);
            command.Parameters.AddWithValue("@period", period);
            command.Parameters.AddWithValue("@station", stationID);



            command.ExecuteNonQuery();
        }

        private static void UpdateColumn(IChannel channel, OdbcConnection connection)
        {

            var query = "IF NOT EXISTS ( SELECT * FROM INFORMATION_SCHEMA.COLUMNS "+
                " WHERE table_name = 'Data_WORK' " +
                $" AND column_name = '{channel.Parameter.Sign}' )"+
                "  BEGIN"+       
                $" ALTER TABLE Data_WORK Add[{channel.Parameter.Sign}]  REAL"+
                $" ALTER TABLE Data_WORK Add[{channel.Parameter.Sign}_CODE]  varchar(50) " +
                " END";

            OdbcCommand command = new OdbcCommand(query, connection);

           
            command.ExecuteNonQuery();
        }




        private static void UpdateChannelData(int stationID,IChannel c, OdbcConnection connection)
        {

            if (c.CurrentValue != null)
            {
                OdbcCommand updateValueCommand = new OdbcCommand($"UPDATE  DATA_WORK  SET [{c.Parameter.Sign}] = ? WHERE STATIONID= ?", connection);

                updateValueCommand.Parameters.AddWithValue("@value", c.CurrentValue);
                updateValueCommand.Parameters.AddWithValue("@station", stationID);


                updateValueCommand.ExecuteNonQuery();
            }
            else
            {
                OdbcCommand updateValueCommand = new OdbcCommand($"UPDATE  DATA_WORK  SET [{c.Parameter.Sign}] = NULL WHERE STATIONID= ?", connection);
                updateValueCommand.Parameters.AddWithValue("@station", stationID);


                updateValueCommand.ExecuteNonQuery();
            }



            OdbcCommand updateCodeCommand = new OdbcCommand($"UPDATE  DATA_WORK  SET [{c.Parameter.Sign}_CODE] = ? WHERE STATIONID= ?", connection);

            updateCodeCommand.Parameters.AddWithValue("@statuscode", c.Status.Code);
            updateCodeCommand.Parameters.AddWithValue("@station", stationID);


            updateCodeCommand.ExecuteNonQuery();


        }


        private string BuildConnectionString()
        {
            _connectionString = String.Empty;

            try
            {
                _connectionString = AppendParameter("Driver");
                _connectionString = AppendParameter("Server");
                _connectionString = AppendParameter("Database");
                _connectionString = AppendParameter("User");
                _connectionString = AppendParameter("Password");
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);

            }

            return _connectionString;

        }

        private string AppendParameter(string parameter)
        {
            StringBuilder builder = new StringBuilder(_connectionString);

            if (_parameters.ContainsKey(parameter))
            {
                builder.Append($"{parameter}={_parameters[parameter]};");
            }
            else
            {
                throw new Exception($"Не задан параметр {parameter}!");
            }

            return builder.ToString();
        }

        public void OpenWriter(int stationId)
        {
            _busy = true;
            _first = true;

            try
            {

                 _connection = new OdbcConnection();

                _connection.ConnectionString = BuildConnectionString();

                _connection.Open();

                

            }
            catch (Exception e)
            {

                throw new Exception($"SQLWriter: {e.Message}", e);
            }

            _busy = false;
        }

        public void CloseWriter()
        {
            _connection.Close();
        }
    }
}
