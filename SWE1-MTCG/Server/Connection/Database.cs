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

namespace Server
{
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

        public void runQuery(string queryToRun)
        {
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

                        Console.WriteLine(myReader.GetValue(0).GetType() + " - " + myReader.GetString(1).GetType() + " - " + myReader.GetString(2).GetType() + " - " + myReader.GetValue(3).GetType());
                        //id                        //firstname                         //givename                          //cardid
                        Console.WriteLine(myReader.GetValue(0) + " - " + myReader.GetString(1) + " - " + myReader.GetString(2) + " - " + myReader.GetValue(3));
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

                        switch(temp_cardProperty)
                        {
                            case cardProperty.Dragon:
                                {
                                    temp = new Dragon(damage, name, temp_elementTypes);
                                        break;
                                }
                            case cardProperty.Knight:
                                {
                                    temp = new Knight(damage, name, temp_elementTypes);
                                    break;
                                }
                            case cardProperty.Ork:
                                {
                                    temp = new Ork(damage, name, temp_elementTypes);
                                    break;
                                }
                            case cardProperty.Wizard:
                                {
                                    temp = new Wizard(damage, name, temp_elementTypes);
                                    break;
                                }
                            case cardProperty.Goblin:
                                {
                                    temp = new Goblin(damage, name, temp_elementTypes);
                                    break;
                                }
                            case cardProperty.Kraken:
                                {
                                    temp = new Kraken(damage, name, temp_elementTypes);
                                    break;
                                }
                            case cardProperty.Elf:
                                {
                                    temp = new Elf(damage, name, temp_elementTypes);
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }


                        //new BaseCards(damage, name, elementTypes);
                        cards.Add(temp);
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
            return cards;
        }
    }
}