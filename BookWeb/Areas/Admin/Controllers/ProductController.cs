using Book.DataAccess.Data;
using Book.Models;
using Book.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Book.DataAccess.Repository;
using Book.Models.ViewModels;

namespace BookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;
		private readonly ICategoryRepository _categoryRepo;
		public ProductController(IProductRepository productRepo, ICategoryRepository categoryRepo)
        {
			_productRepo = productRepo;
			_categoryRepo = categoryRepo;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _productRepo.GetAll().ToList();

            


			return View(objProductList);
        }



        public IActionResult Create()
        {
            ProductVM productVM = new()
            {
                CategoryList = _categoryRepo.GetAll()
			   .Select(u => new SelectListItem
			   {
				   Text = u.Name,
				   Value = u.Id.ToString()
			   }),
                Product = new Product()
            };

            return View(productVM);
			//IEnumerable<SelectListItem> CategoryList = _categoryRepo.GetAll()
			//   .Select(u => new SelectListItem
			//   {
			//	   Text = u.Name,
			//	   Value = u.Id.ToString()
			//   });

			//         ViewData["CategoryList"] = CategoryList;
			//         ProductVM productVM = new()
			//         {
			//             CategoryList = CategoryList,
			//             Product = new Product()
			//         };
		}

		[HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                _productRepo.Add(productVM.Product); //Vad som ska läggas till
                _productRepo.Save();      //Skapar kategorin i databasen
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            else
            {
				productVM.CategoryList = _categoryRepo.GetAll()
               .Select(u => new SelectListItem
               {
                   Text = u.Name,
                   Value = u.Id.ToString()
               });
                return View(productVM);
            }
         }
            
        


        public IActionResult Edit(int? id) //hämtar in id
        {
            if (id == null || id == 0) //Om id är tomt eller noll.
            {
                return NotFound();
            }
            //Om id inte är tomt eller noll, hämta id för vald product
            Product productFromDb = _productRepo.Get(u => u.Id == id);
            if (productFromDb == null)//Om den hämtade kategorin är tom, visa NotFound.
            {
                return NotFound();
            }
            return View(productFromDb); //Skickar kategorin till View
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _productRepo.Update(obj); //Vad som ska läggas till
                _productRepo.Save();      //Skapar kategorin i databasen
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Delete(int? id) //hämtar in id
        {
            if (id == null || id == 0) //Om id är tomt eller noll.
            {
                return NotFound();
            }
            //Om id inte är tomt eller noll, hämta id för vald product
            Product productFromDb = _productRepo.Get(u => u.Id == id);
            if (productFromDb == null)//Om den hämtade kategorin är tom, visa NotFound.
            {
                return NotFound();
            }
            return View(productFromDb); //Skickar kategorin till View
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {

            Product obj = _productRepo.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _productRepo.Remove(obj);
            _productRepo.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");


        }
    }
}
