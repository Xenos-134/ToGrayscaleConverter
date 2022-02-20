using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToGrayscaleConverter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Bitmap imageCoppy = null;
            //using (Bitmap image = new Bitmap(System.IO.Path.Combine(Environment.SpecialFolder.DesktopDirectory.ToString(), "AL.jpg")))
            using (Bitmap image = new Bitmap("C:/Users/artem/Desktop/AL.jpg"))
            {
                int x, y;

                for (x = 0; x < image.Width; x++)
                {
                    for (y = 0; y < image.Height; y++)
                    {
                        Color pixelColor = image.GetPixel(x, y);

                        int nc = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                        Color newColor = Color.FromArgb(nc, nc, nc);





                        image.SetPixel(x, y, newColor); // Now greyscale
                        Console.Write((float)nc/255 + " ");
                    }
                    Console.WriteLine();    
                }


                imageCoppy = image;   // imageCoppy is grayscale version of image
                //System.IO.File.Move("C:/Users/artem/Desktop/", );
                imageCoppy.Save("C:/Users/artem/Desktop/NA", System.Drawing.Imaging.ImageFormat.Png);

            }

        }
    }
}
