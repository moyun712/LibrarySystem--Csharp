// 修改 Program.cs
using System;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("欢迎使用图书馆管理系统！");
            
            // 创建图书馆实例
            Library library = new Library();
            
            // 添加一些示例数据
            AddSampleData(library);
            
            // 显示主菜单
            bool exit = false;
            while (!exit)
            {
                DisplayMainMenu();
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        ManageItems(library);
                        break;
                    case "2":
                        ManageMembers(library);
                        break;
                    case "3":
                        ManageBorrowings(library);
                        break;
                    case "4":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("无效选择，请重试");
                        break;
                }
            }
            
            Console.WriteLine("感谢使用图书馆管理系统，再见！");
        }
        
        static void DisplayMainMenu()
        {
            Console.WriteLine("\n=== 图书馆管理系统 ===");
            Console.WriteLine("1. 图书管理");
            Console.WriteLine("2. 会员管理");
            Console.WriteLine("3. 借阅管理");
            Console.WriteLine("4. 退出");
            Console.Write("请选择操作: ");
        }
        
        static void ManageItems(Library library)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n=== 图书管理 ===");
                Console.WriteLine("1. 添加图书");
                Console.WriteLine("2. 添加杂志");
                Console.WriteLine("3. 查看所有物品");
                Console.WriteLine("4. 搜索物品");
                Console.WriteLine("5. 返回主菜单");
                Console.Write("请选择操作: ");
                
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        AddBook(library);
                        break;
                    case "2":
                        AddMagazine(library);
                        break;
                    case "3":
                        library.DisplayAllItems();
                        break;
                    case "4":
                        SearchItems(library);
                        break;
                    case "5":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("无效选择，请重试");
                        break;
                }
            }
        }
        
        static void ManageMembers(Library library)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n=== 会员管理 ===");
                Console.WriteLine("1. 添加会员");
                Console.WriteLine("2. 查看所有会员");
                Console.WriteLine("3. 搜索会员");
                Console.WriteLine("4. 返回主菜单");
                Console.Write("请选择操作: ");
                
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        AddMember(library);
                        break;
                    case "2":
                        library.DisplayAllMembers();
                        break;
                    case "3":
                        SearchMembers(library);
                        break;
                    case "4":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("无效选择，请重试");
                        break;
                }
            }
        }
        
        static void ManageBorrowings(Library library)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n=== 借阅管理 ===");
                Console.WriteLine("1. 借阅物品");
                Console.WriteLine("2. 归还物品");
                Console.WriteLine("3. 查看所有借阅记录");
                Console.WriteLine("4. 返回主菜单");
                Console.Write("请选择操作: ");
                
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        BorrowItem(library);
                        break;
                    case "2":
                        ReturnItem(library);
                        break;
                    case "3":
                        library.DisplayAllBorrowRecords();
                        break;
                    case "4":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("无效选择，请重试");
                        break;
                }
            }
        }
        
        static void AddBook(Library library)
        {
            Console.WriteLine("\n=== 添加图书 ===");
            
            Console.Write("标题: ");
            string title = Console.ReadLine();
            
            Console.Write("作者: ");
            string author = Console.ReadLine();
            
            Console.Write("出版社: ");
            string publisher = Console.ReadLine();
            
            Console.Write("出版年份: ");
            if (!int.TryParse(Console.ReadLine(), out int publicationYear))
            {
                Console.WriteLine("无效的年份，添加失败");
                return;
            }
            
            Console.Write("ISBN: ");
            string isbn = Console.ReadLine();
            
            Console.Write("可用副本数: ");
            if (!int.TryParse(Console.ReadLine(), out int availableCopies))
            {
                Console.WriteLine("无效的副本数，添加失败");
                return;
            }
            
            Console.WriteLine("选择图书分类:");
            Console.WriteLine("0 - Fiction (小说)");
            Console.WriteLine("1 - NonFiction (非小说)");
            Console.WriteLine("2 - Science (科学)");
            Console.WriteLine("3 - History (历史)");
            Console.WriteLine("4 - Technology (技术)");
            Console.WriteLine("5 - Literature (文学)");
            Console.WriteLine("6 - Other (其他)");
            Console.Write("请选择: ");
            
            if (!int.TryParse(Console.ReadLine(), out int categoryChoice) || !Enum.IsDefined(typeof(BookCategory), categoryChoice))
            {
                Console.WriteLine("无效的分类选择，添加失败");
                return;
            }
            
            BookCategory category = (BookCategory)categoryChoice;
            
            Book book = new Book(0, title, author, publisher, availableCopies, isbn, publicationYear, category);
            library.AddItem(book);
        }
        
        static void AddMagazine(Library library)
        {
            Console.WriteLine("\n=== 添加杂志 ===");
            
            Console.Write("标题: ");
            string title = Console.ReadLine();
            
            Console.Write("作者/编辑: ");
            string author = Console.ReadLine();
            
            Console.Write("出版社: ");
            string publisher = Console.ReadLine();
            
            Console.Write("出版年份: ");
            if (!int.TryParse(Console.ReadLine(), out int publicationYear))
            {
                Console.WriteLine("无效的年份，添加失败");
                return;
            }
            
            Console.Write("ISSN: ");
            string issn = Console.ReadLine();
            
            Console.Write("期号: ");
            if (!int.TryParse(Console.ReadLine(), out int issueNumber))
            {
                Console.WriteLine("无效的期号，添加失败");
                return;
            }
            
            Console.Write("发行频率 (如月刊、季刊): ");
            string frequency = Console.ReadLine();
            
            Console.Write("可用副本数: ");
            if (!int.TryParse(Console.ReadLine(), out int availableCopies))
            {
                Console.WriteLine("无效的副本数，添加失败");
                return;
            }
            
            Magazine magazine = new Magazine(0, title, author, publisher, availableCopies, issn, issueNumber, frequency, publicationYear);
            library.AddItem(magazine);
        }
        
        static void AddMember(Library library)
        {
            Console.WriteLine("\n=== 添加会员 ===");
            
            Console.Write("姓名: ");
            string name = Console.ReadLine();
            
            Console.Write("联系方式: ");
            string contactInfo = Console.ReadLine();
            
            Console.WriteLine("选择会员类型:");
            Console.WriteLine("0 - Student (学生)");
            Console.WriteLine("1 - Faculty (教职工)");
            Console.WriteLine("2 - VIP");
            Console.WriteLine("3 - Regular (普通会员)");
            Console.Write("请选择: ");
            
            if (!int.TryParse(Console.ReadLine(), out int typeChoice) || !Enum.IsDefined(typeof(MemberType), typeChoice))
            {
                Console.WriteLine("无效的会员类型选择，添加失败");
                return;
            }
            
            MemberType type = (MemberType)typeChoice;
            
            Member member = new Member(0, name, contactInfo, type);
            library.AddMember(member);
        }
        
        static void SearchItems(Library library)
        {
            Console.WriteLine("\n=== 搜索物品 ===");
            Console.Write("请输入关键词 (标题、作者或出版社): ");
            string keyword = Console.ReadLine();
            
            var results = library.SearchItems(keyword);
            
            Console.WriteLine($"\n找到 {results.Count} 个结果:");
            foreach (var item in results)
            {
                item.DisplayInfo();
                Console.WriteLine("-------------------");
            }
        }
        
        static void SearchMembers(Library library)
        {
            Console.WriteLine("\n=== 搜索会员 ===");
            Console.Write("请输入关键词 (姓名或联系方式): ");
            string keyword = Console.ReadLine();
            
            var results = library.SearchMembers(keyword);
            
            Console.WriteLine($"\n找到 {results.Count} 个结果:");
            foreach (var member in results)
            {
                member.DisplayInfo();
                Console.WriteLine("-------------------");
            }
        }
        
        static void BorrowItem(Library library)
        {
            Console.WriteLine("\n=== 借阅物品 ===");
            
            Console.Write("会员ID: ");
            if (!int.TryParse(Console.ReadLine(), out int memberId))
            {
                Console.WriteLine("无效的会员ID");
                return;
            }
            
            Console.Write("物品ID: ");
            if (!int.TryParse(Console.ReadLine(), out int itemId))
            {
                Console.WriteLine("无效的物品ID");
                return;
            }
            
            library.BorrowItem(memberId, itemId);
        }
        
        static void ReturnItem(Library library)
        {
            Console.WriteLine("\n=== 归还物品 ===");
            
            Console.Write("会员ID: ");
            if (!int.TryParse(Console.ReadLine(), out int memberId))
            {
                Console.WriteLine("无效的会员ID");
                return;
            }
            
            Console.Write("物品ID: ");
            if (!int.TryParse(Console.ReadLine(), out int itemId))
            {
                Console.WriteLine("无效的物品ID");
                return;
            }
            
            library.ReturnItem(memberId, itemId);
        }
        
        static void AddSampleData(Library library)
        {
            // 添加示例图书
            Book book1 = new Book(0, "C#编程入门", "张三", "编程出版社", 5, "978-1-234567-89-0", 2022, BookCategory.Technology);
            Book book2 = new Book(0, "数据结构与算法", "李四", "计算机出版社", 3, "978-2-345678-90-1", 2021, BookCategory.Science);
            Book book3 = new Book(0, "人工智能导论", "王五", "科学出版社", 2, "978-3-456789-01-2", 2023, BookCategory.Technology);
            
            library.AddItem(book1);
            library.AddItem(book2);
            library.AddItem(book3);
            
            // 添加示例杂志
            Magazine magazine1 = new Magazine(0, "科学世界", "科学编辑部", "科学出版社", 10, "1234-5678", 12, "月刊", 2023);
            Magazine magazine2 = new Magazine(0, "编程技术", "技术编辑部", "计算机出版社", 8, "2345-6789", 4, "季刊", 2023);
            
            library.AddItem(magazine1);
            library.AddItem(magazine2);
            
            // 添加示例会员
            Member member1 = new Member(0, "学生张三", "student@example.com", MemberType.Student);
            Member member2 = new Member(0, "教师李四", "faculty@example.com", MemberType.Faculty);
            Member member3 = new Member(0, "VIP王五", "vip@example.com", MemberType.VIP);
            
            library.AddMember(member1);
            library.AddMember(member2);
            library.AddMember(member3);
            
            Console.WriteLine("已添加示例数据");
        }
    }
}