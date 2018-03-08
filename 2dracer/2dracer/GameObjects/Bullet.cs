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
    class Bullet : GameObject
    {
        public Bullet(Texture2D tex, Vector2 pos, float angle) :
            base (pos, angle, tex, new Vector2(0.5f, 0.5f))
        {
            //bullet position = gun position
            //we want bullet to start at tip of gun
            //move bullet to tip of gun

            //advance bullet by 7 frames
            for (int i = 0; i < 7; i++)
                Update();
        }

        public void Update()
        {
            // only move bullet if it is close enough to matter
            if (Math.Abs(position.X) < 1000 || Math.Abs(position.Y) < 1000)
            {
                float speed = 10;

                position.X += (float)Math.Cos(rotation) * speed;
                position.Y += (float)Math.Sin(rotation) * speed;
            }
        }

        public void Draw()
        {
            rotation += (float)Math.PI / 2;
            base.DrawRect(20);
            rotation -= (float)Math.PI / 2;
        }
    }
}

// Niko Procopi