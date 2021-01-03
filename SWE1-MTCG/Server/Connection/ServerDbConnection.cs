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
    public class ServerDbConnection
    {
        private static readonly string Host = "localhost";
        private static readonly string User = "postgres";
        private static readonly string DBname = "swe_mtcg";
        private static readonly string Password = "admin";
        private static readonly string Port = "5432";

        private static NpgsqlConnection _databaseConnection;
        private static readonly string ConnString = String.Format("Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer", Host, User, DBname, Port, Password);

        public ServerDbConnection()
        {
            SetConnect();
            return;
        }

        public static void SetConnect()
        {
            try
            {
                _databaseConnection = new NpgsqlConnection(ConnString);
                _databaseConnection.Open();
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
            SetConnect();

            NpgsqlCommand commandDatabase = new NpgsqlCommand(query, _databaseConnection);
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
            _databaseConnection.Close();
            return userObjekt;
        }



        public bool VerifyRegister(string query)
        {
            NpgsqlCommand commandDatabase = new NpgsqlCommand(query, _databaseConnection);
            commandDatabase.CommandTimeout = 60;
            try
            {
                _databaseConnection.Open();
                var myReader = commandDatabase.ExecuteReader();
                Console.WriteLine("VerifyRegister executed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Query Error: " + e.Message);
                return false;
            }
            _databaseConnection.Close();
            return true;
        }


        public List<BaseCards> GetCardsFromDb(string username)
        {
            List<BaseCards> cards = new List<BaseCards>();
            string query = DbFunctions.MakeQueryGetCards(username);
            //Console.WriteLine(query);
            var commandDatabase = new NpgsqlCommand(query, _databaseConnection);
            commandDatabase.CommandTimeout = 60;
            try
            {
                _databaseConnection.Open();
                var myReader = commandDatabase.ExecuteReader();
                //Console.WriteLine(myReader);
                if (myReader.HasRows)
                {
                    //Console.WriteLine("Query Generated result:");
                    while (myReader.Read())
                    {
                        BaseCards temp = null;

                        //Console.WriteLine(myReader.GetValue(0) + " - " + myReader.GetString(1) + " - " + myReader.GetString(2) + " - " + myReader.GetValue(3) + " - " + myReader.GetValue(4) + " - " + myReader.GetValue(5));
                        //nur zur übersicht
                        string uid = myReader.GetString(0);
                        elementTypes tempElementTypes = (elementTypes)Enum.Parse(typeof(elementTypes), myReader.GetString(1));
                        cardTypes tempCardTypes = (cardTypes)Enum.Parse(typeof(cardTypes), myReader.GetString(2));
                        cardProperty tempCardProperty = (cardProperty)Enum.Parse(typeof(cardProperty), myReader.GetString(3));
                        string name = myReader.GetString(4);
                        int damage = myReader.GetInt32(5);

                        if (tempCardTypes == cardTypes.Monster)
                        {
                            temp = new MonsterCard(uid, damage, name, tempElementTypes, tempCardProperty);
                        }
                        else if (tempCardTypes == cardTypes.Spell)
                        {
                            temp = new SpellCard(uid, damage, name, tempElementTypes);
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
            _databaseConnection.Close();
            return cards;
        }
        public void GenerateNewCards(Random rand)
        {
            int howManyCards = 100;

            while (howManyCards > 0)
            {
                BaseCards baseCard = CardShop.GetRandCard(rand);

                Console.WriteLine(baseCard.getCardName());

                string query = DbFunctions.MakeQueryForCreateNewCard(baseCard);

                if (ExecuteQuery(query) == false)
                {
                    //error weiterreichen
                    Console.WriteLine("error bei execute");
                }
                Thread.Sleep(Server.rand.Next(1, 17));

                howManyCards--;
            }
        }
        public bool ExecuteQuery(string query)
        {
            NpgsqlCommand commandDatabase = new NpgsqlCommand(query, _databaseConnection);
            commandDatabase.CommandTimeout = 60;
            try
            {
                _databaseConnection.Open();
                var myReader = commandDatabase.ExecuteReader();
                //Console.WriteLine("Query Success");
            }
            catch (Exception e)
            {
                Console.WriteLine("Query Error: " + e.Message);
                return false;
            }
            _databaseConnection.Close();
            return true;
        }
        public BaseCards GetOneRandCardFromDb(string query, int cardsNumber)
        {

            BaseCards temp = null;
            NpgsqlCommand commandDatabase = new NpgsqlCommand(query, _databaseConnection);
            commandDatabase.CommandTimeout = 60;


            int counter = 0;
            try
            {
                _databaseConnection.Open();
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
                        string cardUid = myReader.GetString(0);
                        elementTypes tempElementTypes = (elementTypes)Enum.Parse(typeof(elementTypes), myReader.GetString(1));
                        cardTypes tempCardTypes = (cardTypes)Enum.Parse(typeof(cardTypes), myReader.GetString(2));
                        cardProperty tempCardProperty = (cardProperty)Enum.Parse(typeof(cardProperty), myReader.GetString(3));
                        string name = myReader.GetString(4);
                        int damage = myReader.GetInt32(5);

                        if (tempCardTypes == cardTypes.Monster)
                        {
                            temp = new MonsterCard(cardUid, damage, name, tempElementTypes, tempCardProperty);
                        }
                        else if (tempCardTypes == cardTypes.Spell)
                        {
                            temp = new SpellCard(cardUid, damage, name, tempElementTypes);
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
            _databaseConnection.Close();
            return temp;
        }
        public int GetCardsCountFromDb()
        {
            string query = "SELECT * From cardcollection;";
            NpgsqlCommand commandDatabase = new NpgsqlCommand(query, _databaseConnection);
            commandDatabase.CommandTimeout = 60;
            int counter = 0;
            try
            {
                _databaseConnection.Open();
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
            _databaseConnection.Close();
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
        public void AddCardsToTrade(DbUser dbUser, int numbercard, string cardType, string damage)
        {

            string query = DbFunctions.MakeQuery4AddToTrade(dbUser, numbercard, cardType, damage);

            ExecuteQuery(query);

            //karten aus card collection des users löschen
            query = DbFunctions.MakeMessageTradDelete(dbUser, dbUser.cardCollection[numbercard]);
            ExecuteQuery(query);
        }
        public List<TradingObject> GetCardsToTrade(string query)
        {
            
            List<TradingObject> tradingList = new List<TradingObject>();

            var commandDatabase = new NpgsqlCommand(query, _databaseConnection);
            commandDatabase.CommandTimeout = 60;
            try
            {
                _databaseConnection.Open();
                var myReader = commandDatabase.ExecuteReader();
                //Console.WriteLine(myReader);
                if (myReader.HasRows)
                {
                    Console.WriteLine("Query Generated result:");
                    while (myReader.Read())
                    {
                        TradingObject temp = new TradingObject();
                        Console.WriteLine(myReader.GetValue(0) + "-" + myReader.GetString(1) + " - " + myReader.GetString(2) + " - " + myReader.GetValue(3));
                        //nur zur übersicht   
                        //temp += myReader.GetString(0) + "-" + myReader.GetString(1) + "-" + myReader.GetString(2) + "-" + myReader.GetValue(3).ToString() + "\n";
                        temp.userUid = myReader.GetString(0);
                        temp.cardUid = myReader.GetString(1);
                        temp.wantedCardType = myReader.GetString(2);
                        temp.requiredDamage = myReader.GetInt32(3);                        
                        //temp.card = GetOneCard(temp.cardUid);                        
                        tradingList.Add(temp);
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
            _databaseConnection.Close();
            return tradingList;
        }
        public BaseCards GetOneCard(string card_uid)
        {
            BaseCards card = null;
            string query = "Select * from cardcollection where card_uid = '" + card_uid + "'";
           
            var commandDatabase = new NpgsqlCommand(query, _databaseConnection);
            commandDatabase.CommandTimeout = 60;
            try
            {
                _databaseConnection.Open();
                var myReader = commandDatabase.ExecuteReader();
                //Console.WriteLine(myReader);
                while (myReader.Read())
                {
                    string cardUid = myReader.GetString(0);
                    elementTypes tempElementTypes = (elementTypes)Enum.Parse(typeof(elementTypes), myReader.GetString(1));
                    cardTypes tempCardTypes = (cardTypes)Enum.Parse(typeof(cardTypes), myReader.GetString(2));
                    cardProperty tempCardProperty = (cardProperty)Enum.Parse(typeof(cardProperty), myReader.GetString(3));
                    string name = myReader.GetString(4);
                    int damage = myReader.GetInt32(5);

                    if (tempCardTypes == cardTypes.Monster)
                    {
                        card = new MonsterCard(cardUid, damage, name, tempElementTypes, tempCardProperty);
                    }
                    else if (tempCardTypes == cardTypes.Spell)
                    {
                        card = new SpellCard(cardUid, damage, name, tempElementTypes);
                    }
                }                
            }
            catch (Exception e)
            {
                Console.WriteLine("Query Error: " + e.Message);
            }
            _databaseConnection.Close();
            return card;
        }

        public bool UpdateCardsByTrade(DbUser dbUser, BaseCards card, TradingObject tradingListe)
        {
            bool indicator = false;
            //löschen aus cartencollction
            string queryDelete = "Delete From userdata_cardcollection where fk_user_uid = '" + dbUser.uid
                + "' and fk_card_uid = '" + card.getUID() + "'";
            if(ExecuteQuery(queryDelete))
            {
                indicator = true;
            }
            //löschen aus tauschliste
            queryDelete = "delete from userdata_cardcollectiontotrade where fk_user_uid = '" + tradingListe.userUid
                + "' and fk_card_uid = '" + tradingListe.cardUid + "'";
            if(ExecuteQuery(queryDelete))
            {
                if(indicator == false)
                {
                    return false;
                }
            }
            return true;
        }
        public bool PutInLists(DbUser dbUser ,BaseCards ownCard, TradingObject wantToHave)
        {
            bool indicator = false;

            BaseCards wantedCard = wantToHave.card;

            //ich selbst
            string query = DbFunctions.MakeQueryForInsertCard(wantedCard, dbUser);
            if (ExecuteQuery(query))
            {
                indicator = true;
            }
            //der andere
            dbUser.uid = wantToHave.userUid;
            query = DbFunctions.MakeQueryForInsertCard(ownCard, dbUser);
            if (ExecuteQuery(query))
            {
                if (indicator == false)
                {
                    return false;
                }
            }

            return true;
        }

    }
}