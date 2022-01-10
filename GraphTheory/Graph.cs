using System;
using System.IO;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using Microsoft.VisualBasic;

namespace Graph
{
    public class Graph
    {
        
        private Dictionary<string, Dictionary<string, int>> _adjacencyList;
        private bool _isWeighted;
        private bool _isDirectional;
        private Dictionary<string, bool> _visited;
        private List<string> _allNodes;
        
        // Empty constructor instantiating an adjacency list
        public Graph(bool isWeighted = false, bool isDirectional = false)
        {
            this._adjacencyList = new Dictionary<string, Dictionary<string, int>>();
            this._isWeighted = isWeighted;
            this._isDirectional = isDirectional;

            this._visited = new Dictionary<string, bool>();
        }

        // Constructor that is reading a graph from file with absolute "filePath"
        public Graph(string filePath, bool isWeighted = false, bool isDirectional = false)
        {
            this._isWeighted = isWeighted;
            this._isDirectional = isDirectional;
            this._adjacencyList = new Dictionary<string, Dictionary<string, int>>();
            this._visited = new Dictionary<string, bool>();

            using (StreamReader fileReader = new StreamReader(filePath, Encoding.UTF8))
            {
                string line;
                while ((line = fileReader.ReadLine()) != null)
                {
                    string[] tempArr = line.Split();
                    string name = tempArr[0];
                    _visited[name] = false;
                    _adjacencyList[name] = new Dictionary<string, int>();
                    for (int i = 1; i < tempArr.Length; ++i)
                    {
                        if (_isWeighted)
                        {
                            string[] subTemp = tempArr[i].Split(':');
                            string subName = subTemp[0];
                            int weight = Convert.ToInt32(subTemp[1]);
                            _adjacencyList[name][subName] = weight;
                        }
                        else
                        {
                            string subName = tempArr[i];
                            _adjacencyList[name][subName] = 1;
                        }
                    }
                }
            }

        }

        public void ClearVisited()
        {
            foreach (var item in _adjacencyList.Keys)
            {
                this._visited[item] = false;
            }
        }

        private void InstantiateAllNodesList()
        {
            _allNodes = new List<string>();

            foreach (var dict in _adjacencyList)
            {
                _allNodes.Add(dict.Key);
            }
        }

        // Copy constructor
        public Graph(Graph graph)
        {
            this._isWeighted = graph.IsWeighted;
            this._isDirectional = graph.IsDirectional;
            this._adjacencyList = new Dictionary<string, Dictionary<string, int>>();
            this._visited = new Dictionary<string, bool>();
            foreach (KeyValuePair<string, Dictionary<string, int>> dict in graph.AdjacencyList)
            {
                _adjacencyList[dict.Key] = new Dictionary<string, int>();
                foreach (KeyValuePair<string, int> item in dict.Value)
                {
                    _adjacencyList[dict.Key][item.Key] = item.Value;
                }
            }
            
        }

        public bool this[string key]
        {
            get
            {
                return this._adjacencyList.ContainsKey(key);
            }
        }

        public int this[string from, string to]
        {
            get
            {
                if (_adjacencyList[from].ContainsKey(to))
                {
                    return _adjacencyList[from][to];
                }
                else
                {
                    return Int32.MaxValue;
                }
            }
            set
            {
                _adjacencyList[from][to] = value;
            }
        }

        // Property providing access to adjacency list of Graph
        public Dictionary<string, Dictionary<string, int>> AdjacencyList
        {
            get
            {
                return this._adjacencyList;
            }
        }

        public bool IsWeighted
        {
            get
            {
                return this._isWeighted;
            }
        }

        public bool IsDirectional
        {
            get
            {
                return this._isDirectional;
            }
        }

        // Method adding new vertex to Graph by key
        public void AddNode(string key)
        {
            _adjacencyList.Add(key, new Dictionary<string, int>());
        }
        
        // Method Adding new edge between "from" and "to" vertexes with weight "weight" in Graph
        public void AddEdge(string from, string to, int weight = 1)
        {
            if (this._isWeighted)
            {
                _adjacencyList[from].Add(to, weight);
                if (!this._isDirectional)
                {
                    _adjacencyList[to].Add(from, weight);
                }
            }
            else
            {
                _adjacencyList[from].Add(to, 0);
                if (!this._isDirectional)
                {
                    _adjacencyList[to].Add(from, 0);
                }
            }
        }

        // Method removing vertex from Graph by key
        public void RemoveNode(string key)
        {
            foreach (KeyValuePair<string, Dictionary<string, int>> dict in _adjacencyList)
            {
                foreach (KeyValuePair<string, int> item in dict.Value)
                {
                    if (item.Key.Equals(key))
                    {
                        dict.Value.Remove(key);
                    }
                }
            }
            
            _adjacencyList.Remove(key);
        }

