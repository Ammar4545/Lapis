﻿using System.Collections;
using System.Collections.Generic;

namespace Lapis_Models.ViewModel
{
    public class HomeVM
    {
        public IEnumerable<Product> Products { get; set; }
        public  IEnumerable<Category> categories { get; set; }
    }
}
