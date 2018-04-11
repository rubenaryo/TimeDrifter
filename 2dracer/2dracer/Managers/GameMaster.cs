﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using _2dracer.GameObjects;

namespace _2dracer.Managers
{
    static class GameMaster
    {
        // fields
        private static List<GameObject> gameObjects;
        private static List<Mover> movers;
        private static List<Rigid> rigids;
        private static List<Bullet> bullets;
        private static List<Enemy> enemies;

        // properties
        public static List<GameObject> GameObjects { get { return gameObjects; } }
        public static List<Mover> Movers { get { return movers; } }
        public static List<Rigid> Rigids { get { return rigids; } }
        public static List<Bullet> Bullets { get { return bullets; } }
        public static List<Enemy> Enemies { get { return enemies; } }

        public static void Start()
        {
            gameObjects = new List<GameObject>();
            movers = new List<Mover>();
            rigids = new List<Rigid>();
            bullets = new List<Bullet>(50);
            enemies = new List<Enemy>();

            // Instantiate GameObjects here please
            Instantiate(new Player(new Vector2(Game1.screenWidth / 2, Game1.screenHeight / 2)));
            Instantiate(new Turret());

            for (int i = 0; i < bullets.Capacity; i++)
            {
                Instantiate(new Bullet(new Vector2(-999, -999), 0));
            }

            for (int i = 0; i < 5; i++)
            {
                Instantiate(new Enemy("Textures/Cop", new Vector2(200, 200 * i)));
            }


            // GameMaster Load
            foreach (GameObject obj in GameObjects)
                obj.Sprite = Program.game.Content.Load<Texture2D>(obj.SpritePath);
        }

        // Methods
        /// <summary>
        /// Updates all game objects
        /// </summary>
        public static void Update()
        {
            foreach (GameObject g in gameObjects)
            {
                g.Update();
            }
        }
        
        /// <summary>
        /// Draws all game objects in correct ordering
        /// MUST call spriteBatch.Begin first
        /// </summary>
        public static void Draw()
        {
            // REPLACE THIS WITH SMARTER CODE TO DRAW OBJECTS IN LAYERS
            foreach (GameObject g in gameObjects)
            {
                g.Draw();
            }
        }

        /// <summary>
        /// Instantiatess a GameObject to be updated and drawn
        /// </summary>
        public static void Instantiate(GameObject g)
        {
            gameObjects.Add(g);

            if(g is Mover)
            {
                movers.Add((Mover)g);
            }
            if(g is Rigid)
            {
                rigids.Add((Rigid)g);
            }
            if(g is Bullet)
            {
                bullets.Add((Bullet)g);
            }
            if(g is Enemy)
            {
                enemies.Add((Enemy)g);
            }
        }

        /// <summary>
        /// Clears all GameObjects from the game
        /// </summary>
        public static void ClearAll()
        {
            gameObjects.Clear();
            movers.Clear();
            rigids.Clear();
        }

        //TODO: Create a method/event that is triggered each time the player steps into a new tile (to calculate pathfinding based on that)
    }
}

// Matthew Soriano