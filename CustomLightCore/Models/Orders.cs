﻿using System;
using System.Collections.Generic;

namespace CustomLightCore.Models
{
    public partial class Orders
    {
        public int Id { get; set; }
        public string OrderString { get; set; }
        public DateTime Created { get; set; }
    }
}
