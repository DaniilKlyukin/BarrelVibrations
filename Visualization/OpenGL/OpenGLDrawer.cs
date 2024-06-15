using System.Drawing;
using System.Runtime.InteropServices;
using BasicLibraryWinForm;
using CustomControls;
using SharpGL;
using SharpGL.Enumerations;
using SharpGL.SceneGraph;
using GL = SharpGL.OpenGL;
using IntPoint = System.Drawing.Point;

namespace Visualization.OpenGL
{
    public class OpenGLDrawer
    {
        private Camera Camera = new() { position = new Vertex(1, 1, 1), target = new Vertex(0, 0, 0), up = new Vertex(0, 1, 0) };

        private bool _lmbDown;
        private int rmbX, rmbY;

        public Vertex mouseRightClickWorldCoordinates { get; private set; } = new(0, 0, 0);
        public Vertex mouseLeftClickWorldCoordinates { get; private set; } = new(0, 0, 0);

        public IntPoint mouseLastCoordinates { get; private set; } = new(0, 0);

        private bool _rmbDown;

        private const double BaseFont3DSize = 0.5;
        private const uint axis_index = 1, oxy_grid_index = 2, oxz_grid_index = 3, oyz_grid_index = 4, surfaces_index = 5, edges_index = 6;
        private GL gl;


        private OpenGLControl glControl;

        public OpenGLDrawer(OpenGLControl glControl, HeatBar heatBar, VisualizationProperties properties)
        {
            VisualizationProperties = properties;
            this.glControl = glControl;
            gl = glControl.OpenGL;

            HeatBar = heatBar;

            //  Фоновый цвет по умолчанию (в данном случае цвет голубой)
            gl.ClearColor(1f, 1f, 1.0f, 0);

            glControl.MouseMove += MouseMove;
            glControl.MouseDown += MouseDown;
            glControl.MouseUp += MouseUp;
            glControl.MouseWheel += Wheel;

            glControl.KeyDown += (s, e) =>
            {
                switch (e.KeyData)
                {
                    case Keys.NumPad4:
                        {
                            CreateAxis();
                            CreateGrid();
                        }
                        break;
                    case Keys.NumPad0:
                        {
                            CreateAxis();
                            CreateGrid();
                        }
                        break;
                    case Keys.Home:
                        {
                            if (!Surfaces.Any())
                            {
                                Camera.position = new Vertex(1, 1, 1);
                                Camera.target = new Vertex(0, 0, 0);
                            }

                            var cX = Surfaces.Average(s => s.CenterX);
                            var cY = Surfaces.Average(s => s.CenterY);
                            var cZ = Surfaces.Average(s => s.CenterZ);

                            var dist = Surfaces.Max(s => Algebra.GetDistance(s.Center.X, s.CenterZ, s.CenterZ, cX, cY, cZ));

                            var alpha = Math.PI / 4;
                            var theta = Math.PI / 4;

                            Camera.target = new Vertex((float)cX, (float)cY, (float)cZ);
                            Camera.position = new Vertex(
                                (float)(cX + dist * Math.Cos(alpha) * Math.Cos(theta)),
                                (float)(cY + dist * Math.Sin(theta)),
                                (float)(cZ + dist * Math.Sin(alpha) * Math.Cos(theta)));
                        }
                        break;
                }
            };

            CreateLists();
        }

        public VisualizationProperties VisualizationProperties { get; }
        public HeatBar HeatBar { get; }

        private Dictionary<string, IEnumerable<GLSurface>> objectsSurfaces = new();

        public void AddObjectSurfaces(string key, IEnumerable<GLSurface> surfaces)
        {
            if (objectsSurfaces.ContainsKey(key))
                objectsSurfaces[key] = surfaces.ToList();
            else
                objectsSurfaces.Add(key, surfaces.ToList());
        }

        public IEnumerable<GLSurface> GetObjectSurfaces(string key)
        {
            return objectsSurfaces[key];
        }

        public void RemoveObjectSurfaces(string key)
        {
            if (objectsSurfaces.ContainsKey(key))
                objectsSurfaces.Remove(key);
        }

        public void ClearObjectsSurfaces()
        {
            objectsSurfaces.Clear();
        }

        private IEnumerable<GLSurface> Surfaces => objectsSurfaces.Values.SelectMany(x => x);

