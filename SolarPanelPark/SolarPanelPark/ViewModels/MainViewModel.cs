using SolarPanelPark.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarPanelPark.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public INavigator Navigator { get; set; } = new Navigator();
        public MainViewModel()
        {
            Navigator.CurrentViewModel = new HomePageViewModel(); //default View model when program started
        }
    }
}
