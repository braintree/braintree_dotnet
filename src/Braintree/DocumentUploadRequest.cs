#pragma warning disable 1591
using System.IO;
using System.Collections.Generic;

namespace Braintree
{
	public class DocumentUploadRequest : Request
	{
		public FileStream File { get; set; }
		public DocumentUploadKind? DocumentKind { get; set; }

		public override Dictionary<string, object> ToDictionary()
		{
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                {"document_upload[kind]", DocumentKind.GetDescription()}
            };
            return dictionary;
		}
	}
}
