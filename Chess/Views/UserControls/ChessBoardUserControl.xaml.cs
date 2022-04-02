using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Chess.Models;

namespace Chess.Views.UserControls
{
    public partial class ChessBoardUserControl : UserControl
    {
        private Point? _currentMovePoint;
        private readonly Image?[,] _images = new Image?[8, 8];
        private int _sizeCell;
        private readonly Queue<MoveInfo> _moveInfosQueue = new Queue<MoveInfo>();
        private bool _isAnimGo;
        #region Свойство SizeBoard

        public double SizeBoard
        {
            get => (double)GetValue(SizeBoardProperty);
            set => SetValue(SizeBoardProperty, value);
        }
        public static readonly DependencyProperty SizeBoardProperty =
            DependencyProperty.Register("SizeBoard", typeof(double),
                typeof(ChessBoardUserControl));
        #endregion

        #region Свойство MoveInfo
        public Queue<MoveInfo> MoveInfoQueue
        {
            get => (Queue<MoveInfo>)GetValue(MoveInfoQueueProperty);
            set => SetValue(MoveInfoQueueProperty, value);
        }
        public static readonly DependencyProperty MoveInfoQueueProperty =
            DependencyProperty.Register("MoveInfoQueue", typeof(Queue<MoveInfo>),
                typeof(ChessBoardUserControl),new PropertyMetadata(AddMoveInfo));


        static async void AddMoveInfo(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var control = (ChessBoardUserControl)o;

            var queue = (Queue<MoveInfo>)e.NewValue;
            for (int i = 0; i < queue.Count; i++)
            {
                control._moveInfosQueue.Enqueue(queue.Dequeue());
            }
            
            if (!control._isAnimGo)
            {
                await control.ChangePos();
            }
        }
        private async Task ChangePos()
        {
            if (_moveInfosQueue.Count == 0)
            {
                _isAnimGo = false;
                return;
            }
            _isAnimGo = true;

            var moveInfo = _moveInfosQueue.Dequeue();



            List<double> timeAnimsSec = new List<double>{0};

            if (moveInfo.KillPoint is { } killPoint)
            {
                if (_images[killPoint.X, killPoint.Y] is { } img)
                {
                    CanvasPieces.Children.Remove(img);
                    _images[killPoint.X, killPoint.Y] = null;
                }
            }
            if (moveInfo.ChangePositions is { } changePositions)
            {
                CanvasHints.Children.Clear();
                CanvasCell.Children.Clear();
                
                
                foreach (var (startPoint, endPoint) in changePositions)
                {
                    if (_images[startPoint.X, startPoint.Y] is { } img)
                    {
                        _images[endPoint.X, endPoint.Y] = img;
                        _images[startPoint.X, startPoint.Y] = null;

                        timeAnimsSec.Add(ChangePosImgOnCanvas(img, endPoint, _sizeCell, 400));

                        DrawChoiceCell(startPoint);
                        DrawChoiceCell(endPoint);
                    }
                }

            }
            if (StartPoint is { } startP)
            {
                var img = _images[startP.X, startP.Y];
                if (img != null)
                {
                    timeAnimsSec.Add(ChangePosImgOnCanvas(img, startP, _sizeCell, 4000));
                    img.ReleaseMouseCapture();
                }

            }
            if (moveInfo.KillPoint == null && moveInfo.ChangePositions == null)
            {
                if (EndPoint is { X: <= 7 and >= 0, Y: <= 7 and >= 0 } endP &&
                    _images[endP.X, endP.Y] is { })
                {
                    StartPoint = new System.Drawing.Point(endP.X, endP.Y);
                    EndPoint = null;
                }
                else
                {
                    EndPoint = null;
                    _currentMovePoint = null;
                }
            }
            else
            {
                StartPoint = null;
                EndPoint = null;
                _currentMovePoint = null;
            }

            


            await Task.Run(() =>
            {
                Thread.Sleep((int) (timeAnimsSec.Max() * 1000));
            });
            await ChangePos();



        }
        private double ChangePosImgOnCanvas(Image img, System.Drawing.Point endPoint, int sizeSell, int speed)
        {
            var imgLeftPos = Canvas.GetLeft(img);
            var imgTopPos = Canvas.GetTop(img);

            var endLeftPos = endPoint.Y * sizeSell;
            var endTopPos = endPoint.X * sizeSell;

            double durationSeconds = Math.Sqrt(Math.Pow(endTopPos - imgTopPos, 2) + Math.Pow(endLeftPos - imgLeftPos, 2)) / speed;
            var duration = new Duration(TimeSpan.FromSeconds(durationSeconds));

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
            return durationSeconds;
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
                        CanvasCell.Children.RemoveAt(CanvasCell.Children.Count-1);
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
            foreach (var movePoint in hints.IsHintsForMove)
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
                Canvas.SetLeft(ellipse, movePoint.Y * sizeSell + sizeSell / 3);
                Canvas.SetTop(ellipse, movePoint.X * sizeSell + sizeSell / 3);

                canvasHints.Children.Add(ellipse);
            }
            foreach (var killPoint in hints.IsHintsForKill)
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

