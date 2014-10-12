#region Namespaces

using System;
using System.IO;
using System.Text;

using Kent.Boogaart.KBCsv;

#endregion Namespaces

namespace IowaCodeCamp.Utility.SpeakerSubmissionFlatten
{
	class Program
	{
		static int Main(string[] args)
		{
			if (args.Length < 2)
			{
				Usage(string.Empty);
				return 1;
			}

			if (!File.Exists(args[0]))
			{
				Usage("Input file does not exist.");
				return 2;
			}

			if (!Directory.Exists(args[1]))
			{
				Usage("Output folder does not exist.");
				return 3;
			}

			string speakerFileName = Path.Combine(args[1], "Speakers.csv");
			string sessionFileName = Path.Combine(args[1], "Sessions.csv");

			FlattenFile(args[0],
			            speakerFileName,
						sessionFileName);

			Console.WriteLine("Conversion completed.");
			Console.ReadLine();
			return 0;
		}

		private static void FlattenFile(string inputFileName,
		                                string speakerFileName,
		                                string sessionFileName)
		{
			int recordsRead = 1;
			int sessionsWritten = 0;

			using (CsvReader csvReader = new CsvReader(inputFileName, Encoding.UTF8))
			using (CsvWriter speakerWriter = new CsvWriter(speakerFileName, Encoding.UTF8))
			using (CsvWriter sessionWriter = new CsvWriter(sessionFileName, Encoding.UTF8))
			{
				// Wite the header records out to the new CSV files
				SpeakerSubmission.WriteSpeakerCsvHeader(speakerWriter);
				SpeakerSubmission.WriteSessionCsvHeader(sessionWriter);

				HeaderRecord headerRecord = csvReader.ReadHeaderRecord();

				DataRecord record;
				while ((record = csvReader.ReadDataRecord()) != null)
				{
					// Read the source record
					SpeakerSubmission speakerSubmission = SpeakerSubmission.CreateFromCsvReader(record);

					// Write the output records
					sessionsWritten += speakerSubmission.WriteToCsv(speakerWriter, sessionWriter, recordsRead, sessionsWritten);

					recordsRead++;
				}
			}

			Console.WriteLine("Read {0} records. Wrote {1} records.", recordsRead, sessionsWritten);
		}

		private static void Usage(string message)
		{
			if (!string.IsNullOrEmpty(message))
				Console.WriteLine("ERROR: {0}", message);
			Console.WriteLine("Usage: SpeakerSubmissionFlattin <input file> <output folder>");
			Console.ReadLine();
		}
	}
}
