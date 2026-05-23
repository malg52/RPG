using System;
using static System.Console;

namespace Zadanie
{
    class Character
    {
        protected int HP;
        protected int Dodges;
        protected int Heals;
        protected int MaxHp;

        public virtual int Attack()
        {
            Random rnd = new Random();
            int dam = rnd.Next(10, 30);
            return dam;
        }

        public virtual int Heal()
        {
            return 0;
        }

        public virtual char ChooseTurnAction()
        {
            return 'a';
        }

        public virtual char ChooseReaction()
        {
            return 't';
        }
    }
    class Player : Character
    {
        private bool IsCooldown;
        private int HealCooldown;
        private int DodgeCooldown;
        private int maxHealCd;
        private int maxDodgeCd;
        private bool usedHealThisTurn;
        private bool usedDodgeThisTurn;


        public Player(int hP, int dodges, int heals)
        {
            HP = hP;
            Dodges = dodges;
            Heals = heals;
            MaxHp = hP;
            IsCooldown = false;
            maxHealCd = 0;
            maxDodgeCd = 0;
        }

        public Player()
        {
            HP = 100;
            MaxHp = 100;
            Dodges = 2;
            Heals = 2;
            IsCooldown = false;
        }

        public void SetHealCd(int healCd)
        {
            maxHealCd = healCd;
        }
        public void SetDodgeCd(int dodgeCld)
        {
            maxDodgeCd = dodgeCld;
        }
        public void SetIsCooldown(bool isCooldown)
        {
            IsCooldown = isCooldown;
        }

        public override int Attack()
        {
            Random rnd = new Random();
            int dam = rnd.Next(10, 30);
            return dam;         
        }
        public override int Heal()
        {
            Clear();
            if (IsCooldown)
            {
                if (HealCooldown == 0 && (HP + 15) <= MaxHp)
                {
                    HP += 15;
                    WriteLine("Player chose to heal (+15 HP)");
                    HealCooldown += maxHealCd;
                    usedHealThisTurn = true;
                    PlayerStats();
                    return 1;
                }
                else
                {
                    if ((HP + 15) > MaxHp && HealCooldown != 0)
                    {
                        WriteLine("Heal isn't avaliable due to cooldown and too much HP!");
                        WriteLine("Attack was chosen automatically!!!");
                        PressAny();
                        return 0;
                    }
                    else if ((HP + 15) > MaxHp)
                    {
                        WriteLine("Heal isn't avaliable - too much HP!");
                        WriteLine("Attack was chosen automatically!!!");
                        PressAny();
                        return 0;
                    }
                    else
                    {
                        WriteLine("Heal isn't avaliable due to cooldown");
                        WriteLine("Attack was chosen automatically!!!");
                        PressAny();
                        return 0;
                    }
                }
            }       
            else
            {
                if (Heals == 0)
                {
                    WriteLine("0 heals left");
                    WriteLine("Attack was chosen automatically!!!");
                    PressAny();
                    return 0;
                }
                else if ((HP + 15) <= MaxHp)
                {
                    HP += 15;
                    Heals -= 1;
                    WriteLine("Player chose to heal (+15 HP)");
                    PlayerStats();
                    return 1;
                }
                else
                {
                    WriteLine("Can't heal anymore - too much HP!");
                    WriteLine("Attack was chosen automatically!!!");
                    PressAny();
                    return 0;
                }
            }                      
        }

        public void Dodge(int damage)
        {
            if(IsCooldown)
            {
                if (DodgeCooldown == 0)
                {
                    DodgeCooldown += maxDodgeCd;
                    usedDodgeThisTurn = true;
                    WriteLine("Player chose to dodge");
                    PressAny();
                }
                else
                {
                    WriteLine("Dodge isn't avaliable due to cooldown");
                    WriteLine("Taking damage was chosen automatically!!!");
                    PressAny();
                    HP -= damage;
                    WriteLine("Player took " + damage + " damage");
                }
            }
            else
            {
                Dodges--;
            }            
        }

        public void TakeDamage(int damage, char reaction)
        {
            if (reaction == 'd')
            {
                Dodge(damage);
            }
            else if (reaction == 't')
            {
                HP -= damage;
                WriteLine("Player took " + damage + " damage");
            }

            if (HP <= 0)
            {
                HP = 0;
            }
            PlayerStats();
        }      

