using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using resume.Models;
using resume.Others;
using resume.ResultModels;
using resume.WebSentModel;
using System.ComponentModel.Design;
//using static Google.Protobuf.Collections.MapField<TKey, TValue>;

namespace resume.Services
{
    public class ApplicantService
    {
        private readonly MyDbContext _dbContext;

        public ApplicantService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Applicant> GetAllApplicants()
        {
            return _dbContext.Applicants.ToList();
        }

        public Applicant GetApplicantById(int id)
        {
            return _dbContext.Applicants.FirstOrDefault(a => a.ID == id);
        }

        // Add more methods for querying the Applicants table as needed
        //public async Task<int> CreateApplicantFromDictionary(Dictionary<string, object> data)
        //{
        //    // Convert all JObject instances in 'data' to Dictionary<string, object>
        //    data = ConvertJObjectsToDictionaries(data);
        //    var applicant = new Applicant();
        //    List<SkillCertificate> skillCertificates = new List<SkillCertificate>();
        //    List<Award> awards = new List<Award>();
        //    List<WorkExperience> workExperiences = new List<WorkExperience>();
        //    List<EducationBackground> educationBackground = new List<EducationBackground>();
        //    var applicantProfile = new ApplicantProfile
        //    {
        //        //TalentTraits = data.ContainsKey("技能") ? data["技能"].ToString() : null,
        //        MatchingReason = data.ContainsKey("人岗匹配的理由") ? data["人岗匹配的理由"].ToString() : null,
        //        MatchingScore = data.ContainsKey("人岗匹配程度分数") ? Convert.ToInt32(data["人岗匹配程度分数"]) : 0,
        //        WorkStability = data.ContainsKey("工作稳定性的程度") ? data["工作稳定性的程度"].ToString() : null,
        //        StabilityReason = data.ContainsKey("给出此工作稳定性判断的原因") ? data["给出此工作稳定性判断的原因"].ToString() : null,
        //    };
        //    foreach (var pair in data)
        //    {
        //        switch (pair.Key)
        //        {
        //            case "姓名":
        //                applicant.Name = pair.Value != null ? pair.Value.ToString() : null;
        //                break;
        //            case "个人邮箱":
        //                applicant.Email = pair.Value != null ? pair.Value.ToString() : null;
        //                break;
        //            case "手机号":
        //                applicant.PhoneNumber = pair.Value != null ? pair.Value.ToString() : null;
        //                break;
        //            case "年龄":
        //                applicant.Age = pair.Value != null ? Convert.ToInt32(pair.Value) : 0;
        //                break;
        //            case "性别":
        //                applicant.Gender = pair.Value != null ? pair.Value.ToString() : null;
        //                break;
        //            case "求职意向岗位":
        //                applicant.JobIntention = pair.Value != null ? pair.Value.ToString() : null;
        //                break;
        //            case "自我评价":
        //                applicant.SelfEvaluation = pair.Value != null ? pair.Value.ToString() : null;
        //                break;
        //            case "最高学历":
        //                applicant.HighestEducation = pair.Value != null ? pair.Value.ToString() : null;
        //                break;
        //            case "最高学历学校等级":
        //                applicant.GraduatedFromLevel = pair.Value != null ? pair.Value.ToString() : null;
        //                break;
        //            case "毕业院校":
        //                applicant.GraduatedFrom = pair.Value != null ? pair.Value.ToString() : null;
        //                break;
        //            case "专业":
        //                applicant.Major = pair.Value != null ? pair.Value.ToString() : null;
        //                break;
        //            case "技能证书":
        //                // assuming SkillCertificates field is a comma-separated string
        //                if (pair.Value != null)
        //                {
        //                    string[] skills = pair.Value.ToString().Split(',');
        //                    foreach (var skill in skills)
        //                    {
        //                        // Remove unnecessary characters like "{", "}", "[", "]", and '"'
        //                        string cleanedSkill = skill.Trim().Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "").Replace("\"", "").Replace("\r", "").Replace("\n", "");
        //                        skillCertificates.Add(new SkillCertificate { SkillName = cleanedSkill, ApplicantID = applicant.ID });
        //                    }
        //                }
        //                break;
        //            case "获奖荣誉":
        //                if (pair.Value != null) // check if pair.Value is not null
        //                {
        //                    // assuming Awards field is a comma-separated string
        //                    string[] awardList = pair.Value.ToString().Split(',');
        //                    foreach (var award in awardList)
        //                    {
        //                        // Remove unnecessary characters like "{", "}", "[", "]", and '"'
        //                        string cleanedAward = award.Trim().Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "").Replace("\"", "").Replace("\r", "").Replace("\n", "");
        //                        awards.Add(new Award { AwardName = cleanedAward, ApplicantID = applicant.ID });
        //                    }
        //                }
        //                break;
        //            case "工作经历":
        //                if (pair.Value != null) // check if pair.Value is not null
        //                {
        //                    // assuming WorkExperiences field is a JSON array of WorkExperiences objects
        //                    workExperiences = JsonConvert.DeserializeObject<List<WorkExperience>>(pair.Value.ToString());
        //                    foreach (var exp in workExperiences)
        //                    {
        //                        exp.ApplicantID = applicant.ID;
        //                    }
        //                }

