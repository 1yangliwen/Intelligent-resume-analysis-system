namespace resume.ResultModels
{
    /// <summary>
    /// 返回该公司对应的所有岗位名字以及岗位ID
    /// </summary>
    public class AllJobInfoResultClass
    {
        
       public List<OneJobName> AllJobNames { get; set; }
    }

    public class OneJobName
    {
        public int Id { get; set; }//岗位ID
        public string JobName { get; set; }//岗位名字
        public int ResumeCount { get; set; } // 简历数
        public int NewResumeCount { get; set; } // 当日新添加的简历数
        public List<string> JobKeywords { get; set; } // 岗位关键词

    }

}