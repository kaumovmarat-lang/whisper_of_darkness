
namespace ClassLibrary1
{
    public class Person
    {
        private static Random random = new Random();
        string name;
        int lvl;
        public int xp;
        public int MaxHealth;
        int health;
        public int MaxMana;
        int mana;
        int stamina;
        int p_damage;
        int m_damage;
        int defense;
        public int MaxStamina => 2 + Lvl;
        public int money; //мб магаз добавлю хз

        public string Name
        {
            get { return name; }
            set {
                if (string.IsNullOrWhiteSpace(value))
                    name = "Безымянный";
                else
                name = value; 
            }
        }
        public int Lvl
        {
            get { return lvl; }
            set {
                if (value > 99)
                    lvl = 99;
                else if (value < 1)
                    lvl = 1;
                else
                    lvl = value; 
            }
        }
        public int Health
        {
            get { return health; }
            set { health = Math.Clamp(value, 0, MaxHealth); }
        }
        public int Mana
        {
            get { return mana; }
            set { mana = Math.Clamp(value, 0, MaxMana); }
        }
        public int Stamina
        {
            get { return stamina; }
            set {
                if (value < 0)
                    stamina = 0;
                else
                    stamina = value;
            }
        }
        public int P_damage
        {
            get { return p_damage; }
            set {
                if (value < 1)
                    p_damage = 1;
                else
                    p_damage = value;
            }
        }
        public int M_damage
        {
            get { return m_damage; }
            set {
                if (value < 1)
                    m_damage = 1;
                else
                    m_damage = value;
            }
        }
        public int Defense
        {
            get { return defense; }
            set {
                if (value < 0)
                    defense = 0;
                else
                    defense = value;
            }
        }

