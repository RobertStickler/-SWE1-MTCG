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
            List<BaseCards> cards4Battle1 = new List<BaseCards>();

            cards4Battle1.Add(CardShop.GetRandCard(Server.rand));
            cards4Battle1.Add(CardShop.GetRandCard(Server.rand));
            cards4Battle1.Add(CardShop.GetRandCard(Server.rand));
            cards4Battle1.Add(CardShop.GetRandCard(Server.rand));

            return cards4Battle1;
        }
        public static string AddToBattleQueue(List<DbUser> liste)
        {


            if ((liste.Count >= 2))
            {
                string username = "";

                //int sieger = BattleLogic.StartBattle(playerOne.cardDeck, playerTwo.cardDeck);
                int sieger = BattleLogic.StartBattle(liste[0].cardDeck, liste[1].cardDeck);
                //noch das deck umbauen

                if (sieger == 1)
                {
                    username = liste[0].userName;
                    SendWinnerToClient(1, liste);
                    liste.RemoveAt(1);
                    liste.RemoveAt(0);
                    return username;
                }
                if (sieger == 2)
                {
                    username = liste[1].userName;
                    SendWinnerToClient(2, liste);
                    liste.RemoveAt(1);
                    liste.RemoveAt(0);
                    return username;
                }
                return "noOne";
            }
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

        public static void SendWinnerToClient(int winner, List<DbUser> liste)
        {
            string messageWinner = "you have won the Battle\n";
            string messageLooser = "you have lost the Battle\n";
            

            if(winner == 1)
            {
                //player1 hat gewonnen
                ServerClientConnection.SendData(liste[0].stream, messageWinner);
                ServerClientConnection.SendData(liste[1].stream, messageLooser);
            }
            else if(winner == 2)
            {
                //player2 hat gewonnen
                ServerClientConnection.SendData(liste[1].stream, messageWinner);
                ServerClientConnection.SendData(liste[0].stream, messageLooser);
            }
        }



    }
}
