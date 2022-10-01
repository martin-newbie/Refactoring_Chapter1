﻿using System;
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
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = $"청구 내역 {invoice.customer}\n";

            foreach (var perf in invoice.performances)
            {
                var play = plays[perf.playID];
                var thisAmount = 0;

                switch (play.type)
                {
                    case PlayType.tragedy:
                        thisAmount = 40000;
                        if (perf.audience > 30)
                        {
                            thisAmount += 1000 * (perf.audience - 30);
                        }
                        break;
                    case PlayType.comedy:
                        thisAmount = 30000;
                        if (perf.audience > 20)
                        {
                            thisAmount += 10000 + 500 * (perf.audience - 20);
                        }
                        thisAmount += 300 * perf.audience;
                        break;
                    default:
                        throw new Exception($"알 수 없는 장르: {play.type}");
                }

                volumeCredits += Math.Max(perf.audience - 30, 0);

                if (play.type == PlayType.comedy) volumeCredits += (int)MathF.Floor(perf.audience / 5);

                result += $"{play.name}: {format(thisAmount / 100)} ({perf.audience}석)\n";
                totalAmount += thisAmount;
            }

            result += $"총액: {format(totalAmount / 100)}\n";
            result += $"적립 포인트: {volumeCredits}점\n";
            return result;
        }

        public static string format(float arg)
        {
            return string.Format("${0:0.00}", arg);
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