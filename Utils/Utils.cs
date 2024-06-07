using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AntiqueShop.Utils
{
    internal class Utils
    {
        public static bool AnyIsNullOrEmpty(string[] strings)
        {
            if (strings == null)
            {
                return true;
            }

            foreach (string str in strings)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return true;
                }

                if (string.IsNullOrWhiteSpace(str))
                {
                    return true;
                }
            }

            return false;
        }

        public static BitmapImage GenerateQR(string data)
        {
            QRCodeGenerator generator = new QRCodeGenerator();
            QRCodeData qrCodeData = generator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap graphics = qrCode.GetGraphic(20);

            // Convert Bitmap to BitmapImage
            BitmapImage bitmapImage = new BitmapImage();
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                graphics.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }
    }
}
