using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Chess.Models.Pieces.Base;


namespace Chess.Models.Boards.Base
{
    public class Board : ICloneable
    {
        protected readonly Piece?[,] ArrayBoard;
        public readonly Size Size;
        public double Price { get; set; }
        public Piece? this[int i, int j]
        {
            get => ArrayBoard[i, j];
            protected set => ArrayBoard[i, j] = value;
        }
        public TeamEnum WhoseMove { get; protected set; } = TeamEnum.WhiteTeam;
        public MoveInfo LastMoveInfo { get; set; } = new MoveInfo();
        public Board(Piece?[,] arrayBoard)
        {
            Size = new Size(arrayBoard.GetLength(0), arrayBoard.GetLength(1));
            ArrayBoard = arrayBoard;
        }
        public bool CheckIsEmptySells(IEnumerable<Point>? points)
        {
            if (points is { })
            {
                foreach (var point in points)
                {
                    if (ArrayBoard[point.X, point.Y] is not null)
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }

        }
        public static bool IsNoMoves(Board board)
        {
            for (byte i = 0; i < board.Size.Height; i++)
            {
                for (byte j = 0; j < board.Size.Width; j++)
                {
                    var movesPiece = board.GetMovesForPiece(new Point(i, j));
                    if (movesPiece.Count > 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public static bool IsCellForKill(Point checkPoint, Board board)
        {

            for (int i = 0; i < board.Size.Height; i++)
            {
                for (int j = 0; j < board.Size.Width; j++)
                {
                    if (board[i, j] is { } piece)
                    {
                        var movesForPiece = piece.GetMoves(new Point(i, j), board);
                        foreach (var (_, moveInfoPiece) in movesForPiece)
                        {
                            if (moveInfoPiece.KillPoint is { } killPoint && killPoint == checkPoint)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public virtual Dictionary<(Point, Point), MoveInfo> GetMovesForAllPieces()
        {
            var goodMoves = new Dictionary<(Point, Point), MoveInfo>();

            for (int i = 0; i < Size.Height; i++)
            {
                for (int j = 0; j < Size.Width; j++)
                {
                    if (this[i, j] is { } piece && piece.Team==WhoseMove)
                    {
                        var movesForPiece = piece.GetMoves(new Point(i, j), this);
                        goodMoves = goodMoves.Concat(movesForPiece).ToDictionary(move => move.Key, move => move.Value);
                    }
                }
            }

            return goodMoves;
        }
        public virtual Dictionary<(Point, Point), MoveInfo> GetMovesForPiece(Point? startPoint)
        {
            var goodMoves = new Dictionary<(Point, Point), MoveInfo>();
            if (startPoint is { X: <= 7 and >= 0, Y: <= 7 and >= 0 } startP &&
                this[startP.X, startP.Y] is { } piece && piece.Team == WhoseMove)
            {
                 goodMoves = piece.GetMoves(startP, this);
            }

            return goodMoves;
        }
        public virtual void Move(MoveInfo moveInfo)
        {
            if (moveInfo.IsMoved)
            {
                if (moveInfo.KillPoint is { } killPoint)
                {
                    #region Пересчет цены доски при убийстве

                    if (this[killPoint.X, killPoint.Y] is { } pieceKill)
                    {
                        Price -= pieceKill.Price;
                        if (pieceKill.PieceEval.GetLength(0) == Size.Height &&
                            pieceKill.PieceEval.GetLength(1) == Size.Width)
                        {
                            Price -= pieceKill.PieceEval[killPoint.X, killPoint.Y];
                        }
                    }

                    #endregion

                    this[killPoint.X, killPoint.Y] = null;
                }

                if (moveInfo.ChangePositions is { } changePositions)
                {
                    foreach (var (startP, endP) in changePositions)
                    {

                        #region Пересчет цены доски при перемещении

                        if (this[startP.X, startP.Y] is { } pieceMove)
                        {
                            if (pieceMove.PieceEval.GetLength(0) == Size.Height &&
                                pieceMove.PieceEval.GetLength(1) == Size.Width)
                            {
                                Price -= pieceMove.PieceEval[startP.X, startP.Y];
                                Price += pieceMove.PieceEval[endP.X, endP.Y];
                            }
                        }

                        #endregion

                        this[endP.X, endP.Y] = this[startP.X, startP.Y];
                        this[startP.X, startP.Y] = null;

                        if (this[endP.X, endP.Y] is { IsFirstMove: true } piece)
                        {
                            this[endP.X, endP.Y] = FactoryPiece.GetPiece(piece.TypePiece,piece.Team,piece.Direction,false);
                        }

                        LastMoveInfo = moveInfo;
                    }
                }

                if (moveInfo.ReplaceImg is { Item2: { } } replaceImg)
                {
                    #region Пересчет цены доски при замене

                    if (this[replaceImg.Item1.X, replaceImg.Item1.Y] is { } piece)
                    {
                        Price -= piece.Price;
                        if (piece.PieceEval.GetLength(0) == Size.Height &&
                            piece.PieceEval.GetLength(1) == Size.Width)
                        {
                            Price -= piece.PieceEval[replaceImg.Item1.X, replaceImg.Item1.Y];
                        }

                        Price += replaceImg.Item2.Price;
                        if (replaceImg.Item2.PieceEval.GetLength(0) == Size.Height &&
                            replaceImg.Item2.PieceEval.GetLength(1) == Size.Width)
                        {
                            Price += replaceImg.Item2.PieceEval[replaceImg.Item1.X, replaceImg.Item1.Y];
                        }
                    }

                    #endregion
                    var pieceRepl = replaceImg.Item2;
                    this[replaceImg.Item1.X, replaceImg.Item1.Y] = FactoryPiece.GetPiece(pieceRepl.TypePiece, pieceRepl.Team, pieceRepl.Direction,false);
  
                }
               
            }
        }
        public virtual object Clone()
        {
            return new Board((Piece?[,])ArrayBoard.Clone())
            {
                WhoseMove = WhoseMove,
                LastMoveInfo = LastMoveInfo,
                Price = Price
            };
        }
    }
}
