using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using HLL.HLX.BE.Common;
using HLL.HLX.BE.Core.Model.Vendors;

namespace HLL.HLX.BE.Core.Business.Vendors
{
    /// <summary>
    /// Vendor service
    /// </summary>
    public class VendorDomainService : DomainService
    {
        #region Fields

        private readonly IVendorRepository _vendorRepository;
        private readonly IVendorNoteRepository _vendorNoteRepository;
        //private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="vendorRepository">Vendor repository</param>
        /// <param name="vendorNoteRepository">Vendor note repository</param>
        /// <param name="eventPublisher">Event published</param>
        public VendorDomainService(IVendorRepository vendorRepository,
            IVendorNoteRepository vendorNoteRepository
            //,IEventPublisher eventPublisher
            )
        {
            this._vendorRepository = vendorRepository;
            this._vendorNoteRepository = vendorNoteRepository;
            //this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a vendor by vendor identifier
        /// </summary>
        /// <param name="vendorId">Vendor identifier</param>
        /// <returns>Vendor</returns>
        public virtual Vendor GetVendorById(int vendorId)
        {
            if (vendorId == 0)
                return null;

            return _vendorRepository.FirstOrDefault(vendorId);
        }

        /// <summary>
        /// Delete a vendor
        /// </summary>
        /// <param name="vendor">Vendor</param>
        public virtual void DeleteVendor(Vendor vendor)
        {
            if (vendor == null)
                throw new UserFriendlyException("vendor不存在");

            vendor.Deleted = true;
            UpdateVendor(vendor);
        }

        /// <summary>
        /// Gets all vendors
        /// </summary>
        /// <param name="name">Vendor name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Vendors</returns>
        public virtual IPagedList<Vendor> GetAllVendors(string name = "",
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _vendorRepository.GetAll();
            if (!String.IsNullOrWhiteSpace(name))
                query = query.Where(v => v.Name.Contains(name));
            if (!showHidden)
                query = query.Where(v => v.Active);
            query = query.Where(v => !v.Deleted);
            query = query.OrderBy(v => v.DisplayOrder).ThenBy(v => v.Name);

            var vendors = new PagedList<Vendor>(query, pageIndex, pageSize);
            return vendors;
        }

        /// <summary>
        /// Inserts a vendor
        /// </summary>
        /// <param name="vendor">Vendor</param>
        public virtual void InsertVendor(Vendor vendor)
        {
            if (vendor == null)
                throw new UserFriendlyException("vendor不存在");

            _vendorRepository.Insert(vendor);

            //event notification
            //_eventPublisher.EntityInserted(vendor);
        }

        /// <summary>
        /// Updates the vendor
        /// </summary>
        /// <param name="vendor">Vendor</param>
        public virtual void UpdateVendor(Vendor vendor)
        {
            if (vendor == null)
                throw new UserFriendlyException("vendor不存在");

            _vendorRepository.Update(vendor);

            //event notification
            //_eventPublisher.EntityUpdated(vendor);
        }



        /// <summary>
        /// Gets a vendor note note
        /// </summary>
        /// <param name="vendorNoteId">The vendor note identifier</param>
        /// <returns>Vendor note</returns>
        public virtual VendorNote GetVendorNoteById(int vendorNoteId)
        {
            if (vendorNoteId == 0)
                return null;

            return _vendorNoteRepository.FirstOrDefault(vendorNoteId);
        }

        /// <summary>
        /// Deletes a vendor note
        /// </summary>
        /// <param name="vendorNote">The vendor note</param>
        public virtual void DeleteVendorNote(VendorNote vendorNote)
        {
            if (vendorNote == null)
                throw new UserFriendlyException("vendor不存在");

            _vendorNoteRepository.Delete(vendorNote);

            //event notification
            //_eventPublisher.EntityDeleted(vendorNote);
        }

        #endregion
    }
}
