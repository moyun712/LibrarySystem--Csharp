// 创建文件 Models/BorrowStatus.cs


namespace LibraryManagementSystem.Models
{
    public enum BorrowStatus
    {
        Borrowed,
        Returned,
        Overdue,
        Reserved
    }
}