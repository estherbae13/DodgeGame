using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DodgeGame
{
    public partial class Form1 : Form
    {
        //global variables
        Rectangle hero = new Rectangle(50, 185, 25, 25);
        int heroSpeed = 6;

        Rectangle end = new Rectangle(599, 1, 5, 400);

        List<Rectangle> obstacleL = new List<Rectangle>();
        List<Rectangle> obstacleR = new List<Rectangle>();

        int obstacleHeight = 70;
        int obstacleWidth = 25;
        int obstacleLX = 200;
        int obstacleRX = 450;

        int obstacleLSpeed = 8;
        int obstacleRSpeed = -7;

        int leftCounter = 0;
        int rightCounter = 0;

        bool wDown = false;
        bool aDown = false;
        bool sDown = false;
        bool dDown = false;

        SolidBrush blueBrush = new SolidBrush(Color.CornflowerBlue);
        SolidBrush greenBrush = new SolidBrush(Color.SeaGreen);

        string gameState = "waiting";

        public Form1()
        {
            InitializeComponent();
        }

        public void GameInitialize()
        {
            titleLabel.Text = "";
            subTitleLabel.Text = "";

            gameTimer.Enabled = true;
            gameState = "running";
            leftCounter = 0;
            rightCounter = 0;
            obstacleL.Clear();
            obstacleR.Clear();

            hero.X = 50;
            hero.Y = 185;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.Space:
                    if (gameState == "waiting" || gameState == "over" || gameState == "win")
                    {
                        GameInitialize();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "over" || gameState == "win")
                    {
                        Application.Exit();
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move hero 
            if (wDown == true && hero.Y > 0)
            {
                hero.Y -= heroSpeed;
            }

            if (aDown == true && hero.X > 0)
            {
                hero.X -= heroSpeed;
            }

            if (sDown == true && hero.Y < this.Height - hero.Height)
            {
                hero.Y += heroSpeed;
            }

            if (dDown == true && hero.X < this.Width - hero.Width)
            {
                hero.X += heroSpeed;
            }

            //move left obstacle down screen
            for (int i = 0; i < obstacleL.Count(); i++)
            {
                int y = obstacleL[i].Y + obstacleLSpeed;

                //replace the rectangle in the list with updated one using new y
                obstacleL[i] = new Rectangle(obstacleL[i].X, y, obstacleWidth, obstacleHeight);
            }

            //move right obstacle up screen
            for (int i = 0; i < obstacleR.Count(); i++)
            {
                //find the new postion of y based on speed
                int y = obstacleR[i].Y + obstacleRSpeed;

                //replace the rectangle in the list with updated one using new y
                obstacleR[i] = new Rectangle(obstacleR[i].X, y, obstacleWidth, obstacleHeight);
            }

            //check to send more obstacles
            leftCounter++;

            if (leftCounter % 20 == 0)
            {
                obstacleL.Add(new Rectangle(obstacleLX, 0 - obstacleHeight, obstacleWidth, obstacleHeight));
            }

            rightCounter++;

            if (rightCounter % 20 == 0)
            {
                obstacleR.Add(new Rectangle(obstacleRX, this.Height, obstacleWidth, obstacleHeight));
            }

            //remove obstacles when they get to the bottom or top
            for (int i = 0; i < obstacleL.Count(); i++)
            {
                if (obstacleL[i].Y > this.Height)
                {
                    obstacleL.RemoveAt(i);
                }
            }

            for (int i = 0; i < obstacleR.Count(); i++)
            {
                if (obstacleR[i].Y > this.Height)
                {
                    obstacleR.RemoveAt(i);
                }
            }

            //stop if hero collides with an obstacle?!?!
            for (int i = 0; i < obstacleL.Count(); i++)
            {
                if (hero.IntersectsWith(obstacleL[i]))
                {
                    gameTimer.Enabled = false;
                    gameState = "over";
                }
            }

            for (int i = 0; i < obstacleR.Count(); i++)
            {
                if (hero.IntersectsWith(obstacleR[i]))
                {
                    gameTimer.Enabled = false;
                    gameState = "over";
                }
            }

            //stop if hero makes it to the end!!
            if (hero.IntersectsWith(end))
            {
                gameTimer.Enabled = false;
                gameState = "win";
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "waiting")
            {
                titleLabel.Text = "dodge runner 3000!!";
                subTitleLabel.Text = "Press Space Bar to Start or Escape to Exit";
            }

            else if (gameState == "running")
            {
                //draw hero
                e.Graphics.FillRectangle(blueBrush, hero);

                //draw obstacles 
                for (int i = 0; i < obstacleL.Count(); i++)
                {
                    e.Graphics.FillRectangle(greenBrush, obstacleL[i]);
                }

                for (int i = 0; i < obstacleR.Count(); i++)
                {
                    e.Graphics.FillRectangle(greenBrush, obstacleR[i]);
                }
            }
            else if (gameState == "over")
            {
                titleLabel.Text = "you lose :(";
                subTitleLabel.Text = "\nPress Space Bar to Start or Escape to Exit";
            }
            else if (gameState == "win")
            {
                titleLabel.Text = "you win :D";
                subTitleLabel.Text = "\nPress Space Bar to Start or Escape to Exit";
            }
        }
    }
}
