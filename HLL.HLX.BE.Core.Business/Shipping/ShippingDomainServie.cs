using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Services;
using Abp.Runtime.Caching;
using Abp.UI;
using HLL.HLX.BE.Core.Model.Catalog;
using HLL.HLX.BE.Core.Model.Shipping;

namespace HLL.HLX.BE.Core.Business.Shipping
{
    public class ShippingDomainServie : DomainService
    {
        #region Fields
        private readonly IDeliveryDateRepository _deliveryDateRepository;        
        #endregion

        #region Ctor
        public ShippingDomainServie(IDeliveryDateRepository deliveryDateRepository)
        {
            _deliveryDateRepository = deliveryDateRepository;            
        }
        #endregion


        #region Methods

        #region Delivery dates
        /// <summary>
        /// Deletes a delivery date
        /// </summary>
        /// <param name="deliveryDate">The delivery date</param>
        public virtual void DeleteDeliveryDate(DeliveryDate deliveryDate)
        {
            if (deliveryDate == null)
                throw new UserFriendlyException("deliveryDate不存在");

            _deliveryDateRepository.Delete(deliveryDate);

            //event notification
            //_eventPublisher.EntityDeleted(deliveryDate);
        }

        /// <summary>
        /// Gets a delivery date
        /// </summary>
        /// <param name="deliveryDateId">The delivery date identifier</param>
        /// <returns>Delivery date</returns>
        public virtual DeliveryDate GetDeliveryDateById(int deliveryDateId)
        {
            if (deliveryDateId == 0)
                return null;

            return _deliveryDateRepository.FirstOrDefault(deliveryDateId);
        }

        /// <summary>
        /// Gets all delivery dates
        /// </summary>
        /// <returns>Delivery dates</returns>
        public virtual IList<DeliveryDate> GetAllDeliveryDates()
        {
            var query = from dd in _deliveryDateRepository.GetAll()
                        orderby dd.DisplayOrder
                        select dd;
            var deliveryDates = query.ToList();
            return deliveryDates;
        }

        /// <summary>
        /// Inserts a delivery date
        /// </summary>
        /// <param name="deliveryDate">Delivery date</param>
        public virtual void InsertDeliveryDate(DeliveryDate deliveryDate)
        {
            if (deliveryDate == null)
                throw new UserFriendlyException("deliveryDate不存在");

            _deliveryDateRepository.Insert(deliveryDate);

            //event notification
            //_eventPublisher.EntityInserted(deliveryDate);
        }

        /// <summary>
        /// Updates the delivery date
        /// </summary>
        /// <param name="deliveryDate">Delivery date</param>
        public virtual void UpdateDeliveryDate(DeliveryDate deliveryDate)
        {
            if (deliveryDate == null)
                throw new UserFriendlyException("deliveryDate不存在");

            _deliveryDateRepository.Update(deliveryDate);

            //event notification
            //_eventPublisher.EntityUpdated(deliveryDate);
        }
        #endregion

        #endregion
    }
}
