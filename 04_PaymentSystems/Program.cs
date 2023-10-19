using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace _04_PaymentSystems
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var test = new Test();
            test.Run();
        }
    }

    public class Test
    {
        public void Run()
        {
            var orderFactory = new OrderFactory();
            var paymentFactory = new PaymentSystemFactory();
            var orders = orderFactory.CreateOrders(5);
            var payments = paymentFactory.CreateAll();

            var ordersInfo = GetOrdersInfo(orders, payments);
            PrintOrdersInfo(ordersInfo);
        }

        private static Dictionary<Order, List<string>> GetOrdersInfo(List<Order> orders, List<IPaymentSystem> payments)
        {
            var ordersInfo = new Dictionary<Order, List<string>>();

            foreach (var order in orders)
            {
                var links = new List<string>();

                foreach (var payment in payments)
                {
                    links.Add(payment.GetPayingLink(order));
                }

                ordersInfo.Add(order, links);
            }

            return ordersInfo;
        }

        private static void PrintOrdersInfo(Dictionary<Order, List<string>> ordersInfo)
        {
            foreach (var info in ordersInfo)
            {
                PrintOrderInfo(info.Key, info.Value);
            }
        }

        private static void PrintOrderInfo(Order order, List<string> paymentLinks)
        {
            const string format = "{0, -12} {1, -12}";
            int linkCounter = 1;

            Console.WriteLine("Заказ:");
            Console.WriteLine(format, nameof(order.Id), nameof(order.Amount));
            Console.WriteLine(format + Environment.NewLine, order.Id, order.Amount);

            Console.WriteLine("Ссылки для оплаты:");

            paymentLinks.ForEach(link =>
            {
                var line = $"{linkCounter,3})    {link}" + Environment.NewLine;
                Console.WriteLine(line);
                linkCounter++;
            });

            Console.WriteLine("----" + Environment.NewLine);
        }
    }

    #region Factories
    
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
            return _random.Next(MinAmount, MaxAmount + 1);
        }
    }

    public class PaymentSystemFactory
    {
        private static readonly Random _random = new Random();

        public List<IPaymentSystem> CreateAll()
        {
            return new List<IPaymentSystem>
            {
                new FirstPaymentSystem(),
                new SecondPaymentSystem(),
                new ThirdPaymentSystem(CreateSecretKey())
            };
        }

        private static int CreateSecretKey()
        {
            return _random.Next();
        }
    }

    #endregion Factories

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

    #region HashCalculator

    public interface IHashCalculator
    {
        string CalculateHash(byte[] bytes);
    }

    public sealed class HashCalculator : IHashCalculator
    {
        private readonly HashAlgorithm _hashAlgorithm;

        private HashCalculator(HashAlgorithm hashAlgorithm)
        {
            _hashAlgorithm = hashAlgorithm ?? throw new ArgumentNullException(nameof(hashAlgorithm));
        }

        public static HashCalculator CreateMd5Calculator() => new HashCalculator(MD5.Create());

        public static HashCalculator CreateSha1Calculator() => new HashCalculator(SHA1.Create());

        public string CalculateHash(byte[] bytes)
        {
            var hash = _hashAlgorithm.ComputeHash(bytes);

            return Base64UrlEncoder.Encode(hash);
        }
    }

    #endregion HashCalculator

    #region PaymentSystem
    
    public interface IPaymentSystem
    {
        string GetPayingLink(Order order);
    }

    public abstract class BasePaymentSystem : IPaymentSystem
    {
        private readonly IHashCalculator _hashCalculator;

        protected BasePaymentSystem(IHashCalculator hashCalculator)
        {
            _hashCalculator = hashCalculator;
        }

        public string GetPayingLink(Order order)
        {
            var hashSource = PrepareHashSource(order);
            var hash = _hashCalculator.CalculateHash(hashSource);

            return BuildLink(order, hash);
        }

        protected abstract byte[] PrepareHashSource(Order order);

        protected abstract string BuildLink(Order order, string hash);

        protected static byte[] GetBytes(int value)
        {
            return BitConverter.GetBytes(value);
        }
    }

    /// <summary>
    /// Link:<br/>
    /// pay.system1.ru/order?amount={order.Amount}RUB&amp;hash={MD5 хеш ID заказа}
    /// </summary>
    public class FirstPaymentSystem : BasePaymentSystem
    {
        public FirstPaymentSystem() :
            base(HashCalculator.CreateMd5Calculator())
        {
        }

        protected override string BuildLink(Order order, string hash)
        {
            return $"pay.system1.ru/order?amount={order.Amount}&hash={hash}";
        }

        protected override byte[] PrepareHashSource(Order order)
        {
            return GetBytes(order.Id);
        }
    }


    /// <summary>
    /// Link:<br/>
    /// order.system2.ru/pay?hash={MD5 хеш ID заказа + сумма заказа}
    /// </summary>
    public class SecondPaymentSystem : BasePaymentSystem
    {
        public SecondPaymentSystem() :
            base(HashCalculator.CreateMd5Calculator())
        {
        }

        protected override string BuildLink(Order order, string hash)
        {
            return $"order.system2.ru/pay?hash={hash}";
        }

        protected override byte[] PrepareHashSource(Order order)
        {
            return GetBytes(order.Id)
                .Concat(GetBytes(order.Amount))
                .ToArray();
        }
    }

    /// <summary>
    /// Link:<br/>
    /// system3.com/pay?amount={order.Amount}&amp;curency=RUB&amp;hash={SHA-1 хеш сумма заказа + ID заказа + секретный ключ от системы}
    /// </summary>
    public class ThirdPaymentSystem : BasePaymentSystem
    {
        private readonly int _secretKey;

        /// <param name="secretKey">секретный ключ от системы</param>
        public ThirdPaymentSystem(int secretKey) :
            base(HashCalculator.CreateSha1Calculator())
        {
            _secretKey = secretKey;
        }

        protected override string BuildLink(Order order, string hash)
        {
            return $"system3.com/pay?amount={order.Amount}&curency=RUB&hash={hash}";
        }

        protected override byte[] PrepareHashSource(Order order)
        {
            return GetBytes(order.Amount)
                .Concat(GetBytes(order.Id))
                .Concat(GetBytes(_secretKey))
                .ToArray();
        }
    }

    #endregion PaymentSystem
}
