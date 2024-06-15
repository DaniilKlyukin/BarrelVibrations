using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicLibraryWinForm
{
    public class HeatMap
    {
        public static Color MapHeatColor(
           double value, double blueValue, double redValue)
        {
            if (value >= redValue)
                value = redValue;

            if (value <= blueValue)
                value = blueValue;

            if (Math.Abs(blueValue - redValue) < 1e-50)
            {
                blueValue -= 1;
                redValue += 1;
            }

            if (!double.IsFinite(value) || !double.IsFinite(blueValue) || !double.IsFinite(redValue))
            {
                return Color.Black;
            }

            // Convert into a value between 0 and 1023.
            var intValue = (int)(1023 * (value - redValue) /
                                 (blueValue - redValue));

            // Map different color bands.
            if (intValue < 256)
                // Red to yellow. (255, 0, 0) to (255, 255, 0).
                return Color.FromArgb(255, intValue, 0);

            if (intValue < 512)
            {
                // Yellow to green. (255, 255, 0) to (0, 255, 0).
                intValue -= 256;
                return Color.FromArgb(255 - intValue, 255, 0);
            }

            if (intValue < 768)
            {
                // Green to aqua. (0, 255, 0) to (0, 255, 255).
                intValue -= 512;
                return Color.FromArgb(0, 255, intValue);
            }

            // Aqua to blue. (0, 255, 255) to (0, 0, 255).
            intValue -= 768;
            return Color.FromArgb(0, 255 - intValue, 255);
        }
    }
}
