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

                for (y = 0; y < image.Height; y++)
                {
                    for (x = 0; x < image.Width; x++)
                    {
                        Color newColor;
                        Color pixelColor = image.GetPixel(x, y);
                        int nc = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                        if (nc < (255 * 0.7)) newColor = Color.FromArgb(0, 0, 0);
                        else
                        {
                            newColor = Color.FromArgb(255, 255, 255);
                        }

                        image.SetPixel(x, y, newColor); // Now greyscale

                        //if(x%50 == 0 && y % 50 == 0) Console.Write($"[{(1-(float)nc / 255).ToString("n1")}] ");
                        if (x % 50 == 0 && y % 50 == 0)
                        {
                            if ((1 - (float)nc / 255) != 0) Console.Write("[x] ");
                            else {
                                Console.Write("[ ] ");
                            }
                        }


                    }
                    if (y % 50 == 0)  Console.WriteLine();    
                }


                imageCoppy = image;   // imageCoppy is grayscale version of image
                //System.IO.File.Move("C:/Users/artem/Desktop/", );
                imageCoppy.Save("C:/Users/artem/Desktop/NA", System.Drawing.Imaging.ImageFormat.Png);
                Console.ReadLine();

            }

        }
    }
}
