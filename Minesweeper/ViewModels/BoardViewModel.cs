using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using Minesweeper.Models;

namespace Minesweeper
{
    public class BoardViewModel : BaseNotification
    {
        private readonly int numRows = 9;
        private readonly int numCols = 9;
        private readonly int nrOfBombs = 10;

        int[] rowOffsets = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] colOffsets = { -1, 0, 1, -1, 1, -1, 0, 1 };

        private ObservableCollection<GridElement> _grid;
        public ObservableCollection<GridElement> Grid
        {
            get { return _grid; }
            set
            {
                _grid = value;
                NotifyPropertyChanged(nameof(Grid));
            }
        }

        public BoardViewModel()
        {
            GenerateGrid();
            GenerateBombs();
            GenerateHelperNumbers();
        }

        private void GenerateGrid()
        {
            Grid = new ObservableCollection<GridElement>();
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    var button = new GridElement(row, col);
                    Grid.Add(button);
                }
            }
        }

        private void GenerateBombs()
        {
            int count = 0;
            var random = new Random();

            while (count < nrOfBombs)
            {
                int randomPos = random.Next(0, numRows * numCols);

                if (!Grid[randomPos].IsBomb)
                {
                    Grid[randomPos].IsBomb = true;
                    Grid[randomPos].Content = "Bomb";
                    count++;
                }
            }
        }

        private void GenerateHelperNumbers()
        {
            for (int i = 0; i < numRows * numCols; i++)
            {
                GridElement element = Grid[i];

                for (int j = 0; j < 8; j++)
                {
                    int neighborRow = element.Row + rowOffsets[j];
                    int neighborCol = element.Col + colOffsets[j];

                    if (neighborRow < 0 || neighborRow >= numRows || neighborCol < 0 || neighborCol >= numCols)
                    {
                        continue;
                    }

                    int neighborIndex = neighborRow * numCols + neighborCol;
                    GridElement neighbor = Grid[neighborIndex];

                    if (element.IsBomb == true && neighbor.IsBomb == false)
                    {
                        neighbor.NrOfBombs++;
                        neighbor.Content = neighbor.NrOfBombs.ToString();
                    }
                }
            }
        }

        public void ShowAllBombs()
        {
            for (int i = 0; i < numRows * numCols; i++)
            {
                if (Grid[i].IsBomb == true)
                {
                    Grid[i].IsVisible = Visibility.Visible;
                    Grid[i].Color = "white";
                }
            }
        }

        public void MakeVisible(object param)
        {
            if (param is GridElement element)
            {
                if (element.Color != "white")
                {
                    element.IsVisible = Visibility.Visible;
                    element.Color = "white";

                    if (element.IsBomb == true)
                    {
                        ShowAllBombs();
                        MessageBox.Show("You lost!");
                    }
                    else if (element.Content == "")
                    {
                        MakeVisibleAround(element.Row, element.Col);
                    }
                }
            }
            if (CheckWin())
            {
                MessageBox.Show("You won!");
                return;
            }
        }

        public void MakeVisibleAround(int startRow, int startCol)
        {
            for (int row = startRow - 1; row <= startRow + 1; row++)
            {
                for (int col = startCol - 1; col <= startCol + 1; col++)
                {
                    if (row >= 0 && row < numRows && col >= 0 && col < numCols)
                    {
                        MakeVisible(Grid[row * numCols + col]);
                    }
                }
            }
        }

        public bool CheckWin()
        {
            for (int i = 0; i < numRows * numCols; i++)
            {
                GridElement element = Grid[i];
                if (!element.IsBomb && element.IsVisible != Visibility.Visible)
                {
                    return false;
                }
            }
            return true;
        }

        public ICommand MakeVisibleCommand => new RelayCommand<object>(MakeVisible);
    }
}
