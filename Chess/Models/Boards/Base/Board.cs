using System;
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.Pieces.Base;


namespace Chess.Models.Boards.Base
{
    public class Board : ICloneable
    {
        protected readonly Piece?[,] ArrayBoard;
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
        public static bool IsCellForKill(Point checkPoint, Board board)
        {

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
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
        public static void Move(MoveInfo moveInfo, Board board)
        {
            if (moveInfo.IsMoved)
            {
                if (moveInfo.KillPoint is { } killPoint)
                {
                    #region Пересчет цены доски при убийстве

                    if (board[killPoint.X, killPoint.Y] is { } pieceKill)
                    {
                        board.Price -= pieceKill.Price;
                        board.Price -= pieceKill.PieceEval[killPoint.X, killPoint.Y];
                    }

                    #endregion

                    board[killPoint.X, killPoint.Y] = null;
                }

                if (moveInfo.ChangePositions is { } changePositions)
                {
                    foreach (var (startP, endP) in changePositions)
                    {

                        #region Пересчет цены доски при перемещении

                        if (board[startP.X, startP.Y] is { } pieceMove)
                        {
                            board.Price -= pieceMove.PieceEval[startP.X, startP.Y];
                            board.Price += pieceMove.PieceEval[endP.X, endP.Y];
                        }

                        #endregion

                        board[endP.X, endP.Y] = board[startP.X, startP.Y];
                        board[startP.X, startP.Y] = null;

                        if (board[endP.X, endP.Y] is { IsFirstMove: true } piece)
                        {
                            var f = FactoryPiece.GetFactory(piece.Team, piece.Direction, false);
                            board[endP.X, endP.Y] = FactoryPiece.GetPiece(piece.TypePiece,piece.Team,piece.Direction,false);
                        }

                        board.LastMoveInfo = moveInfo;
                    }
                }

                if (moveInfo.ReplaceImg is { Item2: { } } replaceImg)
                {
                    #region Пересчет цены доски при замене

                    if (board[replaceImg.Item1.X, replaceImg.Item1.Y] is { } piece)
                    {
                        board.Price -= piece.Price;
                        board.Price -= piece.PieceEval[replaceImg.Item1.X, replaceImg.Item1.Y];
                        board.Price += replaceImg.Item2.Price;
                        board.Price += replaceImg.Item2.PieceEval[replaceImg.Item1.X, replaceImg.Item1.Y];
                    }

                    #endregion
                    var pieceRepl = replaceImg.Item2;
                    board[replaceImg.Item1.X, replaceImg.Item1.Y] = FactoryPiece.GetPiece(pieceRepl.TypePiece, pieceRepl.Team, pieceRepl.Direction,false);
  
                }
                board.WhoseMove = board.WhoseMove == TeamEnum.WhiteTeam ? TeamEnum.BlackTeam : TeamEnum.WhiteTeam;
            }
        }
       
        public object Clone()
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
