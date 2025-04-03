using Microsoft.Data.SqlClient;

namespace ReaderInserterIMDB
{
    public interface IInserter<T>
    {
        void Insert(T item);
    }
}