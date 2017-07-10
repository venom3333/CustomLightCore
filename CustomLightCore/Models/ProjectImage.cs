using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class ProjectImage
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }
}
