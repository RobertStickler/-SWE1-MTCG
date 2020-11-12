using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;

namespace Server
{
    public class MySqlDataClass
    {
        public void runQuery(string queryToRun)
        {
            string mySQLConnectionString = "datasource=127.0.0.1;port=3306; username=root;password=;database=swe_mtcg;";
            MySqlConnection databaseConnection = new MySqlConnection(mySQLConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(queryToRun, databaseConnection);
            commandDatabase.CommandTimeout = 60;

            try
            {
                databaseConnection.Open();
                MySqlDataReader myReader = commandDatabase.ExecuteReader();

                if (myReader.HasRows)
                {
                    Console.WriteLine("Query Generated result:");

                    while (myReader.Read())
                    {
                        //Name                        //id
                        Console.WriteLine(myReader.GetString(0) + " - " + myReader.GetString(1));
                    }
                }
                else
                {
                    Console.WriteLine("Query Successfully executed!");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Query Error: " + e.Message);
            }
        }
    }
}

