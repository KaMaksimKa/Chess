/*#region Свойство MoveInfo
public MoveInfo MoveInfo
{
    get => (MoveInfo)GetValue(ChangePosProperty);
    set => SetValue(ChangePosProperty, value);
}
public static readonly DependencyProperty ChangePosProperty =
    DependencyProperty.Register("MoveInfo", typeof(MoveInfo),
        typeof(ChessBoardUserControl), new PropertyMetadata(AddMoveInfo));


static async void AddMoveInfo(DependencyObject o, DependencyPropertyChangedEventArgs e)
{
    var control = (ChessBoardUserControl)o;

    control._moveInfosQueue.Enqueue((MoveInfo)e.NewValue);

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



    List<double> timeAnimsSec = new List<double> { 0 };

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
            *//* img.ReleaseMouseCapture();*//*
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
        Thread.Sleep((int)(timeAnimsSec.Max() * 1000));
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


#endregion*/