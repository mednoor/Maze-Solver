using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mazes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        char[,] map;

        Point _start = new Point();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void browseFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog _openFileDialog = new OpenFileDialog();

            if (_openFileDialog.ShowDialog() == true)
            {
                var data = File.ReadAllLines(_openFileDialog.FileName);

                string firstLine = data[0];
                string[] gridSize = firstLine.Split(' ');

                int width = Convert.ToInt32(gridSize[0]);
                int height = Convert.ToInt32(gridSize[1]);
                Debug.WriteLine("Width: " + width);
                Debug.WriteLine("Height: " + height);


                string secondLine = data[1];
                string[] startLocation = secondLine.Split(' ');

                int startX = Convert.ToInt32(startLocation[0]);
                int startY = Convert.ToInt32(startLocation[1]);
                Debug.WriteLine("startX: " + startX);
                Debug.WriteLine("startY: " + startY);

                string thirdLine = data[2];
                string[] endLocation = thirdLine.Split(' ');

                int endX = Convert.ToInt32(endLocation[0]);
                int endY = Convert.ToInt32(endLocation[1]);
                Debug.WriteLine("endX: " + endX);
                Debug.WriteLine("endY: " + endY);

                var list = new List<string>(data);
                list.RemoveRange(0, 3);

                string str = string.Join("", list.ToArray());
                str = str.Replace(" ", String.Empty);

                map = new char[height, width];

                

                for (int y = 0; y < map.GetLength(0); y++)
                {
                    for (int x = 0; x < map.GetLength(1); x++)
                    {
                        map[y, x] = str[x + y * width];

                        if (map[y,x] == '1')
                        {
                            map[y, x] = '#';
                        }
                        else if (map[y, x] == '0')
                        {
                            map[y, x] = ' ';
                        }
                    }
                }
                map[startY, startX] = 'S';
                map[endY, endX] = 'E';

                solveMaze(startX, startY);
            }
            
        }

        public bool solveMaze(int x, int y)
        {
            output();
            Debug.WriteLine("");
            if (reachedEnd(x, y))
            {
                return true;
            }
            else if (deadEnd(x,y))
            {
                _start.X = x;
                _start.Y = y;
                return false;
            }
            else
            {
                // check directions
                map[y,x] = 'X'; // mark path
                _start.Y = y;
                _start.X = x;

                if (y-1 >= 0 && solveMaze(x,y-1))
                {
                    return true;
                }
                else if (x+1 < map.GetLength(1) && solveMaze(x+1, y))
                {
                    return true;
                }
                else if (y+1 < map.GetLength(0) && solveMaze(x , y+1))
                {
                    return true;
                }
                else if (x-1 >= 0 && solveMaze(x-1, y))
                {
                    return true;
                }
                else
                {
                    map[y, x] = ' ';
                    _start.X = x;
                    _start.Y = y;
                    return false;
                }
            }
        }

        public bool reachedEnd(int x, int y)
        {
            return map[y,x] == 'E';
        }

        public bool deadEnd(int x, int y)
        {
            return "X#".IndexOf(map[y, x]) != -1;
        }

        public void output()
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    Debug.Write(map[y, x]);
                }
                Debug.WriteLine("");
            }
        }
    }
}
