using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Chess.Models;
using Chess.Models.PiecesChess.Base;

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
                DrawBoard();
            }
        }

        #endregion

        #region Свойство MoveInfo
        public MoveInfo MoveInfo
        {
            get => (MoveInfo)GetValue(ChangePosProperty);
            set => SetValue(ChangePosProperty, value);
        }
        public static readonly DependencyProperty ChangePosProperty =
            DependencyProperty.Register("MoveInfo", typeof(MoveInfo),
                typeof(ChessBoardUserControl),new PropertyMetadata(PosChanged));

        static void PosChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var control = (ChessBoardUserControl)o;
            var moveInfo = control.MoveInfo;
            

            if (moveInfo.KillPoint is { } killPoint)
            {
                if (control._images[killPoint.X, killPoint.Y] is { } img)
                {
                    control.CanvasPieces.Children.Remove(img);
                    control._images[killPoint.X, killPoint.Y] = null;
                }
            }

            if (moveInfo.ChangePositions is { } changePositions)
            {
                control.CanvasHints.Children.Clear();
                control.CanvasCell.Children.Clear();
                foreach (var (startPoint,endPoint) in changePositions)
                {
                    if (control._images[startPoint.X, startPoint.Y] is { } img)
                    {
                        control._images[endPoint.X, endPoint.Y] = img;
                        control._images[startPoint.X, startPoint.Y] = null;
                        control.ChangePosImgOnCanvas(img,endPoint,control._sizeCell,400);
                        
                        control.DrawChoiceCell(startPoint);
                        control.DrawChoiceCell(endPoint);
                    }
                }

                control.StartPoint = null;
                control.EndPoint = null;
            }
            
            else
            {
                if (control.StartPoint is { } startPoint)
                {
                    var img = control._images[startPoint.X, startPoint.Y];
                    if (img != null)
                    {
                        control.ChangePosImgOnCanvas(img, startPoint, control._sizeCell, 4000);
                    }
                }

                control.EndPoint = null;
            }
            
        }

        private void ChangePosImgOnCanvas(Image img, System.Drawing.Point endPoint,int sizeSell,int speed)
        {
            var imgLeftPos = Canvas.GetLeft(img);
            var imgTopPos = Canvas.GetTop(img);

            var endLeftPos = endPoint.Y * sizeSell;
            var endTopPos = endPoint.X * sizeSell;

            var duration = new Duration(TimeSpan.FromSeconds(
                Math.Sqrt(Math.Pow(endTopPos - imgTopPos, 2) + Math.Pow(endLeftPos - imgLeftPos, 2)) / speed));

            img.BeginAnimation(Canvas.TopProperty, new DoubleAnimation
            {
                From = imgTopPos,
                To = endTopPos,
                Duration = duration
            });

            img.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation
            {
                From = imgLeftPos,
                To = endLeftPos,
                Duration = duration
            });
        }
        #endregion

        #region Свойство StartPoint
        public System.Drawing.Point? StartPoint
        {
            get => (System.Drawing.Point?)GetValue(StartPointProperty);
            set
            {
                if (value != null)
                {
                    if (StartPoint != null)
                    {
                        CanvasCell.Children.RemoveRange(CanvasCell.Children.Count - 1, 1);
                    }
                    DrawChoiceCell((System.Drawing.Point)value);
                }
                SetValue(StartPointProperty, value);
            }
        }

        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", typeof(System.Drawing.Point?),
                typeof(ChessBoardUserControl));


        #endregion

        #region Свойство EndPoint
        public System.Drawing.Point? EndPoint
        {
            get => (System.Drawing.Point?)GetValue(EndPointProperty);
            set => SetValue(EndPointProperty, value);
        }

        public static readonly DependencyProperty EndPointProperty =
            DependencyProperty.Register("EndPoint", typeof(System.Drawing.Point?),
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
                typeof(ChessBoardUserControl), new PropertyMetadata(DrawHints));

        static void DrawHints(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var canvasHints = ((ChessBoardUserControl) sender).CanvasHints;
            var sizeSell = ((ChessBoardUserControl) sender)._sizeCell;
            canvasHints.Children.Clear();

            var hints=(HintsChess)e.NewValue;

            if (hints.IsHintsForKill != null && hints.IsHintsForMove != null)
            {

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (hints.IsHintsForKill[i, j])
                        {
                            var ellipse = new Ellipse
                            {
                                Opacity = 0.2,
                                Width = sizeSell,
                                Height = sizeSell,
                                Fill = Brushes.Black,
                                OpacityMask = new RadialGradientBrush(new GradientStopCollection(new List<GradientStop>
                                {
                                    new GradientStop((Color)ColorConverter.ConvertFromString("#FFB94444") ,0.8),
                                    new GradientStop((Color)ColorConverter.ConvertFromString("#00FFFFFF") ,0.79),
                                }))
                            };

                            Canvas.SetLeft(ellipse, j * sizeSell);
                            Canvas.SetTop(ellipse, i * sizeSell);

                            canvasHints.Children.Add(ellipse);
                        }

                        if (hints.IsHintsForMove[i, j])
                        {
                            var ellipse = new Ellipse
                            {
                                Opacity = 0.2,
                                // ReSharper disable once PossibleLossOfFraction
                                Width = sizeSell / 3,
                                // ReSharper disable once PossibleLossOfFraction
                                Height = sizeSell / 3,
                                Fill = Brushes.Black
                            };
                            Canvas.SetLeft(ellipse, j * sizeSell + sizeSell / 3);
                            Canvas.SetTop(ellipse, i * sizeSell + sizeSell / 3);

                            canvasHints.Children.Add(ellipse);
                        }
                    }
                }
            }
        }

        #endregion

        #region Свойство BoardForDraw
        public BoardForDraw BoardForDraw
        {
            get => (BoardForDraw)GetValue(BoardForDrawProperty);
            set => SetValue(BoardForDrawProperty, value);
        }
        public static readonly DependencyProperty BoardForDrawProperty =
            DependencyProperty.Register("BoardForDraw", typeof(BoardForDraw),
                typeof(ChessBoardUserControl), new PropertyMetadata(DrawChessBoard));

        private static void DrawChessBoard(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ChessBoardUserControl control = (ChessBoardUserControl)o;
            var icons = ((BoardForDraw)e.NewValue).Icons;
            var sizeCell = control._sizeCell;
            var canvasPieces = control.CanvasPieces;
            control.CanvasCell.Children.Clear();

            control.StartPoint = null;
            control.EndPoint = null;

            #region Нарисовать фигуры
            canvasPieces.Children.Clear();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (icons[i, j]?.Icon is { } icon)
                    {
                        var img = new Image()
                        {
                            Width = sizeCell,
                            Height = sizeCell,
                            Source = (new ImageSourceConverter()).ConvertFrom("../"+icon) as ImageSource
                        };
                        Canvas.SetLeft(img, j * sizeCell);
                        Canvas.SetTop(img, i * sizeCell);

                        img.MouseDown += control.Piece_OnMouseDown;
                        img.MouseMove += control.Piece_OnMouseMove;
                        img.MouseUp += control.Piece_OnMouseUp;

                        canvasPieces.Children.Add(img);
                         
                        control._images[i, j] = img;

                    }
                }

            }
            #endregion

            #region Нарисовать последний ход

            if (((BoardForDraw) e.NewValue).LastMoveInfo.ChangePositions is { } changePositions)
            {
                foreach (var (startP,endP) in changePositions)
                {
                    control.DrawChoiceCell(startP);
                    control.DrawChoiceCell(endP);
                }
            }

            #endregion
        }

        #endregion
        public ChessBoardUserControl()
        {
           InitializeComponent();
        }

        private void DrawChoiceCell(System.Drawing.Point point)
        {
            var rectangle = new Rectangle
            {
                Width = _sizeCell,
                Height = _sizeCell,
                Fill = ((new BrushConverter()).ConvertFrom("#baca2b") as Brush),
                Opacity = 0.7
            };
            Canvas.SetLeft(rectangle, point.Y * _sizeCell);
            Canvas.SetTop(rectangle, point.X * _sizeCell);
            CanvasCell.Children.Add(rectangle);
        }

        private void DrawBoard()
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

            #region Добавить EmptyCells

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var rect = new Rectangle()
                    {
                        Width = _sizeCell,
                        Height = _sizeCell,
                        Fill = ((new BrushConverter()).ConvertFrom("#000000") as Brush),
                        Opacity = 0
                };
                    
                    rect.MouseDown += EmptyCells_Down;

                    Canvas.SetLeft(rect, j * _sizeCell);
                    Canvas.SetTop(rect, i * _sizeCell);

                    
                    CanvasEmptyCells.Children.Add(rect);
                }
            }

            #endregion
        }

        private void EmptyCells_Down(object sender, MouseButtonEventArgs e)
        {
            if (StartPoint != null && EndPoint == null)
            {
                Point p = e.GetPosition(this) - (Vector)e.GetPosition((Rectangle)sender);
                EndPoint = new((int)Math.Round(p.Y / _sizeCell), (int)Math.Round(p.X / _sizeCell));
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
            CanvasPieces.Children.Add(newImg);
            _images[StartPoint.Value.X, StartPoint.Value.Y] = newImg;


            CanvasPieces.Children.Remove(img);

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
