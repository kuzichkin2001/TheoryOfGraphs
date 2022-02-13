using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Graph
{
    public static class GraphUi
    {
        
        public static void Start()
        {
            Console.WriteLine("Hello, fellow! You are entering the Graph Interactive Console Application");
            Console.WriteLine("Is it Directed?(y/n)");
            bool isDirected, isWeighted;

            while (true)
            {
                string answer = Console.ReadLine();
                if (answer.Equals("y"))
                {
                    isDirected = true;
                    break;
                }
                else if (answer.Equals(("n")))
                {
                    isDirected = false;
                    break;
                }
                else
                {
                    Console.WriteLine("Oops! Incorrect answer. Try again.");
                    answer = null;
                }
            }
            
            Console.WriteLine("Is it Weighted?(y/n)");
            
            while (true)
            {
                string answer = Console.ReadLine();
                            
                if (answer.Equals("y"))
                {
                    isWeighted = true;
                    break;
                }
                else if (answer.Equals("n"))
                {
                    isWeighted = false;
                    break;
                }
                else {
                    Console.WriteLine("Oops! Incorrect answer. Try again.");
                    answer = null;
                }
            }

            Graph graph = CreateGraph(isWeighted, isDirected);
            while (true)
            {
                Console.WriteLine("You have a few options:");
                Console.WriteLine(
                    "1. Add vertex\n" +
                    "2. Add edge\n" +
                    "3. Remove vertex\n" +
                    "4. Remove edge\n" +
                    "5. Save Graph to file\n" +
                    "6. Load Graph from file\n" +
                    "7. Print Graph\n" +
                    "8. Print all nodes with lesser incoming power\n" +
                    "9. Print all incoming nodes\n" +
                    "10. Delete all nodes with odd incoming/outgoing power\n" +
                    "11. Check if graph is acycled\n" +
                    "12. Find the graph center\n" +
                    "13. Find minimum ostov tree (Kraskal)\n" +
                    "14. N-perifery of graph (Dijkstra)\n" +
                    "15. All shortest paths in pairs (Floyd)\n" +
                    "16. All shortest paths from vertex U (Ford-Bellman)\n" +
                    "17. Find Maximum flow of oriented weighted graph\n" +
                    "0. End Testing\n"
                );

                int answer = Convert.ToInt32(Console.ReadLine());
                string defaultFilePath = @"C:\Users\Павел\Desktop\Git repositories\TheoryOfGraphs\GraphTheory\";
                switch (answer)
                {
                    case 1:
                        AddNode(graph);
                        break;
                    case 2:
                        AddEdge(graph);
                        break;
                    case 3:
                        RemoveNode(graph);
                        break;
                    case 4:
                        RemoveEdge(graph);
                        break;
                    case 5:
                        if (graph.IsDirectional)
                        {
                            if (graph.IsWeighted)
                            {
                                graph.SaveGraph($"{defaultFilePath}DirectedWeighted.txt");
                            }
                            else
                            {
                                graph.SaveGraph($"{defaultFilePath}DirectedNotWeighted.txt");
                            }
                        }
                        else
                        {
                            if (graph.IsWeighted)
                            {
                                graph.SaveGraph($"{defaultFilePath}notDirectedWeighted.txt");
                            }
                            else
                            {
                                graph.SaveGraph($"{defaultFilePath}notDirectedNotWeighted.txt");
                            }
                        }
                        graph.SaveGraph($"{defaultFilePath}output.txt");
                        break;
                    case 6:
                        graph = CreateGraph(graph.IsWeighted, graph.IsDirectional);
                        break;
                    case 7:
                        Console.WriteLine(graph);
                        break;
                    case 8:
                        PrintNodesWithLesserIncomingPower(graph);
                        break;
                    case 9:
                        PrintAllIncomingNodes(graph);
                        break;
                    case 10:
                        DeleteAllOddNodes(graph);
                        break;
                    case 11:
                        IsGraphAcycled(graph);
                        break;
                    case 12:
                        Center(graph);
                        break;
                    case 13:
                        Kraskal(graph);
                        break;
                    case 14:
                        Dijkstra(graph);
                        break;
                    case 15:
                        Floyd(graph);
                        break;
                    case 16:
                        FordBellman(graph);
                        break;
                    case 17:
                        FordFulkerson(graph);
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Oops! Incorrect answer. Try again.");
                        break;
                }
            }
        }

        public static Graph CreateGraph(bool isWeighted, bool isDirected)
        {
            Graph graph;
            string defaultFilePath = @"C:\Users\Павел\Desktop\Git repositories\TheoryOfGraphs\GraphTheory\";
            if (isWeighted)
            {
                if (isDirected)
                {
                    graph = new Graph($"{defaultFilePath}DirectedWeighted.txt", isWeighted, isDirected);
                }
                else
                {
                    graph = new Graph($"{defaultFilePath}notDirectedWeighted.txt", isWeighted, isDirected);
                }
            }
            else
            {
                if (isDirected)
                {
                    graph = new Graph($"{defaultFilePath}DirectedNotWeighted.txt", isWeighted, isDirected);
                }
                else
                {
                    graph = new Graph($"{defaultFilePath}notDirectedNotWeighted.txt", isWeighted, isDirected);
                }
            }

            return graph;
        }

        private static void AddNode(Graph graph)
        {
            Console.Write("Enter the name of your vertex: ");
            string keyToAdd;
            while (true)
            {
                keyToAdd = Console.ReadLine();
                if (!graph[keyToAdd])
                {
                    graph.AddNode(keyToAdd);
                    break;
                }
                else
                {
                    Console.WriteLine($"There's already a vertex with key {keyToAdd}");
                }
            }
        }

        private static void AddEdge(Graph graph)
        {
            Console.Write("Enter the name of first vertex: ");
            string keyFrom;
            while (true)
            {
                keyFrom = Console.ReadLine();
                if (!graph[keyFrom])
                {
                    Console.WriteLine("Oops! Incorrect answer. Try again.");
                }
                else
                {
                    break;
                }
            }
            
            Console.Write("Enter the name of second vertex: ");
            string keyTo;
            while (true)
            {
                keyTo = Console.ReadLine();
                if (!graph[keyTo])
                {
                    Console.WriteLine("Oops! Incorrect answer. Try again.");
                }
                else
                {
                    break;
                }
            }

            if (graph.IsWeighted)
            {
                Console.Write("Enter the weight of an edge: ");
                int weight = Convert.ToInt32(Console.ReadLine());
                
                graph.AddEdge(keyFrom, keyTo, weight);
            }
            else
            {
                graph.AddEdge(keyFrom, keyTo);
            }
        }

        private static void RemoveNode(Graph graph)
        {
            Console.Write("Enter the of a vertex to remove: ");
            string keyToRemove;
            while (true)
            {
                keyToRemove = Console.ReadLine();
                if (!graph[keyToRemove])
                {
                    Console.WriteLine($"There's no vertex with key {keyToRemove}. Try again.");
                }
                else
                {
                    break;
                }
            }
            
            graph.RemoveNode(keyToRemove);
        }

        private static void RemoveEdge(Graph graph)
        {
            Console.Write("Enter the first vertex: ");
            string keyFrom;
            while (true)
            {
                keyFrom = Console.ReadLine();
                if (!graph[keyFrom])
                {
                    Console.WriteLine("Oops! Incorrect answer. Try again.");
                }
                else
                {
                    break;
                }
            }
            
            Console.Write("Enter the second vertex: ");
            string keyTo;
            while (true)
            {
                keyTo = Console.ReadLine();
                if (!graph[keyTo])
                {
                    Console.WriteLine("Oops! Incorrect answer. Try again.");
                }
                else
                {
                    break;
                }
            }
            
            graph.RemoveEdge(keyFrom, keyTo);
        }

        public static void PrintNodesWithLesserIncomingPower(Graph graph)
        {
            Console.WriteLine("Enter the vertex: ");
            string key;

            while (true)
            {
                key = Console.ReadLine();
                if (!graph[key])
                {
                    Console.WriteLine("Entered incorrect vertex. Try again.");
                }
                else
                {
                    break;
                }
            }
            
            graph.PrintNodesWithLesserIncomingPower(key);
        }

        public static void PrintAllIncomingNodes(Graph graph)
        {
            Console.WriteLine("Enter the vertex: ");
            string key;

            while (true)
            {
                key = Console.ReadLine();
                if (!graph[key])
                {
                    Console.WriteLine("Entered incorrect vertex. Try again.");
                }
                else
                {
                    break;
                }
            }
            
            graph.PrintAllIncomingNodes(key);
        }

        public static void DeleteAllOddNodes(Graph graph)
        {
            Graph newGraph = graph.DeleteAllOddNodes();
            Console.WriteLine(newGraph);
        }

        public static void IsGraphAcycled(Graph graph)
        {
            Console.WriteLine("Enter the vertex: ");
            string key;

            while (true)
            {
                key = Console.ReadLine();
                if (!graph[key])
                {
                    Console.WriteLine("Entered incorrect vertex. Try again.");
                }
                else
                {
                    break;
                }
            }

            if (graph.IsGraphAcycled(key))
            {
                Console.WriteLine("Acycled");
            }
            else
            {
                Console.WriteLine("Cycled");
            }
        }

        public static void Center(Graph graph)
        {
            graph.Center();
        }

        public static void Kraskal(Graph graph)
        {
            Graph newGraph = graph.MinOstovTree();
            newGraph.ClearVisited();
            List<string> comp = new List<string>();
            foreach (var vertex in newGraph.AdjacencyList.Keys)
            {
                comp.Clear();
                if (!newGraph.Dfs(vertex, ref comp))
                {
                    if (comp.Count != 1)
                    {
                        Console.WriteLine("Component:");
                        foreach (var item in comp)
                        {
                            Console.Write($"{item} ");
                        }

                        Console.WriteLine();   
                    }
                };
            }
            
            Console.WriteLine("Min ostov tree:");
            
            Console.WriteLine(newGraph);
        }

        public static void Dijkstra(Graph graph)
        {
            Console.WriteLine("Enter the vertex: ");
            string key;

            while (true)
            {
                key = Console.ReadLine();
                if (!graph[key])
                {
                    Console.WriteLine("Entered incorrect vertex. Try again.");
                }
                else
                {
                    break;
                }
            }
            
            Console.WriteLine("Enter the N: ");
            int N;

            while (true)
            {
                N = Convert.ToInt32(Console.ReadLine());
                if (N <= 0)
                {
                    Console.WriteLine("Entered incorrect N. Try again.");
                }
                else
                {
                    break;
                }
            }

            Dictionary<string, long> dists = graph.Dijkstra(key);

            Console.WriteLine();
            foreach (var item in dists)
            {
                Console.WriteLine($"{item.Key} ---- {item.Value}");
            }

            Console.WriteLine();

            Console.WriteLine($"{N}-perifery of vertex {key} is:\n");

            foreach (var item in dists)
            {
                if (item.Value > N)
                {
                    Console.Write($"{item.Key} ");
                }
            }

            Console.WriteLine();
        }

        public static void Floyd(Graph graph)
        {
            Dictionary<string, Dictionary<string, long>> dists =
                graph.Floyd();

            foreach (var dict in dists)
            {
                Console.WriteLine($"Shortest paths to {dict.Key.ToString()}:\n");

                foreach (var item in dict.Value)
                {
                    if (item.Value != Int32.MaxValue && item.Value != 0)
                    {
                        Console.WriteLine($"Vertex {item.Key.ToString()} : Weight {item.Value}");
                    }
                }

                Console.WriteLine("\n");
            }
        }

        public static void FordBellman(Graph graph)
        {
            Console.WriteLine("Enter the vertex: ");
            string key;

            while (true)
            {
                key = Console.ReadLine();
                if (!graph[key])
                {
                    Console.WriteLine("Entered incorrect vertex. Try again.");
                }
                else
                {
                    break;
                }
            }

            Dictionary<string, long> dists = graph.FordBellman(key);

            Console.WriteLine($"Shortest paths from {key} are:\n");

            foreach (var edge in dists)
            {
                if (edge.Value != Int32.MaxValue && edge.Value != 0)
                {
                    Console.WriteLine($"To vertex {edge.Key} length is {edge.Value}");
                }
            }

            Console.WriteLine();
        }

        public static void FordFulkerson(Graph graph)
        {
            int result = graph.FordFulkerson();

            Console.WriteLine($"Maximum flow: {result}");
        }
    }
}