// 创建文件 Exceptions/LibraryException.cs
using System;

namespace LibraryManagementSystem.Exceptions
{
    // 基础异常类
    public class LibraryException : Exception
    {
        public LibraryException(string message) : base(message) { }
        public LibraryException(string message, Exception innerException) : base(message, innerException) { }
    }
    
    // 特定异常类
    public class ItemNotFoundException : LibraryException
    {
        public int ItemId { get; }
        
        public ItemNotFoundException(int itemId) 
            : base($"物品ID {itemId} 不存在") 
        {
            ItemId = itemId;
        }
    }
    
    public class MemberNotFoundException : LibraryException
    {
        public int MemberId { get; }
        
        public MemberNotFoundException(int memberId) 
            : base($"会员ID {memberId} 不存在") 
        {
            MemberId = memberId;
        }
    }
    
    public class NoAvailableCopiesException : LibraryException
    {
        public int ItemId { get; }
        
        public NoAvailableCopiesException(int itemId) 
            : base($"物品ID {itemId} 没有可用副本") 
        {
            ItemId = itemId;
        }
    }
    
    public class BorrowLimitExceededException : LibraryException
    {
        public int MemberId { get; }
        
        public BorrowLimitExceededException(int memberId) 
            : base($"会员ID {memberId} 已达到最大借阅数量") 
        {
            MemberId = memberId;
        }
    }
    
    public class BorrowRecordNotFoundException : LibraryException
    {
        public BorrowRecordNotFoundException() 
            : base("找不到对应的借阅记录") { }
    }
}