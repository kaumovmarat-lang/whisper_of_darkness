using ClassLibrary1;
using Spectre.Console;
using System.Media;
using System.Numerics;
using System.Xml.Linq;
using рпг;

Console.WriteLine("Инструкция:\nУправление:\n\tВ основном путем ввода чисел\n\tХодьба - стрелочки/WASD\n\tВзаимодействие - f\n! РАЗВЕРНИТЕ ИГРУ ПОЛНОСТЬЮ ЧЕРЕЗ КВАДРАТИК СПРАВА СВЕРХУ !");
Console.ReadKey();
//МУЗЫКА
SoundPlayer ChaosMusic = new SoundPlayer("Sound/chaos.wav");
SoundPlayer Ripple = new SoundPlayer("Sound/ripple.wav");
SoundPlayer FightMusic = new SoundPlayer("Sound/fightmusic.wav");
SoundPlayer FriendTheme = new SoundPlayer("Sound/friend.wav");
SoundPlayer Z = new SoundPlayer("Sound/vlyat.wav");
SoundPlayer victory = new SoundPlayer("Sound/victory.wav");
SoundPlayer guild = new SoundPlayer("Sound/guild.wav");
SoundPlayer maintheme = new SoundPlayer("Sound/main_theme.wav");
Ripple.PlayLooping();

// СПРАЙТЫ
// интерьер
var empty = new CanvasImage("Sprites/empty.png");
var door = new CanvasImage("Sprites/door.png");
var hand = new CanvasImage("Sprites/hand.png");
var friend = new CanvasImage("Sprites/friend.png");
var friendPreDark = new CanvasImage("Sprites/friend3.png");
var friendDark = new CanvasImage("Sprites/friend2.png");
var girl = new CanvasImage("Sprites/girl.png");
var balance = new CanvasImage("Sprites/balance.png");
var story = new CanvasImage("Sprites/story.png");
var castle = new CanvasImage("Sprites/castle.png");

//враги
var eyes = new CanvasImage("Sprites/eyes.png");

var CatImage = new CanvasImage("Sprites/cat.png");
var CatImage2 = new CanvasImage("Sprites/cat2.png");

var LampImage = new CanvasImage("Sprites/lamp.png");
var LampImage2 = new CanvasImage("Sprites/lamp2.png");

var GuardImage = new CanvasImage("Sprites/guard.png");
var GuardImage2 = new CanvasImage("Sprites/guard2.png");

var ClockImage = new CanvasImage("Sprites/clock.png");
var ClockImage2 = new CanvasImage("Sprites/clock2.png");

var FacelessImage2 = new CanvasImage("Sprites/faceless2.png");

// древо навыков
var mag1lvl = new CanvasImage("Sprites/treemag1lvl.png");
var mag2lvl = new CanvasImage("Sprites/treemag2lvl.png");
var mag3lvl = new CanvasImage("Sprites/treemag3lvl.png");
var knight1lvl = new CanvasImage("Sprites/treeknight1lvl.png");
var knight2lvl = new CanvasImage("Sprites/treeknight2lvl.png");
var knight3lvl = new CanvasImage("Sprites/treeknight3lvl.png");
var berserk1lvl = new CanvasImage("Sprites/treeberserk1lvl.png");
var berserk2lvl = new CanvasImage("Sprites/treeberserk2lvl.png");
var berserk3lvl = new CanvasImage("Sprites/treeberserk3lvl.png");

// НАВЫКИ ИГРОКА
int[] needLvl = { 1, 3, 5 };

Skill ManaRegain = new Skill("Восстановить ману", "м", 0, 0);
// маг
Skill IceCrystal = new Skill("Ледяной кристалл", "м", 15, 15);
Skill Fireball = new Skill("Огненный снаряд", "м", 20, 30);
// рыцарь
Skill Whirlwind = new Skill("Вихрь", "ф", 5, 15);
Skill Cleave = new Skill("Раскол", "ф", 15, 20);
// берсерк
Skill Execute = new Skill("Казнь", "ф", 6, 20);
Skill BloodRage = new Skill("Кровавая ярость", "ф", 18, 35);

// конструктор для создания врагов
Enemy MakeEnemy(string name, int lvl, int xp, int hp = 0, int atk = 0, Enemy.Armor armor = Enemy.Armor.none, Enemy.Weapon weapon = Enemy.Weapon.none)
{
    Enemy e = new Enemy(name, 1, xp);
    for (int i = 1; i < lvl; i++) e.increase_lvl();
    e.MaxHealth += hp;
    e.Health = e.MaxHealth;
    e.P_damage += atk;
    e.M_damage += atk;
    e.armor = armor;         
    e.weapon = weapon;
    return e;
}

Enemy eyesEnemy = MakeEnemy("Глаза", 1, 1);
eyesEnemy.LearnSkill(new Skill("Зрительный контакт", "м", 0, 15), 1);

Enemy cat = MakeEnemy("Искаженная кошка", 2, 2, 0, 4);
cat.LearnSkill(new Skill("Когти", "ф", 0, 15), 1);
cat.LearnSkill(new Skill("Прыжок из тени", "ф", 0, 20), 1);

