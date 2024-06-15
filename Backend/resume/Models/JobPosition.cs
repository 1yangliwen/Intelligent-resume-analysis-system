namespace resume.Models
{
    public class JobPosition
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }  // 新增的CreatedDate属性
        public int MinimumWorkYears { get; set; }
        public string? MinimumEducationLevel { get; set; }
        public ICollection<JobKeyword> JobKeywords { get; set; } // 岗位关键词
        // 导航属性
        public ICollection<Resume> Resumes { get; set; }
        public Company Company { get; set; }

    }
    public class JobKeyword
    {
        public int ID { get; set; }
        public int JobPositionID { get; set; } // 外键，连接到JobPosition表的ID
        public string Keyword { get; set; } // 关键词

        // 导航属性
        public JobPosition JobPosition { get; set; }
    }
}
