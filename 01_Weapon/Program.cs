using System;
using System.Collections.Generic;
using System.Linq;

namespace _01_Weapon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var test = new Test();
            test.Run();
        }
    }

    class Test
    {        
        private const int MinHealth = 0;
        private const int MaxHealth = 50;

        private static readonly Random _random = new Random();

        public void Run()
        {
            var players = CreatePlayers(5);
            var bot = CreateBot(10, 50);

            while (!bot.OutOfAmmo() && players.Any(player => player.IsAlive()))
            {
                foreach(var player in players)
                {
                    bot.OnSeePlayer(player);
                }
            }
        }

        private List<Player> CreatePlayers(uint numberOfPlayers)
        {
            var players = new List<Player>();

            for (int i = 0; i < numberOfPlayers; i++)
            {
                players.Add(new Player(GetHealth()));
            }

            return players;
        }

        private int GetHealth()
        {
            return _random.Next(MinHealth, MaxHealth + 1);
        }

        private Bot CreateBot(int damage, int bullets)
        {
            return new Bot(new Weapon(damage, bullets));
        }
    }

    #region Extensions
    
    static class BotExtensions
    {
        public static bool OutOfAmmo(this Bot bot)
        { 
            return bot.Weapon.Bullets <= 0; 
        }
    }

    static class PlayerExtensions
    {
        public static bool IsAlive(this Player player)
        {
            return player.Health > 0;
        }
    }

    #endregion Extensions

    #region Classes
    
    class Weapon
    {
        public Weapon(int damage, int bullets)
        {
            if (damage < 0) 
            {
                throw new ArgumentOutOfRangeException(nameof(damage));
            }
            if (bullets < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bullets));
            }

            Damage = damage;
            Bullets = bullets;
        }

        public int Damage { get; }

        public int Bullets { get; private set; }

        public void Fire(Player player)
        {
            if (Bullets <= 0)
            {
                return;
            }

            player.TakeDamage(Damage);
            Bullets--;
        }
    }

    class Player
    {
        public Player(int health)
        {
            Health = health;
        }

        public int Health { get; private set; }

        public void TakeDamage(int damage)
        {
            if (damage <= 0)
            {
                return;
            }

            Health -= damage;
        }
    }

    class Bot
    {
        public Bot(Weapon weapon)
        {
            if (weapon == null)
            {
                throw new ArgumentNullException(nameof(weapon));
            }

            Weapon = weapon;
        }

        internal Weapon Weapon { get; }

        public void OnSeePlayer(Player player)
        {
            Weapon.Fire(player);
        }
    }

    #endregion Classes
}
