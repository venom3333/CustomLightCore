using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class Essentials
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] LogoImageData { get; set; }
        public string LogoImageMimeType { get; set; }
        public string About { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Boss { get; set; }
        public string Email { get; set; }
        public byte[] LogoImageInvertedData { get; set; }
        public string LogoImageInvertedMimeType { get; set; }
    }
}
