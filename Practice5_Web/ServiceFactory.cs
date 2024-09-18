using Practice5_DataAccess.Service;
using Practice5_DataAccess.Service.ADO;
using Practice5_DataAccess.Service.EF;

namespace Practice5_Web
{
    public class ServiceFactory(IServiceProvider serviceProvider)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public IProductService CreateProductService(HttpContext httpContext)
        {
            var preference = httpContext.Session.GetString("DataAccess");
            if (preference == "ADO")
            {
                return _serviceProvider.GetService<ProductServiceADO>()
                    ?? throw new InvalidOperationException("Product Service is not registered");
            }
            else
            {
                return _serviceProvider.GetService<ProductServiceEF>()
                    ?? throw new InvalidOperationException("Product Service is not registered");
            }
        }

        public IInventoryService CreateInventoryService(HttpContext httpContext)
        {
            var preference = httpContext.Session.GetString("DataAccess");
            if (preference == "ADO")
            {
                return _serviceProvider.GetService<InventoryServiceADO>()
                    ?? throw new InvalidOperationException("Inventory Service is not registered");
            }
            else
            {
                return _serviceProvider.GetService<InventoryServiceEF>()
                    ?? throw new InvalidOperationException("Inventory Service is not registered");
            }
        }

        public IPurchaseService CreatePurchaseService(HttpContext httpContext)
        {
            var preference = httpContext.Session.GetString("DataAccess");
            if (preference == "ADO")
            {
                return _serviceProvider.GetService<PurchaseServiceADO>()
                    ?? throw new InvalidOperationException("Purchase Service is not registered");
            }
            else
            {
                return _serviceProvider.GetService<PurchaseServiceEF>()
                    ?? throw new InvalidOperationException("Purchase Service is not registered");
            }
        }
    }

}
