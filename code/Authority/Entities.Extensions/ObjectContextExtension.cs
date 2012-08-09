using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Objects.DataClasses;
using System.Data.Objects;

namespace Entities.Extensions
{
    public static class ObjectContextExtension
    {
        public static int DeleteEntity<T>(this ObjectSet<T> entity, Expression<Func<T, bool>> predicate) where T : class
        {
            var table = TableMetadata.GetTableMetadata(entity);
            //查询条件表达式转换成SQL的条件语句
            ConditionBuilder builder = new ConditionBuilder();
            builder.Build(predicate.Body,table);
            string sqlCondition = builder.Condition;
            //获取SQL参数数组 
            var args = builder.Arguments;
            var Result = entity.Context.ExecuteStoreCommand("Delete  From  " + table.Name + " Where " + sqlCondition, args);
            return Result;
        }

        public static int UpdateEntity<T>(this ObjectSet<T> entity, Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updater) where T : class
        {
            var table = TableMetadata.GetTableMetadata(entity);        
            //查询条件表达式转换成SQL的条件语句
            ConditionBuilder builder = new ConditionBuilder();
            builder.Build(predicate.Body,table);
            string sqlCondition = builder.Condition;
            //获取Update的赋值语句
            var updateMemberExpr = (MemberInitExpression)updater.Body;
            var updateMemberCollection = updateMemberExpr.Bindings
                .Cast<MemberAssignment>()
                .Select(c => new
                {
                    Name = table.Properties[c.Member.Name].Name,
                    Value = ((ConstantExpression)c.Expression).Value
                });
            int i = builder.Arguments.Length;
            string sqlUpdateBlock = string.Join(", ", updateMemberCollection.Select(c => string.Format("[{0}]={1}", c.Name, "{" + (i++) + "}")).ToArray());
            //SQL命令
            string commandText = string.Format("UPDATE {0} SET {1} WHERE {2}", table.Name, sqlUpdateBlock, sqlCondition);
            //获取SQL参数数组 (包括查询参数和赋值参数)
            var args = builder.Arguments.Union(updateMemberCollection.Select(c => c.Value)).ToArray();
            var Result = entity.Context.ExecuteStoreCommand(commandText, args);
            return Result;
        }
    }
}
