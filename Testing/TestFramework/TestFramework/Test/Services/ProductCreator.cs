using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TestFramework.Test.Models;

namespace TestFramework.Test.Services
{
    public class ProductCreator
    {
        public static Product CreateProduct(IConfiguration configuration)
        {
            var product = new Product
            {
                AddToCart = configuration["product:addToCart"],
                Compare = configuration.GetSection("product:compare").Get<List<string>>(),
                Manufacturer = configuration["product:manufacturer"],
                MaxPrice = configuration.GetValue<int>("product:maxPrice"),
                Search = configuration["product:search"]
            };
            return product;
        }
    }
}
