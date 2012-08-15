using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Reflection;
using System.Data.Metadata.Edm;
using System.Collections;

namespace Entities.Extensions
{
    public class TableMetadata
    {
        public string Name { get; set; }
        public EntitySet EntitySet { get; set; }
        public Dictionary<string, EdmProperty> Properties { get; set; }
        public static Dictionary<Type, TableMetadata> TableMetadatas = new Dictionary<Type, TableMetadata>();

        public static TableMetadata GetTableMetadata<TEntity>(ObjectSet<TEntity> source) where TEntity : class,new()
        {
            if (GetTableMetadata<TEntity>() != null)
            {
                return GetTableMetadata<TEntity>();
            }
            lock (TableMetadatas)
            {
                if (GetTableMetadata<TEntity>() != null)
                {
                    return GetTableMetadata<TEntity>();
                }
                typeof(ObjectContext).InvokeMember("EnsureMetadata", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, source.Context, null);
                var mapContainer = source.Context.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace)[0];
                var mapSet = mapContainer.GetType().InvokeMember("GetSetMapping", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, mapContainer, new object[] { source.EntitySet.Name });
                var mapType = (mapSet.GetType().InvokeMember("TypeMappings", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance, null, mapSet, null) as IList)[0];
                var map = (mapType.GetType().InvokeMember("MappingFragments", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance, null, mapType, null) as IList)[0];
                var tableMetadata = new TableMetadata();
                tableMetadata.EntitySet = map.GetType().InvokeMember("TableSet", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance, null, map, null) as EntitySet;
                tableMetadata.Name = "[" + tableMetadata.EntitySet.GetType().InvokeMember("Schema", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance, null, tableMetadata.EntitySet, null) as string
                    + "].[" + tableMetadata.EntitySet.GetType().InvokeMember("Table", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance, null, tableMetadata.EntitySet, null) as string
                    + "]";
                tableMetadata.Properties = new Dictionary<string, EdmProperty>();
                PropertyInfo pinfo = null, cpinfo = null;
                foreach (var item in (map.GetType().InvokeMember("Properties", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance, null, map, null) as IEnumerable))
                {
                    if (pinfo == null)
                    {
                        cpinfo = item.GetType().GetProperty("ColumnProperty", BindingFlags.NonPublic | BindingFlags.Instance);
                        pinfo = item.GetType().GetProperty("EdmProperty", BindingFlags.NonPublic | BindingFlags.Instance);
                    }
                    EdmProperty cprop = cpinfo.GetValue(item, null) as EdmProperty;
                    EdmProperty prop = pinfo.GetValue(item, null) as EdmProperty;
                    tableMetadata.Properties.Add(prop.Name, cprop);
                }
                TableMetadatas.Add((new TEntity()).GetType(),tableMetadata);
                return tableMetadata;
            }
        }

        public static TableMetadata GetTableMetadata<TEntity>() where TEntity : class,new()
        {
            if (TableMetadatas.Keys.Contains((new TEntity()).GetType()))
            {
                return TableMetadatas[(new TEntity()).GetType()];
            }
            else
                return null;
        }
    }
}
