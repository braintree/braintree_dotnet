#pragma warning disable 1591

using Braintree.Exceptions;
using System.Threading.Tasks;
using System;
using System.Xml;
using System.Collections.Generic;

namespace Braintree
{

    class DisputeAddEvidenceRequest : Request
    {
        public string Comments { get; set; }
        public string DocumentId { get; set; }

        public override string ToXml()
        {
            if (Comments != null) {
                return RequestBuilder.BuildXMLElement("comments", Comments);
            }
            if (DocumentId != null) {
                return RequestBuilder.BuildXMLElement("document-upload-id", DocumentId);
            }
            return "";
        }
    }

    /// <summary>
    /// Provides operations for creating, finding, updating, and deleting disputes
    /// </summary>
    public class DisputeGateway : IDisputeGateway
    {
        private readonly BraintreeService Service;
        private readonly IBraintreeGateway Gateway;
        private DisputeSearchRequest DisputeSearch { get; set; }

        protected internal DisputeGateway(IBraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            Gateway = gateway;
            Service = gateway.Service;
        }

        public virtual Result<Dispute> Accept(string disputeId)
        {
            NotFoundException notFoundException = new NotFoundException($"dispute with id '{disputeId}' not found");

            if (disputeId == null || disputeId.Trim().Equals(""))
            {
                throw notFoundException;
            }

            try {
                XmlNode disputeXML = Service.Put(Service.MerchantPath() + "/disputes/" + disputeId + "/accept");

                return new ResultImpl<Dispute>(new NodeWrapper(disputeXML), Gateway);
            } catch (NotFoundException) {
                throw notFoundException;
            }
        }

        public virtual async Task<Result<Dispute>> AcceptAsync(string disputeId)
        {
            NotFoundException notFoundException = new NotFoundException($"dispute with id '{disputeId}' not found");

            if (disputeId == null || disputeId.Trim().Equals(""))
            {
                throw notFoundException;
            }

            try {
                XmlNode disputeXML = await Service.PutAsync(Service.MerchantPath() + "/disputes/" + disputeId + "/accept").ConfigureAwait(false);

                return new ResultImpl<Dispute>(new NodeWrapper(disputeXML), Gateway);
            } catch (NotFoundException) {
                throw notFoundException;
            }
        }

        public virtual Result<DisputeEvidence> AddFileEvidence(string disputeId, FileEvidenceRequest request)
        {
            NotFoundException notFoundException = new NotFoundException($"dispute with id '{disputeId}' not found");

            if (disputeId == null || disputeId.Trim().Equals(""))
            {
                throw notFoundException;
            }

            if (request.DocumentId == null || request.DocumentId.Trim().Equals(""))
            {
                throw new NotFoundException($"document with id '{request.DocumentId}' not found");
            }

            try {
                XmlNode disputeEvidenceXML = Service.Post(Service.MerchantPath() + "/disputes/" + disputeId + "/evidence", request);

                return new ResultImpl<DisputeEvidence>(new NodeWrapper(disputeEvidenceXML), Gateway);
            } catch (NotFoundException) {
                throw notFoundException;
            }
        }

        public virtual Result<DisputeEvidence> AddFileEvidence(string disputeId, string documentUploadId)
        {
            return AddFileEvidence(disputeId, new FileEvidenceRequest { DocumentId = documentUploadId });
        }

        public virtual async Task<Result<DisputeEvidence>> AddFileEvidenceAsync(string disputeId, FileEvidenceRequest request)
        {
            NotFoundException notFoundException = new NotFoundException($"dispute with id '{disputeId}' not found");

            if (disputeId == null || disputeId.Trim().Equals(""))
            {
                throw notFoundException;
            }

            if (request.DocumentId == null || request.DocumentId.Trim().Equals(""))
            {
                throw new NotFoundException($"document with id '{request.DocumentId}' not found");
            }

            try {
                XmlNode disputeEvidenceXML = await Service.PostAsync(Service.MerchantPath() + "/disputes/" + disputeId + "/evidence", request).ConfigureAwait(false);

                return new ResultImpl<DisputeEvidence>(new NodeWrapper(disputeEvidenceXML), Gateway);
            } catch (NotFoundException) {
                throw notFoundException;
            }
        }

