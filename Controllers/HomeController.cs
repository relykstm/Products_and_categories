using System;
using Microsoft.AspNetCore.Mvc;
using Products_and_Catagories.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace Products_and_Catagories.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]

        public ViewResult Index()
        {
            ViewBag.AllProducts = dbContext.Products;
            return View(); 
        }

        [HttpGet("categories")]

        public ViewResult Categories()
        {
            ViewBag.AllCategories = dbContext.Categories;
            return View();
        }

        [HttpPost("newcategorie")]

        public IActionResult NewCategory(Category fromForm)
        {
            if(ModelState.IsValid){
                dbContext.Add(fromForm);
                dbContext.SaveChanges();
                return RedirectToAction("Categories");
            }
            else
            {
                return RedirectToAction("Categories");
            }
        }

        [HttpPost("newproduct")]

        public IActionResult NewProduct(Product fromForm)
        {
            if(ModelState.IsValid){
                dbContext.Add(fromForm);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        [HttpGet("product/{ProductId}")]
        public IActionResult ProductPage(int ProductId)
        {

            // do a query for the product
            
            Product ThisProduct = dbContext.Products
                .Include(p => p.Categories)
                .ThenInclude(ca => ca.Category)
                .FirstOrDefault(p=> p.ProductId == ProductId);

            // creates a blank list for products
            List<Category> thisProductsCategories = new List<Category>();
            List<Category> NotInCategories = new List<Category>();
            //creates a list of all caterories to pull from 
            List<Category> AllCategories = dbContext.Categories.ToList();
            

            //uses manytomany relationship to add products to blank list
            foreach(var category in ThisProduct.Categories)
            {
                thisProductsCategories.Add(category.Category);
            }

            //adds categories to list if they are not in our products list 
            foreach(var category in AllCategories)
            {
                bool isIncluded = false;
                foreach(var included in ThisProduct.Categories)
                {
                    if (category.Name == included.Category.Name)
                    {
                        isIncluded = true;
                    }
                }
                if (isIncluded == false)
                {
                    NotInCategories.Add(category);
                }
            }
            // assigns our variables to viewBags
            ViewBag.thisProduct = ThisProduct;
            ViewBag.thisProductsCategories = thisProductsCategories;
            ViewBag.NotInCategories = NotInCategories;
                
            return View();

            
        }

        [HttpPost("newassociation")]

        public IActionResult NewAssociation(int ProductId, Association fromForm)
        {
            fromForm.ProductId = ProductId;
            if(ModelState.IsValid)
            {
                dbContext.Add(fromForm);
                dbContext.SaveChanges();
                return RedirectToAction("ProductPage", new {productId = ProductId } );
            }
            else
            {
               return RedirectToAction("ProductPage", new {productId = ProductId } );
            }
        }

        [HttpGet("category/{CategoryId}")]
        public IActionResult CategoryPage(int CategoryId)
        {
            
            Category ThisCategory = dbContext.Categories
                .Include(c => c.ProductAssociations)
                .ThenInclude(ca => ca.Product)
                .FirstOrDefault(p=> p.CategoryId == CategoryId);
            
            List<Product> thisCategoriesProduct = new List<Product>();
            List<Product> AllProducts = dbContext.Products.ToList();
            List<Product> NotInProducts = new List<Product>();
        
            
            foreach(var product in ThisCategory.ProductAssociations)
            {
                thisCategoriesProduct.Add(product.Product);
            }

            foreach(var product in AllProducts)
            {
                bool isIncluded = false;
                foreach(var included in ThisCategory.ProductAssociations)
                {
                    if (product.Name == included.Product.Name)
                    {
                        isIncluded = true;
                    }
                }
                if (isIncluded == false)
                {
                    NotInProducts.Add(product);
                }
            }

            ViewBag.ThisCategory = ThisCategory;
            ViewBag.thisCategoriesProduct = thisCategoriesProduct;
            ViewBag.NotInProducts = NotInProducts;
                
            return View();
        }

        [HttpPost("newassociationbycat")]

        public IActionResult NewAssociationByCategory(int CategoryId, Association fromForm)
        {
            fromForm.CategoryId = CategoryId;
            if(ModelState.IsValid)
            {
                dbContext.Add(fromForm);
                dbContext.SaveChanges();
                return RedirectToAction("CategoryPage", new {CategoryId = CategoryId } );
            }
            else
            {
               return RedirectToAction("CategoryPage", new {CategoryId = CategoryId } );
            }
        }

    }
}