using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1
{
    public class Enemy : Person
    {
        Random random = new Random();
        public int xpEarnEnemy;
        public Enemy(string name, int lvl, int xpEarnEnemy) : base(name, lvl)
        {
            for (int i = 1; i < lvl; i++) increase_lvl();
            this.xpEarnEnemy = xpEarnEnemy;
        }
        public Enemy(string name, int lvl, int xpEarnEnemy, int armor, int weapon) : base(name, lvl)
        {
            for (int i = 1; i < lvl; i++) increase_lvl();
            this.xpEarnEnemy = xpEarnEnemy;
            this.armor = (Armor)armor;
            this.weapon = (Weapon)weapon;
        }

        public override void Attack(Person opponent)
        {
            int a = random.Next(1, 5);
            if (a <= 3)
            {
                base.Attack(opponent);
            }
            else
            {
                if (!UseSkill(random.Next(0, skills.Count), opponent))
                    base.Attack(opponent); 

            }

        }
        
        public override int TotalPDamage => P_damage + (int)weapon;
        public override int TotalDefense => Defense + (int)armor;
    }
}
