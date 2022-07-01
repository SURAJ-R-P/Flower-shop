using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProductTypeModel
/// </summary>
public class ProductTypeModel
{
    public string InsertProductType(ProductType productType)
    {
        try
        {
            FlowerDBEntities db = new FlowerDBEntities();
            db.ProductTypes.Add(productType);
            db.SaveChanges();

            return productType.Name + " was suscessfully inserted";
        }
        catch (Exception e)
        {
            return "Error:" + e;
        }
    }

    public string UpdateProductType(int id, ProductType productType)
    {
        try
        {
            FlowerDBEntities db = new FlowerDBEntities();


            ProductType p = db.ProductTypes.Find(id);

            p.Name = productType.Name;

            db.SaveChanges();
            return productType.Name + " was successfully updated";

        }
        catch (Exception e)
        {
            return "Error" + e;
        }
    }

    public string DeleteProductType(int id)
    {
        try
        {
            FlowerDBEntities db = new FlowerDBEntities();
            ProductType productType = db.ProductTypes.Find(id);

            db.ProductTypes.Attach(productType);
            db.ProductTypes.Remove(productType);
            db.SaveChanges();

            return productType.Name + " was successfully deleted";
        }
        catch (Exception e)
        {
            return "Error:" + e;
        }
    }
}