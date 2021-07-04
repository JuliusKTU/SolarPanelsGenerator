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

        const int MAXSIZEOFAREA = 1000;

        private string _overview;
        public string Overview
        {
            get
            {
                return _overview;
            }
            set
            {
                _overview = value;
                OnPropertyChanged(nameof(Overview));
            }
        }

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
                Overview = "Not implemented yet!";
            }
        }

        public ObservableCollection<SolarPanel> SolarPanels { get; set; }
        public ObservableCollection<BorderLine> BorderLines { get; set; }
        public ObservableCollection<BorderLine> RestrictionZones { get; set; }

        #endregion

        public SolarParkViewModel()
        {
            Overview = "Generated 0 solar panels";
            SolarPanels = new ObservableCollection<SolarPanel>();
            BorderLines = new ObservableCollection<BorderLine>();
            RestrictionZones = new ObservableCollection<BorderLine>();

            //Coordinates of the site
            BorderLines.Add(new BorderLine (new Models.Point(83, 136), new Models.Point(124, 186)));
            BorderLines.Add(new BorderLine ( new Models.Point(124, 186), new Models.Point(252, 155)));
            BorderLines.Add(new BorderLine(new Models.Point(252, 155), new Models.Point(277, 47)));
            BorderLines.Add(new BorderLine(new Models.Point(277, 47), new Models.Point(183, 82)));
            BorderLines.Add(new BorderLine(new Models.Point(183, 82), new Models.Point(163, 4)));
            BorderLines.Add(new BorderLine(new Models.Point(163, 4), new Models.Point(80, 25)));
            BorderLines.Add(new BorderLine(new Models.Point(80, 25), new Models.Point(83, 136)));

            //Coordinates of the restriction zone
            RestrictionZones.Add(new BorderLine(new Models.Point(173, 123), new Models.Point(182, 143)));
            RestrictionZones.Add(new BorderLine(new Models.Point(182, 143), new Models.Point(145, 147)));
            RestrictionZones.Add(new BorderLine(new Models.Point(145, 147), new Models.Point(134, 121)));
            RestrictionZones.Add(new BorderLine(new Models.Point(134, 121), new Models.Point(173, 123)));
        }

        public void CalculateSolarPanels()
        {
            Models.Point StartPoint = MinPointOfBorders();
            Models.Point EndPoint = MaxPointOfBorders();

            double X = StartPoint.X;
            double Y = StartPoint.Y;
            SolarPanels.Clear();

            for (int i = (int)StartPoint.Y; i < EndPoint.Y; i++)
            {
                for (int y = (int)StartPoint.X; y < EndPoint.X; y++)
                {
                    var solarPanel = new SolarPanel { X = X, Y = Y, Length = this.Length, Width = this.Width };
                    if (Width!= 0 && Length!= 0 && CheckIfSolarPanelFits(solarPanel))
                    {
                        SolarPanels.Add(solarPanel);
                        X = X + Length + ColumnSpacing;
                    }
                    else
                        X = X + 1;
                }
                X = StartPoint.X;
                Y = Y + Width + RowSpacing;
            }
            Overview = "Generated " + SolarPanels.Count + " solar panels";
        }

        /// <summary>
        /// Check if the given point is inside of borders area
        /// </summary>
        /// <param name="CheckPoint">Paoint to check</param>
        /// <param name="Borders">Zone borders</param>
        /// <returns></returns>
        public static bool CheckIfPointInsideBorders(Models.Point CheckPoint, ObservableCollection<BorderLine> Borders)
        {
            int howManyLeft = 0;
            int howManyRight = 0;
            int howManyTop = 0;
            int howManyBottom = 0;
            foreach (var border in Borders)
            {
                //right
                if (border.doIntersect(new Models.Point(CheckPoint.X, CheckPoint.Y), new Models.Point(CheckPoint.X + MAXSIZEOFAREA, CheckPoint.Y)))
                {
                    howManyRight++;
                }
                //left
                if(border.doIntersect(new Models.Point(CheckPoint.X, CheckPoint.Y), new Models.Point(CheckPoint.X - MAXSIZEOFAREA, CheckPoint.Y)))
                {
                    howManyLeft++;
                }
                //bottom
                if (border.doIntersect(new Models.Point(CheckPoint.X, CheckPoint.Y), new Models.Point(CheckPoint.X, CheckPoint.Y + MAXSIZEOFAREA)))
                {
                    howManyBottom++;
                }
                //top
                if (border.doIntersect(new Models.Point(CheckPoint.X, CheckPoint.Y), new Models.Point(CheckPoint.X, CheckPoint.Y - MAXSIZEOFAREA)))
                {
                    howManyTop++;
                }

            }

            if (howManyLeft > 0 && howManyRight > 0 && howManyTop > 0 && howManyBottom > 0) 
                return true;
            else return false;
        }

        //lets assume that solar panel can not be bigger than restriction zone so let's check if it fits by checking four corners of the solar panel
        public bool CheckIfSolarPanelFits(SolarPanel solarPanel)
        {
            if ((CheckIfPointInsideBorders(new Models.Point(solarPanel.X, solarPanel.Y), BorderLines) && !CheckIfPointInsideBorders(new Models.Point(solarPanel.X, solarPanel.Y), RestrictionZones)) &&
               (CheckIfPointInsideBorders(new Models.Point(solarPanel.X + solarPanel.Length, solarPanel.Y), BorderLines) && !CheckIfPointInsideBorders(new Models.Point(solarPanel.X + solarPanel.Length, solarPanel.Y), RestrictionZones)) &&
               (CheckIfPointInsideBorders(new Models.Point(solarPanel.X, solarPanel.Y + solarPanel.Width), BorderLines) && !CheckIfPointInsideBorders(new Models.Point(solarPanel.X, solarPanel.Y + solarPanel.Width), RestrictionZones)) &&
               (CheckIfPointInsideBorders(new Models.Point(solarPanel.X + solarPanel.Length, solarPanel.Y + solarPanel.Width), BorderLines) && !CheckIfPointInsideBorders(new Models.Point(solarPanel.X + solarPanel.Length, solarPanel.Y + solarPanel.Width), RestrictionZones)))
                return true;
            else return false;
        }

        private Models.Point MaxPointOfBorders()
        {
            Models.Point max = new Models.Point(0, 0);
            foreach (var border in BorderLines)
            {
                if (border.StartPoint.X > max.X)
                    max.X = border.StartPoint.X;
                if (border.EndPoint.X > max.X)
                    max.X = border.EndPoint.X;

                if (border.StartPoint.Y > max.Y)
                    max.Y = border.StartPoint.Y;
                if (border.EndPoint.Y > max.Y)
                    max.Y = border.EndPoint.Y;
            }
            return max;
        }

        private Models.Point MinPointOfBorders()
        {
            Models.Point min = new Models.Point(0, 0);
            foreach (var border in BorderLines)
            {
                if (border.StartPoint.X < min.X)
                    min.X = border.StartPoint.X;
                if (border.EndPoint.X < min.X)
                    min.X = border.EndPoint.X;

                if (border.StartPoint.Y < min.Y)
                    min.Y = border.StartPoint.Y;
                if (border.EndPoint.Y < min.Y)
                    min.Y = border.EndPoint.Y;
            }
            return min;
        }
    }
}
