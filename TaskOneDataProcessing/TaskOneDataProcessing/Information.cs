using System;
using System.Linq;

namespace DataProcessing
{
    internal class Information
    {

        public int parsed_files { get; set; }
        public int parsed_lines { get; set; }
        public int found_errors { get; set; }
        public string[] invalid_files { get; set; }

        public Information()
        {

        }
        public override string ToString()
        {
            return $"parsed_files: {parsed_files}\r\nparsed_lines: {parsed_lines}\r\nfound_errors: {found_errors}\r\ninvalid_files: {String.Join(" ", invalid_files)}";
        }
    }
}
