using AutoMapper;
using Azure.Storage.Queues;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using ShowerShow.DAL;
using ShowerShow.DTO;
using ShowerShow.Model;
using ShowerShow.Models;
using ShowerShow.Repository.Interface;
using ShowerShow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShowerShow.Repository
{
    public class ProductRepository : IProductRepository
    {
        private DatabaseContext dbContext;


        public ProductRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }     
        public async Task CreateProduct(ProductDTO productDTO)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<ProductDTO, Product>()));
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
        }

        public async Task UpdateProduct(Guid productId, ProductDTO updateProductDTO)
        {
            await dbContext.SaveChangesAsync();

            Product product = await GetProductById(productId);
            product.Description = updateProductDTO.Description;
            product.Name = updateProductDTO.Name;
            product.Stock = updateProductDTO.Stock;
            dbContext.Products.Update(product);
        }
    }
}
