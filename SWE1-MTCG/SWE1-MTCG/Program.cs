using System;
using Npgsql;

namespace Driver
{
    public class Program
    {
        // Obtain connection string information from the portal
        //
        private static readonly string Host = "localhost";
        private static readonly string User = "postgres";
        private static readonly string DBname = "swe_mtcg";
        private static readonly string Password = "admin";
        private static readonly string Port = "5432";

        private static NpgsqlConnection databaseConnection;
        private static Program DBObject = new Program();
        private static readonly string connString = String.Format("Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer", Host, User, DBname, Port, Password);


        static void Main(string[] args)
        {
            // Build connection string using parameters from portal
            setConnect();
            string query = "SELECT* FROM userdata";
            bool temp = DBObject.ReadFromDB(query);
        }
        public static void setConnect()
        {
            try
            {
                databaseConnection = new NpgsqlConnection(connString);
                databaseConnection.Open();
                Console.WriteLine("Connection established");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            

        }


        public bool ReadFromDB(string query)
        {
            NpgsqlCommand commandDatabase = new NpgsqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            try
            {
                //databaseConnection.Open();
                var myreader = commandDatabase.ExecuteReader(); //bei select statement
                while (myreader.Read())
                {
                    Console.WriteLine(string.Format("Reading from table=({0}, {1}, {2})", myreader.GetString(0),  myreader.GetString(1),  myreader.GetString(2)));
                }
                myreader.Close();
            
            }
            catch (Exception e)
            {
                Console.WriteLine("Query Error: " + e.Message);
                return false;
            }
            databaseConnection.Close();
            return true;
        }

        public bool ExecuteQuery(string query)
        {
            NpgsqlCommand commandDatabase = new NpgsqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            try
            {
                databaseConnection.Open();
                var myreader = commandDatabase.ExecuteReader(); //bei select statement
                //Console.WriteLine("Query Success");
            }
            catch (Exception e)
            {
                Console.WriteLine("Query Error: " + e.Message);
                return false;
            }
            databaseConnection.Close();
            return true;
        }



        public void InsertError()
        {
            // Insert
            string abc = "ICh bin eine Biene2!";
            var cmd = new NpgsqlCommand("INSERT INTO atable_test (column_test) VALUES (@p)", databaseConnection);
            cmd.Parameters.AddWithValue("p", abc);
            cmd.Prepare();
            cmd.ExecuteNonQuery();

        }

        public void GetError()
        {
            // Retrieve all rows
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM atable_test", databaseConnection);
            cmd.Prepare();
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                Console.WriteLine(reader.GetString(0));
        }
    }
}