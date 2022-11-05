using UserAndOrdersFunction.DTO;
using UserAndOrdersFunction.Model;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UserAndOrdersFunction.Repository.Interface
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