using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document.Cmd
{
    internal static class StructureTransformer
    {
        internal static IRowTransformerFactory<IStructureTransformer> Build_ColumnReducer(Settings.TransformationSettings.ColumnReducerSettings settings)
        {
            return new ColumnReducerFactory(settings.TargetColumnNames);
        }

        internal static IRowTransformerFactory<IStructureTransformer> Build_ColumnInserter(
            Settings.TransformationSettings.ColumnInserterSettings settings)
        {
            return new ColumnInserterFactory(
                settings.TargetColumnIndex,
                settings.TargetColumnName,
                new CellInserter<string>(
                    new ArrayElementInserter<string>()));
        }
    }
}