using System;
using System.Collections.Generic;
using System.IO;

namespace _03_Logger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var test = new Test();
            test.Run();
        }

        public class Test
        {
            public void Run()
            {
                var consoleLogger = new ConsoleLogger();
                var fileLogger = new FileLogger();

                var finders = new List<Pathfinder>
                {
                    //1) Пишет лог в файл. 
                    new Pathfinder(fileLogger),

                    //2) Пишет лог в консоль. 
                    new Pathfinder(consoleLogger),

                    //3) Пишет лог в файл по пятницам. 
                    new Pathfinder(new FridayLogger(fileLogger)),

                    //4) Пишет лог в консоль по пятницам. 
                    new Pathfinder(new FridayLogger(consoleLogger)),

                    //5) Пишет лог в консоль а по пятницам ещё и в файл.
                    new Pathfinder(new ConsoleLogger(new FridayLogger(fileLogger)))
                };

                finders.ForEach(finder => finder.Find());

                Console.ReadKey();
            }
        }

        public class Pathfinder
        {
            private const string Message = "ALARM!";
            private readonly ILogger _logger;

            public Pathfinder(ILogger logger)
            {
                if (logger == null) 
                {
                    throw new ArgumentNullException(nameof(logger));
                }

                _logger = logger;
            }

            public void Find()
            {
                _logger.Log(Message);
            }
        }

        public interface ILogger
        {
            void Log(string message);
        }

        class ConsoleLogger: ILogger
        {
            private readonly ILogger _logger;

            public ConsoleLogger(ILogger logger = null)
            {
                _logger = logger;
            }

            public void Log(string message)
            {
                Console.WriteLine(message);
                _logger?.Log(message);
            }
        }

        class FileLogger : ILogger
        {
            private const string LogPath = "log.txt";
            private readonly ILogger _logger;

            public FileLogger(ILogger logger = null)
            {
                _logger = logger;
            }

            public void Log(string message)
            {
                File.AppendAllText(LogPath, message + Environment.NewLine);
                _logger?.Log(message);
            }
        }

        class FridayLogger : ILogger
        {
            private readonly ILogger _logger;

            public FridayLogger(ILogger logger = null)
            {
                _logger = logger;
            }

            public void Log(string message)
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                {
                    _logger?.Log(message);
                }
            }
        }
    }
}
