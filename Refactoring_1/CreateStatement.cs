using System;
using System.Collections.Generic;
using System.Linq;


namespace Refactoring_1
{
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

            Performances EnrichPerformance(Performances performances)
            {
                var result = performances.Clone();
                result.play = PlayFor(result);
                result.amount = AmountFor(result);
                result.volumeCredits = VolumeCreditFor(result);
                return result;
            }
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
