using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace minesweeper.Properties
{
    class Minesweeper
    {
        struct mine
        {
            public int tagNum;//9 means mine
            public bool flag;//flag: true, unflag: false
            public bool exposed;//clicked: true, unclicked: false
        }

        public struct updateUI
        {
            public int ui_x;
            public int ui_y;
            public int newState;//same with tagNum

            public updateUI(int x, int y, int state)
            {
                ui_x = x;
                ui_y = y;
                newState = state;
            }
        }

        mine[,] mineArray;//game map
        int victoryCountDown;//victory when 0
        public int totalMine;//number of mine in total
        public Minesweeper(int column, int row)
        {
            mineArray = new mine[column, row];
            Random rnd = new Random();
            totalMine = rnd.Next(((column * row) / 10), ((column * row) / 8));//random number of mine
            victoryCountDown = column * row - totalMine;
            Console.WriteLine("Mine count: {0}", totalMine);
            Console.WriteLine("Col {0}*Row {1}: {2}", column, row, column * row);
            Console.WriteLine("victoryCountDown: {0}", victoryCountDown);
            for (int mineGenerate = totalMine; mineGenerate > 0;) //generate mines
            {
                int x = rnd.Next(0, column - 1);
                int y = rnd.Next(0, row - 1);
                if (mineArray[x, y].tagNum == 0)
                {
                    mineArray[x, y].tagNum = 9;
                    mineGenerate--;
                }
            }

            for (int x = 0; x < column; x++)//calculate tagNum of remaining squares (non mine areas)
            {
                for (int y = 0; y < row; y++)
                {
                    if (mineArray[x, y].tagNum < 9)
                    {
                        int nearbyMine = 0;
                        for (int i = -1; i < 2; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                if (!(x + i < 0 || x + i > column - 1 || y + j < 0 || y + j > row - 1))
                                {
                                    if (mineArray[x + i, y + j].tagNum == 9)
                                    {
                                        nearbyMine++;
                                    }
                                }
                            }
                        }
                        mineArray[x, y].tagNum = nearbyMine;
                    }
                }
            }
        }
        public List<updateUI> leftClick(int x, int y)//left click event
        {
            List<updateUI> ui = new List<updateUI>();
            if (!mineArray[x, y].flag && !mineArray[x, y].exposed)
            {
                ui.Add(new updateUI(x, y, mineArray[x, y].tagNum));
                mineArray[x, y].exposed = true;
                victoryCountDown--;
                if (mineArray[x, y].tagNum == 0)//expose nearby tiles when 0
                {
                    ui.AddRange(findZeros(x, y));
                }
            }
            return ui;
        }
        public bool rightClick(int x, int y)//flag / unflag
        {
            mineArray[x, y].flag = !mineArray[x, y].flag;
            return mineArray[x, y].flag;
        }
        List<updateUI> findZeros(int x, int y)//search nearby tiles until non zero is found
        {
            List<updateUI> ui = new List<updateUI>();
            int column = mineArray.GetLength(0);
            int row = mineArray.GetLength(1);

            if (x + 1 < column && !mineArray[x + 1, y].flag && !mineArray[x + 1, y].exposed)
            {
                ui.Add(new updateUI(x + 1, y, mineArray[x + 1, y].tagNum));
                mineArray[x + 1, y].exposed = true;
                victoryCountDown--;
                if (mineArray[x + 1, y].tagNum == 0)
                    ui.AddRange(findZeros(x + 1, y));
            }
            if (x - 1 > -1 && !mineArray[x - 1, y].flag && !mineArray[x - 1, y].exposed)
            {
                ui.Add(new updateUI(x - 1, y, mineArray[x - 1, y].tagNum));
                mineArray[x - 1, y].exposed = true;
                victoryCountDown--;
                if (mineArray[x - 1, y].tagNum == 0)
                    ui.AddRange(findZeros(x - 1, y));
            }
            if (y + 1 < row && !mineArray[x, y + 1].flag && !mineArray[x, y + 1].exposed)
            {
                ui.Add(new updateUI(x, y + 1, mineArray[x, y + 1].tagNum));
                mineArray[x, y + 1].exposed = true;
                victoryCountDown--;
                if (mineArray[x, y + 1].tagNum == 0)
                    ui.AddRange(findZeros(x, y + 1));
            }
            if (y - 1 > -1 && !mineArray[x, y - 1].flag && !mineArray[x, y - 1].exposed)
            {
                ui.Add(new updateUI(x, y - 1, mineArray[x, y - 1].tagNum));
                mineArray[x, y - 1].exposed = true;
                victoryCountDown--;
                if (mineArray[x, y - 1].tagNum == 0)
                    ui.AddRange(findZeros(x, y - 1));
            }
            return ui;
        }
        public bool checkVictory()//return victory is true when countdown equals zero
        {
            Console.WriteLine("Victory Count Down: {0}", victoryCountDown);
            if (victoryCountDown == 0)
                return true;
            else
                return false;
        }

    }
}
