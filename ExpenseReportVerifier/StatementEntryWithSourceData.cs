﻿using Andy.ExpenseReport.Comparison;
using System;
using System.Collections.Generic;

namespace Andy.ExpenseReport.Verifier
{
    public class StatementEntryWithSourceData : StatementEntry
    {
        public string[] SourceData { get; set; }
    }
}