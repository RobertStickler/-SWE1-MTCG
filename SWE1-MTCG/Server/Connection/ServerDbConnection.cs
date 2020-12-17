using Cards;
using MyEnum;
using MySql.Data.MySqlClient;
using SWE1_MTCG;
using SWE1_MTCG.Cards.Monster;
using SWE1_MTCG.Cards.Zauber;
using System;
using System.Collections.Generic;
using System.Threading;
using Npgsql;


namespace Server
{
    //eine funktioin um die karten vom user herauszufinden
    //eine funktion um die besten 4 karten herauszufinden
    public class ServerDbCOnnection
    {
        private static readonly string Host = "localhost";
        private static readonly string User = "postgres";
        private static readonly string DBname = "swe_mtcg";
        private static readonly string Password = "admin";
        private static readonly string Port = "5432";

        private static NpgsqlConnection databaseConnection;
        private static readonly string connString = String.Format("Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer", Host, User, DBname, Port, Password);



        public ServerDbCOnnection()
        {
            setConnect();
            return;
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

        public DbUser GetOneUser(string userName)
        {
            string query = "SELECT * FROM userdata WHERE UserName = '" + userName + "';";
            //string query = "SELECT * FROM userdata;";
            setConnect();

            NpgsqlCommand commandDatabase = new NpgsqlCommand(query, databaseConnection);
            DbUser userObjekt = new DbUser();
            
            try
            {
                //databaseConnection.Open();
                var myReader = commandDatabase.ExecuteReader();

                if (myReader.HasRows)
                {
                    Console.WriteLine("Query Generated result:");

                    while (myReader.Read())
                    {
                        Console.WriteLine(myReader.GetValue(0) + " - " + myReader.GetString(1) + " - " + myReader.GetString(2) + " - " + myReader.GetString(3) + " - " + myReader.GetValue(4));
                        userObjekt.userName = (string)myReader.GetValue(0);
                        userObjekt.uid = myReader.GetString(1);
                        userObjekt.email = myReader.GetString(2);
                        userObjekt.pwd = myReader.GetString(3);
                        userObjekt.coins = (int)myReader.GetValue(4);
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
            databaseConnection.Close();
            return userObjekt;
        }



        public bool VerifyRegister(string query)
        {
            NpgsqlCommand commandDatabase = new NpgsqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            try
            {
                //databaseConnection.Open();
                var myReader = commandDatabase.ExecuteReader();
                Console.WriteLine("VerifyRegister executed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Query Error: " + e.Message);
                return false;
            }
            databaseConnection.Close();
            return true;
        }


        public List<BaseCards> GetCardsFromDB(string username)
        {
            List<BaseCards> cards = new List<BaseCards>();
            string query = DbFunctions.MakeQueryGetCards(username);
            //Console.WriteLine(query);
            var commandDatabase = new NpgsqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            try
            {
                databaseConnection.Open();
                var myReader = commandDatabase.ExecuteReader();
                //Console.WriteLine(myReader);
                if (myReader.HasRows)
                {
                    Console.WriteLine("Query Generated result:");
                    while (myReader.Read())
                    {
                        BaseCards temp = null;

                        //Console.WriteLine(myReader.GetValue(0) + " - " + myReader.GetString(1) + " - " + myReader.GetString(2) + " - " + myReader.GetValue(3) + " - " + myReader.GetValue(4) + " - " + myReader.GetValue(5));
                        //nur zur übersicht
                        string uid = myReader.GetString(0);
                        elementTypes temp_elementTypes = (elementTypes)Enum.Parse(typeof(elementTypes), myReader.GetString(1));
                        cardTypes temp_cardTypes = (cardTypes)Enum.Parse(typeof(cardTypes), myReader.GetString(2));
                        cardProperty temp_cardProperty = (cardProperty)Enum.Parse(typeof(cardProperty), myReader.GetString(3));
                        string name = myReader.GetString(4);
                        int damage = myReader.GetInt32(5);

                        if (temp_cardTypes == cardTypes.Monster)
                        {
                            temp = new MonsterCard(uid, damage, name, temp_elementTypes, temp_cardProperty);
                        }
                        else if (temp_cardTypes == cardTypes.Spell)
                        {
                            temp = new SpellCard(uid, damage, name, temp_elementTypes);                            
                        }
                        cards.Add(temp);
                    }
                }
                else
                {
                    Console.WriteLine("Query Error!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Query Error: " + e.Message);
            }
            databaseConnection.Close();
            return cards;
        }
        public void GenerateNewCards(Random rand)
        {
            int HowManyCards = 100;

            while(HowManyCards > 0)
            {
                BaseCards baseCard = CardShop.GetRandCard(rand);

                Console.WriteLine(baseCard.getCardName());

                string query = DbFunctions.MakeQueryForCreateNewCard(baseCard);

                if(ExecuteQuery(query) == false)
                {
                    //error weiterreichen
                    Console.WriteLine("error bei execute");
                }
                Thread.Sleep(Server.rand.Next(1, 17));

                HowManyCards--;
            }
        }
        public bool ExecuteQuery (string query)
        {
            NpgsqlCommand commandDatabase = new NpgsqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            try
            {
                databaseConnection.Open();
                var myReader = commandDatabase.ExecuteReader();
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
        public BaseCards GetOneCardFromDb(string query, int cardsNumber)
        {
            
            BaseCards temp = null;
            NpgsqlCommand commandDatabase = new NpgsqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;


            int counter = 0;
            try
            {
                databaseConnection.Open();
                var myReader = commandDatabase.ExecuteReader();
                //Console.WriteLine(myReader);
                if (myReader.HasRows)
                {
                    Console.WriteLine("Query Generated result:");
                    int cardPlace = Server.rand.Next(0, cardsNumber);
                    while (myReader.Read())
                    {

                        //Console.WriteLine(myReader.GetValue(0) + " - " + myReader.GetString(1) + " - " + myReader.GetString(2) + " - " + myReader.GetValue(3) + " - " + myReader.GetValue(4) + " - " + myReader.GetValue(5));
                        //nur zur übersicht
                        string cardUID = myReader.GetString(0);
                        elementTypes temp_elementTypes = (elementTypes)Enum.Parse(typeof(elementTypes), myReader.GetString(1));
                        cardTypes temp_cardTypes = (cardTypes)Enum.Parse(typeof(cardTypes), myReader.GetString(2));
                        cardProperty temp_cardProperty = (cardProperty)Enum.Parse(typeof(cardProperty), myReader.GetString(3));
                        string name = myReader.GetString(4);
                        int damage = myReader.GetInt32(5);

                        if (temp_cardTypes == cardTypes.Monster)
                        {
                            temp = new MonsterCard(cardUID, damage, name, temp_elementTypes, temp_cardProperty);
                        }
                        else if (temp_cardTypes == cardTypes.Spell)
                        {
                            temp = new SpellCard(cardUID, damage, name, temp_elementTypes);
                        }
                        counter++;

                        if (counter == cardPlace)
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Query Error!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Query Error: " + e.Message);
            }
            databaseConnection.Close();
            return temp;
        }
        public int GetCardsCountFromDb()
        {
            string query = "SELECT * From cardcollection;";
            NpgsqlCommand commandDatabase = new NpgsqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            int counter = 0;
            try
            {
                databaseConnection.Open();
                var myReader = commandDatabase.ExecuteReader();
                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        counter++;
                    }
                }
                else
                {
                    Console.WriteLine("Query Error!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Query Error: " + e.Message);
            }
            databaseConnection.Close();
            return counter;
        }
        public void GetCardToUser(BaseCards baseCard, DbUser user)
        {
            string query = DbFunctions.MakeQueryForInsertCard(baseCard, user);
            if (ExecuteQuery(query) == false)
            {
                //error weiterreichen
                Console.WriteLine("error bei execute");
            }
        }
    }
}