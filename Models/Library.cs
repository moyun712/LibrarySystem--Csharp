// 创建文件 Library.cs
using System;
using System.Collections.Generic;
using System.Linq;
using LibraryManagementSystem.Exceptions;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;

namespace LibraryManagementSystem
{
    public class Library
    {
        private readonly IRepository<LibraryItem> _itemRepository;
        private readonly IRepository<Member> _memberRepository;
        private readonly IRepository<BorrowRecord> _borrowRecordRepository;
        
        public Library()
        {
            _itemRepository = new Repository<LibraryItem>();
            _memberRepository = new Repository<Member>();
            _borrowRecordRepository = new Repository<BorrowRecord>();
        }
        
        // 添加图书馆物品
        public void AddItem(LibraryItem item)
        {
            _itemRepository.Add(item);
            Console.WriteLine($"已添加新物品，ID: {item.Id}");
        }
        
        // 添加会员
        public void AddMember(Member member)
        {
            _memberRepository.Add(member);
            Console.WriteLine($"已添加新会员，ID: {member.Id}");
        }
        
        // 借阅物品
        // public bool BorrowItem(int memberId, int itemId)
        // {
        //     // 检查会员是否存在
        //     var member = _memberRepository.GetById(memberId);
        //     if (member == null)
        //     {
        //         Console.WriteLine("会员ID不存在");
        //         return false;
        //     }

        //     // 检查物品是否存在
        //     var item = _itemRepository.GetById(itemId);
        //     if (item == null)
        //     {
        //         Console.WriteLine("物品ID不存在");
        //         return false;
        //     }

        //     // 检查物品是否有可用副本
        //     if (item.AvailableCopies <= 0)
        //     {
        //         Console.WriteLine("该物品没有可用副本");
        //         return false;
        //     }

        //     // 检查会员是否超出借阅限制
        //     if (member.BorrowedItems.Count >= member.MaxBooksAllowed)
        //     {
        //         Console.WriteLine("会员已达到最大借阅数量");
        //         return false;
        //     }

        //     // 创建借阅记录
        //     var record = new BorrowRecord(0, memberId, itemId); // ID将由仓储分配
        //     _borrowRecordRepository.Add(record);

        //     // 更新物品和会员信息
        //     item.AvailableCopies--;
        //     member.BorrowedItems.Add(itemId);

        //     Console.WriteLine($"借阅成功，记录ID: {record.Id}");
        //     return true;
        // }
        public void BorrowItem(int memberId, int itemId)
        {
            try
            {
                // 检查会员是否存在
                var member = _memberRepository.GetById(memberId);
                if (member is null)
                    throw new MemberNotFoundException(memberId);

                // 检查物品是否存在
                var item = _itemRepository.GetById(itemId);
                if (item is null)
                    throw new ItemNotFoundException(itemId);

                // 检查物品是否有可用副本
                if (item.AvailableCopies <= 0)
                    throw new NoAvailableCopiesException(itemId);

                // 检查会员是否超出借阅限制
                if (member.BorrowedItems.Count >= member.MaxBooksAllowed)
                    throw new BorrowLimitExceededException(memberId);

                // 创建借阅记录
                var record = new BorrowRecord(0, memberId, itemId);
                _borrowRecordRepository.Add(record);

                // 更新物品和会员信息
                item.AvailableCopies--;
                member.BorrowedItems.Add(itemId);

                Console.WriteLine($"借阅成功，记录ID: {record.Id}");
            }
            catch (LibraryException ex)
            {
                Console.WriteLine($"借阅失败: {ex.Message}");
                // 可以在这里记录异常日志
                throw; // 或者根据需要处理异常
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生未知错误: {ex.Message}");
                // 记录异常日志
                throw;
            }
        }
        // 归还物品
        public bool ReturnItem(int memberId, int itemId)
        {
            try
            {
                // 检查会员是否存在
                var member = _memberRepository.GetById(memberId) ?? throw new MemberNotFoundException(memberId);
                
                // 检查物品是否存在
                var item = _itemRepository.GetById(itemId) ?? throw new ItemNotFoundException(itemId);

                // 查找对应的借阅记录
                var record = _borrowRecordRepository.Find(r => 
                    r.MemberId == memberId && 
                    r.ItemId == itemId && 
                    r.Status == BorrowStatus.Borrowed).FirstOrDefault() ?? throw new BorrowRecordNotFoundException();
                
                // 处理归还
                record.ReturnItem();
                item.AvailableCopies++;
                member.BorrowedItems.Remove(itemId);

                Console.WriteLine($"归还成功，状态: {record.Status}");
                if (record.FineAmount > 0)
                {
                    Console.WriteLine($"逾期罚款: {record.FineAmount:C}");
                }
                
                return true;
            }
            catch (LibraryException ex)
            {
                Console.WriteLine($"归还失败: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生未知错误: {ex.Message}");
                return false;
            }
        }

        // 查找图书馆物品
        public List<LibraryItem> SearchItems(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return _itemRepository.GetAll().ToList();

            return _itemRepository.Find(item => 
                item.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                item.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                item.Publisher.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // 查找会员
        public List<Member> SearchMembers(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return _memberRepository.GetAll().ToList();

            return _memberRepository.Find(member => 
                member.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                member.ContactInfo.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // 显示所有图书馆物品
        public void DisplayAllItems()
        {
            Console.WriteLine("\n=== 图书馆物品列表 ===");
            var items = _itemRepository.GetAll().ToList();
            if (items.Count is 0)
            {
                Console.WriteLine("没有物品");
                return;
            }

            foreach (var item in items)
            {
                item.DisplayInfo();
                Console.WriteLine("-------------------");
            }
        }

        // 显示所有会员
        public void DisplayAllMembers()
        {
            Console.WriteLine("\n=== 会员列表 ===");
            var members = _memberRepository.GetAll().ToList();
            if (members.Count is 0)
            {
                Console.WriteLine("没有会员");
                return;
            }

            foreach (var member in members)
            {
                member.DisplayInfo();
                Console.WriteLine("-------------------");
            }
        }

        // 显示所有借阅记录
        public void DisplayAllBorrowRecords()
        {
            Console.WriteLine("\n=== 借阅记录列表 ===");
            var borrowRecords = _borrowRecordRepository.GetAll().ToList();

            if (borrowRecords.Count is 0)
            {
                Console.WriteLine("没有借阅记录");
                return;
            }

            foreach (var record in borrowRecords)
            {
                record.DisplayInfo();
                Console.WriteLine("-------------------");
            }
        }

        // 使用LINQ进行更复杂的查询示例
        public IEnumerable<BorrowRecord> GetOverdueRecords()
        {
            return _borrowRecordRepository.Find(r => 
                r.Status == BorrowStatus.Borrowed && DateTime.Now > r.DueDate)
                .OrderBy(r => r.DueDate);
        }
        
        // 使用LINQ进行分组和聚合操作示例
        public Dictionary<BookCategory, int> GetBookCountByCategory()
        {
            return _itemRepository.GetAll()
                .OfType<Book>() // 筛选出Book类型的项目
                .GroupBy(b => b.Category)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(b => b.AvailableCopies)
                );
        }
    }
}