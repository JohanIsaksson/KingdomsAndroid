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
using Microsoft.Xna.Framework.Input.Touch;

namespace KingdomsAndroid
{
    public sealed class TouchManager
    {
        // Singleton objects
        private static TouchManager instance = null;
        private static readonly object padlock = new object();

        // touch points
        private List<TouchLocation> touchPoints;
        private List<TouchLocation> prevTouchPoints;
        
        // Filtered and transformed touch points        
        public List<Vector2> PressPoints { get; private set;}
        public List<Vector2> ClickPoints { get; private set; }

        private Camera camera;

        private bool enableSwipe;
        public bool EnableSwipe
        {
            get { return enableSwipe; }
            set { enableSwipe = value; }
        }

        private Vector2 swipeDirection;
        public Vector2 SwipeDirection
        {
            get { return (enableSwipe ? swipeDirection : Vector2.Zero); }
        }

        public static TouchManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new TouchManager();
                    }
                    return instance;
                }
            }
        }

        private TouchManager()
        {
            touchPoints = new List<TouchLocation>();
            prevTouchPoints = new List<TouchLocation>();
            PressPoints = new List<Vector2>();
            ClickPoints = new List<Vector2>();
            enableSwipe = true;
        }

        public bool IsPressed(Rectangle area)
        {
            foreach (Vector2 point in PressPoints)
            {
                // Transform posistion
                var transformedPos = (point / camera.Zoom) + camera.Position;
                if (area.Contains(transformedPos))
                    return true;
            }
            return false;
        }

        public bool IsClicked(Rectangle area)
        {
            foreach (Vector2 point in ClickPoints)
            {
                // Transform posistion
                var transformedPos = (point / camera.Zoom) + camera.Position;
                if (area.Contains(transformedPos))
                    return true;
            }
            return false;
        }

        public void Update(Camera cam)
        {
            camera = cam;

            // Clear all lists
            touchPoints.Clear();
            PressPoints.Clear();
            ClickPoints.Clear();

            // Update touch state
            TouchCollection touchCollection = TouchPanel.GetState();

            // Add touches to list
            foreach (TouchLocation touch in touchCollection)
            {
                touchPoints.Add(touch);
            }


            if (touchPoints.Count == 0)
            {
                // See if any release states were missed
                foreach (TouchLocation touch in prevTouchPoints)
                {
                    if (touch.State == TouchLocationState.Pressed)
                    {
                        ClickPoints.Add(touch.Position);
                    }
                    else if (touch.State == TouchLocationState.Moved)
                    {
                        TouchLocation prevTouch;
                        if (touch.TryGetPreviousLocation(out prevTouch))
                        {
                            var delta = touch.Position - prevTouch.Position;

                            // Allow some errors
                            if (delta.LengthSquared() < 2)
                            {
                                ClickPoints.Add(touch.Position);
                            }
                        }
                    }
                }
                swipeDirection = Vector2.Zero;
            }
            else if (touchPoints.Count == 1)
            {
                TouchLocation touch = touchPoints[0];

                if ((touch.State == TouchLocationState.Moved) || (touch.State == TouchLocationState.Pressed))
                {
                    TouchLocation prevTouch;

                    // Sometimes TryGetPreviousLocation can fail
                    if (touch.TryGetPreviousLocation(out prevTouch))
                    {
                        // Get swiping direction
                        swipeDirection = touch.Position - prevTouch.Position;
                    }
                    else
                    {
                        PressPoints.Add(touch.Position);
                    }
                }
                else if (touch.State == TouchLocationState.Released)
                {
                    TouchLocation prevTouch;
                    if (touch.TryGetPreviousLocation(out prevTouch))
                    {
                        var delta = touch.Position - prevTouch.Position;

                        // Allow some errors
                        if (delta.LengthSquared() < 2)
                        {
                            ClickPoints.Add(touch.Position);
                        }
                    }
                    else
                    {
                        ClickPoints.Add(touch.Position);
                    }
                }
            }
            else if (touchCollection.Count == 2)
            {
                // TODO
            }
            else
            {
                // TODO
            }

            // Update previous state
            prevTouchPoints.Clear();
            prevTouchPoints.AddRange(touchPoints);
        }
    }
}