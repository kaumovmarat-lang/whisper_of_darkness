using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1
{
    public class Mission
    {
        public bool complete;
        public string name;
        public string description;
        public int Earned_money;
        public int Earned_xp;
        public bool rewarded;

        public Mission(string name, string desc, int m, int x)
        {
            complete = false;
            this.name = name;
            this.description = desc;
            this.Earned_money = m;
            this.Earned_xp = x;
        }
        
        public void Reward(Person p)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (rewarded) MText.Center("Награда уже получена");
            else if (!complete) MText.Center($"Вы еще не выполнили миссию '{name}'");
            else
            {
                rewarded = true;
                Console.ForegroundColor = ConsoleColor.Green;
                MText.Center($"Вы выполнили миссию {name} и получили награду!");
                MText.Center($"Получено: {Earned_xp} XP и {Earned_money} денег");
                p.money += Earned_money;
                p.EarnXp(Earned_xp);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void CheckMission()
        {
            Console.ForegroundColor = ConsoleColor.White;
            string mark = "[ ]";
            if (rewarded)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                mark = "[получено]";
            }
            else if (complete) 
            {
                Console.ForegroundColor = ConsoleColor.Green;
                mark = "[выполнено]";
            }
            MText.Center($"{mark} {name}: {description}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