        public virtual async Task<Result<DisputeEvidence>> AddFileEvidenceAsync(string disputeId, string documentUploadId)
        {
            return await AddFileEvidenceAsync(disputeId, new FileEvidenceRequest { DocumentId = documentUploadId }).ConfigureAwait(false);
        }

        public virtual Result<DisputeEvidence> AddTextEvidence(string disputeId, string content)
        {
            TextEvidenceRequest textEvidenceRequest = new TextEvidenceRequest
            {
                Content = content
            };
            return AddTextEvidence(disputeId, textEvidenceRequest);
        }

        public virtual Result<DisputeEvidence> AddTextEvidence(string disputeId, TextEvidenceRequest textEvidenceRequest)
        {
            NotFoundException notFoundException = new NotFoundException($"Dispute with ID '{disputeId}' not found");

            if (disputeId == null || disputeId.Trim().Equals(""))
            {
                throw notFoundException;
            }
            if (textEvidenceRequest.Content == null || textEvidenceRequest.Content.Trim().Equals(""))
            {
                throw new ArgumentException("Content cannot be empty");
            }

            int temp;
            if (textEvidenceRequest.SequenceNumber != null && !int.TryParse(textEvidenceRequest.SequenceNumber, out temp))
            {
                throw new ArgumentException("SequenceNumber must be an integer");
            }

            try {
                XmlNode disputeEvidenceXML = Service.Post(Service.MerchantPath() + "/disputes/" + disputeId + "/evidence", textEvidenceRequest);

                return new ResultImpl<DisputeEvidence>(new NodeWrapper(disputeEvidenceXML), Gateway);
            } catch (NotFoundException) {
                throw notFoundException;
            }
        }

        public virtual async Task<Result<DisputeEvidence>> AddTextEvidenceAsync(string disputeId, string content)
        {
            TextEvidenceRequest textEvidenceRequest = new TextEvidenceRequest
            {
                Content = content
            };
            return await AddTextEvidenceAsync(disputeId, textEvidenceRequest).ConfigureAwait(false);
        }

        public virtual async Task<Result<DisputeEvidence>> AddTextEvidenceAsync(string disputeId, TextEvidenceRequest textEvidenceRequest)
        {
            NotFoundException notFoundException = new NotFoundException($"Dispute with ID '{disputeId}' not found");

            if (disputeId == null || disputeId.Trim().Equals(""))
            {
                throw notFoundException;
            }
            if (textEvidenceRequest.Content == null || textEvidenceRequest.Content.Trim().Equals(""))
            {
                throw new ArgumentException("Content cannot be empty");
            }

            int temp;
            if (textEvidenceRequest.SequenceNumber != null && !int.TryParse(textEvidenceRequest.SequenceNumber, out temp))
            {
                throw new ArgumentException("SequenceNumber must be an integer");
            }

            try {
                XmlNode disputeEvidenceXML = await Service.PostAsync(Service.MerchantPath() + "/disputes/" + disputeId + "/evidence", textEvidenceRequest).ConfigureAwait(false);

                return new ResultImpl<DisputeEvidence>(new NodeWrapper(disputeEvidenceXML), Gateway);
            } catch (NotFoundException) {
                throw notFoundException;
            }
        }

        public virtual Result<Dispute> Finalize(string disputeId)
        {
            NotFoundException notFoundException = new NotFoundException($"dispute with id '{disputeId}' not found");

            if (disputeId == null || disputeId.Trim().Equals(""))
            {
                throw notFoundException;
            }

            try {
                XmlNode disputeXML = Service.Put(Service.MerchantPath() + "/disputes/" + disputeId + "/finalize");

                return new ResultImpl<Dispute>(new NodeWrapper(disputeXML), Gateway);
            } catch (NotFoundException) {
                throw notFoundException;
            }
        }

