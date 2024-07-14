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
        
        public void CreateMember(tMembers memberInfo)
        {
            db.tMembers.Add(memberInfo);
            db.SaveChanges();
        }


        public Boolean CheckMember(LoginModel memberInfo)
        {
            var ifExist = (from member in db.tMembers
                    where memberInfo.Email == member.Email &&
                    memberInfo.Password == member.Password 
                    select  member).FirstOrDefault();
            if(ifExist != null ){return true; }
            else { return false; }

        }

        //[Authorize] 
        public int GetMemberID(LoginModel memberEmail)
        {
            var memberID = db.tMembers.Where(m => m.Email == memberEmail.Email).FirstOrDefault().MemberID;
            return memberID;
        }

        public int GetCartItemQuantity(int loginMemberID)
        {
            var targetCart = db.tCart.Where(m => m.MemberID == loginMemberID).FirstOrDefault();
            int targetItemQuantity = 0;
            foreach(var item in targetCart.tCartItem)
            {
                targetItemQuantity+= item.Quantity;
            }
            return targetItemQuantity;
        }

        public void AddCartItem(int loginMemberID,int addProductID)
        {
            tCart memberCart = db.tCart.Where(m => m.MemberID == loginMemberID).FirstOrDefault();
            //若該會員的購物車不存在，則建立一個
            if (memberCart==null)
            {
                tCart newCart = new tCart();
                newCart.MemberID = loginMemberID;
                db.tCart.Add(newCart);
                db.SaveChanges();
            }
            
            
                //如果商品已經存在登入會員的購物車內
                //找到所屬購物車ID，並比對商品ID
                tCartItem target = db.tCartItem.Where(m => m.CartID == memberCart.CartID && m.ProductID == addProductID).FirstOrDefault();
                if (target == null)
                {
                    tCartItem newCartItem = new tCartItem();
                    newCartItem.ProductID = addProductID;
                    newCartItem.CartID = db.tCart.Where(m => m.MemberID == loginMemberID).FirstOrDefault().CartID;
                    newCartItem.Quantity = 1;
                    db.tCartItem.Add(newCartItem);
                    db.SaveChanges();

                }
                else
                {
                    target.Quantity += 1;
                    db.SaveChanges();
                }
            
        }



        public List<ShoppingCartViewModel> MemberShoppingCart(int memberID)
        {
            List<ShoppingCartViewModel> cartItemList = new List<ShoppingCartViewModel>();
            tCart targetCart = db.tCart.Where(m=>m.MemberID == memberID).FirstOrDefault();
            //利用會員ID找出購物車
            List<tCartItem> targetCartList = db.tCartItem.Where(m=>m.CartID==targetCart.CartID).ToList();
            foreach(tCartItem item in targetCartList)
            {
                ShoppingCartViewModel cart = new ShoppingCartViewModel();
                cart.CartItemID = item.CartItemID;
                cart.ProductName = item.tProducts.Name;//利用關聯，使用導航屬性查詢
                cart.UnitPrice = item.tProducts.Price;
                cart.Quantity = item.Quantity;
                cart.TotalPrice = cart.UnitPrice*cart.Quantity;
                cart.CartID = item.CartID;
                cartItemList.Add(cart);
            }

            return cartItemList;
        }

        public void DeleteCartItem(int CartItemID)
        {
            var target = db.tCartItem.Where(m => m.CartItemID == CartItemID).FirstOrDefault();
            db.tCartItem.Remove(target);
            db.SaveChanges();
        }
        public List<tOrder> GetOrderList(int memberID)
        {
            return db.tOrder.Where(m=>m.MemberID==memberID).ToList();

        }

        public void CreateOrder(int cartID)
        {

            //購物車ID對應的購物車和購物車項目，放入訂單和訂單項目
            var target = db.tCart.Where(m=>m.CartID==cartID).FirstOrDefault();
            var targetItems = db.tCartItem.Where(m => m.tCart.CartID == cartID);

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

                
            }


        }

    }
}