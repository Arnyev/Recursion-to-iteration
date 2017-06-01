using System;
using System.Collections.Generic;
using ASD.Graphs;

namespace Tarjan
{
	public static partial class Tarjans_strongly_connected_components_algorithm_Extender
	{
		internal struct verticeNodeRec
		{
			public int lowNr;       //the smallest index of any node known to be reachable from v, including v itself
			public int dfsIndex;    //depth first search first visit index, not index in the array
			public int scc;         //nr of the strongly connected component vertice is in
		}
		public static int Recursive(this Graph g, out int[] scc)
		{
			int n = g.VerticesCount;
			verticeNodeRec[] array = new verticeNodeRec[n];
			for (int i = 0; i < n; ++i)
				array[i].dfsIndex = -1;

			Stack<int> s = new Stack<int>();
			bool[] onStack = new bool[n];
			int dfsIndex = 0;
			int sccNr = 0;
			for (int i = 0; i < n; ++i)
				if (array[i].dfsIndex == -1)
					recursiveHelper(g, array, i, ref dfsIndex, ref sccNr, s, onStack);

			scc = new int[n];
			for (int i = 0; i < n; i++)
				scc[i] = array[i].scc;  //output the scc numbers

			return sccNr;
		}

		internal static void recursiveHelper(Graph g, verticeNodeRec[] array, int v, ref int dfsIndex, ref int sccNr, Stack<int> s, bool[] onStack)
		{
			int tmp = -1;
			array[v].lowNr = array[v].dfsIndex = dfsIndex++;
			s.Push(v);
			onStack[v] = true;
			foreach (Edge e in g.OutEdges(v))
			{
				if (array[e.To].dfsIndex == -1)
					recursiveHelper(g, array, e.To, ref dfsIndex, ref sccNr, s, onStack);

				if (onStack[e.To] && array[e.To].lowNr < array[v].lowNr)
					array[v].lowNr = array[e.To].lowNr;
			}

			if (array[v].lowNr == array[v].dfsIndex)
			{
				do
				{
					tmp = s.Pop();
					onStack[tmp] = false;
					array[tmp].scc = sccNr;
				}
				while (tmp != v);
				++sccNr;
			}
		}
	}
}