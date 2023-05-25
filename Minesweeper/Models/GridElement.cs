using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Minesweeper.Models
{
    public class GridElement : BaseNotification
    {
        public int Row { get; set; }
        public int Col { get; set; }

        private Visibility _isVisible;
        public Visibility IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                NotifyPropertyChanged(nameof(IsVisible));
            }
        }

        private bool _isBomb;
        public bool IsBomb
        {
            get { return _isBomb; }
            set
            {
                _isBomb = value;
                NotifyPropertyChanged(nameof(IsBomb));
            }
        }

        private int _nrOfBombs;
        public int NrOfBombs
        {
            get { return _nrOfBombs; }
            set
            {
                _nrOfBombs = value;
                NotifyPropertyChanged(nameof(NrOfBombs));
            }
        }

        private string _content;
        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                NotifyPropertyChanged(nameof(Content));
            }
        }

        private string _color;
        public string Color
        {
            get { return _color; }
            set
            {
                _color = value;
                NotifyPropertyChanged(nameof(Color));
            }
        }

        public GridElement(int row, int col)
        {
            Row = row;
            Col = col;
            IsVisible = Visibility.Collapsed;
            IsBomb = false;
            NrOfBombs = 0;
            Content = string.Empty;
            Color = "gray";
        }
    }
}
