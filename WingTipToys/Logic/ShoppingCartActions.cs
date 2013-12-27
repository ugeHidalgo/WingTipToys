using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WingTipToys.Models;

namespace WingTipToys.Logic
{
    public class ShoppingCartActions
    {
        public string ShoppingCartId { get; set; }
        private ProductContext _myDBContext = new ProductContext();
        public const string CartSessionKey = "CartId";


        public void AddToCart(int id)
        {
            ShoppingCartId = GetCartId();

            var cartItem = _myDBContext.ShoppingCartItems.SingleOrDefault(
                    (c => c.CartId == ShoppingCartId &&  c.ProductId == id  )
                );
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ItemId = Guid.NewGuid().ToString(),
                    CartId = ShoppingCartId,
                    ProductId = id,
                    Product = _myDBContext.Products.SingleOrDefault(
                             p => p.ProductID == id),
                    Quantity = 1,
                    DateCreated = DateTime.Now
                };
                _myDBContext.ShoppingCartItems.Add(cartItem);
            }
            else
            { //The item exists into the cart so add one more
                cartItem.Quantity++;
            }
            _myDBContext.SaveChanges();
        }

        public string GetCartId()
        {
            if (HttpContext.Current.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    HttpContext.Current.Session[CartSessionKey] = HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class.     
                    Guid tempCartId = Guid.NewGuid();
                    HttpContext.Current.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return HttpContext.Current.Session[CartSessionKey].ToString();
        }

        public List<CartItem> GetCartItems()
        {
            ShoppingCartId = GetCartId();
            //List<CartItem> query = myDBContext.CartItems.Where(c => c.CartId == ShoppingCartId).ToList();
            List<CartItem> query = (from cItems in _myDBContext.ShoppingCartItems
             where (cItems.CartId == ShoppingCartId)
             select cItems).ToList();
            return query;

        }


        public decimal GetTotal()
        {
            ShoppingCartId = GetCartId();
            decimal? total = 0;
            total = (decimal?)(from cI in _myDBContext.ShoppingCartItems
                               where cI.CartId == ShoppingCartId
                               select (int?)cI.Quantity * cI.Product.UnitPrice ).Sum();
            return total ?? decimal.Zero;
        }
    }
}