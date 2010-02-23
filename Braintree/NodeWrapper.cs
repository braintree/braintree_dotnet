using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Braintree
{
    public class NodeWrapper
    {
        protected XmlNode node;

        public NodeWrapper(XmlNode node)
        {
            this.node = node;
        }

        public String GetString(String path)
        {
            if (GetNode(path) == null) return null;

            var value = GetNode(path).GetString();

            return value.Length == 0 ? null : value;
        }

        protected String GetString()
        {
            return node.InnerText;
        }

        public Int32? GetInteger(String path)
        {
            if (GetString(path) == null) return null;

            return Int32.Parse(GetString(path));
        }

        public String GetName()
        {
            return node.Name;
        }

        public Boolean IsRootNode()
        {
            return (node.ParentNode == null);
        }

        public NodeWrapper GetNode(String path)
        {
            XmlNode subNode = node.SelectSingleNode(path);
            if (subNode == null) return null;

            return new NodeWrapper(subNode);
        }

        public Dictionary<String, String> GetDictionary(String path)
        {
            if (GetNode(path) == null) return null;

            var result = new Dictionary<String, String>();

            foreach (NodeWrapper node in GetNode(path).GetChildren())
            {
                result.Add(node.GetName(), node.GetString());
            }

            return result;
        }

        public List<NodeWrapper> GetChildren()
        {
            XmlNodeList list = node.ChildNodes;
            var result = new List<NodeWrapper>();
            foreach (XmlNode n in list)
            {
                result.Add(new NodeWrapper(n));
            }

            return result;
        }

        public List<NodeWrapper> GetArray(String path)
        {
            XmlNodeList list = node.SelectNodes(path);
            var result = new List<NodeWrapper>();
            foreach (XmlNode n in list)
            {
                result.Add(new NodeWrapper(n));
            }

            return result;
        }

        public String OuterXml()
        {
            return node.OuterXml;
        }

        public Boolean GetBoolean(String path)
        {
            var value = GetString(path);

            if (value == null) return false;

            return Boolean.Parse(value);
        }

        public DateTime? GetDateTime(String path)
        {
            var value = GetString(path);
            if (value == null) return null;

            return DateTime.Parse(GetString(path));
        }

        public Boolean IsSuccess()
        {
            return GetNode("//api-error-response") == null;
        }

        private Boolean IsSameNode(NodeWrapper other)
        {
            return node.InnerXml == other.node.InnerXml;
        }

        private NodeWrapper GetParent()
        {
            if (node.ParentNode == null) return null;

            return new NodeWrapper(node.ParentNode);
        }

        private String XPathToNode(NodeWrapper node)
        {
            if (this.IsSameNode(node))
                return "";

            return XPathToNode(node.GetParent()) + "/" + node.GetName();
        }

        private String GetFormElementName(NodeWrapper leafNode)
        {
            String[] nodes = XPathToNode(leafNode).Split('/');
            String formElementName = nodes[1];

            for (int i = 2; i < nodes.Length; i++)
            {
                formElementName += "[" + nodes[i] + "]";
            }

            formElementName = formElementName.Replace('-', '_');

            return formElementName;
        }

        public Dictionary<String, String> GetFormParameters()
        {
            var formParameters = new Dictionary<String, String>();

            foreach (NodeWrapper paramNode in GetArray(".//*[not(*)]"))
            {
                formParameters[GetFormElementName(paramNode)] = paramNode.GetString(".");
            }

            return formParameters;
        }
    }
}
