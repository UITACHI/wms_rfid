using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Authority.Bll.Interfaces.Authority;
using THOK.Authority.Dal.EntityModels;
using Microsoft.Practices.Unity;
using THOK.Authority.Dal.Interfaces.Authority;

namespace THOK.Authority.Bll.Service.Authority
{
    public class FunctionService :ServiceBase<Function>, IFunctionService
    {
        [Dependency]
        public IFunctionRepository FunctionRepository { get; set; }
        [Dependency]
        public IModuleRepository ModuleRepository { get; set; }

        #region IFunctionService 成员

        public bool Save(string FunctionId, string FunctionName, string ControlName, string IndicateImage)
        {
            try
            {
                Guid fid = new Guid(FunctionId);
                var function = FunctionRepository.GetQueryable().FirstOrDefault(m => m.FunctionID == fid);
                function.FunctionName = FunctionName;
                function.ControlName = ControlName;
                function.IndicateImage = IndicateImage;
                FunctionRepository.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Delete(string FunctionId)
        {
            try
            {
                Guid fid = new Guid(FunctionId);
                var function = FunctionRepository.GetQueryable().FirstOrDefault(f => f.FunctionID == fid);
                if (function != null)
                {
                    FunctionRepository.Delete(function);
                    FunctionRepository.SaveChanges();
                }
                else
                    return false;
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Add(string ModuleId, string FunctionName, string ControlName, string IndicateImage)
        {
            try
            {
                var module = ModuleRepository.GetQueryable().FirstOrDefault(m => m.ModuleID == new Guid(ModuleId));
                var function = new Function();
                function.FunctionID = Guid.NewGuid();
                function.FunctionName = FunctionName;
                function.ControlName = ControlName;
                function.Module = module;
                function.IndicateImage = IndicateImage;
                FunctionRepository.Add(function);
                FunctionRepository.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region IFunctionService 成员

        public object GetDetails(string ModuleId)
        {
            Guid mid = new Guid(ModuleId);
            var function = FunctionRepository.GetQueryable().Where(f => f.Module.ModuleID == mid).Select(f => new
            {
                f.FunctionID,
                f.FunctionName,
                f.ControlName,
                f.IndicateImage
            });
            return function.ToArray();
        }

        #endregion
        
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }
    }
}
