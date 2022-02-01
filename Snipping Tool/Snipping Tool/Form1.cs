using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snipping_Tool
{
    public partial class Form1 : Form
    {
        int selectX;
        int selectY;
        int selectWidth;
        int selectHeight;
        public Pen selectPen;

        bool start = false;
        private void SaveToClipBoard()
        {
            if(selectWidth > 0)
            {
                Rectangle rect = new Rectangle(selectX, selectY, selectWidth, selectHeight);
                Bitmap OriginalImage = new Bitmap(pictureBox1.Image, pictureBox1.Width, pictureBox1.Height);
                Bitmap _img = new Bitmap(selectWidth, selectHeight);
                Graphics g = Graphics.FromImage(_img);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawImage(OriginalImage,0,0,rect,GraphicsUnit.Pixel);
                Clipboard.SetImage(_img);
            }
            Application.Exit();
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            Bitmap printScreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graphics = Graphics.FromImage(printScreen as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, printScreen.Size);
            using (MemoryStream s = new MemoryStream())
            {
                printScreen.Save(s, ImageFormat.Bmp);
                pictureBox1.Size = new Size(this.Width, this.Height);
                pictureBox1.Image = Image.FromStream(s);
            }
            this.Show();
            Cursor = Cursors.Cross;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(pictureBox1.Image == null)
            {
                return;
            }
            if (start)
            {
                pictureBox1.Refresh();
                selectWidth = e.X - selectX;
                selectHeight = e.Y - selectY;
                pictureBox1.CreateGraphics().DrawRectangle(selectPen,selectX,selectY,selectWidth, selectHeight);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!start)
            {
                
                if(e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    selectX = e.X;
                    selectY = e.Y;
                    selectPen = new Pen(Color.Red, 1);
                    selectPen.DashStyle = DashStyle.DashDotDot;

                }
                pictureBox1.Refresh();
                start = true;
            }
            else
            {
                if(pictureBox1.Image == null)
                    return;
                if(e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    pictureBox1.Refresh();
                    selectWidth = e.X - selectX;
                    selectHeight = e.Y - selectY;
                    pictureBox1.CreateGraphics().DrawRectangle(selectPen, selectX, selectY, selectWidth, selectHeight);
                }
                start = false;
                SaveToClipBoard();
            }
        }
    }
}
