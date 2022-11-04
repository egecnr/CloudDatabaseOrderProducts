
using ProductAndReviewFunction.DTO;
using ProductAndReviewFunction.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductAndReviewFunction.Repository.Interface
{
    public interface IProductService
    {
        Task CreateProduct(CreateUpdateProductDTO productDTO);
        Task RemoveProduct(Guid productId);
        Task UpdateProduct(Guid productId, CreateUpdateProductDTO updateProductDTO);
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(Guid productId);
        Task<bool> CheckIfProductExist(Guid productId);

    }
}