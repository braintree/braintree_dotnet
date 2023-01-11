using Braintree.Exceptions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class DisputeIntegrationTest
    {
#if netcore
        private static string BT_LOGO_PATH = "../../../../../test/fixtures/bt_logo.png";
#else
        private static string BT_LOGO_PATH = "test/fixtures/bt_logo.png";
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

        [Test]
        public void Accept_changesDisputeStatusToAccepted()
        {
            Dispute dispute = createSampleDispute();
            var result = gateway.Dispute.Accept(dispute.Id);

            Assert.IsTrue(result.IsSuccess());

            Dispute finalizedDispute = gateway.Dispute.Find(dispute.Id).Target;
            Assert.AreEqual(DisputeStatus.ACCEPTED, finalizedDispute.Status);

            Dispute disputeFromTransaction = gateway.Transaction.Find(dispute.Transaction.Id).Disputes[0];
            Assert.AreEqual(DisputeStatus.ACCEPTED, disputeFromTransaction.Status);
        }

        [Test]
#if netcore
        public async Task AcceptAsync_changesDisputeStatusToAccepted()
#else
        public void AcceptAsync_changesDisputeStatusToAccepted()
        {
            Task.Run(async () =>
#endif
        {
            Dispute dispute = await createSampleDisputeAsync();
            var result = await gateway.Dispute.AcceptAsync(dispute.Id);

            Assert.IsTrue(result.IsSuccess());

            Result<Dispute> finalizedDisputeResult = await gateway.Dispute.FindAsync(dispute.Id);
            Dispute finalizedDispute = finalizedDisputeResult.Target;
            Assert.AreEqual(DisputeStatus.ACCEPTED, finalizedDispute.Status);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Accept_whenDisputeNotOpenErrors()
        {
            var result = gateway.Dispute.Accept("wells_dispute");

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DISPUTE_CAN_ONLY_ACCEPT_OPEN_DISPUTE, result.Errors.ForObject("Dispute").OnField("Status")[0].Code);
            Assert.AreEqual("Disputes can only be accepted when they are in an Open state", result.Errors.ForObject("Dispute").OnField("Status")[0].Message);
        }

        [Test]
#if netcore
        public async Task AcceptAsync_whenDisputeNotOpenErrors()
#else
        public void AcceptAsync_whenDisputeNotOpenErrors()
        {
            Task.Run(async () =>
#endif
        {
            var result = await gateway.Dispute.AcceptAsync("wells_dispute");

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DISPUTE_CAN_ONLY_ACCEPT_OPEN_DISPUTE, result.Errors.ForObject("Dispute").OnField("Status")[0].Code);
            Assert.AreEqual("Disputes can only be accepted when they are in an Open state", result.Errors.ForObject("Dispute").OnField("Status")[0].Message);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Accept_throwsNotFoundExceptionWhenDisputeIsNotFound()
        {
            NotFoundException exception = Assert.Throws<NotFoundException>(() => gateway.Dispute.Accept("invalid-id"));
            Assert.AreEqual(exception.Message, "dispute with id 'invalid-id' not found");
        }

        [Test]
#if netcore
        public async Task AcceptAsync_throwsNotFoundExceptionWhenDisputeIsNotFound()
#else
        public void AcceptAsync_throwsNotFoundExceptionWhenDisputeIsNotFound()
        {
            Task.Run(async () =>
#endif
        {
            try
            {
                await gateway.Dispute.AcceptAsync("invalid-id");
                Assert.Fail("Expected Exception.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "dispute with id 'invalid-id' not found");
            }
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void AddFileEvidence_addsEvidence()
        {
            DocumentUpload document = createSampleDocumentUpload();
            Dispute dispute = createSampleDispute();

            DisputeEvidence evidence = gateway.Dispute.AddFileEvidence(dispute.Id, document.Id).Target;

            Assert.NotNull(evidence);

            DisputeEvidence foundEvidence = gateway.Dispute.Find(dispute.Id).Target.Evidence[0];

            Assert.Null(evidence.Category);

            Assert.NotNull(foundEvidence);
        }

        [Test]
        public void AddFileEvidence_addsEvidenceWithCategory()
        {
            DocumentUpload document = createSampleDocumentUpload();
            Dispute dispute = createSampleDispute();

            var fileEvidenceRequest = new FileEvidenceRequest
            {
                DocumentId = document.Id,
                Category = "GENERAL",
            };

            DisputeEvidence evidence = gateway.Dispute.AddFileEvidence(dispute.Id, fileEvidenceRequest).Target;

            Assert.NotNull(evidence);

            DisputeEvidence foundEvidence = gateway.Dispute.Find(dispute.Id).Target.Evidence[0];

            Assert.NotNull(evidence.Category);
            Assert.AreEqual(evidence.Category, "GENERAL");

            Assert.NotNull(foundEvidence);
        }

        [Test]
#if netcore
        public async Task AddFileEvidenceAsync_addsEvidence()
#else
        public void AddFileEvidenceAsync_addsEvidence()
        {
            Task.Run(async () =>
#endif
        {
            Dispute dispute = await createSampleDisputeAsync();
            DocumentUpload document = await createSampleDocumentUploadAsync();

            Result<DisputeEvidence> evidenceResult = await gateway.Dispute.AddFileEvidenceAsync(dispute.Id, document.Id);
            Assert.NotNull(evidenceResult.Target);

            Result<Dispute> foundResult = await gateway.Dispute.FindAsync(dispute.Id);
            DisputeEvidence foundEvidence = foundResult.Target.Evidence[0];

            Assert.NotNull(foundEvidence);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void AddFileEvidence_throwsNotFoundExceptionWhenDisputeOrDocumentIdIsNotFound()
        {
            NotFoundException exception = Assert.Throws<NotFoundException>(() => gateway.Dispute.AddFileEvidence("invalid-dispute-id", "invalid-document-id"));
            Assert.AreEqual(exception.Message, "dispute with id 'invalid-dispute-id' not found");
        }

        [Test]
#if netcore
        public async Task AddFileEvidenceAsync_throwsNotFoundExceptionWhenDisputeOrDocumentIdIsNotFound()
#else
        public void AddFileEvidenceAsync_throwsNotFoundExceptionWhenDisputeOrDocumentIdIsNotFound()
        {
            Task.Run(async () =>
#endif
        {
            try
            {
                await gateway.Dispute.AddFileEvidenceAsync("invalid-dispute-id", "invalid-document-id");
                Assert.Fail("Expected Exception.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "dispute with id 'invalid-dispute-id' not found");
            }
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void AddFileEvidence_whenDisputeNotOpenErrors()
        {
            DocumentUpload document = createSampleDocumentUpload();
            Dispute dispute = createSampleDispute();

            gateway.Dispute.Accept(dispute.Id);

            var result = gateway.Dispute.AddFileEvidence(dispute.Id, document.Id);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DISPUTE_CAN_ONLY_ADD_EVIDENCE_TO_OPEN_DISPUTE, result.Errors.ForObject("Dispute").OnField("Status")[0].Code);
            Assert.AreEqual("Evidence can only be attached to disputes that are in an Open state", result.Errors.ForObject("Dispute").OnField("Status")[0].Message);
        }

        [Test]
#if netcore
        public async Task AddFileEvidenceAsync_whenDisputeNotOpenErrors()
#else
        public void AddFileEvidenceAsync_whenDisputeNotOpenErrors()
        {
            Task.Run(async () =>
#endif
        {
            DocumentUpload document = await createSampleDocumentUploadAsync();
            Dispute dispute = await createSampleDisputeAsync();

            await gateway.Dispute.AcceptAsync(dispute.Id);

            var result = await gateway.Dispute.AddFileEvidenceAsync(dispute.Id, document.Id);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DISPUTE_CAN_ONLY_ADD_EVIDENCE_TO_OPEN_DISPUTE, result.Errors.ForObject("Dispute").OnField("Status")[0].Code);
            Assert.AreEqual("Evidence can only be attached to disputes that are in an Open state", result.Errors.ForObject("Dispute").OnField("Status")[0].Message);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void AddFileEvidence_failsToAddEvidenceWithUnSupportedCategory()
        {
            DocumentUpload document = createSampleDocumentUpload();
            Dispute dispute = createSampleDispute();
            FileEvidenceRequest request = new FileEvidenceRequest
            {
                Category = "NOTAREALCATEGORY",
                DocumentId = document.Id,
            };
            var result = gateway.Dispute.AddFileEvidence(dispute.Id, request);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DISPUTE_CAN_ONLY_CREATE_EVIDENCE_WITH_VALID_CATEGORY, result.Errors.ForObject("Dispute").OnField("Evidence")[0].Code);
        }

        [Test]
#if netcore
        public async Task AddFileEvidenceAsync_failsToAddEvidenceWithUnsupportedCategory()
#else
        public void AddFileEvidenceAsync_failsToAddEvidenceWithUnsupportedCategory()
        {
            Task.Run(async () =>
#endif
        {
            DocumentUpload document = await createSampleDocumentUploadAsync();
            Dispute dispute = await createSampleDisputeAsync();

            FileEvidenceRequest request = new FileEvidenceRequest
            {
                Category = "NOTAREALCATEGORY",
                DocumentId = document.Id,
            };
            var result = await gateway.Dispute.AddFileEvidenceAsync(dispute.Id, request);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DISPUTE_CAN_ONLY_CREATE_EVIDENCE_WITH_VALID_CATEGORY, result.Errors.ForObject("Dispute").OnField("Evidence")[0].Code);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
#if netcore
        public async Task AddTextEvidenceAsync_addsTextEvidence()
#else
        public void AddTextEvidenceAsync_addsTextEvidence()
        {
            Task.Run(async () =>
#endif
        {
            Dispute dispute = await createSampleDisputeAsync();

            Result<DisputeEvidence> evidenceResult = await gateway.Dispute.AddTextEvidenceAsync(dispute.Id, "my text evidence");
            DisputeEvidence evidence = evidenceResult.Target;

            Assert.NotNull(evidence.Id);
            Assert.NotNull(evidence.CreatedAt);
            Assert.AreEqual("my text evidence", evidence.Comment);
            Assert.Null(evidence.SentToProcessorAt);
            Assert.Null(evidence.Url);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void AddTextEvidence_addsEvidenceWithCateogry()
        {
            Dispute dispute = createSampleDispute();
            var textEvidenceRequest = new TextEvidenceRequest
            {
                Content = "my content",
                Category = "DEVICE_ID",
            };

            DisputeEvidence evidence = gateway.Dispute.AddTextEvidence(dispute.Id, textEvidenceRequest).Target;

            Assert.NotNull(evidence);

            DisputeEvidence foundEvidence = gateway.Dispute.Find(dispute.Id).Target.Evidence[0];

            Assert.NotNull(evidence.Category);
            Assert.AreEqual(evidence.Category, "DEVICE_ID");

            Assert.NotNull(foundEvidence);
        }

        [Test]
        public void AddTextEvidence_failsToAddEvidenceWithUnSupportedCategory()
        {
            Dispute dispute = createSampleDispute();
            TextEvidenceRequest request = new TextEvidenceRequest
            {
                Category = "NOTAREALCATEGORY",
                Content = "evidence",
            };
            var result = gateway.Dispute.AddTextEvidence(dispute.Id, request);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DISPUTE_CAN_ONLY_CREATE_EVIDENCE_WITH_VALID_CATEGORY, result.Errors.ForObject("Dispute").OnField("Evidence")[0].Code);
        }

        [Test]
#if netcore
        public async Task AddTextEvidenceAsync_failsToAddEvidenceWithUnSupportedCategory()
#else
        public void AddTextEvidenceAsync_failsToAddEvidenceWithUnSupportedCategory()
        {
            Task.Run(async () =>
#endif
        {
            Dispute dispute = await createSampleDisputeAsync();

            TextEvidenceRequest request = new TextEvidenceRequest
            {
                Category = "NOTAREALCATEGORY",
                Content = "evidence",
            };
            var result = await gateway.Dispute.AddTextEvidenceAsync(dispute.Id, request);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DISPUTE_CAN_ONLY_CREATE_EVIDENCE_WITH_VALID_CATEGORY, result.Errors.ForObject("Dispute").OnField("Evidence")[0].Code);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void AddTextEvidence_throwsNotFoundExceptionWhenDisputeNotFound()
        {
            NotFoundException exception = Assert.Throws<NotFoundException>(() => gateway.Dispute.AddTextEvidence("invalid-dispute-id", "some comment"));
            Assert.AreEqual(exception.Message, "Dispute with ID 'invalid-dispute-id' not found");
        }

        [Test]
#if netcore
        public async Task AddTextEvidenceAsync_throwsNotFoundExceptionWhenDisputeNotFound()
#else
        public void AddTextEvidenceAsync_throwsNotFoundExceptionWhenDisputeNotFound()
        {
            Task.Run(async () =>
#endif
        {
            try
            {
                await gateway.Dispute.AddTextEvidenceAsync("invalid-dispute-id", "some comment");
                Assert.Fail("Expected Exception.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "Dispute with ID 'invalid-dispute-id' not found");
            }
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void AddTextEvidence_whenDisputeNotOpenErrors()
        {
            Dispute dispute = createSampleDispute();

            gateway.Dispute.Accept(dispute.Id);

            var result = gateway.Dispute.AddTextEvidence(dispute.Id, "my text evidence");

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DISPUTE_CAN_ONLY_ADD_EVIDENCE_TO_OPEN_DISPUTE, result.Errors.ForObject("Dispute").OnField("Status")[0].Code);
            Assert.AreEqual("Evidence can only be attached to disputes that are in an Open state", result.Errors.ForObject("Dispute").OnField("Status")[0].Message);
        }

        [Test]
#if netcore
        public async Task AddTextEvidenceAsync_whenDisputeNotOpenErrors()
#else
        public void AddTextEvidenceAsync_whenDisputeNotOpenErrors()
        {
            Task.Run(async () =>
#endif
        {
            Dispute dispute = await createSampleDisputeAsync();

            await gateway.Dispute.AcceptAsync(dispute.Id);

            var result = await gateway.Dispute.AddTextEvidenceAsync(dispute.Id, "my text evidence");

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DISPUTE_CAN_ONLY_ADD_EVIDENCE_TO_OPEN_DISPUTE, result.Errors.ForObject("Dispute").OnField("Status")[0].Code);
            Assert.AreEqual("Evidence can only be attached to disputes that are in an Open state", result.Errors.ForObject("Dispute").OnField("Status")[0].Message);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void AddTextEvidence_showsNewRecordInFind()
        {
            Dispute dispute = createSampleDispute();

            DisputeEvidence evidence = gateway.Dispute.AddTextEvidence(dispute.Id, "my text evidence").Target;

            Assert.NotNull(evidence);

            DisputeEvidence foundEvidence = gateway.Dispute.Find(dispute.Id).Target.Evidence[0];

            Assert.AreEqual(evidence.Id, foundEvidence.Id);
        }

        [Test]
#if netcore
        public async Task AddTextEvidenceAsync_showsNewRecordInFind()
#else
        public void AddTextEvidenceAsync_showsNewRecordInFind()
        {
            Task.Run(async () =>
#endif
        {
            Dispute dispute = await createSampleDisputeAsync();

            Result<DisputeEvidence> evidenceResult = await gateway.Dispute.AddTextEvidenceAsync(dispute.Id, "my text evidence");
            DisputeEvidence evidence = evidenceResult.Target;

            Assert.NotNull(evidence);

            Result<Dispute> foundEvidenceResult = await gateway.Dispute.FindAsync(dispute.Id);
            DisputeEvidence foundEvidence = foundEvidenceResult.Target.Evidence[0];

            Assert.AreEqual(evidence.Id, foundEvidence.Id);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Finalize_changesDisputeStatusToDisputed()
        {
            Dispute dispute = createSampleDispute();
            var result = gateway.Dispute.Finalize(dispute.Id);

            Assert.IsTrue(result.IsSuccess());

            Dispute finalizedDispute = gateway.Dispute.Find(dispute.Id).Target;
            Assert.AreEqual(DisputeStatus.DISPUTED, finalizedDispute.Status);
        }

        [Test]
        public void Finalize_whenThereAreValidationErrorsDoesNotSucceed()
        {
            Dispute dispute = createSampleDispute();
            var textEvidenceRequest = new TextEvidenceRequest
            {
                Content = "my content",
                Category = "DEVICE_ID",
            };

            DisputeEvidence evidence = gateway.Dispute.AddTextEvidence(dispute.Id, textEvidenceRequest).Target;

            var result = gateway.Dispute.Finalize(dispute.Id);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual
                (
                    new HashSet<ValidationErrorCode>
                    {
                        ValidationErrorCode.DISPUTE_DIGITAL_GOODS_MISSING_EVIDENCE,
                        ValidationErrorCode.DISPUTE_DIGITAL_GOODS_MISSING_DOWNLOAD_DATE
                    },
                    new HashSet<ValidationErrorCode>(result.Errors.ForObject("Dispute").OnField("Dispute").Select(error => error.Code))
                );
            Dispute finalizedDispute = gateway.Dispute.Find(dispute.Id).Target;
            Assert.AreNotEqual(DisputeStatus.DISPUTED, finalizedDispute.Status);
            Assert.AreEqual(DisputeStatus.OPEN, finalizedDispute.Status);
        }

        [Test]
#if netcore
        public async Task FinalizeAsync_changesDisputeStatusToDisputed()
#else
        public void FinalizeAsync_changesDisputeStatusToDisputed()
        {
            Task.Run(async () =>
#endif
        {
            Dispute dispute = await createSampleDisputeAsync();
            var result = gateway.Dispute.Finalize(dispute.Id);

            Assert.IsTrue(result.IsSuccess());

            Result<Dispute> finalizedDisputeResult = await gateway.Dispute.FindAsync(dispute.Id);
            Dispute finalizedDispute = finalizedDisputeResult.Target;
            Assert.AreEqual(DisputeStatus.DISPUTED, finalizedDispute.Status);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Finalize_whenDisputeNotOpenErrors()
        {
            var result = gateway.Dispute.Finalize("wells_dispute");

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DISPUTE_CAN_ONLY_FINALIZE_OPEN_DISPUTE, result.Errors.ForObject("Dispute").OnField("Status")[0].Code);
            Assert.AreEqual("Disputes can only be finalized when they are in an Open state", result.Errors.ForObject("Dispute").OnField("Status")[0].Message);
        }

        [Test]
#if netcore
        public async Task FinalizeAsync_whenDisputeNotOpenErrors()
#else
        public void FinalizeAsync_whenDisputeNotOpenErrors()
        {
            Task.Run(async () =>
#endif
        {
            var result = await gateway.Dispute.FinalizeAsync("wells_dispute");

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DISPUTE_CAN_ONLY_FINALIZE_OPEN_DISPUTE, result.Errors.ForObject("Dispute").OnField("Status")[0].Code);
            Assert.AreEqual("Disputes can only be finalized when they are in an Open state", result.Errors.ForObject("Dispute").OnField("Status")[0].Message);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Finalize_whenDisputeNotFoundErrors()
        {
            NotFoundException exception = Assert.Throws<NotFoundException>(() => gateway.Dispute.Finalize("invalid-id"));
            Assert.AreEqual(exception.Message, "dispute with id 'invalid-id' not found");
        }

        [Test]
#if netcore
        public async Task FinalizeAsync_whenDisputeNotFoundErrors()
#else
        public void FinalizeAsync_whenDisputeNotFoundErrors()
        {
            Task.Run(async () =>
#endif
        {
            try
            {
                await gateway.Dispute.FinalizeAsync("invalid-id");
                Assert.Fail("Expected Exception.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "dispute with id 'invalid-id' not found");
            }
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Find_returnsDisputeWithGivenId()
        {
            Dispute dispute = gateway.Dispute.Find("open_dispute").Target;

            Assert.AreEqual(31m, dispute.AmountDisputed);
            Assert.AreEqual(0m, dispute.AmountWon);
            Assert.AreEqual("open_dispute", dispute.Id);
            Assert.AreEqual(DisputeStatus.OPEN, dispute.Status);
            Assert.AreEqual("open_disputed_transaction", dispute.Transaction.Id);
        }

        [Test]
#if netcore
        public async Task FindAsync_returnsDisputeWithGivenId()
#else
        public void FindAsync_returnsDisputeWithGivenId()
        {
            Task.Run(async () =>
#endif
        {
            Result<Dispute> disputeResult = await gateway.Dispute.FindAsync("open_dispute");
            Dispute dispute = disputeResult.Target;

            Assert.AreEqual(31m, dispute.AmountDisputed);
            Assert.AreEqual(0m, dispute.AmountWon);
            Assert.AreEqual("open_dispute", dispute.Id);
            Assert.AreEqual(DisputeStatus.OPEN, dispute.Status);
            Assert.AreEqual("open_disputed_transaction", dispute.Transaction.Id);
            Assert.IsNotNull(dispute.GraphQLId);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Find_throwsNotFoundExceptionWhenDisputeNotFound()
        {
            NotFoundException exception = Assert.Throws<NotFoundException>(() => gateway.Dispute.Find("invalid-id"));
            Assert.AreEqual(exception.Message, "dispute with id 'invalid-id' not found");
        }

        [Test]
#if netcore
        public async Task FindAsync_throwsNotFoundExceptionWhenDisputeNotFound()
#else
        public void FindAsync_throwsNotFoundExceptionWhenDisputeNotFound()
        {
            Task.Run(async () =>
#endif
        {
            try
            {
                await gateway.Dispute.FindAsync("invalid-id");
                Assert.Fail("Expected Exception.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "dispute with id 'invalid-id' not found");
            }
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void RemoveEvidence_removesEvidenceFromTheDispute()
        {
            Dispute dispute = createSampleDispute();

            DisputeEvidence evidence = gateway.Dispute.AddTextEvidence(dispute.Id, "my text evidence").Target;

            Assert.NotNull(evidence);

            var result = gateway.Dispute.RemoveEvidence(dispute.Id, evidence.Id);

            Assert.IsTrue(result.IsSuccess());

            Dispute editedDispute = gateway.Dispute.Find(dispute.Id).Target;

            Assert.AreEqual(0, editedDispute.Evidence.Count);
        }

        [Test]
#if netcore
        public async Task RemoveEvidenceAsync_removesEvidenceFromTheDispute()
#else
        public void RemoveEvidenceAsync_removesEvidenceFromTheDispute()
        {
            Task.Run(async () =>
#endif
        {
            Dispute dispute = await createSampleDisputeAsync();

            Result<DisputeEvidence> evidenceResult = await gateway.Dispute.AddTextEvidenceAsync(dispute.Id, "my text evidence");
            DisputeEvidence evidence = evidenceResult.Target;
            Assert.NotNull(evidence);

            var result = await gateway.Dispute.RemoveEvidenceAsync(dispute.Id, evidence.Id);

            Assert.IsTrue(result.IsSuccess());

            Dispute editedDispute = gateway.Dispute.Find(dispute.Id).Target;

            Assert.AreEqual(0, editedDispute.Evidence.Count);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void RemoveEvidence_whenDisputeOrEvidenceNotFoundThrowsNotFoundException()
        {
            NotFoundException exception = Assert.Throws<NotFoundException>(() => gateway.Dispute.RemoveEvidence("invalid-dispute-id", "invalid-evidence-id"));
            Assert.AreEqual(exception.Message, "evidence with id 'invalid-evidence-id' for dispute with id 'invalid-dispute-id' not found");
        }

        [Test]
#if netcore
        public async Task RemoveEvidenceAsync_whenDisputeOrEvidenceNotFoundThrowsNotFoundException()
#else
        public void RemoveEvidenceAsync_whenDisputeOrEvidenceNotFoundThrowsNotFoundException()
        {
            Task.Run(async () =>
#endif
        {
            try
            {
                await gateway.Dispute.RemoveEvidenceAsync("invalid-dispute-id", "invalid-evidence-id");
                Assert.Fail("Expected Exception.");
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual(exception.Message, "evidence with id 'invalid-evidence-id' for dispute with id 'invalid-dispute-id' not found");
            }
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void RemoveEvidence_errorsWhenDisputeNotOpen()
        {
            Dispute dispute = createSampleDispute();

            DisputeEvidence evidence = gateway.Dispute.AddTextEvidence(dispute.Id, "my text evidence").Target;

            Assert.NotNull(evidence);

            gateway.Dispute.Accept(dispute.Id);

            var result = gateway.Dispute.RemoveEvidence(dispute.Id, evidence.Id);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DISPUTE_CAN_ONLY_REMOVE_EVIDENCE_FROM_OPEN_DISPUTE, result.Errors.ForObject("Dispute").OnField("Status")[0].Code);
            Assert.AreEqual("Evidence can only be removed from disputes that are in an Open state", result.Errors.ForObject("Dispute").OnField("Status")[0].Message);
        }

        [Test]
#if netcore
        public async Task RemoveEvidenceAsync_errorsWhenDisputeNotOpen()
#else
        public void RemoveEvidenceAsync_errorsWhenDisputeNotOpen()
        {
            Task.Run(async () =>
#endif
        {
            Dispute dispute = await createSampleDisputeAsync();

            Result<DisputeEvidence> evidenceResult = await gateway.Dispute.AddTextEvidenceAsync(dispute.Id, "my text evidence");
            DisputeEvidence evidence = evidenceResult.Target;
            Assert.NotNull(evidence);
            await gateway.Dispute.AcceptAsync(dispute.Id);

            var result = await gateway.Dispute.RemoveEvidenceAsync(dispute.Id, evidence.Id);

            Assert.IsFalse(result.IsSuccess());

            Assert.AreEqual(ValidationErrorCode.DISPUTE_CAN_ONLY_REMOVE_EVIDENCE_FROM_OPEN_DISPUTE, result.Errors.ForObject("Dispute").OnField("Status")[0].Code);
            Assert.AreEqual("Evidence can only be removed from disputes that are in an Open state", result.Errors.ForObject("Dispute").OnField("Status")[0].Message);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void Search_withEmptyResult()
        {
            DisputeSearchRequest request = new DisputeSearchRequest().
                Id.Is("non_existent_dispute");
            PaginatedCollection<Dispute> disputeCollection = gateway.Dispute.Search(request);

            var disputes = new List<Dispute>();
            foreach (var d in disputeCollection)
            {
                disputes.Add(d);
            }
            Assert.AreEqual(0, disputes.Count);
        }

        [Test]
        public void Search_byIdReturnsSingleDispute()
        {
            DisputeSearchRequest request = new DisputeSearchRequest().
                Id.Is("open_dispute");
            PaginatedCollection<Dispute> disputeCollection = gateway.Dispute.Search(request);

            var disputes = new List<Dispute>();
            foreach (var d in disputeCollection)
            {
                disputes.Add(d);
            }
            Assert.AreEqual(1, disputes.Count);
        }

        [Test]
        public void Search_withMultipleReasonsReturnsMultipleDisputes()
        {
            DisputeSearchRequest request = new DisputeSearchRequest().
                DisputeReason.IncludedIn(DisputeReason.PRODUCT_UNSATISFACTORY, DisputeReason.RETRIEVAL);
            PaginatedCollection<Dispute> disputeCollection = gateway.Dispute.Search(request);

            var disputes = new List<Dispute>();
            foreach (var d in disputeCollection)
            {
                disputes.Add(d);
            }
            Assert.IsTrue(disputes.Count >= 2);
        }

        [Test]
        public void Search_withChargebackProtectionLevelReturnsDispute()
        {
            DisputeSearchRequest request = new DisputeSearchRequest().
                DisputeChargebackProtectionLevel.Is(DisputeChargebackProtectionLevel.EFFORTLESS);
            PaginatedCollection<Dispute> disputeCollection = gateway.Dispute.Search(request);

            var disputes = new List<Dispute>();
            foreach (var d in disputeCollection)
            {
                disputes.Add(d);
            }
            Assert.IsTrue(disputes.Count > 0);

            foreach (var dispute in disputes)
            {
                Assert.AreEqual(DisputeReason.FRAUD, dispute.Reason);
                // NEXT_MAJOR_VERSION Remove this assertion when ChargebackProtectionLevel is removed from the SDK
                Assert.AreEqual(DisputeChargebackProtectionLevel.EFFORTLESS, dispute.ChargebackProtectionLevel);
                Assert.AreEqual(DisputeProtectionLevel.EFFORTLESS_CBP, dispute.ProtectionLevel);
            }
        }

        [Test]
        public void Search_withPreDisputeProgramReturnsDispute()
        {
            DisputeSearchRequest request = new DisputeSearchRequest().
                DisputePreDisputeProgram.Is(DisputePreDisputeProgram.VISA_RDR);
            PaginatedCollection<Dispute> disputeCollection = gateway.Dispute.Search(request);

            var disputes = new List<Dispute>();
            foreach (var d in disputeCollection)
            {
                disputes.Add(d);
            }
            Assert.AreEqual(1, disputes.Count);
            Assert.AreEqual(DisputePreDisputeProgram.VISA_RDR, disputes[0].PreDisputeProgram);
        }

        [Test]
        public void Search_forNonPreDisputesReturnsDisputes()
        {
            DisputeSearchRequest request = new DisputeSearchRequest().
                DisputePreDisputeProgram.IncludedIn(DisputePreDisputeProgram.NONE);
            PaginatedCollection<Dispute> disputeCollection = gateway.Dispute.Search(request);

            var disputes = new List<Dispute>();
            foreach (var d in disputeCollection)
            {
                disputes.Add(d);
            }
            Assert.IsTrue(disputes.Count > 1);

            var preDisputePrograms = disputes.Select(d => d.PreDisputeProgram).Distinct().ToArray();
            Assert.AreEqual(new[] {DisputePreDisputeProgram.NONE}, preDisputePrograms);
        }

        [Test]
        public void Search_receivedDateRangeReturnsDispute()
        {
            DateTime startDate = DateTime.Parse("2014-03-03");
            DateTime endDate = DateTime.Parse("2014-03-05");
            DisputeSearchRequest request = new DisputeSearchRequest().
                ReceivedDate.Between(startDate, endDate);
            PaginatedCollection<Dispute> disputeCollection = gateway.Dispute.Search(request);

            var disputes = new List<Dispute>();
            foreach (var d in disputeCollection)
            {
                disputes.Add(d);
            }
            Assert.IsTrue(disputes.Count >= 1);
            Assert.AreEqual("2014", disputes[0].ReceivedDate.Value.Year.ToString());
            Assert.AreEqual("3", disputes[0].ReceivedDate.Value.Month.ToString());
            Assert.AreEqual("4", disputes[0].ReceivedDate.Value.Day.ToString());
        }

        [Test]
        public void Search_disbursementDateRangeReturnsDispute()
        {
            DateTime startDate = DateTime.Parse("2014-03-03");
            DateTime endDate = DateTime.Parse("2014-03-05");
            DisputeSearchRequest request = new DisputeSearchRequest().
                DisbursementDate.Between(startDate, endDate);
            PaginatedCollection<Dispute> disputeCollection = gateway.Dispute.Search(request);

            var disputes = new List<Dispute>();
            foreach (var d in disputeCollection)
            {
                disputes.Add(d);
            }
            Assert.IsTrue(disputes.Count >= 1);
            Assert.AreEqual("2014", disputes[0].StatusHistory[0].DisbursementDate.Value.Year.ToString());
            Assert.AreEqual("3", disputes[0].StatusHistory[0].DisbursementDate.Value.Month.ToString());
            Assert.AreEqual("5", disputes[0].StatusHistory[0].DisbursementDate.Value.Day.ToString());
        }

        [Test]
        public void Search_effectiveDateRangeReturnsDispute()
        {
            DateTime startDate = DateTime.Parse("2014-03-03");
            DateTime endDate = DateTime.Parse("2014-03-05");
            DisputeSearchRequest request = new DisputeSearchRequest().
                EffectiveDate.Between(startDate, endDate);
            PaginatedCollection<Dispute> disputeCollection = gateway.Dispute.Search(request);

            var disputes = new List<Dispute>();
            foreach (var d in disputeCollection)
            {
                disputes.Add(d);
            }
            Assert.IsTrue(disputes.Count >= 1);
            Assert.AreEqual("2014", disputes[0].StatusHistory[0].EffectiveDate.Value.Year.ToString());
            Assert.AreEqual("3", disputes[0].StatusHistory[0].EffectiveDate.Value.Month.ToString());
            Assert.AreEqual("4", disputes[0].StatusHistory[0].EffectiveDate.Value.Day.ToString());
        }

        [Test]
        public void Search_byCustomerIdReturnsDispute()
        {
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest {});

            string customerId = result.Target.Id;

            TransactionRequest transactionRequest = new TransactionRequest
            {
                Amount = 100M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.Dispute.CHARGEBACK,
                    ExpirationDate = "05/2012",
                },
                CustomerId = customerId
            };

            Transaction transaction = gateway.Transaction.Sale(transactionRequest).Target;

            DisputeSearchRequest request = new DisputeSearchRequest().
                CustomerId.Is(customerId);
            PaginatedCollection<Dispute> disputeCollection = gateway.Dispute.Search(request);

            var disputes = new List<Dispute>();
            foreach (var d in disputeCollection)
            {
                disputes.Add(d);
            }
            Assert.AreEqual(1, disputes.Count);
        }

        public Dispute createSampleDispute(){
            string creditCardToken = $"cc{new Random().Next(1000000).ToString()}";

            TransactionRequest request = new TransactionRequest
            {
                Amount = 100M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.Dispute.CHARGEBACK,
                    ExpirationDate = "05/2012",
                    Token = creditCardToken
                },
            };

            Transaction transaction = gateway.Transaction.Sale(request).Target;
            return transaction.Disputes[0];
        }

        public async Task<Dispute> createSampleDisputeAsync(){
            string creditCardToken = $"cc{new Random().Next(1000000).ToString()}";

            TransactionRequest request = new TransactionRequest
            {
                Amount = 100M,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.Dispute.CHARGEBACK,
                    ExpirationDate = "05/2012",
                    Token = creditCardToken
                },
            };

            Result<Transaction> transactionResult = await gateway.Transaction.SaleAsync(request);
            return transactionResult.Target.Disputes[0];
        }

        public DocumentUpload createSampleDocumentUpload() {
            FileStream file = new FileStream(BT_LOGO_PATH, FileMode.Open, FileAccess.Read);
            DocumentUploadRequest request = new DocumentUploadRequest();
            request.File = file;
            request.DocumentKind = DocumentUploadKind.EVIDENCE_DOCUMENT;
            DocumentUpload documentUpload = gateway.DocumentUpload.Create(request).Target;
            return documentUpload;
        }

        public async Task<DocumentUpload> createSampleDocumentUploadAsync() {
            FileStream file = new FileStream(BT_LOGO_PATH, FileMode.Open, FileAccess.Read);
            DocumentUploadRequest request = new DocumentUploadRequest();
            request.File = file;
            request.DocumentKind = DocumentUploadKind.EVIDENCE_DOCUMENT;
            Result<DocumentUpload> documentUploadResult = await gateway.DocumentUpload.CreateAsync(request);
            return documentUploadResult.Target;
        }
    }
}
