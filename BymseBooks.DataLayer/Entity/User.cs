using System.ComponentModel.DataAnnotations;

namespace BymseBooks.DataLayer.Entity
{
    public class User
    {
        public User()
        {
            Books = new HashSet<Book>();
        }

        public int UserId { get; set; }
        public string Name { get; set; }
        
        public DateTime CreatedDate { get; set; }

        public ICollection<Book> Books { get; private set; }
    }
}