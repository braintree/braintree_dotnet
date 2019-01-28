using Braintree.Exceptions;
using NUnit.Framework;
using System;
using System.Xml;
using System.Text;

namespace Braintree.Tests
{
    [TestFixture]
    public class DocumentUploadTest
    {
        private BraintreeGateway gateway;
        private BraintreeService service;

        [SetUp]
            public void Setup()
            {
                gateway = new BraintreeGateway
                {
                    Environment = Environment.DEVELOPMENT,
                    MerchantId = "integration_merchant_id",
                    PublicKey = "integration_public_key",
                    PrivateKey = "integration_private_key"
                };

                service = new BraintreeService(gateway.Configuration);
            }

        [Test]
        public void Constructor_documentUploadCanMapFileTypeIsInvalidResponse()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(FILE_TYPE_IS_INVALID_RESPONSE);

            var result = new ResultImpl<DocumentUpload>(new NodeWrapper(doc.DocumentElement), gateway);

            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.DOCUMENT_UPLOAD_FILE_TYPE_IS_INVALID, result.Errors.ForObject("DocumentUpload").OnField("File")[0].Code);
        }

        [Test]
        public void Constructor_documentUploadCanMapFileIsMalformedResponse()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(FILE_IS_MALFORMED_RESPONSE);

            var result = new ResultImpl<DocumentUpload>(new NodeWrapper(doc.DocumentElement), gateway);

            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.DOCUMENT_UPLOAD_FILE_IS_MALFORMED_OR_ENCRYPTED, result.Errors.ForObject("DocumentUpload").OnField("File")[0].Code);
        }

        [Test]
        public void Constructor_documentUploadCanMapFileIsTooLargeResponse()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(FILE_IS_TOO_LARGE_RESPONSE);

            var result = new ResultImpl<DocumentUpload>(new NodeWrapper(doc.DocumentElement), gateway);

            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.DOCUMENT_UPLOAD_FILE_IS_TOO_LARGE, result.Errors.ForObject("DocumentUpload").OnField("File")[0].Code);
        }

        [Test]
        public void Constructor_documentUploadCanMapFileIsTooLongResponse()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(FILE_IS_TOO_LONG_RESPONSE);

            var result = new ResultImpl<DocumentUpload>(new NodeWrapper(doc.DocumentElement), gateway);

            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.DOCUMENT_UPLOAD_FILE_IS_TOO_LONG, result.Errors.ForObject("DocumentUpload").OnField("File")[0].Code);
        }

        private String FILE_TYPE_IS_INVALID_RESPONSE = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n"
            + "<api-error-response>\n"
            + "  <errors>\n"
            + "    <errors type=\"array\"/>\n"
            + "    <document-upload>\n"
            + "      <errors type=\"array\">\n"
            + "        <error>\n"
            + "          <code>84903</code>\n"
            + "          <attribute type=\"symbol\">file</attribute>\n"
            + "          <message>Only PNG, JPG, JPEG, and PDF files are accepted.</message>\n"
            + "        </error>\n"
            + "      </errors>\n"
            + "    </document-upload>\n"
            + "  </errors>\n"
            + "  <params>\n"
            + "    <document-upload>\n"
            + "      <kind>identity_document</kind>\n"
            + "    </document-upload>\n"
            + "    <controller>document_uploads</controller>\n"
            + "    <action>create</action>\n"
            + "    <merchant-id>integration_merchant_id</merchant-id>\n"
            + "  </params>\n"
            + "  <message>Only PNG, JPG, JPEG, and PDF files are accepted.</message>\n"
            + "</api-error-response>";

        private String FILE_IS_MALFORMED_RESPONSE = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n"
            + "<api-error-response>\n"
            + "  <errors>\n"
            + "    <errors type=\"array\"/>\n"
            + "    <document-upload>\n"
            + "      <errors type=\"array\">\n"
            + "        <error>\n"
            + "          <code>84904</code>\n"
            + "          <attribute type=\"symbol\">file</attribute>\n"
            + "          <message>Malformed or encrypted files are not accepted</message>\n"
            + "        </error>\n"
            + "      </errors>\n"
            + "    </document-upload>\n"
            + "  </errors>\n"
            + "  <params>\n"
            + "    <document-upload>\n"
            + "      <kind>identity_document</kind>\n"
            + "    </document-upload>\n"
            + "    <controller>document_uploads</controller>\n"
            + "    <action>create</action>\n"
            + "    <merchant-id>integration_merchant_id</merchant-id>\n"
            + "  </params>\n"
            + "  <message>Malformed or encrypted files are not accepted</message>\n"
            + "</api-error-response>";

        private String FILE_IS_TOO_LARGE_RESPONSE = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n"
            + "<api-error-response>\n"
            + "  <errors>\n"
            + "    <errors type=\"array\"/>\n"
            + "    <document-upload>\n"
            + "      <errors type=\"array\">\n"
            + "        <error>\n"
            + "          <code>84902</code>\n"
            + "          <attribute type=\"symbol\">file</attribute>\n"
            + "          <message>File size is limited to 4 MB.</message>\n"
            + "        </error>\n"
            + "      </errors>\n"
            + "    </document-upload>\n"
            + "  </errors>\n"
            + "  <params>\n"
            + "    <document-upload>\n"
            + "      <kind>identity_document</kind>\n"
            + "    </document-upload>\n"
            + "    <controller>document_uploads</controller>\n"
            + "    <action>create</action>\n"
            + "    <merchant-id>integration_merchant_id</merchant-id>\n"
            + "  </params>\n"
            + "  <message>File size is limited to 4 MB.</message>\n"
            + "</api-error-response>";

        private String FILE_IS_TOO_LONG_RESPONSE = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n"
            + "<api-error-response>\n"
            + "  <errors>\n"
            + "    <errors type=\"array\"/>\n"
            + "    <document-upload>\n"
            + "      <errors type=\"array\">\n"
            + "        <error>\n"
            + "          <code>84905</code>\n"
            + "          <attribute type=\"symbol\">file</attribute>\n"
            + "          <message>PDF page length is limited to 50 pages</message>\n"
            + "        </error>\n"
            + "      </errors>\n"
            + "    </document-upload>\n"
            + "  </errors>\n"
            + "  <params>\n"
            + "    <document-upload>\n"
            + "      <kind>identity_document</kind>\n"
            + "    </document-upload>\n"
            + "    <controller>document_uploads</controller>\n"
            + "    <action>create</action>\n"
            + "    <merchant-id>integration_merchant_id</merchant-id>\n"
            + "  </params>\n"
            + "  <message>PDF page length is limited to 50 pages</message>\n"
            + "</api-error-response>";
    }
}
