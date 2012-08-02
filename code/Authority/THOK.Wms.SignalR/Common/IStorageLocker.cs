using System;
using THOK.Wms.DbModel;
namespace THOK.Wms.SignalR.Common
{
    public interface IStorageLocker
    {
        Storage LockEmpty(Cell cell);
        string LockKey { get; set; }
        Storage LockNoEmpty(Cell cell,Product product);
    }
}