        public void Render()
        {
            CreateAxis();

            if (Surfaces.Any())
            {
                CreateSurfaces(Surfaces);
                CreateSurfacesEdges(Surfaces);
            }

            CreateGrid();
        }

        public void SetOxyView()
        {
            Camera.target = Camera.position + new Vertex(0, 0, -1);
        }

        public void SetOxzView()
        {
            Camera.target = Camera.position + new Vertex(0, -1, 0);
        }

        public void SetOyzView()
        {
            Camera.target = Camera.position + new Vertex(-1, 0, 0);
        }

        public void UpdateCamera()
        {
            //  Зададим матрицу проекции
            gl.MatrixMode(GL.GL_PROJECTION);

            //  Единичная матрица для последующих преобразований
            gl.LoadIdentity();

            //  Преобразование
            gl.Perspective(60.0f, glControl.Width / (double)glControl.Height, 0.01, 1000.0);

            gl.LookAt(Camera.position.X, Camera.position.Y, Camera.position.Z,
                Camera.target.X, Camera.target.Y, Camera.target.Z,
                Camera.up.X, Camera.up.Y, Camera.up.Z);

            //  Зададим модель отображения
            gl.MatrixMode(GL.GL_MODELVIEW);
        }

        public void Draw()
        {
            gl.Clear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);
            //gl.Disable(GL.GL_CULL_FACE);
            gl.Enable(GL.GL_BLEND);
            gl.BlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            //gl.Enable(GL.GL_NORMALIZE);

            gl.LoadIdentity();

            UpdateCamera();

            if (VisualizationProperties.ShowAxis)
            {
                gl.CallList(axis_index);
                gl.DrawText(40, 0, 0, 1f, 0, "Arial", 20f, "X");
                gl.DrawText(20, 20, 1f, 0, 0, "Arial", 20f, "Y");
                gl.DrawText(0, 0, 0, 0, 1f, "Arial", 20f, "Z");
            }

            if (_rmbDown)
            {
                var distance = (mouseRightClickWorldCoordinates - Camera.position).Magnitude();

                DrawSphere(
                    mouseRightClickWorldCoordinates.X,
                    mouseRightClickWorldCoordinates.Y,
                    mouseRightClickWorldCoordinates.Z,
                    distance / 100, 32);
            }            

            if (VisualizationProperties.ShowSurfacesEdges)
                gl.CallList(edges_index);

            gl.CallList(surfaces_index);

            if (VisualizationProperties.ShowGrid)
                DrawGrid();

            gl.Flush();
            glControl.Invalidate();
        }



        private void DrawText3D(string text, double x, double y, double z, Color color, float angleX = 0, float angleY = 0, float angleZ = 0, string fontName = "Arial", double fontSize = BaseFont3DSize)
        {
            var textScale = 0.6 * fontSize / BaseFont3DSize;

            var chars = text.Contains(',') || text.Contains('.');

            var deltaChar = chars ? BaseFont3DSize / 4 : 0;

            gl.PushMatrix();
            gl.Translate(x, y, z);
            gl.Color(color.R, color.G, color.B, color.A);
            gl.Rotate(angleX, angleY, angleZ);
            gl.Scale(textScale, textScale, textScale);
            gl.Translate(-BaseFont3DSize * text.Length / 2 + deltaChar, -2 * BaseFont3DSize, 0);
            gl.DrawText3D(fontName, 0, (float)0.1, text);
            gl.PopMatrix();
        }

        private void MouseDown(object? sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != 0)
            {
                _lmbDown = true;

                mouseLeftClickWorldCoordinates = MouseCoordinateToWorld(e.X, e.Y);
            }

            if ((e.Button & MouseButtons.Right) != 0)
            {
                rmbX = e.X;
                rmbY = e.Y;
                _rmbDown = true;

                mouseRightClickWorldCoordinates = MouseCoordinateToWorld(e.X, e.Y);
            }

