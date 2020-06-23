using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document.Cmd
{
    /// <summary>
    /// Builds new instances of non-filter transformers while reusing
    /// the instances of other dependencies
    /// </summary>
    public class DocumentTransformerFactory
    {
        private readonly IColumnMapBuilder columnMapBuilder;
        private readonly ITransformationRunner<ICellContentTransformer> cellContentTransformerRunner;
        private readonly ITransformationRunner<IStructureTransformer> structureTransformerRunner;
        private readonly ITransformationRunner<Filtering.IRowMatchEvaluator> rowFilterRunner;

        public DocumentTransformerFactory(
            IColumnMapBuilder columnMapBuilder,
            ITransformationRunner<ICellContentTransformer> transformerRunner,
            ITransformationRunner<IStructureTransformer> structureTransformerRunner,
            ITransformationRunner<Filtering.IRowMatchEvaluator> rowFilterRunner)
        {
            this.columnMapBuilder = columnMapBuilder;
            this.cellContentTransformerRunner = transformerRunner;
            this.structureTransformerRunner = structureTransformerRunner;
            this.rowFilterRunner = rowFilterRunner;
        }

        public RowTransformer<ICellContentTransformer> Build(IRowTransformerFactory<ICellContentTransformer> transformerFactory)
        {
            return new RowTransformer<ICellContentTransformer>(
                columnMapBuilder,
                transformerFactory,
                cellContentTransformerRunner);
        }

        public RowTransformer<IStructureTransformer> Build(IRowTransformerFactory<IStructureTransformer> transformerFactory)
        {
            return new RowTransformer<IStructureTransformer>(
                columnMapBuilder,
                transformerFactory,
                structureTransformerRunner);
        }

        public RowFilterer Build(IDocumentTransformerFactory<Filtering.IRowMatchEvaluator> transformerFactory)
        {
            return new RowFilterer(
                columnMapBuilder,
                transformerFactory,
                rowFilterRunner);
        }
    }
}