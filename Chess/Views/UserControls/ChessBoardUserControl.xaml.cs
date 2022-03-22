using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Chess.Models;

namespace Chess.Views.UserControls
{
    public partial class ChessBoardUserControl : UserControl
    {
        private Point? _movePoint;
        private readonly Image?[,] _images = new Image?[8, 8];

        #region Свойство SizeBoard

        private int _sizeCell;
        public double SizeBoard
        {
            get => Height;
            set
            {
                _sizeCell = (int)value / 8;
                Height = value;
                Width = value;
                DrawChessBoard();
            }
        }

        #endregion
        
        #region Свойство ChangePos
        public ChangePosition ChangePos
        {
            get => (ChangePosition)GetValue(ChangePosProperty);
            set => SetValue(ChangePosProperty, value);
        }
        public static readonly DependencyProperty ChangePosProperty =
            DependencyProperty.Register("ChangePos", typeof(ChangePosition),
                typeof(ChessBoardUserControl),new PropertyMetadata(PosChanged));

        static void PosChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var userControl = (ChessBoardUserControl)o;
            var (startPoint, endPoint) = userControl.ChangePos;
            var img = userControl._images[startPoint.X, startPoint.Y];

            if (img != null)
            {
                userControl._images[startPoint.X, startPoint.Y] = null;

                userControl.CanvasFace.Children.Remove(userControl._images[endPoint.X, endPoint.Y]);

                userControl._images[endPoint.X, endPoint.Y] = img;

                Canvas.SetLeft(img, endPoint.Y * userControl._sizeCell);
                Canvas.SetTop(img, endPoint.X * userControl._sizeCell);
            }
        }

        #endregion

        #region Свойство StartPoint
        public System.Drawing.Point? StartPoint
        {
            get => (System.Drawing.Point?)GetValue(StartPointProperty);
            set => SetValue(StartPointProperty, value);
        }
        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", typeof(System.Drawing.Point?),
                typeof(ChessBoardUserControl));


        #endregion

        #region Свойство EndPoint
        public System.Drawing.Point EndPoint
        {
            get => (System.Drawing.Point)GetValue(EndPointProperty);
            set => SetValue(EndPointProperty, value);
        }
        public static readonly DependencyProperty EndPointProperty =
            DependencyProperty.Register("EndPoint", typeof(System.Drawing.Point),
                typeof(ChessBoardUserControl));


        #endregion

        #region Свойство Hints
        public HintsChess Hints
        {
            get => (HintsChess)GetValue(HintsProperty);
            set => SetValue(HintsProperty, value);
        }
        public static readonly DependencyProperty HintsProperty =
            DependencyProperty.Register("Hints", typeof(HintsChess),
                typeof(ChessBoardUserControl));

        #endregion
        public ChessBoardUserControl()
        {
           InitializeComponent();
        }

        private void DrawChessBoard()
        {
            #region Нарисовать поле

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var rect = new Rectangle()
                    {
                        Width = _sizeCell,
                        Height = _sizeCell
                    };
                    Canvas.SetLeft(rect, j * _sizeCell);
                    Canvas.SetTop(rect, i * _sizeCell);

                    if ((i + j) % 2 == 0)
                    {
                        rect.Fill = ((new BrushConverter()).ConvertFrom("#779556") as Brush);
                    }
                    else
                    {
                        rect.Fill = ((new BrushConverter()).ConvertFrom("#ebecd0") as Brush);
                    }
                    CanvasBack.Children.Add(rect);
                }
            }

            #endregion

            #region Нарисовать фигуры

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var img = new Image()
                    {
                        Width = _sizeCell,
                        Height = _sizeCell
                    };
                    Canvas.SetLeft(img, j * _sizeCell);
                    Canvas.SetTop(img, i * _sizeCell);

                    Binding binding = new Binding
                    {
                        Path = new PropertyPath($"ChessBoard[{i},{j}].Icon")
                    };

                    img.SetBinding(Image.SourceProperty, binding);
                    img.MouseDown += Piece_OnMouseDown;
                    img.MouseMove += Piece_OnMouseMove;
                    img.MouseUp += Piece_OnMouseUp;

                    CanvasFace.Children.Add(img);

                    _images[i, j] = img;
                }

                #endregion

            }
        }

        #region Анимация перемечения фигур

        private void Piece_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            _movePoint = e.GetPosition(img);
            Point p = e.GetPosition(this) - (Vector)_movePoint.Value;
            StartPoint = new((int)Math.Round(p.Y / _sizeCell), (int)Math.Round(p.X / _sizeCell));

            Image newImg = new Image { Width = img.Width, Height = img.Height, Source = img.Source };
            newImg.MouseDown += Piece_OnMouseDown;
            newImg.MouseMove += Piece_OnMouseMove;
            newImg.MouseUp += Piece_OnMouseUp;
            Canvas.SetLeft(newImg, _movePoint.Value.X);
            Canvas.SetTop(newImg, _movePoint.Value.Y);
            CanvasFace.Children.Add(newImg);
            _images[StartPoint.Value.X, StartPoint.Value.Y] = newImg;


            CanvasFace.Children.Remove(img);

            newImg.CaptureMouse();
            
        }

        private void Piece_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_movePoint == null)
                return;
            Image img = (Image)sender;
            Point p = e.GetPosition(this) - (Vector)_movePoint.Value;
            EndPoint = new((int)Math.Round(p.Y / _sizeCell), (int)Math.Round(p.X / _sizeCell));
            _movePoint = null;
            img.ReleaseMouseCapture();
        }

        private void Piece_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_movePoint == null)
                return;
            Image img = (Image)sender;
            Point p = e.GetPosition(this) - (Vector)_movePoint.Value;
            Canvas.SetLeft(img, p.X);
            Canvas.SetTop(img, p.Y);
        }

        #endregion

    }
}