            if ((e.Button & MouseButtons.Middle) != 0)
            {
                if (!RayIntersectGeometry(MouseCoordinateToWorld(e.X, e.Y), Camera.target - Camera.position, out var surface, out var intersectionPointMin))
                    return;

                Camera.target = surface.Center;
                Camera.position = surface.Center + new Vertex((float)surface.Normal.X, (float)surface.Normal.Y, (float)surface.Normal.Z) * (float)surface.AverageLength * 10;
            }
        }

        private void MouseUp(object? sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != 0)
            {
                _lmbDown = false;
            }

            if ((e.Button & MouseButtons.Right) != 0)
            {
                _rmbDown = false;
            }
        }

        private void MouseMove(object? sender, MouseEventArgs e)
        {
            if (_lmbDown)
            {
                var delta = new Vertex(
                    e.X - mouseLastCoordinates.X,
                    e.Y - mouseLastCoordinates.Y,
                    0).GetNormalize();

                var right = Camera.up.VectorProduct(Camera.target - Camera.position).GetNormalize();

                var move = (right * delta.X + Camera.up * delta.Y).GetNormalize();

                var dist = (Camera.target - Camera.position).Magnitude();

                Camera.Move(move * (float)(VisualizationProperties.MoveSpeed * dist));
            }

            if (_rmbDown)
            {
                var signDx = Math.Sign(e.X - rmbX);
                var signDy = Math.Sign(e.Y - rmbY);

                rmbX = e.X;
                rmbY = e.Y;

                var right = (Camera.target - Camera.position).VectorProduct(Camera.up);

                right.Normalize();

                Camera.position = Math3D.RotateVectorAboutPoint(Camera.position, mouseRightClickWorldCoordinates, new Vertex(0, 1, 0), -signDx * VisualizationProperties.RotateSpeed);
                Camera.position = Math3D.RotateVectorAboutPoint(Camera.position, mouseRightClickWorldCoordinates, right, -signDy * VisualizationProperties.RotateSpeed);

                Camera.target = mouseRightClickWorldCoordinates;
            }

            mouseLastCoordinates = new IntPoint(e.X, e.Y);
        }

        private void Wheel(object? sender, MouseEventArgs e)
        {
            glControl.Focus();

            if (!RayIntersectGeometry(MouseCoordinateToWorld(e.X, e.Y), Camera.target - Camera.position,
                out var surface, out var intersectionPointMin))
            {
                var delta = VisualizationProperties.ScaleSpeed * MathF.Sign(e.Delta) * 0.025f;

                Camera.Move((Camera.target - Camera.position) * delta);
                return;
            }

            Camera.target = intersectionPointMin;
            Camera.position += (intersectionPointMin - Camera.position) * MathF.Sign(e.Delta) / 10;
            SetMouseToCenter();
        }

        private void SetMouseToCenter()
        {
            Cursor.Position = glControl.PointToScreen(new System.Drawing.Point(glControl.Width / 2, glControl.Height / 2));
        }

        private void CreateLists()
        {
            gl.GenLists(6);
            CreateAxis();
            CreateGrid();
        }

        #region Оси координат
        private void CreateAxis()
        {
            gl.NewList(axis_index, GL.GL_COMPILE);

            gl.LineWidth(2f);
            gl.Begin(BeginMode.Lines);

            var x0 = -20;
            var x1 = 20;

            gl.Color(0, 1f, 0, 1);
            gl.Vertex(x0, 0, 0);
            gl.Vertex(x1, 0, 0);

            var y0 = -20;
            var y1 = 20;

            gl.Color(1f, 0, 0);
            gl.Vertex(0, y0, 0);
            gl.Vertex(0, y1, 0);

            var z0 = -20;
            var z1 = 20;

            gl.Color(0, 0, 1f);
            gl.Vertex(0, 0, z0);
            gl.Vertex(0, 0, z1);

            gl.End();

            gl.EndList();
        }

        #endregion

        #region Сетка координат

        private void DrawGrid()
        {
            var v = (Camera.target - Camera.position).GetNormalize();
            var oxz = new Vertex(0, 1, 0);
            var oxy = new Vertex(0, 0, 1);
            var oyz = new Vertex(1, 0, 0);

            switch (VisualizationProperties.CoordinateGridSurfaceType)
            {
                case CoordinateGridSurfaceType.Oxy:
                    gl.CallList(oxy_grid_index);
                    break;
                case CoordinateGridSurfaceType.Oxz:
                    gl.CallList(oxz_grid_index);
                    break;
                case CoordinateGridSurfaceType.Oyz:
                    gl.CallList(oyz_grid_index);
                    break;
                case CoordinateGridSurfaceType.Auto:
                default:
                    {
                        if (MathF.Abs(v.ScalarProduct(oxz)) >= 0.5)
                        {
                            gl.CallList(oxz_grid_index);
                        }
                        else if (MathF.Abs(v.ScalarProduct(oxy)) >= 0.5)
                        {
                            gl.CallList(oxy_grid_index);
                        }
                        else if (MathF.Abs(v.ScalarProduct(oyz)) >= 0.5)
                        {
                            gl.CallList(oyz_grid_index);
                        }
                        else
                        {
                            gl.CallList(oxz_grid_index);
                        }

                        break;
                    }
            }
        }

        private void CreateGrid()
        {
            var x0 = VisualizationProperties.GridCenter.X - VisualizationProperties.GridSize / 2;
            var y0 = VisualizationProperties.GridCenter.Y - VisualizationProperties.GridSize / 2;
            var z0 = VisualizationProperties.GridCenter.Z - VisualizationProperties.GridSize / 2;

            var x1 = VisualizationProperties.GridCenter.X + VisualizationProperties.GridSize / 2;
            var y1 = VisualizationProperties.GridCenter.Y + VisualizationProperties.GridSize / 2;
            var z1 = VisualizationProperties.GridCenter.Z + VisualizationProperties.GridSize / 2;

            CreateOxyGrid(VisualizationProperties.GridCenter.X, VisualizationProperties.GridCenter.Y, VisualizationProperties.GridCenter.Z);
            CreateOxzGrid(VisualizationProperties.GridCenter.X, VisualizationProperties.GridCenter.Y, VisualizationProperties.GridCenter.Z);
            CreateOyzGrid(VisualizationProperties.GridCenter.X, VisualizationProperties.GridCenter.Y, VisualizationProperties.GridCenter.Z);
        }

        private void CreateOxyGrid(double x0, double y0, double z0)
        {
            gl.NewList(oxy_grid_index, GL.GL_COMPILE);

            gl.LineWidth(1f);
            gl.Begin(BeginMode.Lines);

            gl.Color(0 / 255.0, 0 / 255.0, 0 / 255.0, 0.7);

            var correctedSize = 2 * VisualizationProperties.CellSize * Math.Ceiling(0.5 * VisualizationProperties.GridSize / VisualizationProperties.CellSize);

            var x00 = x0 - correctedSize / 2;
            var x11 = x0 + correctedSize / 2;
            var y00 = y0 - correctedSize / 2;
            var y11 = y0 + correctedSize / 2;

            var xCount = (int)Math.Round((x11 - x00) / VisualizationProperties.CellSize);
            var yCount = (int)Math.Round((y11 - y00) / VisualizationProperties.CellSize);

            for (var i = 0; i <= xCount; i++)
            {
                var x = x00 + i * VisualizationProperties.CellSize;

                gl.Vertex(x, y00, z0);
                gl.Vertex(x, y11, z0);
            }

            for (var i = 0; i <= yCount; i++)
            {
                var y = y00 + i * VisualizationProperties.CellSize;

                gl.Vertex(x00, y, z0);
                gl.Vertex(x11, y, z0);
            }

            gl.End();

            gl.Begin(BeginMode.Polygon);
            gl.Color(Color.Transparent);
            gl.Vertex(x00, y00, z0);
            gl.Vertex(x00, y11, z0);
            gl.Vertex(x11, y11, z0);
            gl.Vertex(x11, y0, z0);
            gl.End();

            gl.EndList();
        }

        private void CreateOxzGrid(double x0, double y0, double z0)
        {
            gl.NewList(oxz_grid_index, GL.GL_COMPILE);

            gl.LineWidth(1f);
            gl.Begin(BeginMode.Lines);

            gl.Color(0 / 255.0, 0 / 255.0, 0 / 255.0, 0.7);

            var correctedSize = 2 * VisualizationProperties.CellSize * Math.Ceiling(0.5 * VisualizationProperties.GridSize / VisualizationProperties.CellSize);

            var x00 = x0 - correctedSize / 2;
            var x11 = x0 + correctedSize / 2;
            var z00 = z0 - correctedSize / 2;
            var z11 = z0 + correctedSize / 2;

            var xCount = (int)Math.Round((x11 - x00) / VisualizationProperties.CellSize);
            var zCount = (int)Math.Round((z11 - z00) / VisualizationProperties.CellSize);

            for (var i = 0; i <= xCount; i++)
            {
                var x = x00 + i * VisualizationProperties.CellSize;

                gl.Vertex(x, y0, z00);
                gl.Vertex(x, y0, z11);
            }

            for (var i = 0; i <= zCount; i++)
            {
                var z = z00 + i * VisualizationProperties.CellSize;

                gl.Vertex(x00, y0, z);
                gl.Vertex(x11, y0, z);
            }

            gl.End();

            gl.Begin(BeginMode.Polygon);
            gl.Color(Color.Transparent);
            gl.Vertex(x00, y0, z00);
            gl.Vertex(x00, y0, z11);
            gl.Vertex(x11, y0, z11);
            gl.Vertex(x11, y0, z00);
            gl.End();

            gl.EndList();
        }

        private void CreateOyzGrid(double x0, double y0, double z0)
        {
            gl.NewList(oyz_grid_index, GL.GL_COMPILE);

            gl.LineWidth(1f);
            gl.Begin(BeginMode.Lines);

            gl.Color(0 / 255.0, 0 / 255.0, 0 / 255.0, 0.7);

            var correctedSize = 2 * VisualizationProperties.CellSize * Math.Ceiling(0.5 * VisualizationProperties.GridSize / VisualizationProperties.CellSize);

            var y00 = y0 - correctedSize / 2;
            var y11 = y0 + correctedSize / 2;
            var z00 = z0 - correctedSize / 2;
            var z11 = z0 + correctedSize / 2;

            var yCount = (int)Math.Round((y11 - y00) / VisualizationProperties.CellSize);
            var zCount = (int)Math.Round((z11 - z00) / VisualizationProperties.CellSize);

            for (var i = 0; i <= yCount; i++)
            {
                var y = y00 + i * VisualizationProperties.CellSize;

                gl.Vertex(x0, y, z00);
                gl.Vertex(x0, y, z11);
            }

            for (var i = 0; i <= zCount; i++)
            {
                var z = z00 + i * VisualizationProperties.CellSize;

                gl.Vertex(x0, y00, z);
                gl.Vertex(x0, y11, z);
            }

            gl.End();

            gl.Begin(BeginMode.Polygon);
            gl.Color(Color.Transparent);
            gl.Vertex(x0, y00, z00);
            gl.Vertex(x0, y00, z11);
            gl.Vertex(x0, y11, z11);
            gl.Vertex(x0, y11, z00);
            gl.End();

            gl.EndList();
        }

        #endregion

        private void CreateSurfaces(
            IEnumerable<GLSurface> glSurfaces)
        {
            gl.NewList(surfaces_index, GL.GL_COMPILE);

            foreach (var surface in glSurfaces)
            {
                var avgColor = GetAverageColor(surface.Points.Select(point => point.Color));

                gl.Begin(BeginMode.Polygon);

                foreach (var coloredPoint in surface.Points)
                {
                    if (VisualizationProperties.SmoothDrawing)
                        gl.Color(coloredPoint.Color.R / 255.0, coloredPoint.Color.G / 255.0, coloredPoint.Color.B / 255.0, coloredPoint.Color.A / 255.0);
                    else
                        gl.Color(avgColor.R / 255.0, avgColor.G / 255.0, avgColor.B / 255.0, coloredPoint.Color.A / 255.0);

                    gl.Vertex(coloredPoint.X, coloredPoint.Y, coloredPoint.Z);
                }
                gl.End();
            }

            gl.EndList();
        }

        private void CreateSurfacesEdges(
            IEnumerable<GLSurface> glSurfaces)
        {
            gl.NewList(edges_index, GL.GL_COMPILE);

            foreach (var surface in glSurfaces)
            {
                if (!surface.WithEdges)
                    continue;

                var dn = 0 * surface.Normal;
                gl.Begin(BeginMode.LineLoop);

                gl.Color(Color.FromArgb(38, 36, 36));
                gl.LineWidth(VisualizationProperties.SurfacesEdges);

                foreach (var coloredPoint in surface.Points)
                {
                    gl.Vertex(coloredPoint.X + dn.X, coloredPoint.Y + dn.Y, coloredPoint.Z + dn.Z);
                }

                gl.End();
            }

            gl.EndList();
        }

        private Color GetAverageColor(IEnumerable<Color> colors)
        {
            var r = 0.0;
            var g = 0.0;
            var b = 0.0;

            var colorsArr = colors as Color[] ?? colors.ToArray();

            foreach (var color in colorsArr)
            {
                r += color.R;
                g += color.G;
                b += color.B;
            }

            var n = colorsArr.Length;

            return Color.FromArgb((int)(r / n), (int)(g / n), (int)(b / n));
        }

        private void DrawPolygonWithEdges(List<double[]> vertices, Color color, float alpha = 1, Color? edgeColor = null)
        {
            gl.Color(color.R / 255f, color.G / 255f, color.B / 255f, alpha);
            gl.Begin(BeginMode.Polygon);

            foreach (var vertex in vertices)
            {
                gl.Vertex(vertex);
            }
            gl.End();

            edgeColor ??= Color.Black;

            gl.Color(edgeColor.Value.R / 255f, edgeColor.Value.G / 255f, edgeColor.Value.B / 255f);
            gl.Begin(BeginMode.LineLoop);

            foreach (var vertex in vertices)
            {
                gl.Vertex(vertex);
            }
            gl.End();
        }

        private void DrawArrow(double w, double h, Color color)
        {
            //стрелка направлена вправо

            DrawPolygonWithEdges(new List<double[]>
            {
                new []{-w / 2, -h / 2, 0},
                new []{w / 2, -h / 2, 0},
                new []{w / 2, h / 2, 0},
                new []{-w / 2, h / 2, 0}
            }, color);

            DrawPolygonWithEdges(new List<double[]>
            {
                new []{w / 2, -h, 0},
                new []{w, 0, 0},
                new []{w / 2, h, 0}
            }, color);
        }

        private void DrawSphere(double x, double y, double z, double r, int slices)
        {
            var q = gl.NewQuadric();

            gl.PushMatrix();
            gl.Translate(x, y, z);
            gl.QuadricDrawStyle(q, GL.GLU_FILL);
            gl.Color(Color.Red);
            gl.Sphere(q, r, 32, 32);
            gl.PopMatrix();
        }

        private void DrawRhombus(double x0, double y0, double z0, double w, double h, Color color)
        {
            DrawPolygonWithEdges(new List<double[]>
            {
                new []{x0 - w / 2, y0, z0},
                new []{x0, y0 - h / 2, z0},
                new []{x0 + w / 2, y0, z0},
                new []{ x0, y0 + h / 2, z0 }
            }, color);
        }

        private Vertex MouseCoordinateToWorld(int mx, int my)
        {
            var Viewport = new int[4];//View matrix 
            var ModelView_Matrix = new double[16];//Model matrix 
            var Project_Matrix = new double[16];//Projection matrix

            gl.GetInteger(GL.GL_VIEWPORT, Viewport);
            gl.GetDouble(GL.GL_MODELVIEW_MATRIX, ModelView_Matrix);
            gl.GetDouble(GL.GL_PROJECTION_MATRIX, Project_Matrix);

            var viewport = new int[4];
            var modelview = new double[16];
            var projection = new double[16];
            float winX, winY;
            double posX = 0.0, posY = 0.0, posZ = 0.0;

            gl.GetDouble(GL.GL_MODELVIEW_MATRIX, modelview);
            gl.GetDouble(GL.GL_PROJECTION_MATRIX, projection);
            gl.GetInteger(GL.GL_VIEWPORT, viewport);

            winX = mx;
            winY = viewport[3] - my;

            var array = new float[1];//Store the depth parameter read by 
            var pinned = GCHandle.Alloc(array, GCHandleType.Pinned);
            var winz = pinned.AddrOfPinnedObject();
            gl.ReadPixels((int)winX, (int)winY, 1, 1, GL.GL_DEPTH_COMPONENT, GL.GL_FLOAT, winz);//Get the pixel depth of the mouse position
            pinned.Free();

            gl.UnProject(winX, winY, (double)array[0], ModelView_Matrix, Project_Matrix, Viewport, ref posX, ref posY, ref posZ);

            return new Vertex((float)posX, (float)posY, (float)posZ);
        }
        private bool RayIntersectGeometry(Vertex orig, Vertex dir, out GLSurface surface, out Vertex intersectionPointMin)
        {
            var dist = double.MaxValue;
            intersectionPointMin = new Vertex();
            var found = false;

            surface = Surfaces.FirstOrDefault();

            foreach (var geometrySurface in Surfaces)
            {
                var points = geometrySurface.Points.ToArray();

                if (IsRayIntersectTriangle(orig, dir,
                    new Vertex((float)points[0].Point.X, (float)points[0].Point.Y, (float)points[0].Point.Z),
                    new Vertex((float)points[1].Point.X, (float)points[1].Point.Y, (float)points[1].Point.Z),
                    new Vertex((float)points[2].Point.X, (float)points[2].Point.Y, (float)points[2].Point.Z),
                    out var t, out var intersectionPoint))
                {
                    if (t <= dist)
                    {
                        dist = t;
                        surface = geometrySurface;
                        found = true;
                        intersectionPointMin = intersectionPoint;
                    }
                }
            }

            return found;
        }
        private bool IsRayIntersectTriangle(Vertex orig, Vertex dir, Vertex v0, Vertex v1, Vertex v2, out float distance, out Vertex intersectionPoint)
        {
            // compute plane's normal
            var e1 = v1 - v0;
            var e2 = v2 - v0;
            // no need to normalize

            var pvec = dir.VectorProduct(e2);
            var det = e1.ScalarProduct(pvec);

            distance = float.MaxValue;
            intersectionPoint = new Vertex();

            if (det < 1e-8 && det > -1e-8)
                return false;

            var inv_det = 1 / det;
            var tvec = orig - v0;
            var u = tvec.ScalarProduct(pvec) * inv_det;
            if (u is < 0 or > 1)
                return false;

            var qvec = tvec.VectorProduct(e1);
            var v = dir.ScalarProduct(qvec) * inv_det;
            if (v < 0 || u + v > 1)
                return false;

            distance = e2.ScalarProduct(qvec) * inv_det;
            intersectionPoint = orig + dir * distance;

            return true;
        }

        public void SetColorsToSurfaces(
            List<GLSurface> glSurfaces,
            Dictionary<int, double> colorSource,
            string measureUnits,
            double? minGlobal = null,
            double? maxGlobal = null,
            double valueMultiplier = 1)
        {
            var min = double.MaxValue;
            var max = double.MinValue;

            foreach (var (_, value) in colorSource)
            {
                if (value < min)
                    min = value;

                if (value > max)
                    max = value;
            }

            min = minGlobal ?? min;
            max = maxGlobal ?? max;

            foreach (var surface in glSurfaces)
            {
                foreach (var coloredPoint in surface.Points)
                {
                    if (colorSource.ContainsKey(coloredPoint.Point.Id))
                    {
                        coloredPoint.Color = Algebra.GetHeatColor(
                            colorSource[coloredPoint.Point.Id] * valueMultiplier,
                            min * valueMultiplier,
                            max * valueMultiplier);
                    }
                }
            }

            HeatBar.SetLimits(min * valueMultiplier, max * valueMultiplier);

            HeatBar.MeasureUnits = measureUnits;
        }

        public void SetColorsToSurfaces(
            Dictionary<int, double> colorSource,
            string measureUnits,
            double? minGlobal = null,
            double? maxGlobal = null,
            double valueMultiplier = 1)
        {
            var min = double.MaxValue;
            var max = double.MinValue;

            foreach (var (_, value) in colorSource)
            {
                if (value < min)
                    min = value;

                if (value > max)
                    max = value;
            }

            min = minGlobal ?? min;
            max = maxGlobal ?? max;

            foreach (var surface in Surfaces)
            {
                foreach (var coloredPoint in surface.Points)
                {
                    if (colorSource.ContainsKey(coloredPoint.Point.Id))
                    {
                        coloredPoint.Color = Algebra.GetHeatColor(
                            colorSource[coloredPoint.Point.Id] * valueMultiplier,
                            min * valueMultiplier,
                            max * valueMultiplier);
                    }
                }
            }

            HeatBar.SetLimits(min * valueMultiplier, max * valueMultiplier);

            HeatBar.MeasureUnits = measureUnits;
        }

        public void ChangeSurfacesColor(Color color)
        {
            foreach (var surface in Surfaces)
            {
                foreach (var coloredPoint in surface.Points)
                {
                    coloredPoint.Color = color;
                }
            }
        }

        public void ChangeSurfacesColor(Color color, Func<GLSurface, bool> predicate)
        {
            foreach (var surface in Surfaces.Where(predicate))
            {
                foreach (var coloredPoint in surface.Points)
                {
                    coloredPoint.Color = color;
                }
            }
        }
    }
}

