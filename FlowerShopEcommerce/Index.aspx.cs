using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        FillPage();
    }
    private void FillPage()
    {
        ProductModel productModel = new ProductModel();
        List<Product> products = productModel.GetAllProducts();

        if (products != null)
        {
            foreach (Product product in products)
            {
                Panel productPanel = new Panel();
                ImageButton imagebutton = new ImageButton();
                Label lblName = new Label();
                Label lblPrice = new Label();

                imagebutton.ImageUrl = "~/FlowerImage/Product/" + product.Image;
                imagebutton.CssClass = "productImage";
                imagebutton.PostBackUrl = "~/Pages/Product.aspx?id=" + product.Id;

                lblName.Text = product.Name;
                lblName.CssClass = "productName";

                lblPrice.Text = " ₹ :- " + product.Price;
                lblPrice.CssClass = "productPrice";

                productPanel.Controls.Add(imagebutton);
                productPanel.Controls.Add(new Literal { Text = "<br />" });
                productPanel.Controls.Add(lblName);
                productPanel.Controls.Add(new Literal { Text = "<br />" });
                productPanel.Controls.Add(lblPrice);

                pnlProduct.Controls.Add(productPanel);
            }
        }
        else
        {
            pnlProduct.Controls.Add(new Literal { Text = "No Products found" });
        }
    }
}