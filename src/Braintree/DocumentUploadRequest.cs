#pragma warning disable 1591
using System.IO;
using System.Collections.Generic;

namespace Braintree
{
	public class DocumentUploadRequest : Request
	{
		public Stream ContentStream { get; set; }
		public DocumentUploadKind? DocumentKind { get; set; }
		public FileStream File { get; set; }
		public string FileName { get; set; }

		public override Dictionary<string, object> ToDictionary()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("document_upload[kind]", DocumentKind.GetDescription());
			return dictionary;
		}
	}
}
