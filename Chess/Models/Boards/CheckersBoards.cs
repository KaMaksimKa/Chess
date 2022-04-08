using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Chess.Models.Boards.Base;
using Chess.Models.Pieces.Base;

namespace Chess.Models.Boards
{
    internal class CheckersBoards:GameBoard
    {
        public override event Action<MoveInfo>? ChessBoardMovedEvent;
        public override event Action<TeamEnum?>? EndGameEvent;
        public CheckersBoards(TeamEnum team) : base(GetNewBoard(team))
        {
        }
        public CheckersBoards(Piece?[,] arrayBoard) : base(arrayBoard)
        {

        }
        private static Piece?[,] GetNewBoard(TeamEnum teamDown)
        {
            Piece?[,] board = new Piece?[8, 8];

            TeamEnum teamUp = teamDown == TeamEnum.WhiteTeam ? TeamEnum.BlackTeam : TeamEnum.WhiteTeam;

            var factoryDown = FactoryPiece.GetFactory(teamDown, Direction.Down, true);
            var factoryUp = FactoryPiece.GetFactory(teamUp, Direction.Up, true);

            for (int i = 0; i < 3; i ++)
            {
                for (int j = 0; j < 8; j+=2)
                {
                    board[7-i, j+i%2] = factoryDown.GetPiece(TypePiece.Disc);
                }
            }


            for (int i = 0; i < 3; i++)
            {
                for (int j = 1; j < 8; j += 2)
                {
                    board[0 + i, j - i % 2] = factoryUp.GetPiece(TypePiece.Disc);
                }
            }

            return board;
        }
        public override Dictionary<(Point, Point), MoveInfo> GetMovesForPiece(Point? startPoint)
        {
            var allMoves = GetMovesForAllPieces();
            return allMoves.Where(move=>move.Key.Item1==startPoint)
                .ToDictionary(move => move.Key, move => move.Value);
        }
        public override Dictionary<(Point, Point), MoveInfo> GetMovesForAllPieces()
        {
            var moves = base.GetMovesForAllPieces();
            var movesKill = moves.Where(move => move.Value.KillPoint is { })
                .ToDictionary(move => move.Key, move => move.Value);
            if (movesKill.Count > 0)
            {
                if (LastMoveInfo.IsMoved &&
                    this[LastMoveInfo.Move.EndPoint.X, LastMoveInfo.Move.EndPoint.Y] is { } piece &&
                    piece.Team == WhoseMove)
                {
                    var goodMoves = movesKill.Where(move=>move.Key.Item1== LastMoveInfo.Move.EndPoint)
                        .ToDictionary(move => move.Key, move => move.Value);
                    return goodMoves;
                }
                else
                {
                    var goodKillMoves = new Dictionary<(Point, Point), MoveInfo>();
                    var pointsKill = movesKill.Select(move => (move.Key.Item1, move.Value.KillPoint))
                        .Distinct().ToList();

                    foreach (var point in pointsKill)
                    {
                        var killMovesPoint = movesKill
                            .Where(move => move.Key.Item1 == point.Item1 &&
                                           move.Value.KillPoint == point.KillPoint)
                            .ToDictionary(move => move.Key, move => move.Value);
                        if (killMovesPoint.Count() > 1)
                        {
                            bool flagIsAddAll = true;

                            foreach (var killMovePoint in killMovesPoint)
                            {
                                if (this.Clone() is Board board)
                                {
                                    board.Move(killMovePoint.Value);
                                    if (board.WhoseMove == WhoseMove)
                                    {
                                        flagIsAddAll = false;
                                        goodKillMoves.Add(killMovePoint.Key, killMovePoint.Value);
                                    }
                                }
                            }

                            if (flagIsAddAll)
                            {
                                foreach (var killMovePoint in killMovesPoint)
                                {
                                    goodKillMoves.Add(killMovePoint.Key, killMovePoint.Value);
                                }
                            }
                        }
                        else
                        {
                            goodKillMoves.Add(killMovesPoint.First().Key, killMovesPoint.First().Value);
                        }
                    }

                    return goodKillMoves;
                }

            }
            else
            {
                return moves;
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
        public override void Move(MoveInfo moveInfo)
        {
            base.Move(moveInfo);
            if (moveInfo.IsMoved)
            {
                if (moveInfo.KillPoint is {})
                {
                    var moves = GetMovesForPiece(moveInfo.Move.EndPoint);
                    var movesKill = moves.Where(move => move.Value.KillPoint is { })
                        .ToDictionary(move => move.Key, move => move.Value);
                    if (movesKill.Count > 0)
                    {
                        return;
                    }
                }
                WhoseMove = WhoseMove == TeamEnum.WhiteTeam ? TeamEnum.BlackTeam : TeamEnum.WhiteTeam;
            }
        }
        public void CheckEndGame()
        {
            if (IsNoMoves(this))
            {
                int countWhitePieces = 0;
                int countBlackPieces = 0;
                foreach (var piece in ArrayBoard)
                {
                    if (piece?.Team == TeamEnum.WhiteTeam)
                    {
                        countWhitePieces += 1;
                    }
                    else if (piece?.Team == TeamEnum.BlackTeam)
                    {
                        countBlackPieces += 1;
                    }
                }

                if (countBlackPieces == 0)
                {
                    EndGameEvent?.Invoke(TeamEnum.BlackTeam);
                }
                else if (countWhitePieces == 0)
                {
                    EndGameEvent?.Invoke(TeamEnum.WhiteTeam);
                }
                else
                {
                    EndGameEvent?.Invoke(null);
                }
            }
        }
        public override void MakeMove(Point startPoint, Point endPoint)
        {
            var moveInfo = GetMoveInfo(startPoint, endPoint);
            Move(moveInfo);
            Task.Run(() => ChessBoardMovedEvent?.Invoke(moveInfo));
            Task.Run(CheckEndGame);
        }
        public override object Clone()
        {
            return new CheckersBoards((Piece?[,])ArrayBoard.Clone())
            {
                WhoseMove = WhoseMove,
                LastMoveInfo = LastMoveInfo,
                Price = Price,
                ChessBoardMovedEvent = ChessBoardMovedEvent,
                EndGameEvent = EndGameEvent
            };
        }
    }
}
