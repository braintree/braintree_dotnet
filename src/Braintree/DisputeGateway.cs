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
        public string DocumentUploadId { get; set; }

        public override string ToXml()
        {
            if (Comments != null) {
                return RequestBuilder.BuildXMLElement("comments", Comments);
            }
            if (DocumentUploadId != null) {
                return RequestBuilder.BuildXMLElement("document-upload-id", DocumentUploadId);
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
            Service = new BraintreeService(gateway.Configuration);
        }

        public virtual Result<Dispute> Accept(string disputeId)
        {
            NotFoundException notFoundException = new NotFoundException(String.Format("dispute with id '{0}' not found", disputeId));

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
            NotFoundException notFoundException = new NotFoundException(String.Format("dispute with id '{0}' not found", disputeId));

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

        public virtual Result<DisputeEvidence> AddFileEvidence(string disputeId, string documentUploadId)
        {
            NotFoundException notFoundException = new NotFoundException(String.Format("dispute with id '{0}' not found", disputeId));

            if (disputeId == null || disputeId.Trim().Equals(""))
            {
                throw notFoundException;
            }

            if (documentUploadId == null || documentUploadId.Trim().Equals(""))
            {
                throw new NotFoundException(String.Format("document with id '{0}' not found", documentUploadId));
            }

            DisputeAddEvidenceRequest request = new DisputeAddEvidenceRequest();
            request.DocumentUploadId = documentUploadId;

            try {
                XmlNode disputeEvidenceXML = Service.Post(Service.MerchantPath() + "/disputes/" + disputeId + "/evidence", request);

                return new ResultImpl<DisputeEvidence>(new NodeWrapper(disputeEvidenceXML), Gateway);
            } catch (NotFoundException) {
                throw notFoundException;
            }
        }

        public virtual async Task<Result<DisputeEvidence>> AddFileEvidenceAsync(string disputeId, string documentUploadId)
        {
            NotFoundException notFoundException = new NotFoundException(String.Format("dispute with id '{0}' not found", disputeId));

            if (disputeId == null || disputeId.Trim().Equals(""))
            {
                throw notFoundException;
            }

            if (documentUploadId == null || documentUploadId.Trim().Equals(""))
            {
                throw new NotFoundException(String.Format("document with id '{0}' not found", documentUploadId));
            }

            DisputeAddEvidenceRequest request = new DisputeAddEvidenceRequest();
            request.DocumentUploadId = documentUploadId;

            try {
                XmlNode disputeEvidenceXML = await Service.PostAsync(Service.MerchantPath() + "/disputes/" + disputeId + "/evidence", request).ConfigureAwait(false);

                return new ResultImpl<DisputeEvidence>(new NodeWrapper(disputeEvidenceXML), Gateway);
            } catch (NotFoundException) {
                throw notFoundException;
            }
        }

        public virtual Result<DisputeEvidence> AddTextEvidence(string disputeId, string content)
        {
            NotFoundException notFoundException = new NotFoundException(String.Format("dispute with id '{0}' not found", disputeId));

            if (disputeId == null || disputeId.Trim().Equals(""))
            {
                throw notFoundException;
            }

            if (content == null || content.Trim().Equals(""))
            {
                throw new ArgumentException("content cannot be empty");
            }


            DisputeAddEvidenceRequest request = new DisputeAddEvidenceRequest();
            request.Comments = content;

            try {
                XmlNode disputeEvidenceXML = Service.Post(Service.MerchantPath() + "/disputes/" + disputeId + "/evidence", request);

                return new ResultImpl<DisputeEvidence>(new NodeWrapper(disputeEvidenceXML), Gateway);
            } catch (NotFoundException) {
                throw notFoundException;
            }
        }

        public virtual async Task<Result<DisputeEvidence>> AddTextEvidenceAsync(string disputeId, string content)
        {
            NotFoundException notFoundException = new NotFoundException(String.Format("dispute with id '{0}' not found", disputeId));

            if (disputeId == null || disputeId.Trim().Equals(""))
            {
                throw notFoundException;
            }

            if (content == null || content.Trim().Equals(""))
            {
                throw new ArgumentException("content cannot be empty");
            }

            DisputeAddEvidenceRequest request = new DisputeAddEvidenceRequest();
            request.Comments = content;

            try {
                XmlNode disputeEvidenceXML = await Service.PostAsync(Service.MerchantPath() + "/disputes/" + disputeId + "/evidence", request);

                return new ResultImpl<DisputeEvidence>(new NodeWrapper(disputeEvidenceXML), Gateway);
            } catch (NotFoundException) {
                throw notFoundException;
            }
        }

        public virtual Result<Dispute> Finalize(string disputeId)
        {
            NotFoundException notFoundException = new NotFoundException(String.Format("dispute with id '{0}' not found", disputeId));

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
            NotFoundException notFoundException = new NotFoundException(String.Format("dispute with id '{0}' not found", disputeId));
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
            NotFoundException notFoundException = new NotFoundException(String.Format("dispute with id '{0}' not found", disputeId));

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
            NotFoundException notFoundException = new NotFoundException(String.Format("dispute with id '{0}' not found", disputeId));
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
            NotFoundException notFoundException = new NotFoundException(String.Format("evidence with id '{0}' for dispute with id '{1}' not found", evidenceId, disputeId));

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
            NotFoundException notFoundException = new NotFoundException(String.Format("evidence with id '{0}' for dispute with id '{1}' not found", evidenceId, disputeId));

            if (disputeId == null || disputeId.Trim().Equals("") || evidenceId == null || evidenceId.Trim().Equals(""))
            {
                throw notFoundException; 
            }

            try {
                XmlNode disputeXML = await Service.DeleteAsync(Service.MerchantPath() + "/disputes/" + disputeId + "/evidence/" + evidenceId);

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
