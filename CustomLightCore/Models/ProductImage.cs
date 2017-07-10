using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class ProductImage
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
