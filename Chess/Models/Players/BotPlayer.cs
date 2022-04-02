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
    internal class BotPlayer:Player
    {
        public BotPlayer(TeamEnum team,ChessBoard chessBoard) : base(team,chessBoard)
        {
        }
        public static int GetPricePiece(Piece? piece)
        {
            if (piece is { } p)
            {
                if (piece.Team == TeamEnum.WhiteTeam)
                {
                    return p.Price;
                }
                else
                {
                    return -p.Price;
                }
            }
            else
            {
                return 0;
            }
        }
        public double  GetPriceStateBoard(ChessBoard chessBoard,int depth)
        {
            if (depth == 0)
            {
               

                return chessBoard.Price;
            }
            else
            {
                var allMoves = new List<double>();
                if (chessBoard.WhoseMove != Team && depth > 1 && false)
                {
                    var moves = GetBestMoves(chessBoard, chessBoard.WhoseMove, depth>4?4:2);
                    foreach (var moveInfo in moves)
                    {
                        if (chessBoard.Clone() is ChessBoard board)
                        {
                            Board.Move(moveInfo, board);
                            allMoves.Add(GetPriceStateBoard(board, depth - 1));
                        }

                        if (moves[0].KillPoint is { })
                        {

                        }
                    }
                    

                }
                else
                {
                    for (byte i = 0; i < 8; i++)
                    {
                        for (byte j = 0; j < 8; j++)
                        {
                            if (chessBoard[i, j] is { } piece && piece.Team == chessBoard.WhoseMove)
                            {
                                var movesForPiece = chessBoard[i, j]?.GetMoves(new Point(i, j), chessBoard);
                                if (movesForPiece != null)
                                {
                                    foreach (var (_, moveInfo) in movesForPiece)
                                    {
                                        if (chessBoard.Clone() is ChessBoard board)
                                        {
                                            Board.Move(moveInfo, board);
                                            allMoves.Add(GetPriceStateBoard(board, depth - 1));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (chessBoard.WhoseMove is TeamEnum.WhiteTeam)
                {
                    if (allMoves.Count == 0)
                    {
                        return -900;
                    }
                    return allMoves.Max();
                }
                else
                {
                    if (allMoves.Count == 0)
                    {
                        return 900;
                    }
                    return allMoves.Min();
                }
            }
        }

        public List<MoveInfo> GetBestMoves(ChessBoard chessBoard,TeamEnum team, int depth)
        {
            var allMoves = new List<(MoveInfo, double)>();

            for (byte i = 0; i < 8; i++)
            {
                for (byte j = 0; j < 8; j++)
                {
                    if (chessBoard[i, j] is { } piece && piece.Team == team)
                    {
                        var movesForPiece = chessBoard[i, j]?.GetMoves(new Point(i, j), chessBoard);
                        if (movesForPiece != null)
                        {
                            foreach (var (_, moveInfo) in movesForPiece)
                            {
                                if (chessBoard.Clone() is ChessBoard board)
                                {
                                    Board.Move(moveInfo, board);
                                    allMoves.Add((moveInfo, GetPriceStateBoard(board, depth - 1)));
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

            var bestMoves = allMoves.Where(m => Math.Abs(m.Item2 - bestPrice) < 0.1).Select(m => m.Item1).ToList();
            
            return bestMoves;
        }


        public override (Point,Point)? Move()
        {
            Thread.Sleep(200);
            var bestMoves = GetBestMoves(ChessBoard,Team,4);
            if (bestMoves.Count > 0 &&
                bestMoves[(new Random()).Next(0, bestMoves.Count - 1)].ChangePositions?.First() is {} changePosition)
            {
                var (startPoint, endPoint) = changePosition;
                /*ChessBoard.Move(startPoint, endPoint);*/
                return (startPoint, endPoint);
            }
            else
            {
                ChessBoard.Move(new Point(0,0), new Point(0,0));
                return null;
            }
            
        }
    }
}
