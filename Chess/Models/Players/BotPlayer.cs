using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Chess.Models.Boards.Base;
using Chess.Models.PiecesChess;
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
                if (piece?.Team == TeamEnum.WhiteTeam)
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
        public int  GetPriceStateBoard(ChessBoard chessBoard,int depth)
        {
            if (depth == 0)
            {
                return chessBoard.Price;
            }
            else
            {
                var allMoves = new List<int>();
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
                                        board.WhoseMove = board.WhoseMove == TeamEnum.WhiteTeam ? TeamEnum.BlackTeam : TeamEnum.WhiteTeam;
                                        allMoves.Add(GetPriceStateBoard(board, depth - 1));
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

        public MoveInfo[] GetBestMoves(ChessBoard chessBoard,TeamEnum team, int depth,out int price)
        {
            var allMoves = new List<(MoveInfo, int)>();

            for (byte i = 0; i < 8; i++)
            {
                for (byte j = 0; j < 8; j++)
                {
                    if (chessBoard[i, j] is { } piece && piece.Team == team)
                    {
                        var movesForPiece = chessBoard[i, j]?.GetMoves(new Point(i, j), ChessBoard);
                        if (movesForPiece != null)
                        {
                            foreach (var (_, moveInfo) in movesForPiece)
                            {
                                if (chessBoard.Clone() is ChessBoard board)
                                {
                                    Board.Move(moveInfo, board);
                                    board.WhoseMove = board.WhoseMove == TeamEnum.WhiteTeam
                                        ? TeamEnum.BlackTeam
                                        : TeamEnum.WhiteTeam;


                                    allMoves.Add((moveInfo, GetPriceStateBoard(board, depth - 1)));
                                }
                            }
                        }
                    }
                }
            }


            int bestPrice;
            if (team is TeamEnum.BlackTeam)
            {
                bestPrice = allMoves.Min(moveInfo => moveInfo.Item2);
            }
            else
            {
                bestPrice = allMoves.Max(moveInfo => moveInfo.Item2);
            }

            var bestMoves = allMoves.Where(m => m.Item2 == bestPrice).Select(m => m.Item1).ToArray();
            price = bestPrice;
            return bestMoves;
        }


        public override void Move()
        {
            var bestMoves = GetBestMoves(ChessBoard,Team,4,out var _);
            if (bestMoves.Length > 0 &&
                bestMoves[(new Random()).Next(0, bestMoves.Length - 1)].ChangePositions?.First() is {} changePosition)
            {
                var (startPoint, endPoint) = changePosition;
                ChessBoard.Move(startPoint, endPoint);
            }
            else
            {
                ChessBoard.Move(new Point(0,0), new Point(0,0));
            }
            
        }
    }
}
