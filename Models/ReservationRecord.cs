using System;
using LibraryManagementSystem.Repositories;

namespace LibraryManagementSystem.Models
{
    // 预约状态枚举
    public enum ReservationStatus
    {
        Waiting,    // 等待中
        Fulfilled,  // 已兑现（可借阅）
        Cancelled,  // 已取消
        Expired     // 已过期
    }

    // 预约记录类
    public class ReservationRecord : IEntity
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int ItemId { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ExpiryDate { get; set; }  // 预约有效期
        public ReservationStatus Status { get; set; }
        public DateTime? NotificationDate { get; set; }  // 通知会员可借阅的日期

        public ReservationRecord(int id, int memberId, int itemId)
        {
            Id = id;
            MemberId = memberId;
            ItemId = itemId;
            ReservationDate = DateTime.Now;
            // 预约默认有效期为30天
            ExpiryDate = DateTime.Now.AddDays(30);
            Status = ReservationStatus.Waiting;
        }

        // 标记预约为已兑现状态
        public void MarkAsFulfilled()
        {
            Status = ReservationStatus.Fulfilled;
            NotificationDate = DateTime.Now;
        }

        // 取消预约
        public void Cancel()
        {
            Status = ReservationStatus.Cancelled;
        }

        // 检查预约是否已过期
        public bool IsExpired()
        {
            if (Status == ReservationStatus.Waiting && DateTime.Now > ExpiryDate)
            {
                Status = ReservationStatus.Expired;
                return true;
            }
            return Status == ReservationStatus.Expired;
        }

        // 显示预约信息
        public void DisplayInfo()
        {
            Console.WriteLine($"预约记录 ID: {Id}");
            Console.WriteLine($"会员 ID: {MemberId}");
            Console.WriteLine($"物品 ID: {ItemId}");
            Console.WriteLine($"预约日期: {ReservationDate:yyyy-MM-dd}");
            Console.WriteLine($"有效期至: {ExpiryDate:yyyy-MM-dd}");
            Console.WriteLine($"状态: {Status}");
            
            if (NotificationDate.HasValue)
            {
                Console.WriteLine($"通知日期: {NotificationDate.Value:yyyy-MM-dd}");
            }
        }
    }
} 