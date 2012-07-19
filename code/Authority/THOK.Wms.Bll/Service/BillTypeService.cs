using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{

    public class BillTypeService : ServiceBase<BillType>, IBillTypeService
    {
        [Dependency]
        public IBillTypeRepository BillTypeRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }


        public object GetDetails(int page, int rows, string BillTypeCode, string BillTypeName, string BillClass, string IsActive)
        {
            IQueryable<BillType> billtypeQuery = BillTypeRepository.GetQueryable();
            var billtype = billtypeQuery.Where(b => b.BillClass == BillClass && b.BillTypeCode.Contains(BillTypeCode) && b.BillTypeName.Contains(BillTypeName) && b.IsActive.Contains(IsActive)).OrderBy(b => b.BillTypeCode).AsEnumerable().Select(b => new { b.BillTypeCode, b.BillTypeName, b.BillClass, b.Description,IsActive = b.IsActive == "1" ? "可用" : "不可用", UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });

            int total = billtype.Count();
            billtype = billtype.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = billtype.ToArray() };
        
        }
        public new bool Add(BillType billtype)
        {
            var bi = new BillType();
            bi.BillTypeCode = billtype.BillTypeCode;
            bi.BillTypeName = billtype.BillTypeName;
            bi.BillClass = billtype.BillClass;
            bi.Description = billtype.Description;
            bi.IsActive = billtype.IsActive;
            bi.UpdateTime = DateTime.Now;

            BillTypeRepository.Add(bi);
            BillTypeRepository.SaveChanges();
            return true;
        }
        public bool Save(BillType billtype)
        {
            var br = BillTypeRepository.GetQueryable().FirstOrDefault(b => b.BillTypeCode == billtype.BillTypeCode);
            br.BillTypeCode = billtype.BillTypeCode;
            br.BillTypeName = billtype.BillTypeName;
            br.BillClass = billtype.BillClass;
            br.Description = billtype.Description;
            br.IsActive = billtype.IsActive;
            br.UpdateTime = DateTime.Now;

            BillTypeRepository.SaveChanges();
            return true;
        }
        public bool Delete(string billtypeCode)
        {
            var billtype = BillTypeRepository.GetQueryable()
                .FirstOrDefault(b => b.BillTypeCode == billtypeCode);
            if (billtypeCode != null)
            {
                BillTypeRepository.Delete(billtype);
                BillTypeRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }
    }
}
