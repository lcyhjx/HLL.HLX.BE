using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.UI;
using HLL.HLX.BE.Core.Model.Stores;

namespace HLL.HLX.BE.Core.Business.Stores
{
    /// <summary>
    /// Store context for web application
    /// </summary>
    public partial class WebStoreContext : IStoreContext
    {
        private readonly StoreDomainService _storeService;
        //private readonly IWebHelper _webHelper;

        private Store _cachedStore;

        public WebStoreContext(StoreDomainService storeService
            //, IWebHelper webHelper
            )
        {
            this._storeService = storeService;
            //this._webHelper = webHelper;
        }

        /// <summary>
        /// Gets or sets the current store
        /// </summary>
        public virtual Store CurrentStore
        {
            get
            {
                if (_cachedStore != null)
                    return _cachedStore;

                //ty to determine the current store by HTTP_HOST
                //var host = _webHelper.ServerVariables("HTTP_HOST");
                var host = ".com";
                var allStores = _storeService.GetAllStores();
                var store = allStores.FirstOrDefault(s => s.ContainsHostValue(host));

                if (store == null)
                {
                    //load the first found store
                    store = allStores.FirstOrDefault();
                }
                if (store == null)
                    throw new UserFriendlyException("No store could be loaded");

                _cachedStore = store;
                return _cachedStore;
            }
        }
    }
}
