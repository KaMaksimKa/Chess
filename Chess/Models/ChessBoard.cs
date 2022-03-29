﻿using System;
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
        public event Action<MoveInfo?>? ChessBoardMovedEvent;
        public TeamEnum WhoseMove { get; set; } = TeamEnum.WhiteTeam;
        public ChessBoard()
        {
            
        }
        public ChessBoard(Piece?[,] arrayBoard) :base(arrayBoard)
        {
            
        }

        public bool IsCheck(MoveInfo? moveInfo)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (ArrayBoard[i, j] is King king && king.Team == WhoseMove)
                    {
                        return IsCellForKill(moveInfo, new Point(i, j), king.Team);
                    }
                }
            }

            return false;
        }
        public Dictionary<(Point, Point), MoveInfo>? GetMovesForPiece(Point? startPoint)
        {
            if (startPoint is { } startP && ArrayBoard[startP.X, startP.Y] is { } piece &&
                piece.Team == WhoseMove)
            {
                var moves = piece.GetMoves(startP, this);
                return moves?.Where(i => !IsCheck(i.Value))
                    .ToDictionary(i => i.Key, i => i.Value);
            }

            return null;
        }
        public MoveInfo? IsMove(Point startPoint, Point endPoint)
        {
            var movesForPiece = GetMovesForPiece(startPoint);
            if (movesForPiece is { } && movesForPiece.ContainsKey((startPoint, endPoint)))
            {
                return movesForPiece[(startPoint, endPoint)];
            }
            return null;
        }

        public MoveInfo? Move(Point startPoint, Point endPoint)
        {
            if (IsMove(startPoint,endPoint) is {} moveInfo)
            {
                Board.Move(moveInfo,this);

                WhoseMove = WhoseMove == TeamEnum.WhiteTeam ? TeamEnum.BlackTeam : TeamEnum.WhiteTeam;

                ChessBoardMovedEvent?.Invoke(moveInfo);
                
                return moveInfo;
            }
            ChessBoardMovedEvent?.Invoke(null);
            return null;
            
        }
        public IHaveIcon?[,] GetIcons()
        {
            return (IHaveIcon?[,]) ArrayBoard.Clone();
        }
        public override object Clone()
        {
            return new ChessBoard((Piece?[,])ArrayBoard.Clone()) 
            {
                WhoseMove = WhoseMove,
                LastMoveInfo = LastMoveInfo,
                ChessBoardMovedEvent = ChessBoardMovedEvent,
                AllPieceMoved = AllPieceMoved
            };
        }
    }


    class Board:ICloneable
    {
        
        public Piece? this[int i, int j]
        {
            get => ArrayBoard[i,j];
            protected set => ArrayBoard[i,j] = value;
        }

        public AllPieceMoved AllPieceMoved { get; set; } = new AllPieceMoved();

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
                board[6, i] = new WhitePawn(PawnDirection.Down);
            }

            board[7,0] = new WhiteRook();
            board[7,7] = new WhiteRook();
            board[7,1] = new WhiteKnight();
            board[7,6] = new WhiteKnight();
            board[7,2] = new WhiteBishop();
            board[7,5] = new WhiteBishop();
            board[7,4] = new WhiteKing();
            board[7,3] = new WhiteQueen();

            #endregion

            #region Создание черной команды

            for (int i = 0; i < 8; i++)
            {
                board[1, i] = new BlackPawn(PawnDirection.Up);
            }

            board[0,0] = new BlackRook();
            board[0, 7] = new BlackRook();
            board[0, 1] = new BlackKnight();
            board[0, 6] = new BlackKnight();
            board[0, 2] = new BlackBishop();
            board[0, 5] = new BlackBishop();
            board[0, 3] = new BlackQueen();
            board[0,4] = new BlackKing();

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
                if (moveInfo?.ChangePositions is { } changePositions)
                {
                    foreach (var (startPoint,endPoint) in changePositions)
                    {
                        if (checkPoint == startPoint)
                        {
                            checkPoint = endPoint;
                        }
                    }
                }
                
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (board[i, j] is { } piece && piece.Team !=team)
                        {
                            var movesForPiece = board[i, j]?.GetMoves(new Point(i, j), board);
                            if (movesForPiece != null)
                            {
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
                }
            }

            return false;
        }

        public static void Move(MoveInfo? moveInfo, Board board)
        {
            if (moveInfo is { })
            {
                if (moveInfo.KillPoint is { } killPoint)
                {
                    board[killPoint.X, killPoint.Y] = null;
                }

                if (moveInfo.ChangePositions is { } changePositions)
                {
                    foreach (var (startP, endP) in changePositions)
                    {
                        board[endP.X, endP.Y] = board[startP.X, startP.Y];
                        board[startP.X, startP.Y] = null;
                        
                        if (board[endP.X, endP.Y] is {IsFirstMove:true } piece)
                        {
                            board[endP.X, endP.Y] = board.AllPieceMoved.GetMovedPiece(piece);
                        }

                        board.LastMoveInfo = moveInfo;
                    }
                }
            }
        }
        public virtual object Clone()
        {
            return new Board((Piece?[,])ArrayBoard.Clone()){LastMoveInfo = LastMoveInfo,AllPieceMoved = AllPieceMoved};
        }

    }

}
