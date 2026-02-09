using System;
using System.Drawing;
using System.Windows.Forms;

namespace CGP
{
    public partial class DoubleBuffering : Form
    {
        // square position + speed
        private int x = 10, y = 260;
        private int dx = 4, dy = 4;
        private const int size = 30;

        // back buffer
        private Bitmap backBuffer;
        private Timer timer;

        public DoubleBuffering()
        {
            InitializeComponent();

            this.Width = 600;
            this.Height = 500;
            this.BackColor = Color.White;
            this.Text = "DoubleBuffering";

            // create back buffer
            backBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);

            // timer loop (infinite)
            timer = new Timer();
            timer.Interval = 16; // ~60 FPS
            timer.Tick += (s, e) => StepFrame();
            timer.Start();
        }

        private void StepFrame()
        {
            // move
            x += dx;
            y += dy;

            // bounce off edges (approx ok)
            int maxX = this.ClientSize.Width - size;
            int maxY = this.ClientSize.Height - size;

            if (x < 0) { x = 0; dx = -dx; }
            if (x > maxX) { x = maxX; dx = -dx; }

            if (y < 0) { y = 0; dy = -dy; }
            if (y > maxY) { y = maxY; dy = -dy; }

            // redraw
            this.Invalidate(); // triggers OnPaint
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // if resized, rebuild buffer
            if (backBuffer == null ||
                backBuffer.Width != this.ClientSize.Width ||
                backBuffer.Height != this.ClientSize.Height)
            {
                backBuffer?.Dispose();
                backBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            }

            // 1) draw EVERYTHING onto BACK buffer
            using (Graphics g = Graphics.FromImage(backBuffer))
            {
                g.Clear(Color.White);

                using (Pen p = new Pen(Color.Black, 2))
                using (Brush b = new SolidBrush(Color.Black))
                using (Font f = new Font("Arial", 18, FontStyle.Bold))
                {
                    // moving square
                    g.DrawRectangle(p, x, y, size, size);

                    // center message
                    string msg = "Double Buffering";
                    SizeF s = g.MeasureString(msg, f);
                    float cx = (this.ClientSize.Width - s.Width) / 2f;
                    float cy = (this.ClientSize.Height - s.Height) / 2f;
                    g.DrawString(msg, f, b, cx, cy);
                }
            }

            // 2) copy BACK buffer to FRONT screen (single blit)
            e.Graphics.DrawImageUnscaled(backBuffer, 0, 0);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            timer?.Stop();
            timer?.Dispose();
            backBuffer?.Dispose();
            base.OnFormClosed(e);
        }
    }
}
