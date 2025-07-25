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
                Console.WriteLine("5. 删除物品");
                Console.WriteLine("6. 更新图书信息");
                Console.WriteLine("7. 更新杂志信息");
                Console.WriteLine("8. 返回主菜单");
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
                        RemoveItem(library);
                        break;
                    case "6":
                        UpdateBookInfo(library);
                        break;
                    case "7":
                        UpdateMagazineInfo(library);
                        break;
                    case "8":
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
                Console.WriteLine("4. 删除会员");
                Console.WriteLine("5. 更新会员信息");
                Console.WriteLine("6. 返回主菜单");
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
                        RemoveMember(library);
                        break;
                    case "5":
                        UpdateMemberInfo(library);
                        break;
                    case "6":
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
                Console.WriteLine("4. 查看会员借阅历史");
                Console.WriteLine("5. 高级借阅历史查询");
                Console.WriteLine("6. 预约物品");
                Console.WriteLine("7. 取消预约");
                Console.WriteLine("8. 查看会员预约记录");
                Console.WriteLine("9. 查看所有预约记录");
                Console.WriteLine("10. 返回主菜单");
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
                        ViewMemberBorrowHistory(library);
                        break;
                    case "5":
                        AdvancedBorrowHistorySearch(library);
                        break;
                    case "6":
                        ReserveItem(library);
                        break;
                    case "7":
                        CancelReservation(library);
                        break;
                    case "8":
                        ViewMemberReservations(library);
                        break;
                    case "9":
                        library.DisplayAllReservations();
                        break;
                    case "10":
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
        
        static void RemoveItem(Library library)
        {
            Console.WriteLine("\n=== 删除物品 ===");
            Console.Write("请输入要删除的物品ID: ");
            
            if (!int.TryParse(Console.ReadLine(), out int itemId))
            {
                Console.WriteLine("无效的物品ID，删除失败");
                return;
            }
            
            library.RemoveItem(itemId);
        }
        
        static void UpdateBookInfo(Library library)
        {
            Console.WriteLine("\n=== 更新图书信息 ===");
            Console.Write("请输入要更新的图书ID: ");
            
            if (!int.TryParse(Console.ReadLine(), out int bookId))
            {
                Console.WriteLine("无效的图书ID，更新失败");
                return;
            }
            
            Console.WriteLine("请输入新的信息（如果不需要更新某项，请直接按回车跳过）：");
            
            Console.Write("标题: ");
            string title = Console.ReadLine();
            title = string.IsNullOrWhiteSpace(title) ? null : title;
            
            Console.Write("作者: ");
            string author = Console.ReadLine();
            author = string.IsNullOrWhiteSpace(author) ? null : author;
            
            Console.Write("出版社: ");
            string publisher = Console.ReadLine();
            publisher = string.IsNullOrWhiteSpace(publisher) ? null : publisher;
            
            Console.Write("可用副本数: ");
            string copiesStr = Console.ReadLine();
            int? availableCopies = string.IsNullOrWhiteSpace(copiesStr) ? null : 
                int.TryParse(copiesStr, out int copies) ? copies : (int?)null;
            
            Console.Write("ISBN: ");
            string isbn = Console.ReadLine();
            isbn = string.IsNullOrWhiteSpace(isbn) ? null : isbn;
            
            Console.Write("出版年份: ");
            string yearStr = Console.ReadLine();
            int? publicationYear = string.IsNullOrWhiteSpace(yearStr) ? null : 
                int.TryParse(yearStr, out int year) ? year : (int?)null;
            
            Console.WriteLine("选择图书分类（直接按回车跳过）:");
            Console.WriteLine("0 - Fiction (小说)");
            Console.WriteLine("1 - NonFiction (非小说)");
            Console.WriteLine("2 - Science (科学)");
            Console.WriteLine("3 - History (历史)");
            Console.WriteLine("4 - Technology (技术)");
            Console.WriteLine("5 - Literature (文学)");
            Console.WriteLine("6 - Other (其他)");
            Console.Write("请选择: ");
            
            string categoryStr = Console.ReadLine();
            BookCategory? category = string.IsNullOrWhiteSpace(categoryStr) ? null : 
                int.TryParse(categoryStr, out int categoryVal) && Enum.IsDefined(typeof(BookCategory), categoryVal) ? 
                (BookCategory?)categoryVal : null;
            
            library.UpdateBookInfo(bookId, title, author, publisher, availableCopies, isbn, publicationYear, category);
        }
        
        static void UpdateMagazineInfo(Library library)
        {
            Console.WriteLine("\n=== 更新杂志信息 ===");
            Console.Write("请输入要更新的杂志ID: ");
            
            if (!int.TryParse(Console.ReadLine(), out int magazineId))
            {
                Console.WriteLine("无效的杂志ID，更新失败");
                return;
            }
            
            Console.WriteLine("请输入新的信息（如果不需要更新某项，请直接按回车跳过）：");
            
            Console.Write("标题: ");
            string title = Console.ReadLine();
            title = string.IsNullOrWhiteSpace(title) ? null : title;
            
            Console.Write("作者/编辑: ");
            string author = Console.ReadLine();
            author = string.IsNullOrWhiteSpace(author) ? null : author;
            
            Console.Write("出版社: ");
            string publisher = Console.ReadLine();
            publisher = string.IsNullOrWhiteSpace(publisher) ? null : publisher;
            
            Console.Write("可用副本数: ");
            string copiesStr = Console.ReadLine();
            int? availableCopies = string.IsNullOrWhiteSpace(copiesStr) ? null : 
                int.TryParse(copiesStr, out int copies) ? copies : (int?)null;
            
            Console.Write("ISSN: ");
            string issn = Console.ReadLine();
            issn = string.IsNullOrWhiteSpace(issn) ? null : issn;
            
            Console.Write("期号: ");
            string issueStr = Console.ReadLine();
            int? issueNumber = string.IsNullOrWhiteSpace(issueStr) ? null : 
                int.TryParse(issueStr, out int issue) ? issue : (int?)null;
            
            Console.Write("发行频率 (如月刊、季刊): ");
            string frequency = Console.ReadLine();
            frequency = string.IsNullOrWhiteSpace(frequency) ? null : frequency;
            
            Console.Write("出版年份: ");
            string yearStr = Console.ReadLine();
            int? publicationYear = string.IsNullOrWhiteSpace(yearStr) ? null : 
                int.TryParse(yearStr, out int year) ? year : (int?)null;
            
            library.UpdateMagazineInfo(magazineId, title, author, publisher, availableCopies, issn, issueNumber, frequency, publicationYear);
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
        
        static void RemoveMember(Library library)
        {
            Console.WriteLine("\n=== 删除会员 ===");
            Console.Write("请输入要删除的会员ID: ");
            
            if (!int.TryParse(Console.ReadLine(), out int memberId))
            {
                Console.WriteLine("无效的会员ID，删除失败");
                return;
            }
            
            library.RemoveMember(memberId);
        }
        
        static void UpdateMemberInfo(Library library)
        {
            Console.WriteLine("\n=== 更新会员信息 ===");
            Console.Write("请输入要更新的会员ID: ");
            
            if (!int.TryParse(Console.ReadLine(), out int memberId))
            {
                Console.WriteLine("无效的会员ID，更新失败");
                return;
            }
            
            Console.WriteLine("请输入新的信息（如果不需要更新某项，请直接按回车跳过）：");
            
            Console.Write("姓名: ");
            string name = Console.ReadLine();
            name = string.IsNullOrWhiteSpace(name) ? null : name;
            
            Console.Write("联系方式: ");
            string contactInfo = Console.ReadLine();
            contactInfo = string.IsNullOrWhiteSpace(contactInfo) ? null : contactInfo;
            
            Console.WriteLine("选择会员类型（直接按回车跳过）:");
            Console.WriteLine("0 - Student (学生)");
            Console.WriteLine("1 - Faculty (教职工)");
            Console.WriteLine("2 - VIP");
            Console.WriteLine("3 - Regular (普通会员)");
            Console.Write("请选择: ");
            
            string typeStr = Console.ReadLine();
            MemberType? type = string.IsNullOrWhiteSpace(typeStr) ? null : 
                int.TryParse(typeStr, out int typeVal) && Enum.IsDefined(typeof(MemberType), typeVal) ? 
                (MemberType?)typeVal : null;
            
            library.UpdateMemberInfo(memberId, name, contactInfo, type);
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
        
        static void ViewMemberBorrowHistory(Library library)
        {
            Console.WriteLine("\n=== 查看会员借阅历史 ===");
            
            Console.Write("请输入会员ID: ");
            if (!int.TryParse(Console.ReadLine(), out int memberId))
            {
                Console.WriteLine("无效的会员ID");
                return;
            }
            
            Console.WriteLine("是否包含已归还的记录？ (Y/N，默认Y): ");
            string includeReturnedStr = Console.ReadLine()?.Trim().ToUpper() ?? "Y";
            bool includeReturned = includeReturnedStr != "N";
            
            library.DisplayMemberBorrowRecords(memberId, includeReturned);
        }
        
        static void AdvancedBorrowHistorySearch(Library library)
        {
            Console.WriteLine("\n=== 高级借阅历史查询 ===");
            
            // 会员ID（可选）
            Console.Write("会员ID (直接按回车跳过): ");
            string memberIdStr = Console.ReadLine();
            int? memberId = string.IsNullOrWhiteSpace(memberIdStr) ? null : 
                int.TryParse(memberIdStr, out int mId) ? mId : (int?)null;
            
            // 物品ID（可选）
            Console.Write("物品ID (直接按回车跳过): ");
            string itemIdStr = Console.ReadLine();
            int? itemId = string.IsNullOrWhiteSpace(itemIdStr) ? null : 
                int.TryParse(itemIdStr, out int iId) ? iId : (int?)null;
            
            // 开始日期（可选）
            Console.Write("开始日期 (格式: yyyy-MM-dd, 直接按回车跳过): ");
            string startDateStr = Console.ReadLine();
            DateTime? startDate = string.IsNullOrWhiteSpace(startDateStr) ? null : 
                DateTime.TryParse(startDateStr, out DateTime sDate) ? sDate : (DateTime?)null;
            
            // 结束日期（可选）
            Console.Write("结束日期 (格式: yyyy-MM-dd, 直接按回车跳过): ");
            string endDateStr = Console.ReadLine();
            DateTime? endDate = string.IsNullOrWhiteSpace(endDateStr) ? null : 
                DateTime.TryParse(endDateStr, out DateTime eDate) ? eDate : (DateTime?)null;
            
            // 借阅状态（可选）
            Console.WriteLine("借阅状态 (直接按回车跳过):");
            Console.WriteLine("0 - Borrowed (已借出)");
            Console.WriteLine("1 - Returned (已归还)");
            Console.WriteLine("2 - Overdue (逾期)");
            Console.WriteLine("3 - Reserved (已预约)");
            Console.Write("请选择: ");
            
            string statusStr = Console.ReadLine();
            BorrowStatus? status = string.IsNullOrWhiteSpace(statusStr) ? null : 
                int.TryParse(statusStr, out int statusVal) && Enum.IsDefined(typeof(BorrowStatus), statusVal) ? 
                (BorrowStatus?)statusVal : null;
            
            // 排序方式
            Console.Write("按借阅日期降序排序？ (Y/N，默认Y): ");
            string sortDesc = Console.ReadLine()?.Trim().ToUpper() ?? "Y";
            bool sortByDateDesc = sortDesc != "N";
            
            // 执行查询
            var results = library.SearchBorrowRecords(memberId, itemId, startDate, endDate, status, sortByDateDesc);
            
            // 显示结果
            library.DisplayBorrowRecordSearchResults(results);
        }
        
        static void ReserveItem(Library library)
        {
            Console.WriteLine("\n=== 预约物品 ===");
            
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
            
            library.ReserveItem(memberId, itemId);
        }
        
        static void CancelReservation(Library library)
        {
            Console.WriteLine("\n=== 取消预约 ===");
            
            Console.Write("会员ID: ");
            if (!int.TryParse(Console.ReadLine(), out int memberId))
            {
                Console.WriteLine("无效的会员ID");
                return;
            }
            
            Console.Write("预约记录ID: ");
            if (!int.TryParse(Console.ReadLine(), out int reservationId))
            {
                Console.WriteLine("无效的预约记录ID");
                return;
            }
            
            library.CancelReservation(memberId, reservationId);
        }
        
        static void ViewMemberReservations(Library library)
        {
            Console.WriteLine("\n=== 查看会员预约记录 ===");
            
            Console.Write("请输入会员ID: ");
            if (!int.TryParse(Console.ReadLine(), out int memberId))
            {
                Console.WriteLine("无效的会员ID");
                return;
            }
            
            library.DisplayMemberReservations(memberId);
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
            
            // 添加示例预约记录
            // 注意：只有当物品没有可用副本时才能预约，所以我们先借出所有副本
            if (book3.AvailableCopies > 0)
            {
                library.BorrowItem(member1.Id, book3.Id);
                if (book3.AvailableCopies > 0)
                {
                    library.BorrowItem(member2.Id, book3.Id);
                }
            }
            
            // 现在预约这本书
            library.ReserveItem(member3.Id, book3.Id);
            
            Console.WriteLine("已添加示例数据");
        }
    }
}