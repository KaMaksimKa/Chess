using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Chess.Models.Boards.Base;
using Chess.Models.Pieces.Base;
using Chess.Models.Players.Base;

namespace Chess.Models.Players
{
    internal class BotPlayer:IPlayer
    {
        public event Action<Point, Point>? MovedEvent;
        public event Action<Piece?>? SetSelectedPieceEvent;
        private readonly int _depth;
        public BotPlayer(TeamEnum team, int depth)
        {
            Team = team;
            _depth = depth;
        }
        public virtual double GetPriceStateBoard(Board board, int depth)
        {
            if (depth == 0)
            {
                return board.Price;
            }
            else
            {
                var allMoves = new List<double>();

                foreach (var (_, moveInfo) in board.GetMovesForAllPieces())
                {
                    if (board.Clone() is Board copyBoard)
                    {
                        if (moveInfo.IsReplacePiece && moveInfo.ReplaceImg is { Item1: var point, Item2: null } &&
                            copyBoard[moveInfo.Move.StartPoint.X, moveInfo.Move.StartPoint.Y] is { } pieceRep)
                        {
                            if (copyBoard.WhoseMove is TeamEnum.BlackTeam)
                            {
                                var selectPiece = pieceRep.ReplacementPieces
                                    .OrderBy(pieceItem => pieceItem.Price).First();
                                moveInfo.ReplaceImg = (point, selectPiece);
                            }
                            else
                            {
                                var selectPiece = pieceRep.ReplacementPieces
                                    .OrderBy(pieceItem => pieceItem.Price).Last();
                                moveInfo.ReplaceImg = (point, selectPiece);
                            }
                        }
                        copyBoard.Move(moveInfo);
                        allMoves.Add(GetPriceStateBoard(copyBoard, depth - 1));
                    }
                }

                if (board.WhoseMove is TeamEnum.WhiteTeam)
                {
                    if (allMoves.Count > 0)
                    {
                        return allMoves.Max();
                    }
                    else
                    {
                        return -900;
                    }
                }
                else
                {
                    if (allMoves.Count > 0)
                    {
                        return allMoves.Min();
                    }
                    else
                    {
                        return 900;
                    }
                }
            }
        }
        public List<MoveInfo> GetBestMoves(Board board, TeamEnum team, int depth)
        {
            var allMoves = new List<(MoveInfo, double)>();

            foreach (var (_, moveInfo) in board.GetMovesForAllPieces())
            {
                if (board.Clone() is Board copyBoard)
                {
                    if (moveInfo.IsReplacePiece && moveInfo.ReplaceImg is { Item1: { } point, Item2: null } &&
                        copyBoard[moveInfo.Move.StartPoint.X, moveInfo.Move.StartPoint.Y] is { } pieceRep)
                    {
                        if (team is TeamEnum.BlackTeam)
                        {
                            var piece = pieceRep.ReplacementPieces
                                .OrderBy(pieceItem => pieceItem.Price).First();
                            moveInfo.ReplaceImg = (point,
                                FactoryPiece.GetPiece(piece.TypePiece, piece.Team, piece.Direction, false));
                        }
                        else
                        {
                            var piece = pieceRep.ReplacementPieces
                                .OrderBy(pieceItem => pieceItem.Price).Last();
                            moveInfo.ReplaceImg = (point,
                                FactoryPiece.GetPiece(piece.TypePiece, piece.Team, piece.Direction, false));
                        }
                    }
                    copyBoard.Move(moveInfo);
                    allMoves.Add((moveInfo, GetPriceStateBoard(copyBoard, depth - 1)));
                }
            }

            double bestPrice;
            if (team is TeamEnum.BlackTeam)
            {
                if (allMoves.Count > 0)
                {
                    bestPrice = allMoves.Min(moveInfo => moveInfo.Item2);
                }
                else
                {
                    return new List<MoveInfo>();
                }
            }
            else
            {
                if (allMoves.Count > 0)
                {
                    bestPrice = allMoves.Max(moveInfo => moveInfo.Item2);
                }
                else
                {
                    return new List<MoveInfo>();
                }

            }

            var bestMoves = allMoves.Where(m => Math.Abs(m.Item2 - bestPrice) < 2).Select(m => m.Item1).ToList();
            if (bestMoves.Count == 0)
            {

            }
            return bestMoves;
        }
        public TeamEnum Team { get; init; }
        public void CanMovePlayer(Board board)
        {
            Thread.Sleep(250);
            var bestMoves = GetBestMoves(board, Team, _depth);
            if (bestMoves.Count > 0)
            {
                var (startPoint, endPoint) = bestMoves[(new Random()).Next(0, bestMoves.Count - 1)].Move;
                MovedEvent?.Invoke(startPoint, endPoint);
            }
            else
            {
                MovedEvent?.Invoke(Point.Empty, Point.Empty);
            }
        }
        public void SelectPiece(ChoicePiece choicePiece)
        {
            SetSelectedPieceEvent?.Invoke(Team == TeamEnum.WhiteTeam
                ? choicePiece.PiecesList?.OrderBy(piece => piece.Price).Last()
                : choicePiece.PiecesList?.OrderBy(piece => piece.Price).First());
        }
    }
}
