using SolarPanelPark.Controls;
using SolarPanelPark.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SolarPanelPark.Commands
{
    class UpdateCurrentViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private INavigator _navigator;

        public UpdateCurrentViewModelCommand(INavigator navigator)
        {
            _navigator = navigator;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(parameter is AvailableViews)
            {
                AvailableViews view = (AvailableViews)parameter;
                switch (view)
                {
                    case AvailableViews.HomePage:
                        _navigator.CurrentViewModel = new HomePageViewModel();
                        break;
                    case AvailableViews.SolarPark:
                        _navigator.CurrentViewModel = new SolarParkViewModel();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}