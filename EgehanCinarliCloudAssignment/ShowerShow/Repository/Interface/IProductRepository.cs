using UserAndOrdersFunction.DTO;
using UserAndOrdersFunction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAndOrdersFunction.Repository.Interface
{
    public interface IProductRepository
    {
        Task CreateProduct(CreateUpdateProductDTO productDTO);
        Task RemoveProduct(Guid productId);
        Task<bool> CheckIfProductExist(Guid productId);
        Task<IEnumerable<Product>> GetAllProducts();
        Task UpdateProduct(Guid productId, CreateUpdateProductDTO updateProductDTO);
        Task<Product> GetProductById(Guid productId);

    }
}
