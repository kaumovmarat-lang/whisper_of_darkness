using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1
{
    public class Knight : Person
    {
        public Knight(string name) : base(name)
        {

        }
        public override void increase_lvl()
        {
            if (Lvl < 99)
            {
                Lvl++;
                MaxHealth += 7;
                Health = MaxHealth;
                MaxMana += 5;
                Mana = MaxMana;
                P_damage += 5;
                M_damage += 5;
                Defense += 7;
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
                    return "деревянная броня";
                case 4:
                    return "серебрянная броня";
                case 7:
                    return "рыцарская броня";

            }
        }
        public override string ShowWeapon()
        {
            switch ((int)weapon)
            {
                default:
                    return "Пусто";
                case 2:
                    return "деревянный меч";
                case 4:
                    return "анубис";
                case 7:
                    return "экскалибур";

            }
        }
        public override string ShowGear(int a)
        {
            switch (a)
            {
                case 1:
                    return "деревянная броня";
                case 2:
                    return "серебрянная броня";
                case 3:
                    return "рыцарская броня";
                case 4:
                    return "деревянный меч";
                case 5:
                    return "анубис";
                case 6:
                    return "экскалибур";
                default:
                    return "";
            }
        }
        public override int TotalPDamage => P_damage + (int)weapon;
        public override int TotalDefense => Defense + (int)armor;
    }
}
