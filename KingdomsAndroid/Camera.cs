using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace KingdomsAndroid
{

    public class Camera
    {
        private Matrix transform;
        public Matrix Transform
        {
            get { return transform; }
        }

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
        }

        private float zoom;
        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; }
        }

        private Viewport viewport;

        public Camera(Viewport newViewport)
        {
            viewport = newViewport;
            zoom = 1f;
        }

        public void Update(Vector2 pos, int maxOffsetX, int maxOffsetY)
        {
            int xMax, yMax;
            int viewportZoomWidth = (int)((float)(viewport.Width) / zoom);
            int viewportZoomHeight = (int)((float)(viewport.Height) / zoom);

            // Handle maps smaller than view port
            if (maxOffsetX <= viewportZoomWidth)
                xMax = 0;
            else
                xMax = maxOffsetX - viewportZoomWidth;

            if (maxOffsetY <= viewportZoomHeight)
                yMax = 0;
            else
                yMax = maxOffsetY - viewportZoomHeight;

            // Limit camera movement
            if (pos.X < 0)
            {
                position.X = 0;
            }
            else if (pos.X > xMax)
            {
                position.X = xMax;
            }
            else
            {
                position.X = pos.X;
            }

            if (pos.Y < 0)
            {
                position.Y = 0;
            }
            else if (pos.Y > yMax)
            {
                position.Y = yMax;
            }
            else
            {
                position.Y = pos.Y;
            }



            transform = Matrix.CreateTranslation(new Vector3(-position, 0f)) * Matrix.CreateScale(zoom);
        }

    }

    /*public class Camera
    {
        public float Zoom { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Bounds { get; protected set; }
        public Rectangle VisibleArea { get; protected set; }
        public Matrix Transform { get; protected set; }

        private float currentMouseWheelValue, previousMouseWheelValue, zoom, previousZoom;

        public Camera(Rectangle bounds)
        {
            Bounds = bounds;
            Zoom = 1f;
            Position = Vector2.Zero;
        }


        private void UpdateVisibleArea()
        {
            var inverseViewMatrix = Matrix.Invert(Transform);

            var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
            var tr = Vector2.Transform(new Vector2(Bounds.X, 0), inverseViewMatrix);
            var bl = Vector2.Transform(new Vector2(0, Bounds.Y), inverseViewMatrix);
            var br = Vector2.Transform(new Vector2(Bounds.Width, Bounds.Height), inverseViewMatrix);

            var min = new Vector2(
                MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
            var max = new Vector2(
                MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
            VisibleArea = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        private void UpdateMatrix()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                    Matrix.CreateScale(Zoom) *
                    Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
            //UpdateVisibleArea();
        }

        public void Move(Vector2 movePosition)
        {
            Vector2 newPosition = Position + movePosition;
            Position = newPosition;
        }

        public void AdjustZoom(float zoomAmount)
        {
            Zoom += zoomAmount;
            if (Zoom < .35f)
            {
                Zoom = .35f;
            }
            if (Zoom > 2f)
            {
                Zoom = 2f;
            }
        }

        public void Update(Rectangle bounds)
        {
            Bounds = bounds;
            
            Vector2 cameraMovement = Vector2.Zero;

            TouchCollection touchCollection = TouchPanel.GetState();

            foreach (TouchLocation touch in touchCollection)
            {

                if ((touch.State == TouchLocationState.Moved) || (touch.State == TouchLocationState.Pressed))
                {
                    TouchLocation prevLocation;

                    // Sometimes TryGetPreviousLocation can fail. Bail out early if this happened
                    // or if the last state didn't move
                    if (touch.TryGetPreviousLocation(out prevLocation))
                    {
                        // get your delta
                        var delta = touch.Position - prevLocation.Position;
                        
                        cameraMovement = -delta;
                    }
                    break;
                    // Everything is fine
                    //state = ButtonState.Clicked;
                }
            }
            

            Move(cameraMovement);
            UpdateMatrix();
        }
    }*/
}
 