using System;
using System.Collections.Generic;
using System.Linq;

namespace ReplacingConditionalLogicWithPolymorphism_Task
{
    public class Printer
    {
        protected static void Print(string message)
        {
            Console.WriteLine(message);
        }
    }

    internal class Program : Printer
    {
        static void Main(string[] args)
        {
            var paymentSystemsProvider = new SupportPaymentSystemsProviderFactory().Create();
            var orderForm = new OrderForm(paymentSystemsProvider);
            var paymentHandler = new PaymentHandler();

            var paymentSystem = orderForm.SelectPaymentSystemOrDie();

            Print($"Использование API {paymentSystem.Name}");

            paymentHandler.ShowPaymentResult(paymentSystem);
            
            Exit();
        }

        public static void Exit()
        {
            Print("\nДля продолжения нажмите любую клавишу . . .");
            Console.ReadKey();
            Environment.Exit(0);
        }

    }

    public class OrderForm : Printer
    {
        private readonly SupportedPaymentSystemsProvider _paymentSystemsProvider;

        public OrderForm(SupportedPaymentSystemsProvider paymentSystemsProvider)
        {
            _paymentSystemsProvider = paymentSystemsProvider ??
                throw new ArgumentNullException(nameof(paymentSystemsProvider));
        }

        public PaymentSystem SelectPaymentSystemOrDie()
        {
            var paymentSystems  = _paymentSystemsProvider.PaymentSystems;
            var names = paymentSystems.Values.Select(system => system.Name);
            var namesString = string.Join(", ", names);

            Print($"Мы принимаем: {namesString}");
            Print("Введите имя системы оплаты :");

            var selectionInput = Console.ReadLine();

            if (!paymentSystems.TryGetValue(selectionInput.ToLower(), out var paymentSystem))
            {
                Print("\nВы неверно указали платежную систему.");
                Print("Похоже, вы недостойны быть нашим покупателем.");
                Print("Ваша учетная запись была заблокирована.");
                Print("Прощайте.");

                Program.Exit();
            }

            return paymentSystem;
        }
    }

    public class PaymentHandler : Printer
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

    public class SupportedPaymentSystemsProvider
    {
        private readonly Dictionary<string, PaymentSystem> _paymentSystems = new Dictionary<string, PaymentSystem>();

        public SupportedPaymentSystemsProvider(params PaymentSystem[] paymentSystems)
        {
            if (paymentSystems is null)
            {
                throw new ArgumentNullException(nameof(paymentSystems));
            }

            foreach (var paymentSystem in paymentSystems)
            {
                if (paymentSystem is null)
                {
                    throw new NullReferenceException($"{nameof(paymentSystems)} can't cointains null values");
                }

                _paymentSystems[paymentSystem.Name.ToLower()] = paymentSystem;
            }
        }

        public IReadOnlyDictionary<string, PaymentSystem> PaymentSystems => _paymentSystems;
    }

    public class SupportPaymentSystemsProviderFactory
    {
        public SupportedPaymentSystemsProvider Create()
        {
            return new SupportedPaymentSystemsProvider(new Qiwi(), new WebMoney(), new Card());
        }
    }

    #endregion Payment Systems
}
