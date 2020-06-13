using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document.Cmd
{
    /// <summary>
    /// Builds new instances of <see cref="RowTransformer<IContentTransformer>"/> while reusing
    /// the instances of other dependencies
    /// </summary>
    public class CellContentTransformerFactory
    {
        private readonly IColumnMapBuilder columnMapBuilder;
        private readonly ITransformationRunner<ICellContentTransformer> transformerRunner;

        public CellContentTransformerFactory(
            IColumnMapBuilder columnMapBuilder,
            ITransformationRunner<ICellContentTransformer> transformerRunner)
        {
            this.columnMapBuilder = columnMapBuilder;
            this.transformerRunner = transformerRunner;
        }

        public RowTransformer<ICellContentTransformer> Build(IRowTransformerFactory<ICellContentTransformer> transformerFactory)
        {
            return new RowTransformer<ICellContentTransformer>(
                columnMapBuilder,
                transformerFactory,
                transformerRunner);
        }
    }
}