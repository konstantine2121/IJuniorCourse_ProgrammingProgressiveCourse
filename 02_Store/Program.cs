using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_Store
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public class Good
    {
        public Good(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    public class Warehouse
    {
        private readonly Dictionary<Good, int> _goods = new Dictionary<Good, int>();

        public Warehouse(Dictionary<Good, int> goods)
        {
            _goods = goods;
        }

        public void Delive(Good good, int number)
        {

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
    }

    public class Cart
    {
        private readonly Shop _shop;
        private readonly Order _order;

        public Cart(Shop shop)
        {
            _shop = shop;
            _order = new Order();
        }

        public void Add(Good good, int number)
        {

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

    /*
     * Good iPhone12 = new Good("IPhone 12");
Good iPhone11 = new Good("IPhone 11");

Warehouse warehouse = new Warehouse();

Shop shop = new Shop(warehouse);

warehouse.Delive(iPhone12, 10);
warehouse.Delive(iPhone11, 1);

//Вывод всех товаров на складе с их остатком

Cart cart = shop.Cart();
cart.Add(iPhone12, 4);
cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

//Вывод всех товаров в корзине

Console.WriteLine(cart.Order().Paylink);
     * */
}
