using System;
using System.Globalization;

namespace Station.Device
{

    public class Parameter
    {

        public Parameter() {

        }


        public string Name
        { get; set; }

        public string Sign
        { get; set; }

        public int Code
        { get; set; }

        public int Precesion
        { get; set; }

        public string UnitName
        { get; set; }

        public double Max
        { get; set; }

        public double Min
        { get; set; }

        public bool Flag
        { get; set; }



        public string FormatValue(double? val)
        {
           

            if (val == null || Double.IsNaN((double)val))

                return "-"; 

            else
            {
                if (Flag)

                    if(Double.IsNaN(Max))
                        return val == null || (double)val > 0 ? "ВКЛ" : "ВЫКЛ";
                     else
                        return val == null || (double)val > 0 ? "ЕСТЬ" : "Нет";

                else
                {
                    string format = $"F{Precesion}";

                    return ((double)val).ToString(format, CultureInfo.InvariantCulture);
                }
            }
        }


    }
}