Enemy lamp = MakeEnemy("Погасший фонарь", 3, 4, 45, 6);
lamp.LearnSkill(new Skill("Вспышка тьмы", "м", 0, 20), 1);
lamp.LearnSkill(new Skill("Падение столба", "ф", 0, 30), 1);

Enemy guard = MakeEnemy("Искаженный стражник", 3, 6, 150, 6);
guard.LearnSkill(new Skill("Удар алебардой", "ф", 0, 30), 1);
guard.LearnSkill(new Skill("Приказ тьмы", "м", 0, 25), 1);

Enemy clock = MakeEnemy("Древние часы", 4, 8, 210, 9);
clock.LearnSkill(new Skill("Бой курантов", "м", 0, 35), 1);
clock.LearnSkill(new Skill("Чёрные стрелки", "ф", 0, 30), 1);

Enemy faceless = MakeEnemy("Безликий", 5, 10, 210, 6);
faceless.LearnSkill(new Skill("Украсть имя", "м", 0, 40), 1);
faceless.LearnSkill(new Skill("Прикосновение пустоты", "ф", 0, 45), 1);

//порядок вылазок
List<Foe> Dark = new List<Foe>
{
    //враг, спрайт искаженный, спрайт норм, с какого дня, рек уровень, монеты, текст после победы
    new Foe(cat, CatImage2, CatImage, 2, 2, 10, new string[]
    {
        "Тьма отпускает кошку, и она замирает у ваших ног.",
        "На ошейнике бирка: «Мурка». Бабушка в гильдии будет рада."
    }),
    new Foe(lamp, LampImage2, LampImage, 2, 3, 15, new string[]
    {
        "Фонарь гаснет. Впервые за долгое время вокруг становится чуть светлее."
    }),
    new Foe(guard, GuardImage2, GuardImage, 3, 3, 20, new string[]
    {
        "Стражник оседает на колени. На секунду в его глазах мелькает человек.",
        "«Спасибо... я так долго держал пост...»"
    }),
    new Foe(clock, ClockImage2, ClockImage, 4, 4, 25, new string[]
    {
        "Чёрные стрелки замирают на полуночи.",
        "Время снова идёт."
    }),
    new Foe(faceless, FacelessImage2, empty, 5, 4, 30, new string[0]),   //финал
};

//ЗАДАНИЯ И МАГАЗ
Mission task1 = new Mission("Прокачка", "Прокачай свой уровень до трех", 50, 5);
Mission task2 = new Mission("Потеряшка", "Бабушка потеряла кошку, найди её", 100, 1);
Mission task3= new Mission("Мастер", "Выучи все навыки", 150, 10);
Mission[] missions = { task1, task2, task3 };


string[] survivorLines =
{
    "\\ сначала погасли фонари, потом окна, а дальше и лица... /",
    "\\ говорят, что они могут притворяться близкими и даже говорить их голосами! /",
    "\\ стражи города тоже сражались с тьмой ту ночь и что с ними стало? нам всем конец.. /",
    "\\ парень, что приводит новичков, не спит уже несколько ночей! говорят, что ночью он перешептывается сам с собой.. /",
    "\\ животные меняются первыми.. может, они чувствуют это раньше нас? /",
    "\\ знаешь, что страшнее всего? те, кого тьма тронула, выглядят счастливыми /",
    "\\ часы в городе встали за одну ночь. все. одновременно. /",
    "\\ безликий не выдумка! я правда его видела, в окне, в ту самую ночь! /",
};

//настройка перед началом
Random random = new Random();
int days = 1;
int cleared = 0;                 //сколько врагов побеждено
bool friendJoined = false;
bool finaleDone = false;

Knight friendChar = new Knight("Друг"); //друг         
friendChar.LearnSkill(Whirlwind, 1);
friendChar.LearnSkill(Cleave, 1);

char[,] map1 = {
    { '1', '1', '1', '1', '1', '1', '1', },
    { '1', '0', '0', '0', '0', '0', '1', },
    { '1', '0', '0', '0', '0', '0', '1', },
    { '1', '0', '0', '0', '0', '0', '1', },
    { '1', '0', '0', '0', '0', '0', '1', },
    { '1', '0', '0', '0', '0', '0', '1', },
    { '1', '1', '1', '1', '1', '1', '1', },
};

//МЕЙН ПРОГА ТУТ!!!
Person Mplayer = prologue();
Predmet[] shopItems =
{
    new Potion("Зелье лечения", 30, 60, 0),
    new Potion("Эликсир маны", 25, 0, 40),
    new Predmet(Mplayer.ShowGear(1), 50),
    new Predmet(Mplayer.ShowGear(2), 100),
    new Predmet(Mplayer.ShowGear(3), 150),
    new Predmet(Mplayer.ShowGear(4), 50),
    new Predmet(Mplayer.ShowGear(5), 100),
    new Predmet(Mplayer.ShowGear(6), 150),
};
while (!finaleDone)
{
    DayStart();
    MainMap(Mplayer);
    if (finaleDone) break;
    maintheme.Stop();
    Console.Clear();
    MText.Center("Вы устали и пошли спать");
    days++;
    next();
}

