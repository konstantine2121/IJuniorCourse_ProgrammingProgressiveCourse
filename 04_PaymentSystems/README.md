﻿# Платёжные системы

## Задание

Ваша задача - https://gist.github.com/HolyMonkey/06eb6d684358365f1653301980545d05

Выведите платёжные ссылки для трёх разных систем платежа: 
1) pay.system1.ru/order?amount=12000RUB&hash={MD5 хеш ID заказа}
2) order.system2.ru/pay?hash={MD5 хеш ID заказа + сумма заказа}
3) system3.com/pay?amount=12000&curency=RUB&hash={SHA-1 хеш сумма заказа + ID заказа + секретный ключ от системы}

## Копия исходников

```cs
class Program
{
    static void Main(string[] args)
    {
        //Выведите платёжные ссылки для трёх разных систем платежа: 
        //pay.system1.ru/order?amount=12000RUB&hash={MD5 хеш ID заказа}
        //order.system2.ru/pay?hash={MD5 хеш ID заказа + сумма заказа}
        //system3.com/pay?amount=12000&curency=RUB&hash={SHA-1 хеш сумма заказа + ID заказа + секретный ключ от системы}
    }
}

public class Order
{
    public readonly int Id;
    public readonly int Amount;

    public Order(int id, int amount) => (Id, Amount) = (id, amount);
}

public interface IPaymentSystem
{
    public string GetPayingLink(Order order);
}
```