using System.ComponentModel;

namespace Visualization
{
    public enum SurfaceHidingType
    {
        [Description("По нормали")]
        Normal,
        [Description("По углу камеры")]
        CameraAngle,
        [Description("Не скрывать поверхности")]
        None,
    }
}
