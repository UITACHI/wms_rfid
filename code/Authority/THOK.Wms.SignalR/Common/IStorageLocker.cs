using System;
using THOK.Wms.DbModel;
namespace THOK.Wms.SignalR.Common
{
    public interface IStorageLocker
    {      
        string LockKey { get; set; }

        Storage LockEmpty(Cell cell);
        Storage LockNoEmpty(Cell cell,Product product);
        Storage LockNoEmptyStorage(Storage storage, Product product);

        Storage LockPiece(Cell cell, Product product);
        Storage LockBar(Cell cell, Product product);
        Storage LockStorage(Storage storage, Product product);

        bool Lock(Storage[] storages);
        void UnLock(Storage[] storages);

        bool Lock(Cell[] cells);
        void UnLock(Cell[] cells);

        Storage LockStorage(Cell cell);
        void UnLockStorage(Storage storage);
    }
}
