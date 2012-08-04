using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.SignalR.Common
{
    public class SyncMoveBillCreater
    {
        [Dependency]
        public IMoveBillMasterRepository MoveBillMasterRepository { get; set; }
        [Dependency]
        public IStorageRepository StorageRepository { get; set; }

        public MoveBillMaster NewMoveBillMaster()
        {
            return null;
        }

        public void Create(MoveBillMaster moveBillMaster)
        {
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();

            //1：主库区 1；2：件烟区 2；
            //3；条烟区 3；4：暂存区 4；
            //5：备货区 0；6：残烟区 0；
            //7：罚烟区 0；8：虚拟区 0；
            //9：其他区 0；

            string[] areaTypes = new string[] { "1" };
            var ss = storageQuery.Where(s => areaTypes.Any(a => a == s.Cell.Area.AreaType));
        }
    }
}
