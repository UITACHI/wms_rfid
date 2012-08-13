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
                        if (storage != null)
                        {
                            if (string.IsNullOrEmpty(storage.LockTag)) { storage.LockTag = this.LockKey; }
                            else storage = null;
                        }
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

        public Storage LockPiece(Cell cell, Product product)
        {
            Storage storage = null;
            if (Lock(cell))
            {
                try
                {
                    if (cell.Storages.Count == 1)
                    {
                        storage = cell.Storages.Where(s => (s.ProductCode == product.ProductCode 
                                                            && (s.Cell.MaxQuantity * product.Unit.Count 
                                                                - s.Quantity - s.InFrozenQuantity
                                                                + s.OutFrozenQuantity) > 0)
                                                        || string.IsNullOrEmpty(s.ProductCode)
                                                        || (s.Quantity == 0 && s.InFrozenQuantity == 0))
                                               .FirstOrDefault();
                        if (storage != null) {
                            if (string.IsNullOrEmpty(storage.LockTag)){storage.LockTag = this.LockKey;}
                            else storage = null;
                        }
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

        public Storage LockStorage(Storage storage, Product product)
        {
            var cell = storage.Cell;
            if (Lock(cell))
            {
                if (string.IsNullOrEmpty(storage.LockTag) && storage.ProductCode == product.ProductCode)
                {
                    try
                    {
                        storage.LockTag = this.LockKey;
                        StorageRepository.SaveChanges();
                    }
                    catch (Exception)
                    {
                        if (storage != null) { StorageRepository.Detach(storage); }
                        storage = null;
                    }
                }
                else
                {
                    storage = null;
                }
            }
            else
            {
                storage = null;
            }
            UnLock(cell);
            return storage;
        }

        private bool Lock(Cell cell)
        {
            try
            {
                if (string.IsNullOrEmpty(cell.LockTag))
                {
                    cell.LockTag = this.LockKey;
                    CellRepository.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
                
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



        #region 锁储位，库存，并更新数据库 (用于快速锁定已知目标库记录）

        public bool Lock(Storage[] storages)
        {
            if (storages.All(s => string.IsNullOrEmpty(s.LockTag)))
            {
                try
                {
                    storages.AsParallel().ForAll(s => s.LockTag = this.LockKey);
                    StorageRepository.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void UnLock(Storage[] storages)
        {
            try
            {
                storages.AsParallel().ForAll(s => s.LockTag = string.Empty);
                StorageRepository.SaveChanges();
            }
            catch (Exception)
            {
                return;
            }
        }

        public bool Lock(Cell[] cells)
        {
            if (cells.All(c => string.IsNullOrEmpty(c.LockTag)))
            {
                try
                {
                    cells.AsParallel().ForAll(c => c.LockTag = this.LockKey);
                    StorageRepository.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void UnLock(Cell[] cells)
        {
            try
            {
                cells.AsParallel().ForAll(c => c.LockTag = string.Empty);
                CellRepository.SaveChanges();
            }
            catch (Exception)
            {
                return;
            }
        }

        #endregion

        #region 锁储位上的库存，但不更新数据库        
        
        /// <summary>
        /// 用于选择可入库目标库存记录
        /// </summary>
        /// <param name="cell">目标储位</param>
        /// <returns>目标储位上可入库的库存记录</returns>
        public Storage LockStorage(Cell cell)
        {
            try
            {
                if (string.IsNullOrEmpty(cell.LockTag))
                {
                    Storage storage = null;
                    if (cell.Storages.Any())
                    {                        
                        if (cell.IsSingle == "1")
                        {
                            storage = cell.Storages.Single();
                            if (string.IsNullOrEmpty(storage.LockTag))
                            {
                                return storage;
                            }
                            else
                                return null;
                        }
                        else
                        {
                            storage = cell.Storages.FirstOrDefault(s=>string.IsNullOrEmpty(s.LockTag)
                                                                      && s.Quantity == 0
                                                                      && s.InFrozenQuantity == 0);

                            if (storage != null)
                            {
                                return storage;
                            }
                            else if (cell.Storages.Count < cell.MaxPalletQuantity)
                            {
                                storage = new Storage()
                                {
                                    StorageCode = Guid.NewGuid().ToString(),
                                    CellCode = cell.CellCode,
                                    IsLock = "0",
                                    IsActive = "0",
                                    StorageTime = DateTime.Now,
                                    UpdateTime = DateTime.Now
                                };
                                cell.Storages.Add(storage);
                                return storage;
                            }
                            else
                                return null;
                        }
                    }
                    else
                    {
                        storage = new Storage()
                        {
                            StorageCode = Guid.NewGuid().ToString(),
                            CellCode = cell.CellCode,
                            IsLock = "0",
                            IsActive = "0",
                            StorageTime = DateTime.Now,
                            UpdateTime = DateTime.Now
                        };
                        cell.Storages.Add(storage);
                        return storage;
                    }
                }
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 解锁当前锁锁定的库存记录
        /// </summary>
        /// <param name="storage"></param>
        public void UnLockStorage(Storage storage)
        {
            if (storage.LockTag == this.LockKey)
            {
                storage.LockTag = string.Empty;
            }
        }

        /// <summary>
        /// 用于选择可出库目标库存记录
        /// </summary>
        /// <param name="storage">目标库存记录</param>
        /// <param name="product">要出库的产品</param>
        /// <returns>可出库的库存记录</returns>
        public Storage LockNoEmptyStorage(Storage storage, Product product)
        {
            try
            {
                if (storage != null
                    && string.IsNullOrEmpty(storage.LockTag)
                    && storage.ProductCode == product.ProductCode
                    && storage.Quantity - storage.OutFrozenQuantity > 0)
                {
                    return storage;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

    }
}
