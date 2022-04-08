using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Chess.Models.Boards.Base;
using Chess.Models.Pieces.Base;
using Chess.Models.Pieces.PiecesChess.DifferentPiece;

namespace Chess.Models.Boards
{
    internal class ChessBoard: GameBoard
    {
        public override event Action<MoveInfo>? ChessBoardMovedEvent;
        public event Action<List<Piece>,Point>? ChoiceReplacementPieceEvent;
        public override event Action<TeamEnum?>? EndGameEvent; 

        private MoveInfo? _moveInfoForReplacePiece;
        public ChessBoard(TeamEnum team) :base(GetNewBoard(team))
        {

        }
        public ChessBoard(Piece?[,] arrayBoard) :base(arrayBoard)
        {
            
        }
        public override Dictionary<(Point, Point), MoveInfo> GetMovesForPiece(Point? startPoint)
        {
            var goodMoves = new Dictionary<(Point, Point), MoveInfo>();
            
            var moves = base.GetMovesForPiece(startPoint);
            foreach (var move in moves)
            {
                if (this.Clone() is Board boardClone)
                {
                    boardClone.Move(move.Value);
                    if (!IsCheck(boardClone, WhoseMove))
                    {
                        goodMoves.Add(move.Key,move.Value);
                    }
                }
            }
            

            return goodMoves;
        }
        public override Dictionary<(Point, Point), MoveInfo> GetMovesForAllPieces()
        {
            var goodMoves = new Dictionary<(Point, Point), MoveInfo>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var movesPiece = GetMovesForPiece(new Point(i, j));
                    foreach (var move in movesPiece)
                    {
                        goodMoves.Add(move.Key,move.Value);
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
            var movesForPiece = GetMovesForPiece(startPoint);
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
                    Move(moveInfo);
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
                    Move(nullMoveInfo);
                    Moved(nullMoveInfo);
                }
            }

        }
        public override void Move(MoveInfo moveInfo)
        {
            base.Move(moveInfo);
            if (moveInfo.IsMoved)
            {
                WhoseMove = WhoseMove == TeamEnum.WhiteTeam ? TeamEnum.BlackTeam : TeamEnum.WhiteTeam;
            }
        }
        public override void MakeMove(Point startPoint, Point endPoint)
        {
            var moveInfo = GetMoveInfo(startPoint, endPoint);
            if (moveInfo.IsReplacePiece && moveInfo.ReplaceImg is {Item2:null} replaceImg &&
                ArrayBoard[startPoint.X, startPoint.Y] is {} piece)
            {
                _moveInfoForReplacePiece = moveInfo;
                ChoiceReplacementPieceEvent?.Invoke(piece.ReplacementPieces, replaceImg.Item1);
            }
            else
            {
                Move(moveInfo);
                Moved(moveInfo);
            }
           
            
        }
        private static Piece?[,] GetNewBoard(TeamEnum teamDown)
        {
            
            Piece?[,] board = new Piece?[8, 8];

            TeamEnum teamUp = teamDown == TeamEnum.WhiteTeam ? TeamEnum.BlackTeam : TeamEnum.WhiteTeam;

            var factoryDown = FactoryPiece.GetFactory(teamDown, Direction.Down, true);
            var factoryUp = FactoryPiece.GetFactory(teamUp, Direction.Up, true);

            for (int i = 0; i < 8; i++)
            {
                board[6, i] = factoryDown.GetPiece(TypePiece.Pawn);
            }

            board[7, 0] = factoryDown.GetPiece(TypePiece.Rook);
            board[7, 7] = factoryDown.GetPiece(TypePiece.Rook);
            board[7, 1] = factoryDown.GetPiece(TypePiece.Knight);
            board[7, 6] = factoryDown.GetPiece(TypePiece.Knight);
            board[7, 2] = factoryDown.GetPiece(TypePiece.Bishop);
            board[7, 5] = factoryDown.GetPiece(TypePiece.Bishop);
            if (factoryDown.Team == TeamEnum.WhiteTeam)
            {
                board[7, 4] = factoryDown.GetPiece(TypePiece.King);
                board[7, 3] = factoryDown.GetPiece(TypePiece.Queen);
            }
            else
            {
                board[7, 3] = factoryDown.GetPiece(TypePiece.King);
                board[7, 4] = factoryDown.GetPiece(TypePiece.Queen);
            }





            for (int i = 0; i < 8; i++)
            {
                board[1, i] = factoryUp.GetPiece(TypePiece.Pawn);
            }

            board[0, 0] = factoryUp.GetPiece(TypePiece.Rook);
            board[0, 7] = factoryUp.GetPiece(TypePiece.Rook);
            board[0, 1] = factoryUp.GetPiece(TypePiece.Knight);
            board[0, 6] = factoryUp.GetPiece(TypePiece.Knight);
            board[0, 2] = factoryUp.GetPiece(TypePiece.Bishop);
            board[0, 5] = factoryUp.GetPiece(TypePiece.Bishop);
            if (factoryUp.Team == TeamEnum.BlackTeam)
            {
                board[0, 4] = factoryUp.GetPiece(TypePiece.King);
                board[0, 3] = factoryUp.GetPiece(TypePiece.Queen);
            }
            else
            {
                board[0, 3] = factoryUp.GetPiece(TypePiece.King);
                board[0, 4] = factoryUp.GetPiece(TypePiece.Queen);
            }



            return board;
        }
        public override object Clone()
        {
            return new ChessBoard((Piece?[,])ArrayBoard.Clone()) 
            {
                WhoseMove = WhoseMove,
                LastMoveInfo = LastMoveInfo,
                Price = Price,
                ChessBoardMovedEvent = ChessBoardMovedEvent,
                ChoiceReplacementPieceEvent = ChoiceReplacementPieceEvent,
                EndGameEvent = EndGameEvent,
                _moveInfoForReplacePiece = _moveInfoForReplacePiece
                
            };
        }
    }


    

}
