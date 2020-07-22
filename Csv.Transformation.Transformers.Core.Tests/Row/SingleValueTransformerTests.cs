using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.Csv.Transformation.Row
{
    public class SingleValueTransformerTests
    {
        SingleValueTransformer target;
        Mock<IValueTransformer> valueTransformer;

        [SetUp]
        public void Setup()
        {
            valueTransformer = new Mock<IValueTransformer>();
        }

        [TestCaseSource(nameof(Get_Cells))]
        public void Must_InvokeTheValueTransformer_WithTheValueOfThatsAtTheTargetPosition(
            IList<string> cells,
            int targetColumnIndex)
        {
            var expectedValue = cells[targetColumnIndex];

            target = new SingleValueTransformer(targetColumnIndex, valueTransformer.Object);

            target.Tramsform(cells.ToArray());

            valueTransformer.Verify(
                    x => x.GetValue(
                        It.Is<string>(
                            arg => arg == expectedValue)));
        }

        [TestCaseSource(nameof(Get_Cells2))]
        public void Must_ReplaceTheTargetArrayItem_WithTheTransformedValue(
            IList<string> cells,
            int targetColumnIndex,
            string newValue,
            IList<string> expectedCells)
        {
            valueTransformer.Setup(
                    x => x.GetValue(It.IsAny<string>()))
                .Returns(newValue);

            target = new SingleValueTransformer(targetColumnIndex, valueTransformer.Object);

            var result = target.Tramsform(cells.ToArray());

            AssertionExtensions.SequencesAreEqual(expectedCells, result);
        }

        private static IEnumerable<TestCaseData> Get_Cells()
        {
            yield return new TestCaseData(
                new List<string> { "one" },
                0);

            yield return new TestCaseData(
                new List<string> { "one", "two" },
                0);

            yield return new TestCaseData(
                new List<string> { "one", "two" },
                1);

            yield return new TestCaseData(
                new List<string> { "se7en", "e11even", "six", "five" },
                2);
        }

        private static IEnumerable<TestCaseData> Get_Cells2()
        {
            yield return new TestCaseData(
                new List<string> { "one" },
                0,
                "modded",
                new List<string> { "modded" });

            yield return new TestCaseData(
                new List<string> { "one", "two" },
                0,
                "",
                new List<string> { "", "two" });

            yield return new TestCaseData(
                new List<string> { "one", "two" },
                1,
                null,
                new List<string> { "one", null });

            yield return new TestCaseData(
                new List<string> { "se7en", "e11even", "six", "five" },
                2,
                "nail'd it",
                new List<string> { "se7en", "e11even", "nail'd it", "five" });
        }
    }
}