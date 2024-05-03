using StudentClub.DataBase;
using StudentClub.Models;
using StudentClub.UnitOfWork.Repository;
using System.Threading.Tasks.Dataflow;

namespace StudentClub.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            students = new Repository<Student>(_db);
            sections = new Repository<Section>(_db);
            books = new Repository<Book>(_db);
            messages=new Repository<Message>(_db);
            images= new Repository<Image>(_db);
            replayes= new Repository<Replay>(_db);
        }
        private readonly AppDbContext _db;

        public IRepository<Student> students { get; private set; }

        public IRepository<Section> sections { get; private set; }
        public IRepository<Book> books { get; private set; }
        public IRepository<Message> messages { get; private set; }
        public IRepository<Image> images { get; private set; }
        public IRepository<Replay> replayes { get; private set; }

        public int ComitChanges()
        {
            return _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