                Canvas.SetLeft(ellipse, killPoint.Y * sizeSell);
                Canvas.SetTop(ellipse, killPoint.X * sizeSell);

                canvasHints.Children.Add(ellipse);
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
            control.StartPoint = null;
            control.EndPoint = null;

            control.DrawStateDraw((BoardForDraw)e.NewValue);
           
        }

        private void DrawStateDraw(BoardForDraw boardForDraw)
        {
            if (_sizeCell == 0)
            {
                return;
            }
            CanvasCell.Children.Clear();
            #region Нарисовать фигуры
            CanvasPieces.Children.Clear();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (boardForDraw.Icons[i, j]?.Icon is { } icon)
                    {
                        var img = new Image()
                        {
                            Width = _sizeCell,
                            Height = _sizeCell,
                            Source = (new ImageSourceConverter()).ConvertFrom("../" + icon) as ImageSource
                        };
                        Canvas.SetLeft(img, j * _sizeCell);
                        Canvas.SetTop(img, i * _sizeCell);

                        img.MouseDown += Piece_OnMouseDown;
                        img.MouseMove += Piece_OnMouseMove;
                        img.MouseUp += Piece_OnMouseUp;

                        CanvasPieces.Children.Add(img);

                        _images[i, j] = img;

                    }
                }

            }
            #endregion

            #region Нарисовать последний ход

            if (boardForDraw.LastMoveInfo?.ChangePositions is { } changePositions)
            {
                foreach (var (startP, endP) in changePositions)
                {
                    DrawChoiceCell(startP);
                    DrawChoiceCell(endP);
                }
            }

            #endregion
        }

        #endregion
        public ChessBoardUserControl()
        {
           InitializeComponent();
        }
        protected override Size MeasureOverride(Size constraint)
        {
            var sizeBoard = constraint.Height > constraint.Width ? constraint.Width : constraint.Height;
            
            SizeBoard = sizeBoard;
            _sizeCell = (int)sizeBoard / 8;
            
            DrawBoard();
            DrawStateDraw(BoardForDraw);

            return constraint;
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
            CanvasBack.Children.Clear();
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
                        rect.Fill = ((new BrushConverter()).ConvertFrom("#ebecd0") as Brush);
                    }
                    else
                    {
                        rect.Fill = ((new BrushConverter()).ConvertFrom("#779556") as Brush);
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

        #region Анимация перемечения фигур
        private void EmptyCells_Down(object sender, MouseButtonEventArgs e)
        {
            if (StartPoint != null && EndPoint == null)
            {
                Point p = e.GetPosition(CanvasPieces) - (Vector)e.GetPosition((Rectangle)sender);
                EndPoint = new((int)Math.Round(p.Y / _sizeCell), (int)Math.Round(p.X / _sizeCell));
            }
        }
        private void ReplaceImg(Image img,Point p)
        {
            
            Image newImg = new Image { Width = img.Width, Height = img.Height, Source = img.Source };
            CanvasPieces.Children.Remove(img);
            newImg.MouseDown += Piece_OnMouseDown;
            newImg.MouseMove += Piece_OnMouseMove;
            newImg.MouseUp += Piece_OnMouseUp;
            Canvas.SetLeft(newImg, p.X);
            Canvas.SetTop(newImg, p.Y);
            CanvasPieces.Children.Add(newImg);
            _images[(int)Math.Round(p.Y / _sizeCell), (int)Math.Round(p.X / _sizeCell)] = newImg;


            newImg.CaptureMouse();
            
            
        }
        private void Piece_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            _currentMovePoint = e.GetPosition(img);
            
            Point p = e.GetPosition(CanvasPieces) - (Vector)e.GetPosition(img);

            System.Drawing.Point point = new((int)Math.Round(p.Y / _sizeCell), (int)Math.Round(p.X / _sizeCell));
            if (StartPoint == null || StartPoint == point)
            {
                StartPoint = point;
            }
            else
            {
                EndPoint = point;
            }

            ReplaceImg(img, p);

        }

        private void Piece_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_currentMovePoint == null)
                return;
            Image img = (Image)sender;
            Point p = e.GetPosition(CanvasPieces) - (Vector)_currentMovePoint.Value;
            EndPoint = new((int)Math.Round(p.Y / _sizeCell), (int)Math.Round(p.X / _sizeCell));
            _currentMovePoint = null;
            img.ReleaseMouseCapture();
        }

        private void Piece_OnMouseMove(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            if (StartPoint is { X: 6, Y: 6 })
            {
                var a = this;
            }
            if (_currentMovePoint == null || StartPoint == null)
                return;
            
            Point p = e.GetPosition(CanvasPieces) - (Vector)_currentMovePoint.Value;
            Canvas.SetLeft(img, p.X);
            Canvas.SetTop(img, p.Y);
        }

        #endregion

    }
}
