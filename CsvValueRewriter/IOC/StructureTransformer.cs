using Andy.Csv.Transformation.Row.Document.Cmd.Configuration.Transformer;
using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document.Cmd
{
    internal static class StructureTransformer
    {
        internal static IRowTransformerFactory<IStructureTransformer> Build_ColumnReducer(ColumnReducerSettings settings)
        {
            return new ColumnReducerFactory(settings.TargetColumnNames);
        }

        internal static IRowTransformerFactory<IStructureTransformer> Build_ColumnInserter(
            ColumnInserterSettings settings)
        {
            return new ColumnInserterFactory(
                settings.TargetColumnIndex,
                settings.TargetColumnName,
                new CellInserter<string>(
                    new ArrayElementInserter<string>()));
        }
    }
}