#pragma warning disable 1591

using Braintree.Exceptions;
using System.Threading.Tasks;
using System;
using System.Xml;

namespace Braintree
{

    /// <summary>
    /// Provides operations for creating document uploads
    /// </summary>
    public class DocumentUploadGateway : IDocumentUploadGateway
    {
        private readonly BraintreeService Service;
        private readonly IBraintreeGateway Gateway;

        protected internal DocumentUploadGateway(IBraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            Gateway = gateway;
            Service = gateway.Service;
        }

        public virtual Result<DocumentUpload> Create(DocumentUploadRequest request)
        {
            if (request.DocumentKind == null)
            {
                throw new ArgumentException("DocumentKind must not be null");
            }

            if (request.File == null)
            {
                throw new ArgumentException("File must not be null");
            }

            XmlNode documentUploadXML = Service.PostMultipart(Service.MerchantPath() + "/document_uploads", request, request.File);

            return new ResultImpl<DocumentUpload>(new NodeWrapper(documentUploadXML), Gateway);
        }

        public virtual async Task<Result<DocumentUpload>> CreateAsync(DocumentUploadRequest request)
        {
            if (request.DocumentKind == null)
            {
                throw new ArgumentException("DocumentKind must not be null");
            }

            if (request.File == null)
            {
                throw new ArgumentException("File must not be null");
            }

            XmlNode documentUploadXML = await Service.PostMultipartAsync(Service.MerchantPath() + "/document_uploads", request, request.File).ConfigureAwait(false);

            return new ResultImpl<DocumentUpload>(new NodeWrapper(documentUploadXML), Gateway);
        }
    }
}