        // Method removing edge between "from" and "to" vertexes in Graph
        public void RemoveEdge(string from, string to)
        {
            _adjacencyList[from].Remove(to);
            if (!_isDirectional)
            {
                _adjacencyList[to].Remove(from);
            }
        }

        public int GetOutgoingPower(string key)
        {
            Dictionary<string, int> currentNode = this._adjacencyList[key];

            return currentNode.Count;
        }

        public int GetIncomingPower(string key)
        {
            int count = 0;

            foreach (KeyValuePair<string, Dictionary<string, int>> dict in this._adjacencyList)
            {
                if (dict.Value.ContainsKey(key))
                {
                    count++;
                }
            }

            return count;
        }

        public void PrintNodesWithLesserIncomingPower(string key)
        {
            int currentIncomingPower = GetIncomingPower(key);

            foreach (KeyValuePair<string, Dictionary<string, int>> dict in this._adjacencyList)
            {
                if (GetIncomingPower(dict.Key) < currentIncomingPower)
                {
                    Console.Write("{0} ", dict.Key.ToString());
                }
            }

            Console.WriteLine();
        }

        public void PrintAllIncomingNodes(string key)
        {
            if (!this._isDirectional)
            {
                Console.WriteLine("Unable to do this with not directed graph.");
            }
            else
            {
                foreach (KeyValuePair<string, Dictionary<string, int>> dict in this._adjacencyList)
                {
                    if (dict.Value.ContainsKey(key))
                    {
                        Console.WriteLine(dict.Key.ToString());
                    }
                }
            }
        }

        public Graph DeleteAllOddNodes()
        {
            Graph graph = Copy();

            Dictionary<string, int> powers = new Dictionary<string, int>();

            foreach (var dict in _adjacencyList)
            {
                if (_isDirectional)
                {
                    powers.Add(dict.Key, GetIncomingPower(dict.Key) + GetOutgoingPower(dict.Key));
                }
                else
                {
                    powers.Add(dict.Key, GetIncomingPower(dict.Key));
                }
            }

            foreach (KeyValuePair<string, Dictionary<string, int>> dict in _adjacencyList)
            {
                int currentPower = powers[dict.Key];
                if (currentPower % 2 != 0)
                {
                    graph.RemoveNode(dict.Key);
                }
            }

            return graph;
        }

        public bool Dfs(string key, ref List<string> comp)
        {
            _visited[key] = true;
            comp.Add(key);
            foreach (var item in _adjacencyList[key])
            {
                string to = item.Key;
                if (!_visited[to] && Dfs(to, ref comp)) return true;
            }

            return false;
        }

        // public void IsGraphAcycled()
        // {
        //     Dictionary<string, int> colors = new Dictionary<string, int>();
        //     string cycleStart = "-1";
        //     
        //     foreach (var item in _adjacencyList)
        //     {
        //         colors.Add(item.Key, 0);
        //     }
        //
        //     foreach (var item in _adjacencyList)
        //     {
        //         if (Dfs(item.Key, ref colors, ref cycleStart))
        //         {
        //             break;
        //         }
        //     }
        //
        //     if (cycleStart == "-1")
        //     {
        //         Console.WriteLine("Acycled");
        //     }
        //     else
        //     {
        //         Console.WriteLine("Cycled");
        //     }
        // }
        
        public Dictionary<string, Dictionary<string, long>> ShortPaths()
        {
            ClearVisited();
            InstantiateAllNodesList();

            Dictionary<string, Dictionary<string, long>> dists = new Dictionary<string, Dictionary<string, long>>();

            foreach (var item in _allNodes)
            {
                dists.Add(item, new Dictionary<string, long>());
                foreach (var jtem in _allNodes)
                {
                    if (item.Equals(jtem))
                    {
                        dists[item].Add(jtem, 0);
                    }
                    else
                    {
                        dists[item].Add(jtem, this[item, jtem]);
                    }
                }
            }

            foreach (var ktem in _allNodes)
            {
                foreach (var item in _allNodes)
                {
                    foreach (var jtem in _allNodes)
                    {
                        long dist = dists[item][ktem] + dists[ktem][jtem];

                        if (dist < dists[item][jtem])
                        {
                            dists[item][jtem] = dist;
                        }
                    }
                }
            }

            return dists;
        }
        
