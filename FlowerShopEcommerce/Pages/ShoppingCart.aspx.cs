using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;

public partial class Pages_ShoppingCart : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string userID = User.Identity.GetUserId();
        GetPurchasesInCart(userID);
    }

    private void GetPurchasesInCart(string userId)
    {
        CartModel model = new CartModel();
        double subTotal = 0;

        List<Cart> purchaseList = model.GetOrdersInCart(userId);
        CreateShopTable(purchaseList, out subTotal);

        double vat = subTotal * 0.21;
        double totalAmount = subTotal + vat + 15;

        litTotal.Text = " ₹ " + subTotal;
        litVat.Text = " ₹ " + vat;
        litTotalAmount.Text = " ₹ " + subTotal;
    }

    private void CreateShopTable(List<Cart> purchaseList, out double subTotal)
    {
        subTotal = new Double();

        ProductModel model = new ProductModel();

        foreach (Cart cart in purchaseList)
        {
            Product product = model.GetProduct(cart.ProductId);
            ImageButton btnImage = new ImageButton
            {
                ImageUrl = string.Format("~/FlowerImage/Product/{0}", product.Image),
                PostBackUrl = string.Format("~/Pages/Product.aspx?id={0}", product.Id)
            };

            LinkButton lnkDelete = new LinkButton
            {
                PostBackUrl = string.Format("~/Pages/ShoppingCart.aspx?productId={0}", cart.Id),
                Text = "Delete Item",
                ID = "del" + cart.Id
            };

            lnkDelete.Click += Delete_Item;

            int[] amount = Enumerable.Range(1, 20).ToArray();
            DropDownList ddlAmount = new DropDownList
            {
                DataSource = amount,
                AppendDataBoundItems = true,
                AutoPostBack = true,
                ID = cart.Id.ToString()
            };

            ddlAmount.DataBind();
            ddlAmount.SelectedValue = cart.Amount.ToString();
            ddlAmount.SelectedIndexChanged += ddlAmount_SelectedIndexChanged;

            Table table = new Table { CssClass = "CartTable" };
            TableRow a = new TableRow();
            TableRow b = new TableRow();

            TableCell a1 = new TableCell { RowSpan = 2, Width = 50 };
            TableCell a2 = new TableCell
            {
                Text = string.Format("<h4>{0}</h4><br/>{1}<br/>In Stock",
                product.Name, "Item No: " + product.Id),
                HorizontalAlign = HorizontalAlign.Left,
                Width = 350,
            };
            TableCell a3 = new TableCell { Text = "Unit Price<hr/>" };
            TableCell a4 = new TableCell { Text = "Quantity<hr/>" };
            TableCell a5 = new TableCell { Text = "Item Total<hr/>" };
            TableCell a6 = new TableCell { };

            TableCell b1 = new TableCell { };
            TableCell b2 = new TableCell { Text = " ₹ " + product.Price };
            TableCell b3 = new TableCell { };
            TableCell b4 = new TableCell { /*Text = "₹ " +  Math.Round((cart.Amount * product.Price), 2) */};
            TableCell b5 = new TableCell { };

            a1.Controls.Add(btnImage);
            a6.Controls.Add(lnkDelete);
            b3.Controls.Add(ddlAmount);

            a.Cells.Add(a1);
            a.Cells.Add(a2);
            a.Cells.Add(a3);
            a.Cells.Add(a4);
            a.Cells.Add(a5);
            a.Cells.Add(a6);

            b.Cells.Add(b1);
            b.Cells.Add(b2);
            b.Cells.Add(b3);
            b.Cells.Add(b4);
            b.Cells.Add(b5);
            b.Cells.Add(b5);

            table.Rows.Add(a);
            table.Rows.Add(b);

            pnlShoppingCart.Controls.Add(table);

            /*subTotal += (cart.Amount * product.Price);*/
        }

        Session[User.Identity.GetUserId()] = purchaseList;
    }

    private void Delete_Item(object sender, EventArgs e)
    {
        LinkButton selectedlink = (LinkButton)sender;
        string link = selectedlink.ID.Replace("del", "");
        int cartId = Convert.ToInt32(link);

        var CartModel = new CartModel();
        CartModel.DeleteCart(cartId);

        Response.Redirect("~/Pages/ShoppingCart.aspx");
    }
    private void ddlAmount_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList selectedList = (DropDownList)sender;
        int quantity = Convert.ToInt32(selectedList.SelectedValue);
        int cartId = Convert.ToInt32(selectedList.ID);

        CartModel CartModel = new CartModel();
        CartModel.UpdateQuantity(cartId, quantity);
        Response.Redirect("~/Pages/ShoppingCart.aspx");
    }
    private string GeneratePaypalButton(double subTotal)
    {
        string paypal = string.Format(
            @"<script async='async' src='https://www.paypalobjects.com/js/external/paypal-button.min.js?merchant=flowerseller@gmail.com'
                date-button='buynow'
                data-name='Flower Purchases'
                data-quantity=1
                data-amount='{0}'
                data-tax='{1}'
                data-shipping='15'
                data-callback='http://localhost:64903/Pages/Success.aspx'
                data-sendback='http://localhost:64903/Pages/Success.aspx'
                data-env='sandbox'>
            </script>", subTotal, (subTotal * 0.21));

        return paypal;
    }
}