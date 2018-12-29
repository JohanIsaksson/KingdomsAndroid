using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;


namespace KingdomsAndroid
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SliderBar
    {
        //All of the instance variables
        protected Texture2D slideBar;
        protected Texture2D slideButton;
        protected Vector2 barSize;
        protected Vector2 buttonSize;
        protected Vector2 barPosition;
        protected Vector2 buttonPosition;
        protected Vector2 mousePosition;
        protected Vector2 mouseRange;
        protected MouseState mouseState;       

        protected int finalAttribute;
        protected int numPartitions;
        protected int barAttribute;
        protected int buttonCenter;        
        protected float modDivision;        
        protected int tempPos;
        protected int barEnd;
        protected bool pressed;
        protected bool clicked;
        protected Game1 game;
        
        /// The constucture takes in parameters for the slider bar and scales the existing textures
        /// as well as the existing algorithms to us the defined size.        
        public SliderBar(Game1 game1, int barWidth, int numPartitions, Vector2 barPosition)
        {
            game = game1;
            slideButton = game.Content.Load<Texture2D>("SliderPoint");
            slideBar = game.Content.Load<Texture2D>("Slider");                                   

            // Determines the height of a bar relative to the length of the bar
            barSize.X = barWidth; barSize.Y = barWidth / 40;
            
            // Determines the size of the slider button relative to the length of the bar
            buttonSize.X = barWidth / (float)15; buttonSize.Y = buttonSize.X/ (float)1.5;

            this.numPartitions = numPartitions;
            this.barPosition = barPosition;

            //Centers the slider button initially on the slider bar
            buttonPosition.X = (barWidth / 2) - ((int)buttonSize.X / 2) + (int)barPosition.X;
            buttonPosition.Y = (barPosition.Y - ((buttonSize.Y - barSize.Y) / 2)) - 2;                        

            barEnd = (int)barPosition.X + barWidth - ((int)buttonSize.X / 2);

            buttonCenter = (int)buttonSize.X / 2;

            // Defines the vertical range of the mouse to a specific slider bar
            mouseRange.X = barPosition.Y - buttonSize.Y;
            mouseRange.Y = barPosition.Y + (1.5f * buttonSize.Y);

            // The size of each parition range
            modDivision = (float)barWidth / (float)numPartitions;            
            barAttribute = numPartitions / 2;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize()
        {            
            
        }

        bool IsInBounds(Vector2 pos)
        {
            Camera cam = this.game.Camera;
            var transformedPos = (pos + cam.Position) / cam.Zoom;
            //var inverseTransform = Matrix.Invert(cam.Transform);
            //var transformedPos = Vector2.Transform(pos, cam.Transform);
            if ((transformedPos.X >= this.buttonPosition.X) &&
                (transformedPos.Y >= this.buttonPosition.Y) &&
                (transformedPos.X < (this.buttonPosition.X + this.buttonSize.X)) &&
                (transformedPos.Y < (this.buttonPosition.Y + this.buttonSize.Y)))
            {
                return true;
            }
            return false;
        }

        /// Checks the left mouse button to see if it is pressed down, in a state that it can
        /// manipulate the slider button.
        private void CheckButtonClick()
        {            
            mouseState = Mouse.GetState();
            clicked = false;
            
            if (mouseState.LeftButton == ButtonState.Pressed)
                pressed = true;
            if (mouseState.LeftButton == ButtonState.Released && pressed)
            {
                clicked = true;
                pressed = false;
            }
        }        
        
        /// <summary>
        /// Provides the variable for the draw method to correctly draw the slider button
        /// </summary>              
        public void setButtonPosition()
        {
            mouseState = Mouse.GetState();            

            if ((mouseState.X - buttonCenter) >= barPosition.X && mouseState.X <= barEnd)
            {
                if (mouseState.Y > mouseRange.X && mouseState.Y < mouseRange.Y)
                {
                    buttonPosition.X = mouseState.X - buttonCenter;
                    setBarAttribute();
                }
            }
        }

        /// <summary>
        /// Sets the parameter value of the slider button relative to the number of paritions on the bar.
        /// </summary>
        public void setBarAttribute()
        {
            int tempAttribute = (int)buttonPosition.X - (int)barPosition.X;
            decimal bA = (decimal)tempAttribute / (decimal)modDivision;
            if (Math.Round(bA) >= 0 && Math.Round(bA) < numPartitions)
                barAttribute = (int)Math.Round(bA);
        }

        /// <summary>
        /// Property to allow outside methods to access the bar attribute once it has been set
        /// </summary>
        public int getFinalAttribute
        {
            get { return finalAttribute; }
        }        
        
        /// <summary>
        /// Property to allow outside methods to access the bar attribute dynamically
        /// </summary>
        public int getBarAttribute
        {
            get { return (int)barAttribute; }
        }
        
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            CheckButtonClick();            

            if (pressed == true)
                setButtonPosition();

            if (clicked == true)
            {
                finalAttribute = (int)barAttribute;
            }
            
            
        }

        /// <summary>
        /// Draws the slider bar and the slider button
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(slideBar, new Rectangle((int)barPosition.X, (int)barPosition.Y,
                                                     (int)barSize.X, (int)barSize.Y), new Color(255, 255, 255, 185));
            
            spriteBatch.Draw(slideButton, new Rectangle((int)buttonPosition.X, (int)buttonPosition.Y,
                                                        (int)buttonSize.X, (int)buttonSize.Y), new Color(255, 255, 255, 185));
            
        }
    }
}


