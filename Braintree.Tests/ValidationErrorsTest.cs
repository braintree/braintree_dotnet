using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Braintree;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    class ValidationErrorsTest
    {
        [Test]
        public void OnField_WithValidationError()
        {
            ValidationErrors errors = new ValidationErrors();
            errors.AddError("country_name", new ValidationError("1", "invalid country"));
            Assert.AreEqual("1", errors.OnField("country_name").Code);
            Assert.AreEqual("invalid country", errors.OnField("country_name").Message);
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
            addressErrors.AddError("country_name", new ValidationError("1", "invalid country"));

            ValidationErrors errors = new ValidationErrors();
            errors.AddErrors("address", addressErrors);
            Assert.AreEqual("1", errors.ForObject("address").OnField("country_name").Code);
            Assert.AreEqual("invalid country", errors.ForObject("address").OnField("country_name").Message);
        }

        [Test]
        public void ForObject_WithNonExistingObject()
        {
            ValidationErrors errors = new ValidationErrors();
            Assert.IsNull(errors.ForObject("address"));
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
            Assert.AreEqual("91803", errors.ForObject("address").OnField("country_name").Code);
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
            builder.Append("          <code>1</code>");
            builder.Append("          <message>Country name is not an accepted country.</message>");
            builder.Append("          <attribute type=\"symbol\">country_name</attribute>");
            builder.Append("        </error>");
            builder.Append("        <error>");
            builder.Append("          <code>2</code>");
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
            Assert.AreEqual("1", errors.ForObject("address").OnField("country_name").Code);
            Assert.AreEqual("2", errors.ForObject("address").OnField("street_address").Code);
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
            Assert.AreEqual("91803", errors.ForObject("credit-card").ForObject("billing-address").OnField("country_name").Code);
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
            Assert.AreEqual("81608", errors.ForObject("customer").OnField("first_name").Code);

            Assert.AreEqual(2, errors.ForObject("customer").ForObject("credit-card").DeepSize());
            Assert.AreEqual(1, errors.ForObject("customer").ForObject("credit-card").size());
            Assert.AreEqual("81715", errors.ForObject("customer").ForObject("credit-card").OnField("number").Code);

            Assert.AreEqual(1, errors.ForObject("customer").ForObject("credit-card").ForObject("billing-address").DeepSize());
            Assert.AreEqual(1, errors.ForObject("customer").ForObject("credit-card").ForObject("billing-address").size());
            Assert.AreEqual("91803", errors.ForObject("customer").ForObject("credit-card").ForObject("billing-address").OnField("country_name").Code);
        }
    }
}
