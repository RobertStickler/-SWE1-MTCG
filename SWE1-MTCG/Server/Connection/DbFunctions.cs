using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using SWE1_MTCG;
using Cards;
using System.Runtime.Serialization.Formatters;

namespace Server
{
    public class DbFunctions
    {
        public static DbUser VerifyLogin(RequestContext request, NetworkStream stream)
        {            
            MySqlDataClass mysql = new MySqlDataClass();
            DbUser user = mysql.GetOneUser(request.GetUsernameFromDict()); //erstellt ein DB User objekt
            Console.WriteLine(user.userName);
            if (request.GetPWDFromDict() == user.pwd)
            {
                string message = "Succsessful";
                ServerClientConnection.sendData(stream, message);
                return user;
            }
            Console.WriteLine("Wrong user or Pwd!");
            return null;
        }
        //following is still in progress lol
        public static RequestContext makeAnotherRequest(TcpClient client, NetworkStream stream)
        {
            var request = new RequestContext();
            string message = "please try again";
            // Translate the Message into ASCII.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);

            return request;
        }
        public static void RegisterAtDB(RequestContext request, NetworkStream stream)        
        {
            MySqlDataClass mysql = new MySqlDataClass();            
            //check if username already taken            
            if (!(mysql.GetOneUser(request.GetUsernameFromDict()).userName == null))
            {
                //wenns den user bereits gibt
                Console.WriteLine("Username does already exist!");
                return;
            }

            //chek if email already taken
            bool isValidEmail = ValidEmail(request.GetEmailFromDict());
            if (!((mysql.GetOneUser(request.GetUsernameFromDict()).email == null) && (isValidEmail == true)))
            {
                //wenns die email ned okay ist
                Console.WriteLine("Email does already exist!");
                return;
            }

            //Query statement bilden
            string query = MakeRegisterQuery(request);

            var temp = new MySqlDataClass();
            bool succsess = temp.VerifyRegister(query);

            if(succsess == true)
            {
                Console.WriteLine("You are registrated");
            }
            else
            {
                Console.WriteLine("Error by Databas Conn");
            }
        }

        public static string CreateUid(int size)
        {
            string teststring = string.Format("{0:N}", Guid.NewGuid()); //erstellt eine unique Id 
            teststring =  teststring.Substring(0, size); //kürzt die ID auf 8 stellen
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
                Console.WriteLine("its not a  valide!");
                return false;
            }
        }
        public static string MakeRegisterQuery(RequestContext request)
        {
            int size = 10;
            string username = request.GetUsernameFromDict();
            string password = request.GetPWDFromDict();
            string email = request.GetEmailFromDict();

            string uid = CreateUid(size);

            string temp = "Insert Into UserData\n " +
                           "(user_uid, userName, email, pwd, coins)\n" +
                           "VALUES\n" +
                           "('" + uid + "', '" + username + "', '" + email + "', '" + password + "', '" + 100 + "')";
            return temp;
        }
        public static string MakeQueryGetCards(string username)
        {
            string temp = "SELECT * From cardcollection\n" +
                           "JOIN userdata_cardcollection\n" +
                           "ON cardcollection.card_uid = userdata_cardcollection.fk_card_uid\n" +
                           "JOIN userdata\n" +
                           "on userdata.user_uid = userdata_cardcollection.fk_user_uid\n" +
                           "where username = '" + username + "'";
            return temp;
        } 

    }
}