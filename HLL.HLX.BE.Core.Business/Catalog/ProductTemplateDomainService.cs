using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using HLL.HLX.BE.Core.Model.Catalog;

namespace HLL.HLX.BE.Core.Business.Catalog
{
    public class ProductTemplateDomainService : DomainService
    {
        #region Fields

        private readonly IProductTemplateRepository _productTemplateRepository;
        //private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="productTemplateRepository">Product template repository</param>
        /// <param name="eventPublisher">Event published</param>
        public ProductTemplateDomainService(IProductTemplateRepository productTemplateRepository
            //,IEventPublisher eventPublisher
            )
        {
            this._productTemplateRepository = productTemplateRepository;
            //this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete product template
        /// </summary>
        /// <param name="productTemplate">Product template</param>
        public virtual void DeleteProductTemplate(ProductTemplate productTemplate)
        {
            if (productTemplate == null)
                throw new ArgumentNullException("productTemplate");

            _productTemplateRepository.Delete(productTemplate);

            //event notification
            //_eventPublisher.EntityDeleted(productTemplate);
        }

        /// <summary>
        /// Gets all product templates
        /// </summary>
        /// <returns>Product templates</returns>
        public virtual IList<ProductTemplate> GetAllProductTemplates()
        {
            var query = from pt in _productTemplateRepository.GetAll()
                        orderby pt.DisplayOrder
                        select pt;

            var templates = query.ToList();
            return templates;
        }

        /// <summary>
        /// Gets a product template
        /// </summary>
        /// <param name="productTemplateId">Product template identifier</param>
        /// <returns>Product template</returns>
        public virtual ProductTemplate GetProductTemplateById(int productTemplateId)
        {
            if (productTemplateId == 0)
                return null;

            return _productTemplateRepository.FirstOrDefault(productTemplateId);
        }

        /// <summary>
        /// Inserts product template
        /// </summary>
        /// <param name="productTemplate">Product template</param>
        public virtual void InsertProductTemplate(ProductTemplate productTemplate)
        {
            if (productTemplate == null)
                throw new ArgumentNullException("productTemplate");

            _productTemplateRepository.Insert(productTemplate);

            //event notification
            //_eventPublisher.EntityInserted(productTemplate);
        }

        /// <summary>
        /// Updates the product template
        /// </summary>
        /// <param name="productTemplate">Product template</param>
        public virtual void UpdateProductTemplate(ProductTemplate productTemplate)
        {
            if (productTemplate == null)
                throw new ArgumentNullException("productTemplate");

            _productTemplateRepository.Update(productTemplate);

            //event notification
            //_eventPublisher.EntityUpdated(productTemplate);
        }

        #endregion
    }
}