        public override char ChooseTurnAction()
        {
            if (IsCooldown)
            {
                if(usedDodgeThisTurn)
                {
                    usedDodgeThisTurn = false;
                }
                else if(DodgeCooldown > 0)
                {
                    DodgeCooldown -= 1;
                }


                if (usedHealThisTurn)
                {
                    usedHealThisTurn = false;
                }
                else if (HealCooldown > 0)
                {
                    HealCooldown -= 1;
                }               
            }
            
            if(IsCooldown)
            {
                WriteLine("Player has a heal cooldown of " + HealCooldown + " rounds");
            }
            else
            {
                WriteLine("Player has " +  Heals + " heals");
            }

            char ch = IsChar("Choose your action (a = attack, h = heal): ");
            Clear();

            return ch;
        }
        private char IsChar(string mes)
        {
            char ch;
            while (true)
            {
                Clear();
                Write(mes);
                
                try
                {
                    ch = Convert.ToChar(ReadLine());
                    break;
                }
                catch (FormatException e)
                {
                    WriteLine("Input error!!! Please enter exactly one character!");
                    PressAny();
                }
            }
            return ch;
        }

        public override char ChooseReaction()
        {
            while (true)
            {
                Clear();
                if(IsCooldown)
                {
                    WriteLine("Player has a dodge cooldown of " + DodgeCooldown + " rounds");
                }
                else
                {
                    WriteLine("Player has " + Dodges + " dodges");
                }

                char ch = IsChar("Choose your action (d = dodge, t = take damage): ");

                Clear();

                if (ch == 'd')
                {
                    if(IsCooldown)
                    {
                        return 'd';
                    }
                    else
                    {
                        if (Dodges > 0)
                        {
                            WriteLine("Player chose to dodge");
                            PressAny();
                            return 'd';
                        }
                        else
                        {
                            WriteLine("Player can't dodge anymore!!!");
                            WriteLine("Take damage was chosen automatically!!!");
                            PressAny();
                            return 't';
                        }
                    }                 
                }
                else if (ch == 't')
                {
                    WriteLine("Player chose to take damage");
                    PressAny();
                    return 't';
                }
                else
                {
                    WriteLine("Error (only 'd' or 't')");
                    PressAny();
                }
            }
        }

        public void PlayerStats()
        {
            if (IsCooldown)
            {
                WriteLine("-----------------------------------------");
                WriteLine("Player stats: \nHP = " + HP + "\nDodge cooldown = " + DodgeCooldown + " rounds " + " \nHeal cooldown = " + HealCooldown + " rounds ");
                WriteLine("-----------------------------------------");
                PressAny();
            }
            else
            {
                WriteLine("-----------------------------------------");
                WriteLine("Player stats: \nHP = " + HP + "\nAvaliable dodges = " + Dodges + "\nAvaliable heals = " + Heals);
                WriteLine("-----------------------------------------");
                PressAny();
            }

        }

        public int PlayerGetHp()
        {
            return HP;
        }

        public void PressAny()
        {
            WriteLine("Press any key to continue...");
            ReadKey();
            Clear();
        }
    }
    class Fiend : Character
    {
        private bool IsCooldown;
        private int HealCooldown;
        private int DodgeCooldown;
        private int maxHealCd;
        private int maxDodgeCd;
        private int Diff;
        private bool usedHealThisTurn;
        private bool usedDodgeThisTurn;



        public Fiend(int hP, int dodges, int heals)
        {
            HP = hP;
            Dodges = dodges;
            Heals = heals;
            MaxHp = hP;
            Diff = 1;
            IsCooldown = false;
            maxDodgeCd = 0;
            maxHealCd = 0;
        }

        public Fiend()
        {
            HP = 100;
            MaxHp = 100;
            Dodges = 2;
            Heals = 2;
            Diff = 2;
        }

        public void SetDiff(int diff)
        {
            Diff = diff;
        }
        public void SetHealCd(int healCd)
        {
            maxHealCd = healCd;
        }
        public void SetDodgeCd(int dodgeCld)
        {
            maxDodgeCd = dodgeCld;
        }
        public void SetIsCooldown(bool isCooldown)
        {
            IsCooldown = isCooldown;
        }
        

        public override int Attack()
        {
            int minDmg = 10;
            int maxDmg = 15;

            if (Diff == 2)
            {
                minDmg = 15;
                maxDmg = 25;
            }
            else if (Diff == 3)
            {
                minDmg = 25;
                maxDmg = 35;
            }

            Random rnd = new Random();
            int dam = rnd.Next(minDmg, maxDmg);
            return dam;

        }