//начало дня
void DayStart()
{
    Mplayer.Stamina = Mplayer.MaxStamina;
    Mplayer.Health = Mplayer.MaxHealth;
    Mplayer.Mana = Mplayer.MaxMana;

    if (days == 3)
    {
        friendJoined = true;
    }
    if (friendJoined) SyncFriend();

    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Yellow;
    MText.Center("ДЕНЬ: " + days);
    next();
    DayEvent();
    maintheme.PlayLooping();
}

void DayEvent()
{
    if (days == 2)
    {
        MText.Center("Утро. Гильдия понемногу привыкает к вам");
        MText.Center("Пора вам выходить на вылазки");
        next();
    }
    else if (days == 3)
    {
        Speak(friend, ConsoleColor.Blue,
            "я иду с вами, вдвоем мы справимся быстрее!");
        MText.Center("Друг присоединяется к вашему отряду");
        next();
    }
    else if (days == 4)
    {
        Speak(friendPreDark, ConsoleColor.Blue, "не смотри так, просто рука затекла..");
        MText.Center("друг пытается скрыть свою руку, прикрывая ее одеждой");
        next();
    }
    else if (days == 5)
    {
        Speak(friendPreDark, ConsoleColor.Blue,
            "сегодня мы закончим это. я чувствую, мы совсем близко.",
            "ты ведь тоже слышишь их?..");
        next();
    }
}


void SyncFriend() //синхронизация друга к гг 
{
    int needlvl = Math.Max(3, Mplayer.Lvl + 1);
    for (int i = friendChar.Lvl; i < needlvl; i++)
        friendChar.increase_lvl();
    friendChar.Health = friendChar.MaxHealth;
    friendChar.Mana = friendChar.MaxMana;
}

void MainMap(Person p) //менюшка
{
    while (p.Stamina > 0 && !finaleDone)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("======================");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"{p.Name}  |  Класс: {type(p)}  |  Ур: {p.Lvl}  (опыт {p.xp}/{p.Lvl * 5})");
        Console.WriteLine($"Здоровье: {p.Health}/{p.MaxHealth}  |  Мана: {p.Mana}/{p.MaxMana}  |  Деньги: {p.money}");
        Console.WriteLine($"Оружие: {p.ShowWeapon()} (Бонус: {(int)p.weapon})  |  Броня: {p.ShowArmor()} (Бонус: {(int)p.armor})");
        Console.WriteLine($"Стамина: {p.Stamina}/{p.MaxStamina}");
        if (friendJoined)
            Console.WriteLine($"{friendChar.Name}: {friendChar.Health}/{friendChar.MaxHealth} хп, ур. {friendChar.Lvl}");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("======================");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"День: {days}");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("======================");
        AnsiConsole.Write(Align.Center(castle));

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("1. Гильдия");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("2. Очистка тьмы (1 стамина)");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("3. Тренировка (1 стамина)");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("4. Древо навыков");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("5. Поговорить с выжившими (1 стамина)");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Выберите действие: ");
        string? action = Console.ReadLine();
        switch (action)
        {
            case "1":
                GuildScene();
                break;
            case "2":
                Cleansing();
                break;
            case "3":
                Console.Clear();
                MText.Center("Вы отправляетесь на тренировку");
                MText.Center("Будет выведен список цифр от 1 до 9");
                MText.Center("Вам нужно будет поймать Х и нажать на нужную цифру, пока не истекло время!");
                next();
                minigame.train(p);
                next();
                break;
            case "4":
                Console.Clear();
                tree(p);
                Console.Clear();
                break;
            case "5":
                Survivors();
                break;
            default:
                break;
        }
    }
}

//гильдия
void GuildScene()
{
    guild.PlayLooping();
    while (true)
    {
        if (Mplayer.Lvl >= 3) task1.complete = true;
        if (cleared >= 1) task2.complete = true;
        if (Mplayer.skills.Count == 3) task3.complete = true;

        Console.Clear();
        AnsiConsole.Write(Align.Center(girl));
        Console.ForegroundColor = ConsoleColor.Magenta;
        MText.Center("Добро пожаловать в гильдию");
        Console.ForegroundColor = ConsoleColor.White;
        MText.Center($"Деньги: {Mplayer.money}");
        Console.WriteLine();
        task1.CheckMission();
        task2.CheckMission();
        task3.CheckMission();
        Console.WriteLine();
        Option(1, "Сдать выполненные задания");
        Option(2, "Лавка");
        Option(0, "Выйти");

        string answer = Console.ReadLine();
        if (answer == "0") break;
        if (answer == "1")
        {
            bool any = false;
            foreach (Mission t in missions)
            {
                if (t.complete && !t.rewarded)
                {
                    t.Reward(Mplayer);
                    any = true;
                }
            }
            if (!any)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                MText.Center("Вы не выполнили миссии");
            }
            next();
        }
        else if (answer == "2")
        {
            Shop();
        }
    }
    maintheme.PlayLooping();
}



