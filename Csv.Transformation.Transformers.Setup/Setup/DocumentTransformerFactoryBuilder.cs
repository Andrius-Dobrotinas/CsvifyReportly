using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document.Setup
{
    /// <summary>
    /// Supposed to build new instances of transformers reusing dependencies
    /// </summary>
    public interface IDocumentTransformerFactoryBuilder
    {
        RowTransformer<ICellContentTransformer> BuildCellContentTransformerFactory(IRowTransformerFactory<ICellContentTransformer> transformerFactory);
        RowTransformer<IStructureTransformer> BuildStructureTransformerFactory(IRowTransformerFactory<IStructureTransformer> transformerFactory);
        RowFilterer BuildRowFiltererFactory(IDocumentTransformerFactory<Filtering.IRowMatchEvaluator> transformerFactory);
    }

    /// <inheritdoc/>
    public class DocumentTransformerFactoryBuilder : IDocumentTransformerFactoryBuilder
    {
        private readonly IColumnMapBuilder columnMapBuilder;
        private readonly ITransformationRunner<ICellContentTransformer> cellContentTransformerRunner;
        private readonly ITransformationRunner<IStructureTransformer> structureTransformerRunner;
        private readonly ITransformationRunner<Filtering.IRowMatchEvaluator> rowFilterRunner;
        private readonly FilteringResultReporter filteringReporter;
        private readonly TransformationResultReporter transformationReporter;

        public DocumentTransformerFactoryBuilder(
            IColumnMapBuilder columnMapBuilder,
            ITransformationRunner<ICellContentTransformer> cellContentTransformerRunner,
            ITransformationRunner<IStructureTransformer> structureTransformerRunner,
            ITransformationRunner<Filtering.IRowMatchEvaluator> rowFilterRunner,
            FilteringResultReporter filteringReporter,
            TransformationResultReporter transformationReporter)
        {
            this.columnMapBuilder = columnMapBuilder;
            this.cellContentTransformerRunner = cellContentTransformerRunner;
            this.structureTransformerRunner = structureTransformerRunner;
            this.rowFilterRunner = rowFilterRunner;
            this.filteringReporter = filteringReporter;
            this.transformationReporter = transformationReporter;
        }

        public RowTransformer<ICellContentTransformer> BuildCellContentTransformerFactory(IRowTransformerFactory<ICellContentTransformer> transformerFactory)
        {
            return new RowTransformer<ICellContentTransformer>(
                columnMapBuilder,
                transformerFactory,
                cellContentTransformerRunner,
                transformationReporter);
        }

        public RowTransformer<IStructureTransformer> BuildStructureTransformerFactory(IRowTransformerFactory<IStructureTransformer> transformerFactory)
        {
            return new RowTransformer<IStructureTransformer>(
                columnMapBuilder,
                transformerFactory,
                structureTransformerRunner,
                transformationReporter);
        }

        public RowFilterer BuildRowFiltererFactory(IDocumentTransformerFactory<Filtering.IRowMatchEvaluator> transformerFactory)
        {
            return new RowFilterer(
                columnMapBuilder,
                transformerFactory,
                rowFilterRunner,
                filteringReporter);
        }
    }
}