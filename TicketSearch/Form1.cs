using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TicketSearch
{
    public partial class Form1 : Form
    {
        private Trie trie;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tb1.Enabled = false;
            trie = new Trie();
            try
            {
                trie.InsertRange(File.ReadLines(@"world-cities.txt"));
            }
            catch (Exception)
            {
                throw;
            }
            tb1.Enabled = true;

        }

        private void Tb1_TextChanged(object sender, EventArgs e)
        {
            string prefix = tb1.Text.ToUpper();
            listView1.Items.Clear();

            if (string.IsNullOrEmpty(prefix))
                return;

            var node = trie.Prefix(prefix);
            List<string> cities = new List<string>();

            if (node.Depth == prefix.Length)
            {
                foreach (var suffix in suffixes(node))
                    cities.Add(prefix + suffix);

                foreach (string local in cities)
                    listView1.Items.Add(local);
            }
            else
                listView1.Items.Add("No results");
        }

        static IEnumerable<string> suffixes(TreeNode parent)
        {
            var sb = new StringBuilder();
            return suffixes(parent, sb).Select(suffix => suffix.TrimEnd('$'));
        }

        static IEnumerable<string> suffixes(TreeNode parent, StringBuilder current)
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
