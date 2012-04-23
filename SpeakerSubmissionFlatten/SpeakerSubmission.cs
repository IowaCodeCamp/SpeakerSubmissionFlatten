#region Namespaces

using System.Collections.Generic;

using Kent.Boogaart.KBCsv;

#endregion Namespaces

namespace IowaCodeCamp.Utility.SpeakerSubmissionFlatten
{
	public class SpeakerSubmission
	{
		#region Enumerations

		enum FieldIndex
		{
			Timestamp = 0,
			SpeakerName,
			CityState,
			EmailAddress,
			WebsiteBlogUrl,
			HeadshotUrl,
			SpeakerBio,
			OtherNotes,
			Session1Level,
			Session1Title,
			Session1Description,
			Session2Level,
			Session2Title,
			Session2Description,
			Session3Level,
			Session3Title,
			Session3Description,
		}

		#endregion Enumerations

		#region Public Methods

		public static SpeakerSubmission CreateFromCsvReader(DataRecord dataRecord)
		{
			SpeakerSubmission speakerSubmission = new SpeakerSubmission();

			int fieldCount = dataRecord.Values.Count;

			speakerSubmission.Timestamp = GetField(dataRecord, FieldIndex.Timestamp);
			speakerSubmission.SpeakerName = GetField(dataRecord, FieldIndex.SpeakerName);
			speakerSubmission.CityState = GetField(dataRecord, FieldIndex.CityState);
			speakerSubmission.EmailAddress = GetField(dataRecord, FieldIndex.EmailAddress);
			speakerSubmission.WebsiteBlogUrl = GetField(dataRecord, FieldIndex.WebsiteBlogUrl);
			speakerSubmission.HeadshotUrl = GetField(dataRecord, FieldIndex.HeadshotUrl);
			speakerSubmission.SpeakerBio = GetField(dataRecord, FieldIndex.SpeakerBio);
			speakerSubmission.OtherNotes = GetField(dataRecord, FieldIndex.OtherNotes);
			speakerSubmission.Session1Level = GetField(dataRecord, FieldIndex.Session1Level);
			speakerSubmission.Session1Title = GetField(dataRecord, FieldIndex.Session1Title);
			speakerSubmission.Session1Description = GetField(dataRecord, FieldIndex.Session1Description);
			speakerSubmission.Session2Level = GetField(dataRecord, FieldIndex.Session2Level);
			speakerSubmission.Session2Title = GetField(dataRecord, FieldIndex.Session2Title);
			speakerSubmission.Session2Description = GetField(dataRecord, FieldIndex.Session2Description);
			speakerSubmission.Session3Level = GetField(dataRecord, FieldIndex.Session3Level);
			speakerSubmission.Session3Title = GetField(dataRecord, FieldIndex.Session3Title);
			speakerSubmission.Session3Description = GetField(dataRecord, FieldIndex.Session3Description);

			return speakerSubmission;
		}

		public static void WriteSpeakerCsvHeader(CsvWriter speakerCsvWriter)
		{
			List<string> fields = new List<string>();
			fields.Add("SpeakerKey");
			for(FieldIndex fieldIndex = FieldIndex.Timestamp; fieldIndex <= FieldIndex.OtherNotes; fieldIndex++)
			{
				fields.Add(fieldIndex.ToString());
			}

			speakerCsvWriter.WriteHeaderRecord(fields.ToArray());
		}

		public static void WriteSessionCsvHeader(CsvWriter sessionCsvWriter)
		{
			List<string> fields = new List<string>();
			fields.Add("SpeakerKey");
			fields.Add("SessionKey");
			fields.Add("Selected");
			fields.Add("Room");
			fields.Add("Time");
			fields.Add("SessionLevel");
			fields.Add("SessionTitle");
			fields.Add("SessionDescription");

			sessionCsvWriter.WriteHeaderRecord(fields.ToArray());
		}

		public int WriteToCsv(CsvWriter speakerCsvWriter,
		                      CsvWriter sessionCsvWriter,
		                      int speakerKey,
		                      int sessionKey)
		{
			int recordsWritten = 0;

			speakerCsvWriter.WriteDataRecord(
				speakerKey,
				Timestamp,
				SpeakerName,
				CityState,
				EmailAddress,
				WebsiteBlogUrl,
				HeadshotUrl,
				SpeakerBio,
				OtherNotes
				);

			if ((!string.IsNullOrEmpty(Session1Level)) && 
			    (!string.IsNullOrEmpty(Session1Title)) && 
			    (!string.IsNullOrEmpty(Session1Description)))
			{
				recordsWritten++;
				sessionCsvWriter.WriteDataRecord(
					speakerKey,
					sessionKey + recordsWritten,
					0,
					string.Empty,
					string.Empty,
					Session1Level,
					Session1Title,
					Session1Description
					);
			}

			if ((!string.IsNullOrEmpty(Session2Level)) &&
			    (!string.IsNullOrEmpty(Session2Title)) &&
			    (!string.IsNullOrEmpty(Session2Description)))
			{
				recordsWritten++;
				sessionCsvWriter.WriteDataRecord(
					speakerKey,
					sessionKey + recordsWritten,
					0,
					string.Empty,
					string.Empty,
					Session2Level,
					Session2Title,
					Session2Description
					);
			}

			if ((!string.IsNullOrEmpty(Session3Level)) &&
			    (!string.IsNullOrEmpty(Session3Title)) &&
			    (!string.IsNullOrEmpty(Session3Description)))
			{
				recordsWritten++;
				sessionCsvWriter.WriteDataRecord(
					speakerKey,
					sessionKey + recordsWritten,
					0,
					string.Empty,
					string.Empty,
					Session3Level,
					Session3Title,
					Session3Description
					);
			}
			return recordsWritten;
		}

		#endregion Public Methods

		#region Private Properties

		private string Timestamp { get; set; }
		private string SpeakerName { get; set; }
		private string CityState { get; set; }
		private string EmailAddress { get; set; }
		private string WebsiteBlogUrl { get; set; }
		private string HeadshotUrl { get; set; }
		private string SpeakerBio { get; set; }
		private string OtherNotes { get; set; }
		private string Session1Level { get; set; }
		private string Session1Title { get; set; }
		private string Session1Description { get; set; }
		private string Session2Level { get; set; }
		private string Session2Title { get; set; }
		private string Session2Description { get; set; }
		private string Session3Level { get; set; }
		private string Session3Title { get; set; }
		private string Session3Description { get; set; }

		#endregion Private Properties

		#region Private Methods

		private static string GetField(RecordBase dataRecord, FieldIndex field)
		{
			// Retrieve the field from the record if it exists
			if (dataRecord.Values.Count > (int)field)
			{
				return dataRecord[(int)field];
			}

			return null;
		}

		#endregion Private Methods
	}
}