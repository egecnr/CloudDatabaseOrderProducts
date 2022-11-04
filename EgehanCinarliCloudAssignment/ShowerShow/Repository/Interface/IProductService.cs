using UserAndOrdersFunction.DTO;
using UserAndOrdersFunction.Model;
using System;
using System.Threading.Tasks;

namespace UserAndOrdersFunction.Repository.Interface
{
    public interface IProductService
    {
        public Task CreateProduct(ProductDTO productDTO);
        public Task RemoveProduct(Guid productId);
        public Task UpdateProduct(Guid productId, ProductDTO updateProductDTO);
        public Task<Product> GetProductById(Guid productId);

    }
}