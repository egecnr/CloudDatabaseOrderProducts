using AutoMapper;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using ProductAndReviewFunction.DTO;
using ProductAndReviewFunction.Model;
using ProductAndReviewFunction.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductAndReviewFunction.Service
{
    public class ProductService : IProductService
    {
        private IProductRepository productRepository;


        public ProductService(IProductRepository productRepository)
        {

            this.productRepository = productRepository; 
        }

        public async Task<bool> CheckIfProductExist(Guid productId)
        {
            return await productRepository.CheckIfProductExist(productId);
        }

        public async Task CreateProduct(CreateUpdateProductDTO productDTO)
        {
            await productRepository.CreateProduct(productDTO);
        }

        public Task<IEnumerable<Product>> GetAllProducts()
        {
            return productRepository.GetAllProducts();
        }

        public async Task<Product> GetProductById(Guid productId)
        {
            if(await CheckIfProductExist(productId))
            return await productRepository.GetProductById(productId);
            else
            {
                throw new Exception($"Product with id {productId} does not exist.");
            }
        }

        public async Task RemoveProduct(Guid productId)
        {
            if (await CheckIfProductExist(productId))
                await productRepository.RemoveProduct(productId);
            else
            {
                throw new Exception($"Product with id {productId} does not exist.");
            }
        }

        public async Task UpdateProduct(Guid productId, CreateUpdateProductDTO updateProductDTO)
        {
            if (await CheckIfProductExist(productId))
            {
                await productRepository.UpdateProduct(productId, updateProductDTO);
            }
            else
            {
                throw new Exception($"Product with id {productId} does not exist.");
            }
        }
    }
}
