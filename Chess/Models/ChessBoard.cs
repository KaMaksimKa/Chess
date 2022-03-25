
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Chess.Models.PiecesChess;
using Chess.Models.PiecesChess.Base;
using Chess.Models.PiecesChess.DifferentPiece;

namespace Chess.Models
{
    internal class ChessBoard:Board
    {
        public TeamEnum WhoseMove { get; set; } = TeamEnum.WhiteTeam;

        public HintsChess GetHintsForPiece(Point startPoint)
        {
            bool[,] hintsForMove = new bool[8, 8];
            bool[,] hintsForKill = new bool[8, 8];
            if (ArrayBoard[startPoint.X, startPoint.Y] is { } piece && piece.Team == WhoseMove)
            {
                for (byte i = 0; i < 8; i++)
                {
                    for (byte j = 0; j < 8; j++)
                    {
                        MoveInfo moveInfo = piece.Move(startPoint,new Point(i,j),this);

                        if (moveInfo.KillPoint != null)
                        {
                            hintsForKill[i,j] = true;
                        }
                        else if (moveInfo.ChangePositions != null)
                        {
                            hintsForMove[i,j] = true;
                        }
                    }
                }
            }

            return new HintsChess {IsHintsForKill = hintsForKill, IsHintsForMove = hintsForMove};
        }

        public ChessBoard()
        {
            
        }

        public ChessBoard(Piece?[,] arrayBoard) :base(arrayBoard)
        {
            
        }
        public MoveInfo Move(Point startPoint, Point endPoint)
        {
            if (startPoint.X is >= 0 and <= 7 && startPoint.Y is >= 0 and <= 7 &&
                endPoint.X is >= 0 and <= 7 && endPoint.Y is >= 0 and <= 7)
            {
                if (ArrayBoard[startPoint.X, startPoint.Y] is { } piece && piece.Team == WhoseMove)
                {
                    MoveInfo moveInfo = piece.Move(startPoint, endPoint, this);

                    if (moveInfo.KillPoint is { } killPoint)
                    {
                        ArrayBoard[killPoint.X, killPoint.Y] = null;
                    }

                    if (moveInfo.ChangePositions is { } changePositions)
                    {
                        foreach (var (startP, endP) in changePositions)
                        {
                            ArrayBoard[endP.X, endP.Y] = ArrayBoard[startP.X, startP.Y];
                            ArrayBoard[startP.X, startP.Y] = null;
                            LastMoveInfo = moveInfo;
                            if (ArrayBoard[endP.X, endP.Y] is { } p)
                            {
                                p.IsFirstMove = false;
                            }
                        }
                    }


                    return moveInfo;
                }
            }
            
            return new MoveInfo();
            

        }

        public IHaveIcon?[,] GetIcons()
        {
            return (IHaveIcon?[,]) ArrayBoard.Clone();
        }

        public override object Clone()
        {
            var arrayBoard = new Piece?[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    arrayBoard[i,j] = ArrayBoard[i,j]?.Clone() as Piece;
                }
            }
            return new ChessBoard(arrayBoard) {WhoseMove = WhoseMove,LastMoveInfo = LastMoveInfo};
        }
    }


    class Board:ICloneable
    {
        public Piece? this[int i, int j]
        {
            get => ArrayBoard[i,j];
            protected set => ArrayBoard[i,j] = value;
        }

        protected readonly Piece?[,] ArrayBoard;

        public MoveInfo LastMoveInfo { get; set; } = new MoveInfo();
        public Board()
        {
            ArrayBoard = GetNewBoard();
        }

        public Board(Piece?[,] arrayBoard)
        {
            ArrayBoard = arrayBoard;
        }
        
        private static Piece?[,] GetNewBoard()
        {
            Piece?[,] board = new Piece?[8,8];
            #region Создание пустых ячеек
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    board[i,j] = null;
            }
            #endregion

            #region Создание белой команды

            for (int i = 0; i < 8; i++)
            {
                board[1,i] = new WhitePawn(PawnDirection.Up);
            }

            board[0,0] = new WhiteRook();
            board[0,7] = new WhiteRook();
            board[0,1] = new WhiteKnight();
            board[0,6] = new WhiteKnight();
            board[0,2] = new WhiteBishop();
            board[0,5] = new WhiteBishop();
            board[0,3] = new WhiteKing();
            board[0,4] = new WhiteQueen();

            #endregion

            #region Создание черной команды

            for (int i = 0; i < 8; i++)
            {
                board[6,i] = new BlackPawn(PawnDirection.Down);
            }

            board[7,0] = new BlackRook();
            board[7,7] = new BlackRook();
            board[7,1] = new BlackKnight();
            board[7,6] = new BlackKnight();
            board[7,2] = new BlackBishop();
            board[7,5] = new BlackBishop();
            board[7,4] = new BlackQueen();
            board[7,3] = new BlackKing();

            #endregion

            return board;
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

        public bool IsCellForKill(Point starPoint,Point endPoint,TeamEnum team)
        {
            if (this.Clone() is Board board)
            {
                board[endPoint.X, endPoint.Y] = board[starPoint.X, starPoint.Y];
                board[starPoint.X, starPoint.Y] = null;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (ArrayBoard[i, j] is { } enemyPiece && enemyPiece.Team != team)
                        {
                            MoveInfo moveInfo = enemyPiece.Move(new Point(i, j), endPoint, board);
                            if (moveInfo.KillPoint is { })
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public virtual object Clone()
        {
            var arrayBoard = new Piece?[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    arrayBoard[i, j] = ArrayBoard[i,j]?.Clone() as Piece;
                }
            }
            return new Board(arrayBoard){LastMoveInfo = LastMoveInfo};
        }

    }

}