        public void Center()
        {
            Dictionary<string, Dictionary<string, long>> dists = ShortPaths();
            Dictionary<string, long> maxes = new Dictionary<string, long>();
            foreach (var item in dists)
            {
                long max = -1;
                foreach (var elem in item.Value)
                {
                    if (elem.Value > max)
                    {
                        max = elem.Value;
                    }
                }
                maxes.Add(item.Key, max);
            }

            long min = long.MaxValue;
            List<string> center = new List<string>();
            foreach (var item in maxes)
            {
                if (item.Value < min)
                {
                    min = item.Value;
                }
            }

            foreach (var item in maxes)
            {
                if (item.Value == min)
                {
                    center.Add(item.Key);
                }
            }

            foreach (var item in center)
            {
                Console.Write($"{item} ");
            }

            Console.WriteLine();
        }

        public Dictionary<int, KeyValuePair<string, string>> IncidenceList()
        {
            Dictionary<int, KeyValuePair<string, string>> incidenceList = new Dictionary<int, KeyValuePair<string, string>>();

            foreach (KeyValuePair<string, Dictionary<string, int>> dict in this._adjacencyList)
            {
                foreach (KeyValuePair<string, int> item in dict.Value)
                {
                    if (!incidenceList.ContainsKey(item.Value))
                    {
                        incidenceList.Add(item.Value, new KeyValuePair<string, string>(dict.Key, item.Key));
                    }
                }
            }

            return incidenceList;
        }

        public Graph MinOstovTree()
        {
            Dictionary<int, KeyValuePair<string, string>> incidenceList = IncidenceList();
            var sortedIncidenceList = incidenceList.OrderBy(n => n.Key);

            HashSet<string> used = new HashSet<string>();
            Dictionary<int, KeyValuePair<string, string>> answer = new Dictionary<int, KeyValuePair<string, string>>();

            foreach (KeyValuePair<int, KeyValuePair<string, string>> item in sortedIncidenceList)
            {
                if (!used.Contains(item.Value.Key) || !used.Contains(item.Value.Value))
                {
                    answer.Add(item.Key, item.Value);
                    if (!used.Contains(item.Value.Key))
                    {
                        used.Add(item.Value.Key);
                    }

                    if (!used.Contains(item.Value.Value))
                    {
                        used.Add(item.Value.Value);
                    }
                }
            }

            Graph graph = new Graph(true, false);
            foreach (var item in answer)
            {
                if (!graph[item.Value.Key])
                {
                    graph.AddNode(item.Value.Key);
                }

                if (!graph[item.Value.Value])
                {
                    graph.AddNode(item.Value.Value);
                }
                
                graph.AddEdge(item.Value.Key, item.Value.Value, item.Key);
            }

            return graph;
        }

        public Dictionary<string, long> Dijkstra(string node)
        {
            ClearVisited();
            _visited[node] = true;
            var currentNode = _adjacencyList[node];
            
            InstantiateAllNodesList();
            foreach (var item in _allNodes)
            {
                if (!item.Equals(node))
                {
                    if (!currentNode.ContainsKey(item))
                    {
                        currentNode.Add(item, Int32.MaxValue);
                    }
                }
                else
                {
                    currentNode.Add(item, 0);
                }
            }

            Dictionary<string, long> dists = new Dictionary<string, long>();

            for (int i = 0; i < _allNodes.Count; ++i)
            {
                dists.Add(_allNodes[i], currentNode[_allNodes[i]]);
            }
            
            foreach (var element in _allNodes)
            {
                long min = Int32.MaxValue;
                string w = _allNodes[0];

                foreach (var nextElement in _allNodes)
                {
                    if (!_visited[nextElement] && dists[nextElement] < min)
                    {
                        min = dists[nextElement];
                        w = nextElement;
                    }
                }

                _visited[w] = true;

                foreach (var nextElement in _allNodes)
                {
                    long dist = dists[w] + this[w, nextElement];

                    if (!_visited[nextElement] && dist < dists[nextElement])
                    {
                        dists[nextElement] = dist;
                    }
                }
            }

            return dists;
        }

        public Dictionary<string, Dictionary<string, long>> Floyd()
        {
            ClearVisited();
            InstantiateAllNodesList();

            Dictionary<string, Dictionary<string, long>> dists = new Dictionary<string, Dictionary<string, long>>();

            foreach (var item in _allNodes)
            {
                dists.Add(item, new Dictionary<string, long>());
                foreach (var jtem in _allNodes)
                {
                    if (item.Equals(jtem))
                    {
                        dists[item].Add(jtem, 0);
                    }
                    else
                    {
                        dists[item].Add(jtem, this[item, jtem]);
                    }
                }
            }

            foreach (var ktem in _allNodes)
            {
                foreach (var item in _allNodes)
                {
                    foreach (var jtem in _allNodes)
                    {
                        long dist = dists[item][ktem] + dists[ktem][jtem];

                        if (dist < dists[item][jtem])
                        {
                            dists[item][jtem] = dist;
                        }
                    }
                }
            }

            return dists;
        }

