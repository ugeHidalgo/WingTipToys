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
        private ProductContext myDBContext = new ProductContext();
        public const string CartSessionKey = "CartId";


        public void AddToCart(int id)
        {
            ShoppingCartId = GetCartId();

            var cartItem = myDBContext.CartItems.SingleOrDefault(
                    (c => c.CartId == ShoppingCartId &&  c.ProductId == id  )
                );
            if (cartItem == null)
            {
                cartItem = new CartItem();
                cartItem.ItemId = Guid.NewGuid().ToString();
                cartItem.ProductId = id;
                cartItem.Product = myDBContext.Products.SingleOrDefault(
                     p => p.ProductID == id);
                cartItem.Quantity = 1;
                cartItem.DateCreated = DateTime.Now;

                myDBContext.CartItems.Add(cartItem);
            }
            else
            { //The item exists into the cart so add one more
                cartItem.Quantity++;
            }
            myDBContext.SaveChanges();
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
            List<CartItem> query = myDBContext.CartItems.Where(c => c.CartId == ShoppingCartId).ToList();
                //(from cItems in myDBContext.CartItems
                //                    where (cItems.CartId == ShoppingCartId)
                //                    select cItems).ToList();
            return query;

        }
    }
}