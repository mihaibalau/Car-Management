using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LiteDB;
using Cars_Management.Pages;  // Asigură-te că acest namespace este corect

namespace Cars_Management.Pages
{
    public class ModifyCarModel : PageModel
    {
        [BindProperty]
        public Car Car { get; set; }

        public IActionResult OnGet(int id)
        {
            using (var db = new LiteDatabase($"{AppDomain.CurrentDomain.BaseDirectory}cars.db"))
            {
                var collection = db.GetCollection<Car>("cars");
                Car = collection.FindById(id);

                if (Car == null)
                {
                    return NotFound();
                }
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var db = new LiteDatabase($"{AppDomain.CurrentDomain.BaseDirectory}cars.db"))
            {
                var collection = db.GetCollection<Car>("cars");
                collection.Update(Car);
            }

            return RedirectToPage("/Index");
        }
    }
}
