using System;
using System.Collections.Generic;
using ASD.Graphs;

namespace Tarjan
{
	public static partial class Tarjans_strongly_connected_components_algorithm_Extender
	{
		internal struct verticeNodeStack
		{
			public int lowNr;       //the smallest index of any node known to be reachable from v, including v itself
			public int dfsIndex;    //depth first search first visit index, not index in the array
			public int scc;         //nr of the strongly connected component vertice is in
			public bool deleted;
		}
		public static int Iterative_with_two_stacks(this Graph g, out int[] scc)
		{
			int k;
			int n = g.VerticesCount;
			int curInd = 0;
			int sccnr = 0;

			Stack<int> componentsStack = new Stack<int>();
			Stack<int> searchStack = new Stack<int>();

			verticeNodeStack[] array = new verticeNodeStack[n];
			for (int i = 0; i < n; ++i)
				array[i].dfsIndex = array[i].scc = -1;

			for (int i = 0; i < n; ++i)
			{
				if (array[i].deleted) continue;
				searchStack.Push(i);
				while (searchStack.Count!=0)
				{
					int ver = searchStack.Peek();
					if (array[ver].deleted) 
					{
						searchStack.Pop();
						continue;
					}
					if (array[ver].dfsIndex == -1)
					{
						componentsStack.Push(ver);
						array[ver].dfsIndex = array[ver].lowNr = curInd++;
						foreach (Edge e in g.OutEdges(ver))
							if (array[e.To].dfsIndex == -1)
								searchStack.Push(e.To);
					}
					else
					{
						foreach (Edge e in g.OutEdges(ver))
							if (array[e.To].scc == -1 && array[e.To].lowNr < array[ver].lowNr)
								array[ver].lowNr = array[e.To].lowNr;

						if (array[ver].lowNr == array[ver].dfsIndex)
						{
							do
							{
								k = componentsStack.Pop();
								array[k].scc = sccnr;
								array[k].deleted = true;
							}
							while (k != ver);
							sccnr++;
						}
						searchStack.Pop();
						array[ver].deleted = true;
						continue;
					}
				}
			}
			scc = new int[n];
			for (int i = 0; i < n; i++)
				scc[i] = array[i].scc;  //output the scc numbers

			return sccnr;
		}
	}
}
