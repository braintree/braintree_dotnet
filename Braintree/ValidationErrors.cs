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

        public Dictionary<String, List<String>> ByFormField()
        {
            var dict = new Dictionary<String, List<String>>();

            foreach (var pair in errors)
            {
                String keyName = pair.Key.Replace('-', '_');

                if (!dict.ContainsKey(keyName)) dict[keyName] = new List<String>();

                foreach (var error in pair.Value)
                {
                    dict[keyName].Add(error.Message);
                }
            }

            foreach (var pair in nestedErrors)
            {
                foreach (var error in pair.Value.ByFormField())
                {
                    String keyName = pair.Key.Replace('-', '_');

                    dict[ComposeFieldName(keyName, error.Key)] = error.Value;
                }
            }

            return dict;
        }

        protected String ComposeFieldName(String prefix, String element)
        {
            var fieldName = prefix;

            foreach (var node in element.Replace("]", "").Split('['))
            {
                fieldName = String.Format("{0}[{1}]", fieldName, node);
            }

            return fieldName;
        }
    }
}
