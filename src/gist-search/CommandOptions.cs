using System;
using Mono.Options;
using System.Collections.Generic;
using System.IO;

namespace gist_search
{
    class CommandOptions
    {
        OptionSet m_Options;
        bool m_IsHelp;
        string m_UserName;
        string m_DescriptionRegex;
        string m_OutputFilePath;
        int m_OutputFileCodePage = -1;
        string m_FileNameRegex;
        public string UserName
        {
            get => m_UserName;
        }
        public bool IsHelp
        {
            get => m_IsHelp;
        }

        public string DescriptionRegex
        {
            get => m_DescriptionRegex;
        }

        public string OutputFilePath
        {
            get => m_OutputFilePath;
        }

        public int OutputFileCodePage
        {
            get => m_OutputFileCodePage;
        }

        public string FileNameRegex
        {
            get => m_FileNameRegex;
        }

        CommandOptions()
        {
            m_Options = CreateOptions();
        }
        public static readonly CommandOptions Instance = new CommandOptions();
        static OptionSet CreateOptions()
        {
            return new OptionSet()
                .Add("h|help", "print this message", x => Instance.m_IsHelp = true)
                .Add("u|user=", "target user name", x => Instance.m_UserName = x)
                .Add("d|descrption=", "regex of description", x => Instance.m_DescriptionRegex = x)
                .Add("o|output=", "output file path(default: console)", x => Instance.m_OutputFilePath = x)
                .Add("e|encoding=", "output file encoding(default: UTF8)", x => Instance.m_OutputFileCodePage = int.Parse(x))
                .Add("hasfile=", "regex for filtering by filename", x => Instance.m_FileNameRegex = x)
                ;
        }
        public IList<string> Parse(string[] args)
        {
            return m_Options.Parse(args);
        }
        public void OutputHelp(TextWriter output)
        {
            m_Options.WriteOptionDescriptions(output);
        }
    }
}