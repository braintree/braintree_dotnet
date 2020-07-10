using Braintree.Exceptions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class DocumentUploadIntegrationTest
    {
#if netcore
        private static string BT_LOGO_PATH = "../../../../../test/fixtures/bt_logo.png";
#else
        private static string BT_LOGO_PATH = "test/fixtures/bt_logo.png";
#endif

#if netcore
        private static string LARGE_FILE_PATH = "../../../../../test/fixtures/large_file.png";
#else
        private static string LARGE_FILE_PATH = "test/fixtures/large_file.png";
#endif

#if netcore
        private static string EMPTY_FILE_PATH = "../../../../../test/fixtures/empty_file.png";
#else
        private static string EMPTY_FILE_PATH = "test/fixtures/empty_file.png";
#endif

#if netcore
        private static string TOO_LONG_FILE_PATH = "../../../../../test/fixtures/too_long.pdf";
#else
        private static string TOO_LONG_FILE_PATH = "test/fixtures/too_long.pdf";
#endif

#if netcore
        private static string MALFORMED_FILE_PATH = "../../../../../test/fixtures/malformed_pdf.pdf";
#else
        private static string MALFORMED_FILE_PATH = "test/fixtures/malformed_pdf.pdf";
#endif

#if netcore
        private static string GIF_EXTENSION_FILE_PATH = "../../../../../test/fixtures/gif_extension_bt_logo.gif";
#else
        private static string GIF_EXTENSION_FILE_PATH = "test/fixtures/gif_extension_bt_logo.gif";
#endif

        private BraintreeGateway gateway;

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
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(@LARGE_FILE_PATH))
            {
                File.Delete(@LARGE_FILE_PATH);
            }
        }

        [Test]
        public void Create_returnsSuccessfulWithValidRequest()
        {
			FileStream fs = new FileStream(BT_LOGO_PATH, FileMode.Open, FileAccess.Read);
			DocumentUploadRequest request = new DocumentUploadRequest();
			request.File = fs;
			request.DocumentKind = DocumentUploadKind.EVIDENCE_DOCUMENT; 
			DocumentUpload documentUpload = gateway.DocumentUpload.Create(request).Target;
			Assert.NotNull(documentUpload);
			Assert.AreEqual(DocumentUploadKind.EVIDENCE_DOCUMENT, documentUpload.Kind);
			Assert.AreEqual("bt_logo.png", documentUpload.Name);
			Assert.AreEqual("image/png", documentUpload.ContentType);
			Assert.AreEqual(2443m, documentUpload.Size);
        }

        [Test]
#if netcore
        public async Task CreateAsync_returnsSuccessfulWithValidRequest()
