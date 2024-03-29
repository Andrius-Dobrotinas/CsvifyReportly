﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Andy.ExpenseReport.Comparison.Statement
{
    public class MerchantNameComparer : IDetailsComparer
    {
        private readonly Bank.IMerchanNameVariationComparer nameMapComparer;
        private readonly IDetailsComparer detailsComparer;

        public MerchantNameComparer(
            Bank.IMerchanNameVariationComparer nameMapComparer,
            IDetailsComparer detailsComparer)
        {
            this.nameMapComparer = nameMapComparer;
            this.detailsComparer = detailsComparer;
        }

        public bool AreEqual(string statementDetailsString, string merchantIdentifier)
        {
            if (nameMapComparer.IsMatch(merchantIdentifier, statementDetailsString))
                return true;
            else
                return detailsComparer.AreEqual(statementDetailsString, merchantIdentifier);
        }
    }
}