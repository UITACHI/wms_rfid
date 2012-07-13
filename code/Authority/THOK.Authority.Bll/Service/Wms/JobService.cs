using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.RfidWms.DBModel.Ef.Models.Wms;
using THOK.Authority.Bll.Interfaces.Wms;
using Microsoft.Practices.Unity;
using THOK.Authority.Dal.Interfaces.Wms;

namespace THOK.Authority.Bll.Service.Wms
{
    public class JobService : ServiceBase<Job>, IJobService
    {
        [Dependency]
        public IJobRepository JobRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IJobService 成员

        public object GetDetails(int page, int rows, string JobCode, string JobName, string IsActive)
        {
            IQueryable<Job> jobQuery = JobRepository.GetQueryable();
            var job = jobQuery.Where(j => j.JobCode.Contains(JobCode) && j.JobName.Contains(JobName)).OrderBy(j => j.JobCode).AsEnumerable().Select(j => new { j.ID, j.JobCode, j.JobName, j.Description, IsActive = j.IsActive == "1" ? "可用" : "不可用", UpdateTime = j.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            if (!IsActive.Equals(""))
            {
                job = jobQuery.Where(j => j.JobCode.Contains(JobCode) && j.JobName.Contains(JobName) && j.IsActive.Contains(IsActive)).OrderBy(j => j.JobCode).AsEnumerable().Select(j => new { j.ID, j.JobCode, j.JobName, j.Description, IsActive = j.IsActive == "1" ? "可用" : "不可用", UpdateTime = j.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            }
            int total = job.Count();
            job = job.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = job.ToArray() };

        }

        public bool Add(Job job)
        {
            var jo = new Job();
            jo.ID = Guid.NewGuid();
            jo.JobCode = job.JobCode;
            jo.JobName = job.JobName;
            jo.Description = job.Description;
            jo.IsActive = job.IsActive;
            jo.UpdateTime = DateTime.Now;

            JobRepository.Add(jo);
            JobRepository.SaveChanges();
            return true;
        }

        public bool Delete(string JobId)
        {
            Guid joId = new Guid(JobId);
            var job = JobRepository.GetQueryable()
                .FirstOrDefault(j => j.ID == joId);
            if (job != null)
            {
                JobRepository.Delete(job);
                JobRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(Job job)
        {
            var jo = JobRepository.GetQueryable().FirstOrDefault(j => j.ID == job.ID);
            jo.JobCode = job.JobCode;
            jo.JobName = job.JobName;
            jo.Description = job.Description;
            jo.IsActive = job.IsActive;
            jo.UpdateTime = DateTime.Now;

            JobRepository.SaveChanges();
            return true;
        }

        #endregion
        
    }
}
