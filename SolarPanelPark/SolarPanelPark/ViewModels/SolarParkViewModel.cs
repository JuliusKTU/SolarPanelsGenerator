using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media;
using SolarPanelPark.Models;
using System.Collections.ObjectModel;

namespace SolarPanelPark.ViewModels
{
    public class SolarParkViewModel : BaseViewModel
    {
        #region DEFINITIONS
        private double _width;
        public double Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                OnPropertyChanged(nameof(Width));
                CalculateSolarPanels();
            }
        }

        private double _length;
        public double Length
        {
            get
            {
                return _length;
            }
            set
            {
                _length = value;
                OnPropertyChanged(nameof(Length));
                CalculateSolarPanels();
            }
        }
        private double _rowSpacing;
        public double RowSpacing
        {
            get
            {
                return _rowSpacing;
            }
            set
            {
                _rowSpacing = value;
                OnPropertyChanged(nameof(RowSpacing));
                CalculateSolarPanels();
            }
        }
        private double _columnSpacing;
        public double ColumnSpacing
        {
            get
            {
                return _columnSpacing;
            }
            set
            {
                _columnSpacing = value;
                OnPropertyChanged(nameof(ColumnSpacing));
                CalculateSolarPanels();
            }
        }
        private double _tiltAngle;
        public double TiltAngle
        {
            get
            {
                return _tiltAngle;
            }
            set
            {
                _tiltAngle = value;
                OnPropertyChanged(nameof(TiltAngle));
                CalculateSolarPanels();
            }
        }

        public ObservableCollection<SolarPanel> SolarPanels { get; set; }
        public ObservableCollection<BorderLine> BorderLines { get; set; }
        public ObservableCollection<BorderLine> RestrictionZones { get; set; }

        #endregion

        public SolarParkViewModel()
        {
            SolarPanels = new ObservableCollection<SolarPanel>();
            BorderLines = new ObservableCollection<BorderLine>();
            RestrictionZones = new ObservableCollection<BorderLine>();


            //Coordinates of the site
            BorderLines.Add(new BorderLine { StartX = 83, StartY = 136, EndX = 124, EndY = 186 });
            BorderLines.Add(new BorderLine { StartX = 124, StartY = 186, EndX = 252, EndY = 155 });
            BorderLines.Add(new BorderLine { StartX = 252, StartY = 155, EndX = 277, EndY = 47 });
            BorderLines.Add(new BorderLine { StartX = 277, StartY = 47, EndX = 183, EndY = 82 });
            BorderLines.Add(new BorderLine { StartX = 183, StartY = 82, EndX = 163, EndY = 4 });
            BorderLines.Add(new BorderLine { StartX = 163, StartY = 4, EndX = 80, EndY = 25 });
            BorderLines.Add(new BorderLine { StartX = 80, StartY = 25, EndX = 83, EndY = 136 });

            //Coordinates of the restriction zone
            RestrictionZones.Add(new BorderLine { StartX = 173, StartY = 123, EndX = 182, EndY = 143 });
            RestrictionZones.Add(new BorderLine { StartX = 182, StartY = 143, EndX = 145, EndY = 147 });
            RestrictionZones.Add(new BorderLine { StartX = 145, StartY = 147, EndX = 134, EndY = 121 });
            RestrictionZones.Add(new BorderLine { StartX = 134, StartY = 121, EndX = 173, EndY = 123 });
        }

        public void CheckIfPointInsidePark(double X, double Y, ObservableCollection<BorderLine> borders)
        {
            bool hasTopLine = false;
            bool hasLeftLine = false;
            bool hasRightLine = false;
            bool hasBottomLine = false;

            foreach (var border in borders)
            {
                
            }
        }

        public void CheckIfSolarPanelFits(SolarPanel solarPanel, ObservableCollection<BorderLine> borders)
        {

        }

        public void CalculateSolarPanels()
        {
            //the filtration of not fitted solar panels is not calculated yet
            double StartX = 80;
            double StartY = 4;

            double X = StartX;
            double Y = StartY;
            SolarPanels.Clear();

            for(int i=1; i<10; i++)  //the total number of rows is not calculated yet
            {
                for (int y = 0; y< 10; y++)   //the total number of columns is not calculated yet
                {
                    SolarPanels.Add(new SolarPanel { X = X, Y = Y, Width = Width, Height = Length });
                    X = X + Length + ColumnSpacing;
                }
                X = StartX;
                Y = Y + Width + RowSpacing;
            }
            
        }
    }
}
