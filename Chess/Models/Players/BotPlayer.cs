using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        public int  GetPriceStateBoard(ChessBoard chessBoard,int deep)
        {
            if (deep == 0)
            {
                int price = 0;
                for (byte i = 0; i < 8; i++)
                {
                    for (byte j = 0; j < 8; j++)
                    {
                        price += GetPricePiece(chessBoard[i, j]);
                    }
                }

                return price;
            }
            else
            {
                var allMoves = new List<int>();
                if (chessBoard.WhoseMove != Team && deep > 1)
                {
                    var moves = new List<(MoveInfo, int)>();
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
                                            moves.Add((moveInfo, GetPriceStateBoard(board, 0)));
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (chessBoard.WhoseMove == TeamEnum.WhiteTeam)
                    {
                        moves = moves.Where(i => i.Item2 == moves.Max(j => j.Item2)).ToList();
                    }
                    else
                    {
                        moves = moves.Where(i => i.Item2 == moves.Min(j => j.Item2)).ToList();

                    }
                    foreach (var (moveInfo, _) in moves)
                    {
                        if (chessBoard.Clone() is ChessBoard board)
                        {
                            Board.Move(moveInfo, board);
                            board.WhoseMove = board.WhoseMove == TeamEnum.WhiteTeam ? TeamEnum.BlackTeam : TeamEnum.WhiteTeam;
                            allMoves.Add(GetPriceStateBoard(board, deep - 1));
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
                                            board.WhoseMove = board.WhoseMove == TeamEnum.WhiteTeam ? TeamEnum.BlackTeam : TeamEnum.WhiteTeam;
                                            allMoves.Add(GetPriceStateBoard(board, deep - 1));
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
        public (Point,Point) GetBestMove()
        {
        var allMoves = new List<(ChangePosition, int)>();

        for (byte i = 0; i < 8; i++)
        {
            for (byte j = 0; j < 8; j++)
            {
                if (ChessBoard[i, j] is { } piece && piece.Team == Team)
                {
                    var movesForPiece = ChessBoard[i, j]?.GetMoves(new Point(i, j), ChessBoard);
                    if (movesForPiece != null)
                    {
                        foreach (var ((startPoint, endPoint), moveInfo) in movesForPiece)
                        {
                            if (ChessBoard.Clone() is ChessBoard board)
                            {
                                Board.Move(moveInfo, board);
                                board.WhoseMove = board.WhoseMove == TeamEnum.WhiteTeam ? TeamEnum.BlackTeam : TeamEnum.WhiteTeam;


                                allMoves.Add((new ChangePosition
                                {
                                    StartPoint = startPoint,
                                    EndPoint = endPoint
                                }, GetPriceStateBoard(board, 3)));
                            }
                        }
                    }
                }
            }
        }

        if (allMoves.Count != 0)
        {
            int bestPrice;
            if (Team is TeamEnum.BlackTeam)
            {
                bestPrice = allMoves.Min(moveInfo => moveInfo.Item2);
            }
            else
            {
                bestPrice = allMoves.Max(moveInfo => moveInfo.Item2);
            }

            var bestMoves = allMoves.Where(m => m.Item2 == bestPrice).Select(m => m.Item1).ToArray();
            var (startPoint, endPoint) = bestMoves[(new Random()).Next(0, bestMoves.Length - 1)];
            return (startPoint, endPoint);
        }

        return (new Point(), new Point());

        }
        public static int GetPricePiece(Piece? piece)
        {
            return piece switch
            {
                WhiteKing => 900,
                WhiteQueen => 90,
                WhiteRook => 50,
                WhiteBishop => 30,
                WhiteKnight => 30,
                WhitePawn => 10,
                BlackKing => -900,
                BlackQueen => -90,
                BlackRook => -50,
                BlackBishop => -30,
                BlackKnight => -30,
                BlackPawn => -10,
                _ => 0
            };
        }
        public override void Move()
        {
            var (startPoint, endPoint) = GetBestMove();
            ChessBoard.Move(startPoint, endPoint);
        }
    }
}
