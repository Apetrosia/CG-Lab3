using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace CG_Lab2
{
    public partial class FormTask2 : Form
    {
        int[] pixelsRed = new int[256];
        int[] pixelsGreen = new int[256];
        int[] pixelsBlue = new int[256];

        public FormTask2()
        {
            InitializeComponent();
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
                    
                    /*
                    if (image.Width > image.Height)
                    {
                        PictureBoxSource.Width = 250;
                        PictureBoxSource.Height = image.Height * 250 / image.Width;
                        PictureBoxSource.Location = new Point(21, 38);
                    }
                    else if (image.Width < image.Height)
                    {
                        PictureBoxSource.Height = 200;
                        PictureBoxSource.Width = image.Width * 200 / image.Height;
                        PictureBoxSource.Location = new Point(46, 17);
                    }
                    else
                    {
                        PictureBoxSource.Width = 200;
                        PictureBoxSource.Height = 200;
                        PictureBoxSource.Location = new Point(21, 17);
                    }
                    */

                    PictureBoxSource.Image = image;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void GetRGBImages(Bitmap image)
        {
            for (int i = 0; i < 256; i++)
            {
                pixelsRed[i] = 0;
                pixelsGreen[i] = 0;
                pixelsBlue[i] = 0;
            }

            Bitmap imageRed = new Bitmap(image.Width, image.Height);
            Bitmap imageGreen = new Bitmap(image.Width, image.Height);
            Bitmap imageBlue = new Bitmap(image.Width, image.Height);

            for (int i = 0; i < image.Width; i++)
                for (int j = 0; j < image.Height; j++)
                {
                    int R = image.GetPixel(i, j).R;
                    int G = image.GetPixel(i, j).G;
                    int B = image.GetPixel(i, j).B;

                    imageRed.SetPixel(i, j, Color.FromArgb(R, 0, 0));
                    imageGreen.SetPixel(i, j, Color.FromArgb(0, G, 0));
                    imageBlue.SetPixel(i, j, Color.FromArgb(0, 0, B));

                    pixelsRed[R]++;
                    pixelsGreen[G]++;
                    pixelsBlue[B]++;
                }

            pictureBoxRed.Image = imageRed;
            pictureBoxGreen.Image = imageGreen;
            pictureBoxBlue.Image = imageBlue;
        }

        private void DrawHistograms()
        {
            Bitmap histImageRed = new Bitmap(257, 200);
            Bitmap histImageGreen = new Bitmap(257, 200);
            Bitmap histImageBlue = new Bitmap(257, 200);

            using (Graphics gR = Graphics.FromImage(histImageRed))
            using (Graphics gG = Graphics.FromImage(histImageGreen))
            using (Graphics gB = Graphics.FromImage(histImageBlue))
            {
                gR.Clear(Color.White);
                gG.Clear(Color.White);
                gB.Clear(Color.White);

                for (int i = 0; i < 256; i++)
                {
                    int height = pixelsRed.Max() == 0 ? 0 : (int)((double)pixelsRed[i] / pixelsRed.Max() * 200);
                    gR.FillRectangle(Brushes.Red, i, 200 - height, 1, height);
                    height = pixelsGreen.Max() == 0 ? 0 : (int)((double)pixelsGreen[i] / pixelsGreen.Max() * 200);
                    gG.FillRectangle(Brushes.Green, i, 200 - height, 1, height);
                    height = pixelsBlue.Max() == 0 ? 0 : (int)((double)pixelsBlue[i] / pixelsBlue.Max() * 200);
                    gB.FillRectangle(Brushes.Blue, i, 200 - height, 1, height);
                }
            }

            histRed.Image = histImageRed;
            histGreen.Image = histImageGreen;
            histBlue.Image = histImageBlue;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (PictureBoxSource.Image == null)
                return;

            GetRGBImages(new Bitmap(PictureBoxSource.Image));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (PictureBoxSource.Image == null)
                return;

            DrawHistograms();
        }
    }
}
