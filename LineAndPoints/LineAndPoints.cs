using System.Drawing;
using System.Windows.Forms;

namespace CGP
{
    public partial class LineAndPoints : Form
    {
        // Points
        private readonly Point[] pts =
        {
            // Main outer rectangle corners
            new Point(80, 60),    // 0: outer top-left
            new Point(520, 60),   // 1: outer top-right
            new Point(520, 320),  // 2: outer bottom-right
            new Point(80, 320),   // 3: outer bottom-left

            // Middle divider (full width)
            new Point(80, 160),   // 4: middle left
            new Point(520, 160),  // 5: middle right

            // Vertical split (top section only)
            new Point(320, 60),   // 6: split top
            new Point(320, 160),  // 7: split bottom

            // Short horizontal line inside top-right area (G->H)
            new Point(320, 110),  // 8: G (left end)
            new Point(520, 110)   // 9: H (right end)
        };

        // Lines: NO far-right extension (so the 3 yellow lines are gone)
        // Also: right edge is split into two segments so there is NO vertical line between H (y=110) and E (y=160)
        private readonly int[,] lines =
        {
            // Outer border (top, left, bottom)
            {0, 1},   // top
            {0, 3},   // left
            {3, 2},   // bottom

            // Right border BUT WITH A GAP (no segment between y=110 and y=160)
            {1, 9},   // from top-right down to H (y=110)  => keeps A->B
            {5, 2},   // from E (y=160) down to bottom-right => keeps C->D

            // Middle divider (full width)
            {4, 5},

            // Vertical split (top only)
            {6, 7},

            // Short internal line (G->H)
            {8, 9}
        };

        public LineAndPoints()
        {
            InitializeComponent();
            Text = "LineAndPoints";
            Width = 900;
            Height = 500;
            BackColor = Color.White;
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Pen p = new Pen(Color.Black, 2))
            {
                for (int i = 0; i < lines.GetLength(0); i++)
                {
                    int a = lines[i, 0];
                    int b = lines[i, 1];
                    e.Graphics.DrawLine(p, pts[a], pts[b]);
                }
            }
        }
    }
}
