using Andy.ExpenseReport.Comparison;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Verifier.Comparison
{
    public class Comparer
    {
        public static ComparisonResult Compare(
            IList<string[]> transactionRows,
            IList<string[]> statementRows,
            TransactionDetailsColumnIndexes transactionColumnMapping,
            StatementEntryColumnIndexes statementColumnMapping,
            IDictionary<string, string[]> nameMap)
        {
            var transactionRowParser = new TransactionDetailsParser(transactionColumnMapping);
            var statementRowParser = new StatementEntryParser(statementColumnMapping);

            var transactions = transactionRows.Select(transactionRowParser.Parse).ToArray();
            var statementEntries = statementRows.Select(statementRowParser.Parse).ToArray();

            var comparer = new CollectionComparer<
                    StatementEntryWithSourceData,
                    TransactionDetailsWithSourceData>(
                new MatchFinder<
                    StatementEntryWithSourceData,
                    TransactionDetailsWithSourceData>(
                    new ItemComparer(
                        new MerchantNameComparer(
                            new MerchanNameMapComparer(nameMap)))));

            var result = comparer.Compare(
                    statementEntries,
                    transactions);

            var matchRows = result.Matches
                .Select(
                        x => new Tuple<string[], string[]>(
                            x.Item1.SourceData,
                            x.Item2.SourceData))
                .ToArray();

            var unmatchedStatementEntries = result.UnmatchedStatementEntries
                .Select(x => x.SourceData)
                .ToArray();

            var unmatchedTransactions = result.UnmatchedTransactions
                .Select(x => x.SourceData)
                .ToArray();

            return new ComparisonResult
            {
                Matches = matchRows,
                UnmatchedStatementEntries = unmatchedStatementEntries,
                UnmatchedTransactions = unmatchedTransactions
            };
        }
    }
}