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

        #region Команда SelectChessGameCommand 
        public ICommand SelectGameCheckersCommand { get; }

        private bool CanSelectGameCheckersCommandExecute(object p) => true;

        private void OnSelectGameCheckersCommandExecuted(object p)
        {
            SelectedViewModel = new GameCheckersViewModel();
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
        
        #endregion
        public NavigationViewModel()
        {
            SelectGameChessCommand = new LambdaCommand(OnSelectGameChessCommandExecuted,CanSelectGameChessCommandExecute);
            SelectGameCheckersCommand = new LambdaCommand(OnSelectGameCheckersCommandExecuted,CanSelectGameCheckersCommandExecute);
            _selectedViewModel = new GameChessViewModel();
            SelectGameChess960Command =
                new LambdaCommand(OnSelectGameChess960CommandExecuted, CanSelectGameChess960CommandExecute);
        }

        
    }
}
