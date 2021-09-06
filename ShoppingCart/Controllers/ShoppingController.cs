using ShoppingCart.Models;
using ShoppingCart.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    public class ShoppingController : Controller
    {
        private ECartDBEntities objECartDbEntities;
        List<ShoppingCartViewModel> listOfShoppingCartModels;
        public ShoppingController()
        {
            objECartDbEntities = new ECartDBEntities();
            listOfShoppingCartModels = new List<ShoppingCartViewModel>();
        }
        // GET: Shopping
        public ActionResult Index()
        {
            IEnumerable<ShoppingViewModel> listofShoppingViewModels = (from objItem in objECartDbEntities.Items join objCat in objECartDbEntities.Categories
                on objItem.CategoryID equals objCat.CategoryID
                select new ShoppingViewModel() 
                {
                    ImagePath = objItem.ImagePath,
                    ItemName = objItem.ItemName,
                    Description = objItem.Description,
                    ItemPrice = objItem.ItemPrice,
                    ItemID = objItem.ItemID,
                    Category = objCat.CategoryName,
                    ItemCode = objItem.ItemCode
                }
                ).ToList();
            return View(listofShoppingViewModels);
        }

        [HttpPost]
        public JsonResult Index(string itemID)
        {
            ShoppingCartViewModel objShoppingViewModel = new ShoppingCartViewModel();
            Item objItem = objECartDbEntities.Items.Single(model => model.ItemID.ToString() == itemID);

            if(Session["CartCounter"] != null)
            {
                listOfShoppingCartModels = Session["CartItem"] as List<ShoppingCartViewModel>;
            }

            if(listOfShoppingCartModels.Any(model => model.ItemID == itemID))
            {
                objShoppingViewModel = listOfShoppingCartModels.Single(model => model.ItemID == itemID);
                objShoppingViewModel.Quantity = objShoppingViewModel.Quantity + 1;
                objShoppingViewModel.Total = objShoppingViewModel.Quantity * objShoppingViewModel.UnitPrice;
            }

            else
            {
                objShoppingViewModel.ItemID = itemID;
                objShoppingViewModel.ImagePath = objItem.ImagePath;
                objShoppingViewModel.ItemName = objItem.ItemName;
                objShoppingViewModel.Quantity = 1;
                objShoppingViewModel.Total = objItem.ItemPrice;
                objShoppingViewModel.UnitPrice = objItem.ItemPrice;
                listOfShoppingCartModels.Add(objShoppingViewModel);
                
            }

            Session["CartCounter"] = listOfShoppingCartModels.Count;
            Session["CartItem"] = listOfShoppingCartModels;


            return Json(new {Success = true, Counter = listOfShoppingCartModels.Count}, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult ShoppingCart()
        {
            listOfShoppingCartModels = Session["CartItem"] as List<ShoppingCartViewModel>;
            return View(listOfShoppingCartModels);
        }

        [HttpPost]
        public ActionResult AddOrder()
        {
            int OrderID = 0;
            listOfShoppingCartModels = Session["CartItem"] as List<ShoppingCartViewModel>;
            Order orderObj = new Order()
            {
                OrderDate = DateTime.Now,
                OrderNumber = String.Format("{0:ddmmyyyyHHmmss}", DateTime.Now)
            };
            objECartDbEntities.Orders.Add(orderObj);
            objECartDbEntities.SaveChanges();

            OrderID = orderObj.OrderID;

            foreach(var item in listOfShoppingCartModels)
            {
                OrderDetail objOrderDetail = new OrderDetail();
                objOrderDetail.Total = item.Total;
                objOrderDetail.ItemID = item.ItemID;
                objOrderDetail.OrderID = OrderID;
                objOrderDetail.Quantity = item.Quantity;
                objOrderDetail.UnitPrice = item.UnitPrice;
                objECartDbEntities.OrderDetails.Add(objOrderDetail);
                objECartDbEntities.SaveChanges();
            }

            Session["CartItem"] = null;
            Session["CartCounter"] = null;
            return RedirectToAction("Index");
        }
    }
}