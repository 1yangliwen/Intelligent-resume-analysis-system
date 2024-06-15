namespace resume.Models
{
    public class WorkTrait
    {
        public int ID { get; set; }
        public int ApplicantProfileID { get; set; }  // 外键，连接到ApplicantProfile的ID
        public string? Trait { get; set; }  // 个人工作特性关键词

    }
}
