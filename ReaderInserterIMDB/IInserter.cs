using Microsoft.Data.SqlClient;

namespace ReaderInserterIMDB
{
    public interface IInserter
    {
        void InsertTitle(Title newTitle);
        void InsertPerson(Person newPerson);
    }
}