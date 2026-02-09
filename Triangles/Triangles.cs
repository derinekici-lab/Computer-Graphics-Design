using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CGP
{
    public partial class Triangles : Form
    {
        public Triangles()
        {
            InitializeComponent();
            this.BackColor = Color.White;
            this.Width = 650;
            this.Height = 600;
            this.Text = "Triangles";
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (Pen pen = new Pen(Color.Black, 1))
            {
                PointF p1 = new PointF(100, 100);
                PointF p2 = new PointF(500, 100);
                PointF p3 = new PointF(300, 446);

                DrawMidpointTriangle(g, pen, p1, p2, p3);
            }
        }

        private void DrawMidpointTriangle(Graphics g, Pen pen, PointF a, PointF b, PointF c)
        {
            g.DrawPolygon(pen, new[] { a, b, c });

            float ab = Dist(a, b);
            float bc = Dist(b, c);
            float ca = Dist(c, a);
            float maxSide = Math.Max(ab, Math.Max(bc, ca));
            if (maxSide < 1f) return;

            PointF abMid = Mid(a, b);
            PointF bcMid = Mid(b, c);
            PointF caMid = Mid(c, a);

            DrawMidpointTriangle(g, pen, abMid, bcMid, caMid);
        }

        private static PointF Mid(PointF p1, PointF p2)
            => new PointF((p1.X + p2.X) / 2f, (p1.Y + p2.Y) / 2f);

        private static float Dist(PointF p1, PointF p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
