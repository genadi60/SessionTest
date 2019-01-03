using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SessionTest.Data;
using SessionTest.Models;

namespace SessionTest
{
    public static class Seeder
    {
        public static void Seed(ApplicationDbContext context)
        {

            //var categories = new List<Category>
            //{
            //    new Category{Name = "Electronics"},
            //    new Category{Name = "Bullshits"},
            //    new Category{Name = "Something Else"},
            //    new Category{Name = "Whatever"},
            //    new Category{Name = "Other"}
            //};

            //context.Categories.AddRange(categories);
            //context.SaveChanges();

            //for (int i = 0; i < 10; i++)
            //{
            //    var product = new Product
            //    {
            //        Name = $"Product {i}",
            //        Description = $"Description of product {i}",
            //        Price = (decimal)(new Random().NextDouble() * (new Random()).Next(10000)),
            //        CategoryId = new Random().Next(1,6)
            //    };

            //    context.Products.Add(product);
            //}

            var paymentMethods = new List<PaymentMethod>
            {
                new PaymentMethod{Name = "CreditCard"},
                new PaymentMethod{Name = "BankTransfer"},
                new PaymentMethod{Name = "PayPal"}
            };

            context.PaymentMethods.AddRange(paymentMethods);
            context.SaveChanges();

            //for (int i = 0; i < 20; i++)
            //{
            //    var blog = new Blog
            //    {
            //        Title = $"BlogTitle{i}",
            //        Content = $"{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}{i} nqma takyv blog tova e voenno obuchenie. Vnimanie: Voenno Obuchenie. ALARM, Trevoga, Zaboga. Voenno obuchenie sssssssssssssssssssssss"
            //    };

            //    context.Blogs.Add(blog);
            //}

            //context.SaveChanges();
        }

        public static void SeedPictures(ApplicationDbContext context)
        {
            var blogs = context.Blogs.Take(10).ToList();

            for (int i = 1; i < 11; i++)
            {
                Blog blog = blogs.Skip(i - 1).First();

                blog.PictureUri = $"images/products/{i}.jpg";
            }

            context.SaveChanges();
        }

        public static async void SeedRoles(ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
            }

            if (!context.Roles.Any(r => r.Name == "User"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "User", NormalizedName = "USER" });
            }

            if (!context.Roles.Any(r => r.Name == "Cashier"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "Cashier", NormalizedName = "CASHIER" });
            }

            await context.SaveChangesAsync();
        }
    }
}
