using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1
{
    public class Predmet
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public Predmet(string name, int price)
        {
            Name = name;
            Price = price;
        }
    }
    public class Potion : Predmet
    {
 
        public int Heal { get; set; }
        public int Mana { get; set; }

        public Potion(string name, int price, int heal, int mana) : base(name, price)
        {
            Mana = mana;
            Heal = heal;
        }
    }
}
