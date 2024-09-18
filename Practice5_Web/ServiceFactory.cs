using Practice5_DataAccess.Service;
using Practice5_DataAccess.Service.ADO;
using Practice5_DataAccess.Service.EF;

namespace Practice5_Web
{
    public class ServiceFactory(IServiceProvider serviceProvider)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public IProductService CreateService(HttpContext httpContext)
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
    }
}