void Shop()
{
    while (true)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        MText.Center("Лавка гильдии");
        Console.ForegroundColor = ConsoleColor.White;
        MText.Center($"Деньги: {Mplayer.money}");
        Console.WriteLine();
        for (int i = 0; i < shopItems.Length; i++)
        {
            Option(i + 1, $"{shopItems[i].Name}: {shopItems[i].Price} монет");
        }
        Option(0, "Назад");
        Console.Write("Что купить?: ");
        int choice = baka();
        if (choice == 0) return;
        if (choice < 1 || choice > shopItems.Length)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Такого товара нет!");
        }
        else if (Mplayer.money < shopItems[choice - 1].Price)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Не хватает денег!");
        }
        else
        {
            bool hardcode2 = true;
            if (choice >= 3)
            {
                switch (choice)
                {
                    case 3:
                        if ((int)Mplayer.armor >= 2)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Твоя броня лучше чем {Mplayer.ShowGear(1)}! (либо такая же)");
                            hardcode2 = false;
                        }
                        else Mplayer.armor = Person.Armor.a;
                        break;
                    case 4:
                        if ((int)Mplayer.armor >= 4)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Твоя броня лучше чем {Mplayer.ShowGear(2)}! (либо такая же)");
                            hardcode2 = false;
                        }
                        else Mplayer.armor = Person.Armor.b;
                        break;
                    case 5:
                        if ((int)Mplayer.armor == 7)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("У тебя итак имеется эта броня");
                            hardcode2 = false;
                        }
                        else Mplayer.armor = Person.Armor.c;
                        break;
                    case 6:
                        if ((int)Mplayer.weapon >= 2)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Твое оружие лучше чем {Mplayer.ShowGear(4)}! (либо такое же)");
                            hardcode2 = false;
                        }
                        else Mplayer.weapon = Person.Weapon.a;
                        break;
                    case 7:
                        if ((int)Mplayer.weapon >= 4)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Твое оружие лучше чем {Mplayer.ShowGear(5)}! (либо такое же)");
                            hardcode2 = false;
                        }
                        else Mplayer.weapon = Person.Weapon.b;
                        break;
                    case 8:
                        if ((int)Mplayer.weapon == 7)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("У тебя итак имеется это оружие");
                            hardcode2 = false;
                        }
                        else Mplayer.weapon = Person.Weapon.c;
                        break;
                }
            }
            else
                Mplayer.inventory.Add(shopItems[choice - 1]);
            if (hardcode2)
            {
                Mplayer.money -= shopItems[choice - 1].Price;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Вы купили: {shopItems[choice - 1].Name}");
            }
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.ReadKey(true);
    }
}

void Survivors()
{
    Mplayer.Stamina -= 1;
    Console.Clear();
    MText.Center("Вы подсаживаетесь к выжившим у костра");
    Console.ForegroundColor = ConsoleColor.Cyan;
    int vars = random.Next(0, survivorLines.Length);
    MText.Center(survivorLines[vars]);
    next();
}

//вылазки
void Cleansing()
{
    Console.Clear();
    if (days == 1)
    {
        MText.Center("У вас не хватает смелости идти в первый день");
        next();
        return;
    }
    if (cleared >= Dark.Count) return;

    Foe foe = Dark[cleared];
    if (days < foe.MinDay)
    {
        MText.Center("Дальше идти опасно: тьма в тех краях ещё не двинулась.");
        MText.Center("Тренируйтесь и возвращайтесь завтра.");
        next();
        return;
    }
    if (Mplayer.Health <= 0)
    {
        MText.Center("Вы слишком измотаны для вылазки. Нужен отдых до завтра.");
        next();
        return;
    }

    MText.Center($"Впереди: {foe.Who.Name}");
    MText.Center($"Рекомендуемый уровень: {foe.RecLvl}  (у вас {Mplayer.Lvl})");
    Console.WriteLine();
    Option(1, "Идти");
    Option(2, "Вернуться");
    if (Console.ReadLine() != "1") return;

    Mplayer.Stamina -= 1;
    string result = fight(Mplayer, foe.Who, foe.ImageBattle, foe.ImageSaved);
    if (result == "win")
    {
        Mplayer.money += foe.Money;
        Console.ForegroundColor = ConsoleColor.Yellow;
        MText.Center($"Найдено монет: {foe.Money}");
        Console.ForegroundColor = ConsoleColor.White;
        foreach (string line in foe.After) MText.Center(line);
        next();
        cleared++;
        if (cleared >= Dark.Count) Finale();
    }
    else
    {
        Console.Clear();
        MText.Center("Вы падаете от усталости, глаза закрываются...");
        MText.Center("Очнуться вы успеете только в гильдии.");
        next();
    }
    if (!finaleDone) maintheme.PlayLooping();
}

