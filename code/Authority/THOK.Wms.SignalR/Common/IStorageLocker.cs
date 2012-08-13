using System;
using THOK.Wms.DbModel;
namespace THOK.Wms.SignalR.Common
{
    public interface IStorageLocker
    {      
        string LockKey { get; set; }

        Storage LockEmpty(Cell cell);
        Storage LockNoEmpty(Cell cell, Product product);     
        Storage LockStorage(Storage storage, Product product);

        //Storage LockPiece(Cell cell, Product product);
        //Storage LockBar(Cell cell, Product product);

        #region 锁储位，库存，并更新数据库 (用于快速锁定已知目标库记录）

        bool Lock(Storage[] storages);
        void UnLock(Storage[] storages);

        bool Lock(Cell[] cells);
        void UnLock(Cell[] cells);

        #endregion

        #region 锁储位上的库存，但不更新数据库
        
        /// <summary>
        /// 用于选择可入库目标库存记录
        /// </summary>
        /// <param name="cell">目标储位</param>
        /// <returns>目标储位上可入库的库存记录</returns>
        Storage LockStorage(Cell cell);

        /// <summary>
        /// 解锁当前锁锁定的库存记录
        /// </summary>
        /// <param name="storage"></param>
        void UnLockStorage(Storage storage);

        /// <summary>
        /// 用于选择可出库目标库存记录
        /// </summary>
        /// <param name="storage">目标库存记录</param>
        /// <param name="product">要出库的产品</param>
        /// <returns>可出库的库存记录</returns>
        Storage LockNoEmptyStorage(Storage storage, Product product);

        #endregion
    }
}
