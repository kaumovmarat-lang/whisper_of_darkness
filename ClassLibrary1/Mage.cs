using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1
{
    public class Mage : Person
    {

        public Mage(string name) : base(name)
        {

        }
        public override void increase_lvl()
        {
            if (Lvl < 99)
            {
                Lvl++;
                MaxHealth += 5;
                Health = MaxHealth;
                MaxMana += 10;
                Mana = MaxMana;
                P_damage += 3;
                M_damage += 7;
                Defense += 3;
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
                    return "тканевая роба";
                case 4:
                    return "арканная мантия";
                case 7:
                    return "мантия архимага";

            }
        }
        public override string ShowWeapon()
        {
            switch ((int)weapon)
            {
                default:
                    return "Пусто";
                case 2:
                    return "деревянный посох";
                case 4:
                    return "кристальный посох";
                case 7:
                    return "посох архимага";

            }
        }
        public override string ShowGear(int a)
        {
            switch (a)
            {
                case 1:
                    return "тканевая роба";
                case 2:
                    return "арканная мантия";
                case 3:
                    return "мантия архимага";
                case 4:
                    return "деревянный посох";
                case 5:
                    return "кристальный посох";
                case 6:
                    return "посох архимага";
                default:
                    return "";
            }
        }
        public override int TotalMDamage => M_damage + (int)weapon;
        public override int TotalDefense => Defense + (int)armor;
    }
}
