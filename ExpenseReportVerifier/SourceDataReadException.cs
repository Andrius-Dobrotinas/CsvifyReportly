﻿using System;

namespace Andy.ExpenseReport.Cmd
{
    public class SourceDataReadException : MyApplicationException
    {
        public SourceDataReadException(Exception e)
            : base("Failed to read the source data", e)
        {
        }
    }
}