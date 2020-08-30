using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace minesweeper
{
    public partial class Form1 : Form
    {
        PictureBox[,] square;
        int column;
        int row;
        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            column = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 70;
            row = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 70;
            int[,] mine = new int[column, row];
            Random rnd = new Random();
            for (int totalMine = rnd.Next(((column * row) / 10), ((column * row) / 8)); totalMine > 0;) //setMine
            {
                Console.WriteLine("totalMine = {0}", totalMine);
                int x = rnd.Next(0, column - 1);
                int y = rnd.Next(0, row - 1);
                if (mine[x, y] == 0)
                {
                    mine[x, y] = 9;
                    totalMine--;
                }
            }
            square = new PictureBox[column, row];
            for (int x = 0; x < column - 1; x++)
            {
                for (int y = 0; y < row - 1; y++)
                {
                    if (mine[x, y] < 9)
                    {
                        int nearbyMine = 0;
                        for (int i = -1; i < 2; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                if (!(x + i < 0 || x + i > column - 1 || y + j < 0 || y + j > row - 1))
                                {
                                    if (mine[x + i, y + j] > 8)
                                    {
                                        nearbyMine++;
                                    }
                                }
                            }
                        }
                        mine[x, y] = nearbyMine;
                    }
                    square[x, y] = new PictureBox();
                    square[x, y].Location = new System.Drawing.Point((70 * x) + 2, (70 * y) + 2);
                    square[x, y].Size = new System.Drawing.Size(68, 68);
                    square[x, y].BackColor = Color.Azure;

                    pictureBoxTag tag = new pictureBoxTag();
                    tag.init(x, y, mine[x, y]);
                    square[x, y].Tag = tag;
                    square[x, y].MouseClick += new MouseEventHandler(mouseClickEvent);
                    square[x, y].BackgroundImage = System.Drawing.Image.FromFile("inactive.jpg");
                    square[x, y].BackgroundImageLayout = ImageLayout.Zoom;
                    this.Controls.Add(square[x, y]);

                }
            }
        }
        struct pictureBoxTag
        {
            public int pictureBox_x;
            public int pictureBox_y;
            public int mineNum;

            public void init(int x, int y, int num)
            {
                pictureBox_x = x;
                pictureBox_y = y;
                mineNum = num;
                return;
            }
        }

        void mouseClickEvent(object sender, MouseEventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            pictureBoxTag tag = (pictureBoxTag)pictureBox.Tag;
            int mineNum = tag.mineNum;
            Console.WriteLine("{0} {1}", tag.pictureBox_x, tag.pictureBox_y);
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (pictureBox.BackColor != Color.Yellow) //yellow means flag
                    {
                        switch (mineNum)
                        {
                            case 0: //no nearby mines
                                pictureBox.BackColor = Color.LightGray;
                                pictureBox.BackgroundImage = null;
                                findZeros(tag.pictureBox_x, tag.pictureBox_y);
                                break;
                            case 1:
                                pictureBox.BackColor = Color.LightGray;
                                pictureBox.BackgroundImage = System.Drawing.Image.FromFile("mine1.jpg");
                                pictureBox.BackgroundImageLayout = ImageLayout.Zoom;
                                break;
                            case 2:
                                pictureBox.BackColor = Color.LightGray;
                                pictureBox.BackgroundImage = System.Drawing.Image.FromFile("mine2.jpg");
                                pictureBox.BackgroundImageLayout = ImageLayout.Zoom;
                                break;
                            case 3:
                                pictureBox.BackColor = Color.LightGray;
                                pictureBox.BackgroundImage = System.Drawing.Image.FromFile("mine3.jpg");
                                pictureBox.BackgroundImageLayout = ImageLayout.Zoom;
                                break;
                            case 4:
                                pictureBox.BackColor = Color.LightGray;
                                pictureBox.BackgroundImage = System.Drawing.Image.FromFile("mine4.jpg");
                                pictureBox.BackgroundImageLayout = ImageLayout.Zoom;
                                break;
                            case 5:
                                pictureBox.BackColor = Color.LightGray;
                                pictureBox.BackgroundImage = System.Drawing.Image.FromFile("mine5.jpg");
                                pictureBox.BackgroundImageLayout = ImageLayout.Zoom;
                                break;
                            case 6:
                                pictureBox.BackColor = Color.LightGray;
                                pictureBox.BackgroundImage = System.Drawing.Image.FromFile("mine6.jpg");
                                pictureBox.BackgroundImageLayout = ImageLayout.Zoom;
                                break;
                            case 7:
                                pictureBox.BackColor = Color.LightGray;
                                pictureBox.BackgroundImage = System.Drawing.Image.FromFile("mine7.jpg");
                                pictureBox.BackgroundImageLayout = ImageLayout.Zoom;
                                break;
                            case 8:
                                pictureBox.BackColor = Color.LightGray;
                                pictureBox.BackgroundImage = System.Drawing.Image.FromFile("mine8.jpg");
                                pictureBox.BackgroundImageLayout = ImageLayout.Zoom;
                                break;
                            case 9:
                                pictureBox.BackColor = Color.Red;
                                pictureBox.BackgroundImage = System.Drawing.Image.FromFile("mine9.jpg");
                                pictureBox.BackgroundImageLayout = ImageLayout.Zoom;
                                MessageBox.Show("You lose!");
                                Application.Restart();
                                break;
                        }
                    }
                    break;
                case MouseButtons.Right:
                    if (pictureBox.BackColor == Color.Azure)
                    {
                        pictureBox.BackColor = Color.Yellow;
                        pictureBox.BackgroundImage = System.Drawing.Image.FromFile("flag.jpg");
                        pictureBox.BackgroundImageLayout = ImageLayout.Zoom;
                    }
                    else if (pictureBox.BackColor == Color.Yellow)
                    {
                        pictureBox.BackColor = Color.Azure;
                        pictureBox.BackgroundImage = System.Drawing.Image.FromFile("inactive.jpg");
                        pictureBox.BackgroundImageLayout = ImageLayout.Zoom;
                    }
                    break;
            }
        }
        void findZeros(int x, int y)
        {
            if (x + 1 < column - 1 && square[x + 1, y].BackColor == Color.Azure)
            {
                pictureBoxTag tag = (pictureBoxTag)square[x + 1, y].Tag;
                if (tag.mineNum == 0)
                {
                    square[x + 1, y].BackColor = Color.LightGray;
                    square[x + 1, y].BackgroundImage = null;
                    findZeros(x + 1, y);
                }
                else if (tag.mineNum < 9)
                {
                    square[x + 1, y].BackColor = Color.LightGray;
                    square[x + 1, y].BackgroundImage = setPicture(tag.mineNum);
                }
            }
            if (x - 1 > -1 && square[x - 1, y].BackColor == Color.Azure)
            {
                pictureBoxTag tag = (pictureBoxTag)square[x - 1, y].Tag;
                if (tag.mineNum == 0)
                {
                    square[x - 1, y].BackColor = Color.LightGray;
                    square[x - 1, y].BackgroundImage = null;
                    findZeros(x - 1, y);
                }
                else if (tag.mineNum < 9)
                {
                    square[x - 1, y].BackColor = Color.LightGray;
                    square[x - 1, y].BackgroundImage = setPicture(tag.mineNum);
                }
            }
            if (y + 1 < row - 1 && square[x, y + 1].BackColor == Color.Azure)
            {
                pictureBoxTag tag = (pictureBoxTag)square[x, y + 1].Tag;
                if (tag.mineNum == 0)
                {
                    square[x, y + 1].BackColor = Color.LightGray;
                    square[x, y + 1].BackgroundImage = null;
                    findZeros(x, y + 1);
                }
                else if (tag.mineNum < 9)
                {
                    square[x, y + 1].BackColor = Color.LightGray;
                    square[x, y + 1].BackgroundImage = setPicture(tag.mineNum);
                }
            }
            if (y - 1 > -1 && square[x, y - 1].BackColor == Color.Azure)
            {
                pictureBoxTag tag = (pictureBoxTag)square[x, y - 1].Tag;
                if (tag.mineNum == 0)
                {
                    square[x, y - 1].BackColor = Color.LightGray;
                    square[x, y - 1].BackgroundImage = null;
                    findZeros(x, y - 1);
                }
                else if (tag.mineNum < 9)
                {
                    square[x, y - 1].BackColor = Color.LightGray;
                    square[x, y - 1].BackgroundImage = setPicture(tag.mineNum);
                }
            }
            return;
        }

        Image setPicture(int mine)
        {
            switch (mine)
            {
                case 1:
                    return System.Drawing.Image.FromFile("mine1.jpg");
                    break;
                case 2:
                    return System.Drawing.Image.FromFile("mine2.jpg");
                    break;
                case 3:
                    return System.Drawing.Image.FromFile("mine3.jpg");
                    break;
                case 4:
                    return System.Drawing.Image.FromFile("mine4.jpg");
                    break;
                case 5:
                    return System.Drawing.Image.FromFile("mine5.jpg");
                    break;
                case 6:
                    return System.Drawing.Image.FromFile("mine6.jpg");
                    break;
                case 7:
                    return System.Drawing.Image.FromFile("mine7.jpg");
                    break;
                case 8:
                    return System.Drawing.Image.FromFile("mine8.jpg");
                    break;
                default:
                    return System.Drawing.Image.FromFile("mine9.jpg");
                    break;

            }
        }
    }
}
