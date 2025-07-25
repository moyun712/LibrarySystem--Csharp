using System;

namespace LibraryManagementSystem.Models
{
    // Book 类继承自 LibraryItem
    public class Book : LibraryItem
    {
        // Book 特有的属性
        public string ISBN { get; set; }
        public int PublicationYear { get; set; } // 添加为 Book 特有的属性
        public BookCategory Category { get; set; }

        // 构造函数
        public Book(int id, string title, string author, string publisher, int availableCopies, 
                   string isbn, int publicationYear, BookCategory category) 
            : base(id, title, author, publisher, availableCopies)
        {
            ISBN = isbn;
            PublicationYear = publicationYear;
            Category = category;
        }

        // 重写基类的抽象方法
        public override void DisplayInfo()
        {
            Console.WriteLine($"书籍 ID: {Id}");
            Console.WriteLine($"标题: {Title}");
            Console.WriteLine($"作者: {Author}");
            Console.WriteLine($"出版社: {Publisher}");
            Console.WriteLine($"可用副本数: {AvailableCopies}");
            Console.WriteLine($"ISBN: {ISBN}");
            Console.WriteLine($"出版年份: {PublicationYear}");
            Console.WriteLine($"分类: {Category}");
        }
    }
}