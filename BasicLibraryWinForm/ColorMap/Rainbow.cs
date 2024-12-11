using Newtonsoft.Json.Linq;

namespace BasicLibraryWinForm.ColorMap
{
    public static class Rainbow
    {
        private class ColorRange
        {
            public ColorPoint From { get; set; }
            public ColorPoint To { get; set; }
        }

        private class ColorPoint
        {
            public double Value { get; set; }
            public Color Color { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="progress">value from 0 to 1</param>
        /// <returns></returns>
        public static Color Map(double progress)
        {
            const int max = 255, min = 178;

            var colors = new List<Color>
            {
            Color.FromArgb(0, 0, max),   // Синий
            Color.FromArgb(0, min, max), // Синий-Голубой
            Color.FromArgb(0, max, max), // Голубой
            Color.FromArgb(0, max, min), // Голубой-Зелёный
            Color.FromArgb(0, max, 0), // Зелёный
            Color.FromArgb(min, max, 0), // Лайм
            Color.FromArgb(max, max, 0), // Желтый
            Color.FromArgb(max, min, 0),     // Оранжевый
            Color.FromArgb(max, 0, 0)     // Красный
            };

            var step = 1.0 / colors.Count;

            var ranges = new List<ColorRange>();

            ranges.Add(
                new ColorRange
                {
                    From = new ColorPoint { Value = 0, Color = colors.First() },
                    To = new ColorPoint { Value = step / 2, Color = colors.First() }
                });

            for (int i = 0; i < colors.Count - 1; i++)
            {
                ranges.Add(
                    new ColorRange
                    {
                        From = new ColorPoint { Value = ranges[i].To.Value, Color = colors[i] },
                        To = new ColorPoint { Value = ranges[i].To.Value + step, Color = colors[i + 1] }
                    });
            }

            ranges.Add(
                 new ColorRange
                 {
                     From = new ColorPoint { Value = 1 - step / 2, Color = colors.Last() },
                     To = new ColorPoint { Value = 1, Color = colors.Last() }
                 });

            for (int i = 0; i < ranges.Count; i++)
            {
                var l = ranges[i].From.Value;
                var r = ranges[i].To.Value;

                // Переход между цветами
                if (progress <= ranges[i].To.Value && progress >= ranges[i].From.Value)
                {
                    var R = (int)(Algebra.GetValueAtLine(progress, l, ranges[i].From.Color.R, r, ranges[i].To.Color.R) * 0.67);
                    var G = (int)(Algebra.GetValueAtLine(progress, l, ranges[i].From.Color.G, r, ranges[i].To.Color.G) * 0.67);
                    var B = (int)(Algebra.GetValueAtLine(progress, l, ranges[i].From.Color.B, r, ranges[i].To.Color.B) * 0.67);

                    return Color.FromArgb(R, G, B);
                }
            }

            return Color.Black;
        }

        // Метод для интерполяции между двумя цветами
        private static Color InterpolateColors(Color color1, Color color2, double factor)
        {
            int r = Math.Clamp((int)(color1.R + (color2.R - color1.R) * factor), 0, 200);
            int g = Math.Clamp((int)(color1.G + (color2.G - color1.G) * factor), 0, 200);
            int b = Math.Clamp((int)(color1.B + (color2.B - color1.B) * factor), 0, 200);


            return Color.FromArgb(r, g, b);
        }

        public static Color Map(double value, double min, double max)
        {
            return Map((value - min) / (max - min));
        }

        // Given a Color (RGB Struct) in range of 0-255
        // Return H,S,L in range of 0-1
        public static void RGB2HSL(ColorRGB rgb, out double h, out double s, out double l)
        {

            double r = rgb.R / 255.0;
            double g = rgb.G / 255.0;
            double b = rgb.B / 255.0;

            double v;
            double m;
            double vm;
            double r2, g2, b2;

            h = 0; // default to black
            s = 0;
            l = 0;

            v = Math.Max(r, g);
            v = Math.Max(v, b);

            m = Math.Min(r, g);
            m = Math.Min(m, b);

            l = (m + v) / 2.0;

            if (l <= 0.0)
            {
                return;
            }

            vm = v - m;
            s = vm;

            if (s > 0.0)
            {
                s /= (l <= 0.5) ? (v + m) : (2.0 - v - m);
            }

            else
            {
                return;
            }

            r2 = (v - r) / vm;
            g2 = (v - g) / vm;
            b2 = (v - b) / vm;

            if (r == v)
            {
                h = (g == m ? 5.0 + b2 : 1.0 - g2);
            }
            else if (g == v)
            {
                h = (b == m ? 1.0 + r2 : 3.0 - b2);
            }
            else
            {
                h = (r == m ? 3.0 + g2 : 5.0 - r2);
            }
            h /= 6.0;
        }

        // Given H,S,L in range of 0-1

        // Returns a Color (RGB struct) in range of 0-255

        public static ColorRGB HSL2RGB(double h, double sl, double l)
        {
            double v;
            double r, g, b;

            r = l;   // default to gray
            g = l;
            b = l;

            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);

            if (v > 0)
            {
                double m;
                double sv;
                int sextant;

                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;

                h *= 6.0;

                sextant = (int)h;

                fract = h - sextant;

                vsf = v * sv * fract;

                mid1 = m + vsf;
                mid2 = v - vsf;

                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;

                        break;

                    case 1:
                        r = mid2;
                        g = v;
                        b = m;

                        break;

                    case 2:
                        r = m;
                        g = v;
                        b = mid1;

                        break;

                    case 3:
                        r = m;
                        g = mid2;
                        b = v;

                        break;

                    case 4:
                        r = mid1;
                        g = m;
                        b = v;

                        break;

                    case 5:
                        r = v;
                        g = m;
                        b = mid2;

                        break;
                }
            }

            ColorRGB rgb;

            rgb.R = Convert.ToByte(r * 255.0f);
            rgb.G = Convert.ToByte(g * 255.0f);
            rgb.B = Convert.ToByte(b * 255.0f);

            return rgb;
        }
    }
}