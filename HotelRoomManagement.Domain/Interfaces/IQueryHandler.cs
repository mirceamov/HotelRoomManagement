namespace HotelRoomManagement.Domain.Interfaces
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TQuery, TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
