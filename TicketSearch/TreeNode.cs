using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketSearch
{
    public class TreeNode
    {
        public char Value { get; set; }
        public List<TreeNode> Children { get; set; }
        public TreeNode Parent { get; set; }
        public int Depth { get; set; }

        public TreeNode(char value, int depth, TreeNode parent)
        {
            Value = value;
            Children = new List<TreeNode>();
            Depth = depth;
            Parent = parent;
        }

        public bool IsLeaf()
        {
            return Children.Count == 0;
        }

        public TreeNode FindChildNode(char c)
        {
            return Children.FirstOrDefault(child => child.Value == c);
        }

        public void DeleteChildNode(char c)
        {
            for (var i = 0; i < Children.Count; i++)
                if (Children[i].Value == c)
                    Children.RemoveAt(i);
        }
    }

    public class Trie
    {
        readonly TreeNode _root;

        public Trie()
        {
            _root = new TreeNode('^', 0, null);
        }

        public TreeNode Prefix(string s)
        {
            var currentNode = _root;
            var result = currentNode;

            foreach (var c in s)
            {
                currentNode = currentNode.FindChildNode(c);

                if (currentNode == null)
                    break;

                result = currentNode;
            }

            return result;
        }

        public bool Search(string s)
        {
            var prefix = Prefix(s);
            return prefix.Depth == s.Length && prefix.FindChildNode('$') != null;
        }

        public void InsertRange(IEnumerable<string> items)
        {
            foreach (string item in items)
            {
                Insert(item.ToUpper().Trim());
            }
        }

        public void Insert(string s)
        {
            var commonPrefix = Prefix(s);
            var current = commonPrefix;

            for (var i = current.Depth; i < s.Length; i++)
            {
                var newNode = new TreeNode(s[i], current.Depth + 1, current);
                current.Children.Add(newNode);
                current = newNode;
            }

            current.Children.Add(new TreeNode('$', current.Depth + 1, current));
        }

        public void Delete(string s)
        {
            if (!Search(s))
                return;

            var node = Prefix(s).FindChildNode('$');

            while (node.IsLeaf())
            {
                var parent = node.Parent;
                parent.DeleteChildNode(node.Value);
                node = parent;
            }
        }

        public static IEnumerable<string> suffixes(TreeNode parent)
        {
            var sb = new StringBuilder();
            return suffixes(parent, sb).Select(suffix => suffix.TrimEnd('$'));
        }

        public static IEnumerable<string> suffixes(TreeNode parent, StringBuilder current)
        {
            if (parent.IsLeaf())
            {
                yield return current.ToString();
            }
            else
            {
                foreach (var child in parent.Children)
                {
                    current.Append(child.Value);

                    foreach (var value in suffixes(child, current))
                        yield return value;

                    --current.Length;
                }
            }
        }
    }
}

//refhttps://stackoverflow.com/questions/45509117/fastest-starts-with-search-algorithm