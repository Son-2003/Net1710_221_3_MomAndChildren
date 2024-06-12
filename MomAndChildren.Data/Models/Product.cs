﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MomAndChildren.Data.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public int Status { get; set; }

    public DateTime ExpireDate { get; set; }

    public DateTime ManufacturingDate { get; set; }

    public int Quantity { get; set; }

    public double Price { get; set; }

    public string Description { get; set; }

    public int BrandId { get; set; }

    public int CategoryId { get; set; }

    public virtual Brand Brand { get; set; }

    public virtual Category Category { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}