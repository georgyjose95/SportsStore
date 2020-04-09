using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class EFProductRepository : IProductRepository
    {
        private ApplicationDbContext context;

        public EFProductRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Product> Products => context.Products;
        /*The repository implementation just maps the Products property defined by the 
         IProductRepository interface on to the Products property defined by the ApplicationDbContext class. 
         The Products property in the context class returns a DbSet<Product> object, which implements the IQueryable<T> interface 
         and makes it easy to implement the IProductRepository interface uisng EFCore. This ensure that the DB will retrieve only the object that are required. 
         */

        public void SaveProduct(Product product)
        {
            if(product.ProductID == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                Product dbEntry = context.Products.FirstOrDefault(p => p.ProductID == product.ProductID);
                if(dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                }
            }
            context.SaveChanges();
        }          
        public Product DeleteProduct(int productID)
        {
            Product dbEntry = context.Products.FirstOrDefault(p => p.ProductID == productID);
            if(dbEntry != null)
            {
                context.Products.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
