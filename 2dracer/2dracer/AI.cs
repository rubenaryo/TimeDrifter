﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2dracer
{
    class AI
    {
        private Enemy[] enemies;

        List<Node> nodes = new List<Node>(); //List of nodes to test A* algorithm

        Queue<Node> testQueue = new Queue<Node>(); //TEMPORARY queue to test giving instructions to enemies

        public AI(Texture2D tex)
        {
            enemies = new Enemy[1];

            enemies[0] = new Enemy(tex, new Vector2(200, 200));
            

            #region node populating
            //Create nodes
            Node nodeStart = new Node(new Point(400, 200));
            Node node1 = new Node(new Point(600, 200));
            Node node2 = new Node(new Point(600, 400));
            Node node3 = new Node(new Point(800, 200));
            Node node4 = new Node(new Point(1000, 200));
            Node node5 = new Node(new Point(600, 600));
            Node node6 = new Node(new Point(800, 400));
            Node node7 = new Node(new Point(1000, 400));
            Node node8 = new Node(new Point(800, 600));
            Node node9 = new Node(new Point(1000, 600));
            Node node10 = new Node(new Point(600, 700));

            //Populate each node's list of neighbors sigh
            nodeStart.PopulateNeighborsList(node1);
            node1.PopulateNeighborsList(nodeStart, node3, node2);
            node2.PopulateNeighborsList(node1, node6, node5);
            node3.PopulateNeighborsList(node1, node6, node4);
            node4.PopulateNeighborsList(node3, node7);
            node5.PopulateNeighborsList(node2, node8, node10);
            node6.PopulateNeighborsList(node2, node3, node7, node8);
            node7.PopulateNeighborsList(node4, node6, node9);
            node8.PopulateNeighborsList(node6, node5, node9, node10);
            node9.PopulateNeighborsList(node7, node8);
            node10.PopulateNeighborsList(node5, node8);

            //Populate AI class' list of nodes
            nodes.Add(nodeStart);
            nodes.Add(node1);
            nodes.Add(node2);
            nodes.Add(node3);
            nodes.Add(node4);
            nodes.Add(node5);
            nodes.Add(node6);
            nodes.Add(node7);
            nodes.Add(node8);
            nodes.Add(node9);
            nodes.Add(node10);


            //Now for the Queue
            testQueue.Enqueue(new Node(new Point(200, 400)));
            testQueue.Enqueue(node1);
            testQueue.Enqueue(node2);
            testQueue.Enqueue(node3);
            testQueue.Enqueue(node4);
            testQueue.Enqueue(node5);
            testQueue.Enqueue(node6);
            testQueue.Enqueue(node7);
            testQueue.Enqueue(node8);
            testQueue.Enqueue(node9);
            testQueue.Enqueue(node10);
            #endregion
        }

        public void Update(Vector2 PlayerPos)
        {
            foreach (Enemy i in enemies)
            {
                i.Route = testQueue;
                i.UpdatePositionTowardsNextNode();
            }
        }

        public void Draw()
        {
            foreach (Enemy i in enemies)
                i.Draw();

            foreach (Node n in nodes) //Draw all the test nodes
            {
                Game1.spriteBatch.Draw(Game1.square, new Rectangle(n.Location, new Point(10, 10)), Color.Purple);
            }
        }

        public void Pathfind(Node target) //Implementation of A* fingers crossed
        {
            foreach (Node n in nodes) //Gives all the nodes the distance to the target;
            {
                n.CalcDistanceToTarget(target);
            }

            //TODO: Finish AI's Main Pathfind function
            foreach (Enemy e in enemies) //Do this for every cop that exists
            {
                Queue<Node> ShortestPath = new Queue<Node>();

                foreach (Node n in nodes) //Worst Case Scenario
                {
                    List<Node> toCompare = new List<Node>();
                    foreach (Node neighbor in n.Neighbors)
                    {
                        neighbor.Parent = n; //Set the node's parent 
                        neighbor.AssignHeuristic(target); //Set the node's weight

                        //Sort Neighbors list
                        n.Neighbors.Sort(CompareNodesBasedOnHeuristic);
                    }
                }
            } 
        }

        private int CompareNodesBasedOnHeuristic(Node x, Node y) //Comparator for sorting the Neighbors List based off heuristics
        {
            if(x != null)
            {
                if(y != null)
                {
                    int returnValue = x.Heuristic - y.Heuristic; //get return value based off their individual heuristics
 
                    if(returnValue != 0)
                    {
                        return returnValue;
                    }
                    else
                    {
                        return 1; //if they're the same it doesn't matter
                    }

                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if(y == null) //if y is null, then x is better by default
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }
    }
}
