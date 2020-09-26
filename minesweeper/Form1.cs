using minesweeper.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace minesweeper
{
    public partial class Form1 : Form
    {
        PictureBox[,] square;
        Minesweeper game;
        public Form1()
        {
            InitializeComponent();

            int column = Screen.PrimaryScreen.WorkingArea.Width / 70;
            int row = (Screen.PrimaryScreen.WorkingArea.Height - 70) / 70;
            this.Size = new Size(column * 70 + 20, (row + 1) * 70 + 40);//set window size

            game = new Minesweeper(column, row);

            this.Controls.Add(new PictureBox //show mine icon on top
            {
                Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - 70, 2),
                Size = new Size(68, 68),
                BackgroundImage = Image.FromFile("mine9.jpg"),
                BackgroundImageLayout = ImageLayout.Zoom
            });
            this.Controls.Add(new Label //show mine number on top
            {
                Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2, 2),
                Text = String.Format("{0}", game.totalMine),
                Font = new Font("Arial", 45, FontStyle.Bold),
                AutoSize = true
            });

            square = new PictureBox[column, row];

            for (int x = 0; x < column; x++)
            {
                for (int y = 0; y < row; y++)
                {
                    square[x, y] = new PictureBox
                    {
                        Location = new Point((70 * x) + 2, (70 * y) + 72),
                        Size = new Size(68, 68),
                        Tag = new pictureBoxTag(x, y),
                        BackgroundImage = Image.FromFile("inactive.jpg"),
                        BackgroundImageLayout = ImageLayout.Zoom,
                        BackColor = Color.LightGray
                    };
                    square[x, y].MouseClick += new MouseEventHandler(mouseClickEvent);
                    this.Controls.Add(square[x, y]);
                }
            }
        }
        struct pictureBoxTag //put coordinates into tag of picturebox
        {
            public int pictureBox_x;
            public int pictureBox_y;
            public pictureBoxTag(int x, int y)
            {
                pictureBox_x = x;
                pictureBox_y = y;
                return;
            }
        }
        void mouseClickEvent(object sender, MouseEventArgs e)
        {
            pictureBoxTag tag = (pictureBoxTag)((PictureBox)sender).Tag;

            switch (e.Button)//mouse click button check
            {
                case MouseButtons.Left://left click
                    List<Minesweeper.updateUI> leftClickUpdate = game.leftClick(tag.pictureBox_x, tag.pictureBox_y);//get list of ui update
                    if(leftClickUpdate.Count!=0 && leftClickUpdate[0].newState == 9)//check game over
                    {
                        square[leftClickUpdate[0].ui_x, leftClickUpdate[0].ui_y].BackgroundImage = setPicture(leftClickUpdate[0].newState);
                        if (MessageBox.Show("You lose!", "", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                            Application.Restart();
                        else
                            this.Close();
                        break;
                    }
                    for (int i = 0; i < leftClickUpdate.Count(); i++)//update ui
                    {
                        square[leftClickUpdate[i].ui_x, leftClickUpdate[i].ui_y].BackgroundImage = setPicture(leftClickUpdate[i].newState);
                        square[leftClickUpdate[i].ui_x, leftClickUpdate[i].ui_y].MouseClick -= mouseClickEvent;
                    }
                    if(game.checkVictory())//check victory
                    {
                        if (MessageBox.Show("You win!", "", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                            Application.Restart();
                        else
                            this.Close();
                    }
                    break;//end left click
                case MouseButtons.Right://right click (flag / unflag)
                    PictureBox pictureBox = (PictureBox)sender;
                    if (game.rightClick(tag.pictureBox_x, tag.pictureBox_y))
                    {
                        pictureBox.BackgroundImage = Image.FromFile("flag.jpg");
                    }
                    else
                    {
                        pictureBox.BackgroundImage = Image.FromFile("inactive.jpg");
                    }
                    break;
            }
        }
        Image setPicture(int tag)
        {
            switch (tag)
            {
                case 0:
                    return null;
                    break;
                case 1:
                    return Image.FromFile("mine1.jpg");
                    break;
                case 2:
                    return Image.FromFile("mine2.jpg");
                    break;
                case 3:
                    return Image.FromFile("mine3.jpg");
                    break;
                case 4:
                    return Image.FromFile("mine4.jpg");
                    break;
                case 5:
                    return Image.FromFile("mine5.jpg");
                    break;
                case 6:
                    return Image.FromFile("mine6.jpg");
                    break;
                case 7:
                    return Image.FromFile("mine7.jpg");
                    break;
                case 8:
                    return Image.FromFile("mine8.jpg");
                    break;
                default:
                    return Image.FromFile("mine9.jpg");
                    break;

            }
        }
    }
}
