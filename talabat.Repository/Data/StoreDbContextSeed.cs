using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order;

namespace talabat.Repository.Data
{
    public static class StoreDbContextSeed
    {
        public static async Task SeedAsync(StoreDbContext _context)
        {

            #region BrandSeeding 

            if (_context.Brands.Count() == 0)
            {
                var brandData = await File.ReadAllTextAsync("../talabat.Repository/Data/DataSeed/brands.json");

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                if (brands?.Count > 0)
                {
                    foreach (var brand in brands)
                    {

                        _context.Set<ProductBrand>().Add(brand);
                    }
                    await _context.SaveChangesAsync();
                }

            }

            #endregion

            #region CategorySeeding

            if (_context.Types.Count() == 0)
            {
                var categoryData = await File.ReadAllTextAsync("../talabat.Repository/Data/DataSeed/categories.json");

                var categories = JsonSerializer.Deserialize<List<ProductType>>(categoryData);

                if (categories?.Count > 0)
                {
                    foreach (var category in categories)
                    {

                        _context.Set<ProductType>().Add(category);
                    }
                    await _context.SaveChangesAsync();
                }
            }
            #endregion

            #region Product Seeding 
            if (_context.Products.Count() == 0)
            {


                var productData = await File.ReadAllTextAsync("../talabat.Repository/Data/DataSeed/products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(productData);

                if (products?.Count > 0)
                {
                    foreach (var product in products)
                    {

                        _context.Set<Product>().Add(product);
                    }
                    await _context.SaveChangesAsync();
                }
            }
            #endregion
            #region Delievry methods Seeding 

            if (_context.DeliveryMethods.Count() == 0)
            {


                var delievryData = await File.ReadAllTextAsync("../talabat.Repository/Data/DataSeed/delivery.json");

                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(delievryData);

                if (deliveryMethods?.Count > 0)
                {
                    foreach (var delivery in deliveryMethods)
                    {

                        _context.DeliveryMethods.Add(delivery);
                    }
                    await _context.SaveChangesAsync();
                }
            }
            #endregion
        }
    }
}
 