using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Chess.Models.Boards.Base;



namespace Chess.Models.Players
{
    internal class BotChessPlayer:BotPlayer
    {
        public BotChessPlayer(TeamEnum team,int depth) :base(team, depth)
        {
        }
        public override double  GetPriceStateBoard(Board board,int depth)
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
                            Board.Move(moveInfo, copyBoard);
                            allMoves.Add(GetPriceStateBoard(copyBoard, depth - 1));
                        }
                    }
                    
                }
                else
                {*/
                for (byte i = 0; i < board.Size.Height; i++)
                {
                    for (byte j = 0; j < board.Size.Width; j++)
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
                            }
                        }
                    }
                }
                /*}*/

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
        

    }
}