//Финао
void Finale()
{
    FightMusic.Stop();
    Console.Clear();
    MText.Center("Безликий падает. На его маске на миг проступают черты и исчезают.");
    next();
    Speak(friendPreDark, ConsoleColor.Blue, "Мы справились... Слышишь? Тишина.");
    MText.Center("Но тьма, вырвавшаяся из Безликого, никуда не исчезла.");
    MText.Center("Черный дым тянется к вашему другу.");
    next();
    Speak(friendPreDark, ConsoleColor.Blue, "Нет... Что происходит?!");
    ChaosMusic.PlayLooping();

    Speak(friendDark, ConsoleColor.DarkMagenta,
        "...",
        "Теперь я понял.",
        "Мы сражались зря. Тьма не пожирает мир, она помогает ему жить.");
    Speak(friendDark, ConsoleColor.DarkMagenta,
        "Посмотри на мою руку. Никакой боли. Никаких сомнений. Я наконец то целый.",
        "Тьма это следующая ступень эволюции. Само совершенство.");
    Speak(friendDark, ConsoleColor.DarkMagenta,
        "Ты не помнишь, как сюда попал? Она знает. Протяни руку, и вспомнишь.",
        "Брось оружие. Мы не должны сражаться, мы должны примкнуть.");

    Console.Clear();
    AnsiConsole.Write(Align.Right(friendDark));
    Console.ForegroundColor = ConsoleColor.Yellow;
    Option(1, "Прекрати это безумие!");
    Option(2, "Может ты и прав.");
    if (Console.ReadLine() == "2")
    {
        BadEnding();
        return;
    }

    while (true)
    {
        Mplayer.Health = Mplayer.MaxHealth;
        Mplayer.Mana = Mplayer.MaxMana;
        Enemy boss = MakeEnemy("Друг", 5, 0, 150, 15, Enemy.Armor.c, Enemy.Weapon.c);
        boss.LearnSkill(new Skill("Клинок тьмы", "ф", 0, 35), 1);
        boss.LearnSkill(new Skill("Шепот тьмы", "м", 0, 30), 1);
        friendJoined = false;
        string result = fight(Mplayer, boss, friendDark, friend, true); 

        if (result == "win") break;

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        MText.Center("Тьма смыкается над вами...");
        Console.WriteLine();
        Option(1, "Подняться и продолжить бой");
        Option(2, "Сдаться");
        if (Console.ReadLine() == "2")
        {
            BadEnding();
            return;
        }
    }

    GoodEnding();
}

void GoodEnding()
{
    ChaosMusic.Stop();
    FriendTheme.PlayLooping();
    Console.Clear();
    MText.Center("Друг падает на колено. Чёрные трещины на его руках гаснут.");
    MText.Center("На секунду его глаза становятся прежними, зелёными.");
    next();
    Speak(friend, ConsoleColor.Blue,
        "Я... я здесь?..",
        "Прости. Она шептала так тихо, а потом так громко...",
        "Спасибо, что не отступил.");
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.White;
    MText.Center("Тьма отступает. Не везде и не навсегда, но здесь становится светлее.");
    MText.Center("Гильдия выстоит.");
    next();
    MText.Center("Вы так и не узнали, как попали в этот мир.");
    MText.Center("Но теперь это неважно: у вас есть имя, гильдия и друг.");
    next();
    TheEnd("КОНЕЦ");
}

void BadEnding()
{
    ChaosMusic.PlayLooping();
    Speak(friendDark, ConsoleColor.DarkMagenta,
        "Я рад, что ты все осознал.",
        "Сейчас ты всё вспомнишь. Всё, что хочет показать тебе тьма.");
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.DarkMagenta;
    MText.Center("Тишина. Ни страха, ни вопросов.");
    MText.Center("Вы впервые за долгое время чувствуете себя целым.");
    next();
    MText.Center("Гильдия ещё долго искала вас.");
    MText.Center("Но нашла только тьму, которая улыбалась знакомыми лицами.");
    next();
    TheEnd("КОНЕЦ. Плохая концовка");
}

void TheEnd(string title)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Yellow;
    MText.Center(title);
    Console.ForegroundColor = ConsoleColor.White;
    MText.Center("Спасибо за игру!");
    Console.WriteLine();
    MText.Center("(Нажмите любую клавишу, чтобы выйти)");
    Console.ReadKey(true);
    finaleDone = true;
}


void Speak(CanvasImage image, ConsoleColor color, params string[] lines)
{
    Console.Clear();
    AnsiConsole.Write(Align.Right(image));
    Console.ForegroundColor = color;
    foreach (string line in lines) MText.Right(line);
    next();
}

