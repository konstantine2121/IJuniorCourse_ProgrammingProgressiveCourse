using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _04_PaymentSystems
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public class Test
    {
        public void Run()
        {
            var orderFactory = new OrderFactory();
            var orders = orderFactory.CreateOrders(2);

        }

        
    }

    public class OrderFactory
    {
        private const int MinAmount = 1;
        private const int MaxAmount = 100;

        private static readonly Random _random = new Random();
        private static int _orderIdCounter = 0;

        public Order Create()
        {
            return new Order(GetNextId(), GetRandomAmount());
        }

        public List<Order> CreateOrders(uint number)
        {
            var orders = new List<Order>();

            for (int i = 0; i < number; i++)
            {
                orders.Add(Create());
            }

            return orders;
        }

        private static int GetNextId()
        {
            return ++_orderIdCounter;
        }

        private static int GetRandomAmount()
        {
            return _random.Next(MinAmount, MaxAmount+1);
        }
    }

    public class Order
    {
        public readonly int Id;
        public readonly int Amount;

        public Order(int id, int amount)
        {
            Id = id;
            Amount = amount;
        }
    }

    public interface IPaymentSystem
    {
        string GetPayingLink(Order order);
    }

    public interface IHashCalculator
    {
        string CalculateHash(Order order);
    }

    public abstract class BaseHashCalculator : IHashCalculator
    {
        public abstract string CalculateHash(Order order);

        protected byte[] TranslateInteger(int integer)
        {
            const int mask = 0xFF;
            const int bytesInInteger = 32 / 8;
            var result = new byte[bytesInInteger];

            //BitConverter.

            for (int i = 0; i < bytesInInteger; i++)
            {

            }

            return result;
        }
    }

    //pay.system1.ru/order?amount=12000RUB&hash={MD5 хеш ID заказа}
    public class FirstPaymentSystem : IPaymentSystem
    {
        public string GetPayingLink(Order order)
        {
            var amount = order.Amount;
            var hash = CalculateHash(order.Id);//{MD5 хеш ID заказа}

            return $"pay.system1.ru/order?amount={amount}&hash={hash}";
        }

        private string CalculateHash(int orderId)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();

            //mD5CryptoServiceProvider.

            return null;
        }
    }


    //order.system2.ru/pay?hash={MD5 хеш ID заказа + сумма заказа}
    public class SecondPaymentSystem : IPaymentSystem
    {
        public string GetPayingLink(Order order)
        {
            throw new NotImplementedException();
        }
    }

    //system3.com/pay?amount=12000&curency=RUB&hash={SHA-1 хеш сумма заказа + ID заказа + секретный ключ от системы}
    public class ThirdPaymentSystem : IPaymentSystem
    {
        public string GetPayingLink(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
