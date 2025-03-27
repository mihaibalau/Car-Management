using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LiteDB;
using System.Collections.Generic;

namespace Cars_Management.Pages
{
    public class DisplayCarsModel : PageModel
    {
        public List<Car> Cars { get; set; }
        public int PageSize { get; set; } = 10; 
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public Car NewestCar { get; set; }
        public Car OldestCar { get; set; }

        public void OnGet(int? pageNumber)
        {
            CurrentPage = pageNumber ?? 1;
            LoadCars();
        }

        private void LoadCars()
        {
            using (var db = new LiteDatabase($"{AppDomain.CurrentDomain.BaseDirectory}cars.db"))
            {
                var collection = db.GetCollection<Car>("cars");
                var allCars = collection.FindAll().ToList();
                
                TotalPages = (int)Math.Ceiling(allCars.Count / (double)PageSize);
                Cars = allCars.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

                NewestCar = allCars.OrderByDescending(c => c.ProductionYear).FirstOrDefault();
                OldestCar = allCars.OrderBy(c => c.ProductionYear).FirstOrDefault();
                AverageProductionYear = allCars.Any() ? allCars.Average(c => c.ProductionYear) : 0;
            }
        }

        public void OnPost()
        {
            LoadCars();
        }
        public double AverageProductionYear { get; set; }

        public IActionResult OnPostModify(int id)
        {
            return RedirectToPage("/ModifyCar", new { id = id });
        }

        public IActionResult OnPostDelete(int id)
        {
            using (var db = new LiteDatabase($"{AppDomain.CurrentDomain.BaseDirectory}cars.db"))
            {
                var collection = db.GetCollection<Car>("cars");
                collection.Delete(id);
            }
            return RedirectToPage("/Index");
        }

        public IActionResult OnPostFilter()
        {
            LoadCars();
            Cars = Cars.OrderBy(c => c.ProductionYear).ToList();
            return Page();
        }

    }

}
