using Andy.Csv;
using Andy.Csv.IO;
using Andy.Csv.Transformation.Row.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Csv.CsvStream
{
    public class ReportingComparer<TItem1, TItem2> : IReportingComparer
    {
        private readonly ICsvDocumentByteStreamReader csvStream1Reader;
        private readonly ICsvDocumentByteStreamReader csvStream2Reader;
        private readonly IMultiTransformer transformer1;
        private readonly IMultiTransformer transformer2;
        private readonly Statement.IStatementEntryParserFactory<TItem1> item1ParserFactory;
        private readonly Statement.IStatementEntryParserFactory<TItem2> item2ParserFactory;
        private readonly IComparerFactory<TItem1, TItem2> comparerFactory;
        private readonly IColumnMapBuilder columnMapBuilder;

        public ReportingComparer(
            ICsvDocumentByteStreamReader csvStream1Reader,
            ICsvDocumentByteStreamReader csvStream2Reader,
            IMultiTransformer transformer1,
            IMultiTransformer transformer2,
            Statement.IStatementEntryParserFactory<TItem1> item1ParserFactory,
            Statement.IStatementEntryParserFactory<TItem2> item2ParserFactory,
            IComparerFactory<TItem1, TItem2> comparerFactory,
            IColumnMapBuilder columnMapBuilder)
        {
            this.csvStream1Reader = csvStream1Reader ?? throw new ArgumentNullException(nameof(csvStream1Reader));
            this.csvStream2Reader = csvStream2Reader ?? throw new ArgumentNullException(nameof(csvStream2Reader));
            this.transformer1 = transformer1 ?? throw new ArgumentNullException(nameof(transformer1));
            this.transformer2 = transformer2 ?? throw new ArgumentNullException(nameof(transformer2));
            this.comparerFactory = comparerFactory ?? throw new ArgumentNullException(nameof(comparerFactory));
            this.columnMapBuilder = columnMapBuilder ?? throw new ArgumentNullException(nameof(columnMapBuilder));
            this.item1ParserFactory = item1ParserFactory ?? throw new ArgumentNullException(nameof(item1ParserFactory));
            this.item2ParserFactory = item2ParserFactory ?? throw new ArgumentNullException(nameof(item2ParserFactory));
        }

        public Stream Compare(
            Stream source1,
            Stream source2,
            char reportValueDelimiter)
        {
            CsvDocument doc1 = Read(
                    csvStream1Reader,
                    1,
                    source1);

            CsvDocument doc2 = Read(
                    csvStream2Reader,
                    2,
                    source2);

            doc1 = TransformWithErrorHandling(doc1, transformer1, 1);
            doc2 = TransformWithErrorHandling(doc2, transformer2, 2);

            var comparer = BuildComparer(doc1.HeaderCells, doc2.HeaderCells);

            ComparisonResult result;
            try
            {
                result = comparer.Compare(
                    doc1.ContentRows,
                    doc2.ContentRows);
            }
            catch (InputParsingException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new DataComparisonException(e);
            }

            var stringyfyer = new Andy.Csv.Serialization.RowStringifier(
                new Andy.Csv.Serialization.CellValueEncoder());

            try
            {
                string[] lines = ResultStringification.StringyfyyResults(
                    result,
                    doc1.HeaderCells,
                    doc2.HeaderCells,
                    reportValueDelimiter,
                    stringyfyer);

                return CsvFileWriter.Write(lines);
            }
            catch (Exception e)
            {
                throw new ReportProductionException(e);
            }
        }

        private IComparer<TItem1, TItem2> BuildComparer(string[] headerCells1, string[] headerCells2)
        {
            var columnIndexes1 = columnMapBuilder.GetColumnIndexMap(headerCells1);
            var item1Parser = item1ParserFactory.Build(columnIndexes1);

            var columnIndexes2 = columnMapBuilder.GetColumnIndexMap(headerCells2);
            var item2Parser = item2ParserFactory.Build(columnIndexes2);

            return comparerFactory.Build(item1Parser, item2Parser);
        }

        private static CsvDocument Read(
            ICsvDocumentByteStreamReader csvStreamReader,
            int sourceNumber,
            Stream source)
        {
            try
            {
                return csvStreamReader.Read(source);
            }
            catch (RowReadingException e)
            {
                throw new SourceDataReadException(e.Message, sourceNumber, e.InnerException);
            }
            catch (Exception e)
            {
                throw new SourceDataReadException(sourceNumber, e);
            }
        }

        private static CsvDocument TransformWithErrorHandling(CsvDocument source, IMultiTransformer transformer, int sourceFileNumber)
        {
            try
            {
                return transformer.Transform(source);
            }
            catch (Exception e)
            {
                throw new Exception($"An error occurred while transforming source file {sourceFileNumber}", e);
            }
        }
    }
}