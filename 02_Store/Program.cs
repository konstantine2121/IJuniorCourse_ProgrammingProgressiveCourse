using System;
using System.Collections.Generic;

namespace _02_Store
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var test = new Test();
            test.Run();
            Console.ReadKey();
        }
    }

    public class Test
    {
        private const string SuccessBuyMessage = "Товар успешно добавлен.";
        private const string ErrorBuyMessage = "Ошибка при добавлении товара.";

        public void Run()
        {
            var iPhone11 = new Good("IPhone 11");
            var iPhone12 = new Good("IPhone 12");

            var warehouse = new Warehouse();
            var shop = new Shop(warehouse);

            warehouse.Delive(iPhone11, 1);
            warehouse.Delive(iPhone12, 10);

            //Вывод всех товаров на складе с их остатком

            Console.WriteLine("Содержимое склада: ");
            PrintGoodsInfo(warehouse.GoodsInfo);

            Cart cart = shop.Cart();
            //cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе
            Buy(cart, iPhone12, 4);
            Buy(cart, iPhone11, 3);

            Console.WriteLine("Содержимое корзины: ");
            PrintGoodsInfo(cart.GoodsInfo);

            //Вывод всех товаров в корзине
            Console.WriteLine("Ссылка на оплату товара: ");
            Console.WriteLine(cart.Order().Paylink);
        }

        private static void Buy(Cart cart, Good good, uint amount)
        {
            Console.WriteLine("Добавление товара в корзину: ");
            Console.WriteLine("{0, -16} {1, -5}", good.Name, amount);

            var message = cart.TryAdd(good, amount) ?
                SuccessBuyMessage : 
                ErrorBuyMessage;

            Console.WriteLine(message + Environment.NewLine);
        }

        private static void PrintGoodsInfo(IReadOnlyDictionary<Good, uint> goods)
        {
            Console.WriteLine($"Записей {goods.Count}");
            int counter = 1;

            foreach (var record in goods)
            {
                Console.WriteLine("{0, 3}) {1, -16} {2, -5}",
                    counter,
                    record.Key.Name,
                    record.Value);
                counter++;
            }

            Console.WriteLine();
        }
    }

    #region Classes

    public class Good
    {
        public Good(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Can't be empty or white space.", nameof(name));
            }

            Name = name;
        }

        public string Name { get; }
    }

    public class Warehouse : IGoodsProvider
    {
        private readonly Dictionary<Good, uint> _goods;

        public Warehouse() : this(new Dictionary<Good, uint>())
        {
        }

        public Warehouse(Dictionary<Good, uint> goods)
        {   
            _goods = goods ?? throw new ArgumentNullException(nameof(goods)); ;
        }

        public IReadOnlyDictionary<Good, uint> GoodsInfo => _goods;

        /// <summary>
        /// Доставить на склад.
        /// </summary>
        /// <param name="good">товар</param>
        /// <param name="amount">количество</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Delive(Good good, uint amount)
        {
            if (good == null)
            {
                throw new ArgumentNullException(nameof(good));
            }

            if (!ContainsRecord(good))
            {
                _goods.Add(good, amount);
            }
            else
            {
                _goods[good] += amount;
            }
        }

        public void Take(Good good, uint amount)
        {
            if (!ContainsRecord(good))
            {
                throw new InvalidOperationException("Такого товара нет на складе.");
            }

            _goods[good] -= amount;
        }
        
        public bool CanTake(Good good, uint amount)
        {
            return ContainsRecord(good) && _goods[good] >= amount;
        }

        private bool ContainsRecord(Good good)
        {
            return good != null && _goods.ContainsKey(good);
        }
    }

    public interface IGoodsProvider
    {
        void Take(Good good, uint amount);

        bool CanTake(Good good, uint amount);
    }

    public class Shop 
    {
        private readonly Warehouse _warehouse;

        public Shop(Warehouse warehouse)
        {   
            _warehouse = warehouse ?? throw new ArgumentNullException(nameof(warehouse)); ;
        }

        public Cart Cart()
        {
            return new Cart(_warehouse);
        }
    }

    public class Cart
    {
        private readonly IGoodsProvider _provider;
        private readonly Dictionary<Good, uint> _goods = new Dictionary<Good, uint>();
        
        public Cart(IGoodsProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public IReadOnlyDictionary<Good, uint> GoodsInfo => _goods;

        public bool TryAdd(Good good, uint amount)
        {
            if (!_provider.CanTake(good, amount))
            {
                return false;
            }

            if (_goods.TryGetValue(good, out uint currentAmount))
            {
                _goods[good] = currentAmount + amount;
            }
            else
            {
                _goods.Add(good, amount);
            }

            return true;
        }

        public Order Order()
        {
            foreach (var pair in _goods)
            {
                _provider.Take(pair.Key, pair.Value);
            }

            return new Order();
        }
    }

    public class Order
    {
        public Order()
        {
            Paylink = GeneratePaylink();
        }

        public string Paylink { get; }

        private string GeneratePaylink()
        {
            var id = Guid.NewGuid().ToString("N");
            var link = $"my.shop.com/order/{id}";

            return link;
        }
    }

    #endregion Classes
}
