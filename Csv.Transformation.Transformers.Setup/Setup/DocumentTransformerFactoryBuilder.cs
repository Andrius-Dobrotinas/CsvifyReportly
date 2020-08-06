﻿using System;
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
        private readonly IResultReporter reporter;

        public DocumentTransformerFactoryBuilder(
            IColumnMapBuilder columnMapBuilder,
            ITransformationRunner<ICellContentTransformer> cellContentTransformerRunner,
            ITransformationRunner<IStructureTransformer> structureTransformerRunner,
            ITransformationRunner<Filtering.IRowMatchEvaluator> rowFilterRunner,
            IResultReporter reporter)
        {
            this.columnMapBuilder = columnMapBuilder;
            this.cellContentTransformerRunner = cellContentTransformerRunner;
            this.structureTransformerRunner = structureTransformerRunner;
            this.rowFilterRunner = rowFilterRunner;
            this.reporter = reporter;
        }

        public RowTransformer<ICellContentTransformer> BuildCellContentTransformerFactory(IRowTransformerFactory<ICellContentTransformer> transformerFactory)
        {
            return new RowTransformer<ICellContentTransformer>(
                columnMapBuilder,
                transformerFactory,
                cellContentTransformerRunner,
                reporter);
        }

        public RowTransformer<IStructureTransformer> BuildStructureTransformerFactory(IRowTransformerFactory<IStructureTransformer> transformerFactory)
        {
            return new RowTransformer<IStructureTransformer>(
                columnMapBuilder,
                transformerFactory,
                structureTransformerRunner,
                reporter);
        }

        public RowFilterer BuildRowFiltererFactory(IDocumentTransformerFactory<Filtering.IRowMatchEvaluator> transformerFactory)
        {
            return new RowFilterer(
                columnMapBuilder,
                transformerFactory,
                rowFilterRunner,
                reporter);
        }
    }
}