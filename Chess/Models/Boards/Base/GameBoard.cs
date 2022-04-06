using System;
using System.Drawing;
using Chess.Models.Pieces.Base;

namespace Chess.Models.Boards.Base
{
    internal abstract class GameBoard:Board,IBoardForGame
    {
        protected GameBoard(Piece?[,] arrayBoard) : base(arrayBoard) {}
        public abstract event Action<MoveInfo>? ChessBoardMovedEvent;
        public abstract event Action<TeamEnum?>? EndGameEvent;
        public abstract void Move(Point startPoint, Point endPoint);
        public IHaveIcon?[,] GetIcons()
        {
            return (IHaveIcon?[,])ArrayBoard.Clone();
        }
        public new abstract object Clone();
    }
}
