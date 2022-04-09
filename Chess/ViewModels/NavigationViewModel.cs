using System.Windows.Input;
using Chess.Infrastructure.Commands;
using Chess.ViewModels.Base;

namespace Chess.ViewModels
{
    internal class NavigationViewModel:ViewModel
    {
        #region SelectedViewModel

        private object _selectedViewModel;

        public object SelectedViewModel
        {
            get => _selectedViewModel;
            set => Set(ref _selectedViewModel, value);
        }
        #endregion

        #region Команды

        #region Команда SelectChessGameCommand 
        public ICommand SelectGameChessCommand { get; }

        private bool CanSelectGameChessCommandExecute(object p) => true;

        private void OnSelectGameChessCommandExecuted(object p)
        {
            SelectedViewModel = new GameChessViewModel();
        }

        #endregion

        #region Команда SelectGameChess960Command 
        public ICommand SelectGameChess960Command { get; }

        private bool CanSelectGameChess960CommandExecute(object p) => true;

        private void OnSelectGameChess960CommandExecuted(object p)
        {
            SelectedViewModel = new GameChess960ViewModel();
        }

        #endregion

        #region Команда SelectGameCheckersCommand 
        public ICommand SelectGameCheckersCommand { get; }

        private bool CanSelectGameCheckersCommandExecute(object p) => true;

        private void OnSelectGameCheckersCommandExecuted(object p)
        {
            SelectedViewModel = new GameCheckersViewModel();
        }

        #endregion

        #region Команда SelectGameCheckers10X10Command 
        public ICommand SelectGameCheckers10X10Command { get; }

        private bool CanSelectGameCheckers10X10CommandExecute(object p) => true;

        private void OnSelectGameCheckers10X10CommandExecuted(object p)
        {
            SelectedViewModel = new GameCheckers10X10ViewModel();
        }

        #endregion
        #endregion
        public NavigationViewModel()
        {
            SelectGameChessCommand = new LambdaCommand(OnSelectGameChessCommandExecuted,CanSelectGameChessCommandExecute);
            SelectGameCheckersCommand = new LambdaCommand(OnSelectGameCheckersCommandExecuted,CanSelectGameCheckersCommandExecute);
            SelectGameChess960Command =
                new LambdaCommand(OnSelectGameChess960CommandExecuted, CanSelectGameChess960CommandExecute);
            SelectGameCheckers10X10Command = new LambdaCommand(OnSelectGameCheckers10X10CommandExecuted,
                CanSelectGameCheckers10X10CommandExecute);

            _selectedViewModel = new GameChessViewModel();
        }

        
    }
}
