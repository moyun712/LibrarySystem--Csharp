// 创建一个新文件 Magazine.cs
using System;

namespace LibraryManagementSystem.Models
{
    // Magazine 类继承自 LibraryItem
    public class Magazine : LibraryItem
    {
        // Magazine 特有的属性
        public string ISSN { get; set; }
        public int IssueNumber { get; set; }
        public int PublicationYear { get; set; }
        public string Frequency { get; set; } // 如月刊、季刊等

        // 构造函数
        public Magazine(int id, string title, string author, string publisher, int availableCopies,
                      string issn, int issueNumber, string frequency, int publicationYear)
            : base(id, title, author, publisher, availableCopies)
        {
            ISSN = issn;
            IssueNumber = issueNumber;
            Frequency = frequency;
            PublicationYear = publicationYear;
        }

        // 重写基类的抽象方法
        public override void DisplayInfo()
        {
            Console.WriteLine($"杂志 ID: {Id}");
            Console.WriteLine($"标题: {Title}");
            Console.WriteLine($"作者/编辑: {Author}");
            Console.WriteLine($"出版年份: {PublicationYear}");
            Console.WriteLine($"可用副本数: {AvailableCopies}");
            Console.WriteLine($"ISSN: {ISSN}");
            Console.WriteLine($"期号: {IssueNumber}");
            Console.WriteLine($"发行频率: {Frequency}");
        }
    }
}