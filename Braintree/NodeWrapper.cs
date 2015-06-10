#pragma warning disable 1591

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Globalization;

namespace Braintree
{
    public class NodeWrapper
    {
        protected XmlNode node;

        public NodeWrapper(XmlNode node)
        {
            this.node = node;
        }

        public bool IsEmpty()
        {
          var attribute = node.Attributes["nil"];
          if (attribute != null)
          {
            return XmlConvert.ToBoolean(attribute.Value);
          }
          return false;
        }

        public virtual string GetString(string path)
        {
            if (GetNode(path) == null) return null;

            var value = GetNode(path).GetString();

            return value.Length == 0 ? null : value;
        }

        protected virtual string GetString()
        {
            return node.InnerText;
        }

        public virtual int? GetInteger(string path)
        {
            if (GetString(path) == null) return null;

            return int.Parse(GetString(path));
        }

        public virtual Decimal? GetDecimal(string path)
        {
            if (GetString(path) == null) return null;

            return Decimal.Parse(GetString(path), System.Globalization.CultureInfo.InvariantCulture);
        }

        public virtual string GetName()
        {
            return node.Name;
        }

        public virtual bool IsRootNode()
        {
            return (node.ParentNode == null);
        }

        public virtual NodeWrapper GetNode(string path)
        {
            XmlNode subNode = node.SelectSingleNode(path);
            if (subNode == null) return null;

            return new NodeWrapper(subNode);
        }

        public virtual Dictionary<string, string> GetDictionary(string path)
        {
            if (GetNode(path) == null) return null;

            var result = new Dictionary<string, string>();

            foreach (NodeWrapper node in GetNode(path).GetChildren())
            {
                result.Add(Underscore(node.GetName()), node.GetString());
            }

            return result;
        }

        public virtual List<NodeWrapper> GetChildren()
        {
            XmlNodeList list = node.ChildNodes;
            var result = new List<NodeWrapper>();
            foreach (XmlNode n in list)
            {
                result.Add(new NodeWrapper(n));
            }

            return result;
        }

        public virtual List<NodeWrapper> GetList(string path)
        {
            XmlNodeList list = node.SelectNodes(path);
            var result = new List<NodeWrapper>();
            foreach (XmlNode n in list)
            {
                result.Add(new NodeWrapper(n));
            }

            return result;
        }

        public virtual List<string> GetStrings(string path)
        {
            List<string> strings = new List<string>();
            foreach(NodeWrapper stringNode in GetList(path))
            {
                strings.Add(stringNode.GetString("."));
            }
            return strings;
        }

        public virtual string OuterXml()
        {
            return node.OuterXml;
        }

        public virtual bool? GetBoolean(string path)
        {
            if (GetString(path) == null) return null;

            return bool.Parse(GetString(path));
        }

        public virtual DateTime? GetDateTime(string path)
        {
            var value = GetString(path);
            if (value == null) return null;

            return DateTime.Parse(value, null, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }

        public virtual bool IsSuccess()
        {
            return GetNode("//api-error-response") == null;
        }

        private bool IsSameNode(NodeWrapper other)
        {
            return node.InnerXml == other.node.InnerXml;
        }

        private NodeWrapper GetParent()
        {
            if (node.ParentNode == null) return null;

            return new NodeWrapper(node.ParentNode);
        }

        private string Underscore(string str)
        {
            return str.Replace("-", "_");
        }

        private string XPathToNode(NodeWrapper node)
        {
            if (this.IsSameNode(node))
                return "";

            return XPathToNode(node.GetParent()) + "/" + node.GetName();
        }

        private string GetFormElementName(NodeWrapper leafNode)
        {
            string[] nodes = XPathToNode(leafNode).Split('/');
            string formElementName = nodes[1];

            for (int i = 2; i < nodes.Length; i++)
            {
                formElementName += "[" + nodes[i] + "]";
            }

            formElementName = formElementName.Replace('-', '_');

            return formElementName;
        }

        public virtual Dictionary<string, string> GetFormParameters()
        {
            var formParameters = new Dictionary<string, string>();

            foreach (NodeWrapper paramNode in GetList(".//*[not(*)]"))
            {
                formParameters[GetFormElementName(paramNode)] = paramNode.GetString(".");
            }

            return formParameters;
        }
    }
}
