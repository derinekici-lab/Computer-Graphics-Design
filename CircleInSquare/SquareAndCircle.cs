using System.Drawing;
using System.Windows.Forms;

namespace CGP
{
    public partial class SquareAndCircle : Form
    {
        public SquareAndCircle()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.Width = 500;
            this.Height = 500;
            this.BackColor = Color.White;
            this.Text = "SquareAndCircle";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            using (Pen p = new Pen(Color.Black, 2))
            {
                int x = 150, y = 150, size = 200;
                g.DrawRectangle(p, x, y, size, size);
                g.DrawEllipse(p, x, y, size, size);
            }
        }
    }
}
