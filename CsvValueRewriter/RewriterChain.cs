using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Rewrite
{
    public static class RewriterChain
    {
        private static class RewriterKeys
        {
            public const string DateRewriter = "DateRewriter";
        }

        public static IList<ICsvRewriter> GetRewriterChain(Settings settings)
        {
            var csvRewriters = new List<ICsvRewriter>(settings.RewriterChain.Length);

            if (settings.RewriterChain.Contains(RewriterKeys.DateRewriter))
                csvRewriters.Add(GetDateRewriter(settings.Rewriters.DateRewriter));

            return csvRewriters;
        }

        private static ICsvRewriter GetDateRewriter(Settings.RewriterSettings.DateRewriterSettings settings)
        {
            IRowRewriter rowRewriter = new RowSingleValueRewriter(
                    settings.TargetColumnIndex,
                    new Rewriters.DateRewriter(settings.SourceFormat, settings.TargetFormat));

            return new CsvRewriter(rowRewriter);
        }
    }
}