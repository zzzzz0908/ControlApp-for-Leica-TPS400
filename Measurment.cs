using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Measurment : INotifyPropertyChanged
    {
        private string _id;
        private double _hzAngle;
        private double _vAngle;
        private double _slopeDist;
        private double _reflHeight;
        private double _instrHeight;
        private string HzString;
        private string VAString;

        private double _northing;
        private double _easting;
        private double _height;

        public string data { get; private set; }


        public event PropertyChangedEventHandler PropertyChanged;

        public Measurment(string Data)
        {
            data = Data.Remove(Data.Length - 4);

            string[] values = data.Split(' ');

            _id = values[0].Substring(7).TrimStart('0');
            _slopeDist = Double.Parse(values[3].Substring(7).TrimStart('0')) / 1000;

            string temp = values[4].Substring(6);
            _easting = Double.Parse(temp) / 1000;

            temp = values[5].Substring(6);
            _northing = Double.Parse(temp) / 1000;

            temp = values[6].Substring(6);
            _height = Double.Parse(temp) / 1000;



            temp = values[7].Substring(7).TrimStart('0');
            if (temp == "") temp = "0";
            _reflHeight = Double.Parse(temp) / 1000;


            //temp = values[6].Substring(7).TrimStart('0');
            //if (temp == "") temp = "0";
            //_instrHeight = Double.Parse(temp) / 1000;


            if (values[1][5] == '4')
            {
                temp = values[1].Substring(values[1].Length - 8);
                string deg = temp.Substring(0, 3);
                string min = temp.Substring(3, 2);
                string sec = temp.Substring(5, 2);
                HzString = $"{deg}° {min}' {sec}\"";
                _hzAngle = double.Parse(deg) + double.Parse(min) / 60 + double.Parse(sec) / 3600;
            } else
            {
                _hzAngle = double.Parse(values[1].Substring(values[1].Length - 8)) / 100000;
                HzString = _hzAngle.ToString();
            }

            if (values[2][5] == '4')
            {
                temp = values[2].Substring(values[2].Length - 8);
                string deg = temp.Substring(0, 3);
                string min = temp.Substring(3, 2);
                string sec = temp.Substring(5, 2);
                VAString = $"{deg}° {min}' {sec}\"";
                _vAngle = double.Parse(deg) + double.Parse(min) / 60 + double.Parse(sec) / 3600;
            }
            else
            {
                _hzAngle = double.Parse(values[2].Substring(values[2].Length - 8)) / 100000;
                VAString = _hzAngle.ToString();
            }

            //_northing = northing;
            //_easting = easting;
            //_height = height;
        }


        public string HZ
        {
            get { return HzString; }
            set
            {
                HzString = value;
                RaisePropertyChanged(nameof(HZ));
            }
        }

        public string VA
        {
            get { return VAString; }
            set
            {
                VAString = value;
                RaisePropertyChanged(nameof(VA));
            }
        }

        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;               
                RaisePropertyChanged(nameof(Id));
            }
        }

        public double HzAngle
        {
            get { return _hzAngle; }
            set
            {
                _hzAngle = value;
                RaisePropertyChanged(nameof(HzAngle));
            }
        }

        public double VAngle
        {
            get { return _vAngle; }
            set
            {
                _vAngle = value;
                RaisePropertyChanged(nameof(VAngle));
            }
        }

        public double SlopeDist
        {
            get { return _slopeDist; }
            set
            {
                _slopeDist = value;
                RaisePropertyChanged(nameof(SlopeDist));
            }
        }

        public double ReflHeight
        {
            get { return _reflHeight; }
            set
            {
                _reflHeight = value;
                RaisePropertyChanged(nameof(ReflHeight));
            }
        }

        public double InstrHeight
        {
            get { return _instrHeight; }
            set
            {
                _instrHeight = value;
                RaisePropertyChanged(nameof(InstrHeight));
            }
        }




        #region coords
        public double Northing
        {
            get { return _northing; }
            set
            {
                _northing = value;
                RaisePropertyChanged(nameof(Northing));
            }
        }
        public double Easting
        {
            get { return _easting; }
            set
            {
                _easting = value;
                RaisePropertyChanged(nameof(Easting));
            }
        }
        public double Height
        {
            get { return _height; }
            set
            {
                _height = value;
                RaisePropertyChanged(nameof(Height));
            }
        }
        #endregion


        private void RaisePropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
