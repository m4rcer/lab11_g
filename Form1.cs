using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace lab11_g
{
    public partial class Form1 : Form
    {
        private Random rand;
        public Form1()
        {
            InitializeComponent();
            rand = new Random();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawStar(new PointF(ClientSize.Width / 2, ClientSize.Height / 2), 100, 5, 0, e.Graphics);
        }

        private void DrawStar(PointF center, int radius, int rays, double rotationAngle, Graphics g)
        {
            if (radius < 5) return; // минимальный размер звезды

            var angleStep = 360.0 / rays;
            var halfAngleStep = angleStep / 2;

            var points = new PointF[rays * 2];

            for (int i = 0; i < rays ; i += 1)
            {
                var outerRadius = radius;
                var innerRadius = radius / 2;

                var angle = i * angleStep + rotationAngle; // добавляем угол поворота
                var innerAngle = angle + halfAngleStep;

                points[i*2] = PointOnCircle(center, outerRadius, angle);
                points[i*2 + 1] = PointOnCircle(center, innerRadius, innerAngle);
            }

            var brush = new SolidBrush(Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256)));

            g.FillPolygon(brush, points);

            for (int i = 0; i < rays; i++)
            {
                var newCenter = PointOnCircle(center, radius + radius / 2, i * angleStep + rotationAngle);

                // проверяем расстояние между новым центром и предыдущим центром
                if (Distance(center, newCenter) < (radius + radius / 2))
                {
                    // если звезды перекрываются, то генерируем новый центр
                    newCenter = PointOnCircle(center, radius + radius / 2, i * angleStep + rotationAngle + angleStep / 2);
                }

                DrawStar(newCenter, radius / 3, rays, i * angleStep + rotationAngle, g); // рисуем звезду меньшего размера с углом поворота
            }
        }

        private PointF PointOnCircle(PointF center, double radius, double angleInDegrees)
        {
            var angleInRadians = angleInDegrees * Math.PI / 180;
            var x = (float)(center.X + radius*1.5 * Math.Cos(angleInRadians));
            var y = (float)(center.Y + radius*1.5 * Math.Sin(angleInRadians));
            return new PointF(x, y);
        }

        private double Distance(PointF a, PointF b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
