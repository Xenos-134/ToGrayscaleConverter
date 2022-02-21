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

                int topLX = Int32.MaxValue, botRX = 0;
                int topLY = Int32.MaxValue, botRY = 0;

                for (y = 0; y < image.Height; y++)
                {
                    for (x = 0; x < image.Width; x++)
                    {
                        Color newColor;
                        Color pixelColor = image.GetPixel(x, y);
                        int nc = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

                        if (nc < (255 * 0.7))
                        {
                            //Gets coordinates of the upper corner
                            if (x < topLX) topLX = x;
                            if(y < topLY) topLY = y;    

                            if(x > botRX) botRX = x;
                            if(y > botRY) botRY = y;
                        }




                        if (nc < (255 * 0.7)) newColor = Color.FromArgb(0, 0, 0);
                        else
                        {
                            newColor = Color.FromArgb(255, 255, 255);
                        }

                        image.SetPixel(x, y, newColor); // Now greyscale

                    }
                }
                Console.WriteLine($"{image.Width}-{image.Height}");

                Console.WriteLine($"Top ({topLX},{topLY})| ({botRX}, {botRY})");

                Console.WriteLine($" Width {(botRX - topLX)} || Height {(botRY - topLY)}");

                //Bitmap nImage = image.Clone(new System.Drawing.Rectangle(topLX, botRY, (botRX-topLX), (botRY-topLY)), image.PixelFormat);
                Bitmap nImage = image.Clone(new System.Drawing.Rectangle(topLX, topLY, (botRX - topLX), (botRY - topLY)), image.PixelFormat);


                imageCoppy = image;   // imageCoppy is grayscale version of image
                
                imageCoppy.Save("C:/Users/artem/Desktop/NA", System.Drawing.Imaging.ImageFormat.Png);
                nImage.Save("C:/Users/artem/Desktop/NAR", System.Drawing.Imaging.ImageFormat.Png);
                Console.ReadLine();

            }

        }
    }
}
