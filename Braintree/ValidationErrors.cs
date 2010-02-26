using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class ValidationErrors
    {
        private Dictionary<String, List<ValidationError>> errors;
        private Dictionary<String, ValidationErrors> nestedErrors;

        public ValidationErrors(NodeWrapper node)
        {
            errors = new Dictionary<String, List<ValidationError>>();
            nestedErrors = new Dictionary<String, ValidationErrors>();
            PopulateErrors(node);
        }

        public ValidationErrors()
        {
            errors = new Dictionary<String, List<ValidationError>>();
            nestedErrors = new Dictionary<String, ValidationErrors>();
        }

        public virtual void AddError(String fieldName, ValidationError error)
        {
            if (!errors.ContainsKey(fieldName)) errors[fieldName] = new List<ValidationError>();

            errors[fieldName].Add(error);
        }

        public virtual void AddErrors(String objectName, ValidationErrors errors)
        {
            nestedErrors[objectName] = errors;
        }

        public virtual int DeepSize()
        {
            int size = 0;

            foreach (List<ValidationError> errorList in errors.Values)
            {
                size += errorList.Count;
            }

            foreach (ValidationErrors nestedError in nestedErrors.Values)
            {
                size += nestedError.DeepSize();
            }

            return size;
        }

        public virtual ValidationErrors ForObject(String objectName)
        {
            if (nestedErrors.ContainsKey(objectName)) return nestedErrors[objectName];

            return null;
        }

        public virtual List<ValidationError> OnField(String fieldName)
        {
            if (errors.ContainsKey(fieldName)) return errors[fieldName];

            return null;
        }

        public virtual int size()
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
            foreach (NodeWrapper errorResponse in errorResponses)
            {
                if (errorResponse.GetName() != "errors")
                {
                    nestedErrors[errorResponse.GetName()] = new ValidationErrors(errorResponse);
                }
                else
                {
                    PopulateTopLevelErrors(errorResponse.GetList("error"));
                }
            }
        }

        private void PopulateTopLevelErrors(List<NodeWrapper> childErrors)
        {
            foreach (NodeWrapper childError in childErrors)
            {
                if (!errors.ContainsKey(childError.GetString("attribute"))) errors[childError.GetString("attribute")] = new List<ValidationError>();

                errors[childError.GetString("attribute")].Add(new ValidationError(childError.GetString("code"), childError.GetString("message")));
            }
        }
    }
}
