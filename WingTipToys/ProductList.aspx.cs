using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;
using WingTipToys.Models;

namespace WingTipToys
{
    public partial class ProductList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public IQueryable<Product> GetProducts([QueryString("id")] int? aCategoryID )
        {
            var db = new WingTipToys.Models.ProductContext();
            IQueryable<Product> query = db.Products;
            if (aCategoryID.HasValue && aCategoryID > 0)
                query = query.Where(p => p.CategoryID == aCategoryID);
            return query;                                        
        }
    }
}