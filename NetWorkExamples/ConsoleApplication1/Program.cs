using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        private const string Start = "start";
        private const string A = "a";
        private const string B = "b";
        private const string Finish = "finish";
        private const string None = "none";

        static void Main(string[] args)
        {
            var graph = new Dictionary<string, Dictionary<string, int>>();
            graph[Start] = new Dictionary<string, int>();
            graph[Start][A] = 6;
            graph[Start][B] = 2;
            graph[A] = new Dictionary<string, int>();
            graph[A][Finish] = 1;
            graph[B] = new Dictionary<string, int>();
            graph[B][A] = 3;
            graph[B][Finish] = 5;
            graph[Finish] = new Dictionary<string, int>();

            var costs = new Dictionary<string, int>();
            costs[A] = 6;
            costs[B] = 2;
            costs[Finish] = int.MaxValue;

            var parents = new Dictionary<string, string>();
            parents[A] = Start;
            parents[B] = Start;
            parents[Finish] = None;

            var processed = new List<string>(graph.Count);

            //Алгоритм Дейкстры
            var node = FindLowestCostNode(costs, processed);
            while (node != None)
            {
                var cost = costs[node];
                var neighbors = graph[node];
                foreach (var key in neighbors.Keys)
                {
                    var new_cost = cost + neighbors[key];
                    if (costs[key] > new_cost)
                    {
                        costs[key] = new_cost;
                        parents[key] = node;
                    }
                }

                processed.Add(node);
                node = FindLowestCostNode(costs, processed);
            }
        }

        private static string FindLowestCostNode(Dictionary<string, int> costs, List<string> processed)
        {
            var lowest_cost = int.MaxValue;
            var lowest_cost_node = None;

            foreach (var key in costs.Keys)
            {
                var cost = costs[key];
                if (cost < lowest_cost && !processed.Contains(key))
                {
                    lowest_cost = cost;
                    lowest_cost_node = key;
                }
            }

            return lowest_cost_node;
        }
    }
}
