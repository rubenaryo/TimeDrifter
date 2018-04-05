﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using _2dracer.Managers;

namespace _2dracer.MapElements
{
    public class Building
    {
        private Model model;

        public Vector2 Position;

        public Building(Model m, Vector2 p)
        {
            model = m;
            Position = p;
        }

        public void Draw()
        {
            BasicEffect effect = (BasicEffect)model.Meshes[0].Effects[0];
            
            float aspectRatio = Game1.graphics.PreferredBackBufferWidth / (float)Game1.graphics.PreferredBackBufferHeight;
            float fieldOfView = MathHelper.PiOver4;

            int z = 6;

            effect.Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, 0.1f, 200);
            effect.View = Matrix.CreateTranslation(-1 * Game1.camera.Position.X / (55 * z), Game1.camera.Position.Y / (55 * z), (float)-10 / z);
            effect.World = Matrix.CreateTranslation(Position.X, Position.Y, 0);
            
            model.Meshes[0].Draw();
        }
    }
}