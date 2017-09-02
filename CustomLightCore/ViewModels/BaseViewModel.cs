using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageSharp;
using System.IO;

namespace CustomLightCore.ViewModels
{
    public enum ImageType
    {
        Icon,
        Full,
        Slide,
        Logo
    }

    public class BaseViewModel
    {
        private const int IconImageHeight = 270;
        private const int IconImageWidth = 360;

        private const int FullImageHeight = 768;
        private const int FullImageWidth = 1024;

        private const int SlideImageHeight = 300;
        private const int SlideImageWidth = 1920;

        private const int LogoImageHeight = 80;
        private const int LogoImageWidth = 400;

        private const int quality = 85;


        /// <summary>
        /// Ресайз изображений
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="imageType"></param>
        protected static byte[] ImageProcess(byte[] img, ImageType imageType = ImageType.Full)
        {
            int width;
            int height;

            byte[] result = null;

            switch (imageType)
            {
                case ImageType.Icon:
                    width = IconImageWidth;
                    height = IconImageHeight;
                    break;
                case ImageType.Logo:
                    width = LogoImageWidth;
                    height = LogoImageHeight;
                    break;
                case ImageType.Slide:
                    width = SlideImageWidth;
                    height = SlideImageHeight;
                    break;
                case ImageType.Full:
                default:
                    width = FullImageWidth;
                    height = FullImageHeight;
                    break;
            }

            // Загрузка изображения
            var imgFormat = Image.DetectFormat(img);

            if(imgFormat == null)
            {
                return img;
            }

            var newImage = Image.Load(img);

            // Ресайз
            newImage = newImage.Resize(new ImageSharp.Processing.ResizeOptions
            {
                Mode = ImageSharp.Processing.ResizeMode.Crop,
                Size = new SixLabors.Primitives.Size(width, height)
            });

            // Преобразование в byte[]
            MemoryStream ms = new MemoryStream();
            newImage.Save(ms, imgFormat);

            result = ms.ToArray();

            // Возврат результата
            return result;
        }
    }
}
