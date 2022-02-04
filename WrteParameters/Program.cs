using Station.Device;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WrteParameters
{
    class Program
    {
        static void Main(string[] args)
        {

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ChannelCollection));

            TextReader reader = new StreamReader("Data/channels.xml");

            ChannelCollection channels = (ChannelCollection)xmlSerializer.Deserialize(reader); ;

            OdbcConnection connection = new OdbcConnection();



            connection.ConnectionString = "Driver={SQL Server};Server=localhost;Database=ecostation;User=sa;Password=sa";

            connection.Open();

            int i = 0;
            foreach (Channel c in channels.Collection)
            {

                string query = "INSERT INTO  [dbo].[PARAMETERS] (CODE,DATATYPE, DISCRETE,ORDERING,PARAMNAME,PARAMSIGN,SHORTNAME,GROUP_ID, UNITMEASURE_ID,ACTIVE,PRECISION) " +
                       "VALUES(?,?,?,?,?,?,?,?,?,?,?)";
                

                try
                {

                    OdbcCommand command = new OdbcCommand(query, connection);

                    command.Parameters.AddWithValue("@code", c.Parameter.Code);
                    command.Parameters.AddWithValue("@type", 0);
                    command.Parameters.AddWithValue("@discrete", c.Parameter.Flag);
                    command.Parameters.AddWithValue("@order", i++);
                    command.Parameters.AddWithValue("@paramname", c.Parameter.Name);
                    command.Parameters.AddWithValue("@paramsign", c.Parameter.Sign);
                    command.Parameters.AddWithValue("@shortname", c.Parameter.Sign);
                    command.Parameters.AddWithValue("@group", 1);
                    command.Parameters.AddWithValue("@unit", 1);
                    command.Parameters.AddWithValue("@active", true);
                    command.Parameters.AddWithValue("@prec", c.Parameter.Precesion);

                    command.ExecuteNonQuery();

                 
                }
                catch (Exception e) {

                    Console.WriteLine(e.Message);
                }


            }



        }
    }
}
