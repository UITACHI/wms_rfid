using System;
using THOK.Wms.DbModel;
namespace THOK.Wms.SignalR.Common
{
    public interface IStorageLocker
    {      
        string LockKey { get; set; }
        Storage LockEmpty(Cell cell);
        Storage LockNoEmpty(Cell cell,Product product);
        Storage LockBar(Cell cell, Product product);
        Storage LockNoEmptyStorage(Storage s, Product product);
    }
}
