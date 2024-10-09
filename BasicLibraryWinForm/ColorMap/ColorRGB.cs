namespace BasicLibraryWinForm.ColorMap
{
    public struct ColorRGB
    {
        public byte R;

        public byte G;

        public byte B;

        public ColorRGB(Color value)
        {
            this.R = value.R;
            this.G = value.G;
            this.B = value.B;
        }

        public static implicit operator Color(ColorRGB rgb)
        {
            return Color.FromArgb(rgb.R, rgb.G, rgb.B);
        }

        public static explicit operator ColorRGB(Color c)
        {
            return new ColorRGB(c);
        }
    }
}