using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq.Expressions;
using System.Data.Objects;

namespace THOK.Common
{
    public static class Obj
    {
        public static TEntity Clone<TEntity>(this TEntity o) where TEntity : class
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, o);
            stream.Position = 0;
            return (TEntity)formatter.Deserialize(stream);
        }
    }
}
