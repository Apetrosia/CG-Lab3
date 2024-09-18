using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CG_Lab2
{
    public partial class FormTask1 : Form
    {
        public FormTask1()
        {
            InitializeComponent();
        }

        private enum ConvertionType { PAL, HDTV }

        private Bitmap ConvertToGray(Bitmap image, ConvertionType convertionType)
        {
            Bitmap grayScale = new Bitmap(image.Width, image.Height);
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixel = image.GetPixel(x, y);
                    
                    int gray = 0;
                    if (convertionType == ConvertionType.PAL)
                    {
                        gray = (int)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);
                    }
                    else if (convertionType == ConvertionType.HDTV)
                    {
                        gray = (int)(0.2126 * pixel.R + 0.7152 * pixel.G + 0.0722 * pixel.B);
                    }
                    
                    grayScale.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }
            return grayScale;
        }

        private Bitmap GetDifference(Bitmap image1, Bitmap image2)
        {
            Bitmap difference = new Bitmap(image1.Width, image1.Height);
            for (int x = 0; x < image1.Width; x++)
            {
                for (int y = 0; y < image1.Height; y++)
                {
                    Color pixel1 = image1.GetPixel(x, y);
                    Color pixel2 = image2.GetPixel(x, y);
                    int diffR = Math.Abs(pixel1.R - pixel2.R);
                    int diffG = Math.Abs(pixel1.G - pixel2.G);
                    int diffB = Math.Abs(pixel1.B - pixel2.B);
                    difference.SetPixel(x, y, Color.FromArgb(diffR, diffG, diffB));
                }
            }
            return difference;
        }

        private void DrawHistogram(Bitmap image, PictureBox pictureBox)
        {
            int[] hist = new int[256];
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixel = image.GetPixel(x, y);
                    hist[pixel.R]++;
                }
            }

            int maxValue = hist.Max();

            Bitmap histImage = new Bitmap(256, 200);
            using (Graphics g = Graphics.FromImage(histImage))
            {
                g.Clear(Color.White);

                for (int i = 0; i < 256; i++)
                {
                    int height = (int)((double)hist[i] / maxValue * 200);
                    g.FillRectangle(Brushes.Red, i, 200 - height, 1, height);
                }
            }

            pictureBox.Image = histImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Bitmap image = new Bitmap(openFileDialog.FileName);
                    mainPictureBox.Image = image;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (mainPictureBox.Image == null)
                return;

            Bitmap grayScale = ConvertToGray((Bitmap)mainPictureBox.Image, ConvertionType.PAL);

            pictureBox1.Image = grayScale;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (mainPictureBox.Image == null)
                return;

            Bitmap grayScale = ConvertToGray((Bitmap)mainPictureBox.Image, ConvertionType.HDTV);

            pictureBox2.Image = grayScale;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null || pictureBox2.Image == null)
                return;

            Bitmap diff = GetDifference((Bitmap)pictureBox1.Image, (Bitmap)pictureBox2.Image);

            pictureBox3.Image = diff;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
                return;

            DrawHistogram((Bitmap)pictureBox1.Image, pictureBox4);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image == null)
                return;

            DrawHistogram((Bitmap)pictureBox2.Image, pictureBox5);
        }
    }
}
