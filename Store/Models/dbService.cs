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

        //[Authorize] private
        public int GetMemberID(LoginModel memberEmail)
        {
            var memberID = db.tMembers.Where(m => m.Email == memberEmail.Email).FirstOrDefault().MemberID;
            return memberID;
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
                cart.ProductName = item.tProducts.Name;//利用關聯查詢
                cart.UnitPrice = item.tProducts.Price;
                cart.Quantity = item.Quantity;
                cart.TotalPrice = cart.UnitPrice*cart.Quantity;
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
    }
}