using ClassLibrary1;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace рпг
{
    internal class Foe
    {
        public Enemy Who { get; private set; }
        public CanvasImage ImageBattle { get; private set; } //искаженная
        public CanvasImage ImageSaved { get; private set; } //спасенная
        public int MinDay { get; private set; }
        public int RecLvl { get; private set; }
        public int Money { get; private set; }
        public string[] After { get; private set; }

        public Foe(Enemy who, CanvasImage imageBattle, CanvasImage imageSaved, int minDay, int recLvl, int money, string[] after)
        {
            Who = who;
            ImageBattle = imageBattle;
            ImageSaved = imageSaved;
            MinDay = minDay;
            RecLvl = recLvl; 
            Money = money; 
            After = after;
        }
    }
}
