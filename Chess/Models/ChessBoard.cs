using System;
using System.Collections.Generic;
using System.Drawing;
using Chess.Models.PiecesChess;
using Chess.Models.PiecesChess.Base;
using Chess.Models.PiecesChess.DifferentPiece;

namespace Chess.Models
{
    internal class ChessBoard:Board
    {
        public TeamEnum WhoseMove { get; set; } = TeamEnum.WhiteTeam;
        public ChessBoard()
        {
            
        }
        public ChessBoard(Piece?[,] arrayBoard) :base(arrayBoard)
        {
            
        }

        public bool IsCheck(ChangePosition? changePosition)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (ArrayBoard[i, j] is King king && king.Team == WhoseMove)
                    {
                        if (changePosition is { })
                        {
                            var startPoint = changePosition.StartPoint;
                            var endPoint = changePosition.EndPoint;
                            if (ArrayBoard[startPoint.X, startPoint.Y] is not King)
                            {
                                return IsCellForKill(ArrayBoard[startPoint.X, startPoint.Y]?
                                        .Move(startPoint, endPoint, this),
                                    new Point(i, j), king.Team);
                            }
                            else
                            {
                                return IsCellForKill(ArrayBoard[startPoint.X, startPoint.Y]?
                                        .Move(startPoint, endPoint, this), endPoint, king.Team);
                            }
                        }
                        else
                        {
                            return IsCellForKill(null, new Point(i, j), king.Team);
                        }
                        
                    }
                }
            }

            return false;
        }

        public MoveInfo? IsMove(Point startPoint, Point endPoint)
        {
            if (startPoint.X is >= 0 and <= 7 &&
                startPoint.Y is >= 0 and <= 7 &&
                endPoint.X is >= 0 and <= 7 &&
                endPoint.Y is >= 0 and <= 7 &&
                ArrayBoard[startPoint.X, startPoint.Y] is { } piece &&
                piece.Team == WhoseMove &&
                piece.Move(startPoint, endPoint, this) is {} moveInfo &&
                !IsCheck(new ChangePosition{StartPoint = startPoint,EndPoint = endPoint}))
            {
                return moveInfo;
            }
            return null;
        }

        
        public MoveInfo? Move(Point startPoint, Point endPoint)
        {
            if (IsMove(startPoint,endPoint) is {} moveInfo)
            {
                Board.Move(moveInfo,this);
                return moveInfo;
                
            }
            
            return null;
            

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
                board[1, i] = new WhitePawn(PawnDirection.Up);
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
                board[6, i] = new BlackPawn(PawnDirection.Down);
            }

            board[7,0] = new BlackRook();
            board[7, 7] = new BlackRook();
            board[7, 1] = new BlackKnight();
            board[7, 6] = new BlackKnight();
            board[7, 2] = new BlackBishop();
            board[7, 5] = new BlackBishop();
            board[7, 4] = new BlackQueen();
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

        public bool IsCellForKill(MoveInfo? moveInfo,Point checkPoint,TeamEnum team)
        {
            if (this.Clone() is Board board)
            {
                Board.Move(moveInfo,board);
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (board[i, j] is { } enemyPiece &&
                            enemyPiece.Team != team &&
                            enemyPiece.Move(new Point(i, j), checkPoint, board)?.KillPoint is {})
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        protected static void Move(MoveInfo? moveInfo, Board board)
        {
            if (moveInfo is { })
            {
                if (moveInfo.KillPoint is { } killPoint)
                {
                    board.ArrayBoard[killPoint.X, killPoint.Y] = null;
                }

                if (moveInfo.ChangePositions is { } changePositions)
                {
                    foreach (var (startP, endP) in changePositions)
                    {
                        board.ArrayBoard[endP.X, endP.Y] = board.ArrayBoard[startP.X, startP.Y];
                        board.ArrayBoard[startP.X, startP.Y] = null;
                        board.LastMoveInfo = moveInfo;
                        if (board.ArrayBoard[endP.X, endP.Y] is { } p)
                        {
                            p.IsFirstMove = false;
                        }
                    }
                }
            }
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
