using Andy.ExpenseReport.Comparison.Csv.CsvStream;
using Andy.ExpenseReport.Verifier.Cmd;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier.Cmd
{
    public static class FileComparerBuilder
    {
        public static ReportingFileComparer BuildFileComparer<TEntry1, TEntry2>(
            Comparison.Csv.IComparerFactory<TEntry1, TEntry2> comparerFactory,
            Comparison.Csv.Statement.IStatementEntryParserFactory<TEntry1> entry1ParserFactory,
            Comparison.Csv.Statement.IStatementEntryParserFactory<TEntry2> entry2ParserFactory,
            IEnumerable<Csv.Transformation.Row.Document.IDocumentTransformer> doc1Transformers,
            IEnumerable<Csv.Transformation.Row.Document.IDocumentTransformer> doc2Transformers,
            char source1CsvDelimiter,
            char source2CsvDelimiter)
        {
            return new ReportingFileComparer(
                    new ReportingComparer<TEntry1, TEntry2>(
                            Build_CsvStreamReader(source1CsvDelimiter),
                            Build_CsvStreamReader(source2CsvDelimiter),
                            new Csv.Transformation.Row.Document.MultiTransformer(doc1Transformers),
                            new Csv.Transformation.Row.Document.MultiTransformer(doc2Transformers),
                            entry1ParserFactory,
                            entry2ParserFactory,
                            comparerFactory,
                            new Csv.Transformation.Row.Document.ColumnMapBuilder()));
        }

        private static Csv.IO.ICsvDocumentByteStreamReader Build_CsvStreamReader(char csvDelimiter)
        {
            // todo: i want to reuse some of these instances with the other reader

            return new Csv.IO.CsvDocumentByteStreamReader(
                new Csv.IO.RowLengthValidatingCsvRowByteStreamReader(
                    new Csv.IO.CsvReenumerableRowByteStreamReader(
                        new Csv.IO.CsvRowByteStreamReader(
                            new Csv.IO.CellByteStreamReader(
                                new Csv.Serialization.RowParser(csvDelimiter)),
                            new Csv.IO.StreamReaderFactory(),
                            new Csv.IO.StreamReaderPositionReporter()))),
                new Csv.ArrayValueUniquenessChecker());
        }

        public static IEnumerable<Csv.Transformation.Row.Document.IDocumentTransformer> GetTransformerChain(
            IDictionary<string, Csv.Transformation.Row.TransformerSettings[]> transformationProfiles, 
            string profileName)
        {
            return Csv.Transformation.Row.Document.Setup.Profile.GetTransformerChain(transformationProfiles, profileName);
        }
    }
}