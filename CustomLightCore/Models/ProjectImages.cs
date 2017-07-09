using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class ProjectImages
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        public int ProjectId { get; set; }

        public virtual Projects Project { get; set; }
    }
}
