using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UserInfoModel
/// </summary>
public class UserInfoModel
{
    public UserInformation GetUserInformation(string guId)
    {
        FlowerDBEntities db = new FlowerDBEntities();
        UserInformation info = (from x in db.UserInformations
                                where x.GUID == guId
                                select x).FirstOrDefault();

        return info;
    }

    public void InsertUserInformation(UserInformation info)
    {
        FlowerDBEntities db = new FlowerDBEntities();
        db.UserInformations.Add(info);
        db.SaveChanges();
    }
}