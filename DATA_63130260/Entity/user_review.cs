//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DATA_63130260.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class user_review
    {
        public int id { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> ordered_product_id { get; set; }
        public Nullable<int> rating_value { get; set; }
        public Nullable<System.DateTime> rating_date { get; set; }
        public string comment { get; set; }
    
        public virtual order_line order_line { get; set; }
        public virtual site_user site_user { get; set; }
    }
}