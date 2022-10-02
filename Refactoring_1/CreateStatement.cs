using System;
using System.Collections.Generic;
using System.Linq;


namespace Refactoring_1
{
    public class PerformanceCalculator
    {
        public Performances performance;
        public Play play;
        public virtual int amount { get; }
        public int volumeCredits
        {
            get
            {
                var result = Math.Max(performance.audience - 30, 0);

                if (play.type == PlayType.comedy) result += (int)MathF.Floor(performance.audience / 5);
                return result;
            }
        }

        public PerformanceCalculator(Performances performance, Play play)
        {
            this.performance = performance;
            this.play = play;
        }
    }

    public class TragedyCalculator : PerformanceCalculator
    {
        public override int amount
        {
            get
            {
                int result = 40000;
                if (performance.audience > 30)
                {
                    result += 1000 * (performance.audience - 30);
                }

                return result;
            }
        }
        public TragedyCalculator(Performances performance, Play play) : base(performance, play)
        {
        }
    }

    public class ComedyCalculator : PerformanceCalculator
    {
        public override int amount
        {
            get
            {
                int result = 30000;
                if (performance.audience > 20)
                {
                    result += 10000 + 500 * (performance.audience - 20);
                }
                result += 300 * performance.audience;

                return result;
            }
        }

        public ComedyCalculator(Performances performance, Play play) : base(performance, play)
        {
        }
    }


    class CreateStatement
    {

        public static Invoice CreateStatementData(Invoice invoice, Dictionary<string, Play> plays)
        {
            Invoice statementData = new Invoice();


            statementData.customer = invoice.customer;
            statementData.performances = invoice.performances.Select(EnrichPerformance).ToList();
            statementData.totalAmount = TotalAmount(statementData);
            statementData.totalVolumeCredits = TotalVolumeCredit(statementData);

            return statementData;

            PerformanceCalculator CreatePerformanceCalculator(Performances perf, Play play)
            {

                switch (play.type)
                {
                    case PlayType.tragedy: return new TragedyCalculator(perf, play);
                    case PlayType.comedy: return new ComedyCalculator(perf, play);
                    default:
                        throw new Exception($"알 수 없는 장르: {play.type}");
                }
            }
            Performances EnrichPerformance(Performances performances)
            {
                PerformanceCalculator calculator = CreatePerformanceCalculator(performances, PlayFor(performances));
                var result = performances.Clone();

                result.play = calculator.play;
                result.amount = calculator.amount;
                result.volumeCredits = calculator.volumeCredits;
                return result;
            }
            Play PlayFor(Performances performance)
            {
                return plays[performance.playID];
            }
            int AmountFor(Performances performance)
            {// todo 없어도 될 것 같음
                return new PerformanceCalculator(performance, PlayFor(performance)).amount;
            }
            int VolumeCreditFor(Performances performance)
            {
                var result = Math.Max(performance.audience - 30, 0);

                if (performance.play.type == PlayType.comedy) result += (int)MathF.Floor(performance.audience / 5);
                return result;
            }
            int TotalAmount(Invoice data)
            {
                return data.performances.Aggregate(0, (total, p) => total + p.amount);
            }
            int TotalVolumeCredit(Invoice data)
            {
                return data.performances.Aggregate(0, (total, p) => total + p.volumeCredits);
            }
        }

    }
}
