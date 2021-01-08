using Cards;
using SWE1_MTCG;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Server
{
    public class DbFunctions
    {
        public static ServerDbConnection mysql = new ServerDbConnection();
        public static DbUser VerifyLogin(RequestContext request, NetworkStream stream)
        {
            string tempUsername = request.GetUsernameFromDict();

            //überprüfung, ob der Token passd
            if (!(CheckToken(tempUsername)))
            {
                return null;
            }

            string username = "";
            string[] tempToken = tempUsername.Split(new char[] { '_' });

            //falls auch _ im usernamen drinnen sind
            for (int i = 0; i < tempToken.Length - 1; i++)
            {
                username += tempToken[i];
            }

            DbUser user = mysql.GetOneUser(username); //erstellt ein DB User objekt
            Console.WriteLine(user.userName);
            if (request.GetPwdFromDict() == user.pwd)
            {
                string message = "Succsessful";
                ServerClientConnection.SendData(stream, message);
                return user;
            }
            Console.WriteLine("Wrong user or Pwd!");
            return null;
        }

        public static bool CheckToken(string username)
        {
            string tokenForCheck = "MTCG-Game-Token";
            bool temp = false;
            string[] token = username.Split(new char[] { '_' });

            int lenght = token.Length;

            if (token[lenght - 1] == tokenForCheck)
            {
                return true;
            }
            return temp;
        }
        public static bool RegisterAtDb(RequestContext request, NetworkStream stream)
        {
            ServerDbConnection mysql = new ServerDbConnection();
            //check if username already taken            
            if ((mysql.GetOneUser(request.GetUsernameFromDict()).userName != null))
            {
                //wenns den user bereits gibt
                Console.WriteLine("Wrong username");
                return false;
            }
            

            //chek if email already taken
            bool isValidEmail = ValidEmail(request.GetEmailFromDict());
            if (!((mysql.GetOneUser(request.GetUsernameFromDict()).email == null) && (isValidEmail == true)))
            {
                //wenns die email ned okay ist
                Console.WriteLine("Wrong email!");
                return false;
            }

            //Query statement bilden
            string query = MakeRegisterQuery(request);

            //var temp = new ServerDbCOnnection();
            bool succsess = mysql.VerifyRegister(query);

            if (succsess == true)
            {
                Console.WriteLine("You are registered");
            }
            else
            {
                Console.WriteLine("Error by executing Databas Conn");
                return false;
            }
            return true;
        }

        public static string CreateUid(int size)
        {
            string teststring = string.Format("{0:N}", Guid.NewGuid()); //erstellt eine unique Id 
            teststring = teststring.Substring(0, size); //kürzt die ID auf 8 stellen
            return teststring;
        }

        public static bool ValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                Console.WriteLine("its not a valide email!");
                return false;
            }
        }
        public static string MakeRegisterQuery(RequestContext request)
        {
            int size = 10;
            string username = request.GetUsernameFromDict();
            string password = request.GetPwdFromDict();
            string email = request.GetEmailFromDict();
            string newUsername = "";
                
            string[] tempToken = username.Split(new char[] { '_' });

            //falls auch _ im usernamen drinnen sind
            for (int i = 0; i < tempToken.Length - 1; i++)
            {
                newUsername += tempToken[i];
            }

            string uid = CreateUid(size);

            string temp = "Insert Into UserData\n " +
                           "(user_uid, userName, email, pwd, coins, elo_points)\n" +
                           "VALUES\n" +
                           "('" + uid + "', '" + newUsername + "', '" + email + "', '" + password + "', '" + 100 + "', '" + 100 + "')";
            return temp;
        }
        public static string MakeQueryGetCards(string username)
        {
            string temp = "SELECT * From cardcollection\n" +
                           "JOIN userdata_cardcollection\n" +
                           "ON cardcollection.card_uid = userdata_cardcollection.fk_card_uid\n" +
                           "JOIN userdata\n" +
                           "on userdata.user_uid = userdata_cardcollection.fk_user_uid\n" +
                           "where username = '" + username + "'" +
                           "order by card_damage desc";
            return temp;
        }
        public static List<BaseCards> OptainNewCards(DbUser userFromDb, Random rand)
        {
            List<BaseCards> tempList = new List<BaseCards>();
            BaseCards baseCard;
            ServerDbConnection dbConn = new ServerDbConnection();
            int cost = 25;
            string query = "SELECT * From cardcollection;";
            int temprand = rand.Next(100000);

            dbConn.CloseDbCon();

            if (userFromDb.coins >= cost)
            {
                userFromDb.coins -= cost; //coins abziehen
                for (int i = 0; i < 4; i++)
                {
                    //welche karte bekommt man
                    int cardsNumber = dbConn.GetCardsCountFromDb();
                    int cardPlace = rand.Next(0, cardsNumber);
                    baseCard = dbConn.GetOneRandCardFromDb(query, cardsNumber, cardPlace * temprand);
                    Console.WriteLine(baseCard.getCardName());
                    //karte in datenbank einfügen
                    dbConn.GetCardToUser(baseCard, userFromDb);
                    tempList.Add(baseCard);
                }
                //update coin number
                query = MakeQueryForUpdateCoins(userFromDb);
                dbConn.ExecuteQuery(query);
            }
            else
            {
                //nicht genug coins
                return null;
            }
            return tempList;
        }
        public static string MakeQueryForUpdateCoins(DbUser userFromDb)
        {
            string temp = "update userdata " +
                          "set coins = " + userFromDb.coins + " " +
                          "where userName = '" + userFromDb.userName + "';";
            return temp;
        }
        public static string MakeQueryForCreateNewCard(BaseCards baseCard)
        {
            string temp = "Insert into cardcollection\n" +
                          "(card_uid, element_type, card_property, card_type, card_name, card_damage)\n" +
                          "VALUES\n" +
                          "('" + baseCard.getUID() + "', '" + baseCard.getElementTypes() + "', '" + baseCard.getCardProperty() + "', '" + baseCard.getCardType() + "', '" + baseCard.getCardName() + "', '" + baseCard.getCardDamage() + "');";

            return temp;
        }
        public static string MakeQueryForInsertCard(BaseCards baseCard, DbUser user)
        {
            string temp = "Insert Into UserData_CardCollection\n" +
                          "(fk_user_uid, fk_card_uid)\n" +
                          "VALUES\n" +
                          "('" + user.uid + "', '" + baseCard.getUID() + "');";
            return temp;
        }
        public static bool PassQuery(string message)
        {
            bool temp = false;
            temp = mysql.ExecuteQuery(message);
            return temp;
        }
        public static string MakeQueryForUpdateElo(DbUser userFromDb, string wert)
        {
            string temp = "update userdata " +
                          "set elo_points = elo_points " + wert + " " +
                          "where userName = '" + userFromDb.userName + "';";
            return temp;
        }
        public static string MakeQuery4AddToTrade(DbUser dbUser, int numbercard, string cardType, string damage)
        {
            if (cardType == "1")
            {
                cardType = "Monster";
            }
            else if (cardType == "2")
            {
                cardType = "Spell";
            }
            else
                cardType = "error";

            string temp = $"Insert Into UserData_CardCollectiontotrade(fk_user_uid, fk_card_uid, card_type, required_damage) values " +
                $"('{dbUser.uid}', '{dbUser.cardCollection[numbercard].getUID()}', '{cardType}', {damage})";

            return temp;
        }
        public static string MakeMessageTradDelete(DbUser userFromDb, BaseCards card)
        {
            string temp = "DELETE FROM userdata_cardcollection WHERE ";

            temp += "fk_user_uid = '" + userFromDb.uid + "' AND fk_card_uid = '" + card.getUID() + "';";

            return temp;
        }
        public static string ReturnCardsToTradeString()
        {
            List<TradingObject> cardsListTrading = new List<TradingObject>();
            TradingObject tempObject = new TradingObject();
            string query = "SELECT * FROM userdata_cardcollectiontotrade";
            int counter = 0;
            string answer = "";

            cardsListTrading = mysql.GetCardsToTrade(query);

            foreach(TradingObject part in cardsListTrading)
            {
                part.card = mysql.GetOneCard(part.cardUid);
            }

            foreach (TradingObject part in cardsListTrading)
            {
                answer += $"{ counter + 1} : The Player wants a , {cardsListTrading[counter].wantedCardType},  card with min damage , {cardsListTrading[counter].requiredDamage}\n";
                answer += $"Offeded is a , {cardsListTrading[counter].card.getCardType()} card with element {cardsListTrading[counter].card.getElementTypes()} ";

                if (cardsListTrading[counter].card.getCardType() == MyEnum.cardTypes.Monster)
                {
                    answer += " and Property " + cardsListTrading[counter].card.getCardProperty();
                }

                answer += $"with min damage {cardsListTrading[counter].card.getCardDamage()}\n\n";
                counter++;
            }
            return answer;
        }

        public static List<TradingObject> ReturnCardsToTradeCards()
        {
            List<TradingObject> cardsListTrading = new List<TradingObject>();            
            string query = "SELECT * FROM userdata_cardcollectiontotrade";

            cardsListTrading = mysql.GetCardsToTrade(query);

            foreach (TradingObject part in cardsListTrading)
            {
                part.card = mysql.GetOneCard(part.cardUid);
            }

            return cardsListTrading;
        }

        public static bool ChekTrade(string cardWantToHave, List<TradingObject> cardListToTrade, string choiceToTrade, List<BaseCards> userCardcollection, string answerToTrade)
        {
            BaseCards card = userCardcollection[Int32.Parse(choiceToTrade) - 1];
            BaseCards checkFromCard = cardListToTrade[Int32.Parse(cardWantToHave) - 1].card;
            TradingObject tempObject = cardListToTrade[Int32.Parse(cardWantToHave) - 1];
            bool indicator = false;

            int damage = tempObject.requiredDamage; //damage den man will
            string temp = tempObject.wantedCardType;

            if (temp.Contains("Spell"))
            {
                if (MyEnum.cardTypes.Spell == card.getCardType())
                {
                    if (card.getCardDamage() >= damage)
                    {
                        indicator = true;
                    }
                }
            }
            else if (temp.Contains("Monster"))
            {
                if (MyEnum.cardTypes.Monster == card.getCardType())
                {
                    if (card.getCardDamage() >= damage)
                    {
                        indicator = true;
                    }
                }
            }
            return indicator;
        }
    }
}