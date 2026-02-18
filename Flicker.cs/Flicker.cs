using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CGP
{
    public partial class Flicker : Form
    {
        // Square state
        private float x = 40f, y = 40f;
        private const float SizePx = 50f;

        // Velocity (px/sec)
        private float vx = 220f, vy = 180f;

        private readonly Stopwatch clock = new Stopwatch();
        private double lastTime;
        private bool running = true;

        public Flicker()
        {
            InitializeComponent();

            Text = "Ultra Smooth Bouncing Square";
            ClientSize = new Size(500, 500);
            BackColor = Color.White;

            // Best available WinForms buffering settings
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            UpdateStyles();
            DoubleBuffered = true;

            // Start high-frequency loop
            clock.Start();
            lastTime = clock.Elapsed.TotalSeconds;

            Application.Idle += OnAppIdle;
            FormClosed += (_, __) => running = false;
        }

        // Windows message-queue check (for continuous render loop)
        [StructLayout(LayoutKind.Sequential)]
        private struct NativeMessage
        {
            public IntPtr handle;
            public uint msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public Point p;
        }

        [DllImport("user32.dll")]
        private static extern bool PeekMessage(out NativeMessage lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

        private static bool AppStillIdle()
        {
            return !PeekMessage(out _, IntPtr.Zero, 0, 0, 0);
        }

        private void OnAppIdle(object sender, EventArgs e)
        {
            while (running && AppStillIdle())
            {
                StepPhysics();
                Invalidate();   // request repaint
                Update();       // paint immediately (smoother than waiting)
            }
        }

        private void StepPhysics()
        {
            double now = clock.Elapsed.TotalSeconds;
            float dt = (float)(now - lastTime);
            lastTime = now;

            // Safety clamp to avoid jumps
            if (dt <= 0f) return;
            if (dt > 0.02f) dt = 0.02f;

            x += vx * dt;
            y += vy * dt;

            float maxX = ClientSize.Width - SizePx;
            float maxY = ClientSize.Height - SizePx;

            // Bounce with edge correction
            if (x < 0f) { x = 0f; vx = -vx; }
            else if (x > maxX) { x = maxX; vx = -vx; }

            if (y < 0f) { y = 0f; vy = -vy; }
            else if (y > maxY) { y = maxY; vy = -vy; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // High-quality rendering hints
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

            using (var brush = new SolidBrush(Color.Red))
            {
                e.Graphics.FillRectangle(brush, x, y, SizePx, SizePx);
            }
        }

        // Optional: reduce background erase flicker even more
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Fill once with solid color (instead of default erase path)
            e.Graphics.Clear(Color.White);
        }
    }
}
