using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Chess.Models.Boards.Base;
using Chess.Models.PiecesChess.Base;
using Chess.Models.Players.Base;


namespace Chess.Models.Players
{
    internal class BotPlayer:IPlayer
    {
        public event Action<Point, Point>? MovedEvent;
        public event Action<Piece?>? SetSelectedPieceEvent;
        public BotPlayer(TeamEnum team)
        {
            Team = team;
        }
        public double  GetPriceStateBoard(Board board,int depth)
        {
            if (depth == 0)
            {
                return board.Price;
            }
            else
            {
                var allMoves = new List<double>();
                /*if (board.WhoseMove != Team && depth > 1)
                {
                    var moves = GetBestMoves(board, board.WhoseMove, depth>4?4:2);
                    foreach (var moveInfo in moves)
                    {
                        if (board.Clone() is Board copyBoard)
                        {
                            if (moveInfo.IsReplacePiece && moveInfo.ReplaceImg is { Item1:{} point, Item2: null } replaceImg &&
                                copyBoard[moveInfo.Move.StartPoint.X, moveInfo.Move.StartPoint.Y] is { } pieceRep)
                            {
                                if (copyBoard.WhoseMove is TeamEnum.BlackTeam)
                                {
                                    moveInfo.ReplaceImg = (point,
                                        FactoryPiece.GetMovedPiece(pieceRep.ReplacementPieces
                                            .OrderBy(i => i.Price).First()));
                                }
                                else
                                {
                                    moveInfo.ReplaceImg = (point,
                                        FactoryPiece.GetMovedPiece(pieceRep.ReplacementPieces
                                            .OrderBy(i => i.Price).Last()));
                                }
                            }
                            Board.Move(moveInfo, copyBoard);
                            allMoves.Add(GetPriceStateBoard(copyBoard, depth - 1));
                        }
                    }
                    
                }
                else
                {*/
                for (byte i = 0; i < 8; i++)
                {
                    for (byte j = 0; j < 8; j++)
                    {
                        if (board[i, j] is { } piece && piece.Team == board.WhoseMove)
                        {
                            var movesForPiece = board[i, j]?.GetMoves(new Point(i, j), board);
                            if (movesForPiece != null)
                            {
                                foreach (var (_, moveInfo) in movesForPiece)
                                {
                                    if (board.Clone() is Board copyBoard)
                                    {
                                        if (moveInfo.IsReplacePiece && moveInfo.ReplaceImg is { Item1: var point, Item2: null } &&
                                            copyBoard[moveInfo.Move.StartPoint.X, moveInfo.Move.StartPoint.Y] is { } pieceRep)
                                        {
                                            if (copyBoard.WhoseMove is TeamEnum.BlackTeam)
                                            {
                                                moveInfo.ReplaceImg = (point,
                                                    FactoryPiece.GetMovedPiece(pieceRep.ReplacementPieces
                                                        .OrderBy(pieceItem => pieceItem.Price).First()));
                                            }
                                            else
                                            {
                                                moveInfo.ReplaceImg = (point,
                                                    FactoryPiece.GetMovedPiece(pieceRep.ReplacementPieces
                                                        .OrderBy(pieceItem => pieceItem.Price).Last()));
                                            }
                                        }
                                        Board.Move(moveInfo, copyBoard);
                                        allMoves.Add(GetPriceStateBoard(copyBoard, depth - 1));
                                    }
                                }
                            }
                        }
                    }
                }
                /*}*/

                if (board.WhoseMove is TeamEnum.WhiteTeam)
                {
                    return allMoves.Max();
                }
                else
                {
                    return allMoves.Min();
                }
            }
        }
        public List<MoveInfo> GetBestMoves(Board board, TeamEnum team, int depth)
        {
            var allMoves = new List<(MoveInfo, double)>();

            for (byte i = 0; i < 8; i++)
            {
                for (byte j = 0; j < 8; j++)
                {
                    if (board[i, j] is { } piece && piece.Team == team)
                    {
                        var movesForPiece = board[i, j]?.GetMoves(new Point(i, j), board);
                        if (movesForPiece != null)
                        {
                            foreach (var (_, moveInfo) in movesForPiece)
                            {
                                if (board.Clone() is Board copyBoard)
                                {
                                    if (moveInfo.IsReplacePiece && moveInfo.ReplaceImg is { Item1: {} point , Item2: null }  &&
                                        copyBoard[moveInfo.Move.StartPoint.X, moveInfo.Move.StartPoint.Y] is {} pieceRep)
                                    {
                                        if (team is TeamEnum.BlackTeam)
                                        {
                                            moveInfo.ReplaceImg = (point,
                                                FactoryPiece.GetMovedPiece(pieceRep.ReplacementPieces
                                                    .OrderBy(pieceItem => pieceItem.Price).First()));
                                        }
                                        else
                                        {
                                            moveInfo.ReplaceImg = (point,
                                                FactoryPiece.GetMovedPiece(pieceRep.ReplacementPieces
                                                    .OrderBy(pieceItem => pieceItem.Price).Last()));
                                        }
                                    }
                                    Board.Move(moveInfo, copyBoard);
                                    allMoves.Add((moveInfo, GetPriceStateBoard(copyBoard, depth - 1)));
                                }
                            }
                        }
                    }
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

            var bestMoves = allMoves.Where(m => Math.Abs(m.Item2 - bestPrice) < 1).Select(m => m.Item1).ToList();
            
            return bestMoves;
        }
        public TeamEnum Team { get; set; }
        public void CanMovePlayer(Board board)
        {
            Thread.Sleep(200);
            var bestMoves = GetBestMoves(board, Team, 4);
            var (startPoint, endPoint) = bestMoves[(new Random()).Next(0, bestMoves.Count - 1)].Move;
            MovedEvent?.Invoke(startPoint,endPoint);
        }
        public void SelectPiece(ChoicePiece choicePiece)
        {
            if (Team == TeamEnum.WhiteTeam)
            {
                SetSelectedPieceEvent?.Invoke(choicePiece.PiecesList?.OrderBy(piece=>piece.Price).Last());
            }
            else
            {
                SetSelectedPieceEvent?.Invoke(choicePiece.PiecesList?.OrderBy(piece => piece.Price).First());

            }
        }

    }
}
