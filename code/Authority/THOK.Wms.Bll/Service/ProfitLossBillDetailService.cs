using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Bll.Models;

namespace THOK.Wms.Bll.Service
{
    public class ProfitLossBillDetailService:ServiceBase<ProfitLossBillDetail>,IProfitLossBillDetailService
    {
        [Dependency]
        public IProfitLossBillDetailRepository ProfitLossBillDetailRepository { get; set; }
        [Dependency]
        public IUnitRepository UnitRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IProfitLossBillDetailService 成员

        /// <summary>
        /// 查询损益细单信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="BillNo">损益单号</param>
        /// <returns></returns>
        public object GetDetails(int page, int rows, string BillNo)
        {
            if (BillNo != "" && BillNo != null)
            {
                IQueryable<ProfitLossBillDetail> ProfitLossBillDetailQuery = ProfitLossBillDetailRepository.GetQueryable();
                var profitLossBillDetail = ProfitLossBillDetailQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo).AsEnumerable().Select(i => new
                {
                    i.ID,
                    i.BillNo,
                    i.CellCode,
                    i.StorageCode,
                    i.ProductCode,
                    i.Product.ProductName,
                    i.UnitCode,
                    i.Unit.UnitName,
                    i.Price,
                    Quantity = i.Quantity / i.Unit.Count,
                    i.Description
                });
                int total = profitLossBillDetail.Count();
                profitLossBillDetail = profitLossBillDetail.Skip((page - 1) * rows).Take(rows);
                return new { total, rows = profitLossBillDetail.ToArray() };
            }
            return "";
        }

        /// <summary>
        /// 新增损益单明细
        /// </summary>
        /// <param name="profitLossBillDetail">损益单细表</param>
        /// <returns></returns>
        public new bool Add(ProfitLossBillDetail profitLossBillDetail)
        {
            IQueryable<ProfitLossBillDetail> profitLossBillDetailQuery = ProfitLossBillDetailRepository.GetQueryable();
            var unit = UnitRepository.GetQueryable().FirstOrDefault(u => u.UnitCode == profitLossBillDetail.UnitCode);
            var pbd = new ProfitLossBillDetail();
            pbd.BillNo = profitLossBillDetail.BillNo;
            pbd.CellCode = profitLossBillDetail.CellCode;
            pbd.StorageCode = profitLossBillDetail.StorageCode;
            pbd.ProductCode = profitLossBillDetail.ProductCode;
            pbd.UnitCode = profitLossBillDetail.UnitCode;
            pbd.Price = profitLossBillDetail.Price;
            pbd.Quantity = profitLossBillDetail.Quantity * unit.Count;
            pbd.Description = profitLossBillDetail.Description;

            ProfitLossBillDetailRepository.Add(pbd);
            ProfitLossBillDetailRepository.SaveChanges();
            return true;
        }

        /// <summary>
        /// 删除损益细单
        /// </summary>
        /// <param name="ID">损益单细单ID</param>
        /// <returns></returns>
        public bool Delete(string ID)
        {
            IQueryable<ProfitLossBillDetail> profitLossBillDetailQuery = ProfitLossBillDetailRepository.GetQueryable();
            int intID = Convert.ToInt32(ID);
            var pbd = profitLossBillDetailQuery.FirstOrDefault(i => i.ID == intID);
            ProfitLossBillDetailRepository.Delete(pbd);
            ProfitLossBillDetailRepository.SaveChanges();
            return true;
        }

        /// <summary>
        /// 修改损益细单
        /// </summary>
        /// <param name="profitLossBillDetail">损益细单</param>
        /// <returns></returns>
        public bool Save(ProfitLossBillDetail profitLossBillDetail)
        {
            IQueryable<ProfitLossBillDetail> profitLossBillDetailQuery = ProfitLossBillDetailRepository.GetQueryable();
            var pbd = profitLossBillDetailQuery.FirstOrDefault(i => i.ID == profitLossBillDetail.ID && i.BillNo == profitLossBillDetail.BillNo);
            var unit = UnitRepository.GetQueryable().FirstOrDefault(u => u.UnitCode == profitLossBillDetail.UnitCode);
            pbd.CellCode = profitLossBillDetail.CellCode;
            pbd.StorageCode = profitLossBillDetail.StorageCode;
            pbd.ProductCode = profitLossBillDetail.ProductCode;
            pbd.UnitCode = profitLossBillDetail.UnitCode;
            pbd.Price = profitLossBillDetail.Price;
            pbd.Quantity = profitLossBillDetail.Quantity * unit.Count;
            pbd.Description = profitLossBillDetail.Description;
            ProfitLossBillDetailRepository.SaveChanges();
            return true;
        }

        #endregion
    }
}
