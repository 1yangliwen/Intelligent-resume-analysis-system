using Microsoft.EntityFrameworkCore;
using resume.Models;
using resume.ResultModels;
using resume.WebSentModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace resume.Services
{
    public class JobService
    {
        private readonly MyDbContext _dbContext;

        public JobService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public JobIdResultClass UploadJob(JobInfoSentModel jobInfo)
        {
            var userId = jobInfo.UserId;/* 你需要提供一个方式来获取当前登录用户的Id */
                
            var company = _dbContext.Users
                                    .Include(u => u.Company)
                                    .FirstOrDefault(u => u.ID == userId)
                                    ?.Company;
            if (company == null)
            {
                // 这可能意味着没有找到与userId关联的Company
                // 在这种情况下，你可能需要返回一个错误信息，而不是继续执行后面的代码
                return new JobIdResultClass();
            }

            var newJob = new JobPosition
            {
                CompanyID = company.ID,
                Title = jobInfo.JobName,
                Description = jobInfo.JobDetails,
                //Options = string.Join(",", jobInfo.Options), // 这假设Options是以逗号分隔的字符串存储的
                CreatedDate = DateTime.Now,
                MinimumWorkYears = jobInfo.MinimumWorkYears,
                MinimumEducationLevel = jobInfo.MinimumEducationLevel,
                JobKeywords = jobInfo.JobKeywords
                .Select(keyword => new JobKeyword { Keyword = keyword }).ToList()
            };

            _dbContext.JobPositions.Add(newJob);
            _dbContext.SaveChanges();

            return new JobIdResultClass { Id = newJob.ID };
        }

        public JobMatchResultModelClass JobMatchResume(int userId, int jobId)
        {
            var company = _dbContext.Users
                            .Include(u => u.Company)
                            .FirstOrDefault(u => u.ID == userId)
                            ?.Company;

            if (company == null)
            {
                // 这可能意味着没有找到与userId关联的Company
                // 在这种情况下，你可能需要返回一个错误信息，而不是继续执行后面的代码
            }

            var jobTitle = _dbContext.JobPositions
                                     .FirstOrDefault(jp => jp.ID == jobId)
                                     ?.Title;

            if (string.IsNullOrEmpty(jobTitle))
            {
                // 这可能意味着没有找到与jobId关联的JobPosition
                // 在这种情况下，你可能需要返回一个错误信息，而不是继续执行后面的代码
            }

            var matches = _dbContext.ApplicantProfiles
                                    .Include(ap => ap.JobMatches)
                                    .Include(ap => ap.WorkTraits)  // 添加这行来包含WorkTraits属性
                                    .Where(ap => ap.JobMatches.Any(jm => jm.JobTitle == jobTitle && jm.Score != 0))
                                    .Select(ap => new ResumeMatch
                                    {
                                        ResumeId = ap.ID,
                                        Name = ap.Applicant.Name,
                                        Score = ap.JobMatches.First(jm => jm.JobTitle == jobTitle).Score,
                                        HighestEducation = ap.Applicant.HighestEducation,
                                        TotalWorkYears = ap.Applicant.TotalWorkYears,
                                        GraduatedFrom = ap.Applicant.GraduatedFrom,
                                        Major = ap.Applicant.Major,
                                        WorkTraits = ap.WorkTraitList,
                                        MatchReason = ap.JobMatches.First(jm => jm.JobTitle == jobTitle).Reason
                                    })
                                    .OrderByDescending(rm => rm.Score)
                                    .ToList();
            return new JobMatchResultModelClass { Matches = matches };
        }

        public AllJobInfoResultClass forAllJobName(int userId)
        {
            var company = _dbContext.Users
                            .Include(u => u.Company)
                            .FirstOrDefault(u => u.ID == userId)
                            ?.Company;

            if (company == null)
            {
                // 这可能意味着没有找到与userId关联的Company
                // 在这种情况下，你可能需要返回一个错误信息，而不是继续执行后面的代码
            }

            var jobPositions = _dbContext.JobPositions
                                //.Include(jp => jp.JobKeywords)
                                .Include(jp => jp.Resumes)
                                .Include(jp => jp.JobKeywords)
                                .Where(jp => jp.CompanyID == company.ID)
                                .ToList();

            var jobInfos = jobPositions.Select(jp => new OneJobName
            {
                Id = jp.ID,
                JobName = jp.Title,
                ResumeCount = jp.Resumes.Count,
                NewResumeCount = jp.Resumes.Count(r => r.CreatedDate.Date == DateTime.Now.Date),
                JobKeywords = jp.JobKeywords.Select(jk => jk.Keyword).ToList()
            }).ToList();

            return new AllJobInfoResultClass { AllJobNames = jobInfos };
        }

        public AllJobInfoResultClass forAllJobNameOrderedByResumeCount(int userId)
        {
            var company = _dbContext.Users
                            .Include(u => u.Company)
                            .FirstOrDefault(u => u.ID == userId)
                            ?.Company;

            if (company == null)
            {
                // Handle error here
            }

            var jobPositions = _dbContext.JobPositions
                                .Include(jp => jp.Resumes)
                                .Include(jp => jp.JobKeywords)
                                .Where(jp => jp.CompanyID == company.ID)
                                .ToList();

            var jobInfos = jobPositions
                            .Select(jp => new OneJobName
                            {
                                Id = jp.ID,
                                JobName = jp.Title,
                                ResumeCount = jp.Resumes.Count,
                                NewResumeCount = jp.Resumes.Count(r => r.CreatedDate.Date == DateTime.Now.Date),
                                JobKeywords = jp.JobKeywords.Select(jk => jk.Keyword).ToList()
                            })
                            .OrderByDescending(ojn => ojn.ResumeCount)  // Order by ResumeCount
                            .ToList();

            return new AllJobInfoResultClass { AllJobNames = jobInfos };
        }

        public int GetNewResumeCount(int jobId)
        {
            // 获取一天前的时间
            var oneDayAgo = DateTime.Now.AddDays(-1);

            // 计算在一天内创建的简历数
            return _dbContext.Resumes.Count(r => r.JobPositionID == jobId && r.CreatedDate > oneDayAgo);
        }

        public JobInfoSentModel GetJobInfo(int userId)
        {
            var job = _dbContext.JobPositions
                                .Where(j => j.ID == userId).FirstOrDefault();
            JobInfoSentModel jobInfo = new JobInfoSentModel() {
                UserId = userId,
                JobName = job.Title,
                JobDetails = job.Description,
            };
            return jobInfo;
        }
    }
}
