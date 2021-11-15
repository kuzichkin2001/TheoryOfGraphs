using System;

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

            Graph<string> graph = CreateGraph(isWeighted, isDirected);
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
                    "0. End Testing\n"
                );

                int answer = Convert.ToInt32(Console.ReadLine());
                string defaultFilePath = @"C:\Users\kuzic\TheoryOfGraphs\GraphTheory\";
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
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Oops! Incorrect answer. Try again.");
                        break;
                }
            }
        }

        public static Graph<string> CreateGraph(bool isWeighted, bool isDirected)
        {
            Graph<string> graph;
            string defaultFilePath = @"C:\Users\kuzic\TheoryOfGraphs\GraphTheory\";
            if (isWeighted)
            {
                if (isDirected)
                {
                    graph = new Graph<string>($"{defaultFilePath}DirectedWeighted.txt", isWeighted, isDirected);
                }
                else
                {
                    graph = new Graph<string>($"{defaultFilePath}notDirectedWeighted.txt", isWeighted, isDirected);
                }
            }
            else
            {
                if (isDirected)
                {
                    graph = new Graph<string>($"{defaultFilePath}DirectedNotWeighted.txt", isWeighted, isDirected);
                }
                else
                {
                    graph = new Graph<string>($"{defaultFilePath}notDirectedNotWeighted.txt", isWeighted, isDirected);
                }
            }

            return graph;
        }

        private static void AddNode(Graph<string> graph)
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

        private static void AddEdge(Graph<string> graph)
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

        private static void RemoveNode(Graph<string> graph)
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

        private static void RemoveEdge(Graph<string> graph)
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

        public static void PrintNodesWithLesserIncomingPower(Graph<string> graph)
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

        public static void PrintAllIncomingNodes(Graph<string> graph)
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

        public static void DeleteAllOddNodes(Graph<string> graph)
        {
            Graph<string> newGraph = graph.DeleteAllOddNodes();
            Console.WriteLine(newGraph);
        }

        public static void IsGraphAcycled(Graph<string> graph)
        {
            graph.IsGraphAcycled();
        }

    }
}