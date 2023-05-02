using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleLang
{
    internal class Graph
    {
        public int VertexSize = 0;
        public List<Tuple<int, int>>  Edges = new List<Tuple<int, int>>();
        public Graph(int n)
        {
            VertexSize = n;
        }
        public void AddEdge(int x, int y)
        {
            Edges.Add(Tuple.Create(x, y));
        }
        public void PrintEdges()
        {
            foreach (var edge in Edges)
            {
                Console.WriteLine(edge.Item1+ " " + edge.Item2);
            }
        }
        public SortedSet<int> P(int v)
        {
           var s = new SortedSet<int>();
            foreach (var edge in Edges)
            {
                if (edge.Item2 == v && edge.Item1 != edge.Item2)
                {
                    s.Add(edge.Item1);
                }
            }
            return s;
        }
        public SortedSet<int> S(int v)
        {
            var s = new SortedSet<int>();
            foreach (var edge in Edges)
            {
                if (edge.Item1 == v && edge.Item1 != edge.Item2)
                {
                    s.Add(edge.Item2);
                }
            }
            return s;
        }
    }
}