//БОЙ
string fight(Person player, Enemy enemy, CanvasImage pic, CanvasImage saved, bool isboss = false)
{
    Console.Clear();
    if (!isboss)
        FightMusic.PlayLooping();
    enemy.Health = enemy.MaxHealth;
    while (player.Health > 0 && enemy.Health > 0)
    {
        bool hardcode = false;
        AnsiConsole.Write(Align.Center(pic));
        MText.Center("=========================================================");
        Console.ForegroundColor = ConsoleColor.Red;
        MText.Center($"{enemy.Name} - {enemy.Health}/{enemy.MaxHealth} Здоровья");
        Console.ForegroundColor = ConsoleColor.White;
        MText.Center("=========================================================");
        MText.Center(player.Name);
        Console.ForegroundColor = ConsoleColor.Green;
        MText.Center($"{player.Health}/{player.MaxHealth} Здоровья");
        Console.ForegroundColor = ConsoleColor.Cyan;
        MText.Center($"{player.Mana}/{player.MaxMana} Маны");
        Console.ForegroundColor = ConsoleColor.White;
        MText.Center("=========================================================");
        if (friendJoined)
        {
            MText.Center("Друг");
            Console.ForegroundColor = ConsoleColor.Green;
            MText.Center($"{friendChar.Health}/{friendChar.MaxHealth} Здоровья");
            Console.ForegroundColor = ConsoleColor.Cyan;
            MText.Center($"{friendChar.Mana}/{friendChar.MaxMana} Маны");
            Console.ForegroundColor = ConsoleColor.White;
            MText.Center("=========================================================");
        }
        Console.ForegroundColor = ConsoleColor.Yellow;
        MText.Center("Выберите действие");
        Console.ForegroundColor = ConsoleColor.White;
        MText.Center("===========================");
        Console.ForegroundColor = ConsoleColor.White;
        MText.Center("1. Атака");
        MText.Center("2. Навык");
        MText.Center("3. Предмет");
        Console.ForegroundColor = ConsoleColor.White;
        MText.Center("===========================");
        switch (Console.ReadLine())
        {
            case "1":
                player.Attack(enemy);
                break;
            case "2":
                if (player.skills.Count <= 0)
                {
                    MText.Center("У вас нет навыков!");
                    hardcode = true;
                    break;
                }
                else
                {
                    hardcode = player.CheckSkills(enemy);
                    break;
                }
            case "3":
                if (player.inventory.Count <= 0)
                {
                    MText.Center("У вас нет предметов!");
                    hardcode = true;
                    break;
                }
                else
                {
                    for (int i = 0; i < player.inventory.Count; i++)
                        Option(i + 1, player.inventory[i].Name);

                    int choice = baka();

                    if (choice == 0)
                    {
                        hardcode = true;
                        break;
                    }

                    if (choice < 1 || choice > player.inventory.Count)
                    {
                        hardcode = true;
                        break;
                    }

                    Potion potion = (Potion)player.inventory[choice - 1];
                    player.inventory.RemoveAt(choice - 1);
                    player.Health += potion.Heal;
                    player.Mana += potion.Mana;
                    break;
                }
            default:
                MText.Center("Введите действие(цифра) без ошибок!");
                hardcode = true;
                break;
        }
        if (enemy.Health <= 0) break;
        if (!hardcode)
        {
            Thread.Sleep(200);
            if (friendJoined && friendChar.Health > 0)
            {
                friendChar.RandomAttack(enemy);
                Thread.Sleep(200);
                bool l = random.Next(2) == 1;
                if (l)
                    enemy.Attack(player);
                else
                    enemy.Attack(friendChar);

            }
            else
                enemy.Attack(player);
        }
        next();
    }
    if (player.Health <= 0) return "lost";
    else
    {
        Console.Clear();
        victory.Play();
        AnsiConsole.Write(Align.Center(saved));
        Console.ForegroundColor = ConsoleColor.Green;
        MText.Center($"Вы одержали победу и очистили тьму!");
        player.EarnXp(enemy.xpEarnEnemy);
        next();
        return "win";
    }


}

//карта в прологе
void mapcheck(int Mapnumber, int shir, int vis, char[,] map, int xp, int yp, Person p)
{
    bool switch1 = false;
    char chname = p.Name[0];
    while (true)
    {
        Console.Clear();
        for (int j = 0; j < map.GetLength(1); j++)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                if (i == xp && j == yp)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(chname + " ");
                }
                else
                {
                    Console.ForegroundColor = (i == 0 && j == 4) ? ConsoleColor.Yellow : ConsoleColor.White;
                    Console.Write(map[i, j] + " ");
                }
            }
            Console.WriteLine();
        }

        char a = char.ToLower(Console.ReadKey(true).KeyChar);
        switch (a)
        {
            case 'w':
            case 'ц':
                if (yp > 1) yp--;
                break;
            case 'a':
            case 'ф':
                if (xp > 1) xp--;
                break;
            case 's':
            case 'ы':
                if (yp < vis) yp++;
                break;
            case 'd':
            case 'в':
                if (xp < shir) xp++;
                break;
            case 'f':
            case 'а':
                if (Mapnumber == 1)
                {
                    if (xp == 1 && yp == 4)
                    {
                        Console.WriteLine("Вы нажали на кнопку");
                        map[3, 0] = '?';
                        switch1 = true;
                        Console.ReadKey(true);
                    }
                    else if (xp == 3 && yp == 1 && switch1)
                    {
                        return;
                    }
                }
                break;
        }
    }
}

//методы для всяких хреней
string type(Person p)
{
    if (p.GetType().ToString() == "ClassLibrary1.Mage")
    {
        return "Маг";
    }
    else if (p.GetType().ToString() == "ClassLibrary1.Berserk")
    {
        return "Берсерк";
    }
    else if (p.GetType().ToString() == "ClassLibrary1.Knight")
    {
        return "Рыцарь";
    }
    else
    {
        return "Человек";
    }
}

void next()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    MText.Center("(Нажмите, чтобы продолжить)");
    Console.ForegroundColor = ConsoleColor.White;
    Console.ReadKey(true);
    Console.Clear();
}

