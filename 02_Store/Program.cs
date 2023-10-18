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

        private static void Buy(Cart cart, Good good, uint number)
        {
            Console.WriteLine("Добавление товара в корзину: ");
            Console.WriteLine("{0, -16} {1, -5}", good.Name, number);

            var message = cart.TryAdd(good, number) ?
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

    public class Warehouse
    {
        private readonly Dictionary<Good, uint> _goods;

        public Warehouse()
        {
            _goods = new Dictionary<Good, uint>();
        }

        public IReadOnlyDictionary<Good, uint> GoodsInfo => _goods;

        public Warehouse(Dictionary<Good, uint> goods)
        {
            if (goods == null)
            {
                throw new ArgumentNullException(nameof(goods));
            }

            _goods = goods;
        }

        /// <summary>
        /// Доставить на склад.
        /// </summary>
        /// <param name="good">товар</param>
        /// <param name="number">количество</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Delive(Good good, uint number)
        {
            if (good == null)
            {
                throw new ArgumentNullException(nameof(good));
            }

            if (!ContainsRecord(good))
            {
                _goods.Add(good, number);
            }
            else
            {
                var rest = _goods[good];
                _goods[good] = rest + number;
            }
        }

        /// <summary>
        /// Попытаться забрать со склада.
        /// </summary>
        /// <param name="good">товар</param>
        /// <param name="number">количество</param>
        /// <returns>Получилось или нет.</returns>
        public bool TryTake(Good good, uint number)
        {
            if (!CanTake(good, number))
            {
                return false;
            }

            var rest = _goods[good];
            _goods[good] = rest - number;

            return true;
        }

        private bool CanTake(Good good, uint number)
        {
            return ContainsRecord(good) && _goods[good] >= number;
        }

        private bool ContainsRecord(Good good)
        {
            return good != null && _goods.ContainsKey(good);
        }
    }

    public class Shop
    {
        private readonly Warehouse _warehouse;

        public Shop(Warehouse warehouse)
        {
            if (warehouse == null)
            {
                throw new ArgumentNullException(nameof(warehouse));
            }

            _warehouse = warehouse;
        }

        public Cart Cart()
        {
            return new Cart(this);
        }

        public bool TrySell(Good good, uint number)
        {
            return _warehouse.TryTake(good, number);
        }
    }

    public class Cart
    {
        private readonly Shop _shop;
        private readonly Dictionary<Good, uint> _goods = new Dictionary<Good, uint>();
        private readonly Order _order;

        public Cart(Shop shop)
        {
            _shop = shop;
            _order = new Order();
        }

        public IReadOnlyDictionary<Good, uint> GoodsInfo => _goods;

        public bool TryAdd(Good good, uint number)
        {
            if (!_shop.TrySell(good, number))
            {
                return false;
            }

            if (_goods.TryGetValue(good, out uint selectedNumber))
            {
                _goods[good] = selectedNumber + number;
            }
            else
            {
                _goods.Add(good, number);
            }

            return true;
        }

        public Order Order()
        {
            return _order;
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
