﻿using _2dracer.Managers;

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2dracer
{
    public class Enemy : Mover
    {
        // Fields
        protected Queue<Node> Route { get; set; } //The path the enemy will take
        protected Node currentDestination; //The node within the path that the car will currently go towards
        protected Node currentNode; //This node holds the center node of the tile the car just stepped on. Used for A* calculations
        private float prevRotation;

        #region Constructors
        public Enemy(Texture2D sprite, Vector2 position) 
            : base(new GameObject(position, 0, sprite, new Vector2(Options.ScreenWidth / 12, Options.ScreenHeight / 13.5f), 0.15f), Vector2.Zero, 0)
        {
            prevRotation = rotation;
            if (MapElements.Map.Nodes[(int)this.Position.X / 768 + 1, (int)this.Position.Y / 768 + 1] != null)
            {
                currentNode = MapElements.Map.Nodes[(int)this.Position.X / 768 + 1, (int)this.Position.Y / 768 + 1];
            }
        }

        //Constructor that takes a Node
        public Enemy(Texture2D sprite, Vector2 position, Node startingNode)
            : base(new GameObject(position, 0, sprite, new Vector2(Options.ScreenWidth / 12, Options.ScreenHeight / 13.5f), 0.15f), Vector2.Zero, 0)
        {
            prevRotation = rotation;
            currentNode = startingNode; //set the initial ai path
            Player.Enter += this.FindRoute; //subscribe the recalculate method for this cop to the player's enter event
            Route = SetPatrolRoute();
        }
        #endregion

        // Methods
        public override void Draw()
        {
            base.Draw();
            if(currentDestination != null)
            Game1.spriteBatch.DrawString(LoadManager.Fonts["Connection"], "GOING TO " + currentDestination.Location, new Vector2(this.Position.X + 10, this.Position.Y - 10), Color.Red, 0f, Vector2.Zero, 0.25f, SpriteEffects.None, 1.0f);
        }

        public override void Update()
        {
            UpdatePositionTowardsNextNode();

            if(MapElements.Map.Nodes[(int)this.Position.X / 768 + 1, (int)this.Position.Y / 768 + 1] != null)
            { 
                currentNode = MapElements.Map.Nodes[(int)this.Position.X / 768, (int)this.Position.Y / 768];
            }
            base.Update();
        }

        /// <summary>
        /// Finds route to the Node the Player just stepped on. 
        /// </summary>
        public void FindRoute(Node playerNode)
        {
            if(currentNode != null && playerNode != null)
            {
                Console.WriteLine("PATHFINDING!");
                this.Route = AI.Pathfind(currentNode, playerNode);
            }
            else
            {
                Console.WriteLine("Tried to calculate AI, but a component was null!");
            }

            
        }

        private void UpdatePositionTowardsNextNode() //Moves the car a little along its current route
        {
            if(Route != null && Route.Count > 0 && currentDestination != null) //Don't do anything if there's no Route assigned
            {
                // set range to 100
                // cop should not touch the point before going to the next
                if (WithinRange(10, this.Route.Peek()))
                {
                    currentDestination = this.Route.Dequeue(); //If reached current target node, fetch next one from the Queue
                }

                Vector2 toNode = new Vector2(currentDestination.Location.X - this.Position.X, currentDestination.Location.Y - this.Position.Y); //Vector to the target 

                #region rotation stuff i dont wanna touch

                // rot is the rotation that the player SHOULD be moving in
                // it is not the rotation that the player IS moving in
                float rot = (float)Math.Atan2(toNode.Y, toNode.X);
                toNode.Normalize(); //turn to unit vector
           
                // if this is not here, then current rotation may be 359 degrees
                // and the required rotation may be 2 degrees, and then
                // car will turn in the wrong directoin
                // this if-statement fixes it, trust me
                // comment it out and see what happens

                if (Math.Abs(rot - rotation) > Math.PI)
                    rot += 2 * (float)Math.PI;

                // if player's rotation is not the correct rotation
                // then slowly turn the player
                if (rot > rotation)
                    rotation += 0.02f;

                if (rot < rotation)
                    rotation -= 0.02f;

                //Apply movement with the current rotation of the car

                double totalVelocity = Math.Sqrt(velocity.X * velocity.X + velocity.Y + velocity.Y);

                // cops slam on breaks if they go too fast
                if (Math.Abs(totalVelocity) > 100)
                {
                    velocity.X /= 2;
                    velocity.Y /= 2;
                }


                velocity.X += (float)Math.Cos(rotation) * 3;
                velocity.Y += (float)Math.Sin(rotation) * 3;
                
                if (prevRotation != rotation)
                {
                    float rotDiff = prevRotation - rotation;

                    velocity = new Vector2(
                        (float)(velocity.X * Math.Cos(-rotDiff) - velocity.Y * Math.Sin(-rotDiff)),
                        (float)(velocity.X * Math.Sin(-rotDiff) + velocity.Y * Math.Cos(-rotDiff)));


                    prevRotation = rotation;
                }
                #endregion
            }
            
        }

        private bool WithinRange(int offset, Node center) //Creates an acceptable area to check when to get the next target
        {
            Rectangle acceptableArea = new Rectangle(center.Location.X - offset, center.Location.Y - offset, 2 * offset, 2 * offset);
            
            if (acceptableArea.Contains(this.Position))
            { 
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// dirty helper method to get the cop to go in the longest possible direction 
        /// </summary>
        private Queue<Node> SetPatrolRoute()
        {
            int thisX = (int)this.Position.X / 768 + 1;
            int thisY = (int)this.Position.Y / 768 + 1;
            int[] directionCounts = { 0, 0, 0, 0 }; //array to hold clear counts in all directions {+X, -X, +Y, -Y}

            MapElements.Tile[,] tiles = MapElements.Map.Tiles; //points to the array of tiles

            #region set direction values
            int loopCounter = thisX;
            while(tiles[loopCounter, thisY].Type != MapElements.TileType.Building && loopCounter < tiles.GetLength(0)) //loop through +x
            {
                directionCounts[0]++; //increment the counter 
                loopCounter++;
            }
            loopCounter = thisX;
            while (tiles[loopCounter, thisY].Type != MapElements.TileType.Building && loopCounter > 0) //loop through -x
            {
                directionCounts[1]++; //increment the counter 
                loopCounter--;
            }
            loopCounter = thisY;
            while (tiles[thisX, loopCounter].Type != MapElements.TileType.Building && loopCounter < tiles.GetLength(1)) //loop through +y
            {
                directionCounts[2]++; //increment the counter 
                loopCounter++;
            }
            loopCounter = thisY;
            while (tiles[thisX, loopCounter].Type != MapElements.TileType.Building && loopCounter > 0) //loop through -y
            {
                directionCounts[3]++; //increment the counter 
                loopCounter--;
            }
            #endregion

            #region hold values for the longest possible route
            int maxLength = 0; //to compare against
            int bestIndex = -1; //to hold the index to use
            for (int i = 0; i < directionCounts.Length; i++)
            {
                if(directionCounts[i] > maxLength) //hold best direction
                {
                    maxLength = directionCounts[i];
                    bestIndex = i;
                }
            }
            #endregion

            Queue<Node> route = new Queue<Node>();
            switch(bestIndex)
            {
                case 0:
                    for (int i = 0; i < directionCounts[bestIndex]; i++) //add nodes along this direction to the route
                    {
                        if(tiles[i, thisY].Node != null)
                        route.Enqueue(tiles[i, thisY].Node);
                    }
                    break;
                case 1:
                    for (int i = thisX; i > thisX-directionCounts[bestIndex]; i--) //add nodes along this direction to the route
                    {
                        if (tiles[i, thisY].Node != null)
                            route.Enqueue(tiles[i, thisY].Node);
                    }
                    break;
                case 2:
                    for (int i = thisY; i < directionCounts[bestIndex]; i++) //add nodes along this direction to the route
                    {
                        if (tiles[thisX, i].Node != null)
                            route.Enqueue(tiles[thisX, i].Node);
                    }
                    break;
                case 3:
                    for (int i = thisY; i > thisY - directionCounts[bestIndex]; i--) //add nodes along this direction to the route
                    {
                        if (tiles[thisX, i].Node != null)
                            route.Enqueue(tiles[thisX, i].Node);
                    }
                    break;
            }
            return route;
        }

        public void PrintDebug()
        {
            Console.WriteLine("mostRecent: " + currentNode);
            if(currentDestination != null)
            Console.WriteLine("currentDestination: " + currentDestination);
        }
    }
}
//Ruben Young