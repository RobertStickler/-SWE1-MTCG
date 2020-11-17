using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;
using SWE1_MTCG;
using Cards;
using MyEnum;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters;
using SWE1_MTCG.Cards.Monster;
using SWE1_MTCG.Cards.Zauber;

namespace Server
{
    //eine funktioin um die karten vom user herauszufinden
    //eine funktion um die besten 4 karten herauszufinden
    public class MySqlDataClass
    {
        public MySqlConnection databaseConnection;

        public MySqlDataClass()
        {
            setConnect();
        }

        public void setConnect()
        {
            string mySQLConnectionString = "datasource=127.0.0.1;port=3306; username=root;password=;database=swe_mtcg;";
            databaseConnection = new MySqlConnection(mySQLConnectionString);


        }

        public DbUser GetOneUser(string userName)
        {
            string query = "SELECT * FROM userdata WHERE UserName = '" + userName + "';";
            //string query = "SELECT * FROM userdata;";


            DbUser userObjekt = new DbUser();
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
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
            DbUser userObjekt = new DbUser();
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            try
            {
                databaseConnection.Open();
                MySqlDataReader myReader = commandDatabase.ExecuteReader();
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





















        public List<BaseCards> getCardsFromDB()
        {
            List<BaseCards> cards = new List<BaseCards>();
            string query = "SELECT * FROM cardcollection;";
            Console.WriteLine(query);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            try
            {
                databaseConnection.Open();
                MySqlDataReader myReader = commandDatabase.ExecuteReader();
                Console.WriteLine(myReader);
                if (myReader.HasRows)
                {
                    Console.WriteLine("Query Generated result:");
                    while (myReader.Read())
                    {
                        BaseCards temp = null;

                        Console.WriteLine(myReader.GetValue(0) + " - " + myReader.GetString(1) + " - " + myReader.GetString(2) + " - " + myReader.GetValue(3) + " - " + myReader.GetValue(4) + " - " + myReader.GetValue(5));
                        //nur zur übersicht
                        elementTypes temp_elementTypes = (elementTypes)Enum.Parse(typeof(elementTypes), myReader.GetString(1));
                        cardTypes temp_cardTypes = (cardTypes)Enum.Parse(typeof(cardTypes), myReader.GetString(2));
                        cardProperty temp_cardProperty = (cardProperty)Enum.Parse(typeof(cardProperty), myReader.GetString(3));
                        string name = myReader.GetString(4);
                        int damage = Int32.Parse(myReader.GetString(5));

                        if (temp_cardTypes == cardTypes.Monster)
                        {
                            temp = new MonsterCard(damage, name, temp_elementTypes, temp_cardProperty);
                        }
                        else if (temp_cardTypes == cardTypes.Spell)
                        {
                            temp = new SpellCard(damage, name, temp_elementTypes);                            
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
            return cards;
        }
    }
}