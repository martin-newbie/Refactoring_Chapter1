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

            return RenderPlainText(CreateStatement.CreateStatementData(invoice, plays));

        }

        static string RenderPlainText(Invoice data)
        {
            var result = $"청구 내역 {data.customer}\n";

            foreach (var perf in data.performances)
            {
                result += $"{perf.play.name}: {USD(perf.amount)} ({perf.audience}석)\n";
            }

            result += $"총액: {USD(data.totalAmount)}\n";
            result += $"적립 포인트: {data.totalVolumeCredits}점\n";
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

        public int totalAmount;
        public int totalVolumeCredits;

        public Invoice(string customer, List<Performances> performances)
        {
            this.customer = customer;
            this.performances = performances;
        }

        public Invoice()
        {

        }
    }

    public class Performances
    {
        public string playID;
        public int audience;
        public int amount;
        public int volumeCredits;
        public Play play;

        public Performances(string playID, int audience)
        {
            this.playID = playID;
            this.audience = audience;
        }

        internal Performances Clone()
        {
            return new Performances(this.playID, this.audience);
        }
    }

}
