using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CustomControls
{
    public enum Orientation
    {
        Circle,
        Arc,
        Rotate
    }

    public enum Direction
    {
        Clockwise,
        AntiClockwise
    }

    public partial class OrientedTextLabel : UserControl
    {
        #region Variables

        private double _rotationAngle;
        private string _text;
        private Orientation _textOrientation;
        private Direction _textDirection;

        #endregion

        #region Constructor

        public OrientedTextLabel()
        {
            InitializeComponent();
            //Setting the initial condition.
            _text = "Text";
            _rotationAngle = 0d;
            _textOrientation = Orientation.Rotate;
            Size = new Size(105, 12);
        }

        #endregion

        #region Properties
        
        [Description("Rotation Angle"), Category("Appearance")]
        public double RotationAngle
        {
            get
            {
                return _rotationAngle;
            }
            set
            {
                _rotationAngle = value;
                Invalidate();
            }
        }

        [Description("Kind of Text Orientation"), Category("Appearance")]
        public Orientation TextOrientation
        {
            get
            {
                return _textOrientation;
            }
            set
            {
                _textOrientation = value;
                Invalidate();
            }
        }

        [Description("Direction of the Text"), Category("Appearance")]
        public Direction TextDirection
        {
            get
            {
                return _textDirection;
            }
            set
            {
                _textDirection = value;
                Invalidate();
            }
        }

        [Description("Text"), Category("Appearance")]
        public override string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                Invalidate();
            }
        }

        #endregion

        #region Method

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;

            StringFormat stringFormat = new() { Alignment = StringAlignment.Center, Trimming = StringTrimming.None };

            Brush textBrush = new SolidBrush(ForeColor);

            //Getting the width and height of the text, which we are going to write
            var width = graphics.MeasureString(_text, Font).Width;
            var height = graphics.MeasureString(_text, Font).Height;

            //The radius is set to 0.9 of the width or height, b'cos not to
            //hide and part of the text at any stage
            var radius = 0f;
            if (ClientRectangle.Width < ClientRectangle.Height)
            {
                radius = ClientRectangle.Width * 0.9f / 2;
            }
            else
            {
                radius = ClientRectangle.Height * 0.9f / 2;
            }

            //Setting the text according to the selection
            switch (_textOrientation)
            {
                case Orientation.Arc:
                    {
                        //Arc angle must be get from the length of the text.
                        var arcAngle = 2 * width / radius / _text.Length;
                        if (_textDirection == Direction.Clockwise)
                        {
                            for (var i = 0; i < _text.Length; i++)
                            {
                                graphics.TranslateTransform(
                                    (float)(radius * (1 - Math.Cos(arcAngle * i + _rotationAngle / 180 * Math.PI))),
                                    (float)(radius * (1 - Math.Sin(arcAngle * i + _rotationAngle / 180 * Math.PI))));
                                graphics.RotateTransform(-90 + (float)_rotationAngle + 180 * arcAngle * i / (float)Math.PI);
                                graphics.DrawString(_text[i].ToString(), Font, textBrush, 0, 0);
                                graphics.ResetTransform();
                            }
                        }
                        else
                        {
                            for (var i = 0; i < _text.Length; i++)
                            {
                                graphics.TranslateTransform(
                                    (float)(radius * (1 - Math.Cos(arcAngle * i + _rotationAngle / 180 * Math.PI))),
                                    (float)(radius * (1 + Math.Sin(arcAngle * i + _rotationAngle / 180 * Math.PI))));
                                graphics.RotateTransform(-90 - (float)_rotationAngle - 180 * arcAngle * i / (float)Math.PI);
                                graphics.DrawString(_text[i].ToString(), Font, textBrush, 0, 0);
                                graphics.ResetTransform();
                            }
                        }
                        break;
                    }
                case Orientation.Circle:
                    {
                        if (_textDirection == Direction.Clockwise)
                        {
                            for (var i = 0; i < _text.Length; i++)
                            {
                                graphics.TranslateTransform(
                                    (float)(radius * (1 - Math.Cos(2 * Math.PI / _text.Length * i + _rotationAngle / 180 * Math.PI))),
                                    (float)(radius * (1 - Math.Sin(2 * Math.PI / _text.Length * i + _rotationAngle / 180 * Math.PI))));
                                graphics.RotateTransform(-90 + (float)_rotationAngle + 360 / _text.Length * i);
                                graphics.DrawString(_text[i].ToString(), Font, textBrush, 0, 0);
                                graphics.ResetTransform();
                            }
                        }
                        else
                        {
                            for (var i = 0; i < _text.Length; i++)
                            {
                                graphics.TranslateTransform(
                                    (float)(radius * (1 - Math.Cos(2 * Math.PI / _text.Length * i + _rotationAngle / 180 * Math.PI))),
                                    (float)(radius * (1 + Math.Sin(2 * Math.PI / _text.Length * i + _rotationAngle / 180 * Math.PI))));
                                graphics.RotateTransform(-90 - (float)_rotationAngle - 360 / _text.Length * i);
                                graphics.DrawString(_text[i].ToString(), Font, textBrush, 0, 0);
                                graphics.ResetTransform();
                            }

                        }
                        break;
                    }

                case Orientation.Rotate:
                    {
                        //For rotation, who about rotation?
                        var angle = _rotationAngle / 180 * Math.PI;
                        graphics.TranslateTransform(
                            (ClientRectangle.Width + (float)(height * Math.Sin(angle)) - (float)(width * Math.Cos(angle))) / 2,
                            (ClientRectangle.Height - (float)(height * Math.Cos(angle)) - (float)(width * Math.Sin(angle))) / 2);
                        graphics.RotateTransform((float)_rotationAngle);
                        graphics.DrawString(_text, Font, textBrush, 0, 0);
                        graphics.ResetTransform();

                        break;
                    }
            }
        }
        #endregion
    }
}
