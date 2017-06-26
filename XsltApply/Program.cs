using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;
using CommandLine;

namespace XsltApply
{
	class Options
	{
		[Option('i', "input", Required = true,
		  HelpText = "Input files to be processed.")]
		public string InputFile { get; set; }

		[Option('o', "output", Required = true,
		  HelpText = "output file.")]
		public string OutputFile { get; set; }

		[Option('x', "xsl", Required = true,
		  HelpText = "xsl file.")]
		public string XslFile { get; set; }

		// Omitting long name, default --verbose
		[Option(HelpText = "Prints all messages to standard output.")]
		public bool Verbose { get; set; }
	}

	class Program
	{
		static void Main(string[] args)
		{
			var options = new Options();
			if (Parser.Default.ParseArgumentsStrict(args, options, () =>ProcessError(options)))
			{
				if (!System.IO.File.Exists(options.InputFile))
				{
					throw new ArgumentException($"File {options.InputFile} does not exist");
				}

				if (!System.IO.File.Exists(options.XslFile))
				{
					throw new ArgumentException($"File {options.XslFile} does not exist");
				}

				var xslCompiledTransform = new XslCompiledTransform();
				xslCompiledTransform.Load(options.XslFile);
				using (XmlWriter xmlWriter = new XmlTextWriter(options.OutputFile, Encoding.Unicode))
				{
					xslCompiledTransform.Transform(options.InputFile, xmlWriter);
				}

			}
		}

		private static void ProcessError(Options options)
		{
			Console.WriteLine(options.ToString());
		}
	}
}
