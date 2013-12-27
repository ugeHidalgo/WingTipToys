using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WingTipToys.Logic;
using WingTipToys.Models;

namespace WingTipToys
{
    public partial class ShoppingCart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ShoppingCartActions userShoppingCart = new ShoppingCartActions();
            decimal carTotal = decimal.Zero;
            carTotal = userShoppingCart.GetTotal();
            lblTotal.Text = String.Format("{0:c}", carTotal);
            if (carTotal <= 0)
            {
                 ShoppingCartTitle.InnerText="Empty cart";
                 UpdateBtn.Visible = false;
            }            
        }

        public List<CartItem> GetShoppingCartItems()
        {
            ShoppingCartActions actions = new ShoppingCartActions();
            return actions.GetCartItems();
        }
    }
}