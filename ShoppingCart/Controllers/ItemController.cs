using ShoppingCart.Models;
using ShoppingCart.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    public class ItemController : Controller
    {
        ECartDBEntities objECartEntities;

        public ItemController()
        {
            objECartEntities = new ECartDBEntities();
        }

        public ActionResult Index()
        {
            ItemViewModel objItemViewModel = new ItemViewModel();
            objItemViewModel.CategorySelectListItem = (from objCat in objECartEntities.Categories
                select new SelectListItem()
                {
                    Text = objCat.CategoryName,
                    Value = objCat.CategoryID.ToString(),
                    Selected = true
                });
            return View(objItemViewModel);
        }

        [HttpPost]
        public JsonResult Index(ItemViewModel objItemViewModel)
        {
            string NewImage = Guid.NewGuid() + Path.GetExtension(objItemViewModel.ImagePath.FileName);
            objItemViewModel.ImagePath.SaveAs(Server.MapPath("~/Images/" + NewImage));

            Item objItems = new Item();
            objItems.ImagePath = "~/Images/" + NewImage;
            objItems.CategoryID = objItemViewModel.CategoryID;
            objItems.Description = objItemViewModel.Description;
            objItems.ItemCode = objItemViewModel.ItemCode;
            objItems.ItemID = Guid.NewGuid();
            objItems.ItemName = objItemViewModel.ItemName;
            objItems.ItemPrice = objItemViewModel.ItemPrice;
            objECartEntities.Items.Add(objItems);
            objECartEntities.SaveChanges();

            return Json(new {Success = true, Message = "New Item has been added successfully"}, JsonRequestBehavior.AllowGet);
        } 
    }
}