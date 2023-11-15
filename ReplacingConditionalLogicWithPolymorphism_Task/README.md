# Замена условной логики полиморфизмом

## Задание

Сейчас система не готова к расширению. К сожалению при добавление нового способа оплаты, нам нужно модифицировать все ифы которые совершают те или иные действия с разными системами.

Необходимо зафиксировать интерфейс платежёной системы и сокрыть их многообразие под какой-нибудь сущностью. Например фабрикой (или фабричным методом).

Важное условие: пользователь вводит именно идентификатор платёжной системы.

https://gist.github.com/HolyMonkey/14f78ed72bda289980fce43f50143278

## Копия исходников

```cs
using System;

namespace IMJunior
{
    class Program
    {
        static void Main(string[] args)
        {
            var orderForm = new OrderForm();
            var paymentHandler = new PaymentHandler();

            var systemId = orderForm.ShowForm();

            if (systemId == "QIWI")
                Console.WriteLine("Перевод на страницу QIWI...");
            else if (systemId == "WebMoney")
                Console.WriteLine("Вызов API WebMoney...");
            else if (systemId == "Card")
                Console.WriteLine("Вызов API банка эмитера карты Card...");

            paymentHandler.ShowPaymentResult(systemId);
        }
    }

    public class OrderForm
    {
        public string ShowForm()
        {
            Console.WriteLine("Мы принимаем: QIWI, WebMoney, Card");

            //симуляция веб интерфейса
            Console.WriteLine("Какое системой вы хотите совершить оплату?");
            return Console.ReadLine();
        }
    }

    public class PaymentHandler
    {
        public void ShowPaymentResult(string systemId)
        {
            Console.WriteLine($"Вы оплатили с помощью {systemId}");

            if (systemId == "QIWI")
                Console.WriteLine("Проверка платежа через QIWI...");
            else if (systemId == "WebMoney")
                Console.WriteLine("Проверка платежа через WebMoney...");
            else if (systemId == "Card")
                Console.WriteLine("Проверка платежа через Card...");

            Console.WriteLine("Оплата прошла успешно!");
        }
    }
}
```