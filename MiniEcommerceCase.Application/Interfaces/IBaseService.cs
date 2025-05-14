namespace MiniEcommerceCase.Application.Interfaces
{
    public interface IBaseService<TRequest, TResponse>
    {
        Task<TResponse> CreateAsync(TRequest request);
        Task<List<TResponse>> GetAllAsync(); 
    }
}
