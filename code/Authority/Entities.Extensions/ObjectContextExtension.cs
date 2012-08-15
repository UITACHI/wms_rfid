using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using System.Reflection;

namespace Entities.Extensions
{
    public static class ObjectContextExtension
    {
        public static int DeleteEntity<T>(this ObjectSet<T> entity, Expression<Func<T, bool>> predicate) where T : class,new()
        {
            var table = TableMetadata.GetTableMetadata(entity);
            var tmp = entity.Where(predicate) as ObjectQuery<T>;
            string sqlCondition = "DELETE FROM " + table.Name + " WHERE ";
            sqlCondition = sqlCondition + tmp.ToTraceString().Split(new string[] { "WHERE" }, StringSplitOptions.None)[1];
            sqlCondition = sqlCondition.Replace("@", "");
            sqlCondition = sqlCondition.Replace("[Extent1].", "");
            int i = 0;
            foreach (var p in tmp.Parameters)
            {
                sqlCondition = sqlCondition.Replace(p.Name, "{" + i + "}");
            }
            var args = tmp.Parameters.Select(p => p.Value).ToArray();
            var Result = entity.Context.ExecuteStoreCommand(sqlCondition, args);
            return Result;
        }

        public static int UpdateEntity<T>(this ObjectSet<T> entity, Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updater) where T : class,new()
        {
            var table = TableMetadata.GetTableMetadata(entity);
            var tmp = entity.Where(predicate) as ObjectQuery<T>;
            string sqlCondition = tmp.ToTraceString().Split(new string[] { "WHERE" }, StringSplitOptions.None)[1];
            sqlCondition = sqlCondition.Replace("@", "");
            sqlCondition = sqlCondition.Replace("[Extent1].", "");
            
            var updateMemberExpr = (MemberInitExpression)updater.Body;
            var updateMemberCollection = updateMemberExpr.Bindings
                .Cast<MemberAssignment>()
                .Select(c => new
                {
                    Name = table.Properties[c.Member.Name].Name,
                    Value = GetExpressionValue(c.Expression)
                });
            int i = 0;
            string sqlUpdateBlock = string.Join(", ", updateMemberCollection.Select(c => string.Format("[{0}]={1}", c.Name, "{" + (i++) + "}")).ToArray());
            foreach (var p in tmp.Parameters)
            {
                sqlCondition = sqlCondition.Replace(p.Name, "{" + i++ + "}");
            }
            var args = updateMemberCollection.Select(c => c.Value).ToArray();
            var whereArgs = tmp.Parameters.Select(p => p.Value).ToArray();
            args = args.Concat(whereArgs).ToArray();
            string commandText = string.Format("UPDATE {0} SET {1} WHERE {2}", table.Name, sqlUpdateBlock, sqlCondition);
            return entity.Context.ExecuteStoreCommand(commandText, args);           
        }

        private static object GetExpressionValue(Expression expression)
        {
            if (expression is ConstantExpression)
            {
                return ((ConstantExpression)expression).Value;
            }
            else if (expression is MemberExpression)
            {
                if (((MemberExpression)expression).Member is FieldInfo)
	            {
                    return Expression.Lambda(expression).Compile().DynamicInvoke();
	            }
            }
            throw new Exception("传值 Expression 解析错误，详情：" + expression.ToString() + "未能识别！");
        }
    }
}
