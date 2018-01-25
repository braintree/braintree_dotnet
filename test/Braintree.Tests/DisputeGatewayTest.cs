using Braintree.Exceptions;
using NUnit.Framework;
using System;
using System.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Braintree.Tests
{
    [TestFixture]
    public class DisputeGatewayTest
    {
        private BraintreeGateway gateway;
        private BraintreeService service;
        private DisputeGateway disputeGateway;

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
            disputeGateway = new DisputeGateway(gateway);
        }

        [Test]
        public void Accept_nullOrEmptyThrowsNotFoundException()
        {
            NotFoundException nullException = Assert.Throws<NotFoundException>(() => disputeGateway.Accept(null));
            Assert.AreEqual(nullException.Message, "dispute with id '' not found");

            NotFoundException emptyException = Assert.Throws<NotFoundException>(() => disputeGateway.Accept(" "));
            Assert.AreEqual(emptyException.Message, "dispute with id ' ' not found");
        }

        [Test]
#if netcore
        public async Task AcceptAsync_nullOrEmptyThrowsNotFoundException()
#else
        public void AcceptAsync_nullOrEmptyThrowsNotFoundException()
        {
            Task.Run(async () =>
#endif
        {
            try
            {
                await disputeGateway.AcceptAsync(null);
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "dispute with id '' not found");
            }

            try
            {
                await disputeGateway.AcceptAsync(" ");
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "dispute with id ' ' not found");
            }
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void AddFileEvidence_nullOrEmptyDisputeIdThrowsNotFoundException()
        {
            NotFoundException nullException = Assert.Throws<NotFoundException>(() => disputeGateway.AddFileEvidence(null, "evidence"));
            Assert.AreEqual(nullException.Message, "dispute with id '' not found");

            NotFoundException emptyException = Assert.Throws<NotFoundException>(() => disputeGateway.AddFileEvidence(" ", "evidence"));
            Assert.AreEqual(emptyException.Message, "dispute with id ' ' not found");
        }

        [Test]
#if netcore
        public async Task AddFileEvidence_nullOrEmptyDisputeIdThrowsNotFoundExceptionAsync()
#else
        public void AddFileEvidence_nullOrEmptyDisputeIdThrowsNotFoundExceptionAsync()
        {
            Task.Run(async () =>
#endif
        {
            try
            {
                await disputeGateway.AddFileEvidenceAsync(null, "evidence");
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "dispute with id '' not found");
            }

            try
            {
                await disputeGateway.AddFileEvidenceAsync(" ", "evidence");
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "dispute with id ' ' not found");
            }
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void AddFileEvidence_nullOrEmptyDocumentUploadIdThrowsNotFoundException()
        {
            NotFoundException nullException = Assert.Throws<NotFoundException>(() => disputeGateway.AddFileEvidence("dispute", null));
            Assert.AreEqual(nullException.Message, "document with id '' not found");

            NotFoundException emptyException = Assert.Throws<NotFoundException>(() => disputeGateway.AddFileEvidence("dispute", " "));
            Assert.AreEqual(emptyException.Message, "document with id ' ' not found");
        }

        [Test]
#if netcore
        public async Task AddFileEvidence_nullOrEmptyDocumentUploadIdThrowsNotFoundExceptionAsync()
#else
        public void AddFileEvidence_nullOrEmptyDocumentUploadIdThrowsNotFoundExceptionAsync()
        {
            Task.Run(async () =>
#endif
        {
            try
            {
                await disputeGateway.AddFileEvidenceAsync("dispute", null);
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "document with id '' not found");
            }

            try
            {
                await disputeGateway.AddFileEvidenceAsync("dispute", " ");
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "document with id ' ' not found");
            }
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void AddTextEvidence_nullOrEmptyDisputeIdThrowsNotFoundException()
        {
            NotFoundException nullException = Assert.Throws<NotFoundException>(() => disputeGateway.AddTextEvidence(null, "evidence"));
            Assert.AreEqual(nullException.Message, "Dispute with ID '' not found");

            NotFoundException emptyException = Assert.Throws<NotFoundException>(() => disputeGateway.AddTextEvidence(" ", "evidence"));
            Assert.AreEqual(emptyException.Message, "Dispute with ID ' ' not found");
        }

        [Test]
#if netcore
        public async Task AddTextEvidence_nullOrEmptyDisputeIdThrowsNotFoundExceptionAsync()
#else
        public void AddTextEvidence_nullOrEmptyDisputeIdThrowsNotFoundExceptionAsync()
        {
            Task.Run(async () =>
#endif
        {
            try
            {
                await disputeGateway.AddTextEvidenceAsync(null, "evidence");
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "Dispute with ID '' not found");
            }

            try
            {
                await disputeGateway.AddTextEvidenceAsync(" ", "evidence");
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "Dispute with ID ' ' not found");
            }
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void AddTextEvidence_nullOrEmptyContentThrowsArgumentException()
        {
            string Content = null;
            ArgumentException nullException = Assert.Throws<ArgumentException>(() => disputeGateway.AddTextEvidence("dispute", Content));
            Assert.AreEqual(nullException.Message, "Content cannot be empty");

            Content = " ";
            nullException = Assert.Throws<ArgumentException>(() => disputeGateway.AddTextEvidence("dispute", Content));
            Assert.AreEqual(nullException.Message, "Content cannot be empty");

            TextEvidenceRequest textEvidenceRequest = new TextEvidenceRequest()
            {
                Content = null
            };
            nullException = Assert.Throws<ArgumentException>(() => disputeGateway.AddTextEvidence("dispute", textEvidenceRequest));
            Assert.AreEqual(nullException.Message, "Content cannot be empty");

            textEvidenceRequest = new TextEvidenceRequest()
            {
                Content = " "
            };
            nullException = Assert.Throws<ArgumentException>(() => disputeGateway.AddTextEvidence("dispute", textEvidenceRequest));
            Assert.AreEqual(nullException.Message, "Content cannot be empty");
        }

        [Test]
        public void AddTextEvidence_nonIntegerSequenceNumberThrowsArgumentException()
        {
            TextEvidenceRequest textEvidenceRequest = new TextEvidenceRequest()
            {
                Content = "content",
                Tag = "Tag",
                SequenceNumber = "four"
            };
            ArgumentException nullException = Assert.Throws<ArgumentException>(() => disputeGateway.AddTextEvidence("dispute", textEvidenceRequest));
            Assert.AreEqual(nullException.Message, "SequenceNumber must be an integer");

            textEvidenceRequest = new TextEvidenceRequest()
            {
                Content = "content",
                Tag = "Tag",
                SequenceNumber = "4.5"
            };
            nullException = Assert.Throws<ArgumentException>(() => disputeGateway.AddTextEvidence("dispute", textEvidenceRequest));
            Assert.AreEqual(nullException.Message, "SequenceNumber must be an integer");
        }

        [Test]
#if netcore
        public async Task AddTextEvidenceAsync_nullOrEmptyContentThrowsArgumentExceptionAsync()
#else
        public void AddTextEvidenceAsync_nullOrEmptyContentThrowsArgumentExceptionAsync()
        {
            Task.Run(async () =>
#endif
        {
            string Content = null;
            try
            {
                await disputeGateway.AddTextEvidenceAsync("dispute", Content);
                Assert.Fail("Expected NotFoundException.");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual(exception.Message, "Content cannot be empty");
            }

            Content = " ";
            try
            {
                await disputeGateway.AddTextEvidenceAsync("dispute", Content);
                Assert.Fail("Expected NotFoundException.");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual(exception.Message, "Content cannot be empty");
            }

            TextEvidenceRequest textEvidenceRequest = new TextEvidenceRequest()
            {
                Content = null
            };
            try
            {
                await disputeGateway.AddTextEvidenceAsync("dispute", textEvidenceRequest);
                Assert.Fail("Expected NotFoundException.");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual(exception.Message, "Content cannot be empty");
            }

            textEvidenceRequest = new TextEvidenceRequest()
            {
                Content = " "
            };
            try
            {
                await disputeGateway.AddTextEvidenceAsync("dispute", textEvidenceRequest);
                Assert.Fail("Expected NotFoundException.");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual(exception.Message, "Content cannot be empty");
            }
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif



        [Test]
#if netcore
        public async Task AddTextEvidenceAsync_nonIntegerSequenceNumberThrowsArgumentExceptionAsync()
#else
        public void AddTextEvidenceAsync_nonIntegerSequenceNumberThrowsArgumentExceptionAsync()
        {
            Task.Run(async () =>
#endif
        {
            TextEvidenceRequest textEvidenceRequest = new TextEvidenceRequest()
            {
                Content = "content",
                Tag = "Tag",
                SequenceNumber = "four"
            };
            try
            {
                await disputeGateway.AddTextEvidenceAsync("dispute", textEvidenceRequest);
                Assert.Fail("Expected ArgumentException.");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual(exception.Message, "SequenceNumber must be an integer");
            }

            textEvidenceRequest = new TextEvidenceRequest()
            {
                Content = "content",
                Tag = "Tag",
                SequenceNumber = "4.5"
            };
            try
            {
                await disputeGateway.AddTextEvidenceAsync("dispute", textEvidenceRequest);
                Assert.Fail("Expected ArgumentException.");
            }
            catch (ArgumentException exception)
            {
                Assert.AreEqual(exception.Message, "SequenceNumber must be an integer");
            }
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Finalize_nullOrEmptyThrowsNotFoundException()
        {
            NotFoundException nullException = Assert.Throws<NotFoundException>(() => disputeGateway.Finalize(null));
            Assert.AreEqual(nullException.Message, "dispute with id '' not found");

            NotFoundException emptyException = Assert.Throws<NotFoundException>(() => disputeGateway.Finalize(" "));
            Assert.AreEqual(emptyException.Message, "dispute with id ' ' not found");
        }

        [Test]
#if netcore
        public async Task FinalizeAsync_nullOrEmptyThrowsNotFoundException()
#else
        public void FinalizeAsync_nullOrEmptyThrowsNotFoundException()
        {
            Task.Run(async () =>
#endif
        {
            try
            {
                await disputeGateway.FinalizeAsync(null);
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "dispute with id '' not found");
            }

            try
            {
                await disputeGateway.FinalizeAsync(" ");
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "dispute with id ' ' not found");
            }
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Find_nullOrEmptyThrowsNotFoundException()
        {
            NotFoundException nullException = Assert.Throws<NotFoundException>(() => disputeGateway.Find(null));
            Assert.AreEqual(nullException.Message, "dispute with id '' not found");

            NotFoundException emptyException = Assert.Throws<NotFoundException>(() => disputeGateway.Find(" "));
            Assert.AreEqual(emptyException.Message, "dispute with id ' ' not found");
        }

        [Test]
#if netcore
        public async Task FindAsync_nullOrEmptyThrowsNotFoundException()
#else
        public void FindAsync_nullOrEmptyThrowsNotFoundException()
        {
            Task.Run(async () =>
#endif
        {
            try
            {
                await disputeGateway.FindAsync(null);
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "dispute with id '' not found");
            }

            try
            {
                await disputeGateway.FindAsync(" ");
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "dispute with id ' ' not found");
            }
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void RemoveEvidence_nullOrEmptyDisputeIdThrowsNotFoundException()
        {
            NotFoundException nullException = Assert.Throws<NotFoundException>(() => disputeGateway.RemoveEvidence(null, "evidence"));
            Assert.AreEqual(nullException.Message, "evidence with id 'evidence' for dispute with id '' not found");

            NotFoundException emptyException = Assert.Throws<NotFoundException>(() => disputeGateway.RemoveEvidence(" ", "evidence"));
            Assert.AreEqual(emptyException.Message, "evidence with id 'evidence' for dispute with id ' ' not found");
        }

        [Test]
#if netcore
        public async Task RemoveEvidence_nullOrEmptyDisputeIdThrowsNotFoundExceptionAsync()
#else
        public void RemoveEvidence_nullOrEmptyDisputeIdThrowsNotFoundExceptionAsync()
        {
            Task.Run(async () =>
#endif
        {
            try
            {
                await disputeGateway.RemoveEvidenceAsync(null, "evidence");
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "evidence with id 'evidence' for dispute with id '' not found");
            }

            try
            {
                await disputeGateway.RemoveEvidenceAsync(" ", "evidence");
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "evidence with id 'evidence' for dispute with id ' ' not found");
            }
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void RemoveEvidence_nullOrEmptyEvidenceIdThrowsNotFoundException()
        {
            NotFoundException nullException = Assert.Throws<NotFoundException>(() => disputeGateway.RemoveEvidence("dispute", null));
            Assert.AreEqual(nullException.Message, "evidence with id '' for dispute with id 'dispute' not found");

            NotFoundException emptyException = Assert.Throws<NotFoundException>(() => disputeGateway.RemoveEvidence("dispute", " "));
            Assert.AreEqual(emptyException.Message, "evidence with id ' ' for dispute with id 'dispute' not found");
        }

        [Test]
#if netcore
        public async Task RemoveEvidence_nullOrEmptyEvidenceIdThrowsNotFoundExceptionAsync()
#else
        public void RemoveEvidence_nullOrEmptyEvidenceIdThrowsNotFoundExceptionAsync()
        {
            Task.Run(async () =>
#endif
        {
            try
            {
                await disputeGateway.RemoveEvidenceAsync("dispute", null);
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "evidence with id '' for dispute with id 'dispute' not found");
            }

            try
            {
                await disputeGateway.RemoveEvidenceAsync("dispute", " ");
                Assert.Fail("Expected NotFoundException.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "evidence with id ' ' for dispute with id 'dispute' not found");
            }
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif
    }
}
