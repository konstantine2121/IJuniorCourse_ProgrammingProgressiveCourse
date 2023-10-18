# Интернет магазин

## Задание

У вас есть UseCase от проектировщика который показывает как ожидается работа с системой магазина.

Вот он - https://gist.github.com/HolyMonkey/0ea27f88a1ce55d514588ab013c91076

Paylink - просто какая-нибудь случайная строка.

## Копия исходников

```cs
Good iPhone12 = new Good("IPhone 12");
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

cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары
```