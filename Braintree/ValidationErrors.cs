using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class ValidationErrors
    {
        private Dictionary<String, ValidationError> errors;
        private Dictionary<String, ValidationErrors> nestedErrors;

        public ValidationErrors(NodeWrapper node)
        {
            errors = new Dictionary<String, ValidationError>();
            nestedErrors = new Dictionary<String, ValidationErrors>();
            PopulateErrors(node);
        }

        public ValidationErrors()
        {
            errors = new Dictionary<String, ValidationError>();
            nestedErrors = new Dictionary<String, ValidationErrors>();
        }

        public void AddError(String fieldName, ValidationError error)
        {
            errors[fieldName] = error;
        }

        public void AddErrors(String objectName, ValidationErrors errors)
        {
            nestedErrors[objectName] = errors;
        }

        public int DeepSize() {
            int size = errors.Count;

            foreach (ValidationErrors nestedError in nestedErrors.Values) {
                size += nestedError.DeepSize();
            }
        
            return size;
        }

        public ValidationErrors ForObject(String objectName)
        {
            if (nestedErrors.ContainsKey(objectName)) return nestedErrors[objectName];

            return null;
        }

        public ValidationError OnField(String fieldName)
        {
            if (errors.ContainsKey(fieldName)) return errors[fieldName];

            return null;
        }

        public int size()
        {
            return errors.Count;
        }

        private void PopulateErrors(NodeWrapper node)
        {
            if (node.GetName() == "api-error-response")
            {
                node = node.GetNode("errors");
            }
            
            List<NodeWrapper> errorResponses = node.GetChildren();
            foreach (NodeWrapper errorResponse in errorResponses) {
                if (errorResponse.GetName() != "errors") {
                    nestedErrors[errorResponse.GetName()] = new ValidationErrors(errorResponse);
                } else {
                    PopulateTopLevelErrors(errorResponse.GetArray("error"));
                }
            }
        }

        private void PopulateTopLevelErrors(List<NodeWrapper> childErrors) {
            foreach (NodeWrapper childError in childErrors) {
                errors[childError.GetString("attribute")] = new ValidationError(childError.GetString("code"), childError.GetString("message"));
            }
        }
    }
}
