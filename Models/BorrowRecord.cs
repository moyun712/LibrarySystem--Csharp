// 创建文件 Models/BorrowRecord.cs
using System;
using LibraryManagementSystem.Repositories;

namespace LibraryManagementSystem.Models
{
    public class BorrowRecord : IEntity
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int ItemId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; } // 可为空，表示尚未归还
        public BorrowStatus Status { get; set; }
        public decimal FineAmount { get; set; } // 逾期归还图书的罚款金额

        public BorrowRecord(int id, int memberId, int itemId)
        {
            Id = id;
            MemberId = memberId;
            ItemId = itemId;
            BorrowDate = DateTime.Now;
            DueDate = DateTime.Now.AddDays(14); // 默认借阅期限14天
            Status = BorrowStatus.Borrowed;
            FineAmount = 0;
        }

        public void ReturnItem()
        {
            ReturnDate = DateTime.Now;
            
            if (DateTime.Now > DueDate)
            {
                Status = BorrowStatus.Overdue;
                // 计算罚款：每天1元
                int daysOverdue = (int)(DateTime.Now - DueDate).TotalDays;
                FineAmount = daysOverdue * 1.0m; // 1.0m表示1元（decimal类型）
            }
            else
            {
                Status = BorrowStatus.Returned;
            }
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"借阅记录 ID: {Id}");
            Console.WriteLine($"会员 ID: {MemberId}");
            Console.WriteLine($"物品 ID: {ItemId}");
            Console.WriteLine($"借阅日期: {BorrowDate:yyyy-MM-dd}");
            Console.WriteLine($"应还日期: {DueDate:yyyy-MM-dd}");
            
            if (ReturnDate.HasValue)
            {
                Console.WriteLine($"归还日期: {ReturnDate.Value:yyyy-MM-dd}");
            }
            else
            {
                Console.WriteLine("归还日期: 尚未归还");
            }
            
            Console.WriteLine($"状态: {Status}");
            
            if (FineAmount > 0)
            {
                Console.WriteLine($"罚款金额: {FineAmount:C}");
            }
        }
    }
}