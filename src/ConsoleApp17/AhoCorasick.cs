using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SearchText
{
    public class AhoCorasick
    {
        private TreeNode treeRoot=null;

        public bool Build(params string[] keywords)
        {
            CheckKeywords(keywords);

            treeRoot = BuildTree(keywords);

            return true;
        }

        private static TreeNode BuildTree(string[] keywords)
        {
            TreeNode root = new TreeNode(null, ' ');

            {
                TreeNode cNode = root;
                TreeNode newNode = null;
                foreach(string keyword in keywords)
                {
                    cNode = root;

                    foreach(char c in keyword)
                    {
                        newNode = null;

                        newNode = cNode.GetTransition(c);
                        if(newNode==null)
                        {
                            newNode = new TreeNode(cNode, c);
                            cNode.AddTransition(newNode);
                        }
                        cNode = newNode;
                    }
                    cNode.AddResult(keyword);
                }
            }

            var nodesQueue = new Queue<TreeNode>();
            foreach(TreeNode cNode in root.Transition.Values)
            {
                cNode.Failure = root;

                QueueAddRange(nodesQueue, cNode.Transition.Values);
            }

            {
                TreeNode cNode = null;
                TreeNode r = null;
                TreeNode nNode = null;

                while(nodesQueue.Count!=0)
                {
                    cNode = nodesQueue.Dequeue();
                    r = cNode.Parent.Failure;
                   

                    while (r != null && ((nNode = r.GetTransition(cNode.Ch)) == null))
                        r = r.Failure;

                    if(r==null)
                    {
                        cNode.Failure = root;
                    }
                    else
                    {
                        cNode.Failure = nNode;
                        cNode.AddResults(cNode.Failure.Outputs);
                    }
                    QueueAddRange(nodesQueue, cNode.Transition.Values);
                }

            }
            root.Failure = root;
            return root;
        }


        [System.Diagnostics.DebuggerHidden]
        private void CheckKeywords(params string[] keywords)
        {
            if (keywords == null)
                throw new ArgumentException("keywords");
            if (keywords.Length == 0)
                throw new ArgumentException("keywords");

            foreach(string keyword in keywords)
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    throw new ArgumentException("thie keywords set cannot contain null references or empty strings.");
            }
        }

        private static void QueueAddRange(Queue<TreeNode> queue, IEnumerable<TreeNode> collection)
        {
            if (queue == null || collection == null)
                return;

            foreach (var item in collection)
            {
                queue.Enqueue(item);
            }
        }

        public string Replace(string text,string newWord)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var result = SearchAll(text,0,int.MaxValue);
            if (result == null || result.Length <= 0)
                return text;

            if (newWord == null)
                newWord = "";

            int startIndex = 0;
            int endIndex = 0;
            var resContent = new StringBuilder();
            foreach(var v in result)
            {
                endIndex = v.Index;
                if(endIndex>startIndex)
                {
                    resContent.Append(text.Substring(startIndex, endIndex - startIndex));
                }

                resContent.Append(newWord);
                startIndex = v.Index + v.Match.Length;
            }

            var rcLen = text.Length;
            if(startIndex<rcLen)
            {
                endIndex = rcLen;
                resContent.Append(text.Substring(startIndex, endIndex - startIndex));
            }
            return resContent.ToString();
        }

        public SearchResult[] SearchAll(string text,int start,int count)
        {
            CheckArguments(text, start, int.MaxValue);

            List<SearchResult> results = null;
            if(count==int.MaxValue)
            {
                results = new List<SearchResult>();
            }
            else
            {
                results = new List<SearchResult>(count);
            }

            foreach(SearchResult result in SearchIterator(text,start))
            {
                results.Add(result);
                if (results.Count == count)
                    break;
            }
            return results.ToArray();
        }

        public SearchResult SearchFirst(string text,int start)
        {
            CheckArguments(text, start, int.MaxValue);

            IEnumerator<SearchResult> iter = SearchIterator(text, start).GetEnumerator();
            if(iter.MoveNext())
            {
                return iter.Current;
            }

            return SearchResult.Empty;
        }

        protected IEnumerable<SearchResult> SearchIterator(string text,int start)
        {
            var root = treeRoot;
            if (root == null)
                throw new ArgumentException("root", "need search.Build()");

            var ptr = root;
            int index = 0;
            if (start > 0)
                text = text.Substring(start);

            while(index<text.Length)
            {
                TreeNode trans = null;
                while(trans==null)
                {
                    trans = ptr.GetTransition(text[index]);
                    if (ptr == root)
                        break;

                    if (trans == null)
                        ptr = ptr.Failure;
                }

                if (trans != null)
                    ptr = trans;

                if(ptr.Outputs!=null)
                {
                    foreach(string found in ptr.Outputs)
                    {
                        yield return new SearchResult(index - found.Length + 1, found);
                    }
                }
                index++;
            }
        }
        
        [System.Diagnostics.DebuggerHidden]
        protected void  CheckArguments(string text,int start,int count)
        {
            if (text == null||text.Length==0)
                throw new ArgumentException("text");

            if (start < 0)
                throw new ArgumentException("start");

            if (start >= text.Length)
                throw new ArgumentException("start");

            if (count <= 0)
                throw new ArgumentException("count");
            
        }
    }

    public class TreeNode
    {
        public TreeNode Parent { set; get; }
        public TreeNode Failure { set; get; }
        public char Ch { set; get; }

        public HashSet<string> Outputs { set; get; }
        public Dictionary<char, TreeNode> Transition { private set; get; }

        public TreeNode(TreeNode _parent,char c)
        {
            Ch = c;
            Parent = _parent;

            Transition = new Dictionary<char, TreeNode>();
        }

        
        public void AddResult(string result)
        {
            if (string.IsNullOrWhiteSpace(result))
                return;
            if (Outputs == null)
                Outputs = new HashSet<string>();

            Outputs.Add(result);
        }

        public void AddResults(IEnumerable<string> results)
        {
            if (results == null)
                return;

            if (Outputs == null)
                Outputs = new HashSet<string>();

            foreach(var result in results)
            {
                Outputs.Add(result);
            }
        }

        public void AddTransition(TreeNode node)
        {
            Transition.Add(node.Ch, node);
        }

        public TreeNode GetTransition(char c)
        {
            TreeNode node = null;
            if(Transition.TryGetValue(c,out node))
            {
                return node;
            }
            return null;
        }
        
        
    }

    public struct SearchResult : IEquatable<SearchResult>
    {
        public static readonly SearchResult Empty = new SearchResult(-1, null);

        internal SearchResult(int index,string match)
            :this()
        {
            Index = index;
            Match = match;
        }

        public int Index { get; internal set; }
        public int Length { get; internal set; }

        public string Match { get; internal set; }

        public override string ToString()
        {
            return $"[SearchResult Index:{Index},Length:{Length},Match:{Match}]";
        }

        public override bool Equals(object obj)
        {
            if(obj==null)
            {
                return false;
            }

            SearchResult other = (SearchResult)obj;
            return Index == other.Index && Match == other.Match;
        }

        public bool Equals(SearchResult other)
        {
            return Index == other.Index && Match == other.Match;
        }

        public override int GetHashCode()
        {
            return Index.GetHashCode() ^ Match.GetHashCode();
        }

        public static bool operator ==(SearchResult sr1,SearchResult sr2)
        {
            return sr1.Index == sr2.Index && sr1.Match == sr2.Match;
        }

        public static bool operator !=(SearchResult sr1,SearchResult sr2)
        {
            return !(sr1.Index == sr2.Index && sr1.Match == sr2.Match);
        }
    }
}
