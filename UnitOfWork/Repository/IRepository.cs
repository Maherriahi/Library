namespace StudentClub.UnitOfWork.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> FindById(int id);

        Task<IEnumerable<T>> FindAllAsync();

        Task<IEnumerable<T>> FindAllAsync(params string[] agers);

        //-----------methode normal
        void AddOne(T MyItem);

        void UpdateOne(T MyItem);
        void DeleteOne(T MyItem);

    }
}
