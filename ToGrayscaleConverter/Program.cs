using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
                int[,] bitMapArr = new int[7,5];
                int topLX = Int32.MaxValue, botRX = 0;
                int topLY = Int32.MaxValue, botRY = 0;

                for (y = 0; y < image.Height; y++)
                {
                    bool trigger = false;
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
               
                Bitmap nImage = image.Clone(new System.Drawing.Rectangle(topLX, topLY, (botRX - topLX), (botRY - topLY)), image.PixelFormat);
                imageCoppy = nImage;   // imageCoppy is grayscale version of image


                Image rImageCoppy = (Image)(new Bitmap(imageCoppy, new Size(500,700)));

                Console.WriteLine($"Height:{rImageCoppy.Height}/ Width:{rImageCoppy.Width}");

                //imageCoppy.Save("C:/Users/artem/Desktop/NA", System.Drawing.Imaging.ImageFormat.Png);
                rImageCoppy.Save("C:/Users/artem/Desktop/NAR", System.Drawing.Imaging.ImageFormat.Png);


                //Faz print das cores da imagem
                Bitmap bmi = new Bitmap(rImageCoppy);
                for (int h = 0; h < 700; h++)
                {
                    for (int w = 0; w < 500; w++)
                    { 
                        Color pc = bmi.GetPixel(w, h);
                        int mp = (pc.R + pc.G + pc.B) / 3;

                        if (mp < (255 * 0.7))
                        {
                            //Console.WriteLine($"{h/100}, {w/100}");
                            //Caso area tem algma parte da letra
                            bitMapArr[h/100, w /100] = 1;
                        }
                    }
                }

                for (int h = 0; h < 7; h++)
                {
                    for (int w = 0; w < 5; w++)
                    {
                        if (bitMapArr[h, w] == 1)
                        {
                            Console.Write($"[X] ");
                        }
                        else
                        {
                            Console.Write($"[ ] ");
                        }
                    }
                    Console.WriteLine();
                }
                        Console.ReadLine();
            }

        }
    }
}
