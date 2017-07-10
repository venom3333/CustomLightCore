using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class Slide
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