        public virtual async Task<Result<Dispute>> FinalizeAsync(string disputeId)
        {
            NotFoundException notFoundException = new NotFoundException($"dispute with id '{disputeId}' not found");
            if (disputeId == null || disputeId.Trim().Equals(""))
            {
                throw notFoundException;
            }

            try {
                XmlNode disputeXML = await Service.PutAsync(Service.MerchantPath() + "/disputes/" + disputeId + "/finalize").ConfigureAwait(false);

                return new ResultImpl<Dispute>(new NodeWrapper(disputeXML), Gateway);
            } catch (NotFoundException) {
                throw notFoundException;
            }
        }

        public virtual Result<Dispute> Find(string disputeId)
        {
            NotFoundException notFoundException = new NotFoundException($"dispute with id '{disputeId}' not found");

            if (disputeId == null || disputeId.Trim().Equals(""))
            {
                throw notFoundException;
            }

            try {
                XmlNode disputeXML = Service.Get(Service.MerchantPath() + "/disputes/" + disputeId);

                return new ResultImpl<Dispute>(new NodeWrapper(disputeXML), Gateway);
            } catch (NotFoundException) {
                throw notFoundException;
            }
        }

        public virtual async Task<Result<Dispute>> FindAsync(string disputeId)
        {
            NotFoundException notFoundException = new NotFoundException($"dispute with id '{disputeId}' not found");
            if (disputeId == null || disputeId.Trim().Equals(""))
            {
                throw notFoundException;
            }

            try {
                XmlNode disputeXML = await Service.GetAsync(Service.MerchantPath() + "/disputes/" + disputeId).ConfigureAwait(false);

                return new ResultImpl<Dispute>(new NodeWrapper(disputeXML), Gateway);
            } catch (NotFoundException) {
                throw notFoundException;
            }
        }

        public virtual Result<Dispute> RemoveEvidence(string disputeId, string evidenceId)
        {
            NotFoundException notFoundException = new NotFoundException(
                $"evidence with id '{evidenceId}' for dispute with id '{disputeId}' not found");

            if (disputeId == null || disputeId.Trim().Equals("") || evidenceId == null || evidenceId.Trim().Equals(""))
            {
                throw notFoundException;
            }

            try {
                XmlNode disputeXML = Service.Delete(Service.MerchantPath() + "/disputes/" + disputeId + "/evidence/" + evidenceId);

                return new ResultImpl<Dispute>(new NodeWrapper(disputeXML), Gateway);
            } catch (NotFoundException) {
                throw notFoundException;
            }
        }

        public virtual async Task<Result<Dispute>> RemoveEvidenceAsync(string disputeId, string evidenceId)
        {
            NotFoundException notFoundException = new NotFoundException(
                $"evidence with id '{evidenceId}' for dispute with id '{disputeId}' not found");

            if (disputeId == null || disputeId.Trim().Equals("") || evidenceId == null || evidenceId.Trim().Equals(""))
            {
                throw notFoundException; 
            }

            try {
                XmlNode disputeXML = await Service.DeleteAsync(Service.MerchantPath() + "/disputes/" + disputeId + "/evidence/" + evidenceId).ConfigureAwait(false);

                return new ResultImpl<Dispute>(new NodeWrapper(disputeXML), Gateway);
            } catch (NotFoundException) {
                throw notFoundException;
            }
        }

        public virtual PaginatedCollection<Dispute> Search(DisputeSearchRequest request)
        {
            DisputeSearch = request;
            return new PaginatedCollection<Dispute>(FetchDisputes);
        }

        private PaginatedResult<Dispute> FetchDisputes(int page)
        {
            DisputeSearchRequest request = DisputeSearch;
            XmlNode disputeXML = Service.Post(Service.MerchantPath() + "/disputes/advanced_search?page=" + page, request);
            var nodeWrapper = new NodeWrapper(disputeXML);

            var totalItems = nodeWrapper.GetInteger("total-items").Value;
            var pageSize = nodeWrapper.GetInteger("page-size").Value;
            var disputes = new List<Dispute>();
            foreach (var node in nodeWrapper.GetList("dispute"))
            {
                disputes.Add(new Dispute(node));
            }

            return new PaginatedResult<Dispute>(totalItems, pageSize, disputes);
        }
    }
}
