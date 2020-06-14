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

        public DocumentTransformerFactory(
            IColumnMapBuilder columnMapBuilder,
            ITransformationRunner<ICellContentTransformer> transformerRunner,
            ITransformationRunner<IStructureTransformer> structureTransformerRunner)
        {
            this.columnMapBuilder = columnMapBuilder;
            this.cellContentTransformerRunner = transformerRunner;
            this.structureTransformerRunner = structureTransformerRunner;
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
    }
}