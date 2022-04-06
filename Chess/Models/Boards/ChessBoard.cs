using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Chess.Models.Boards.Base;
using Chess.Models.Pieces.Base;
using Chess.Models.Pieces.PiecesChess;
using Chess.Models.Pieces.PiecesChess.DifferentPiece;

namespace Chess.Models.Boards
{
    internal class ChessBoard:Board
    {
        public event Action<MoveInfo>? ChessBoardMovedEvent;
        public event Action<List<Piece>,Point>? ChoiceReplacementPieceEvent;
        public event Action<TeamEnum?>? EndGameEvent; 

        private MoveInfo? _moveInfoForReplacePiece;
        public ChessBoard():base(GetNewBoard())
        {

        }
        public ChessBoard(Piece?[,] arrayBoard) :base(arrayBoard)
        {
            
        }
        public static Dictionary<(Point, Point), MoveInfo> GetMovesForPiece(Point? startPoint, Board board)
        {
            var goodMoves = new Dictionary<(Point, Point), MoveInfo>();
            if (startPoint is { X: <= 7 and >= 0, Y: <= 7 and >= 0 } startP &&
                board[startP.X, startP.Y] is { } piece && piece.Team == board.WhoseMove)
            {
                var moves = piece.GetMoves(startP, board);
                foreach (var move in moves)
                {
                    if (board.Clone() is Board boardClone)
                    {
                        Board.Move(move.Value,boardClone);
                        if (!IsCheck(boardClone,board.WhoseMove))
                        {
                            goodMoves.Add(move.Key,move.Value);
                        }
                    }
                }
            }

            return goodMoves;
        }
        public static bool IsCheck(Board board,TeamEnum team)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] is King king && king.Team == team)
                    {
                        return IsCellForKill(new Point(i, j),board);
                    }
                }
            }

            return false;
        }
        public static bool IsNoMoves(Board board)
        {
            for (byte i = 0; i < 8; i++)
            {
                for (byte j = 0; j < 8; j++)
                {
                    var movesPiece = ChessBoard.GetMovesForPiece(new Point(i, j), board);
                    if (movesPiece.Count > 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public void CheckEndGame()
        {
            if (IsNoMoves(this))
            {
                if (IsCheck(this,WhoseMove))
                {
                    EndGameEvent?.Invoke(WhoseMove);
                }
                else
                {
                    EndGameEvent?.Invoke(null);
                }
            }
        }
        public MoveInfo GetMoveInfo(Point startPoint, Point endPoint)
        {
            var movesForPiece = GetMovesForPiece(startPoint,this);
            if (movesForPiece.ContainsKey((startPoint, endPoint)))
            {
                return movesForPiece[(startPoint, endPoint)];
            }
            return new MoveInfo
            {
                IsMoved = false,
                Move = new ChangePosition(startPoint, endPoint)
            };
        }
        public void Moved(MoveInfo moveInfo)
        {
            Task.Run(() => ChessBoardMovedEvent?.Invoke(moveInfo));
            Task.Run(CheckEndGame);
        }
        public void SetReplasementPiece(Piece? piece)
        {
            if (_moveInfoForReplacePiece is { ReplaceImg: { } replaceImg } moveInfo)
            {
                if (piece is { })
                {
                    _moveInfoForReplacePiece = null;
                    moveInfo.ReplaceImg = (replaceImg.Item1, piece);
                    Board.Move(moveInfo, this);
                    Moved(moveInfo);
                }
                else
                {
                    _moveInfoForReplacePiece = null;
                    var nullMoveInfo = new MoveInfo
                    {
                        IsMoved = false,
                        Move = moveInfo.Move,
                    };
                    Board.Move(nullMoveInfo, this);
                    Moved(nullMoveInfo);
                }
            }

        }
        public void Move(Point startPoint, Point endPoint)
        {
            var moveInfo = GetMoveInfo(startPoint, endPoint);
            if (moveInfo.IsReplacePiece && moveInfo.ReplaceImg is {Item2:null} replaceImg &&
                ArrayBoard[startPoint.X, startPoint.Y] is {} piece)
            {
                ChoiceReplacementPieceEvent?.Invoke(piece.ReplacementPieces, replaceImg.Item1);
                _moveInfoForReplacePiece = moveInfo;
            }
            else
            {
                Board.Move(moveInfo, this);
                Moved(moveInfo);
            }
           
            
        }
        private static Piece?[,] GetNewBoard()
        {
            Piece?[,] board = new Piece?[8, 8];

            #region Создание белой команды

            /*for (int i = 0; i < 8; i++)
            {
                board[1, i] = new WhitePawn(Direction.Up);
            }

            board[0, 0] = new WhiteRook();
            board[0, 7] = new WhiteRook();
            board[0, 1] = new WhiteKnight();
            board[0, 6] = new WhiteKnight();
            board[0, 2] = new WhiteBishop();
            board[0, 5] = new WhiteBishop();
            board[0, 3] = new WhiteKing();
            board[0, 4] = new WhiteQueen();*/

            for (int i = 0; i < 8; i++)
            {
                board[6, i] = new WhitePawn(Direction.Down);
            }

            board[7, 0] = new WhiteRook();
            board[7, 7] = new WhiteRook();
            board[7, 1] = new WhiteKnight();
            board[7, 6] = new WhiteKnight();
            board[7, 2] = new WhiteBishop();
            board[7, 5] = new WhiteBishop();
            board[7, 4] = new WhiteKing();
            board[7, 3] = new WhiteQueen();

            #endregion

            #region Создание черной команды

            /* for (int i = 0; i < 8; i++)
             {
                 board[6, i] = new BlackPawn(Direction.Down);
             }

             board[7, 0] = new BlackRook();
             board[7, 7] = new BlackRook();
             board[7, 1] = new BlackKnight();
             board[7, 6] = new BlackKnight();
             board[7, 2] = new BlackBishop();
             board[7, 5] = new BlackBishop();
             board[7, 4] = new BlackQueen();
             board[7, 3] = new BlackKing();*/

            for (int i = 0; i < 8; i++)
            {
                board[1, i] = new BlackPawn(Direction.Up);
            }

            board[0, 0] = new BlackRook();
            board[0, 7] = new BlackRook();
            board[0, 1] = new BlackKnight();
            board[0, 6] = new BlackKnight();
            board[0, 2] = new BlackBishop();
            board[0, 5] = new BlackBishop();
            board[0, 3] = new BlackQueen();
            board[0, 4] = new BlackKing();

            #endregion

            return board;
        }
        public new object Clone()
        {
            return new ChessBoard((Piece?[,])ArrayBoard.Clone()) 
            {
                WhoseMove = WhoseMove,
                LastMoveInfo = LastMoveInfo,
                Price = Price,
                ChessBoardMovedEvent = ChessBoardMovedEvent,
                ChoiceReplacementPieceEvent = ChoiceReplacementPieceEvent,
                EndGameEvent = EndGameEvent
                
            };
        }
    }


    

}
