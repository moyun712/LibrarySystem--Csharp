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
    private readonly IRepository<ReservationRecord> _reservationRepository;
    
    public Library()
    {
        _itemRepository = new Repository<LibraryItem>();
        _memberRepository = new Repository<Member>();
        _borrowRecordRepository = new Repository<BorrowRecord>();
        _reservationRepository = new Repository<ReservationRecord>();
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
    
    // 删除会员
    public bool RemoveMember(int memberId)
    {
        try
        {
            // 检查会员是否存在
            var member = _memberRepository.GetById(memberId);
            if (member is null)
                throw new MemberNotFoundException(memberId);
            
            // 检查会员是否有未归还的借阅记录
            var activeBorrowings = _borrowRecordRepository.Find(r => 
                r.MemberId == memberId && r.Status == BorrowStatus.Borrowed);
                
            if (activeBorrowings.Any())
                throw new LibraryException($"会员ID {memberId} 有未归还的借阅记录，无法删除账户");
            
            // 删除会员
            bool result = _memberRepository.Remove(memberId);
            if (result)
                Console.WriteLine($"会员ID {memberId} 已成功删除");
            
            return result;
        }
        catch (LibraryException ex)
        {
            Console.WriteLine($"删除失败: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发生未知错误: {ex.Message}");
            return false;
        }
    }
    
    // 更新会员信息
    public bool UpdateMemberInfo(int memberId, string name = null, string contactInfo = null, MemberType? type = null)
    {
        try
        {
            // 检查会员是否存在
            var member = _memberRepository.GetById(memberId);
            if (member is null)
                throw new MemberNotFoundException(memberId);
            
            // 更新会员信息
            if (name != null) member.Name = name;
            if (contactInfo != null) member.ContactInfo = contactInfo;
            
            // 如果会员类型发生变化，需要重新计算最大借阅数量
            if (type.HasValue && type.Value != member.Type)
            {
                member.Type = type.Value;
                
                // 根据会员类型设置最大借阅数量
                member.MaxBooksAllowed = type.Value switch
                {
                    MemberType.Student => 3,
                    MemberType.Faculty => 5,
                    MemberType.VIP => 10,
                    MemberType.Regular => 2,
                    _ => 2
                };
                
                // 检查当前借阅数量是否超过新的最大借阅数量
                if (member.BorrowedItems.Count > member.MaxBooksAllowed)
                {
                    Console.WriteLine($"警告: 当前借阅数量({member.BorrowedItems.Count})超过了新会员类型的最大借阅限制({member.MaxBooksAllowed})");
                    Console.WriteLine("请尽快归还超出的物品");
                }
            }
            
            Console.WriteLine($"会员ID {memberId} 信息已成功更新");
            return true;
        }
        catch (LibraryException ex)
        {
            Console.WriteLine($"更新失败: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发生未知错误: {ex.Message}");
            return false;
        }
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
                
                // 处理该物品的预约通知
                ProcessReservationsAfterReturn(itemId);
                
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

            // 根据ID删除图书馆物品
    public bool RemoveItem(int itemId)
    {
        try
        {
            // 检查物品是否存在
            var item = _itemRepository.GetById(itemId);
            if (item is null)
                throw new ItemNotFoundException(itemId);
            
            // 检查物品是否已被借出
            var borrowedRecords = _borrowRecordRepository.Find(r => 
                r.ItemId == itemId && r.Status == BorrowStatus.Borrowed);
                
            if (borrowedRecords.Any())
                throw new LibraryException($"物品ID {itemId} 已被借出，无法删除");
            
            // 删除物品
            bool result = _itemRepository.Remove(itemId);
            if (result)
                Console.WriteLine($"物品ID {itemId} 已成功删除");
            
            return result;
        }
        catch (LibraryException ex)
        {
            Console.WriteLine($"删除失败: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发生未知错误: {ex.Message}");
            return false;
        }
    }
    
    // 更新图书信息
    public bool UpdateBookInfo(int bookId, string title = null, string author = null, 
                             string publisher = null, int? availableCopies = null, 
                             string isbn = null, int? publicationYear = null, 
                             BookCategory? category = null)
    {
        try
        {
            // 检查图书是否存在
            var item = _itemRepository.GetById(bookId);
            if (item is null)
                throw new ItemNotFoundException(bookId);
            
            // 确认是图书类型
            if (!(item is Book book))
                throw new LibraryException($"物品ID {bookId} 不是图书类型");
            
            // 更新图书信息
            if (title != null) book.Title = title;
            if (author != null) book.Author = author;
            if (publisher != null) book.Publisher = publisher;
            if (availableCopies.HasValue) book.AvailableCopies = availableCopies.Value;
            if (isbn != null) book.ISBN = isbn;
            if (publicationYear.HasValue) book.PublicationYear = publicationYear.Value;
            if (category.HasValue) book.Category = category.Value;
            
            Console.WriteLine($"图书ID {bookId} 信息已成功更新");
            return true;
        }
        catch (LibraryException ex)
        {
            Console.WriteLine($"更新失败: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发生未知错误: {ex.Message}");
            return false;
        }
    }
    
    // 更新杂志信息
    public bool UpdateMagazineInfo(int magazineId, string title = null, string author = null, 
                                 string publisher = null, int? availableCopies = null, 
                                 string issn = null, int? issueNumber = null, 
                                 string frequency = null, int? publicationYear = null)
    {
        try
        {
            // 检查杂志是否存在
            var item = _itemRepository.GetById(magazineId);
            if (item is null)
                throw new ItemNotFoundException(magazineId);
            
            // 确认是杂志类型
            if (!(item is Magazine magazine))
                throw new LibraryException($"物品ID {magazineId} 不是杂志类型");
            
            // 更新杂志信息
            if (title != null) magazine.Title = title;
            if (author != null) magazine.Author = author;
            if (publisher != null) magazine.Publisher = publisher;
            if (availableCopies.HasValue) magazine.AvailableCopies = availableCopies.Value;
            if (issn != null) magazine.ISSN = issn;
            if (issueNumber.HasValue) magazine.IssueNumber = issueNumber.Value;
            if (frequency != null) magazine.Frequency = frequency;
            if (publicationYear.HasValue) magazine.PublicationYear = publicationYear.Value;
            
            Console.WriteLine($"杂志ID {magazineId} 信息已成功更新");
            return true;
        }
        catch (LibraryException ex)
        {
            Console.WriteLine($"更新失败: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发生未知错误: {ex.Message}");
            return false;
        }
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
    
    // 获取指定会员的借阅记录
    public List<BorrowRecord> GetMemberBorrowRecords(int memberId, bool includeReturned = true)
    {
        try
        {
            // 检查会员是否存在
            var member = _memberRepository.GetById(memberId);
            if (member is null)
                throw new MemberNotFoundException(memberId);
            
            // 获取该会员的所有借阅记录
            var records = _borrowRecordRepository.Find(r => r.MemberId == memberId);
            
            // 根据参数决定是否包含已归还的记录
            if (!includeReturned)
            {
                records = records.Where(r => r.Status == BorrowStatus.Borrowed);
            }
            
            // 按借阅日期降序排序，最近的借阅排在前面
            return records.OrderByDescending(r => r.BorrowDate).ToList();
        }
        catch (LibraryException ex)
        {
            Console.WriteLine($"获取借阅记录失败: {ex.Message}");
            return new List<BorrowRecord>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发生未知错误: {ex.Message}");
            return new List<BorrowRecord>();
        }
    }
    
    // 显示指定会员的借阅记录
    public void DisplayMemberBorrowRecords(int memberId, bool includeReturned = true)
    {
        try
        {
            // 获取会员信息
            var member = _memberRepository.GetById(memberId);
            if (member is null)
                throw new MemberNotFoundException(memberId);
                
            Console.WriteLine($"\n=== 会员 {member.Name} (ID: {memberId}) 的借阅记录 ===");
            
            // 获取借阅记录
            var records = GetMemberBorrowRecords(memberId, includeReturned);
            
            if (records.Count is 0)
            {
                Console.WriteLine(includeReturned ? "该会员没有借阅记录" : "该会员当前没有借出的图书");
                return;
            }
            
            // 显示借阅记录
            foreach (var record in records)
            {
                // 获取物品信息以显示标题
                var item = _itemRepository.GetById(record.ItemId);
                string itemTitle = item != null ? item.Title : "未知物品";
                
                Console.WriteLine($"物品: {itemTitle} (ID: {record.ItemId})");
                Console.WriteLine($"借阅日期: {record.BorrowDate:yyyy-MM-dd}");
                Console.WriteLine($"应还日期: {record.DueDate:yyyy-MM-dd}");
                
                if (record.ReturnDate.HasValue)
                {
                    Console.WriteLine($"归还日期: {record.ReturnDate.Value:yyyy-MM-dd}");
                }
                else
                {
                    Console.WriteLine("归还日期: 尚未归还");
                }
                
                Console.WriteLine($"状态: {record.Status}");
                
                if (record.FineAmount > 0)
                {
                    Console.WriteLine($"罚款金额: {record.FineAmount:C}");
                }
                
                Console.WriteLine("-------------------");
            }
            
            // 显示统计信息
            int currentBorrowings = records.Count(r => r.Status == BorrowStatus.Borrowed);
            int overdueBorrowings = records.Count(r => r.Status == BorrowStatus.Overdue);
            int returnedItems = records.Count(r => r.Status == BorrowStatus.Returned);
            
            Console.WriteLine($"统计信息: 当前借阅 {currentBorrowings} 项，逾期 {overdueBorrowings} 项，已归还 {returnedItems} 项");
        }
        catch (LibraryException ex)
        {
            Console.WriteLine($"显示借阅记录失败: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发生未知错误: {ex.Message}");
        }
    }

            // 获取逾期记录
    public IEnumerable<BorrowRecord> GetOverdueRecords()
    {
        return _borrowRecordRepository.Find(r => 
            r.Status == BorrowStatus.Borrowed && DateTime.Now > r.DueDate)
            .OrderBy(r => r.DueDate);
    }
    
    // 高级借阅记录查询
    public List<BorrowRecord> SearchBorrowRecords(
        int? memberId = null,
        int? itemId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        BorrowStatus? status = null,
        bool sortByDateDesc = true)
    {
        try
        {
            // 构建查询条件
            var records = _borrowRecordRepository.GetAll().AsEnumerable();
            
            // 按会员ID筛选
            if (memberId.HasValue)
            {
                records = records.Where(r => r.MemberId == memberId.Value);
            }
            
            // 按物品ID筛选
            if (itemId.HasValue)
            {
                records = records.Where(r => r.ItemId == itemId.Value);
            }
            
            // 按借阅日期范围筛选
            if (startDate.HasValue)
            {
                records = records.Where(r => r.BorrowDate >= startDate.Value);
            }
            
            if (endDate.HasValue)
            {
                records = records.Where(r => r.BorrowDate <= endDate.Value);
            }
            
            // 按状态筛选
            if (status.HasValue)
            {
                records = records.Where(r => r.Status == status.Value);
            }
            
            // 排序
            if (sortByDateDesc)
            {
                records = records.OrderByDescending(r => r.BorrowDate);
            }
            else
            {
                records = records.OrderBy(r => r.BorrowDate);
            }
            
            return records.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"搜索借阅记录时发生错误: {ex.Message}");
            return new List<BorrowRecord>();
        }
    }
    
    // 显示借阅历史查询结果
    public void DisplayBorrowRecordSearchResults(List<BorrowRecord> records)
    {
        if (records.Count is 0)
        {
            Console.WriteLine("没有找到符合条件的借阅记录");
            return;
        }
        
        Console.WriteLine($"\n=== 找到 {records.Count} 条借阅记录 ===");
        
        foreach (var record in records)
        {
            // 获取会员信息
            var member = _memberRepository.GetById(record.MemberId);
            string memberName = member != null ? member.Name : "未知会员";
            
            // 获取物品信息
            var item = _itemRepository.GetById(record.ItemId);
            string itemTitle = item != null ? item.Title : "未知物品";
            
            Console.WriteLine($"记录ID: {record.Id}");
            Console.WriteLine($"会员: {memberName} (ID: {record.MemberId})");
            Console.WriteLine($"物品: {itemTitle} (ID: {record.ItemId})");
            Console.WriteLine($"借阅日期: {record.BorrowDate:yyyy-MM-dd}");
            Console.WriteLine($"应还日期: {record.DueDate:yyyy-MM-dd}");
            
            if (record.ReturnDate.HasValue)
            {
                Console.WriteLine($"归还日期: {record.ReturnDate.Value:yyyy-MM-dd}");
                
                // 计算借阅天数
                int borrowDays = (record.ReturnDate.Value - record.BorrowDate).Days;
                Console.WriteLine($"借阅时长: {borrowDays} 天");
            }
            else
            {
                Console.WriteLine("归还日期: 尚未归还");
                
                // 计算当前借阅天数
                int borrowDays = (DateTime.Now - record.BorrowDate).Days;
                Console.WriteLine($"当前借阅时长: {borrowDays} 天");
            }
            
            Console.WriteLine($"状态: {record.Status}");
            
            if (record.FineAmount > 0)
            {
                Console.WriteLine($"罚款金额: {record.FineAmount:C}");
            }
            
            Console.WriteLine("-------------------");
        }
        
        // 显示统计信息
        int currentBorrowings = records.Count(r => r.Status == BorrowStatus.Borrowed);
        int overdueBorrowings = records.Count(r => r.Status == BorrowStatus.Overdue);
        int returnedItems = records.Count(r => r.Status == BorrowStatus.Returned);
        
        Console.WriteLine($"统计信息: 当前借阅 {currentBorrowings} 项，逾期 {overdueBorrowings} 项，已归还 {returnedItems} 项");
        
        // 计算平均借阅时长（仅针对已归还的项目）
        var returnedRecords = records.Where(r => r.ReturnDate.HasValue).ToList();
        if (returnedRecords.Any())
        {
            double avgBorrowDays = returnedRecords.Average(r => (r.ReturnDate.Value - r.BorrowDate).TotalDays);
            Console.WriteLine($"平均借阅时长: {avgBorrowDays:F1} 天");
        }
    }
        
            // 预约图书
    public bool ReserveItem(int memberId, int itemId)
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

            // 检查物品是否有可用副本（如果有可用副本，应该直接借阅而不是预约）
            if (item.AvailableCopies > 0)
                throw new LibraryException($"物品ID {itemId} 有可用副本，可以直接借阅，无需预约");

            // 检查会员是否已经借阅了该物品
            var existingBorrow = _borrowRecordRepository.Find(r => 
                r.MemberId == memberId && r.ItemId == itemId && r.Status == BorrowStatus.Borrowed).FirstOrDefault();
            if (existingBorrow != null)
                throw new LibraryException($"会员ID {memberId} 已经借阅了物品ID {itemId}，不能重复预约");

            // 检查会员是否已经预约了该物品
            var existingReservation = _reservationRepository.Find(r => 
                r.MemberId == memberId && r.ItemId == itemId && 
                (r.Status == ReservationStatus.Waiting || r.Status == ReservationStatus.Fulfilled)).FirstOrDefault();
            if (existingReservation != null)
                throw new LibraryException($"会员ID {memberId} 已经预约了物品ID {itemId}，不能重复预约");

            // 创建预约记录
            var reservation = new ReservationRecord(0, memberId, itemId);
            _reservationRepository.Add(reservation);

            Console.WriteLine($"预约成功，记录ID: {reservation.Id}");
            return true;
        }
        catch (LibraryException ex)
        {
            Console.WriteLine($"预约失败: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发生未知错误: {ex.Message}");
            return false;
        }
    }

    // 取消预约
    public bool CancelReservation(int memberId, int reservationId)
    {
        try
        {
            // 检查会员是否存在
            var member = _memberRepository.GetById(memberId);
            if (member is null)
                throw new MemberNotFoundException(memberId);

            // 检查预约记录是否存在
            var reservation = _reservationRepository.GetById(reservationId);
            if (reservation is null)
                throw new LibraryException($"预约记录ID {reservationId} 不存在");

            // 检查预约是否属于该会员
            if (reservation.MemberId != memberId)
                throw new LibraryException($"预约记录ID {reservationId} 不属于会员ID {memberId}");

            // 检查预约状态是否可以取消
            if (reservation.Status != ReservationStatus.Waiting && reservation.Status != ReservationStatus.Fulfilled)
                throw new LibraryException($"预约记录ID {reservationId} 状态为 {reservation.Status}，无法取消");

            // 取消预约
            reservation.Cancel();
            Console.WriteLine($"预约记录ID {reservationId} 已成功取消");
            return true;
        }
        catch (LibraryException ex)
        {
            Console.WriteLine($"取消预约失败: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发生未知错误: {ex.Message}");
            return false;
        }
    }

    // 处理图书归还后的预约通知
    public void ProcessReservationsAfterReturn(int itemId)
    {
        try
        {
            // 获取该物品的所有等待中的预约，按预约日期排序（先预约先得）
            var waitingReservations = _reservationRepository.Find(r => 
                r.ItemId == itemId && r.Status == ReservationStatus.Waiting)
                .OrderBy(r => r.ReservationDate)
                .ToList();

            if (waitingReservations.Any())
            {
                // 获取第一个等待中的预约
                var nextReservation = waitingReservations.First();
                
                // 将预约状态更新为已兑现
                nextReservation.MarkAsFulfilled();
                
                // 获取会员信息用于通知
                var member = _memberRepository.GetById(nextReservation.MemberId);
                var item = _itemRepository.GetById(nextReservation.ItemId);
                
                if (member != null && item != null)
                {
                    Console.WriteLine($"通知: 会员 {member.Name} (ID: {member.Id}) 预约的物品 {item.Title} (ID: {item.Id}) 现在可以借阅了");
                    Console.WriteLine($"预约记录ID: {nextReservation.Id}，请在 {nextReservation.ExpiryDate:yyyy-MM-dd} 前来借阅");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"处理预约通知时发生错误: {ex.Message}");
        }
    }

    // 查看会员的预约记录
    public List<ReservationRecord> GetMemberReservations(int memberId)
    {
        try
        {
            // 检查会员是否存在
            var member = _memberRepository.GetById(memberId);
            if (member is null)
                throw new MemberNotFoundException(memberId);

            // 获取该会员的所有预约记录
            var reservations = _reservationRepository.Find(r => r.MemberId == memberId).ToList();
            
            // 检查预约是否已过期
            foreach (var reservation in reservations)
            {
                reservation.IsExpired();
            }
            
            return reservations;
        }
        catch (LibraryException ex)
        {
            Console.WriteLine($"获取预约记录失败: {ex.Message}");
            return new List<ReservationRecord>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发生未知错误: {ex.Message}");
            return new List<ReservationRecord>();
        }
    }

    // 显示会员的预约记录
    public void DisplayMemberReservations(int memberId)
    {
        try
        {
            // 获取会员信息
            var member = _memberRepository.GetById(memberId);
            if (member is null)
                throw new MemberNotFoundException(memberId);
                
            Console.WriteLine($"\n=== 会员 {member.Name} (ID: {memberId}) 的预约记录 ===");
            
            // 获取预约记录
            var reservations = GetMemberReservations(memberId);
            
            if (reservations.Count is 0)
            {
                Console.WriteLine("该会员没有预约记录");
                return;
            }
            
            // 显示预约记录
            foreach (var reservation in reservations)
            {
                // 获取物品信息以显示标题
                var item = _itemRepository.GetById(reservation.ItemId);
                string itemTitle = item != null ? item.Title : "未知物品";
                
                Console.WriteLine($"预约ID: {reservation.Id}");
                Console.WriteLine($"物品: {itemTitle} (ID: {reservation.ItemId})");
                Console.WriteLine($"预约日期: {reservation.ReservationDate:yyyy-MM-dd}");
                Console.WriteLine($"有效期至: {reservation.ExpiryDate:yyyy-MM-dd}");
                Console.WriteLine($"状态: {reservation.Status}");
                
                if (reservation.NotificationDate.HasValue)
                {
                    Console.WriteLine($"通知日期: {reservation.NotificationDate.Value:yyyy-MM-dd}");
                }
                
                Console.WriteLine("-------------------");
            }
        }
        catch (LibraryException ex)
        {
            Console.WriteLine($"显示预约记录失败: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发生未知错误: {ex.Message}");
        }
    }

    // 显示所有预约记录
    public void DisplayAllReservations()
    {
        Console.WriteLine("\n=== 预约记录列表 ===");
        var reservations = _reservationRepository.GetAll().ToList();

        if (reservations.Count is 0)
        {
            Console.WriteLine("没有预约记录");
            return;
        }

        foreach (var reservation in reservations)
        {
            // 检查预约是否已过期
            reservation.IsExpired();
            
            // 获取会员信息
            var member = _memberRepository.GetById(reservation.MemberId);
            string memberName = member != null ? member.Name : "未知会员";
            
            // 获取物品信息
            var item = _itemRepository.GetById(reservation.ItemId);
            string itemTitle = item != null ? item.Title : "未知物品";
            
            Console.WriteLine($"预约ID: {reservation.Id}");
            Console.WriteLine($"会员: {memberName} (ID: {reservation.MemberId})");
            Console.WriteLine($"物品: {itemTitle} (ID: {reservation.ItemId})");
            Console.WriteLine($"预约日期: {reservation.ReservationDate:yyyy-MM-dd}");
            Console.WriteLine($"有效期至: {reservation.ExpiryDate:yyyy-MM-dd}");
            Console.WriteLine($"状态: {reservation.Status}");
            
            if (reservation.NotificationDate.HasValue)
            {
                Console.WriteLine($"通知日期: {reservation.NotificationDate.Value:yyyy-MM-dd}");
            }
            
            Console.WriteLine("-------------------");
        }
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