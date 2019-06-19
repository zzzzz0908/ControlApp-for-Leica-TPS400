using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class SettingsVM
    {
        public int EDMmode { get; set; }
        public int AngleUnit { get; set; }
        public int DisplayLight { get; set; }
        public int LaserPointer { get; set; }


        public Dictionary<string, int> EDMmodeList { get; set; }
        public Dictionary<string, int> AngleUnitList { get; set; }
        public Dictionary<string, int> DisplayLightList { get; set; }
        public Dictionary<string, int> LaserPointerList { get; set; }



        public SettingsVM()
        {
            EDMmodeList = new Dictionary<string, int>()
            {
                {"IR-Точно", 0 },
                {"IR-Fast", 1 },
                {"IR-Track", 5 },
                {"IR-Tape", 10 },
                {"RL-Short", 7 },
                {"RL-Long", 6 },
                {"RL-Track", 9 }
            };

            AngleUnitList = new Dictionary<string, int>()
            {
                {"GON", 0 },
                {"Градусы десятичные", 1 },
                {"Гр-Мин-Сек", 2 },
                {"Тысячные", 3 }                
            };
            
            DisplayLightList = new Dictionary<string, int>()
            {
                {"Выключена", 0 },
                {"Низкая", 1 },
                {"Средняя", 2 },
                {"Высокая", 3 }
            };

            LaserPointerList = new Dictionary<string, int>()
            {
                {"Выключен", 0 },
                {"Включен", 1 }
            };

        }
    }
}
