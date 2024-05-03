
using Microsoft.EntityFrameworkCore;
using StudentClub.DataBase;

namespace StudentClub.UnitOfWork.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public Repository(AppDbContext db)
        {
            _db = db;
        }
        private readonly AppDbContext _db;


        public void AddOne(T MyItem)
        {
            _db.Set<T>().Add(MyItem);
            _db.SaveChanges();
        }


        public void UpdateOne(T MyItem)
        {
            _db.Set<T>().Update(MyItem);
            _db.SaveChanges();
        }

        public void DeleteOne(T MyItem)
        {
            _db.Set<T>().Remove(MyItem);
            _db.SaveChanges();
        }
        public async Task<T> FindById(int id)
        {
            return await  _db.Set<T>().FindAsync(id);
        }
        public async Task<IEnumerable<T>> FindAllAsync()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(params string[] agers)
        {
            IQueryable<T> query = _db.Set<T>();

            if (agers.Length > 0)
            {
                foreach (var ager in agers)
                    query = query.Include(ager);
            }
            return await query.ToListAsync();
        }

    }
}
