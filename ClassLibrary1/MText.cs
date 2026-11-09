using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1
{
    public class MText
    {

        public static void Left(string text)
        {
            Console.WriteLine(text);
        }

        public static void Center(string text)
        {
            int windowWidth = Console.WindowWidth;
            if (text.Length >= windowWidth)
            {
                Console.WriteLine(text);
                return;
            }
            int leftPadding = (windowWidth - text.Length) / 2;
            Console.WriteLine(new string(' ', leftPadding) + text);
        }

        public static void Right(string text)
        {
            int windowWidth = Console.WindowWidth;
            if (text.Length >= windowWidth)
            {
                Console.WriteLine(text);
                return;
            }
            int leftPadding = windowWidth - text.Length;
            Console.WriteLine(new string(' ', leftPadding) + text);
        }

    }
}
