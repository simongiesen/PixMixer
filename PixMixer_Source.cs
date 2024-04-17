using System.Drawing;
using System.Drawing.Imaging;

class Program
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Plattformkompatibilität überprüfen", Justification = "<Ausstehend>")]
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Bitte geben Sie den Dateipfad des JPEG-Bildes als Argument an.");
            return;
        }

        string inputImagePath = args[0];
        if (!File.Exists(inputImagePath))
        {
            Console.WriteLine("Die angegebene Datei existiert nicht.");
            return;
        }

        try
        {
            using (Bitmap originalImage = new Bitmap(inputImagePath))
            {
                int width = originalImage.Width / 2;
                int height = originalImage.Height / 2;

                Bitmap[] quadrants = new Bitmap[4];
                quadrants[0] = originalImage.Clone(new Rectangle(0, 0, width, height), PixelFormat.Format24bppRgb);
                quadrants[1] = originalImage.Clone(new Rectangle(width, 0, width, height), PixelFormat.Format24bppRgb);
                quadrants[2] = originalImage.Clone(new Rectangle(0, height, width, height), PixelFormat.Format24bppRgb);
                quadrants[3] = originalImage.Clone(new Rectangle(width, height, width, height), PixelFormat.Format24bppRgb);
                 
                int resultWidth = originalImage.Width + width;
                int resultHeight = originalImage.Height + height * 2;

                using (Bitmap resultImage = new Bitmap(resultWidth, resultHeight))
                {
                    resultImage.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);

                    using (Graphics graphics = Graphics.FromImage(resultImage))
                    {
                        graphics.Clear(Color.White);
                    }

                    using (Graphics graphics = Graphics.FromImage(resultImage))
                    { 
                        graphics.DrawImage(originalImage, 0, 0);
                         
                        graphics.DrawImage(quadrants[0], originalImage.Width, originalImage.Height);  
                        graphics.DrawImage(quadrants[2], originalImage.Width, originalImage.Height - height); 
                        graphics.DrawImage(quadrants[1], originalImage.Width - width, originalImage.Height);
                    }

                    #pragma warning disable CS8604  
                    string outputImagePath = Path.Combine(Path.GetDirectoryName(inputImagePath), $"{Path.GetFileNameWithoutExtension(inputImagePath)}_result.jpg");
                    #pragma warning restore CS8604  
                    resultImage.Save(outputImagePath, ImageFormat.Jpeg);
                    Console.WriteLine("Das Ergebnisbild wurde unter {0} gespeichert.", outputImagePath);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ein Fehler ist aufgetreten: " + ex.Message);
        }
    }
}
