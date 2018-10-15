using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
                Stopwatch sw = new Stopwatch();
                sw.Start();
                foreach (var suffix in Trie.suffixes(node))
                    cities.Add(prefix + suffix);
                sw.Stop();
                TimeElapseLabel.Text = sw.Elapsed.TotalMilliseconds.ToString() + " ms";
                foreach (string local in cities)
                    listView1.Items.Add(local);
            }
            else
                listView1.Items.Add("No results");
        }

        
    }
}
