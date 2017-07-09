using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class Pages
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public string PageContent { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
