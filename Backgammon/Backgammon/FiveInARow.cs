using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Backgammon
{
    public partial class Form_Five : Form
    {
        private int[,] chessCheck = new int[15, 15]; // The color flag of the position. ChessCheck = 1 is Black, ChessCheck = 2 is White.
        private int count = 0; // the number of chesses in the whole game
        private List<Chess> chesses = new List<Chess>();
        private Point[] directions = new Point[4] { 
            new Point(0, 1), 
            new Point(1, 0),
            new Point(1, 1), 
            new Point(1, -1)
        };
        public const int SquareNumber = 14;
        public class Chess
        {
            public int x;  // Relative X of the Chess
            public int y;  // Relative Y of the Chess
            public int times; //the number of times of chess.
        }

        public int SquareSize
        {
            get
            {
                return Math.Min(this.ClientSize.Height / 16, this.ClientSize.Width / 16);
            }
        }
        
        public int StartPositionX
        {
            get 
            {
                return this.ClientSize.Width / 2 - this.SquareSize * 7; 
            }
        }

        public int StartPositionY
        {
            get 
            {
                return this.ClientSize.Height / 2 - this.SquareSize * 7;
            }
        }

        public Form_Five()
        {
            InitializeComponent();
            ResizeRedraw = true;
            CreateMainMenu();
            BackColor = Color.LightBlue;
            DoubleBuffered = true;
            iniChessCheck();
        }

        private void iniChessCheck()
        {
            for (int i = 0; i <= 14; i++)
            {
                for (int j = 0; j <= 14; j++)
                {
                    chessCheck[i, j] = 0;
                }
            }
        }
        
        public void CreateMainMenu()
        {
            MainMenu mainMenu = new MainMenu();
            MenuItem gameMenu = new MenuItem();
            MenuItem restartMenu = new MenuItem();
            MenuItem withdrawMenu = new MenuItem();
            MenuItem exitMenu = new MenuItem();
            
            gameMenu.Text = "Game";
            restartMenu.Text = "Restart";
            withdrawMenu.Text = "Withdraw";
            exitMenu.Text = "Exit";

            mainMenu.MenuItems.Add(gameMenu);
            gameMenu.MenuItems.Add(restartMenu);
            gameMenu.MenuItems.Add(withdrawMenu);

            restartMenu.Click += new System.EventHandler(this.restartMenu_Click);
            withdrawMenu.Click += new System.EventHandler(this.withdrawMenu_Click);
            Menu = mainMenu;
        }

        private void restartMenu_Click(object sender, System.EventArgs e)
        {
            chesses.Clear();
            count= 0;
            iniChessCheck();
            Invalidate();
        }

        private void withdrawMenu_Click(object sender, System.EventArgs e)
        {
            chessCheck[chesses.Last().y, chesses.Last().x] = 0;
            chesses.Remove(chesses.Last());
            count--;
            Invalidate(); 
        }

        private void Form_Load(object sender, EventArgs e)
        {

        }

        private void Form_Paint(object sender, PaintEventArgs e)
        {
            // Paint Chessboard
            Pen RedPen = new Pen(Brushes.Red); // pen class derives from idispose class.
            for (int i = 0; i < SquareNumber + 1; i++)
            {
                    Point pointLeft = new Point(StartPositionX, StartPositionY + SquareSize * i);
                    Point pointRight = new Point(StartPositionX + SquareSize * SquareNumber, StartPositionY + SquareSize * i);
                    Point pointUp = new Point(StartPositionX + SquareSize * i, StartPositionY);
                    Point pointDown= new Point(StartPositionX + SquareSize * i, StartPositionY + SquareSize * SquareNumber);
                    e.Graphics.DrawLine(RedPen, pointLeft, pointRight);
                    e.Graphics.DrawLine(RedPen, pointUp, pointDown);
            }
            int radius = SquareSize / 6;

            DrawStarPoint(e.Graphics, 7, 7, radius);
            DrawStarPoint(e.Graphics, 3, 3, radius);
            DrawStarPoint(e.Graphics, 3, 11, radius);
            DrawStarPoint(e.Graphics, 11, 3, radius);
            DrawStarPoint(e.Graphics, 11, 11, radius);

            //paint chesses
            foreach (Chess chess in chesses)
            {
                    Rectangle recChess = new Rectangle
                    (
                        chess.x * SquareSize + StartPositionX - this.SquareSize / 3,
                        chess.y * SquareSize + StartPositionY - this.SquareSize / 3,
                        this.SquareSize / 3 * 2,
                        this.SquareSize / 3 * 2
                    );
                    if (chess.times % 2 != 0)
                    {
                        e.Graphics.FillEllipse(Brushes.Black, recChess);
                    }
                    else
                    {
                        e.Graphics.FillEllipse(Brushes.White, recChess);
                    }
            }
        }

        private void DrawStarPoint(Graphics graphics, int relativeX, int relativeY, int radius)
        {
            int LeftX = StartPositionX + SquareSize * relativeX - radius;
            int LeftY = StartPositionY + SquareSize * relativeY - radius;
            Rectangle rec = new Rectangle(LeftX, LeftY, radius * 2, radius * 2);
            graphics.FillEllipse(Brushes.Red, rec);
        }

        private void Form_MouseClick(object sender, MouseEventArgs e)
        {
            Chess chess = new Chess();
            chess.x = (int)(e.X + SquareSize / 2 - StartPositionX) / SquareSize;
            chess.y = (int)(e.Y + SquareSize / 2 - StartPositionY) / SquareSize;
            if ((chess.x >= 0) && (chess.x <= SquareNumber) && (chess.y >= 0) && (chess.y <= SquareNumber) && chessCheck[chess.y, chess.x] == 0)
            {
                count++;
                chess.times = count;
                chesses.Add(chess);
                Invalidate();
                chessCheck[chess.y, chess.x] = (chess.times % 2 != 0) ? 1 : 2;
                if (isWin(chess))
                {
                    if (chessCheck[chess.y, chess.x] == 1)
                    {
                        MessageBox.Show("Black Win! Game End!");
                    }
                    else
                    {
                        MessageBox.Show("White Win! Game End!");
                    }
                }
            }
        }
        
        private bool isWin(Chess chess)
        {
            int curColor = chessCheck[chess.y, chess.x];  //Color of the current point
            for (int i = 0; i < 4; i++)
            {
                int num = 1;
                for (int X = chess.x + directions[i].X, Y = chess.y + directions[i].Y;
                    X >= 0 && X <= SquareNumber && Y >= 0 && Y <= SquareNumber && chessCheck[Y, X] == curColor;
                    X += directions[i].X, Y += directions[i].Y)
                {
                    num++;
                }

                for (int X = chess.x - directions[i].X, Y = chess.y - directions[i].Y;
                    X >= 0 && X <= SquareNumber && Y >= 0 && Y <= SquareNumber && chessCheck[Y, X] == curColor;
                    X -= directions[i].X, Y -= directions[i].Y)
                {
                    num++;
                }

                if (num == 5)
                {
                    return true;
                }
            }

            return false;
        }  
    }
}

