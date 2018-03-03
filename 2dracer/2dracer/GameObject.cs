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
    public class GameObject
    {
        // fields
        protected Vector2 position;     // position of center in world space
        protected float rotation;       // rotation in degrees from +x axis (remember y is flipped)

        protected Texture2D sprite;     // static sprite
        protected Color color;          // color tint of the sprite
        protected Vector2 size;         // fixed standard size (width and height) of objects of this type
        protected Vector2 scale;        // the scaling factor for this particular object

        protected bool isEnabled;       // if this object should be updated and drawn

        // properties
        public Vector2 Position { get { return position; } }
        public float Rotation { get { return rotation; } }
        public Texture2D Sprite { get { return sprite; } set { sprite = value; } }
        public Color Color { get { return color; } }
        public Vector2 Size { get { return size; } }
        public Vector2 Scale { get { return scale; } }
        public bool IsEnabled { get { return isEnabled; } }

        // constructors
        public GameObject(GameObject g)
                   : this(g.Position, g.Rotation, g.Sprite, g.Color, g.Scale, g.IsEnabled) { }
        
        public GameObject(Vector2 position, float rotation, Texture2D sprite, Color color, Vector2 scale, bool startEnabled)
        {
            this.position = position;
            this.rotation = rotation;

            this.sprite = sprite;
            this.color = color;
            size = new Vector2(50, 50);
            this.scale = scale;

            isEnabled = startEnabled;
        }

        public GameObject(Vector2 position, float rotation, Texture2D sprite, Color color, Vector2 scale)
                   : this(position, rotation, sprite, color, scale, true) { }

        public GameObject(Vector2 position, float rotation, Texture2D sprite, Vector2 scale)
                   : this(position, rotation, sprite, Color.White, scale) { }

        public GameObject(Vector2 position, float rotation, Texture2D sprite)
                   : this(position, rotation, sprite, Vector2.One) { }

        public GameObject(Vector2 position, float rotation)
                   : this(position, rotation, Game1.square) { }

        public GameObject()
                   : this(Vector2.Zero, 0f) { }

        // methods
        /// <summary>
        /// Updates logic for this game object every frame
        /// </summary>
        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Draws this object's texture to the screen
        /// MUST call spriteBatch.Begin first
        /// </summary>
        public virtual void Draw()
        {
            Vector2 appliedScale = new Vector2((size.X * scale.X) / sprite.Width, (size.Y * scale.Y) / sprite.Height);
            float radianRot = (float)((rotation * Math.PI) / 180);
            Vector2 origin = new Vector2((sprite.Width) / 2, (sprite.Height) / 2);
            Game1.spriteBatch.Draw(sprite, position, null, color, radianRot, origin, appliedScale, SpriteEffects.None, 0f);
        }
    }
}

// Matthew Soriano