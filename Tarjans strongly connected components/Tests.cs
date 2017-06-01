using System;
using System.Collections.Generic;
using ASD.Graphs;
using System.Diagnostics;

namespace Tarjan
{
	class Tests
	{
		public static void Main()
		{
			var rgg = new RandomGraphGenerator();
			int[] recSCC, stackSCC, listSCC;
			int verticeCount;
			double density;
			var rand = new Random();
			int maxVertice = 4000;
			long stackTime = 0, listTime = 0, recursiveTime = 0;
			long stackTotalTime = 0, listTotalTime = 0, recursiveTotalTime = 0;

			double densityRatio = 0.2;
			int testCount = 10;

			int stackResult = -1, listResult = -1, recursiveResult = -1;

			Stopwatch sw = new Stopwatch();
			sw.Start();
			for (int i = 0; i < testCount; i++)
			{
				verticeCount = rand.Next() % maxVertice;
				density = rand.NextDouble() * densityRatio;
				Graph g = rgg.DirectedGraph(typeof(AdjacencyListsGraph<SimpleAdjacencyList>), verticeCount, density);

				long start = sw.ElapsedMilliseconds;

				stackResult = g.Iterative_with_two_stacks(out stackSCC);
				stackTime = sw.ElapsedMilliseconds - start;
				stackTotalTime += stackTime;
				start = sw.ElapsedMilliseconds;


				recursiveResult = g.Recursive(out recSCC);
				recursiveTime = sw.ElapsedMilliseconds - start;
				recursiveTotalTime += recursiveTime;
				start = sw.ElapsedMilliseconds;

				listResult = g.Iterative_with_doubly_linked_list(out listSCC);
				listTime = sw.ElapsedMilliseconds - start;
				listTotalTime += listTime;
				start = sw.ElapsedMilliseconds;

				Console.WriteLine("Recursive time: {0}\t\tList time: {1}\t\t Stack time: {2}", recursiveTime, listTime, stackTime);
				Console.WriteLine();
				Console.WriteLine("Recursive result: {0}\t\tList Result: {1}\t\t Stack Result: {2}", recursiveResult, listResult, stackResult);
			}
			Console.WriteLine();
			Console.WriteLine("Recursive total time: {0}\t\tList total time: {1}\t\t Stack total time: {2}", recursiveTotalTime, listTotalTime, stackTotalTime);

		}
	}
}
