﻿using System;

namespace Graph
{
    class Program
    {
        static void Main(string[] args)
        {
            // string filepath = @"C:\Users\User\RiderProjects\TheoryOfGraphs\GraphTheory\input.txt";
            // Graph<int> intGraph = new Graph<int>(filepath);
            //
            // intGraph.RemoveNode(3);
            // intGraph.AddNode(5);
            // intGraph.AddEdge(5, 1, 10);
            //
            // Graph<string> stringGraph = new Graph<string>();
            //
            // stringGraph.AddNode("Saratov");
            // stringGraph.AddNode("Moscow");
            // stringGraph.AddNode("Samara");
            //
            // stringGraph.AddEdge("Saratov", "Moscow", 850);
            // stringGraph.AddEdge("Saratov", "Samara", 370);
            // stringGraph.AddEdge("Moscow", "Samara", 560);
            //
            //
            // string outputFilepath = @"C:\Users\User\RiderProjects\TheoryOfGraphs\GraphTheory\output.txt";
            // stringGraph.SaveGraph(outputFilepath);
            
            GraphUi.Start();
        }
    }
}