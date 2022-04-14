using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections.Generic;

namespace BasicPlayerMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Used to Run the Game
        public DispatcherTimer dispatcherTimer = new();

        // Create Player Object
        public PlayerMaker ThisPlayer = new("Hello", 5);

        // List of BlueSquares
        public List<Rectangle> GreenSquares = new();

        // List that will be used to remove items
        public List<Rectangle> itemstoremove = new();


        // This is the thing that activates as soon as the prorgam runs
        public MainWindow()
        {
            InitializeComponent();

            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1); // The number in this repersents how fast the game runs. The larger the number, the slower it is.
            dispatcherTimer.Tick += new EventHandler(GameTimerEvent); // linking the timer event, GameTimerEvent. 
            dispatcherTimer.Start(); // starting the timer

            PlayerSpace.Focus(); // this is what will be mainly focused on for the program
        }



        // This is the code that will run every interval
        private void GameTimerEvent(object? sender, EventArgs e) // The things in the parameter is like an event handler
        {
            // Create the hitbox around the player
            // The .intersect can ONLY work with Rect and not with Rectangle.
            var PlayerHitbox = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player), Player.Width, Player.Height);


            // Move Player
            ThisPlayer.Mover(Player);

            // Check for Interactions
            CheckInteract(PlayerHitbox);

            // Check if space bar is pressed and if so, create a blue square
            CreateGreenSquare();


            // For each object in the list, remove them from the canvas
            foreach (Rectangle x in itemstoremove)
            {
                // remove them permanently from the canvas
                PlayerSpace.Children.Remove(x);

            }

        }

        
        public void CheckInteract(Rect PlayerHitbox)
        {

            // Search for every rectangle in the canvas with the name "PlayerSpace"
            // call this rectangle "Interact"
            foreach (Rectangle Interact in PlayerSpace.Children.OfType<Rectangle>())
            {
                BlueSquareInteract(Interact, PlayerHitbox);
                GreenSquareInteract(Interact, PlayerHitbox);

            }
        }

        public void BlueSquareInteract(Rectangle Interact, Rect PlayerHitbox)
        {

            // If this Rectangle has the tag "ThisThing"
            if ((string)Interact.Tag == "ThisThing")
            {

                // Create the hitbox around this object. This will be mostly used for for .Intersects with this
                var ThisThing = new Rect(Canvas.GetLeft(Interact), Canvas.GetTop(Interact), Interact.Width, Interact.Height);


                // If the Player hit box interacts with another hitbox with the tag "ThisThing"
                if (PlayerHitbox.IntersectsWith(ThisThing))
                {

                    // Creates a Color Brush
                    var ColorBrush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));

                    // The object with the tag "ThisThing" will be filled
                    Interact.Fill = ColorBrush;
                }
                else
                {
                    // Creates a Color Brush
                    var ColorBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));

                    // The object with the tag "ThisThing" will be filled
                    Interact.Fill = ColorBrush;
                }
            }
        }

        public void GreenSquareInteract(Rectangle Interact, Rect PlayerHitbox)
        {
            // For all the Green Squares in the list
            for (int i = 0; i < GreenSquares.Count; i++)
            {
                // If the Rectangle has this specific Tag
                if ((string)Interact.Tag == $"GreenSquare-{i}")
                {
                    // Create the hitbox around this object. This will be mostly used for for .Intersects with this
                    var ThisThing = new Rect(Canvas.GetLeft(Interact), Canvas.GetTop(Interact), Interact.Width, Interact.Height);


                    // If the Player hit box interacts with another hitbox with the tag "ThisThing"
                    if (PlayerHitbox.IntersectsWith(ThisThing))
                    {
                        // Add it to the remove lost
                        itemstoremove.Add(Interact);
                    }
                    else
                    {
                        // Creates a Color Brush
                        var ColorBrush = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));

                        // The object with the tag "ThisThing" will be filled
                        Interact.Fill = ColorBrush;
                    }
                }
            }
        }



        public void CreateGreenSquare()
        {

            // If the Space bar is bressed
            if (Keyboard.IsKeyDown(Key.Space))
            {

                // Get the basic paremeters of the rectangle
                Rectangle Green = new()
                {
                    Tag= $"GreenSquare-{GreenSquares.Count}",
                    Width = 25,
                    Height = 25,
                };

                // Create a random object
                Random rand = new();

                // set this based on the variables
                Canvas.SetTop(Green, rand.Next(25, 200));
                Canvas.SetLeft(Green, rand.Next(25, 200));

                // Create this rectangle onto the Canvas known as "PlayerSpace"
                PlayerSpace.Children.Add(Green);

                GreenSquares.Add(Green);

                GC.Collect(); // collect any unused resources for this game
            }


        }


    }


    public class PlayerMaker
    {
        // These are the variables that the players use
        public string name;
        public double speed;



        public PlayerMaker(string name, double speed)
        {
            this.name = name;
            this.speed = speed;

        }

        public void Mover(Rectangle Player)
        {

            // This only allows you to go in one dirrection at a time!
            // Key.Left is the left arrow key. You can change this to anything else if you want.


            // get Speed
            double speedX = 0;
            double speedY = 0;



            if (Keyboard.IsKeyDown(Key.Left))
            {

                speedX -= speed;



            }
            else if (Keyboard.IsKeyDown(Key.Right))
            {
                speedX += speed;


            }
            else if (Keyboard.IsKeyDown(Key.Up))
            {


                speedY -= speed;


            }
            else if (Keyboard.IsKeyDown(Key.Down))
            {

                speedY += speed;

            }

            //get the rectangle Player to teleprt to new position. To get this position, use Canvas.GetLeft to find the x position of the Player and adding the speed to it
            Canvas.SetLeft(Player, Canvas.GetLeft(Player) + speedX);
            Canvas.SetTop(Player, Canvas.GetTop(Player) + speedY);


        }
    }



}
