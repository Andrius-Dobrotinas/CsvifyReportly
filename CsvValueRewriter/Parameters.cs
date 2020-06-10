using System;
using System.IO;

namespace Andy.Csv.Transformation.Cmd
{
    public class Parameters
    {
        public FileInfo SourceFile { get; set; }
        public FileInfo ResultFile { get; set; }
        public string RewriterChainName { get; set; }
        public string FilterChainName { get; set; }
    }
}