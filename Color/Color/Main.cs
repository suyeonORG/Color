using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Color
{
    /* Author: Suyeon */
    public partial class Main : Form
    {
        static Graphics g;
        static Bitmap source;

        /* Filters */
        float[][] sepia = { new float[] { 0.393f, 0.349f, 0.272f, 0, 0 }, new float[] { 0.769f, 0.686f, 0.534f, 0, 0 }, new float[] { 0.189f, 0.168f, 0.131f, 0, 0 }, new float[] { 0, 0, 0, 0.5f, 0 }, new float[] { 0, 0, 0, 0, 1 } };
        float[][] blue = { new float[] { 0.393f, 0.349f, 0.272f, 0, 0 }, new float[] { 0.769f, 0.686f, 0.534f, 0, 0 }, new float[] { 0.4f, 0.4f, 0.4f, 0, 0 }, new float[] { 0, 0, 0, 0.5f, 0 }, new float[] { 0, 0, 120, 0, 1 } };
        float[][] red = { new float[] { 0.393f, 0.349f, 0.272f, 0, 0 }, new float[] { 0.769f, 0.686f, 0.534f, 0, 0 }, new float[] { 0.7f, 0.7f, 0.7f, 0, 0 }, new float[] { 0, 0, 0, 0.5f, 0 }, new float[] { 120, 0, 0, 0, 1 } };
        float[][] ultrared = { new float[] { 0.25f, 0.25f, 0.25f, 0, 0 }, new float[] { 0.2f, 0.2f, 0.2f, 0, 0 }, new float[] { 0.7f, 0.7f, 1.2f, 0, 0 }, new float[] { 0, 0, 0, 0.7f, 0 }, new float[] { 250, 0, 0, 0, 1 } };
        float[][] pink = { new float[] { 0.393f, 0.349f, 0.272f, 0, 0 }, new float[] { 0.769f, 0.686f, 0.534f, 0, 0 }, new float[] { 0.7f, 0.7f, 0.7f, 0, 0 }, new float[] { 0, 0, 0, 0.5f, 0 }, new float[] { 250, 0, 250, 0, 1 } };
        float[][] vintage  = { new float[] { 0.4f, 0.4f, 0.4f, 0, 0 }, new float[] { 0.4f, 0.4f, 0.4f, 0, 0 }, new float[] { 0.4f, 0.4f, 0.4f, 0, 0 }, new float[] { 0, 0, 0, 0.7f, 0 }, new float[] { 0, 0, 0, 0, 1 } };

        /* Form Drag */
        int x;
        int y;
        Boolean drag;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            source = (Bitmap)Bitmap.FromFile("Template.min.jpg");
            ResetFrame.Image = source;
            MainFrame.Image = source;

            GrayScale(BNFrame);
            ColorFilter(blue, BlueFrame);
            ColorFilter(sepia, SepiaFrame);
            ColorFilter(red, RedFrame);
            ColorFilter(ultrared, UltraRedFrame);
            ColorFilter(pink, PinkFrame);
            Contrast(ContrastFrame);
            Vintage(vintage, VintageFrame);     
        }

        void GrayScale(PictureBox control)
        {
            this.Text = "Color - Processing...";
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            if (control.Image == null)
            {
                control.Image = new Bitmap(source);
            }

            Bitmap local_source = new Bitmap(control.Image);

            int x = 0;
            int y = 0;
            int color = 0;

            for (x = 0; x <= local_source.Width - 1; x++)
            {
                for (y = 0; y <= local_source.Height - 1; y++)
                {
                    color = (local_source.GetPixel(x, y).R + local_source.GetPixel(x, y).G + local_source.GetPixel(x, y).B) / 3;
                    local_source.SetPixel(x, y, System.Drawing.Color.FromArgb(color, color, color));
                }
            }
            control.Image = local_source;

            this.Text = "Color - Ready !";
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        private void ColorFilter(float[][] values, PictureBox control)
        {
            this.Text = "Color - Processing...";
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            if (control.Image == null)
            {
                control.Image = new Bitmap(source);
            }

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            Bitmap local_source = new Bitmap(control.Image);
            System.Drawing.Imaging.ColorMatrix filterMatrix = new System.Drawing.Imaging.ColorMatrix(values);
            System.Drawing.Imaging.ImageAttributes IA = new System.Drawing.Imaging.ImageAttributes();
            IA.SetColorMatrix(filterMatrix);
            Bitmap filter = new Bitmap(local_source);
            using (Graphics G = Graphics.FromImage(filter))
            {
                G.DrawImage(local_source, new Rectangle(0, 0, filter.Width, filter.Height), 0, 0, filter.Width, filter.Height, GraphicsUnit.Pixel, IA);
            }

            control.Image = filter;

            this.Text = "Color - Ready !";
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        public void Contrast(PictureBox control)
        {
            float _contrastFactor = 0.04F * 28;
            control.Image = new Bitmap(source);
            Graphics g = Graphics.FromImage(control.Image);
            ImageAttributes IA = new ImageAttributes();
            ColorMatrix filterMatrix = new ColorMatrix(new float[][] { new float[] { _contrastFactor, 0f, 0f, 0f, 0f }, new float[] { 0f, _contrastFactor, 0f, 0f, 0f }, new float[] { 0f, 0f, _contrastFactor, 0f, 0f }, new float[] { 0f, 0f, 0f, 1f, 0f }, new float[] { 0.001f, 0.001f, 0.001f, 0f, 1f } });
            IA.SetColorMatrix(filterMatrix);
            g.DrawImage(control.Image, new Rectangle(0, 0, control.Image.Width, control.Image.Height), 0, 0, control.Image.Width, control.Image.Height, GraphicsUnit.Pixel, IA);
            g.Dispose();
            IA.Dispose();
        }

        public void Vintage(float[][] values, PictureBox control)
        {
            this.Text = "Color - Processing...";
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            if (control.Image == null)
            {
                control.Image = new Bitmap(source);
            }

            System.Drawing.Imaging.ColorMatrix filterMatrix = new System.Drawing.Imaging.ColorMatrix(values);
            System.Drawing.Imaging.ImageAttributes IA = new System.Drawing.Imaging.ImageAttributes();
            IA.SetColorMatrix(filterMatrix);
            Bitmap filter = new Bitmap(control.Image);
            using (Graphics G = Graphics.FromImage(filter))
            {
                G.DrawImage(control.Image, new Rectangle(0, 0, filter.Width, filter.Height), 0, 0, filter.Width, filter.Height, GraphicsUnit.Pixel, IA);
            }
            control.Image = filter;
            this.Text = "Color - Ready !";
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        void DragOn()
        {
            
            drag = true;
            x = System.Windows.Forms.Cursor.Position.X - this.Left;
            y = System.Windows.Forms.Cursor.Position.Y - this.Top;
        }

        void DragOff()
        {
            drag = false;
        }

        void Displace() {
            if (drag)
            {
                this.Top = System.Windows.Forms.Cursor.Position.Y - y;
                this.Left = System.Windows.Forms.Cursor.Position.X - x;
            }
        }

        private void Main_MouseDown(object sender, MouseEventArgs e)
        {
            DragOn();
        }

        private void Main_MouseUp(object sender, MouseEventArgs e)
        {
            DragOff();
        }

        private void Main_MouseMove(object sender, MouseEventArgs e)
        {
            Displace();
        }

        private void ResetFrame_Click(object sender, EventArgs e)
        {
            MainFrame.Image = source;
        }

        private void ContrastFrame_Click(object sender, EventArgs e)
        {
            MainFrame.Image = ContrastFrame.Image;
        }

        private void BNFrame_Click(object sender, EventArgs e)
        {
            MainFrame.Image = BNFrame.Image;
        }

        private void BlueFrame_Click(object sender, EventArgs e)
        {
            MainFrame.Image = BlueFrame.Image;
        }

        private void RedFrame_Click(object sender, EventArgs e)
        {
            MainFrame.Image = RedFrame.Image;
        }

        private void SepiaFrame_Click(object sender, EventArgs e)
        {
            MainFrame.Image = SepiaFrame.Image;
        }

        private void UltraRedFrame_Click(object sender, EventArgs e)
        {
            MainFrame.Image = UltraRedFrame.Image;
        }

        private void PinkFrame_Click(object sender, EventArgs e)
        {
            MainFrame.Image = PinkFrame.Image;
        }

        private void VintageFrame_Click(object sender, EventArgs e)
        {
            MainFrame.Image = VintageFrame.Image;
        }

        private void MainFrame_Click(object sender, EventArgs e)
        {
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            saveFileDialog.Filter = "Bitmap Image (.bmp)|*.bmp|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png";
            saveFileDialog.ShowDialog();
        }

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {           
            MainFrame.Image.Save(saveFileDialog.FileName);
        }

        private void MainFrame_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void MainFrame_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        private void ResetFrame_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void ResetFrame_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        private void ContrastFrame_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void ContrastFrame_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        private void BNFrame_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void BNFrame_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        private void BlueFrame_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void BlueFrame_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        private void RedFrame_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void RedFrame_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        private void SepiaFrame_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void SepiaFrame_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        private void UltraRedFrame_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void UltraRedFrame_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        private void PinkFrame_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void PinkFrame_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }

        private void VintageFrame_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void VintageFrame_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }
    }
}
