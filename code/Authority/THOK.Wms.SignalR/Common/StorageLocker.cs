using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.SignalR.Common
{
    public class StorageLocker : THOK.Wms.SignalR.Common.IStorageLocker
    {
        [Dependency]
        public ICellRepository CellRepository { get; set; }
        [Dependency]
        public IStorageRepository StorageRepository { get; set; }

        public string LockKey {get;set;}
        
        public Storage LockEmpty(Cell cell)
        {
            Storage storage = null;
            if (Lock(cell))
            {
                try
                {
                    if (cell.IsSingle == "1")
                    {
                        if (cell.Storages.Count == 0)
                        {
                            storage = new Storage()
                            {
                                StorageCode = Guid.NewGuid().ToString(),
                                CellCode = cell.CellCode,
                                IsLock = "0",
                                LockTag = this.LockKey,
                                IsActive = "0",
                                StorageTime = DateTime.Now,
                                UpdateTime = DateTime.Now
                            };
                            cell.Storages.Add(storage);
                        }
                        else if (cell.Storages.Count == 1)
                        {
                            storage = cell.Storages.Where(s => string.IsNullOrEmpty(s.LockTag)
                                                          && s.Quantity == 0
                                                          && s.InFrozenQuantity == 0)
                                                   .FirstOrDefault();
                            if (storage!=null) { storage.LockTag = this.LockKey; }
                        }
                    }
                    else
                    {
                        storage = cell.Storages.Where(s => string.IsNullOrEmpty(s.LockTag)
                                                    && s.Quantity == 0
                                                    && s.InFrozenQuantity == 0)
                                               .FirstOrDefault();
                        if (storage != null)
                        {
                            storage.LockTag = this.LockKey;
                        }
                        else
                        {
                            storage = new Storage()
                            {
                                StorageCode = Guid.NewGuid().ToString(),
                                CellCode = cell.CellCode,
                                IsLock = "0",
                                LockTag = this.LockKey,
                                IsActive = "0",
                                StorageTime = DateTime.Now,
                                UpdateTime = DateTime.Now
                            };
                            cell.Storages.Add(storage);
                        }
                    }
                    StorageRepository.SaveChanges();
                }
                catch (Exception)
                {
                    if (storage != null) { StorageRepository.Detach(storage); }
                    storage = null;
                }
            }
            UnLock(cell);
            return storage;
        }

        public Storage LockNoEmpty(Cell cell,Product product)
        {
            Storage storage = null;
            if (Lock(cell))
            {
                try
                {
                    storage = cell.Storages.Where(s => s.ProductCode == product.ProductCode 
                                                  && s.Quantity - s.OutFrozenQuantity > 0)
                                           .FirstOrDefault();
                    if (storage != null)
                    {
                        storage.LockTag = this.LockKey;
                        StorageRepository.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    if (storage != null) { StorageRepository.Detach(storage); }
                    storage = null;
                }
            }
            UnLock(cell);
            return storage;
        }

        public Storage LockBar(Cell cell, Product product)
        {
            Storage storage = null;
            if (Lock(cell))
            {
                try
                {
                    if (cell.Storages.Count == 1)
                    {
                        storage = cell.Storages.Where(s => s.ProductCode == product.ProductCode
                                                        || string.IsNullOrEmpty(s.ProductCode))
                                               .FirstOrDefault();
                        if (storage != null) { storage.LockTag = this.LockKey; }
                    }
                    else if (cell.Storages.Count == 0)
                    {
                        storage = new Storage()
                        {
                            StorageCode = Guid.NewGuid().ToString(),
                            CellCode = cell.CellCode,
                            IsLock = "0",
                            LockTag = this.LockKey,
                            IsActive = "0",
                            StorageTime = DateTime.Now,
                            UpdateTime = DateTime.Now
                        };
                        cell.Storages.Add(storage);
                    }
                    StorageRepository.SaveChanges();
                }
                catch (Exception)
                {
                    if (storage != null) { StorageRepository.Detach(storage); }
                    storage = null;
                }
            }
            UnLock(cell);
            return storage;
        }

        private bool Lock(Cell cell)
        {
            try
            {
                cell.LockTag = this.LockKey;
                CellRepository.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                CellRepository.Detach(cell);
                return false;
            }
        }

        private void UnLock(Cell cell)
        {
            try
            {
                cell.LockTag = string.Empty;
                CellRepository.SaveChanges();
            }
            catch (Exception)
            {
                CellRepository.Detach(cell);
            }
        }


    }
}
