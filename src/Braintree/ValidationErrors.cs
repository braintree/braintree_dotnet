#pragma warning disable 1591

using System.Collections.Generic;

namespace Braintree
{

    /// <summary>
    /// A collection of Validation Errors returned by the Braintree Gateway
    /// </summary>
    /// <example>
    /// For more information about Validation Errors, see <a href="https://developers.braintreepayments.com/reference/general/validation-errors/overview/dotnet" target="_blank">https://developers.braintreepayments.com/reference/general/validation-errors/overview/dotnet</a>
    /// </example>
    public class ValidationErrors
    {
        private Dictionary<string, List<ValidationError>> errors;
        private Dictionary<string, ValidationErrors> nestedErrors;
        
        public int Count => errors.Count;

        public int DeepCount
        {
            get
            {
                int size = 0;

                foreach (var errorList in errors.Values)
                {
                    size += errorList.Count;
                }

                foreach (var nestedError in nestedErrors.Values)
                {
                    size += nestedError.DeepCount;
                }

                return size;
            }
        }

        public ValidationErrors(NodeWrapper node)
        {
            errors = new Dictionary<string, List<ValidationError>>();
            nestedErrors = new Dictionary<string, ValidationErrors>();
            PopulateErrors(node);
        }

        public ValidationErrors()
        {
            errors = new Dictionary<string, List<ValidationError>>();
            nestedErrors = new Dictionary<string, ValidationErrors>();
        }

        public virtual void AddError(string fieldName, ValidationError error)
        {
            if (!errors.ContainsKey(fieldName)) errors[fieldName] = new List<ValidationError>();

            errors[fieldName].Add(error);
        }

        public virtual void AddErrors(string objectName, ValidationErrors errors)
        {
            nestedErrors[objectName] = errors;
        }

        public List<ValidationError> All()
        {
            var results = new List<ValidationError>();
            foreach (var validationErrors in errors.Values)
            {
                results.AddRange(validationErrors);
            }

            return results;
        }

        public Dictionary<string, List<string>> ByFormField()
        {
            var dict = new Dictionary<string, List<string>>();

            foreach (var pair in errors)
            {
                string keyName = pair.Key.Replace('-', '_');

                if (!dict.ContainsKey(keyName))
                    dict[keyName] = new List<string>();

                foreach (var error in pair.Value)
                {
                    dict[keyName].Add(error.Message);
                }
            }

            foreach (var pair in nestedErrors)
            {
                foreach (var error in pair.Value.ByFormField())
                {
                    string keyName = pair.Key.Replace('-', '_');

                    dict[ComposeFieldName(keyName, error.Key)] = error.Value;
                }
            }

            return dict;
        }

        protected string ComposeFieldName(string prefix, string element)
        {
            var fieldName = prefix;

            foreach (var node in element.Replace("]", "").Split('['))
            {
                fieldName = $"{fieldName}[{node}]";
            }

            return fieldName;
        }

        public virtual List<ValidationError> DeepAll()
        {
            var results = new List<ValidationError>();
            results.AddRange(All());
            foreach (var validationErrors in nestedErrors.Values)
            {
                results.AddRange(validationErrors.DeepAll());
            }
            return results;
        }

        public virtual ValidationErrors ForIndex(int index)
        {
            return ForObject("index-" + index);
        }

        public virtual ValidationErrors ForObject(string objectName)
        {
            string key = StringUtil.Dasherize(objectName);
            if (nestedErrors.ContainsKey(key))
                return nestedErrors[key];

            return new ValidationErrors();
        }

        public virtual List<ValidationError> OnField(string fieldName)
        {
            string key = StringUtil.Underscore(fieldName);
            if (errors.ContainsKey(key))
                return errors[key];

            return null;
        }

        private void PopulateErrors(NodeWrapper node)
        {
            if (node.GetName() == "api-error-response")
            {
                node = node.GetNode("errors");
            }

            List<NodeWrapper> errorResponses = node.GetChildren();
            foreach (var errorResponse in errorResponses)
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
            foreach (var childError in childErrors)
            {
                if (!errors.ContainsKey(childError.GetString("attribute")))
                    errors[childError.GetString("attribute")] = new List<ValidationError>();

                errors[childError.GetString("attribute")].Add(new ValidationError(childError.GetString("attribute"), childError.GetString("code"), childError.GetString("message")));
            }
        }
    }
}
