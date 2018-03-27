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
    public class Player : Mover
    {
        // Fields
        private Turret turret;
        private SpriteFont font;
        private double timeJuice;

        // Properties
        public int Health { get; private set; }

        // Constructor
        public Player(Texture2D sprite, Texture2D bulletSprite, Texture2D turretSprite, Vector2 position, SpriteFont s) 
            : base(new GameObject(position, 0, sprite, new Vector2(64, 128)), 
                  new Vector2(0,0), new Vector2(0,0), 0, 0, 1)
        {
            turret = new Turret(turretSprite, bulletSprite);
            font = s;
            Health = 100;
            timeJuice = 0;
        }

        // Methods
        public override void Update()
        {
            float xAxis = Input.GetAxisRaw(Axis.X);

            // turn car
            AddTorque(xAxis * 0.4f);
            if (xAxis == 0)
            {
                angularVelocity *= 0.99f;
            }
            // move car
            float yAxis = Input.GetAxisRaw(Axis.Y);
            float horsepower = yAxis * 90.0f;

            Vector2 force = new Vector2();
            force.X += (float)Math.Cos(rotation) * horsepower;
            force.Y += (float)Math.Sin(rotation) * horsepower;
            
            AddForce(force);

            // friction of road
            if (yAxis == 0)
            {
                AddForce(velocity * -1);
            }

            // friction of breaks
            if (Input.KeyHold(Keys.Space))
            {
                // yes! angularVelocity = 0, because breaks stop the car
                // this wont be in the final version, its just here to make testing the game easier
                velocity = Vector2.Zero;
                angularVelocity = 0;
            }

            if (timeJuice < 10)
                timeJuice += Game1.gameTime.ElapsedGameTime.TotalMilliseconds/1000;


            // Update turret
            turret.Update(position);
            base.Update();
        }

        public override void Draw()
        {
            rotation += (float)Math.PI / 2;
            base.Draw();
            rotation -= (float)Math.PI / 2;

            // Draw turret
            turret.Draw();
        }
    }
}

// Niko Procopi