int baka() //конвертер для инта, мне название бака уже прижилось
{
    while (true)
    {
        bool x = int.TryParse(Console.ReadLine(), out int a);
        if (!x)
        {
            Console.WriteLine("Введите число");
            continue;
        }
        return a;
    }
}

void Option(int n, string text)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write($"{n}. ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(text);
}

//ДРЕВО НАВЫКОВ
void tree(Person p)
{
    while (true)
    {
        Console.Clear();
        switch (type(p))
        {
            case "Маг":
                if (p.Lvl <= needLvl[1]-1)
                {
                    AnsiConsole.Write(Align.Center(mag1lvl));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("1. ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Восстановить ману");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("2. ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Ледянной кристал");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("3. ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Огненный снаряд");
                }
                else if (p.Lvl <= needLvl[2]-1)
                {
                    AnsiConsole.Write(Align.Center(mag2lvl));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("1. ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Восстановить ману");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("2. ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Ледянной кристал");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("3. ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Огненный снаряд");
                }
                else if (p.Lvl >= needLvl[2])
                {
                    AnsiConsole.Write(Align.Center(mag3lvl));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("1. ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Восстановить ману");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("2. ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Ледянной кристал");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("3. ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Огненный снаряд");
                }

                break;
            case "Рыцарь":
                if (p.Lvl <= needLvl[1]-1)
                {
                    AnsiConsole.Write(Align.Center(knight1lvl));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("1. ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Восстановить ману");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("2. ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Вихрь");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("3. ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Раскол");
                }
                else if (p.Lvl <= needLvl[2]-1)
                {
                    AnsiConsole.Write(Align.Center(knight2lvl));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("1. ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Восстановить ману");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("2. ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Вихрь");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("3. ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Раскол");
                }
                else if (p.Lvl >= needLvl[2])
                {
                    AnsiConsole.Write(Align.Center(knight3lvl));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("1. ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Восстановить ману");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("2. ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Вихрь");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("3. ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Раскол");
                }
                break;
            case "Берсерк":
                if (p.Lvl <= needLvl[1]-1)
                {
                    AnsiConsole.Write(Align.Center(berserk1lvl));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("1. ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Восстановить ману");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("2. ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Казнь");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("3. ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Кровавая ярость");
                }
                else if (p.Lvl <= needLvl[2] - 1)
                {
                    AnsiConsole.Write(Align.Center(berserk2lvl));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("1. ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Восстановить ману");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("2. ");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Казнь");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("3. ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Кровавая ярость");
                }
                else if (p.Lvl >= needLvl[2])
                {
                    AnsiConsole.Write(Align.Center(berserk3lvl));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("1. ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Восстановить ману");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("2. ");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Казнь");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("3. ");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Кровавая ярость");
                }

                break;
            default:
                break;
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("(Напишите 0, чтобы выйти)");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Какой навык хотите выучить?: ");
        Console.ForegroundColor = ConsoleColor.White;
        int choice = baka();
        if (choice == 0)
        {
            break;
        }
        if ((choice > 3) || choice < 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Такого навыка не существует!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
            continue;
        }
        else if (p.Lvl < needLvl[choice-1])
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Вы недостаточно прокачены для данного навыка!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
            continue;
        }
        switch (choice)
        {
            case 1:
                p.LearnSkill(ManaRegain);
                break;
            case 2:
                if (type(p) == "Маг")
                    p.LearnSkill(IceCrystal);
                else if (type(p) == "Рыцарь")
                    p.LearnSkill(Whirlwind);
                else
                    p.LearnSkill(Execute);
                break;
            case 3:
                if (type(p) == "Маг")
                    p.LearnSkill(Fireball);
                else if (type(p) == "Рыцарь")
                    p.LearnSkill(Cleave);
                else
                    p.LearnSkill(BloodRage);
                break;
        }
    }
}
    //пролог
    Person prologue()
{
    Console.Clear();
    MText.Center("Вы просыпаетесь и не понимаете, где находитесь");
    MText.Center("вокруг сплошная тьма");
    next();
    AnsiConsole.Write(Align.Center(hand));
    MText.Center("Вы пытаетесь вспомнить свое имя...");
    MText.Center("Как вас звали?");

    Console.Write(new string(' ', Math.Max(0, Console.WindowWidth / 2 - 7)));

    string name = (Console.ReadLine() ?? "").Trim();
    if (string.IsNullOrWhiteSpace(name))
        name = "Безымянный";
    if (name.ToLower() == "ролан")
    {
        Console.Clear() ;
        MText.Center("Пошел нахуй ролан");
        Thread.Sleep(1000);
        Environment.Exit(0);
    }
    else if (name.ToLower() == "зус")
    {
        Console.Clear();
        MText.Center("иди нахуй зус");
        Z.Play();
        Thread.Sleep(1000);
        Environment.Exit(0);
    }

    Person player = new Person(name);
    for (int x5 = 0; x5 < 2; x5++)
    {
        player.increase_lvl();
    }
    Console.Clear();
    AnsiConsole.Write(Align.Center(hand));
    MText.Center($"Точно, вы {name}..");
    MText.Center("Но как вы сюда попали?");
    next();
    mapcheck(1, 5, 5, map1, 3, 3, player);
    Console.Clear();
    AnsiConsole.Write(Align.Center(door));
    MText.Center("Идя на ощупь, вы натыкаетесь на дверь");
    next();
    Console.Clear();
    AnsiConsole.Write(Align.Center(door));
    MText.Center("У вас особо нет выбора, кроме как открыть ее");
    next();
    Ripple.Stop();
    Thread.Sleep(3000);

    string FirstFight = fight(player, eyesEnemy, eyes, empty);
    if (FirstFight != "win")
    {
        Console.Clear();
        MText.Center("Вы падаете от усталости, глаза закрываются...");
        MText.Center("Вы слышите чей-то голос...");
        next();
    }
    FightMusic.Stop();
    FriendTheme.PlayLooping();
    while (true)
    {
        Console.Clear();
        AnsiConsole.Write(Align.Right(friend));
        Console.ForegroundColor = ConsoleColor.Blue;
        MText.Right("все в порядке? Я был неподалеку и услышал как ты противостоял тьме");

        //вопросы
        Option(1, "Где я нахожусь?");
        Option(2, "Кто ты?");
        Option(3, "Что здесь происходит?!");

        string? question = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Blue;
        if (question == "1")
        {
            MText.Right("Тебе трудно будет поверить, но ты дома..");
            MText.Right("Наверное для тебя он сильно преобразился");
            break;
        }
        else if (question == "2")
        {
            MText.Right("Я из гильдии по очищению тьмы, мы ищем таких как ты");
            break;
        }
        else if (question == "3")
        {
            MText.Right("ты ничего не знаешь? Неужели тьма настолько исказила это место");
            break;
        }
    }
    MText.Right("У нас не особо много времени, расскажу тебе обо всём по пути.");
    next();

    Console.ForegroundColor = ConsoleColor.Blue;
    AnsiConsole.Write(Align.Center(balance));
    MText.Center("Раньше в нашем мире свет и тьма жили в гармонии");
    MText.Center("Но несколько дней назад баланс был нарушен.");
    MText.Center("Тьма распространилась по всему миру за несколько часов");
    MText.Center("и исказила предметы, животных и даже людей...");
    next();

    Console.ForegroundColor = ConsoleColor.Blue;
    AnsiConsole.Write(Align.Center(story));
    MText.Center("Те, кто не поддались тьме");
    MText.Center("смогли собраться вместе и образовать гильдию");
    MText.Center("Мы верим, что сможем одолеть тьму");
    MText.Center("и восстановить баланс");
    next();

    MText.Center("Парень проводил вас до гильдии и ушел по делам");
    next();

    FriendTheme.Stop();
    guild.PlayLooping();
    while (true)
    {
        Console.Clear();
        AnsiConsole.Write(Align.Center(girl));
        Console.ForegroundColor = ConsoleColor.Magenta;
        MText.Center("Добрый день, вы хотите вступить в гильдию?");

        //вопросы
        Option(1, "Да");
        Option(2, "Нет");

        string? question = Console.ReadLine();
        if (question == "1")
        {
            break;
        }
        else if (question == "2")
        {
            Console.WriteLine("(И что мне делать тогда? лучше согласиться, не?)");
            Console.ReadKey(true);
        }
    }

    // выбор класса: Confirm спрашивает «все верно?», дальше создаётся нужный класс
    Person? chosen = null;
    while (chosen == null)
    {
        Console.Clear();
        AnsiConsole.Write(Align.Center(girl));
        Console.ForegroundColor = ConsoleColor.Magenta;
        MText.Center("Какой класс желаете выбрать?");
        Option(1, "Рыцарь");
        Option(2, "Берсерк");
        Option(3, "Маг");

        switch (Console.ReadLine())
        {
            case "1": if (Confirm(player.Name, "Рыцарь")) chosen = new Knight(player.Name); break;
            case "2": if (Confirm(player.Name, "Берсерк")) chosen = new Berserk(player.Name); break;
            case "3": if (Confirm(player.Name, "Маг")) chosen = new Mage(player.Name); break;
        }
    }

    Console.Clear();
    AnsiConsole.Write(Align.Center(girl));
    Console.ForegroundColor = ConsoleColor.Magenta;
    MText.Center($"Добро пожаловать в гильдию, {chosen.Name}!");
    MText.Center("Загляните в древо навыков: там вы сможете выучить первые приёмы.");
    next();
    guild.Stop();
    return chosen;
}

bool Confirm(string playerName, string className)
{
    while (true)
    {
        Console.Clear();
        AnsiConsole.Write(Align.Center(girl));
        Console.ForegroundColor = ConsoleColor.Magenta;
        MText.Center("Сейчас зафиксирую информацию...");
        MText.Center($"{playerName}, класс {className}");
        MText.Center("все верно?");
        Option(1, "Да");
        Option(2, "Нет");
        string? answer = Console.ReadLine();
        if (answer == "1") return true;
        if (answer == "2") return false;
    }
}

