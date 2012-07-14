using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using THOK.Ef.Common.MappingStrategy.Strategies;

namespace THOK.Common.Ef.MappingStrategy
{
    public class EntityMappingBase<TEntity> : EntityTypeConfiguration<TEntity>, IMapping
        where TEntity : class
    {
        protected static readonly Lazy<IMappingStrategy<string>> TableNameMappingStrategy =
            new Lazy<IMappingStrategy<string>>(() => new CompositeMappingStrategy<string>
            {
                Strategies = new IMappingStrategy<string>[]
                        {
                            new AddUnderscoresBetweenWordsMappingStrategy(),
                            new ToLowerMappingStrategy()
                        }
            });

        protected static readonly Lazy<IMappingStrategy<string>> AddUnderscoresBetweenWordsThenToLowerMappingStrategy =
            new Lazy<IMappingStrategy<string>>(() => new CompositeMappingStrategy<string>
            {
                Strategies = new IMappingStrategy<string>[]
                {
                    new AddUnderscoresBetweenWordsMappingStrategy(),
                    new ToLowerMappingStrategy()
                }
            });

        protected static readonly Lazy<IMappingStrategy<string>> ColumnMap =
            new Lazy<IMappingStrategy<string>>(() => new CompositeMappingStrategy<string>
            {
                Strategies = new List<IMappingStrategy<string>>(new[]
                {
                    AddUnderscoresBetweenWordsThenToLowerMappingStrategy.Value
                })
            });

        public EntityMappingBase()
            : this(null)
        {

        }

        public EntityMappingBase(string tableNamePrefix)
        {
            if (!string.IsNullOrEmpty(tableNamePrefix))
            {
                Lazy<IMappingStrategy<string>> tableNameMappingStrategy =
                new Lazy<IMappingStrategy<string>>(() => new CompositeMappingStrategy<string>
                {
                    Strategies = new IMappingStrategy<string>[]
                            {
                                new AddPrefixMappingStrategy(tableNamePrefix),
                                AddUnderscoresBetweenWordsThenToLowerMappingStrategy.Value
                            }
                });
                EntityMapping(tableNameMappingStrategy.Value);
            }
            else
            {
                EntityMapping(TableNameMappingStrategy.Value);
            }
        }

        public void EntityMapping(IMappingStrategy<string> tableNameMappingStrategy)
        {
            if (tableNameMappingStrategy != null)
            {
                ToTable(tableNameMappingStrategy.To(typeof(TEntity).Name));
            }
        }

        #region Implementation of IMapping

        public void RegistTo(ConfigurationRegistrar configurationRegistrar)
        {
            configurationRegistrar.Add(this);
        }

        #endregion
    }
}