﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CartModel
/// </summary>
public class CartModel
{
    public string InsertCart(Cart cart)
    {
        try
        {
            FlowerDBEntities db = new FlowerDBEntities();
            db.Carts.Add(cart);
            db.SaveChanges();

            return cart.DatePurchased + " Order was successfully inserted";
        }
        catch (Exception e)
        {
            return "Error:" + e;
        }
    }

    public string UpdateCart(int id, Cart cart)
    {
        try
        {
            FlowerDBEntities db = new FlowerDBEntities();


            Cart p = db.Carts.Find(id);

            p.DatePurchased = cart.DatePurchased;
            p.ClientId = cart.ClientId;
            p.Amount = cart.Amount;
            p.IsInCart = cart.IsInCart;
            p.ProductId = cart.ProductId;

            db.SaveChanges();
            return cart.DatePurchased + "Order was successfully updated";
        }
        catch (Exception e)
        {
            return "Error:" + e;
        }
    }


    public string DeleteCart(int id)
    {
        try
        {
            FlowerDBEntities db = new FlowerDBEntities();
            Cart cart = db.Carts.Find(id);

            db.Carts.Attach(cart);
            db.Carts.Remove(cart);
            db.SaveChanges();

            return cart.DatePurchased + "Order was successfully deleted";
        }
        catch (Exception e)
        {
            return "Error:" + e;
        }
    }
    public List<Cart> GetOrdersInCart(string userId)
    {
        FlowerDBEntities db = new FlowerDBEntities();
        List<Cart> orders = (from x in db.Carts
                             where x.ClientId == userId
                             && x.IsInCart
                             orderby x.DatePurchased
                             select x).ToList();

        return orders;
    }

    public int GetAmountOfOrders(string userId)
    {
        try
        {
            FlowerDBEntities db = new FlowerDBEntities();
            int amount = (from x in db.Carts
                          where x.ClientId == userId
                          && x.IsInCart
                          select x.Amount).Sum();

            return amount;
        }
        catch
        {
            return 0;
        }
    }

    public void UpdateQuantity(int id, int quantity)
    {
        FlowerDBEntities db = new FlowerDBEntities();
        Cart cart = db.Carts.Find(id);
        cart.Amount = quantity;

        db.SaveChanges();
    }

    public void MarkOrdersAsPaid(List<Cart> carts)
    {
        FlowerDBEntities db = new FlowerDBEntities();

        if (carts != null)
        {
            foreach (Cart cart in carts)
            {
                Cart oldCart = db.Carts.Find(cart.Id);
                oldCart.DatePurchased = DateTime.Now;
                oldCart.IsInCart = false;
            }

            db.SaveChanges();
        }
    }
}