        public List<Skill> skills = new List<Skill>();
        public List<Predmet> inventory = new List<Predmet>();
        public virtual int TotalPDamage => P_damage;
        public virtual int TotalMDamage => M_damage;
        public virtual int TotalDefense => Defense;
        public void EarnXp(int xp)
        {
            MText.Center($"Заработано {xp} опыта!");
            this.xp += xp;
            while (Lvl < 99 && this.xp >= Lvl * 5)
            {
                this.xp -= Lvl * 5;
                increase_lvl();
                MText.Center($"{Name} повысил уровень!");
            }
        }
        public virtual void increase_lvl()
        {
            if (lvl < 99)
            {
              lvl++;
              MaxHealth += 2;
              health = MaxHealth;
              MaxMana += 2;
              mana = MaxMana;
              p_damage += 2;
              m_damage += 2;
              defense += 2;
              stamina++;
            }
            else Console.WriteLine("Больше некуда прокачиваться");
            
        }
        public virtual void Attack(Person opponent)
        {
            int hit = TotalPDamage + random.Next(0,5);
            int def = opponent.TotalDefense + random.Next(0,5);
            int result = hit - def;
            if (result > 0) 
            {
                opponent.Health -= hit - def;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{Name} нанес {result} урона по {opponent.Name}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Атака не прошла по {opponent.Name}!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            
        }
        public void LearnSkill(Skill skill)
        {
            foreach (Skill sk in skills)
            {
                if (skill.Name == sk.Name)
                {
                    Console.WriteLine("Этот навык у вас уже имеется");
                    Console.ReadKey();
                    return;
                }
            }
            skills.Add(skill);
            Console.WriteLine($"Вы выучили навык '{skill.Name}'");
            Console.ReadKey();
        }

        public void LearnSkill(Skill skill, int a)
        {
            foreach (Skill sk in skills)
            {
                if (skill.Name == sk.Name)
                {
                    Console.WriteLine("Этот навык уже имеется");
                    Console.ReadKey();
                    return;
                }
            }
            skills.Add(skill);
        }
        public bool UseSkill(int which, Person opponent)
        {
            if (which < 0 || which >= skills.Count)
            {
                Console.WriteLine("Такого скилла нет");
                return false;
            }
            if (Mana < this.skills[which].Manacost)
            {
                Console.ForegroundColor= ConsoleColor.DarkBlue;
                Console.WriteLine($"Не хватает маны на {this.skills[which].Name} (нужно {this.skills[which].Manacost}, а имеется {Mana})");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            Mana -= this.skills[which].Manacost;
            int hit = 0;
            int def = 0;
            int man = 0;
            bool hardcode = false;
            if (this.skills[which].Name == "Восстановить ману")
            {
                man = this.TotalMDamage + random.Next(0, 5);
                this.Mana += man;
                hardcode = true;
            }
            else if (this.skills[which].Type == "Физ.")
            {
                hit = Convert.ToInt32(Math.Round((this.TotalPDamage / 1.5) + this.skills[which].Damage + random.Next(0, 5)));
                def = opponent.TotalDefense + random.Next(0, 5);
            }
            else
            {
                hit = Convert.ToInt32(Math.Round((this.TotalMDamage / 1.5) + this.skills[which].Damage + random.Next(0, 5)));
                def = opponent.TotalDefense + random.Next(0, 5);
            }
            
            int totaldamage = hit - def;

            if (totaldamage > 0)
            {
                opponent.Health -= totaldamage;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(this.Name);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(" использует ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(this.skills[which].Name);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"! \nНавык нанес ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(totaldamage);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($" урона по ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(opponent.Name);
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (hardcode)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{this.Name} восстановил {man} маны!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Скилл не прошел по {opponent.Name}!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            return true;
        }
        public bool CheckSkills(Person opponent)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Навыки:");
            int i = 1;
            foreach (Skill skill in skills)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(i + " ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(skill.Name);
                i++;
            }

            Console.WriteLine("Напишите '0' чтобы вернуться");
            string x = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(x) || x == "0") 
            { 

                return true; 
            }
            bool y = int.TryParse(x, out int a);
            if (!y)
            {
                Console.WriteLine("Введите число, а не что попало");
                Console.ReadKey();
                return true;
            }
            if (a <= 0 || a > skills.Count)
            {
                Console.WriteLine("Такого скилла не существует");
                Console.ReadKey();
                return true;
            }
            if (UseSkill(a - 1, opponent))
                return false;
            else
                return true;
        }
        

        public Person(string name)
        {
                Name = name;
                Lvl = 1;
                health = 100;
                MaxHealth = Health;
                mana = 30;
                MaxMana = mana;
                stamina = 3;
                P_damage = 20;
                M_damage = 20;
                Defense = 5;
                xp = 0;
                money = 0;
        }
        public Person(string name, int lvl)
        {
            Name = name;
            Lvl = lvl;
            health = 100;
            MaxHealth = Health;
            mana = 9999;
            MaxMana = mana;
            stamina = 3;
            P_damage = 20;
            M_damage = 20;
            Defense = 5;
            xp = 0;
            money = 0;
        }
        public Person(string name, int lvl, int health, int mana, int stamina, int p_damage, int m_damage, int defense)
        {
            Name = name;
            Lvl = lvl;
            this.health = health;
            MaxHealth = health;
            this.mana = mana;
            MaxMana = mana;
            Stamina = stamina;
            P_damage = p_damage;
            M_damage = m_damage;
            Defense = defense;
            xp = 0;
            money = 0;
        }
        public virtual string ShowGear(int a) { return ""; }
        public virtual string ShowWeapon() { return ""; }
        public virtual string ShowArmor() { return ""; }
        public Armor armor = Armor.none;
        public Weapon weapon = Weapon.none;
        public enum Armor
        {
            none = 0,
            a = 2,
            b = 4,
            c = 7
        }
        public enum Weapon
        {
            none = 0,
            a = 2,
            b = 4,
            c = 7
        }
        public void RandomAttack(Person opponent)
        {
            int a = random.Next(1, 5);
            if (a <= 3)
            {
                Attack(opponent);
            }
            else
            {
                if (!UseSkill(random.Next(0, skills.Count), opponent))
                    Attack(opponent);
            }

        }
    }
    
    
    
    public class Skill
    {
        string name;
        string type;
        int manacost;
        int damage;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Type
        {
            get { return type; }
            set {
                if ((value.ToLower() == "физ") || (value.ToLower() == "ф"))
                    type = "Физ.";
                else if ((value.ToLower() == "маг") || (value.ToLower() == "м"))
                    type = "Маг.";
                else
                    type = "-";
            }
        }
        public int Manacost
        {
            get { return manacost; }
            set { 
                if (value < 0)
                {
                    manacost = 0;
                }
                else
                    manacost = value; 
            }
        }
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public Skill(string name, string type, int manacost, int damage)
        {
            Name = name;
            Type = type;
            Manacost = manacost;
            Damage = damage;
        }

       

    }
}
