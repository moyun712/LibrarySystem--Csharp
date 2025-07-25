using LibraryManagementSystem.Repositories;
namespace LibraryManagementSystem.Models{
    public class Member :IEntity{
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public MemberType Type { get; set; }
        public int MaxBooksAllowed { get; set; }
        public List<int> BorrowedItems { get; set; } = new List<int>(); // 存储已借阅物品的ID

        public Member(int id, string name, string contactInfo, MemberType type)
        {
            Id = id;
            Name = name;
            ContactInfo = contactInfo;
            Type = type;
            
            // 根据会员类型设置最大借阅数量
            MaxBooksAllowed = type switch
            {
                MemberType.Student => 3,
                MemberType.Faculty => 5,
                MemberType.VIP => 10,
                MemberType.Regular => 2,
                _ => 2
            };
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"会员 ID: {Id}");
            Console.WriteLine($"姓名: {Name}");
            Console.WriteLine($"联系方式: {ContactInfo}");
            Console.WriteLine($"会员类型: {Type}");
            Console.WriteLine($"最大借阅数量: {MaxBooksAllowed}");
            Console.WriteLine($"当前借阅数量: {BorrowedItems.Count}");
        }

        
    }
}