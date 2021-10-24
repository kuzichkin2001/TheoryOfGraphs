using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Graph
{
    public class Graph<T>
    {
        
        private Dictionary<T, Dictionary<T, int>> _adjacencyList;
        private bool _isWeighted;
        private bool _isDirectional;
        private List<T> _visited;
        
        // Empty constructor instantiating an adjacency list
        public Graph(bool isWeighted = false, bool isDirectional = false)
        {
            this._adjacencyList = new Dictionary<T, Dictionary<T, int>>();
            this._isWeighted = isWeighted;
            this._isDirectional = isDirectional;
            this._visited = new List<T>();
        }

        // Constructor that is reading a graph from file with absolute "filePath"
        public Graph(string filePath, bool isWeighted = false, bool isDirectional = false)
        {
            this._isWeighted = isWeighted;
            this._isDirectional = isDirectional;
            this._adjacencyList = new Dictionary<T, Dictionary<T, int>>();
            using (StreamReader fileReader = new StreamReader(filePath, Encoding.UTF8))
            {
                string line;
                while ((line = fileReader.ReadLine()) != null)
                {
                    string[] tempArr = line.Split();
                    if (typeof(T) == typeof(int))
                    {
                        int name = Convert.ToInt32(tempArr[0]);
                        _adjacencyList[(T) (object) name] = new Dictionary<T, int>();
                        for (int i = 1; i < tempArr.Length; ++i)
                        {
                            if (this._isWeighted)
                            {
                                string[] subTemp = tempArr[i].Split(':');
                                int subName = Convert.ToInt32(subTemp[0]);
                                int weight = Convert.ToInt32(subTemp[1]);
                                _adjacencyList[(T) (object) name][(T) (object) subName] = weight;
                            }
                            else
                            {
                                int subName = Convert.ToInt32(tempArr[i]);
                                _adjacencyList[(T) (object) name][(T) (object) subName] = 0;
                            }
                        }
                    }
                    else if (typeof(T) == typeof(string))
                    {
                        string name = tempArr[0];
                        _adjacencyList[(T) (object) name] = new Dictionary<T, int>();
                        for (int i = 1; i < tempArr.Length; ++i)
                        {
                            if (_isWeighted)
                            {
                                string[] subTemp = tempArr[i].Split(':');
                                string subName = subTemp[0];
                                int weight = Convert.ToInt32(subTemp[1]);
                                _adjacencyList[(T) (object) name][(T) (object) subName] = weight;
                            }
                            else
                            {
                                string subName = tempArr[i];
                                _adjacencyList[(T) (object) name][(T) (object) subName] = 0;
                            }
                        }
                    }
                }
            }

            this._visited = new List<T>(this._adjacencyList.Count);
        }

        // Copy constructor
        public Graph(Graph<T> graph)
        {
            this._isWeighted = graph.IsWeighted;
            this._isDirectional = graph.IsDirectional;
            this._adjacencyList = new Dictionary<T, Dictionary<T, int>>();
            foreach (KeyValuePair<T, Dictionary<T, int>> dict in graph.AdjacencyList)
            {
                _adjacencyList[dict.Key] = new Dictionary<T, int>();
                foreach (KeyValuePair<T, int> item in dict.Value)
                {
                    _adjacencyList[dict.Key][item.Key] = item.Value;
                }
            }

            this._visited = new List<T>(this._adjacencyList.Count);
        }

        public bool this[string key]
        {
            get
            {
                return this._adjacencyList.ContainsKey((T) (object) key);
            }
        }

        // Property providing access to adjacency list of Graph
        public Dictionary<T, Dictionary<T, int>> AdjacencyList
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
        public void AddNode(T key)
        {
            _adjacencyList.Add(key, new Dictionary<T, int>());
        }
        
        // Method Adding new edge between "from" and "to" vertexes with weight "weight" in Graph
        public void AddEdge(T from, T to, int weight = 1)
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
        public void RemoveNode(T key)
        {
            foreach (KeyValuePair<T, Dictionary<T, int>> dict in _adjacencyList)
            {
                foreach (KeyValuePair<T, int> item in dict.Value)
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
        public void RemoveEdge(T from, T to)
        {
            _adjacencyList[from].Remove(to);
            if (!_isDirectional)
            {
                _adjacencyList[to].Remove(from);
            }
        }

        public int GetIncomingPower(T key)
        {
            Dictionary<T, int> currentNode = this._adjacencyList[key];

            return currentNode.Count;
        }

        public int GetOutgoingPower(T key)
        {
            int count = 0;

            foreach (KeyValuePair<T, Dictionary<T, int>> dict in this._adjacencyList)
            {
                if (dict.Value.ContainsKey(key))
                {
                    count++;
                }
            }

            return count;
        }

        public void PrintNodesWithLesserIncomingPower(T key)
        {
            int currentIncomingPower = GetIncomingPower(key);

            foreach (KeyValuePair<T, Dictionary<T, int>> dict in this._adjacencyList)
            {
                if (GetIncomingPower(dict.Key) == currentIncomingPower)
                {
                    Console.Write("{0} ", dict.Key.ToString());
                }
            }
        }

        public void PrintAllIncomingNodes(T key)
        {
            if (!this._isDirectional)
            {
                Console.WriteLine("Unable to do this with not directed graph.");
            }
            else
            {
                foreach (KeyValuePair<T, Dictionary<T, int>> dict in this._adjacencyList)
                {
                    if (dict.Value.ContainsKey(key))
                    {
                        Console.WriteLine(dict.Key.ToString());
                    }
                }
            }
        }

        public Graph<T> DeleteAllOddNodes()
        {
            Graph<T> graph = this.Copy();

            foreach (KeyValuePair<T, Dictionary<T, int>> dict in graph.AdjacencyList)
            {
                int currentPower = graph.GetIncomingPower(dict.Key);
                if (currentPower % 2 != 0)
                {
                    graph.RemoveNode(dict.Key);
                }
            }

            return graph;
        }
        
        // Method saving graph to a file with absolute "filePath"
        public void SaveGraph(string filePath)
        {
            using (StreamWriter fileWriter = new StreamWriter(filePath, false))
            {
                foreach (KeyValuePair<T, Dictionary<T, int>> dict in this._adjacencyList)
                {
                    fileWriter.Write(dict.Key.ToString() + ' ');
                    foreach (KeyValuePair<T, int> item in dict.Value)
                    {
                        fileWriter.Write(item.Key.ToString() + ':' + item.Value + ' ');
                    }
                
                    fileWriter.WriteLine();
                }
            }
        }

        public void Clear()
        {
            foreach (KeyValuePair<T, Dictionary<T, int>> dict in this._adjacencyList)
            {
                dict.Value.Clear();
            }
            this._adjacencyList.Clear();
        }

        public Graph<T> Copy()
        {
            Graph<T> graph = new Graph<T>();

            foreach (KeyValuePair<T, Dictionary<T, int>> dict in this._adjacencyList)
            {
                graph.AddNode(dict.Key);
                foreach (KeyValuePair<T, int> item in dict.Value)
                {
                    graph.AddEdge(dict.Key, item.Key, item.Value);
                }
            }

            return graph;
        }

        public override string ToString()
        {
            string result = $"Is directed: {this._isDirectional}\nIs weighted: {this._isWeighted}\n";
            foreach (KeyValuePair<T, Dictionary<T, int>> dict in this._adjacencyList)
            {
                result += dict.Key.ToString() + " ";
                foreach (KeyValuePair<T, int> item in dict.Value)
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