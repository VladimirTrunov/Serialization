using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLDeserTesting
{
    public class PC
    {
        public string str = "HI!!!!";
        public List<Player> guys;
        public List<int> cash;
        public List<string> characterNames { get; set; }
        public static int s; 
        private int a;
        public Player player;
        
        public string GameName { get; set; }
        public string[] WordsArray;
        public string[] WordsArray2 { get; set; }
        public PC(string playerName, int playerAge, int playerRank, string gameName)
        {
            GameName = gameName;
            player = new Player(playerName, playerAge, playerRank);
            cash = new List<int>() { 12, 11, 13 };
            a = 30;
            s = 5;

            characterNames = new List<string>() {"Bill", "Bob", "Victor" };
            WordsArray = new string[3] { "Hi", "Bye", "You" };
            WordsArray2 = new string[3] { "Screw", "You", "Guys" };


            guys = new List<Player>() { new Player("Viktor", 10, 1), new Player("Sarah", 23, 81) };

        }
        public PC() { }
    }
}
