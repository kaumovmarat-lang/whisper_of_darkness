using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Text;

namespace рпг
{
    internal class minigame
    {

        static Random random = new Random();

        public static void train(Person p)
        {
            if (p.Stamina < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                MText.Center("Недостаточно стамины!");
                return;
            }

            int a = minigameStart();
            p.Stamina -= 1;
            if (a == 5)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                MText.Center("Тренировка прошла отлично!");
                Console.ForegroundColor = ConsoleColor.White;
                p.EarnXp(2);
            }
            else if (a >= 3)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                MText.Center("Тренировка прошла хорошо!");
                Console.ForegroundColor = ConsoleColor.White;
                p.EarnXp(1);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                MText.Center("Тренировка провалилась");
                Console.ForegroundColor = ConsoleColor.White;
            }
            
        }
        static int minigameStart()
        {

            int tries = 0;
            int victories = 0;
            while (tries < 5)
            {
                while (Console.KeyAvailable) Console.ReadKey(true); //сброс прошлого нажатия
                int x = random.Next(1, 10);
                Console.Clear();
                Console.WriteLine("\t1 2 3 4 5 6 7 8 9");

                int answer = 0;
                bool answered = false;

                Console.Write("\t");
                for (int i = 1; i < x; i++)
                {
                    Console.Write(". ");
                }
                Console.WriteLine("X");

                for (int t = 0; t < 15; t++)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true);
                        if (key.KeyChar >= '1' && key.KeyChar <= '9')
                        {
                            answer = Convert.ToInt32(key.KeyChar.ToString());
                            answered = true;
                            break;
                        }
                    }
                    Thread.Sleep(100);
                }

                if (!answered)
                {
                    Console.WriteLine("Время вышло!");
                    tries++;
                }
                else if (answer == x)
                {
                    Console.WriteLine("вы поймали число!");
                    victories++;
                    tries++;
                }
                else
                {
                    Console.WriteLine("неправильно!");
                    tries++;
                }
                Thread.Sleep(1000);
            }
            Console.WriteLine($"Вы выиграли {victories} раз из 5!");
            return victories;
        }
    }
}
