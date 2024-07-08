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
            if(ifExist != null ) { return true; }
            else { return false; }

        }
 

    }
}