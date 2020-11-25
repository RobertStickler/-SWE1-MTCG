using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG;
using Cards;


namespace Server
{
    class BattleMaker
    {
        public static List<BaseCards> GetRandCards()
        {
            List<BaseCards> Cards4Battle1 = new List<BaseCards>();

            Cards4Battle1.Add(CardShop.GetRandCard(Server.rand));
            Cards4Battle1.Add(CardShop.GetRandCard(Server.rand));
            Cards4Battle1.Add(CardShop.GetRandCard(Server.rand));
            Cards4Battle1.Add(CardShop.GetRandCard(Server.rand));

            return Cards4Battle1;
        }
        public static string AddToBattleQueue(List<RequestContext> Liste)
        {


            if ((Liste.Count >= 2))
            {
                //start battle
                /*
                var playerOne = Liste[0];
                var playerTwo = Liste[1];
                Liste.RemoveAt(1);
                Liste.RemoveAt(0);
                */
                string username = "";

                //int sieger = BattleLogic.StartBattle(playerOne.cardDeck, playerTwo.cardDeck);
                int sieger = BattleLogic.StartBattle(Liste[0].cardDeck, Liste[1].cardDeck);
                //noch das deck umbauen

                if (sieger == 1)
                {
                    username = Liste[0].GetUsernameFromDict();
                    SendWinnerToClient(1, Liste);
                    Liste.RemoveAt(1);
                    Liste.RemoveAt(0);
                    return username;
                }
                if (sieger == 2)
                {
                    username = Liste[1].GetUsernameFromDict();
                    SendWinnerToClient(2, Liste);
                    Liste.RemoveAt(1);
                    Liste.RemoveAt(0);
                    return username;
                }
                return "noOne";
            }
            //start the battel
            //Console.WriteLine("two players found!");

            return "noOne";
        }
        public static List<BaseCards> The4BestCards(List<BaseCards> cardCollection)
        {
            List<BaseCards> cardDeck = new List<BaseCards>();
            BaseCards tempCard = null;

            while (cardDeck.Count < 4)//solange weniger als 4 karten in der liste sind
            {
                int temp = 0;
                for (int i = 0; i < cardCollection.Count; i++)
                {
                    if (cardCollection[i].getCardDamage() >= temp) //der damager höher
                    {
                        //noch prüfen, ob noch nicht in der liste
                        bool isInList = CheckList(cardCollection[i], cardDeck);

                        if(isInList == false)
                        {
                            temp = cardCollection[i].getCardDamage();
                            tempCard = cardCollection[i];
                        }
                    }
                }
                cardDeck.Add(tempCard);
            }



            return cardDeck;
        }
        public static bool CheckList(BaseCards cardToAdd, List<BaseCards> cardDeck)
        {

            for(int i = 0; i < cardDeck.Count; i++)
            {
                if(cardDeck[i] == cardToAdd)
                {
                    return true;
                }
            }
            return false;
        }

        public static void SendWinnerToClient(int winner, List<RequestContext> Liste)
        {
            string messageWinner = "you have won the Battle\n";
            string messageLooser = "you have lost the Battle\n";
            

            if(winner == 1)
            {
                //player1 hat gewonnen
                ServerClientConnection.sendData(Liste[0].stream, messageWinner);
                ServerClientConnection.sendData(Liste[1].stream, messageLooser);
            }
            else if(winner == 2)
            {
                //player2 hat gewonnen
                ServerClientConnection.sendData(Liste[1].stream, messageWinner);
                ServerClientConnection.sendData(Liste[0].stream, messageLooser);
            }
        }



    }
}