        public Dictionary<string, long> FordBellman(string node)
        {
            Dictionary<string, long> dists = new Dictionary<string, long>();
            Dictionary<string, string> paths = new Dictionary<string, string>();

            foreach (var dict in _adjacencyList)
            {
                dists.Add(dict.Key, int.MaxValue);
                paths.Add(dict.Key, "-1");
            }

            dists[node] = 0;

            string x = "";
            for (int i = 0; i < _adjacencyList.Count - 1; ++i)
            {
                x = "-1";
                foreach (var dict in _adjacencyList)
                {
                    foreach (var item in dict.Value)
                    {
                        string first = dict.Key;
                        string second = item.Key;

                        int weight = item.Value;

                        if (dists[first] < int.MaxValue && dists[second] > dists[first] + weight)
                        {
                            dists[second] = dists[first] + weight;
                            paths[second] = first;
                            x = second;
                        }
                    }
                }
            }

            if (x == "-1")
            {
                Console.WriteLine("No negative cycle");

                foreach (var item in _adjacencyList)
                {
                    if (item.Key != node)
                    {
                        List<string> path = new List<string>();
                        string cur = item.Key;
                        path.Add(cur);

                        while (paths[cur] != "-1")
                        {
                            cur = paths[cur];
                            path.Add(cur);
                        }

                        path.Reverse();
                        Console.Write($"{item.Key}: ");
                        for (int i = 0; i < path.Count; ++i)
                        {
                            Console.Write(path[i] + " ");
                        }

                        Console.WriteLine();
                    }
                }
            }
            else
            {
                string y = x;
            }

            // Console.WriteLine("Incidence list:");
            // foreach (var item in ancestors)
            // {
            //     Console.WriteLine($"{item.Key} --- {item.Value}");
            // }
            //
            // if (x.Equals("-1"))
            // {
            //     Console.WriteLine($"No negative cycle from {node}");
            // }
            // else
            // {
            //     Console.WriteLine($"x: {x}");
            //     string y = x;
            //
            //     for (int i = 0; i < _adjacencyList.Count; ++i)
            //     {
            //         y = ancestors[y];
            //     }
            //
            //     List<string> path = new List<string>();
            //     for (string cur = y; !cur.Equals(x); cur = ancestors[cur])
            //     {
            //         path.Add(cur);
            //     }
            //
            //     path.Reverse();
            //
            //     Console.WriteLine("Negative cycle: ");
            //     for (int i = 0; i < path.Count; ++i)
            //     {
            //         Console.Write(path[i] + " ");
            //     }
            //
            //     Console.WriteLine();
            // }

            return dists;
        }

        // Method saving graph to a file with absolute "filePath"
        public void SaveGraph(string filePath)
        {
            using (StreamWriter fileWriter = new StreamWriter(filePath, false))
            {
                foreach (KeyValuePair<string, Dictionary<string, int>> dict in _adjacencyList)
                {
                    fileWriter.Write(dict.Key.ToString() + ' ');
                    foreach (KeyValuePair<string, int> item in dict.Value)
                    {
                        fileWriter.Write(item.Key.ToString() + ':' + item.Value + ' ');
                    }
                
                    fileWriter.WriteLine();
                }
            }
        }

        public void Clear()
        {
            foreach (KeyValuePair<string, Dictionary<string, int>> dict in this._adjacencyList)
            {
                dict.Value.Clear();
            }
            this._adjacencyList.Clear();
        }

        public Graph Copy()
        {
            Graph graph = new Graph(_isWeighted, _isDirectional);

            string vertex = "";
            foreach (var dict in _adjacencyList)
            {
                vertex = dict.Key;
                Dictionary<string, int> tempDict = new Dictionary<string, int>();
                foreach (var item in dict.Value)
                {
                    tempDict.Add(item.Key, item.Value);
                }
                
                graph.AdjacencyList.Add(vertex, tempDict);
            }

            return graph;
        }

        public override string ToString()
        {
            string result = $"Is directed: {this._isDirectional}\nIs weighted: {this._isWeighted}\n";
            foreach (KeyValuePair<string, Dictionary<string, int>> dict in this._adjacencyList)
            {
                result += dict.Key.ToString() + " ";
                foreach (KeyValuePair<string, int> item in dict.Value)
                {
                    if (this._isWeighted)
                    {
                        result += item.Key.ToString() + ":" + item.Value + " ";
                    }
                    else
                    {
                        result += item.Key.ToString() + " ";
                    }
                }

                result += '\n';
            }

            return result;
        }
        
    }
}