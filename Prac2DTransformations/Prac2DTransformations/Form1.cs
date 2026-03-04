using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prac2DTransformations
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.Width = 500;
            this.Height = 500;
            this.BackColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            using (Pen beforePen = new Pen(Color.Gray, 2))
            using (Pen afterPen = new Pen(Color.Black, 2))
            using (Font myFont = new Font("Helvetica", 9))
            {
                Brush blackwriter = Brushes.Black;

                // BEFORE: square
                RectangleF square = new RectangleF(180, 140, 120, 120);
                PointF[] pts = RectToPoints(square);

                g.DrawPolygon(beforePen, pts);
                g.DrawString("Before", myFont, blackwriter, square.Left, square.Bottom + 6);

                // AFTER: rotated square (NO GDI+ Matrix)
                float angle = 30f;
                PointF pivot = new PointF(square.X + square.Width / 2f, square.Y + square.Height / 2f);

                PointF[] rotatedPts = Tmatrix.matrixRotate(angle, pts, pivot);

                g.DrawPolygon(afterPen, rotatedPts);
                g.DrawString("After (" + angle + "°)", myFont, blackwriter, square.Left, square.Bottom + 22);

                // pivot point (optional)
                g.FillEllipse(Brushes.Red, pivot.X - 3, pivot.Y - 3, 6, 6);
            }
        }

        private static PointF[] RectToPoints(RectangleF r)
        {
            return new PointF[]
            {
                new PointF(r.Left,  r.Top),
                new PointF(r.Right, r.Top),
                new PointF(r.Right, r.Bottom),
                new PointF(r.Left,  r.Bottom)
            };
        }
    }

    // ---- Our own rotation "matrix" class (NO System.Drawing.Drawing2D.Matrix) ----
    public static class Tmatrix
    {
        public static PointF matrixRotate(float angleDeg, PointF p, PointF pivot)
        {
            float rad = angleDeg * (float)Math.PI / 180f;
            float cos = (float)Math.Cos(rad);
            float sin = (float)Math.Sin(rad);

            // translate so pivot is origin
            float tx = p.X - pivot.X;
            float ty = p.Y - pivot.Y;

            // rotate
            float rx = (tx * cos) - (ty * sin);
            float ry = (tx * sin) + (ty * cos);

            // translate back
            return new PointF(rx + pivot.X, ry + pivot.Y);
        }

        public static PointF[] matrixRotate(float angleDeg, PointF[] points, PointF pivot)
        {
            PointF[] rotated = new PointF[points.Length];
            for (int i = 0; i < points.Length; i++)
                rotated[i] = matrixRotate(angleDeg, points[i], pivot);

            return rotated;
        }
    }
}