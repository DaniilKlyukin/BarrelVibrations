using SharpGL.SceneGraph;

namespace Visualization.OpenGL
{
    public class Camera
    {
        public Vertex position;
        public Vertex target;
        public Vertex up;

        public void Move(Vertex direction)
        {
            position += direction;
            target += direction;
        }

        public void Rotate(Vertex axis, float angle)
        {
            /*
            target = Math3D.RotateAround(axis, angle, position, target);
            up = Math3D.RotateAround(axis, angle, position, up);*/
        }
    }
}
