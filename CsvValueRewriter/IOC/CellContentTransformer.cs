﻿using Andy.Csv.Transformation.Row.Document.Cmd.Conguration.Transformer;
using System;
using System.Collections.Generic;

namespace Andy.Csv.Transformation.Row.Document.Cmd
{
    internal static class CellContentTransformer
    {
        internal static IRowTransformerFactory<ICellContentTransformer> Build_DateRewriter(
            DateRewriterSettings settings)
        {
            return new SingleValueTransformerFactory(
                settings.TargetColumnName,
                    new DateTransformer(settings.SourceFormat, settings.TargetFormat));
        }

        internal static IRowTransformerFactory<ICellContentTransformer> Build_TheCurrencyAmountThing(CurrencyAmountThingSettings settings)
        {
            return new CurrencyAmount_CantThinkOfNameFactory(
                settings.AmountColumnName,
                settings.CurrencyColumnName,
                settings.ResultAmountColumnName,
                new TargetCurrencyValueSelector(settings.TargetCurrency));
        }
    }
}