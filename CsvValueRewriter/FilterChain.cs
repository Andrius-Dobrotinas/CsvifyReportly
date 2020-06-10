using Andy.Csv.Transformation.Filter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Cmd
{
    public static class FilterChain
    {
        private class Key
        {
            internal const string InvertedSingleValue = "InvertedSingleValue";
        }

        public static IRowMatchEvaluator[] GetFilterChain(Settings settings, string targetChainName)
        {
            var filterNames = GetFilterNames(settings, targetChainName);

            return filterNames
                .Select(name => GetFilter(name, settings.Filters))
                .ToArray();
        }

        private static IRowMatchEvaluator GetFilter(string name, Settings.FilterSettings filterSettings)
        {
            switch (name)
            {
                case Key.InvertedSingleValue:
                    return Build_InvertedSingleRowValueEvaluator(filterSettings.InvertedSingleValue);
                default:
                    throw new NotImplementedException($"Value: {name}");
            }
        }

        private static string[] GetFilterNames(Settings settings, string targetChainName)
        {
            if (settings.FilterChains.Count == 0)
            {
                if (string.IsNullOrEmpty(targetChainName))
                    return new string[0];
            }
            else
            {
                if (string.IsNullOrEmpty(targetChainName))
                    return settings.FilterChains.FirstOrDefault().Value;

                string[] result;
                if (settings.FilterChains.TryGetValue(targetChainName, out result))
                    return result;
            }

            throw new Exception($"Filter chain {targetChainName} does not exist");
        }

        private static IRowMatchEvaluator Build_InvertedSingleRowValueEvaluator(Settings.FilterSettings.InvertedSingleRowValueEvaluatorSettings settings)
        {
            return new InvertedSingleRowValueEvaluator(
                settings.TargetColumnIndex,
                settings.TargetValue);
        }
    }
}