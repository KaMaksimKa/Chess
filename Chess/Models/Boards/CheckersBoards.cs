using System;
using System.Collections.Generic;
using System.Drawing;
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
            var moves = base.GetMovesForPiece(startPoint);


            return moves;
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
        public override void MakeMove(Point startPoint, Point endPoint)
        {
            var moveInfo = GetMoveInfo(startPoint, endPoint);
            Board.Move(moveInfo, this);
            ChessBoardMovedEvent?.Invoke(moveInfo);
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