        //                break;
        //            case "教育背景":
        //                if (pair.Value != null)
        //                {
        //                    educationBackground = JsonConvert.DeserializeObject<List<EducationBackground>>(pair.Value.ToString());
        //                    foreach (var edu in educationBackground)
        //                    {
        //                        edu.ApplicantID = applicant.ID;
        //                    }
        //                }
        //                break;

        //            case "个人特性":
        //                {
        //                    var characteristicsData = pair.Value is JObject jObject ? ConvertJObjectsToDictionaries(jObject.ToObject<Dictionary<string, object>>()) : (Dictionary<string, object>)pair.Value;
        //                    applicantProfile.PersonalCharacteristics = ExtractPersonalCharacteristics(characteristicsData);
        //                    break;
        //                }
        //            case "技能和经验":
        //                {
        //                    var skillsData = pair.Value is JObject jObject ? ConvertJObjectsToDictionaries(jObject.ToObject<Dictionary<string, object>>()) : (Dictionary<string, object>)pair.Value;
        //                    applicantProfile.SkillsAndExperiences = ExtractSkillsAndExperiences(skillsData);
        //                    break;
        //                }
        //            case "成就和亮点":
        //                {
        //                    var achievementsData = pair.Value is JObject jObject ? ConvertJObjectsToDictionaries(jObject.ToObject<Dictionary<string, object>>()) : (Dictionary<string, object>)pair.Value;
        //                    applicantProfile.AchievementsAndHighlights = ExtractAchievementsAndHighlights(achievementsData);
        //                    break;
        //                }   
        //            case "工作总时间":
        //                applicant.TotalWorkYears = pair.Value != null ? Convert.ToInt32(pair.Value) : 0;
        //                break;
        //            // new cases
        //            case "工作特性":
        //                // 先确保这个字段存在，并且是一个列表类型
        //                if (pair.Value is IEnumerable<object> traitsList)
        //                {
        //                    // 初始化工作特性列表
        //                    //applicantProfile.WorkTraits = new List<WorkTrait>();
        //                    if (applicantProfile.WorkTraits == null)
        //                    {
        //                        applicantProfile.WorkTraits = new List<WorkTrait>();
        //                    }
        //                    // 遍历列表，将每个特性转换为字符串并添加到列表中
        //                    foreach (var trait in traitsList)
        //                    {
        //                        applicantProfile.WorkTraits.Add(new WorkTrait
        //                        {
        //                            Trait = trait.ToString(),
        //                            ApplicantProfileID = applicantProfile.ID
        //                        });
        //                    }
        //                }
        //                break;
        //            case "产品运营":
        //            case "平面设计师":
        //            case "财务":
        //            case "市场营销":
        //            case "项目主管":
        //            case "开发工程师":
        //            case "文员":
        //            case "电商运营":
        //            case "人力资源管理":
        //            case "风控专员":
        //                // ... (all other job titles)
        //                {
        //                    var jobMatchData = pair.Value as Dictionary<string, object>;
        //                    if (jobMatchData != null)
        //                    {
        //                        var jobMatch = new JobMatch
        //                        {
        //                            JobTitle = pair.Key,
        //                            Score = jobMatchData.ContainsKey("人岗匹配程度分数") ? Convert.ToInt32(jobMatchData["人岗匹配程度分数"]) : 0,
        //                            Reason = jobMatchData.ContainsKey("人岗匹配的理由") ? (string)jobMatchData["人岗匹配的理由"] : null,
        //                        };
        //                        if (applicantProfile.JobMatches == null)
        //                        {
        //                            applicantProfile.JobMatches = new List<JobMatch>();
        //                        }
        //                        applicantProfile.JobMatches.Add(jobMatch);
        //                    }
        //                    break;
        //                }
        //        }
        //    }

        //    applicant.ApplicantProfile = applicantProfile;

        //    _dbContext.Applicants.Add(applicant);
        //    await _dbContext.SaveChangesAsync();

        //    // 设置每个 SkillCertificate 对象的 ApplicantID，并添加到数据库
        //    foreach (var skill in skillCertificates)
        //    {
        //        skill.ApplicantID = applicant.ID;
        //    }
        //    _dbContext.SkillCertificates.AddRange(skillCertificates);

        //    // 设置每个 Award 对象的 ApplicantID，并添加到数据库
        //    foreach (var award in awards)
        //    {
        //        award.ApplicantID = applicant.ID;
        //    }
        //    _dbContext.Awards.AddRange(awards);

        //    // 设置每个 WorkExperiences 对象的 ApplicantID，并添加到数据库
        //    foreach (var exp in workExperiences)
        //    {
        //        exp.ApplicantID = applicant.ID;
        //    }
        //    _dbContext.WorkExperiences.AddRange(workExperiences);

        //    // 设置每个 EducationBackground 对象的 ApplicantID，并添加到数据库
        //    foreach (var edu in educationBackground)
        //    {
        //        edu.ApplicantID = applicant.ID;
        //    }
        //    _dbContext.EducationBackgrounds.AddRange(educationBackground);

        //    await _dbContext.SaveChangesAsync();

        //    return applicant.ID;
        //}


