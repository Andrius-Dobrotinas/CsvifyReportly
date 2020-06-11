using System;
using System.IO;

namespace Andy.Csv.Transformation.Row.Document.Cmd
{
    public class Parameters
    {
        public FileInfo SourceFile { get; set; }
        public FileInfo ResultFile { get; set; }
        public string RewriterChainName { get; set; }
        public string FilterChainName { get; set; }
    }
}