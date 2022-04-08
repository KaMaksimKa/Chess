using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Chess.Models.Boards.Base;
using Chess.Models.Pieces.Base;
using Chess.Models.Players.Base;

namespace Chess.Models.Players
{
    internal class SelfPlayer:IPlayer
    {
        public event Action<Point, Point>? MovedEvent;
        public event Action<Piece?>? SetSelectedPieceEvent;
        public event Action<ChoicePiece>? GetSelectedPieceEvent;
        public event Action<HintsChess>? SetHintsForMoveEvent;


        private Board? _board;
        public TeamEnum Team { get; init; }

        #region Свойство StartPoint
        private Point? _startPoint;
        public Point? StartPoint
        {
            get => _startPoint;
            set
            {
                _startPoint = value;
                Task.Run(() =>
                {
                    SetHintsForMoveEvent?.Invoke(GetNewHintsChessAsync(value));
                });
            }
        }

        #endregion

        #region Свойство EndPoint
        private Point? _endPoint;
        public Point? EndPoint
        {
            get => _endPoint;
            set
            {
                _endPoint = value;
                if (StartPoint is { } startPoint && value is { } endPoint)
                {
                    Task.Run(() => MovedEvent?.Invoke(startPoint, endPoint)) ;
                    
                }
            }

        }
        #endregion

        public SelfPlayer(TeamEnum team)
        {
            Team = team;
        }
        public void CanMovePlayer(Board board)
        {
            _board = board;
        }
        public void SelectPiece(ChoicePiece choicePiece)
        {
            GetSelectedPieceEvent?.Invoke(choicePiece);
        }
        public void SetSelectPiece(ChoicePiece choicePiece)
        {
            if (choicePiece.IndexReplacementPiece is { } index && choicePiece.PiecesList != null)
            {
                if (index != -1)
                {
                    SetSelectedPieceEvent?.Invoke(choicePiece.PiecesList[index]);
                }
                else
                {
                    SetSelectedPieceEvent?.Invoke(null);
                }

            }
        }
        private HintsChess GetNewHintsChessAsync(Point? startPoint)
        {
            List<Point> hintsForMove = new List<Point>();
            List<Point> hintsForKill = new List<Point>();

            if (_board is { } board)
            {
                var moves = board.GetMovesForPiece(startPoint);
                foreach (var (_, moveInfo) in moves)
                {
                    if (moveInfo.KillPoint != null)
                    {
                        hintsForKill.Add(moveInfo.Move.EndPoint);
                    }
                    else if (moveInfo.ChangePositions != null)
                    {
                        hintsForMove.Add(moveInfo.Move.EndPoint);
                    }
                }
            }
            
            return new HintsChess { IsHintsForKill = hintsForKill, IsHintsForMove = hintsForMove };
        }
    }
}
