using Microsoft.AspNetCore.Mvc;
using Project_PRN.Models;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;

namespace Project_PRN.Controllers
{
    public class ProductController : Controller
    {
        public List<Product> oldPrice()
        {
            using (ProjectPrnContext context = new ProjectPrnContext())
            {
                List<Product> p = context.Products.ToList();

                return p;
            }
        }

        

        public  List<Category> listCategory()
        {
            using (ProjectPrnContext context = new ProjectPrnContext())
            {
                List<Category> c = context.Categories.ToList();


                return c;
            }
        }

        public IActionResult Index()
        {
            using (ProjectPrnContext context = new ProjectPrnContext())
            {
                var data = context.Products.ToList();


                ViewBag.Product = data;

                ViewBag.Category = listCategory();

                return View();
            }
        }

        public IActionResult Order(int id)
        {
            using (ProjectPrnContext context = new ProjectPrnContext())
            {
                Product p = context.Products.FirstOrDefault(item => item.ProductId == id);
                


                if(p.Quantity > 100)
                {
                    double newPrice;

                    newPrice = double.Parse( (@p.ProductPrice * 0.8).ToString());
                    ViewBag.NewPrice = newPrice;
                }

                context.SaveChanges();

                Cart c = context.Carts.FirstOrDefault(item => item.ProductId == id);
                int cartQuantity = 0;
                if (c != null)
                {
                    cartQuantity = c.CartQuantity.Value;
                }
                else
                {
                    cartQuantity = 0;
                }

                ViewBag.CartQuantity = cartQuantity;

                return View("OrderDetail", p);
            }
        }

        public IActionResult AddToCart(int productid, int quantity)
        {
            using (ProjectPrnContext context = new ProjectPrnContext())
            {
                Product p = context.Products.FirstOrDefault(item => item.ProductId == productid);
                context.Products.ToList();

                //get Bill of the Cart
                Cart checkCart = context.Carts.FirstOrDefault(item => item.ProductId == productid);

                if (quantity != 0)
                {
                    if (checkCart == null)
                    {
                        Cart c = new Cart()
                        {
                            ProductId = p.ProductId,
                            CartQuantity = quantity
                        };

                        context.Carts.Add(c);
                        context.SaveChanges();
                    }
                    else
                    {
                        checkCart.CartQuantity += quantity;
                        context.SaveChanges();
                    }
                } else
                {
                    return RedirectToAction("Index");
                }
                


                double Bill = 0.0;
                foreach (Cart cart in context.Carts)
                {
                    Bill += (double)(cart.CartQuantity * cart.Product.ProductPrice);
                }
                ViewBag.Bill = Bill;

                List<Cart> carts = context.Carts.ToList();

                ViewBag.Carts = carts;

                return View("Cart");
            }
        }

        public IActionResult Delete(int cartId)
        {
            using (ProjectPrnContext context = new ProjectPrnContext())
            {
                Cart c = context.Carts.Find(cartId);
                if (c != null)
                {
                    context.Carts.Remove(c);
                    context.SaveChanges();
                }

                context.Products.ToList();

                if (context.Carts.Count() != 0)
                {
                    double Bill = 0.0;
                    foreach (Cart cart in context.Carts)
                    {
                        Bill += (double)(cart.CartQuantity * cart.Product.ProductPrice);
                    }
                    ViewBag.Bill = Bill;

                    List<Cart> carts = context.Carts.ToList();
                    ViewBag.Carts = carts;

                    return View("Cart");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
        }

        public IActionResult CheckOut()
        {
            using (ProjectPrnContext context = new ProjectPrnContext())
            {
                //cap nhat lai quantity cho product
                List<Product> products = context.Products.ToList();
                List<Cart> carts = context.Carts.ToList();



                foreach (Product p in products)
                {
                    foreach (Cart c in carts)
                    {
                        if (c.ProductId == p.ProductId)
                        {
                            p.Quantity -= c.CartQuantity;
                        }
                    }
                }

                context.Carts.RemoveRange(carts.ToArray());
                context.SaveChanges();
                return View("Order");
            }
        }

        public IActionResult Searching(string productName)
        {
            using (ProjectPrnContext context = new ProjectPrnContext())
            {
                if (productName != null)
                {
                    List<Product> products = context.Products.Where(item => item.ProductName.Contains(productName)).ToList();
                    ViewBag.Product = products;
                } else
                {
                    List<Product> products = context.Products.ToList();
                    ViewBag.Product = products;
                }

                ViewBag.Category = listCategory();
                return View("Index");
            }
        }

        public IActionResult Category(int categoryId)
        {
            using (ProjectPrnContext context = new ProjectPrnContext())
            {
                ViewBag.CategoryId = categoryId;

                if (categoryId != 0)
                {
                    List<Product> products = context.Products.Where(item => item.CategoryId == categoryId).ToList();
                    ViewBag.Product = products;
                } else
                {
                    List<Product> products = context.Products.ToList();
                    ViewBag.Product = products;
                }
                
                ViewBag.Category = listCategory();
                
                return View("Index");
            }
        }

        public IActionResult SortProduct(int sortChoose)
        {
            using (ProjectPrnContext context = new ProjectPrnContext())
            {
                ViewBag.Category = listCategory();

                if (sortChoose == 1)
                {
                    var data = context.Products.OrderBy(o => o.ProductPrice).ToList();
                    ViewBag.Product = data;
                } else if (sortChoose == 2)
                {
                    var data = context.Products.OrderByDescending(o => o.ProductPrice).ToList();
                    ViewBag.Product = data;
                } else
                {
                    var data = context.Products.ToList();
                    ViewBag.Product = data;
                }
                

                return View("Index");
            }
        }
    }
}
