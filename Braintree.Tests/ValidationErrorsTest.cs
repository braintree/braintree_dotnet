using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Braintree;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class ValidationErrorsTest
    {
        [Test]
        public void OnField_WithValidationError()
        {
            ValidationErrors errors = new ValidationErrors();
            errors.AddError("country_name", new ValidationError("91803", "invalid country"));
            Assert.AreEqual(ValidationErrorCode.ADDRESS_COUNTRY_NAME_IS_NOT_ACCEPTED, errors.OnField("country_name")[0].Code);
            Assert.AreEqual("invalid country", errors.OnField("country_name")[0].Message);
        }

        [Test]
        public void OnField_WithNonExistingField()
        {
            ValidationErrors errors = new ValidationErrors();
            Assert.IsNull(errors.OnField("foo"));
        }

        [Test]
        public void ForObject_WithNestedErrors()
        {
            ValidationErrors addressErrors = new ValidationErrors();
            addressErrors.AddError("country_name", new ValidationError("91803", "invalid country"));

            ValidationErrors errors = new ValidationErrors();
            errors.AddErrors("address", addressErrors);
            Assert.AreEqual(ValidationErrorCode.ADDRESS_COUNTRY_NAME_IS_NOT_ACCEPTED, errors.ForObject("address").OnField("country_name")[0].Code);
            Assert.AreEqual("invalid country", errors.ForObject("address").OnField("country_name")[0].Message);
        }

        [Test]
        public void ForObject_WithNonExistingObject()
        {
            ValidationErrors errors = new ValidationErrors();
            Assert.AreEqual(0, errors.ForObject("address").size());
        }

        [Test]
        public void Size_WithShallowErrors()
        {
            ValidationErrors errors = new ValidationErrors();
            errors.AddError("country_name", new ValidationError("1", "invalid country"));
            errors.AddError("another_field", new ValidationError("2", "another message"));
            Assert.AreEqual(2, errors.size());
        }

        [Test]
        public void DeepSize_WithNestedErrors()
        {
            ValidationErrors addressErrors = new ValidationErrors();
            addressErrors.AddError("country_name", new ValidationError("1", "invalid country"));
            addressErrors.AddError("another_field", new ValidationError("2", "another message"));

            ValidationErrors errors = new ValidationErrors();
            errors.AddError("some_field", new ValidationError("3", "some message"));
            errors.AddErrors("address", addressErrors);

            Assert.AreEqual(3, errors.DeepSize());
            Assert.AreEqual(1, errors.size());

            Assert.AreEqual(2, errors.ForObject("address").DeepSize());
            Assert.AreEqual(2, errors.ForObject("address").size());
        }

        [Test]
        public void Constructor_ParsesSimpleValidationErrors()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<api-error-response>");
            builder.Append("  <errors>");
            builder.Append("    <address>");
            builder.Append("      <errors type=\"array\">");
            builder.Append("        <error>");
            builder.Append("          <code>91803</code>");
            builder.Append("          <message>Country name is not an accepted country.</message>");
            builder.Append("          <attribute type=\"symbol\">country_name</attribute>");
            builder.Append("        </error>");
            builder.Append("      </errors>");
            builder.Append("    </address>");
            builder.Append("    <errors type=\"array\"/>");
            builder.Append("  </errors>");
            builder.Append("</api-error-response>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());
            ValidationErrors errors = new ValidationErrors(new NodeWrapper(doc.ChildNodes[1]));
            Assert.AreEqual(1, errors.DeepSize());
            Assert.AreEqual(ValidationErrorCode.ADDRESS_COUNTRY_NAME_IS_NOT_ACCEPTED, errors.ForObject("address").OnField("country_name")[0].Code);
        }

        [Test]
        public void Constructor_ParsesMulitpleValidationErrorsOnOneObject()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<api-error-response>");
            builder.Append("  <errors>");
            builder.Append("    <address>");
            builder.Append("      <errors type=\"array\">");
            builder.Append("        <error>");
            builder.Append("          <code>91803</code>");
            builder.Append("          <message>Country name is not an accepted country.</message>");
            builder.Append("          <attribute type=\"symbol\">country_name</attribute>");
            builder.Append("        </error>");
            builder.Append("        <error>");
            builder.Append("          <code>81812</code>");
            builder.Append("          <message>Street address is too long.</message>");
            builder.Append("          <attribute type=\"symbol\">street_address</attribute>");
            builder.Append("        </error>");
            builder.Append("      </errors>");
            builder.Append("    </address>");
            builder.Append("    <errors type=\"array\"/>");
            builder.Append("  </errors>");
            builder.Append("</api-error-response>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());
            ValidationErrors errors = new ValidationErrors(new NodeWrapper(doc.ChildNodes[1]));
            Assert.AreEqual(2, errors.DeepSize());
            Assert.AreEqual(ValidationErrorCode.ADDRESS_COUNTRY_NAME_IS_NOT_ACCEPTED, errors.ForObject("address").OnField("country_name")[0].Code);
            Assert.AreEqual(ValidationErrorCode.ADDRESS_STREET_ADDRESS_IS_TOO_LONG, errors.ForObject("address").OnField("street_address")[0].Code);
        }

        [Test]
        public void Constructor_ParsesValidationErrorOnNestedObject()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<api-error-response>");
            builder.Append("  <errors>");
            builder.Append("    <errors type=\"array\"/>");
            builder.Append("    <credit-card>");
            builder.Append("      <billing-address>");
            builder.Append("        <errors type=\"array\">");
            builder.Append("          <error>");
            builder.Append("            <code>91803</code>");
            builder.Append("            <message>Country name is not an accepted country.</message>");
            builder.Append("            <attribute type=\"symbol\">country_name</attribute>");
            builder.Append("          </error>");
            builder.Append("        </errors>");
            builder.Append("      </billing-address>");
            builder.Append("      <errors type=\"array\"/>");
            builder.Append("    </credit-card>");
            builder.Append("  </errors>");
            builder.Append("</api-error-response>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());
            ValidationErrors errors = new ValidationErrors(new NodeWrapper(doc.ChildNodes[1]));
            Assert.AreEqual(1, errors.DeepSize());
            Assert.AreEqual(ValidationErrorCode.ADDRESS_COUNTRY_NAME_IS_NOT_ACCEPTED, errors.ForObject("credit-card").ForObject("billing-address").OnField("country_name")[0].Code);
        }

        [Test]
        public void Constructor_ParsesMultipleErrorsOnSingleField()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<api-error-response>");
            builder.Append("  <errors>");
            builder.Append("    <transaction>");
            builder.Append("      <errors type=\"array\">");
            builder.Append("        <error>");
            builder.Append("          <code>91516</code>");
            builder.Append("          <message>Cannot provide both payment_method_token and customer_id unless the payment_method belongs to the customer.</message>");
            builder.Append("          <attribute type=\"symbol\">base</attribute>");
            builder.Append("        </error>");
            builder.Append("        <error>");
            builder.Append("          <code>91515</code>");
            builder.Append("          <message>Cannot provide both payment_method_token and credit_card attributes.</message>");
            builder.Append("          <attribute type=\"symbol\">base</attribute>");
            builder.Append("        </error>");
            builder.Append("      </errors>");
            builder.Append("    </transaction>");
            builder.Append("    <errors type=\"array\"/>");
            builder.Append("  </errors>");
            builder.Append("</api-error-response>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());
            ValidationErrors errors = new ValidationErrors(new NodeWrapper(doc.ChildNodes[1]));
            Assert.AreEqual(2, errors.DeepSize());
            Assert.AreEqual(2, errors.ForObject("transaction").OnField("base").Count);
        }

        [Test]
        public void Constructor_ParsesValidationErrorsAtMultipleLevels()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<api-error-response>");
            builder.Append("  <errors>");
            builder.Append("    <customer>");
            builder.Append("      <errors type=\"array\">");
            builder.Append("        <error>");
            builder.Append("          <code>81608</code>");
            builder.Append("          <message>First name is too long.</message>");
            builder.Append("          <attribute type=\"symbol\">first_name</attribute>");
            builder.Append("        </error>");
            builder.Append("      </errors>");
            builder.Append("      <credit-card>");
            builder.Append("        <billing-address>");
            builder.Append("          <errors type=\"array\">");
            builder.Append("            <error>");
            builder.Append("              <code>91803</code>");
            builder.Append("              <message>Country name is not an accepted country.</message>");
            builder.Append("              <attribute type=\"symbol\">country_name</attribute>");
            builder.Append("            </error>");
            builder.Append("          </errors>");
            builder.Append("        </billing-address>");
            builder.Append("        <errors type=\"array\">");
            builder.Append("          <error>");
            builder.Append("            <code>81715</code>");
            builder.Append("            <message>Credit card number is invalid.</message>");
            builder.Append("            <attribute type=\"symbol\">number</attribute>");
            builder.Append("          </error>");
            builder.Append("        </errors>");
            builder.Append("      </credit-card>");
            builder.Append("    </customer>");
            builder.Append("    <errors type=\"array\"/>");
            builder.Append("  </errors>");
            builder.Append("</api-error-response>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());
            ValidationErrors errors = new ValidationErrors(new NodeWrapper(doc.ChildNodes[1]));

            Assert.AreEqual(3, errors.DeepSize());
            Assert.AreEqual(0, errors.size());

            Assert.AreEqual(3, errors.ForObject("customer").DeepSize());
            Assert.AreEqual(1, errors.ForObject("customer").size());
            Assert.AreEqual(ValidationErrorCode.CUSTOMER_FIRST_NAME_IS_TOO_LONG, errors.ForObject("customer").OnField("first_name")[0].Code);

            Assert.AreEqual(2, errors.ForObject("customer").ForObject("credit-card").DeepSize());
            Assert.AreEqual(1, errors.ForObject("customer").ForObject("credit-card").size());
            Assert.AreEqual(ValidationErrorCode.CREDIT_CARD_NUMBER_IS_INVALID, errors.ForObject("customer").ForObject("credit-card").OnField("number")[0].Code);

            Assert.AreEqual(1, errors.ForObject("customer").ForObject("credit-card").ForObject("billing-address").DeepSize());
            Assert.AreEqual(1, errors.ForObject("customer").ForObject("credit-card").ForObject("billing-address").size());
            Assert.AreEqual(ValidationErrorCode.ADDRESS_COUNTRY_NAME_IS_NOT_ACCEPTED, errors.ForObject("customer").ForObject("credit-card").ForObject("billing-address").OnField("country_name")[0].Code);
        }

        [Test]
        public void ByFormField_FlattensErrorsToFormElementNames()
        {
            var errors = new ValidationErrors();
            errors.AddError("some_error", new ValidationError("1", "some error"));
            errors.AddError("some_error", new ValidationError("2", "some other error"));

            var nestedErrors = new ValidationErrors();
            nestedErrors.AddError("some_nested_error", new ValidationError("3", "some nested error"));
            nestedErrors.AddError("some_nested_error", new ValidationError("4", "some other nested error"));

            var nestedNestedErrors = new ValidationErrors();
            nestedNestedErrors.AddError("some_nested_nested_error", new ValidationError("5", "some nested nested error"));
            nestedNestedErrors.AddError("some_nested_nested_error", new ValidationError("6", "some other nested nested error"));

            nestedErrors.AddErrors("some_nested_object", nestedNestedErrors);
            errors.AddErrors("some_object", nestedErrors);

            Dictionary<String, List<String>> formErrors = errors.ByFormField();
            Assert.AreEqual("some error", formErrors["some_error"][0]);
            Assert.AreEqual("some other error", formErrors["some_error"][1]);

            Assert.AreEqual("some nested error", formErrors["some_object[some_nested_error]"][0]);
            Assert.AreEqual("some other nested error", formErrors["some_object[some_nested_error]"][1]);

            Assert.AreEqual("some nested nested error", formErrors["some_object[some_nested_object][some_nested_nested_error]"][0]);
            Assert.AreEqual("some other nested nested error", formErrors["some_object[some_nested_object][some_nested_nested_error]"][1]);
        }

        [Test]
        public void ByFormField_UnderscoresNodes()
        {
            var errors = new ValidationErrors();
            errors.AddError("some-error", new ValidationError("1", "some error"));
            errors.AddError("some-error", new ValidationError("2", "some other error"));

            var nestedErrors = new ValidationErrors();
            nestedErrors.AddError("some-nested-error", new ValidationError("3", "some nested error"));
            nestedErrors.AddError("some-nested-error", new ValidationError("4", "some other nested error"));

            var nestedNestedErrors = new ValidationErrors();
            nestedNestedErrors.AddError("some-nested-nested-error", new ValidationError("5", "some nested nested error"));
            nestedNestedErrors.AddError("some-nested-nested-error", new ValidationError("6", "some other nested nested error"));

            nestedErrors.AddErrors("some-nested-object", nestedNestedErrors);
            errors.AddErrors("some-object", nestedErrors);

            Dictionary<String, List<String>> formErrors = errors.ByFormField();
            Assert.AreEqual("some error", formErrors["some_error"][0]);
            Assert.AreEqual("some other error", formErrors["some_error"][1]);

            Assert.AreEqual("some nested error", formErrors["some_object[some_nested_error]"][0]);
            Assert.AreEqual("some other nested error", formErrors["some_object[some_nested_error]"][1]);

            Assert.AreEqual("some nested nested error", formErrors["some_object[some_nested_object][some_nested_nested_error]"][0]);
            Assert.AreEqual("some other nested nested error", formErrors["some_object[some_nested_object][some_nested_nested_error]"][1]);
        }
    }
}