        // Add more methods for querying the Applicants table as needed
        public async Task<int> CreateApplicantFromDictionary(Dictionary<string, object> data)
        {
            // Convert all JObject instances in 'data' to Dictionary<string, object>
            Console.WriteLine("开始转换JObjects...");
            data = ConvertJObjectsToDictionaries(data);
            var applicant = new Applicant();
            List<SkillCertificate> skillCertificates = new List<SkillCertificate>();
            List<Award> awards = new List<Award>();
            List<WorkExperience> workExperiences = new List<WorkExperience>();
            List<EducationBackground> educationBackground = new List<EducationBackground>();

            Console.WriteLine("创建ApplicantProfile...");
            var applicantProfile = new ApplicantProfile
            {
                MatchingReason = data.ContainsKey("人岗匹配的理由") ? data["人岗匹配的理由"].ToString() : null,
                MatchingScore = data.ContainsKey("人岗匹配程度分数") ? Convert.ToInt32(data["人岗匹配程度分数"]) : 0,
                WorkStability = data.ContainsKey("工作稳定性的程度") ? data["工作稳定性的程度"].ToString() : null,
                StabilityReason = data.ContainsKey("给出此工作稳定性判断的原因") ? data["给出此工作稳定性判断的原因"].ToString() : null,
            };
            Console.WriteLine("ApplicantProfile创建成功...");

            foreach (var pair in data)
            {
                Console.WriteLine($"处理字段：{pair.Key}");
                switch (pair.Key)
                {
                    case "姓名":
                        applicant.Name = pair.Value != null ? pair.Value.ToString() : null;
                        Console.WriteLine($"姓名: {applicant.Name}");
                        break;
                    case "个人邮箱":
                        applicant.Email = pair.Value != null ? pair.Value.ToString() : null;
                        Console.WriteLine($"个人邮箱: {applicant.Email}");
                        break;
                    case "手机号":
                        applicant.PhoneNumber = pair.Value != null ? pair.Value.ToString() : null;
                        Console.WriteLine($"手机号: {applicant.PhoneNumber}");
                        break;
                    case "年龄":
                        applicant.Age = pair.Value != null ? Convert.ToInt32(pair.Value) : 0;
                        Console.WriteLine($"年龄: {applicant.Age}");
                        break;
                    case "性别":
                        applicant.Gender = pair.Value != null ? pair.Value.ToString() : null;
                        Console.WriteLine($"性别: {applicant.Gender}");
                        break;
                    case "求职意向岗位":
                        applicant.JobIntention = pair.Value != null ? pair.Value.ToString() : null;
                        Console.WriteLine($"求职意向岗位: {applicant.JobIntention}");
                        break;
                    case "自我评价":
                        applicant.SelfEvaluation = pair.Value != null ? pair.Value.ToString() : null;
                        Console.WriteLine($"自我评价: {applicant.SelfEvaluation}");
                        break;
                    case "最高学历":
                        applicant.HighestEducation = pair.Value != null ? pair.Value.ToString() : null;
                        Console.WriteLine($"最高学历: {applicant.HighestEducation}");
                        break;
                    case "最高学历学校等级":
                        applicant.GraduatedFromLevel = pair.Value != null ? pair.Value.ToString() : null;
                        Console.WriteLine($"最高学历学校等级: {applicant.GraduatedFromLevel}");
                        break;
                    case "毕业院校":
                        applicant.GraduatedFrom = pair.Value != null ? pair.Value.ToString() : null;
                        Console.WriteLine($"毕业院校: {applicant.GraduatedFrom}");
                        break;
                    case "专业":
                        applicant.Major = pair.Value != null ? pair.Value.ToString() : null;
                        Console.WriteLine($"专业: {applicant.Major}");
                        break;
                    case "技能证书":
                        if (pair.Value != null)
                        {
                            Console.WriteLine("处理技能证书...");
                            string[] skills = pair.Value.ToString().Split(',');
                            foreach (var skill in skills)
                            {
                                string cleanedSkill = skill.Trim().Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "").Replace("\"", "").Replace("\r", "").Replace("\n", "");
                                skillCertificates.Add(new SkillCertificate { SkillName = cleanedSkill, ApplicantID = applicant.ID });
                            }
                        }
                        break;
                    case "获奖荣誉":
                        if (pair.Value != null)
                        {
                            Console.WriteLine("处理获奖荣誉...");
                            string[] awardList = pair.Value.ToString().Split(',');
                            foreach (var award in awardList)
                            {
                                string cleanedAward = award.Trim().Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "").Replace("\"", "").Replace("\r", "").Replace("\n", "");
                                awards.Add(new Award { AwardName = cleanedAward, ApplicantID = applicant.ID });
                            }
                        }
                        break;
                    case "工作经历":
                        if (pair.Value != null)
                        {
                            Console.WriteLine("处理工作经历...");
                            workExperiences = JsonConvert.DeserializeObject<List<WorkExperience>>(pair.Value.ToString());
                            foreach (var exp in workExperiences)
                            {
                                exp.ApplicantID = applicant.ID;
                            }
                        }
                        break;
                    case "教育背景":
                        if (pair.Value != null)
                        {
                            Console.WriteLine("处理教育背景...");
                            educationBackground = JsonConvert.DeserializeObject<List<EducationBackground>>(pair.Value.ToString());
                            foreach (var edu in educationBackground)
                            {
                                edu.ApplicantID = applicant.ID;
                            }
                        }
                        break;
                    case "个人特性":
                        {
                            Console.WriteLine("处理个人特性...");
                            var characteristicsData = pair.Value is JObject jObject ? ConvertJObjectsToDictionaries(jObject.ToObject<Dictionary<string, object>>()) : (Dictionary<string, object>)pair.Value;
                            applicantProfile.PersonalCharacteristics = ExtractPersonalCharacteristics(characteristicsData);
                            break;
                        }
                    case "技能和经验":
                        {
                            Console.WriteLine("处理技能和经验...");
                            var skillsData = pair.Value is JObject jObject ? ConvertJObjectsToDictionaries(jObject.ToObject<Dictionary<string, object>>()) : (Dictionary<string, object>)pair.Value;
                            applicantProfile.SkillsAndExperiences = ExtractSkillsAndExperiences(skillsData);
                            break;
                        }
                    case "成就和亮点":
                        {
                            Console.WriteLine("处理成就和亮点...");
                            var achievementsData = pair.Value is JObject jObject ? ConvertJObjectsToDictionaries(jObject.ToObject<Dictionary<string, object>>()) : (Dictionary<string, object>)pair.Value;
                            applicantProfile.AchievementsAndHighlights = ExtractAchievementsAndHighlights(achievementsData);
                            break;
                        }
                    case "工作总时间":
                        applicant.TotalWorkYears = pair.Value != null ? Convert.ToInt32(pair.Value) : 0;
                        Console.WriteLine($"工作总时间: {applicant.TotalWorkYears}");
                        break;
                    case "工作特性":
                        {
                            if (pair.Value is IEnumerable<object> traitsList)
                            {
                                Console.WriteLine("处理工作特性...");
                                if (applicantProfile.WorkTraits == null)
                                {
                                    applicantProfile.WorkTraits = new List<WorkTrait>();
                                }
                                foreach (var trait in traitsList)
                                {
                                    applicantProfile.WorkTraits.Add(new WorkTrait
                                    {
                                        Trait = trait.ToString(),
                                        ApplicantProfileID = applicantProfile.ID
                                    });
                                }
                            }
                            break;
                        }
                    case "产品运营":
                    case "平面设计师":
                    case "财务":
                    case "市场营销":
                    case "项目主管":
                    case "开发工程师":
                    case "文员":
                    case "电商运营":
                    case "人力资源管理":
                    case "风控专员":
                        {
                            var jobMatchData = pair.Value as Dictionary<string, object>;
                            if (jobMatchData != null)
                            {
                                Console.WriteLine($"处理职位匹配: {pair.Key}");
                                var jobMatch = new JobMatch
                                {
                                    JobTitle = pair.Key,
                                    Score = jobMatchData.ContainsKey("人岗匹配程度分数") ? Convert.ToInt32(jobMatchData["人岗匹配程度分数"]) : 0,
                                    Reason = jobMatchData.ContainsKey("人岗匹配的理由") ? (string)jobMatchData["人岗匹配的理由"] : null,
                                };
                                if (applicantProfile.JobMatches == null)
                                {
                                    applicantProfile.JobMatches = new List<JobMatch>();
                                }
                                applicantProfile.JobMatches.Add(jobMatch);
                            }
                            break;
                        }
                }
            }

