using resume.Models;

namespace resume.ResultModels
{
    /// <summary>
    /// 该公司的，该岗位的人岗匹配结果
    /// </summary>
    public class JobMatchResultModelClass
    {
        public ICollection<ResumeMatch> Matches { get; set; }

    }
    public class ResumeMatch
    {
        public int ResumeId { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public string? HighestEducation { get; set; } // 最高学历
        public int TotalWorkYears { get; set; } // 工作总时间
        public string? GraduatedFrom { get; set; } // 毕业院校
        public string? Major { get; set; } //专业      
        public List<string> WorkTraits { get; set; }      
        public string MatchReason { get; set; }
    }


}
