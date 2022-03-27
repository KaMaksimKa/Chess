using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chess.Models.PiecesChess;
using Chess.Models.PiecesChess.Base;
using Chess.Models.PiecesChess.DifferentPiece;

namespace Chess.Models.BotChess
{
    internal class BotChess:Player
    {
        public BotChess(TeamEnum team,ChessBoard chessBoard) : base(team,chessBoard)
        {
        }

        public static int  GetPriceStateBoard(ChessBoard chessBoard,int deep)
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
                        for (byte i = 0; i < 8; i++)
                {
                    for (byte j = 0; j < 8; j++)
                    {
                        for (byte h = 0; h < 8; h++)
                        {
                            for (byte k = 0; k < 8; k++)
                            {
                                if (chessBoard.IsMove(new Point(i, j), new Point(h, k)) is { } moveInfo)
                                {
                                    if (chessBoard.Clone() is ChessBoard board)
                                    {
                                        Board.Move(moveInfo,board);
                                        board.WhoseMove = board.WhoseMove == TeamEnum.WhiteTeam ? TeamEnum.BlackTeam : TeamEnum.WhiteTeam;
                                        
                                        int price = GetPriceStateBoard(board,deep-1);
                                        allMoves.Add(price);
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

        

        public List<(ChangePosition,int)> GetAllMoves()
        {
        var allMoves = new List<(ChangePosition, int)>();

        for (byte i = 0; i < 8; i++)
        {
            for (byte j = 0; j < 8; j++)
            {
                for (byte h = 0; h < 8; h++)
                {
                    for (byte k = 0; k < 8; k++)
                    {
                        if (ChessBoard.IsMove(new Point(i, j), new Point(h, k)) is {} moveInfo)
                        {
                            if (ChessBoard.Clone() is ChessBoard board)
                            {
                                Board.Move(moveInfo, board);
                                board.WhoseMove = board.WhoseMove == TeamEnum.WhiteTeam ? TeamEnum.BlackTeam : TeamEnum.WhiteTeam;


                                allMoves.Add((new ChangePosition
                               {
                                   StartPoint = new Point(i,j),
                                   EndPoint = new Point(h,k)
                               },GetPriceStateBoard(board,2))); 
                            }
                        }
                    }
                }
            }
        }
        return allMoves;
        
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
            var allMoveInfos = GetAllMoves();
            if (allMoveInfos.Count != 0)
            {
                int bestPrice;
                if (Team is TeamEnum.BlackTeam)
                {
                     bestPrice = allMoveInfos.Min(moveInfo => moveInfo.Item2);
                }
                else
                {
                     bestPrice = allMoveInfos.Max(moveInfo => moveInfo.Item2);
                }
                
                var bestMoves = allMoveInfos.Where(m => m.Item2 == bestPrice).Select(m => m.Item1).ToArray();
                var (startPoint, endPoint) = bestMoves[(new Random()).Next(0, bestMoves.Length - 1)];
                ChessBoard.Move(startPoint, endPoint);
            }
        }
    }
}
