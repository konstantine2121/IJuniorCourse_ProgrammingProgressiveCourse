using System;
using static System.Net.Mime.MediaTypeNames;

namespace _20_Task
{
    //20. Группировка полей по префиксу
    //Поправьте код - https://gist.github.com/HolyMonkey/228d407270b740387bbab0fede8fc29b

    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }

    class Player
    {
        public Player(string name, int age, Weapon weapon, Movement movement)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name can't be empty.", nameof(name));
            }

            if (age < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(age));
            }

            Name = name;
            Age = age;
            Weapon = weapon ?? throw new ArgumentNullException(nameof(weapon));
            Movement = movement ?? throw new ArgumentNullException(nameof(movement));
        }

        public string Name { get; }

        public int Age { get; private set; }

        public Weapon Weapon { get; private set; }
        public Movement Movement { get; }
    }

    class Weapon
    {
        public Weapon(float cooldown, int damage)
        {
            if (cooldown < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cooldown));
            }

            if (damage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(damage));
            }

            Cooldown = cooldown;
            Damage = damage;
        }

        public float Cooldown { get; private set; }

        public int Damage { get; private set; }
    }

    class Movement
    {
        public Movement(float speed, Direction direction)
        {
            if (speed < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(speed));
            }

            Speed = speed;
            Direction = direction ?? throw new ArgumentNullException(nameof(direction)); ;
        }

        public float Speed { get; private set; }

        public Direction Direction { get; private set; }
        
    }

    class Direction
    {
        public Direction(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; private set; }

        public float Y { get; private set; }
    }


}