        public override int Heal()
        {
            Clear();

            if (IsCooldown)
            {
                if (HealCooldown == 0)
                {
                    HP += 15;
                    WriteLine("Fiend chose to heal (+15 HP)");
                    HealCooldown += maxHealCd;
                    usedHealThisTurn = true;
                    FiendStats();
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (Heals == 0)
                {
                    return 0;
                }
                else if (HP + 15 < MaxHp)
                {
                    HP += 15;
                    Heals -= 1;
                    WriteLine("Fiend chose to heal (+15 HP)");
                    FiendStats();
                    return 1;
                }
                return 0;
            }       
        }


        public void Dodge(int damage)
        {
            if (IsCooldown)
            {
                if (DodgeCooldown == 0)
                {
                    DodgeCooldown += maxDodgeCd;
                    usedDodgeThisTurn = true;
                    WriteLine("Fiend chose to dodge");
                    PressAny();
                }
                else
                {
                    HP -= damage;
                    WriteLine("Fiend took " + damage + " damage");
                    PressAny();
                }
            }
            else
            {
                Dodges--;
            }
        }

        public void TakeDamage(int damage, char reaction)
        {
            if (reaction == 'd' )
            {
                Dodge(damage);
            }
            else if (reaction == 't')
            {
                HP -= damage;
                WriteLine("Fiend took " + damage + " damage");
            }

            if (HP <= 0)
            {
                HP = 0;
            }
            FiendStats();
        }

        public override char ChooseTurnAction()
        {
            if (IsCooldown)
            {
                if (usedDodgeThisTurn)
                {
                    usedDodgeThisTurn = false;
                }
                else if (DodgeCooldown > 0)
                {
                    DodgeCooldown -= 1;
                }


                if (usedHealThisTurn)
                {
                    usedHealThisTurn = false;
                }
                else if (HealCooldown > 0)
                {
                    HealCooldown -= 1;
                }
            }

            Random rnd = new Random();
            int a = rnd.Next(1, 100);

             if (a > 0 && a < 30 && HP < MaxHp-15 && HealCooldown==0)
             {
                WriteLine("Fiend chose to heal");
                PressAny();
                return 'h';
             }
             else
             {
                WriteLine("Fiend chose to attack");
                PressAny();
                return 'a';
             }
        }

        public override char ChooseReaction()
        {
            Clear();
            Random rnd = new Random();
            int a = rnd.Next(1, 3);

            if (a == 1)
            {
                if (IsCooldown)
                {
                    return 'd';
                }
                else
                {
                    if (Dodges > 0)
                    {
                        WriteLine("Fiend chose to dodge");
                        PressAny();
                        return 'd';
                    }
                    else
                    {
                        WriteLine("Fiend chose to take damage");
                        PressAny();
                        return 't';
                    }
                }           
            }
            else
            {
                WriteLine("Fiend chose to take damage");
                PressAny();
                return 't';
            }           
        }

        public void FiendStats()
        {
            if (IsCooldown)
            {
                WriteLine("-----------------------------------------");
                WriteLine("Fiend's stats: \nHP = " + HP + "\nDodge cooldown = " + DodgeCooldown + " rounds " + " \nHeal cooldown = " + HealCooldown + " rounds ");
                WriteLine("-----------------------------------------");
                PressAny();
            }
            else
            {
                WriteLine("-----------------------------------------");
                WriteLine("Fiend's stats: \nHP = " + HP + "\nAvaliable dodges = " + Dodges + "\nAvaliable heals = " + Heals);
                WriteLine("-----------------------------------------");
                PressAny();
            }        
        }

        public int FiendGetHp()
        {
            return HP;
        }

        public void PressAny()
        {
            WriteLine("Press any key to continue...");
            ReadKey();
            Clear();
        }
    }

    internal class Program
    {       
        public static void PressAny()
        {
            WriteLine("Press any key to continue...");
            ReadKey();
            Clear();
        }

