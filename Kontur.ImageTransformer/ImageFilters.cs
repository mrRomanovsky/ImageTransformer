using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer
{
    static class ImageFilters
    {
        public static Bitmap Grayscale(Bitmap original)
        {
            var res = new Bitmap(original.Width, original.Height);
            int width = original.Width;
            int height = original.Height;
            for (int i = 0; i < width; ++i)
            for (int j = 0; j < height; ++j)
            {
                var pixel = original.GetPixel(i, j);
                var intensity = GetIntensity(pixel);
                res.SetPixel(i, j, Color.FromArgb(intensity, intensity, intensity));

            }
            return res;
        }

        public static Bitmap Sepia(Bitmap original)
        {
            var res = new Bitmap(original.Width, original.Height);
            int width = original.Width;
            int height = original.Height;
            for (int i = 0; i < width; ++i)
            for (int j = 0; j < height; ++j)
            {
                var pixel = original.GetPixel(i, j);
                int newR = TruncateFloat((float)(pixel.R * 0.393) + (float)(pixel.G * 0.769) + (float)(pixel.B * 0.189));
                int newG = TruncateFloat((float)(pixel.R * 0.349) + (float)(pixel.G * 0.686) + (float)(pixel.B * 0.168));
                int newB = TruncateFloat((float)(pixel.R * 0.272) + (float)(pixel.G * 0.534) + (float)(pixel.B * 0.131));
                res.SetPixel(i, j, Color.FromArgb(newR, newG, newB));
            }
            return res;
        }

        private static int TruncateFloat(float flNumer)
        {
            return flNumer > 255 ? 255 : (int)flNumer;
        }

        public static Bitmap Threshold(Bitmap original, int param)
        {
            var res = new Bitmap(original.Width, original.Height);
            int width = original.Width;
            int height = original.Height;
            for (int i = 0; i < width; ++i)
            for (int j = 0; j < height; ++j)
            {
                var intensity = GetIntensity(original.GetPixel(i, j));
                if (intensity >= 255 * param / 100)
                    res.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                else
                    res.SetPixel(i, j, Color.FromArgb(0, 0, 0));
            }
            return res;
        }

        private static int GetIntensity(Color color)
        {
            return (color.R + color.G + color.B) / 3;
        }
    }
}

