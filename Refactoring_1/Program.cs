using System;
using System.Collections.Generic;

namespace Refactoring_1
{
    public enum PlayType
    {
        tragedy,
        comedy
    }

    class Program
    {
        public static string Statement(Invoice invoice, Dictionary<string, Play> plays)
        {
            Play PlayFor(Performances performance)
            {
                return plays[performance.playID];
            }
            int AmountFor(Performances performance)
            {
                int result;
                switch (PlayFor(performance).type)
                {
                    case PlayType.tragedy:
                        result = 40000;
                        if (performance.audience > 30)
                        {
                            result += 1000 * (performance.audience - 30);
                        }
                        break;
                    case PlayType.comedy:
                        result = 30000;
                        if (performance.audience > 20)
                        {
                            result += 10000 + 500 * (performance.audience - 20);
                        }
                        result += 300 * performance.audience;
                        break;
                    default:
                        throw new Exception($"알 수 없는 장르: {PlayFor(performance).type}");
                }

                return result;
            }
            int VolumeCreditFor(Performances performance)
            {
                var result = Math.Max(performance.audience - 30, 0);

                if (PlayFor(performance).type == PlayType.comedy) result += (int)MathF.Floor(performance.audience / 5);
                return result;
            }
            int TotalVolumeCredit()
            {
                var volumeCredits = 0;
                foreach (var perf in invoice.performances)
                {
                    volumeCredits += VolumeCreditFor(perf);
                }

                return volumeCredits;
            }
            int TotalAmount(Invoice invoice, Dictionary<string, Play> plays)
            {
                var totalAmount = 0;
                foreach (var perf in invoice.performances)
                {
                    totalAmount += AmountFor(perf);
                }

                return totalAmount;
            }

            var result = $"청구 내역 {invoice.customer}\n";

            foreach (var perf in invoice.performances)
            {
                result += $"{PlayFor(perf).name}: {USD(AmountFor(perf))} ({perf.audience}석)\n";
            }

            int totalAmount = TotalAmount(invoice, plays);

            result += $"총액: {USD(totalAmount)}\n";
            result += $"적립 포인트: {TotalVolumeCredit()}점\n";
            return result;

        }

        public static string USD(float arg)
        {
            return string.Format("${0:0,0.00}", arg / 100);
        }

        static void Main(string[] args)
        {
            Dictionary<string, Play> plays = new Dictionary<string, Play>();
            Invoice invoice;

            plays.Add("hamlet", new Play("Hamlet", PlayType.tragedy));
            plays.Add("as-like", new Play("As You Like It", PlayType.comedy));
            plays.Add("othello", new Play("Othello", PlayType.tragedy));

            invoice = new Invoice("BigCo",
                new List<Performances>(3) { new Performances("hamlet", 55), new Performances("as-like", 35), new Performances("othello", 40) }
                );

            Console.WriteLine(Statement(invoice, plays));
        }
    }

    public class Play
    {
        public string name;
        public PlayType type;

        public Play(string name, PlayType type)
        {
            this.name = name;
            this.type = type;
        }
    }

    public class Invoice
    {
        public string customer;
        public List<Performances> performances = new List<Performances>();

        public Invoice(string customer, List<Performances> performances)
        {
            this.customer = customer;
            this.performances = performances;
        }
    }

    public class Performances
    {
        public string playID;
        public int audience;

        public Performances(string playID, int audience)
        {
            this.playID = playID;
            this.audience = audience;
        }
    }

}
