using System;

namespace Andy.Csv.Transformation.Row.Document.Cmd.Transformer
{
    public class ColumnInserterSettings : TransformerSettings
    {
        public int TargetColumnIndex { get; set; }
        public string TargetColumnName { get; set; }

        public override IDocumentTransformerFactory BuildFactory()
        {
            return new ColumnInserterFactory(
                TargetColumnIndex,
                TargetColumnName,
                new CellInserter<string>(
                    new ArrayElementInserter<string>()));
        }
    }
}