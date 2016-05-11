using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using HLL.HLX.BE.Common.Util;
using HLL.HLX.BE.EntityFramework;

namespace HLL.HLX.BE.Core.Business.Common
{
    public static class GenericAttributeExtensions
    {
        ///// <summary>
        ///// Get an attribute of an entity
        ///// </summary>
        ///// <typeparam name="TPropType">Property type</typeparam>
        ///// <param name="entity">Entity</param>
        ///// <param name="key">Key</param>
        ///// <param name="storeId">Load a value specific for a certain store; pass 0 to load a value shared for all stores</param>
        ///// <returns>Attribute</returns>
        //public static TPropType GetAttribute<TPropType>(this BaseEntity entity, string key, int storeId = 0)
        //{
        //    var genericAttributeService = EngineContext.Current.Resolve<IGenericAttributeService>();
        //    return GetAttribute<TPropType>(entity, key, genericAttributeService, storeId);
        //}

        /// <summary>
        /// Get an attribute of an entity
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="genericAttributeService">GenericAttributeService</param>
        /// <param name="storeId">Load a value specific for a certain store; pass 0 to load a value shared for all stores</param>
        /// <returns>Attribute</returns>
        public static TPropType GetAttribute<TPropType>(this FullAuditedEntity entity,
            string key, GenericAttributeDomianService genericAttributeService, int storeId = 0)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            string keyGroup = entity.GetUnproxiedEntityType().Name;

          return  GetAttribute<TPropType>(entity.Id, keyGroup, key, genericAttributeService, storeId);
        }

        /// <summary>
        /// Get an attribute of an entity
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="genericAttributeService">GenericAttributeService</param>
        /// <param name="storeId">Load a value specific for a certain store; pass 0 to load a value shared for all stores</param>
        /// <returns>Attribute</returns>
        public static TPropType GetAttribute<TPropType>(this FullAuditedEntity<long> entity,
            string key, GenericAttributeDomianService genericAttributeService, int storeId = 0)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            string keyGroup = entity.GetUnproxiedEntityType().Name;

            return GetAttribute<TPropType>((int)entity.Id, keyGroup, key, genericAttributeService, storeId);
        }


        /// <summary>
        /// Get an attribute of an entity
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="genericAttributeService">GenericAttributeService</param>
        /// <param name="storeId">Load a value specific for a certain store; pass 0 to load a value shared for all stores</param>
        /// <returns>Attribute</returns>
        public static TPropType GetAttribute<TPropType>(int entityId, string keyGroup,
            string key, GenericAttributeDomianService genericAttributeService, int storeId = 0)
        {
            //if (entity == null)
            //    throw new ArgumentNullException("entity");

            //string keyGroup = entity.GetUnproxiedEntityType().Name;

            var props = genericAttributeService.GetAttributesForEntity(entityId, keyGroup);
            //little hack here (only for unit testing). we should write ecpect-return rules in unit tests for such cases
            if (props == null)
                return default(TPropType);
            props = props.Where(x => x.StoreId == storeId).ToList();
            if (props.Count == 0)
                return default(TPropType);

            var prop = props.FirstOrDefault(ga =>
                ga.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            if (prop == null || string.IsNullOrEmpty(prop.Value))
                return default(TPropType);

            return CommonUtil.To<TPropType>(prop.Value);
        }
    }
}
