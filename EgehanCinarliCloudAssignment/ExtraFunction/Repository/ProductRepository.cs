using AutoMapper;
using ExtraFunction.DAL;
using ExtraFunction.DTO;
using ExtraFunction.Model;
using ExtraFunction.Repository.Interface;
using ExtraFunction.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExtraFunction.Repository
{
    public class ProductRepository : IProductRepository
    {
        private DatabaseContext dbContext;


        public ProductRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }     
        public async Task CreateProduct(CreateUpdateProductDTO productDTO)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateUpdateProductDTO, Product>()));
            Product product = mapper.Map<Product>(productDTO);
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
        }
        public async Task<Product> GetProductById(Guid productId)
        {
            await dbContext.SaveChangesAsync();
            return dbContext.Products.FirstOrDefault(c => c.Id == productId);
        }

        public async Task RemoveProduct(Guid productId)
        {
            Product product = await GetProductById(productId);
            dbContext.Products.Remove(product);
           await dbContext.SaveChangesAsync();
        }

        public async Task<bool> CheckIfProductExist(Guid productId)
        {
            await dbContext.SaveChangesAsync();
            if (dbContext.Products.Count(x => x.Id == productId) > 0)
                return true;
            else
                return false;
        }

        public async Task UpdateProduct(Guid productId, CreateUpdateProductDTO updateProductDTO)
        {
            await dbContext.SaveChangesAsync();

            Product product = await GetProductById(productId);
            product.Description = updateProductDTO.Description;
            product.Name = updateProductDTO.Name;
            product.Stock = updateProductDTO.Stock;
            dbContext.Products.Update(product);
            await dbContext.SaveChangesAsync();
        }

        public  async Task<IEnumerable<Product>> GetAllProducts()
        {
            List<Product> listOfProducts=  dbContext.Products.OrderBy(p=>p.Name).ToList();

            return listOfProducts;
        }
    }
}