#else
        public void CreateAsync_returnsSuccessfulWithValidRequest()
        {
            Task.Run(async () =>
#endif
        {
            FileStream fs = new FileStream(BT_LOGO_PATH, FileMode.Open, FileAccess.Read);
            DocumentUploadRequest request = new DocumentUploadRequest();
            request.File = fs;
            request.DocumentKind = DocumentUploadKind.EVIDENCE_DOCUMENT; 
            Result<DocumentUpload> documentUploadResult = await gateway.DocumentUpload.CreateAsync(request);
            DocumentUpload documentUpload = documentUploadResult.Target; 
            Assert.NotNull(documentUpload);
            Assert.AreEqual(DocumentUploadKind.EVIDENCE_DOCUMENT, documentUpload.Kind);
            Assert.AreEqual("bt_logo.png", documentUpload.Name);
            Assert.AreEqual("image/png", documentUpload.ContentType);
            Assert.AreEqual(2443m, documentUpload.Size);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Create_throwsWithEmptyKind()
        {
            FileStream fs = new FileStream(BT_LOGO_PATH, FileMode.Open, FileAccess.Read);
            DocumentUploadRequest request = new DocumentUploadRequest();
            request.File = fs;
            ArgumentException nullException = Assert.Throws<ArgumentException>(() => gateway.DocumentUpload.Create(request));
            Assert.AreEqual(nullException.Message, "DocumentKind must not be null");
        }

        [Test]
#if netcore
        public async Task CreateAsync_throwsWithEmptyKind()
#else
        public void CreateAsync_throwsWithEmptyKind()
        {
            Task.Run(async () =>
#endif
        {
            FileStream fs = new FileStream(BT_LOGO_PATH, FileMode.Open, FileAccess.Read);
            DocumentUploadRequest request = new DocumentUploadRequest();
            request.File = fs;
            try
            {
                await gateway.DocumentUpload.CreateAsync(request);
                Assert.Fail("Expected Exception.");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual(exception.Message, "DocumentKind must not be null");
            }
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Create_throwsWithEmptyFile()
        {
            DocumentUploadRequest request = new DocumentUploadRequest();
            request.DocumentKind = DocumentUploadKind.EVIDENCE_DOCUMENT; 
            ArgumentException nullException = Assert.Throws<ArgumentException>(() => gateway.DocumentUpload.Create(request));
            Assert.AreEqual(nullException.Message, "File must not be null");
        }

        [Test]
#if netcore
        public async Task CreateAsync_throwsWithEmptyFile()
#else
        public void CreateAsync_throwsWithEmptyFile()
        {
            Task.Run(async () =>
#endif
        {
            DocumentUploadRequest request = new DocumentUploadRequest();
            request.DocumentKind = DocumentUploadKind.EVIDENCE_DOCUMENT; 
            try
            {
                await gateway.DocumentUpload.CreateAsync(request);
                Assert.Fail("Expected Exception.");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual(exception.Message, "File must not be null");
            }
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Create_returnsErrorWithUnsupportedFileType()
        {
            FileStream fs = new FileStream(GIF_EXTENSION_FILE_PATH, FileMode.Open, FileAccess.Read);
            DocumentUploadRequest request = new DocumentUploadRequest();
            request.File = fs;
            request.DocumentKind = DocumentUploadKind.EVIDENCE_DOCUMENT; 
            var result = gateway.DocumentUpload.Create(request);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DOCUMENT_UPLOAD_FILE_TYPE_IS_INVALID, result.Errors.ForObject("DocumentUpload").OnField("File")[0].Code);
        }

        [Test]
#if netcore
        public async Task CreateAsync_returnsErrorWithUnsupportedFileType()
#else
        public void CreateAsync_returnsErrorWithUnsupportedFileType()
        {
            Task.Run(async () =>
#endif
        {
            FileStream fs = new FileStream(GIF_EXTENSION_FILE_PATH, FileMode.Open, FileAccess.Read);
            DocumentUploadRequest request = new DocumentUploadRequest();
            request.File = fs;
            request.DocumentKind = DocumentUploadKind.EVIDENCE_DOCUMENT; 
            var result = await gateway.DocumentUpload.CreateAsync(request);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DOCUMENT_UPLOAD_FILE_TYPE_IS_INVALID, result.Errors.ForObject("DocumentUpload").OnField("File")[0].Code);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Create_returnsErrorWithMalformedFile()
        {
            FileStream fs = new FileStream(MALFORMED_FILE_PATH, FileMode.Open, FileAccess.Read);
            DocumentUploadRequest request = new DocumentUploadRequest();
            request.File = fs;
            request.DocumentKind = DocumentUploadKind.EVIDENCE_DOCUMENT; 
            var result = gateway.DocumentUpload.Create(request);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DOCUMENT_UPLOAD_FILE_IS_MALFORMED_OR_ENCRYPTED, result.Errors.ForObject("DocumentUpload").OnField("File")[0].Code);
        }

        [Test]
#if netcore
        public async Task CreateAsync_returnsErrorWithMalformedFile()
#else
        public void CreateAsync_returnsErrorWithMalformedFile()
        {
            Task.Run(async () =>
#endif
        {
            FileStream fs = new FileStream(MALFORMED_FILE_PATH, FileMode.Open, FileAccess.Read);
            DocumentUploadRequest request = new DocumentUploadRequest();
            request.File = fs;
            request.DocumentKind = DocumentUploadKind.EVIDENCE_DOCUMENT; 
            var result = await gateway.DocumentUpload.CreateAsync(request);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DOCUMENT_UPLOAD_FILE_IS_MALFORMED_OR_ENCRYPTED, result.Errors.ForObject("DocumentUpload").OnField("File")[0].Code);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Create_returnsErrorWhenFileIsOver4Mb()
        {
            using(StreamWriter writetext = new StreamWriter(new FileStream(LARGE_FILE_PATH, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
            {
                for (int i = 0; i <= 1048577; i++) {
                    writetext.WriteLine("aaaa");
                }
            }
            FileStream fs = new FileStream(LARGE_FILE_PATH, FileMode.Open, FileAccess.Read);
            DocumentUploadRequest request = new DocumentUploadRequest();
            request.File = fs;
            request.DocumentKind = DocumentUploadKind.EVIDENCE_DOCUMENT; 
            var result = gateway.DocumentUpload.Create(request);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DOCUMENT_UPLOAD_FILE_IS_TOO_LARGE, result.Errors.ForObject("DocumentUpload").OnField("File")[0].Code);
        }

        [Test]
#if netcore
        public async Task CreateAsync_returnsErrorWhenFileIsOver4Mb()
#else
        public void CreateAsync_returnsErrorWhenFileIsOver4Mb()
        {
            Task.Run(async () =>
#endif
        {
            using(StreamWriter writetext = new StreamWriter(new FileStream(LARGE_FILE_PATH, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
            {
                for (int i = 0; i <= 1048577; i++) {
                    writetext.WriteLine("aaaa");
                }
            }
            FileStream fs = new FileStream(LARGE_FILE_PATH, FileMode.Open, FileAccess.Read);
            DocumentUploadRequest request = new DocumentUploadRequest();
            request.File = fs;
            request.DocumentKind = DocumentUploadKind.EVIDENCE_DOCUMENT; 
            var result = await gateway.DocumentUpload.CreateAsync(request);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DOCUMENT_UPLOAD_FILE_IS_TOO_LARGE, result.Errors.ForObject("DocumentUpload").OnField("File")[0].Code);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Create_returnsErrorWhenFileIsEmpty()
        {
            using(StreamWriter writetext = new StreamWriter(new FileStream(EMPTY_FILE_PATH, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
            {
            }
            FileStream fs = new FileStream(EMPTY_FILE_PATH, FileMode.Open, FileAccess.Read);
            DocumentUploadRequest request = new DocumentUploadRequest();
            request.File = fs;
            request.DocumentKind = DocumentUploadKind.EVIDENCE_DOCUMENT; 
            var result = gateway.DocumentUpload.Create(request);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DOCUMENT_UPLOAD_FILE_IS_EMPTY, result.Errors.ForObject("DocumentUpload").OnField("File")[0].Code);
        }

        [Test]
#if netcore
        public async Task CreateAsync_returnsErrorWhenFileIsEmpty()
#else
        public void CreateAsync_returnsErrorWhenFileIsEmpty()
        {
            Task.Run(async () =>
#endif
        {
            using(StreamWriter writetext = new StreamWriter(new FileStream(EMPTY_FILE_PATH, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
            {
            }
            FileStream fs = new FileStream(EMPTY_FILE_PATH, FileMode.Open, FileAccess.Read);
            DocumentUploadRequest request = new DocumentUploadRequest();
            request.File = fs;
            request.DocumentKind = DocumentUploadKind.EVIDENCE_DOCUMENT; 
            var result = await gateway.DocumentUpload.CreateAsync(request);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DOCUMENT_UPLOAD_FILE_IS_EMPTY, result.Errors.ForObject("DocumentUpload").OnField("File")[0].Code);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Create_returnsErrorWhenFileIsOver50Pages()
        {
            FileStream fs = new FileStream(TOO_LONG_FILE_PATH, FileMode.Open, FileAccess.Read);
            DocumentUploadRequest request = new DocumentUploadRequest();
            request.File = fs;
            request.DocumentKind = DocumentUploadKind.EVIDENCE_DOCUMENT;
            var result = gateway.DocumentUpload.Create(request);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DOCUMENT_UPLOAD_FILE_IS_TOO_LONG, result.Errors.ForObject("DocumentUpload").OnField("File")[0].Code);
        }

        [Test]
#if netcore
        public async Task CreateAsync_returnsErrorWhenFileIsOver50Pages()
#else
        public void CreateAsync_returnsErrorWhenFileIsOver50Pages()
        {
            Task.Run(async () =>
#endif
        {
            FileStream fs = new FileStream(TOO_LONG_FILE_PATH, FileMode.Open, FileAccess.Read);
            DocumentUploadRequest request = new DocumentUploadRequest();
            request.File = fs;
            request.DocumentKind = DocumentUploadKind.EVIDENCE_DOCUMENT;
            var result = await gateway.DocumentUpload.CreateAsync(request);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DOCUMENT_UPLOAD_FILE_IS_TOO_LONG, result.Errors.ForObject("DocumentUpload").OnField("File")[0].Code);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif
    }
}
