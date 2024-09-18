using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CG_Lab2
{
    public partial class FormTask3 : Form
    {
        private Bitmap RGBImage;
        private Bitmap HSVImage;
        public FormTask3()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;**.PNG)|*.BMP;*.JPG;**.PNG|All files (*.*)|*.*";
        }

        private void RGBtoHSV(Color color, out float hue, out float saturation, out float value)
        {
            float red = color.R / 255f;
            float green = color.G / 255f;
            float blue = color.B / 255f;

            float min = Math.Min(red, Math.Min(green, blue));
            float max = Math.Max(red, Math.Max(green, blue));

            hue = 0;
            if (min == max)
            {
                hue = 0;
            }
            else if (max == red && green >= blue)
            {
                hue = 60 * (green - blue) / (float)(max - min) + 0;
            }
            else if (max == red && green < blue)
            {
                hue = 60 * (green - blue) / (float)(max - min) + 360;
            }
            else if (max == green)
            {
                hue = 60 * (blue - red) / (float)(max - min) + 120;
            }
            else if (max == blue)
            {
                hue = 60 * (red - green) / (float)(max - min) + 240;
            }

            saturation = (max == 0) ? 0 : 1f - (1f * min / max);

            value = max ;
        }
        private Color HSVtoRGB(float hue, float saturation, float value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            float f = hue / 60 - (float)Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));


            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                 return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        private Bitmap makeHSVImage(Bitmap image, float hShift, float sShift, float vShift)
        {
            Bitmap result = new Bitmap(image.Width, image.Height);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    float h, s, v;
                    RGBtoHSV(pixelColor, out h, out s, out v);

                    // Изменение значений оттенка, насыщенности и яркости
                    h = (h + hShift) % 360; //от 0 до 360
                    s = Math.Min(1, s * sShift); //от 0 до 1
                    v = Math.Min(1, v * vShift); // от 0 до 1

                    Color newColor = HSVtoRGB(h, s, v);
                    result.SetPixel(x, y, newColor);
                }
            }
            return result;
        }
        private void trackBar_Scroll(object sender, EventArgs e)
        {
            int trackBar1_new_value = trackBar1.Value % 360;
            label1.Text = String.Format("Hue: {0}°", trackBar1_new_value);
            label2.Text = String.Format("Saturation: {0}%", trackBar2.Value);
            label3.Text = String.Format("Brightness: {0}%", trackBar3.Value);
            if (RGBImage != null)
            {
                float h = trackBar1.Value;
                float s = trackBar2.Value / 100f;
                float v = trackBar3.Value / 100f;

                HSVImage = makeHSVImage(RGBImage, h, s, v);
                pictureBox1.Image = HSVImage;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image.Save("image_new.jpeg", ImageFormat.Jpeg);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Title = "Выберите изображение";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                RGBImage = new Bitmap(openFileDialog1.FileName);
                int old_pb_width = pictureBox1.Width;
                int old_pb_height = pictureBox1.Height;
                pictureBox1.Size = RGBImage.Size;
                pictureBox1.Image = RGBImage;

                // Получаем размер изображения
                int imageWidth = RGBImage.Width;
                int imageHeight = RGBImage.Height;

                //разница между размером изображения и старым размером picturebox
                int extraWidth = imageWidth - old_pb_width;
                int extraHeight = imageHeight - old_pb_height;

                // Ограничение размеров по экрану
                int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
                int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

                int newWidth = Math.Min(imageWidth + extraWidth, screenWidth);
                int newHeight = Math.Min(imageHeight + extraHeight, screenHeight);

                // Изменяем размер формы, чтобы вместить изображение
                this.Size = new Size(newWidth, newHeight);


                this.Update();
            }

        }


    }
}
