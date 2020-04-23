using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Comparison.Statement.Bank
{
    public class MerchanNameMapComparerTests
    {
        [TestCaseSource(nameof(GetNameMatchCases))]
        public void When_NameMapDoesNotContainAKey_ThatCorrespondsToAGivenMerchantName__Must_Return_False(
            IDictionary<string, string[]> map,
            string merchantName,
            string detailsString)
        {
            var target = new MerchanNameMapComparer(map);

            var result = target.IsMatch(merchantName, detailsString);

            Assert.IsFalse(result);
        }        

        [TestCaseSource(nameof(GetSuccessfulVariationMatchCases))]
        public void When_MerchantNameIsMapped_AndDetailsStringMatchesANameVariation__Should_Return_True(
            IDictionary<string, string[]> map,
            string merchantName,
            string detailsString)
        {
            var target = new MerchanNameMapComparer(map);

            var result = target.IsMatch(merchantName, detailsString);

            Assert.IsTrue(result);
        }

        [TestCaseSource(nameof(GetCasesForWhenNameVariationFormsFirstPartOfDetailsString))]
        public void When_MerchantNameIsMapped_AndDetailsStringStartsWithOneOfNameVariations__Should_Return_True(
            IDictionary<string, string[]> map,
            string merchantName,
            string detailsString)
        {
            var target = new MerchanNameMapComparer(map);

            var result = target.IsMatch(merchantName, detailsString);

            Assert.IsTrue(result);
        }

        [TestCaseSource(nameof(GetSuccessulCaseInsensitiveVariationMatchCases))]
        public void Should_CompareVariationsInACaseInsensitiveManner(
            IDictionary<string, string[]> map,
            string merchantName,
            string detailsString)
        {
            var target = new MerchanNameMapComparer(map);

            var result = target.IsMatch(merchantName, detailsString);

            Assert.IsTrue(result);
        }

        [TestCaseSource(nameof(GetNoNameVariationMatches))]
        public void When_MerchantNameIsMapped_AndDetailsStringDoesNotMatchAnyNameVariation__Should_Return_False(
            IDictionary<string, string[]> map,
            string merchantName,
            string detailsString)
        {
            var target = new MerchanNameMapComparer(map);

            var result = target.IsMatch(merchantName, detailsString);

            Assert.IsFalse(result);
        }

        private static IEnumerable<TestCaseData> GetSuccessulCaseInsensitiveVariationMatchCases()
        {
            yield return new TestCaseData(
                new Dictionary<string, string[]>
                {
                    { "Certain Name", new [] { "CaseInsensitivity" } }
                },
                "Certain Name",
                "caseinsensitivity");

            yield return new TestCaseData(
                new Dictionary<string, string[]>
                {
                    { "Certain Name", new [] { "Case Insensitivity" } }
                },
                "Certain Name",
                "case insensitivity");

            yield return new TestCaseData(
                new Dictionary<string, string[]>
                {
                    { "Certain Name", new [] { "Case" } }
                },
                "Certain Name",
                "case insensitivity");
        }

        private static IEnumerable<TestCaseData> GetSuccessfulVariationMatchCases()
        {
            yield return new TestCaseData(
                new Dictionary<string, string[]>
                {
                    { "Certain Name", new [] { "CertainName" } }
                },
                "Certain Name",
                "CertainName");

            yield return new TestCaseData(
                new Dictionary<string, string[]>
                {
                    { "Certain Name", new [] { "Name 6" } }
                },
                "Certain Name",
                "Name 6");

            yield return new TestCaseData(
                new Dictionary<string, string[]>
                {
                    { "Certain Name", new [] { "NoMatch", "CertainName" } }
                },
                "Certain Name",
                "CertainName");

            yield return new TestCaseData(
                new Dictionary<string, string[]>
                {
                    { "Certain Name", new [] { "NoMatch", "CertainName" } },
                    { "Another Name", new [] { "No Match", "Match", "No" } }
                },
                "Another Name",
                "Match");
        }

        private static IEnumerable<TestCaseData> GetCasesForWhenNameVariationFormsFirstPartOfDetailsString()
        {
            yield return new TestCaseData(
                new Dictionary<string, string[]>
                {
                    { "Certain Name", new [] { "MustStartWith" } }
                },
                "Certain Name",
                "MustStartWithTheRestDoesntMatter");

            yield return new TestCaseData(
                new Dictionary<string, string[]>
                {
                    { "Certain Name", new [] { "Starts With" } }
                },
                "Certain Name",
                "Starts With nuthin'");
        }

        private static IEnumerable<TestCaseData> GetNoNameVariationMatches()
        {
            yield return new TestCaseData(
                new Dictionary<string, string[]>
                {
                    { "Certain Name", new [] { "CertainName" } }
                },
                "Certain Name",
                "Same Name");

            yield return new TestCaseData(
                new Dictionary<string, string[]>
                {
                    { "Another Name", new [] { "Match" } },
                    { "Certain Name", new [] { "CertainName" } }
                },
                "Certain Name",
                "Match");

            yield return new TestCaseData(
                new Dictionary<string, string[]>
                {
                    { "Certain Name", new [] { "MustStartWith" } }
                },
                "Certain Name",
                "aMustStartWith");
        }

        private static IEnumerable<TestCaseData> GetNameMatchCases()
        {
            yield return new TestCaseData(
                new Dictionary<string, string[]> { },
                "Certain Name",
                "Certain Name");

            yield return new TestCaseData(
                new Dictionary<string, string[]>
                {
                    { "Name Two", new string[] { "Match" } }
                },
                "Another Name",
                "Match");
        }
    }
}