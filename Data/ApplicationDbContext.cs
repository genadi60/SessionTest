﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SessionTest.Models;

namespace SessionTest.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        //public DbSet<IdentityUser> WebShopUsers { get; set; }

        //public DbSet<Order> Orders { get; set; }

        //public DbSet<Receipt> Receipts { get; set; }

        //public DbSet<Payment> Payments { get; set; }

        //public DbSet<ClientReceipt> ClientReceipts { get; set; }

        //public DbSet<ReceiptOrder> ReceiptOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


        }
    }
}
