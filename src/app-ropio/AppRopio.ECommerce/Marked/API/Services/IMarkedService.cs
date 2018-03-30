using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Marked.Responses;

namespace AppRopio.ECommerce.Marked.API.Services
{
    public interface IMarkedService
    {
	    Task<List<MarkedProduct>> GetMarkedProducts(int count, int offset = 0);

        Task<int> GetQuantity();
    }
}