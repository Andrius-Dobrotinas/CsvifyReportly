using System;
using System.Collections.Generic;
using System.Text;

namespace Andy.Csv.Transformation.Row.Document.Cmd
{
    internal static class StructureTransformer
    {
        internal static IRowTransformerFactory<IStructureTransformer> Build_ColumnReducer(Settings.TransformationSettings.ColumnReducerSettings settings)
        {
            return new ColumnReducerFactory(settings.TargetColumnNames);
        }
    }
}