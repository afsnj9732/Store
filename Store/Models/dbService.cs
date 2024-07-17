using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class dbService
    {
        dbStoreEntities db = new dbStoreEntities();

        public int? GetMemberID(LoginViewModel memberInfo)
        {
            var target = db.tMembers.Where(m => m.Email == memberInfo.Email && m.Password == memberInfo.Password).FirstOrDefault();
            if (target != null)
            {
                return target.MemberID;
            }
            else
            {
                return null;
            }
        }

        public void CreateMember(RegisterViewModel memberInfo)
        {
            tMembers newMember = new tMembers();
            newMember.Email = memberInfo.Email;
            newMember.UserName = memberInfo.UserName;
            newMember.Password = memberInfo.Password;
            db.tMembers.Add(newMember);
            db.SaveChanges();
        }

        public int? GetCartItemQuantity(int loginMemberID)
        {
            //調用View簡化查詢
            int? ItemQuantity = db.vw_GetCartItemQuantity
                .Where(m => m.MemberID == loginMemberID).Select(m => m.TotalQuantity).FirstOrDefault();
            //TotalQuantity是聚合函數顯示的值，默認為int，int的default值是0

            //var targetCart = db.tCart.Where(m => m.MemberID == loginMemberID).FirstOrDefault();
            //int targetItemQuantity = 0;
            //foreach (var item in targetCart.tCartItem)
            //{
            //    targetItemQuantity += item.Quantity;
            //}
            return ItemQuantity;
        }

        public void AddCartItem(int loginMemberID, int addProductID)
        {
            tCart memberCart = db.tCart.Where(m => m.MemberID == loginMemberID).FirstOrDefault();
            //若該會員的購物車不存在，則建立一個
            if (memberCart == null)
            {
                tCart newCart = new tCart();
                newCart.MemberID = loginMemberID;
                db.tCart.Add(newCart);
                db.SaveChanges();

                memberCart = newCart;//將新建立的購物車設值給剛剛查詢的memberCart
            }


            var target = db.tCartItem.Where(m => m.CartID == memberCart.CartID && m.ProductID == addProductID).FirstOrDefault();
            if (target != null)
            {
                //如果商品已經存在登入會員的購物車內
                target.Quantity += 1;
                db.SaveChanges();
            }
            else
            {
                tCartItem newCartItem = new tCartItem();
                newCartItem.ProductID = addProductID;
                newCartItem.CartID = db.tCart.Where(m => m.MemberID == loginMemberID).FirstOrDefault().CartID;
                newCartItem.Quantity = 1;
                db.tCartItem.Add(newCartItem);
                db.SaveChanges();
            }

        }

        public List<ShoppingCartViewModel> MemberShoppingCart(int loginMemberID)
        {
            List<ShoppingCartViewModel> cartItemList = new List<ShoppingCartViewModel>();
            var targetCart = db.tCart.Where(m => m.MemberID == loginMemberID).FirstOrDefault();
            if (targetCart != null)
            {
                List<tCartItem> targetCartList = db.tCartItem.Include(o => o.tProducts).Where(m => m.CartID == targetCart.CartID).ToList();
                //Include預先載入，能避免下面foreach利用導航屬性時每次都重複查詢
                foreach (tCartItem item in targetCartList)
                {
                    ShoppingCartViewModel cart = new ShoppingCartViewModel();
                    cart.CartItemID = item.CartItemID;
                    cart.ProductName = item.tProducts.Name;//利用關聯，使用導航屬性查詢
                    cart.UnitPrice = item.tProducts.Price;
                    cart.Quantity = item.Quantity;
                    cart.TotalPrice = cart.UnitPrice * cart.Quantity;
                    cart.CartID = item.CartID;
                    cartItemList.Add(cart);
                }
            }
            return cartItemList;

        }

        public void DeleteCartItem(int cartItemID)
        {
            var target = db.tCartItem.Where(m => m.CartItemID == cartItemID).FirstOrDefault();
            db.tCartItem.Remove(target);
            db.SaveChanges();
        }

        public List<tOrder> GetOrderList(int loginMemberID)
        {
            return db.tOrder.Where(m => m.MemberID == loginMemberID).ToList();

        }

        public void CreateOrder(int cartID)
        {
            var transaction = db.Database.BeginTransaction();
            //Transaction衝突防止
            try
            {
                var target = db.tCart.Where(m => m.CartID == cartID).FirstOrDefault();
                var targetItems = db.tCartItem.Where(m => m.tCart.CartID == cartID).ToList();

                if (target != null && targetItems.Any())
                {
                    tOrder tempOrder = new tOrder();
                    tempOrder.MemberID = target.MemberID;
                    tempOrder.TotalPrice = 0;
                    tempOrder.OrderDate = DateTime.Now;
                    db.tOrder.Add(tempOrder);
                    db.SaveChanges();
                    //先將訂單建立，再回頭修改TotalPrice
                    //透過訂單的確立，讓識別項的ID自動生成
                    //因為是關聯的關係
                    //所以下面遍歷只要抓上面tempOrder的OrderID就是對應的ID
                    foreach (var item in targetItems)
                    {
                        tOrderItem tempOrderItem = new tOrderItem();
                        tempOrderItem.OrderID = tempOrder.OrderID;
                        tempOrderItem.ProductID = item.ProductID;
                        tempOrderItem.Quantity = item.Quantity;
                        db.tOrderItem.Add(tempOrderItem);
                        tempOrder.TotalPrice += item.tProducts.Price * item.Quantity;
                    }

                    //清空購物車
                    db.tCartItem.RemoveRange(targetItems);
                    db.SaveChanges();
                    transaction.Commit();
                }
            }
            catch
            {
                transaction.Rollback();
            }
            finally
            {
                transaction.Dispose();
            }
        }

    }
}