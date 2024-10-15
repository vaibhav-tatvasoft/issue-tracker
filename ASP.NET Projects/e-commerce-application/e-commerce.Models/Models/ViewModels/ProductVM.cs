﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace e_commerce.Models.Models.ViewModels
{
    public class ProductVM
    {
        public Product product { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