            Console.WriteLine("开始保存申请人信息...");
            applicant.ApplicantProfile = applicantProfile;

            _dbContext.Applicants.Add(applicant);
            await _dbContext.SaveChangesAsync();
            Console.WriteLine($"申请人信息保存成功，ID: {applicant.ID}");

            // 设置每个 SkillCertificate 对象的 ApplicantID，并添加到数据库
            foreach (var skill in skillCertificates)
            {
                skill.ApplicantID = applicant.ID;
            }
            _dbContext.SkillCertificates.AddRange(skillCertificates);
            Console.WriteLine("技能证书保存成功...");

            // 设置每个 Award 对象的 ApplicantID，并添加到数据库
            foreach (var award in awards)
            {
                award.ApplicantID = applicant.ID;
            }
            _dbContext.Awards.AddRange(awards);
            Console.WriteLine("获奖荣誉保存成功...");

            // 设置每个 WorkExperiences 对象的 ApplicantID，并添加到数据库
            foreach (var exp in workExperiences)
            {
                exp.ApplicantID = applicant.ID;
            }
            _dbContext.WorkExperiences.AddRange(workExperiences);
            Console.WriteLine("工作经历保存成功...");

            // 设置每个 EducationBackground 对象的 ApplicantID，并添加到数据库
            foreach (var edu in educationBackground)
            {
                edu.ApplicantID = applicant.ID;
            }
            _dbContext.EducationBackgrounds.AddRange(educationBackground);
            Console.WriteLine("教育背景保存成功...");

            await _dbContext.SaveChangesAsync();
            Console.WriteLine("所有信息保存成功...");