        public static (Player, Fiend) Customize()
        {
            int abil = IsIntAndPositive("Choose: \n1 - enable cooldown; \n2 - limited abilities; \nYour choice: ", 1, 2);
            switch (abil)
            {
                case 1:
                    int MaxHp = 0;
                    int dodgeCld = 0;
                    int healsCld = 0;
                    while (true)
                    {
                        int ch = IsIntAndPositive("1 - MaxHp\n2 - Heal cooldown\n3 - Dodge cooldown\n0 - Complete customization\nYour choice: ", 0, 3);

                        switch (ch)
                        {
                            case 0:
                                Clear();
                                if (dodgeCld == 0)
                                {
                                    dodgeCld = 1;
                                }
                                if (healsCld == 0)
                                {
                                    healsCld = 1;
                                }
                                if (MaxHp == 0)
                                {
                                    MaxHp = 100;
                                }
                                Player player = new Player(MaxHp, 0, 0);
                                Fiend fiend = new Fiend(MaxHp, 0, 0);
                                player.SetDodgeCd(dodgeCld);
                                fiend.SetDodgeCd(dodgeCld);
                                player.SetHealCd(healsCld);
                                fiend.SetHealCd(healsCld);
                                player.SetIsCooldown(true);
                                fiend.SetIsCooldown(true);

                                WriteLine("Customization complete!!!");
                                PressAny();

                                return (player, fiend);
                            case 1:
                                MaxHp = IsIntAndPositive("Enter the amount of HP in range between 1 and 500: ", 1, 500);
                                break;
                            case 2:
                                healsCld = IsIntAndPositive("Enter the heal cooldown in range between 1 and 3: ", 1, 3);
                                break;
                            case 3:
                                dodgeCld = IsIntAndPositive("Enter the dodge cooldown in range between 1 and 3: ", 1, 3);
                                break;
                        }
                    }
                case 2:
                    int Dodges = 0;
                    int Heals = 0;
                    int MaxHP = 0;
                    while (true)
                    {
                        int ch = IsIntAndPositive("1 - MaxHp\n2 - Heals\n3 - Dodges\n0 - Complete customization\nYour choice: ", 0, 3);

                        switch (ch)
                        {                           
                            case 0:
                                Clear();
                                if (Dodges == 0)
                                {
                                    Dodges = 2;
                                }
                                if (Heals == 0)
                                {
                                    Heals = 2;
                                }
                                if (MaxHP == 0)
                                {
                                    MaxHP = 100;
                                }
                                Player player = new Player(MaxHP, Dodges, Heals);
                                Fiend fiend = new Fiend(MaxHP, Dodges, Heals);
                                WriteLine("Customization complete!!!");
                                PressAny();
                                return (player, fiend);
                            case 1:
                                MaxHP = IsIntAndPositive("Enter the amount of HP in range between 1 and 500: ", 1, 500);
                                break;
                            case 2:
                                Heals = IsIntAndPositive("Enter the amount of heals in range between 1 and 10: ", 1, 10);
                                break;
                            case 3:
                                Dodges = IsIntAndPositive("Enter the amount of dodges in range between 1 and 10: ", 1, 10);
                                break;
                        }                       
                    }
                default:
                    return (new Player(), new Fiend());
            }           
        }         
     

        static void Main(string[] args)
        {
            Player player;
            Fiend fiend;

            int diff = IsIntAndPositive("Choose difficulty: \n1 - easy\n2 - medium\n3 - hard\nYour choice: ", 1, 3);          
            int customize = IsIntAndPositive("Press '1' to customize settings or any other digit to proceed without customization: ", 0, 9);
            
            if (customize == 1)
            {
                var result = Customize();
                player = result.Item1;
                fiend = result.Item2;
            }
            else
            {                
                player = new Player();
                fiend = new Fiend();
                WriteLine("Default settings were set!!!");
                PressAny();
            }
            fiend.SetDiff(diff);


            Game(player, fiend);
        }

       
        static void Game(Player player, Fiend fiend)
        {
            char react;
            int damage;
            int heal;
            char ch;
            while (true)
            {
                while (true)
                {
                    ch = player.ChooseTurnAction();
                    switch (ch)
                    {
                        case 'a':
                            damage = player.Attack();
                            react = fiend.ChooseReaction();
                            fiend.TakeDamage(damage, react);
                            break;
                        case 'h':
                            heal = player.Heal();
                            if (heal == 0)
                            {
                                damage = player.Attack();
                                react = fiend.ChooseReaction();
                                fiend.TakeDamage(damage, react);
                            }
                            break;
                        default:
                            WriteLine("Error (only 'a' or 'h')");
                            PressAny();
                            continue;
                    }
                    break;
                }

                int Hp = fiend.FiendGetHp();
                if (Hp <= 0)
                {
                    WriteLine("Fiend is dead");
                    PressAny();
                    break;
                }

                while (true)
                {
                    ch = fiend.ChooseTurnAction();
                    switch (ch)
                    {
                        case 'a':
                            damage = fiend.Attack();
                            react = player.ChooseReaction();
                            player.TakeDamage(damage, react);
                            Clear();
                            break;
                        case 'h':
                            heal = fiend.Heal();
                            if (heal == 0)
                            {
                                damage = fiend.Attack();
                                react = player.ChooseReaction();
                                player.TakeDamage(damage, react);
                            }
                            break;
                        default:
                            WriteLine("Error!!!");
                            PressAny();
                            continue;
                    }
                    break;
                }

                Hp = player.PlayerGetHp();
                if (Hp <= 0)
                {
                    WriteLine("Player is dead");
                    PressAny();
                    break;
                }
            }
        }
        static int IsIntAndPositive(string s, int min, int max)
        {
            int n;
            while (true)
            {
                Clear();
                Write(s);
                try
                {
                    n = Convert.ToInt32(ReadLine());
                    if (n >= min && n <= max)
                        break;
                    else
                        throw new Exception("The number should be between " + min + " and " + max + "\nPress any key to continue...");
                }
                catch (FormatException)
                {
                    WriteLine("ERROR!!! Enter an integer \nPress any key to continue...");
                    ReadKey();
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message);
                    ReadKey();
                }
            }
            return n;
        }
    }
}
