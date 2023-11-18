using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReplacingConditionalLogicWithPolymorphism_Task
{
    using static Printer;

    public static class Printer
    {
        public static void Print(string message)
        {
            Console.WriteLine(message);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var paymentSystems = new List<string>()
            {
                "QIWI",
                "WebMoney",
                "Card"
            };

            var paymentSystemsFactory = new PaymentSystemFactory(paymentSystems);
            var orderForm = new OrderForm(paymentSystems);
            var paymentHandler = new PaymentHandler();

            var paymentSystemName = orderForm.SelectPaymentSystemOrDie();

            Print($"Использование API {paymentSystemName}");

            if (paymentSystemsFactory.TryCreate(paymentSystemName, out var paymentSystem))
            {
                paymentHandler.ShowPaymentResult(paymentSystem);
            }
            else
            {
                Print($"Платежная система {paymentSystemName} сейчас не доступна");
            }

            Exit();
        }

        public static void Exit()
        {
            Print("\nДля продолжения нажмите любую клавишу . . .");
            Console.ReadKey();
            Environment.Exit(0);
        }

    }

    public class OrderForm
    {
        private readonly IEnumerable<string> _paymentSystems;

        public OrderForm(IEnumerable<string> paymentSystems)
        {
            _paymentSystems = paymentSystems ??
                throw new ArgumentNullException(nameof(paymentSystems));
        }

        public string SelectPaymentSystemOrDie()
        {
            var namesString = string.Join(", ", _paymentSystems);

            Print($"Мы принимаем: {namesString}");
            Print("Введите имя системы оплаты :");

            var selectionInput = Console.ReadLine().ToLower();

            if (!_paymentSystems.Contains(selectionInput, StringComparer.OrdinalIgnoreCase))
            {
                Print("\nВы неверно указали платежную систему.");
                Print("Похоже, вы недостойны быть нашим покупателем.");
                Print("Ваша учетная запись была заблокирована.");
                Print("Прощайте.");

                Program.Exit();
            }

            return selectionInput;
        }
    }

    public class PaymentHandler
    {
        public void ShowPaymentResult(PaymentSystem paymentSystem)
        {
            var name = paymentSystem.Name;

            Print($"Вы оплатили с помощью {name}");
            Print($"Проверка платежа через {name}...");
            Print("Оплата прошла успешно!");
        }
    }

    #region Payment Systems

    public abstract class PaymentSystem
    {
        public abstract string Name { get; }
    }

    public class Qiwi : PaymentSystem
    {
        public override string Name => "QIWI";
    }

    public class WebMoney : PaymentSystem
    {
        public override string Name => "WebMoney";
    }

    public class Card : PaymentSystem
    {
        public override string Name => "Card";
    }



    public class PaymentSystemFactory
    {
        private readonly Dictionary<string, Func<PaymentSystem>> _creators = new Dictionary<string, Func<PaymentSystem>>();

        public PaymentSystemFactory(IEnumerable<string> supportedSystemIds)
        {
            if (supportedSystemIds == null)
            {
                throw new ArgumentNullException(nameof(supportedSystemIds));
            }

            var paymentSystems = GetExistingPaymantsSystemsMap();

            foreach (var systemId in supportedSystemIds)
            {
                var className = systemId.ToLower();

                if (!paymentSystems.ContainsKey(className))
                {
                    throw new InvalidOperationException("Данной системы нет в списке поддерживаемых платежных систем");
                }

                var type = paymentSystems[className];
                var hasConstructorWithoutArsuments = type.GetConstructors()
                    .Any(c => c.GetParameters().Length == 0);

                if (!hasConstructorWithoutArsuments)
                {
                    throw new InvalidOperationException("Данная платежная система не содержит конструкторов по умолчанию.");
                }

                _creators.Add(className, () => (PaymentSystem)Activator.CreateInstance(paymentSystems[className]));
            }
        }

        public bool TryCreate(string systemId, out PaymentSystem paymentSystem)
        {
            paymentSystem = null;

            if (_creators.TryGetValue(systemId.ToLower(), out var create))
            {
                try
                {
                    paymentSystem = create();
                    return true;
                }
                catch (Exception ex)
                {
                    //TODO: logging
                    return false;
                }
            }
            return false;
        }

        private static Dictionary<string, Type> GetExistingPaymantsSystemsMap()
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => type.IsSubclassOf(typeof(PaymentSystem)))
                .ToDictionary(type => type.Name.ToLower(), type => type);
        }
    }

    #endregion Payment Systems
}
