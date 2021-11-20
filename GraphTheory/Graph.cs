using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Graph
{
    public class Graph<T> where T : class
    {
        
        private Dictionary<T, Dictionary<T, int>> _adjacencyList;
        private bool _isWeighted;
        private bool _isDirectional;
        private Dictionary<T, bool> _visited;
        
        // Empty constructor instantiating an adjacency list
        public Graph(bool isWeighted = false, bool isDirectional = false)
        {
            this._adjacencyList = new Dictionary<T, Dictionary<T, int>>();
            this._isWeighted = isWeighted;
            this._isDirectional = isDirectional;
        }

        // Constructor that is reading a graph from file with absolute "filePath"
        public Graph(string filePath, bool isWeighted = false, bool isDirectional = false)
        {
            this._isWeighted = isWeighted;
            this._isDirectional = isDirectional;
            this._adjacencyList = new Dictionary<T, Dictionary<T, int>>();
            this._visited = new Dictionary<T, bool>();

            using (StreamReader fileReader = new StreamReader(filePath, Encoding.UTF8))
            {
                string line;
                while ((line = fileReader.ReadLine()) != null)
                {
                    string[] tempArr = line.Split();
                    if (typeof(T) == typeof(int))
                    {
                        int name = Convert.ToInt32(tempArr[0]);
                        _visited[(T) (object) name] = false;
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
                        _visited[(T) (object) name] = false;
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

        }

        private void ClearVisited()
        {
            foreach (var item in this._visited)
            {
                this._visited[item.Key] = false;
            }
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
            
        }

        public bool this[T key]
        {
            get
            {
                return this._adjacencyList.ContainsKey(key);
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
                if (GetIncomingPower(dict.Key) < currentIncomingPower)
                {
                    Console.Write("{0} ", dict.Key.ToString());
                }
            }

            Console.WriteLine();
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

        public bool Dfs(T key, ref T cycleStart)
        {
            Dictionary<T, int> currentVertex = this._adjacencyList[key];

            foreach (var item in currentVertex)
            {
                T to = item.Key;
                if (!this._visited[to])
                {
                    this._visited[to] = true;
                    if (Dfs(to, ref cycleStart)) return true;
                }
                else if (this._visited[to])
                {
                    cycleStart = to;
                    return false;
                }
            }
            
            return false;
        }
        
        

        public void IsGraphAcycled()
        {
            T cycleStart = default(T);
            foreach (var dict in this._adjacencyList)
            {
                ClearVisited();
                if (Dfs(dict.Key, ref cycleStart))
                {
                    break;
                }
            }

            if (EqualityComparer<T>.Default.Equals(cycleStart, default(T)))
            {
                Console.WriteLine("Acycled");
            }
            else
            {
                Console.WriteLine("Cycled");
            }
        }

        public void Bfs(T key, out Dictionary<T, int> dist)
        {
            ClearVisited();
            Queue<T> q = new Queue<T>();
            q.Enqueue(key);
            this._visited[key] = true;

            dist = new Dictionary<T, int>();
            dist[key] = 0;

            int countIteration = 1;
            while (q.Count != 0)
            {
                T vertex = q.Dequeue();

                if (!this._visited[vertex])
                {
                    this._visited[vertex] = true;
                    foreach (var item in this._adjacencyList[vertex])
                    {
                        if (!this._visited[item.Key])
                        {
                            this._visited[item.Key] = true;
                            dist[item.Key] = countIteration;
                            q.Enqueue(item.Key);
                        }
                    }
                }
                else
                {
                    if (dist[vertex] < countIteration)
                    {
                        dist[vertex] = countIteration;
                    }
                    q.Dequeue();
                }

                countIteration++;
            }
        }

        private int MinRadius(Dictionary<T, int> eccentricities)
        {
            int min = Convert.ToInt32(1e9);
            foreach (var ecc in eccentricities)
            {
                min = Math.Min(min, ecc.Value);
            }

            return min;
        }

        public List<T> FindGraphCenter()
        {
            List<T> centerResult = new List<T>();
            Dictionary<T, Dictionary<T, int>> dists = new Dictionary<T, Dictionary<T, int>>();

            foreach (var dict in this._adjacencyList)
            {
                Dictionary<T, int> currentDist;
                Bfs(dict.Key, out currentDist);
                dists[dict.Key] = currentDist;
            }

            Dictionary<T, int> eccentricities = new Dictionary<T, int>();
            foreach (var dist in dists)
            {
                int max = dist.Value[dist.Key];
                foreach (var item in dist.Value)
                {
                    if (item.Value > max)
                    {
                        max = item.Value;
                    }
                }

                eccentricities[dist.Key] = max;
            }

            int radius = MinRadius(eccentricities);

            foreach (var item in eccentricities)
            {
                if (item.Value == radius)
                {
                    centerResult.Add(item.Key);
                }
            }

            return centerResult;
        }

        public Dictionary<int, KeyValuePair<T, T>> IncidenceList()
        {
            Dictionary<int, KeyValuePair<T, T>> incidenceList = new Dictionary<int, KeyValuePair<T, T>>();

            foreach (KeyValuePair<T, Dictionary<T, int>> dict in this._adjacencyList)
            {
                foreach (KeyValuePair<T, int> item in dict.Value)
                {
                    if (!incidenceList.ContainsKey(item.Value))
                    {
                        incidenceList.Add(item.Value, new KeyValuePair<T, T>(dict.Key, item.Key));
                    }
                }
            }

            return incidenceList;
        }

        public Graph<T> MinOstovTree()
        {
            Dictionary<int, KeyValuePair<T, T>> incidenceList = IncidenceList();
            var sortedIncidenceList = incidenceList.OrderBy(n => n.Key);

            HashSet<T> used = new HashSet<T>();
            Dictionary<int, KeyValuePair<T, T>> answer = new Dictionary<int, KeyValuePair<T, T>>();

            foreach (KeyValuePair<int, KeyValuePair<T, T>> item in sortedIncidenceList)
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

            Graph<T> graph = new Graph<T>(true, false);
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