            return applicant.ID;
        }

        public async Task<int> CreateApplicantFromDictionary(MyDbContext context, Dictionary<string, object> data)
        {
            // Convert all JObject instances in 'data' to Dictionary<string, object>
            Console.WriteLine("开始转换JObjects...");
            data = ConvertJObjectsToDictionaries(data);
            var applicant = new Applicant();
            List<SkillCertificate> skillCertificates = new List<SkillCertificate>();
            List<Award> awards = new List<Award>();
            List<WorkExperience> workExperiences = new List<WorkExperience>();
            List<EducationBackground> educationBackground = new List<EducationBackground>();

            Console.WriteLine("创建ApplicantProfile...");
            var applicantProfile = new ApplicantProfile
            {
                MatchingReason = data.ContainsKey("人岗匹配的理由") ? data["人岗匹配的理由"].ToString() : null,
                MatchingScore = data.ContainsKey("人岗匹配程度分数") ? Convert.ToInt32(data["人岗匹配程度分数"]) : 0,
                WorkStability = data.ContainsKey("工作稳定性的程度") ? data["工作稳定性的程度"].ToString() : null,
                StabilityReason = data.ContainsKey("给出此工作稳定性判断的原因") ? data["给出此工作稳定性判断的原因"].ToString() : null,
            };
            Console.WriteLine("ApplicantProfile创建成功...");

            foreach (var pair in data)
            {
                Console.WriteLine($"处理字段：{pair.Key}");
                switch (pair.Key)
                {
                    case "姓名":
                        applicant.Name = pair.Value != null ? pair.Value.ToString() : null;
                        Console.WriteLine($"姓名: {applicant.Name}");
                        break;
                    case "个人邮箱":
                        applicant.Email = pair.Value != null ? pair.Value.ToString() : null;
                        Console.WriteLine($"个人邮箱: {applicant.Email}");
                        break;
                    case "手机号":
                        applicant.PhoneNumber = pair.Value != null ? pair.Value.ToString() : null;
                        Console.WriteLine($"手机号: {applicant.PhoneNumber}");
                        break;
                    case "年龄":
                        applicant.Age = pair.Value != null ? Convert.ToInt32(pair.Value) : 0;
                        Console.WriteLine($"年龄: {applicant.Age}");
                        break;
                    case "性别":
                        applicant.Gender = pair.Value != null ? pair.Value.ToString() : null;
                        Console.WriteLine($"性别: {applicant.Gender}");
                        break;
                    case "求职意向岗位":
                        applicant.JobIntention = pair.Value != null ? pair.Value.ToString() : null;
                        Console.WriteLine($"求职意向岗位: {applicant.JobIntention}");
                        break;
                    case "自我评价":
                        applicant.SelfEvaluation = pair.Value != null ? pair.Value.ToString() : null;
                        Console.WriteLine($"自我评价: {applicant.SelfEvaluation}");
                        break;
                    case "最高学历":
                        applicant.HighestEducation = pair.Value != null ? pair.Value.ToString() : null;
                        Console.WriteLine($"最高学历: {applicant.HighestEducation}");
                        break;
                    case "最高学历学校等级":
                        applicant.GraduatedFromLevel = pair.Value != null ? pair.Value.ToString() : null;
                        Console.WriteLine($"最高学历学校等级: {applicant.GraduatedFromLevel}");
                        break;
                    case "毕业院校":
                        applicant.GraduatedFrom = pair.Value != null ? pair.Value.ToString() : null;
                        Console.WriteLine($"毕业院校: {applicant.GraduatedFrom}");
                        break;
                    case "专业":
                        applicant.Major = pair.Value != null ? pair.Value.ToString() : null;
                        Console.WriteLine($"专业: {applicant.Major}");
                        break;
                    case "技能证书":
                        if (pair.Value != null)
                        {
                            Console.WriteLine("处理技能证书...");
                            string[] skills = pair.Value.ToString().Split(',');
                            foreach (var skill in skills)
                            {
                                string cleanedSkill = skill.Trim().Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "").Replace("\"", "").Replace("\r", "").Replace("\n", "");
                                skillCertificates.Add(new SkillCertificate { SkillName = cleanedSkill, ApplicantID = applicant.ID });
                            }
                        }
                        break;
                    case "获奖荣誉":
                        if (pair.Value != null)
                        {
                            Console.WriteLine("处理获奖荣誉...");
                            string[] awardList = pair.Value.ToString().Split(',');
                            foreach (var award in awardList)
                            {
                                string cleanedAward = award.Trim().Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "").Replace("\"", "").Replace("\r", "").Replace("\n", "");
                                awards.Add(new Award { AwardName = cleanedAward, ApplicantID = applicant.ID });
                            }
                        }
                        break;
                    case "工作经历":
                        if (pair.Value != null)
                        {
                            Console.WriteLine("处理工作经历...");
                            workExperiences = JsonConvert.DeserializeObject<List<WorkExperience>>(pair.Value.ToString());
                            foreach (var exp in workExperiences)
                            {
                                exp.ApplicantID = applicant.ID;
                            }
                        }
                        break;
                    case "教育背景":
                        if (pair.Value != null)
                        {
                            Console.WriteLine("处理教育背景...");
                            educationBackground = JsonConvert.DeserializeObject<List<EducationBackground>>(pair.Value.ToString());
                            foreach (var edu in educationBackground)
                            {
                                edu.ApplicantID = applicant.ID;
                            }
                        }
                        break;
                    case "个人特性":
                        {
                            Console.WriteLine("处理个人特性...");
                            var characteristicsData = pair.Value is JObject jObject ? ConvertJObjectsToDictionaries(jObject.ToObject<Dictionary<string, object>>()) : (Dictionary<string, object>)pair.Value;
                            applicantProfile.PersonalCharacteristics = ExtractPersonalCharacteristics(characteristicsData);
                            break;
                        }
                    case "技能和经验":
                        {
                            Console.WriteLine("处理技能和经验...");
                            var skillsData = pair.Value is JObject jObject ? ConvertJObjectsToDictionaries(jObject.ToObject<Dictionary<string, object>>()) : (Dictionary<string, object>)pair.Value;
                            applicantProfile.SkillsAndExperiences = ExtractSkillsAndExperiences(skillsData);
                            break;
                        }
                    case "成就和亮点":
                        {
                            Console.WriteLine("处理成就和亮点...");
                            var achievementsData = pair.Value is JObject jObject ? ConvertJObjectsToDictionaries(jObject.ToObject<Dictionary<string, object>>()) : (Dictionary<string, object>)pair.Value;
                            applicantProfile.AchievementsAndHighlights = ExtractAchievementsAndHighlights(achievementsData);
                            break;
                        }
                    case "工作总时间":
                        applicant.TotalWorkYears = pair.Value != null ? Convert.ToInt32(pair.Value) : 0;
                        Console.WriteLine($"工作总时间: {applicant.TotalWorkYears}");
                        break;
                    case "工作特性":
                        {
                            if (pair.Value is IEnumerable<object> traitsList)
                            {
                                Console.WriteLine("处理工作特性...");
                                if (applicantProfile.WorkTraits == null)
                                {
                                    applicantProfile.WorkTraits = new List<WorkTrait>();
                                }
                                foreach (var trait in traitsList)
                                {
                                    applicantProfile.WorkTraits.Add(new WorkTrait
                                    {
                                        Trait = trait.ToString(),
                                        ApplicantProfileID = applicantProfile.ID
                                    });
                                }
                            }
                            break;
                        }
                    case "产品运营":
                    case "平面设计师":
                    case "财务":
                    case "市场营销":
                    case "项目主管":
                    case "开发工程师":
                    case "文员":
                    case "电商运营":
                    case "人力资源管理":
                    case "风控专员":
                        {
                            var jobMatchData = pair.Value as Dictionary<string, object>;
                            if (jobMatchData != null)
                            {
                                Console.WriteLine($"处理职位匹配: {pair.Key}");
                                var jobMatch = new JobMatch
                                {
                                    JobTitle = pair.Key,
                                    Score = jobMatchData.ContainsKey("人岗匹配程度分数") ? Convert.ToInt32(jobMatchData["人岗匹配程度分数"]) : 0,
                                    Reason = jobMatchData.ContainsKey("人岗匹配的理由") ? (string)jobMatchData["人岗匹配的理由"] : null,
                                };
                                if (applicantProfile.JobMatches == null)
                                {
                                    applicantProfile.JobMatches = new List<JobMatch>();
                                }
                                applicantProfile.JobMatches.Add(jobMatch);
                            }
                            break;
                        }
                }
            }

            Console.WriteLine("开始保存申请人信息...");
            applicant.ApplicantProfile = applicantProfile;

            context.Applicants.Add(applicant);
            await context.SaveChangesAsync();
            Console.WriteLine($"申请人信息保存成功，ID: {applicant.ID}");

            // 设置每个 SkillCertificate 对象的 ApplicantID，并添加到数据库
            foreach (var skill in skillCertificates)
            {
                skill.ApplicantID = applicant.ID;
            }
            context.SkillCertificates.AddRange(skillCertificates);
            Console.WriteLine("技能证书保存成功...");

            // 设置每个 Award 对象的 ApplicantID，并添加到数据库
            foreach (var award in awards)
            {
                award.ApplicantID = applicant.ID;
            }
            context.Awards.AddRange(awards);
            Console.WriteLine("获奖荣誉保存成功...");

            // 设置每个 WorkExperiences 对象的 ApplicantID，并添加到数据库
            foreach (var exp in workExperiences)
            {
                exp.ApplicantID = applicant.ID;
            }
            context.WorkExperiences.AddRange(workExperiences);
            Console.WriteLine("工作经历保存成功...");

            // 设置每个 EducationBackground 对象的 ApplicantID，并添加到数据库
            foreach (var edu in educationBackground)
            {
                edu.ApplicantID = applicant.ID;
            }
            context.EducationBackgrounds.AddRange(educationBackground);
            Console.WriteLine("教育背景保存成功...");

            await context.SaveChangesAsync();
            Console.WriteLine("所有信息保存成功...");

            return applicant.ID;
        }



        public AllSimpleResumeInfoClass ForAllSimpleResumes(int userId)
        {
            // 查找当前用户对应的公司
            var company = _dbContext.Users
                                    .Include(u => u.Company)
                                    .FirstOrDefault(u => u.ID == userId)
                                    ?.Company;

            if (company == null)
            {
                // 如果找不到对应的公司，返回错误信息
                return new AllSimpleResumeInfoClass { SimpleResumes = new List<SimpleResume>(), Code = 60204 };
            }

            // 查询数据库，获取所有属于该用户的简历
            var resumeList = _dbContext.Resumes
                           .Include(r => r.Applicant)
                           .ThenInclude(a => a.ApplicantProfile)
                           .ThenInclude(ap => ap.JobMatches)
                           .Where(r => r.CompanyID == company.ID)
                           .ToList();

            // 将每个Applicant对象转换为SimpleResume对象
            var simpleResumeList = resumeList.Select(_resume => new SimpleResume
            {
                Rid = _resume.Applicant.ID,
                Name = _resume.Applicant.Name,
                Age = _resume.Applicant.Age,
                Gender = _resume.Applicant.Gender,
                HighestEducation = _resume.Applicant.HighestEducation,
                PhoneNumber = _resume.Applicant.PhoneNumber,
                JobIntention = _resume.Applicant.JobIntention,
                GraduatedFrom = _resume.Applicant.GraduatedFrom,
                MatchingPosition = _resume.Applicant.ApplicantProfile?.JobMatches
                        ?.OrderByDescending(jm => jm.Score)
                        ?.FirstOrDefault()?.JobTitle ?? "N/A" // Defaults to "N/A" if any value is null

                //MatchingScore = _resume.Applicant.ApplicantProfile?.MatchingScore ?? 0
            }).ToList();
            simpleResumeList.Reverse();
            // 创建AllSimpleResumes对象并返回
            var allSimpleResumeInfo = new AllSimpleResumeInfoClass
            {
                SimpleResumes = simpleResumeList,
                Code = 20000
            };

            return allSimpleResumeInfo;
        }

        public Boolean UpdateApplicant(DetailedResume detailedResume)
        {
            var resume = _dbContext.Resumes.FirstOrDefault(r => r.ID == detailedResume.Id);

            if (resume == null) { return false; }

            var existingApplicant = _dbContext.Applicants
                .Include(a => a.ApplicantProfile)
                .Include(a => a.WorkExperiences)
                .Include(a => a.SkillCertificates)
                .Include(a => a.EducationBackgrounds)
                .Include(a => a.Awards)
                .FirstOrDefault(applicant => applicant.ID == resume.ApplicantID);
            if (existingApplicant == null) { return false; }
            existingApplicant.Name = detailedResume.Name;
            existingApplicant.Email = detailedResume.Email;
            existingApplicant.PhoneNumber = detailedResume.PhoneNumber;
            existingApplicant.Age = detailedResume.Age;
            existingApplicant.Gender = detailedResume.Gender;
            existingApplicant.JobIntention = detailedResume.JobIntention;
            existingApplicant.SelfEvaluation = detailedResume.SelfEvaluation;
            existingApplicant.HighestEducation = detailedResume.HighestEducation;
            existingApplicant.ApplicantProfile.WorkStability = detailedResume.WorkStability;
            existingApplicant.ApplicantProfile.StabilityReason = detailedResume.WorkStabilityReason;
            existingApplicant.ApplicantProfile.MatchingScore = detailedResume.MatchingScore;
            existingApplicant.ApplicantProfile.MatchingReason = detailedResume.MatchingReason;

            // Handle WorkExperiences
            _dbContext.WorkExperiences.RemoveRange(existingApplicant.WorkExperiences); // Remove existing
            existingApplicant.WorkExperiences = detailedResume.WorkExperience.Select(w => new WorkExperience
            {
                ApplicantID = existingApplicant.ID,
                CompanyName = w.CompanyName,
                Position = w.Position,
                Time = w.Time,
                Task = w.Task
            }).ToList();  // Add new

            // Handle SkillCertificates
            _dbContext.SkillCertificates.RemoveRange(existingApplicant.SkillCertificates); // Remove existing
            existingApplicant.SkillCertificates = detailedResume.SkillCertificate.Select(s => new SkillCertificate
            {
                ApplicantID = existingApplicant.ID,
                SkillName = s.SkillName
            }).ToList();  // Add new

            // Handle Awards
            _dbContext.Awards.RemoveRange(existingApplicant.Awards); // Remove existing
            existingApplicant.Awards = detailedResume.Awards.Select(a => new Award
            {
                ApplicantID = existingApplicant.ID,
                AwardName = a.AwardName
            }).ToList();  // Add new
            _dbContext.SaveChanges();

            if (detailedResume.EducationBackgrounds == null)
            {
                detailedResume.EducationBackgrounds = new List<EducationBackground>();
            }
            _dbContext.EducationBackgrounds.RemoveRange(existingApplicant.EducationBackgrounds); // Remove existing
            existingApplicant.EducationBackgrounds = detailedResume.EducationBackgrounds.Select(ed => new EducationBackground
            {
                ApplicantID = existingApplicant.ID,
                Time = ed.Time,
                School = ed.School,
                Major = ed.Major
            }).ToList();  // Add new
            _dbContext.SaveChanges();
            return true;
        }

        private PersonalCharacteristics ExtractPersonalCharacteristics(Dictionary<string, object> data)
        {
            var personalCharacteristics = new PersonalCharacteristics();
            var characteristics = new List<Characteristic>();

            foreach (var pair in data)
            {
                if (pair.Value is Dictionary<string, object> characteristicData)
                {
                    var characteristic = new Characteristic
                    {
                        Name = pair.Key,
                        Score = characteristicData.ContainsKey("分数") ? Convert.ToInt32(characteristicData["分数"]) : 0,
                        Reason = characteristicData.ContainsKey("原因") ? characteristicData["原因"].ToString() : null
                    };
                    characteristics.Add(characteristic);
                }
            }

            personalCharacteristics.Characteristics = characteristics;

            return personalCharacteristics;
        }

        private SkillsAndExperiences ExtractSkillsAndExperiences(Dictionary<string, object> data)
        {
            var skillsAndExperiences = new SkillsAndExperiences();
            var characteristics = new List<Characteristic>();

            foreach (var pair in data)
            {
                if (pair.Value is Dictionary<string, object> characteristicData)
                {
                    var characteristic = new Characteristic
                    {
                        Name = pair.Key,
                        Score = characteristicData.ContainsKey("分数") ? Convert.ToInt32(characteristicData["分数"]) : 0,
                        Reason = characteristicData.ContainsKey("原因") ? characteristicData["原因"].ToString() : null
                    };
                    characteristics.Add(characteristic);
                }
            }

            skillsAndExperiences.Characteristics = characteristics;

            return skillsAndExperiences;
        }

        private AchievementsAndHighlights ExtractAchievementsAndHighlights(Dictionary<string, object> data)
        {
            var achievementsAndHighlights = new AchievementsAndHighlights();
            var characteristics = new List<Characteristic>();

            foreach (var pair in data)
            {
                if (pair.Value is Dictionary<string, object> characteristicData)
                {
                    var characteristic = new Characteristic
                    {
                        Name = pair.Key,
                        Score = characteristicData.ContainsKey("分数") ? Convert.ToInt32(characteristicData["分数"]) : 0,
                        Reason = characteristicData.ContainsKey("原因") ? characteristicData["原因"].ToString() : null
                    };
                    characteristics.Add(characteristic);
                }
            }

            achievementsAndHighlights.Characteristics = characteristics;

            return achievementsAndHighlights;
        }


        public PersonalCharacteristics GetPersonalCharacteristics(int resumeId)
        {
            var applicant = _dbContext.Applicants
                          .Include(a => a.ApplicantProfile)
                          .ThenInclude(ap => ap.PersonalCharacteristics)
                          .ThenInclude(pc => pc.Characteristics)
                          .FirstOrDefault(a => a.ID == resumeId);
            if (applicant == null || applicant.ApplicantProfile == null)
            {
                throw new Exception("Applicant or ApplicantProfile not found");
            }

            return applicant.ApplicantProfile.PersonalCharacteristics;
        }

        public SkillsAndExperiences GetSkillsAndExperiences(int resumeId)
        {
            var applicant = _dbContext.Applicants.Include(a => a.ApplicantProfile)
                                       .ThenInclude(ap => ap.SkillsAndExperiences)
                                       .ThenInclude(pc => pc.Characteristics)
                                       .FirstOrDefault(a => a.ID == resumeId);
            if (applicant == null || applicant.ApplicantProfile == null)
            {
                throw new Exception("Applicant or ApplicantProfile not found");
            }

            return applicant.ApplicantProfile.SkillsAndExperiences;
        }

        public AchievementsAndHighlights GetAchievementsAndHighlights(int resumeId)
        {
            var applicant = _dbContext.Applicants.Include(a => a.ApplicantProfile)
                                       .ThenInclude(ap => ap.AchievementsAndHighlights)
                                       .ThenInclude(pc => pc.Characteristics)
                                       .FirstOrDefault(a => a.ID == resumeId);
            if (applicant == null || applicant.ApplicantProfile == null)
            {
                throw new Exception("Applicant or ApplicantProfile not found");
            }

            return applicant.ApplicantProfile.AchievementsAndHighlights;
        }

        public JobMatchScoresInfoForGraphClass JobMatchScoresInfoForGraph(int userId) 
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

            var jobMatches = _dbContext.JobPositions
                                       .Include(jp => jp.Resumes)
                                       .ThenInclude(r => r.Applicant)
                                       .ThenInclude(a => a.ApplicantProfile)
                                       .ThenInclude(ap => ap.JobMatches)
                                       .Where(jp => jp.CompanyID == company.ID)
                                       .SelectMany(jp => jp.Resumes)
                                       .Select(r => r.Applicant.ApplicantProfile)
                                       .SelectMany(ap => ap.JobMatches)
                                       .ToList();

            var groupedJobMatches = jobMatches.GroupBy(jm => jm.ApplicantProfileID)
                                              .Select(g => g.OrderByDescending(jm => jm.Score).First())
                                              .ToList();

            var scores = new JobMatchScores
            {
                Below60 = groupedJobMatches.Count(jm => jm.Score < 60),
                Range60_70 = groupedJobMatches.Count(jm => jm.Score >= 60 && jm.Score < 70),
                Range70_80 = groupedJobMatches.Count(jm => jm.Score >= 70 && jm.Score < 80),
                Range80_90 = groupedJobMatches.Count(jm => jm.Score >= 80 && jm.Score < 90),
                Range90_100 = groupedJobMatches.Count(jm => jm.Score >= 90 && jm.Score <= 100),
            };

            return new JobMatchScoresInfoForGraphClass { JobMatchScores = scores };
        }

        public Dictionary<string, object> ConvertJObjectsToDictionaries(Dictionary<string, object> data)
        {
            var result = new Dictionary<string, object>();

            foreach (var pair in data)
            {
                if (pair.Value is Newtonsoft.Json.Linq.JObject jObject)
                {
                    result[pair.Key] = ConvertJObjectsToDictionaries(jObject.ToObject<Dictionary<string, object>>());
                }
                else
                {
                    result[pair.Key] = pair.Value;
                }
            }

            return result;
        }
    }
}