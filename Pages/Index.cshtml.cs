using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LiteDB;
using System.Collections.Generic;

namespace Cars_Management.Pages
{
public class DisplayCarsModel : PageModel
{
    public List<Car> Cars { get; set; }

    public void OnGet()
    {
        LoadCars();
    }

    public void OnPost()
    {
        LoadCars();
    }

    private void LoadCars()
    {
        using (var db = new LiteDatabase($"{AppDomain.CurrentDomain.BaseDirectory}cars.db"))
        {
            var collection = db.GetCollection<Car>("cars");
            Cars = collection.FindAll()?.ToList() ?? new List<Car>();
        }
    }

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
