using System;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Allot.Interfaces;
using System.Linq;

namespace THOK.Wms.Allot.Service
{
    public class InBillAllotService:ServiceBase<InBillAllot>,IInBillAllotService
    {
        [Dependency]
        public IInBillAllotRepository InBillAllotRepository { get; set; }
        [Dependency]
        public IInBillMasterRepository InBillMasterRepository { get; set; }
        [Dependency]
        public IInBillDetailRepository InBillDetailRepository { get; set; }

        [Dependency]
        public IWarehouseRepository WarehouseRepository { get; set; }
        [Dependency]
        public IAreaRepository AreaRepository { get; set; }
        [Dependency]
        public IShelfRepository ShelfRepository { get; set; }
        [Dependency]
        public ICellRepository CellRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool Allot(string billNo, string[] areaCodes)
        {
            IQueryable<InBillMaster> inBillMasterQuery = InBillMasterRepository.GetQueryable();
            IQueryable<Cell> cellQuery = CellRepository.GetQueryable();

            InBillMaster billMaster = inBillMasterQuery.Single(b => b.BillNo == billNo);
            var cells = cellQuery.Where(c => c.WarehouseCode == billMaster.WarehouseCode
                                    && areaCodes.Any(a => a == c.AreaCode)
                                    && c.IsSingle == "1"
                                    && (c.Storage == null
                                        || c.Storage.Any(s => s.IsLock != "1" && s.Quantity == 0 && s.InFrozenQuantity == 0)))
                                 .Select(c => c);

            var billDetails = billMaster.InBillDetails.Where(b => (b.BillQuantity - b.AllotQuantity) > 0);
            var cellArray = cells.ToArray();
            var cellQueryForArray = cellArray.Where(c => c.Storage == null
                                        || c.Storage.Any(s => s.IsLock != "1" && s.Quantity == 0 
                                            && s.InFrozenQuantity == 0));
            foreach (var billDetail in billDetails.ToArray())
            {
                while ((billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    var cell = cellQueryForArray.First();
                    if (cell != null)
                    {
                        decimal allotQuantity = cell.MaxQuantity * billDetail.Product.Unit.COUNT;
                        decimal billQuantity = ((billDetail.BillQuantity - billDetail.AllotQuantity)
                                                    /billDetail.Product.Unit.COUNT)
                                                    *billDetail.Product.Unit.COUNT;
                        allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;

                        billDetail.AllotQuantity += allotQuantity;
                        cell.Storage.Single().InFrozenQuantity = (int)allotQuantity;
                    }
                }               

            }
            return true;
        }


    }
}
