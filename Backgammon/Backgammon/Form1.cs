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
    public partial class Form1 : Form
    {
        private int[,] ChessCheck = new int[15, 15]; // The color flag of the position. ChessCheck = 1 is Black, ChessCheck = 2 is White.
        private int count = 0; // the number of chesses in the whole game
        private List<Chess> chesses = new List<Chess>();
        private Point[] Directions = new Point[4] { new Point(0, 1), new Point(1, 0), new Point(1, 1), new Point(1, -1) };

        public class Chess
        {
            public int X;  // Relative X of the Chess
            public int Y;  // Relative Y of the Chess
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

        public Form1()
        {
            InitializeComponent();
            ResizeRedraw = true;
            CreateMainMenu();
            BackColor = Color.LightBlue;
            DoubleBuffered = true;
            iniChessCheck(ChessCheck);
        }

        private void iniChessCheck(int[ , ] ChessCheck)
        {
            for (int i = ClientSize.Width / 2 - this.SquareSize * 7; i <= ClientSize.Width / 2 + this.SquareSize * 7; i += SquareSize)
            {
                for (int j = ClientSize.Height / 2 - this.SquareSize * 7; j <= ClientSize.Height / 2 + this.SquareSize * 7; j += SquareSize)
                {
                    ChessCheck[(j - (ClientSize.Height / 2 - this.SquareSize * 7)) / SquareSize,
                        (i - (ClientSize.Width / 2 - this.SquareSize * 7)) / SquareSize] = 0;
                }
            }
        }
        
        public void CreateMainMenu()
        {
            MainMenu mainMenu = new MainMenu();
            MenuItem menuItem1 = new MenuItem();
            MenuItem menuItem2 = new MenuItem();
            MenuItem menuItem3 = new MenuItem();
            MenuItem menuItem4 = new MenuItem();
            MenuItem menuItem5 = new MenuItem();
            
            menuItem1.Text = "Game";
            menuItem2.Text = "Chess";
            menuItem3.Text = "Restart";
            menuItem4.Text = "Withdraw";
            menuItem5.Text = "Exit";

            mainMenu.MenuItems.Add(menuItem1);
            mainMenu.MenuItems.Add(menuItem2);
            menuItem1.MenuItems.Add(menuItem3);
            menuItem1.MenuItems.Add(menuItem4);
            menuItem1.MenuItems.Add(menuItem5);

            menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            Menu = mainMenu;
        }

        private void menuItem3_Click(object sender, System.EventArgs e)
        {
            chesses.Clear();
            count= 0;
            iniChessCheck(ChessCheck);
            Invalidate();
        }

        private void menuItem4_Click(object sender, System.EventArgs e)
        {
            ChessCheck[chesses.Last().Y, chesses.Last().X] = 0;
            chesses.Remove(chesses.Last());
            count--;
            Invalidate(); 
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Paint Chessboard
            Pen RedPen = new Pen(Brushes.Red); // pen class derives from idispose class.
            for (int i = 0; i < 15; i++)
            {
                    Point pointLeft = new Point(this.StartPositionX, this.StartPositionY + SquareSize * i);
                    Point pointRight = new Point(this.StartPositionX + this.SquareSize * 14, this.StartPositionY + this.SquareSize * i);
                    Point pointUp = new Point(this.StartPositionX + this.SquareSize * i, this.StartPositionY);
                    Point pointDown= new Point(this.StartPositionX + this.SquareSize * i, this.StartPositionY + this.SquareSize * 14);
                    e.Graphics.DrawLine(RedPen, pointLeft, pointRight);
                    e.Graphics.DrawLine(RedPen, pointUp, pointDown);
            }
            int radius = this.SquareSize / 6;
            DrawPoint(e.Graphics, this.ClientSize.Width / 2, this.ClientSize.Height / 2, radius);
            DrawPoint(e.Graphics, this.ClientSize.Width / 2 - this.SquareSize * 4, this.ClientSize.Height / 2 - this.SquareSize * 4, radius);
            DrawPoint(e.Graphics, this.ClientSize.Width / 2 - this.SquareSize * 4, this.ClientSize.Height / 2 + this.SquareSize * 4, radius);
            DrawPoint(e.Graphics, this.ClientSize.Width / 2 + this.SquareSize * 4, this.ClientSize.Height / 2 - this.SquareSize * 4, radius);
            DrawPoint(e.Graphics, this.ClientSize.Width / 2 + this.SquareSize * 4, this.ClientSize.Height / 2 + this.SquareSize * 4, radius);
            
            //paint chesses
            foreach (Chess chess in chesses)
            {
                    Rectangle recChess = new Rectangle
                    (
                        chess.X * SquareSize + StartPositionX - this.SquareSize / 3,
                        chess.Y * SquareSize + StartPositionY - this.SquareSize / 3,
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

        private void DrawPoint(Graphics graphics, int startPointX, int startPointY, int radius)
        {
            int LeftX = startPointX - radius;
            int LeftY = startPointY - radius;
            Rectangle rec = new Rectangle(LeftX, LeftY, radius * 2, radius * 2);
            graphics.FillEllipse(Brushes.Red, rec);
        }
     
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Chess chess = new Chess();
            chess.X = (int)(e.X + SquareSize / 2 - StartPositionX) / SquareSize;
            chess.Y = (int)(e.Y + SquareSize / 2 - StartPositionY) / SquareSize;
            if ((chess.X >= 0) && (chess.X <= 14) && (chess.Y >= 0) && (chess.Y <= 14) && ChessCheck[chess.Y, chess.X] == 0)
            {
                count++;
                chess.times = count;
                chesses.Add(chess);
                Invalidate();
                ChessCheck[chess.Y, chess.X] = (chess.times % 2 != 0) ? 1 : 2;
                if (isWin(chess, ChessCheck))
                {
                    if (ChessCheck[chess.Y, chess.X] == 1)
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
        
        private bool isWin(Chess chess, int[ , ] ChessCheck)
        {
            int curColor = ChessCheck[chess.Y, chess.X];  //Color of the current point
            for (int i = 0; i < 4; i++)
            {
                int num = 1;
                for (int X = chess.X + Directions[i].X, Y = chess.Y + Directions[i].Y;
                    X >= 0 && X <= 14 && Y >= 0 && Y <= 14 && ChessCheck[Y, X] == curColor;
                    X += Directions[i].X, Y += Directions[i].Y)
                {
                    num++;
                }
                for (int X = chess.X - Directions[i].X, Y = chess.Y - Directions[i].Y;
                    X >= 0 && X <= 14 && Y >= 0 && Y <= 14 && ChessCheck[Y, X] == curColor;
                    X -= Directions[i].X, Y -= Directions[i].Y)
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

