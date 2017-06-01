using System;
using System.Collections.Generic;
using ASD.Graphs;

namespace Tarjan
{
	public static partial class Tarjans_strongly_connected_components_algorithm_Extender
	{
		internal struct vertiveNodeList
		{
			public int next;		//index in array of next element
			public int prev;		//index in array of previous element
			public int lowNr;		//the smallest index of any node known to be reachable from v, including v itself
			public int dfsIndex;	//depth first search first visit index, not index in the array
			public int scc;			//nr of the strongly connected component vertice is in
		}

		/// <summary>
		/// Repositions an element in the list
		/// </summary>
		/// <param name="array">List of nodes implemented in an array</param>
		/// <param name="elemNr">Index of the element to be placed after the current one</param>
		/// <param name="currentNr">Index of the current element</param>
		internal static void repositionElement(vertiveNodeList[] array, int elemNr, int currentNr)
		{
			if (array[elemNr].next != -1)	
			{
				//element was already on the list, the list needs to be "glued" after it is deleted
				array[array[elemNr].next].prev = array[elemNr].prev;
				array[array[elemNr].prev].next = array[elemNr].next;
			}
			array[elemNr].next = array[currentNr].next;
			//array[array[currentNr].next].prev = elemNr;	//not necessary for the method to work
			array[currentNr].next = elemNr;
			array[elemNr].prev = currentNr;
		}

		/// <summary>
		/// Deletes all of the elements between indexes from and to including the "from" one
		/// </summary>
		internal static void deleteSection(vertiveNodeList[] array, int from, int to)
		{
			if (to != -1) array[to].prev = array[from].prev;
			if (array[from].prev != -1) array[array[from].prev].next = to;
		}
		/// <summary>
		/// Uses doubly linked list instead of a normal stack to find the strongly connected components
		/// </summary>
		/// <param name="g">Analysed graph</param>
		/// <param name="scc">Numbers of the strongly connected component the vertice is part of</param>
		/// <returns>returns the strongly connected component count</returns>
		public static int Iterative_with_doubly_linked_list(this Graph g, out int[] scc)
		{
			int sccNr = 0;				//number of already used sccs
			int n = g.VerticesCount;	
			int curInd = 0;             //depth first search pre visit index
			int currentVertice = -1;

			vertiveNodeList[] listArray = new vertiveNodeList[n];
			for (int i = 0; i < n; i++)
				listArray[i].prev = listArray[i].next = listArray[i].scc = listArray[i].dfsIndex = -1;

			for (int i = 0; i < n; ++i)
			{
				if (listArray[i].scc != -1) continue;	//already visited vertice
				currentVertice = i;
				while (currentVertice != -1)			//the list can be treated as a stack so it is pretty much: 
														//"while stack isn't empty"
				{
					if (listArray[currentVertice].dfsIndex == -1)	
					{
						//visiting the vertice for the first time

						listArray[currentVertice].dfsIndex = listArray[currentVertice].lowNr = curInd++;
						foreach (Edge e in g.OutEdges(currentVertice))
							if (listArray[e.To].dfsIndex == -1)
							{
								//adds or repositions elements so that all the unvisited successors of current are after it on the list
								repositionElement(listArray, e.To, currentVertice);
								currentVertice = e.To;
							}
					}
					else
					{
						//visiting the vertice for the second time

						//setting the lowNr in the current element to minimum from its succesors in the component and itself
						foreach (Edge e in g.OutEdges(currentVertice))
							if (listArray[e.To].dfsIndex != -1 && listArray[e.To].scc == -1 && listArray[currentVertice].lowNr > listArray[e.To].lowNr)
								listArray[currentVertice].lowNr = listArray[e.To].lowNr;

						if (listArray[currentVertice].lowNr == listArray[currentVertice].dfsIndex)
						{
							//time to delete current component from list
							int tmpIndex = currentVertice;
							do
							{
								//attach the sccnr to all elements of the component and find the components end
								listArray[tmpIndex].scc = sccNr;	
								tmpIndex = listArray[tmpIndex].next;
							}
							while (tmpIndex != -1 && listArray[tmpIndex].dfsIndex >= listArray[currentVertice].dfsIndex);
							//tmpIndex==-1 means we have reached the end of the list, and 
							//listArray[tmpIndex].index < listArray[currentVertice].index means we've reached the next scc

							deleteSection(listArray, currentVertice, tmpIndex);//delete component from the list
							sccNr++;
						}
						currentVertice = listArray[currentVertice].prev;//go back on the list
						continue;
					}
				}
			}

			scc = new int[n];
			for (int i = 0; i < n; i++)
				scc[i] = listArray[i].scc;	//output the scc numbers

			return sccNr;
		}
	}
}
