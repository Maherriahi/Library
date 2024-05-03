using StudentClub.Models;
using StudentClub.UnitOfWork.Repository;

namespace StudentClub.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        IRepository<Student> students { get; }
        IRepository<Section> sections { get; }
        IRepository<Book> books { get; }
        IRepository<Message> messages { get; }
        IRepository<Image> images { get; }
        IRepository<Replay> replayes { get; }

        int ComitChanges();
    }
}
