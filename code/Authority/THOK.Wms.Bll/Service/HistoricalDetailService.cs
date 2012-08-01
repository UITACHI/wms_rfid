using System;
using System.Linq;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class HistoricalDetailService : IHistoricalDetailService
    {
        [Dependency]
        public IInBillDetailRepository InBillDetailRepository { get; set; }
        [Dependency]
        public IOutBillDetailRepository OutBillDetailRepository { get; set; }

        //protected override Type LogPrefix
        //{
        //    get { return this.GetType(); }
        //}

        #region IHistoricalDetailService 成员

        public object GetDetails(int page, int rows, string warehouseCode, string productCode, string beginDate, string endDate)
        {
            var inQuery = InBillDetailRepository.GetQueryable().AsEnumerable();
            var outQuery = OutBillDetailRepository.GetQueryable().AsEnumerable();
            
            var Allquery = inQuery.Select(a => new
            {
                BillDate=a.InBillMaster.BillDate.ToString("yyyy-MM-dd"),
                a.InBillMaster.Warehouse.WarehouseCode,
                a.InBillMaster.Warehouse.WarehouseName,
                a.BillNo,
                a.InBillMaster.BillType.BillTypeCode,
                a.InBillMaster.BillType.BillTypeName,
                a.ProductCode,
                a.Product.ProductName,
                a.RealQuantity,
                a.Unit.UnitName
            }).Union(outQuery.Select(a => new
            {
                BillDate = a.OutBillMaster.BillDate.ToString("yyyy-MM-dd"),
                a.OutBillMaster.Warehouse.WarehouseCode,
                a.OutBillMaster.Warehouse.WarehouseName,
                a.BillNo,
                a.OutBillMaster.BillType.BillTypeCode,
                a.OutBillMaster.BillType.BillTypeName,
                a.ProductCode,
                a.Product.ProductName,
                a.RealQuantity,
                a.Unit.UnitName
            }));
            var query = Allquery.Where(i => i.ProductCode.Contains(productCode) 
                                         && i.WarehouseCode.Contains(warehouseCode)
                                       ).OrderBy(i =>i.BillDate).OrderBy(i =>i.WarehouseName
                                       ).AsEnumerable().Select(i => new
            {
                i.BillDate,
                i.WarehouseCode,
                i.WarehouseName,
                i.BillNo,
                i.BillTypeCode,
                i.BillTypeName,
                i.ProductCode,
                i.ProductName,
                i.RealQuantity,
                i.UnitName

            });
            if (!beginDate.Equals(string.Empty))
            {
                DateTime begin = Convert.ToDateTime(beginDate);
                query = query.Where(i => Convert.ToDateTime(i.BillDate) >= begin);
            }

            if (!endDate.Equals(string.Empty))
            {
                DateTime end = Convert.ToDateTime(endDate);
                query = query.Where(i => Convert.ToDateTime(i.BillDate) <= end);
            }
            int total = query.Count();
            query = query.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = query.ToArray() };
        }

        #endregion
    }
}
