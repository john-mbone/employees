using Algorithms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.csv
{
    public class Employees
    {
        public DirectedSparseGraph<Worker> nodeGraph;

        public Dictionary<string, Worker> workers;

        /// <summary>
        /// Title for debug
        /// </summary>
        private readonly string debug = "--From Lib.csv Debug--";

        /// <summary>
        /// Owner counter
        /// </summary>
        public int Owner = 0;


        /// <summary>
        /// Employees constructor takes an array of strings from readallline -System.IO
        /// Iterates through each record in the array using the GetEnumerator() and moving the cursor.
        /// </summary>
        /// <param name="lines"></param>

        public Employees(string[] lines)
        {
            nodeGraph = new DirectedSparseGraph<Worker>();
            workers = new Dictionary<string, Worker>();
            IEnumerable<string[]> rows = lines
                .Select(r => r.Split('\t'));

           

            IEnumerable<IEnumerable<string>> records = from row in rows
                                                       select (from item in row
                                                               select item);

            
            foreach (var n in records)
            {
                var p = n.GetEnumerator();
                while (p.MoveNext())
                {
                    try
                    {
                        var data = p.Current.Split(',');
                        if (string.IsNullOrEmpty(data[0]))
                        {
                            Debug.WriteLine("Invalid employee id- please resolve this!", debug);
                            continue;
                        }

                        if (string.IsNullOrEmpty(data[1]) && Owner < 1)
                        {
                            Owner++;
                        }
                        else if (string.IsNullOrEmpty(data[1]) && Owner == 1)
                        {
                            Debug.WriteLine($"A company can have only one C.E.O {data[1]} Adding error", debug);
                            continue;
                        }


                        // Salary check using a discard
                        if (Int32.TryParse(data[2], out _))
                        {
                            var empl = new Worker(data[0], data[1], int.Parse(data[2]));
                            try
                            {
                                workers.Add(empl.Id, empl);
                            }
                            catch
                            {
                                //Employee appers twice in he dictionary<string,Worker>
                                Debug.WriteLine($"{data[1]} Has alrady been added to the list", debug);
                            }

                            if (!nodeGraph.HasVertex(empl))//false
                            {
                                nodeGraph.AddVertex(empl);
                            }

                        }
                        else
                        {
                            Debug.WriteLine($"Invalid salary value... for user {data[1]}", debug);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message, debug);
                    }
                }
                p.Dispose();

            }
            ///Check for d-Linking
        
            foreach (KeyValuePair<string, Worker> keyValuePair in workers)
            {
                if (!string.IsNullOrEmpty(keyValuePair.Value.Manager))
                {
                    // check for double linking
                    bool doubleLinked = false;
                    foreach (Worker worker in nodeGraph.D_F_S(keyValuePair.Value).ToArray())
                    {
                        if (worker.Equals(keyValuePair.Value.Manager))
                        {
                            doubleLinked = true;
                            break;
                        }
                    }
                    // ensure that each employee has only one manager
                    if (nodeGraph.IncomingEdges(keyValuePair.Value).ToArray().Length < 1 && !doubleLinked)
                    {
                        nodeGraph.AddEdge(workers[keyValuePair.Value.Manager], keyValuePair.Value);
                    }
                    else
                    {
                        if (nodeGraph.IncomingEdges(keyValuePair.Value).ToArray().Length >= 1)
                        {
                            Debug.WriteLine($"Employee {keyValuePair.Value.Id} have more than one manager", debug);
                            //resume not returning
                        }
                        Debug.WriteLine("Double linking not allowed", debug);
                    }
                }

            }
        }

        public long Budget(string manager)
        {
            var salaryBudget = 0;
            try
            {
                var employeesInPath = nodeGraph.D_F_S(workers[manager]).GetEnumerator();

                while (employeesInPath.MoveNext())
                {
                    salaryBudget += employeesInPath.Current.Salary;
                }

            }
            catch (Exception error)
            {
                salaryBudget = 0;
                Debug.WriteLine(error.Message, debug);
            }

            return salaryBudget;
        }
    }

    public class Worker : IComparable<Worker>
    {
        public string Id { get; set; }
        public int Salary { get; set; }
        public string Manager { get; set; }

        public Worker(string id, string manager, int salary)
        {
            Id = id;
            Salary = salary;
            Manager = manager;
        }

        public int CompareTo(Worker other)
        {
            if (other == null) return -1;
            return string.Compare(this.Id, other.Id,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
