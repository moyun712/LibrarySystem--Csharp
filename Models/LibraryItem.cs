
using LibraryManagementSystem.Repositories;

namespace  LibraryManagementSystem.Models
{
    public abstract class LibraryItem : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public int AvailableCopies { get; set; }
        
        public LibraryItem(int id, string title, string author, string publisher, int availableCopies)
        {
            Id = id;
            Title = title;
            Author = author;
            Publisher = publisher;
            AvailableCopies = availableCopies;
        }

        public abstract void DisplayInfo();
    }
}