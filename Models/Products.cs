//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AntiqueShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    
    public partial class Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Products()
        {
            this.CartItems = new HashSet<CartItems>();
        }
    
        public int product_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int category_id { get; set; }
        public string image_url { get; set; }
        public string image_path
        {
            get
            {
                if (!File.Exists($"../../Images/{image_url}"))
                {
                    return "../Images/placeholder.jpg";
                }

                return $"../Images/{image_url}";
            }
        }
        public int stock { get; set; }
        public byte is_featured { get; set; }
        public Nullable<int> size_id { get; set; }
        public Nullable<int> color_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CartItems> CartItems { get; set; }
        public virtual Categories Categories { get; set; }
        public virtual Colors Colors { get; set; }
        public virtual Sizes Sizes { get; set; }
    }
}
