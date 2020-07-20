﻿using System;

namespace Andy.Csv.Transformation.Row.Document.Cmd.Transformer
{
    public class ColumnReducerSettings : TransformerSettings
    {
        public string[] TargetColumnNames { get; set; }

        public override IDocumentTransformerFactory BuildFactory()
        {
            return new ColumnReducerFactory(TargetColumnNames);
        }
    }
}