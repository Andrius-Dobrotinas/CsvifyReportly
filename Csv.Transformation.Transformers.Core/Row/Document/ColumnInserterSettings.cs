﻿using System;

namespace Andy.Csv.Transformation.Row.Document
{
    public class ColumnInserterSettings : TransformerSettings
    {
        public int TargetColumnIndex { get; set; }
        public string TargetColumnName { get; set; }

        public override IDocumentTransformerFactory BuildFactory(ICultureSettings globalSettings)
        {
            return new ColumnInserterFactory(
                this.GetDescription(),
                TargetColumnIndex,
                TargetColumnName,
                new CellInserter<string>(
                    new ArrayElementInserter<string>()));
        }
    }
}