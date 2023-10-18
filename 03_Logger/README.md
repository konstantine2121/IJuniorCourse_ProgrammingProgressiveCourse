# Логирование

## Задание

Есть такая реализация логирования - https://pastebin.com/7xL6S4vV

Защищённый логер даёт функционал, что логер пишется только по пятницам (такая условность).

Представьте класс Pathfinder у которого есть зависимость от условного ILogger, в процессе своей работы он что-то пишет в лог. 
Что не принципиально. 
Сделайте в нём один метод Find который только пишет в лог через своего логера.

Перепроектируйте систему логирования так, чтобы у меня было 5 объектов класса Pathfinder:
1) Пишет лог в файл. 
2) Пишет лог в консоль. 
3) Пишет лог в файл по пятницам. 
4) Пишет лог в консоль по пятницам. 
5) Пишет лог в консоль а по пятницам ещё и в файл.

## Копия исходников

```cs
using System;
using System.IO;

namespace Lesson
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }

    class ConsoleLogWritter
    {
        public virtual void WriteError(string message)
        {
            Console.WriteLine(message);
        }
    }

    class FileLogWritter
    {
        public virtual void WriteError(string message)
        {
            File.WriteAllText("log.txt", message);
        }
    }

    class SecureConsoleLogWritter : ConsoleLogWritter
    {
        public override void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                base.WriteError(message);
            }
        }
    }
}
```