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
                var fridayConsoleLogger = DayOfWeekLogger.CreateFridayLogger(consoleLogger);
                var fridayFileLogger = DayOfWeekLogger.CreateFridayLogger(fileLogger);

                var finders = new List<Pathfinder>
                {
                    //1) Пишет лог в файл. 
                    new Pathfinder(fileLogger),

                    //2) Пишет лог в консоль. 
                    new Pathfinder(consoleLogger),

                    //3) Пишет лог в файл по пятницам. 
                    new Pathfinder(fridayFileLogger),

                    //4) Пишет лог в консоль по пятницам. 
                    new Pathfinder(fridayConsoleLogger),

                    //5) Пишет лог в консоль а по пятницам ещё и в файл.
                    new Pathfinder(new CompositeLogger(consoleLogger, fridayFileLogger))
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
            public void Log(string message)
            {
                Console.WriteLine(message);
            }
        }

        class FileLogger : ILogger
        {
            private const string LogPath = "log.txt";

            public void Log(string message)
            {
                File.AppendAllText(LogPath, message + Environment.NewLine);
            }
        }

        class CompositeLogger : ILogger
        {
            private readonly IEnumerable<ILogger> _loggers;

            public CompositeLogger(params ILogger[] loggers)
            {
                _loggers = new List<ILogger>(
                    loggers ?? 
                    throw new ArgumentNullException(nameof(loggers)));
            }

            public void Log(string message)
            {
                foreach(var logger in _loggers)
                {
                    logger.Log(message);
                }
            }
        }

        class DayOfWeekLogger : ILogger
        {
            private readonly ILogger _logger;
            private readonly DayOfWeek _dayOfWeek;

            private DayOfWeekLogger(ILogger logger, DayOfWeek dayOfWeek)
            {
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
                _dayOfWeek = dayOfWeek;
            }

            public static DayOfWeekLogger CreateFridayLogger(ILogger logger)
            {
                return new DayOfWeekLogger(logger, DayOfWeek.Friday);
            }

            public void Log(string message)
            {
                if (DateTime.Now.DayOfWeek == _dayOfWeek)
                {
                    _logger?.Log(message);
                }
            }
        }
    }
}
