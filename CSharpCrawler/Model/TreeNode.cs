using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCrawler.Model
{
    /// <summary>
    /// 字典树(Trie树)节点
    /// </summary>
    /// <remarks>Aho-Corasick算法</remarks>
    public class TreeNode
    {
        private char c;
        private TreeNode parent;
        private TreeNode failure;
        private ArrayList results;
        private TreeNode[] transitionAr;
        private string[] resultAr;
        private Hashtable transHash;

        public TreeNode(TreeNode parent,char c)
        {
            this.c = c;
            this.parent = parent;
            results = new ArrayList();
            transitionAr = new TreeNode[] { };
            resultAr = new string[] { };
            transHash = new Hashtable();
        }


        public char Char
        {
            get { return c; }
        }

        public TreeNode Parent
        {
            get { return parent; }
        }

        public TreeNode Failure
        {
            get { return failure; }
            set { failure = value; }
        }

        public TreeNode[] Transitions
        {
            get { return transitionAr; }
        }

        public string[] Results
        {
            get { return resultAr; }
        }

        public void AddResult(string result)
        {
            if (this.results.Contains(result))
                return;
            this.results.Add(result);
            resultAr = (string[])results.ToArray(typeof(string));
        }

        public void AddTransition(TreeNode node)
        {
            transHash.Add(node.c, node);
            TreeNode[] ar = new TreeNode[transHash.Values.Count];
            transHash.Values.CopyTo(ar, 0);
            transitionAr = ar;
        }

        public TreeNode GetTransition(char c)
        {
            return (TreeNode)transHash[c];
        }

        public bool ContainsTransition(char c)
        {
            return GetTransition(c) != null;
        }
    }
}
