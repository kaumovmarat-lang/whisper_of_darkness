using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1
{
    public class Berserk : Person
    {

        public Berserk(string name) : base(name)
        {

        }
        public override void increase_lvl()
        {
            if (Lvl < 99)
            {
                Lvl++;
                MaxHealth += 10;
                Health = MaxHealth;
                MaxMana += 3;
                Mana = MaxMana;
                P_damage += 7;
                M_damage += 3;
                Defense += 5;
                Stamina++;
            }
            else Console.WriteLine("Больше некуда прокачиваться");
        }
        public override string ShowArmor()
        {
            switch ((int)armor)
            {
                default:
                    return "Пусто";
                case 2:
                    return "кожаная броня";
                case 4:
                    return "медвежья шкура";
                case 7:
                    return "броня ярости";

            }
        }
        public override string ShowWeapon()
        {
            switch ((int)weapon)
            {
                default:
                    return "Пусто";
                case 2:
                    return "дубина";
                case 4:
                    return "железный топор";
                case 7:
                    return "клинок ярости";

            }
        }
        public override string ShowGear(int a)
        {
            switch (a)
            {
                case 1:
                    return "кожаная броня";
                case 2:
                    return "медвежья шкура";
                case 3:
                    return "броня ярости";
                case 4:
                    return "дубина";
                case 5:
                    return "железный топор";
                case 6:
                    return "клинок ярости";
                default:
                    return "";
            }
            
        }
        public override int TotalPDamage => P_damage + (int)weapon;
        public override int TotalDefense => Defense + (int)armor;
    }
}
