using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace CGP
{
    public partial class XORDrawing : Form
    {
        Rectangle aRect;
        Rectangle anEllipse;
        Rectangle moving;

        int x = 0, y = 0;

        public XORDrawing()
        {
            InitializeComponent();

            // fixed shapes
            aRect = new Rectangle(100, 100, 200, 200);
            anEllipse = new Rectangle(150, 150, 200, 100);

            // moving square (starts at origin)
            moving = new Rectangle(x, y, 10, 10);

            // form settings
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.Width = 500;
            this.Height = 500;
            this.BackColor = Color.White;
            this.Text = "XOR Drawing";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            // draw background shapes
            using (Brush redBrush = new SolidBrush(Color.Red))
                g.FillRectangle(redBrush, aRect);

            using (Brush greenBrush = new SolidBrush(Color.Green))
                g.FillEllipse(greenBrush, anEllipse);

            // ===== XOR animation (from origin while x < 500) =====
            x = 0;
            y = 0;

            while (x < 500)
            {
                // convert to screen coordinates
                moving.Location = this.PointToScreen(new Point(x, y));

                // draw (XOR-like)
                ControlPaint.FillReversibleRectangle(moving, Color.Red);

                // pause so movement is visible
                Thread.Sleep(10);

                // draw again to "erase"
                ControlPaint.FillReversibleRectangle(moving, Color.Red);

                // move diagonally
                x += 2;
                y += 2;
            }
        }
    }
}
