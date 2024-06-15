namespace BasicLibraryWinForm
{
    public static class Physics
    {
        public static double GetInteractionCoefficient(int groovesCount, double smoothFrictionCoefficient, double groovesSlope)
        {
            if (groovesCount == 0 || smoothFrictionCoefficient == 0 || groovesSlope == 0)
                return smoothFrictionCoefficient;
            else
            {
                var a = Math.Atan(Math.PI / groovesSlope);
                return 0.6 * (Math.Sin(a) + smoothFrictionCoefficient * Math.Cos(a));
            }
        }      

        public static double GetGravity(double height, double latitude = 0.785398)
        {
            return
                9.780318 *
                (1 + 0.005302 * Math.Sin(latitude) * Math.Sin(latitude) -
                 0.000006 * Math.Sin(2 * latitude) * Math.Sin(2 * latitude))
                - 0.000003086 * height;
        }
    }
}
