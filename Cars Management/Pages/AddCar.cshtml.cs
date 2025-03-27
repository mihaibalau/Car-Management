using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using LiteDB;


namespace Cars_Management.Pages
{

    public class Car
    {
        public int Id { get; set; }
        public string CarBrand { get; set; }
        public string Model { get; set; }
        public int ProductionYear { get; set; }
        public string Engine { get; set; }
        public string Transmission { get; set; }
        public string FuelType { get; set; }
        public string Photos { get; set; }
        public string Options { get; set; }
    }


    public class AddCarModel : PageModel
    {
        private readonly ILogger<AddCarModel > _logger;

        public AddCarModel (ILogger<AddCarModel > logger)
        {
            _logger = logger;
        }

        [BindProperty]
        [Required(ErrorMessage = "Car brand is required.")]
        public string CarBrand { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Model is required.")]
        public string Model { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Production year is required.")]
        [Range(1886, 2025, ErrorMessage = "Production year must be between 1886 and 2025.")]
        public int? ProductionYear { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Engine is required.")]
        public string Engine { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Transmission is required.")]
        public string Transmission { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Fuel type is required.")]
        [StringLength(20, ErrorMessage = "Fuel type cannot be longer than 20 characters.")]
        public string FuelType { get; set; }

        [BindProperty]
        public string Photos { get; set; }

        [BindProperty]
        public string Options { get; set; }

        public List<string> ValidationErrors { get; set; } = new List<string>();

    public IActionResult OnPostAddCar()
    {
        if (!ModelState.IsValid)
        {
            ValidationErrors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return Page();
        }

        using (var db = new LiteDatabase($"{AppDomain.CurrentDomain.BaseDirectory}cars.db"))
        {
            var collection = db.GetCollection<Car>("cars");

            var car = new Car
            {
                CarBrand = CarBrand,
                Model = Model,
                ProductionYear = ProductionYear.Value,
                Engine = Engine,
                Transmission = Transmission,
                FuelType = FuelType,
                Photos = Photos,
                Options = Options
            };

            collection.Insert(car);
        }

        _logger.LogInformation("Car added: {CarBrand}, {Model}, {ProductionYear}, {Engine}, {Transmission}, {FuelType}, Photos: {Photos}, Options: {Options}",
            CarBrand, Model, ProductionYear, Engine, Transmission, FuelType, Photos, Options);

        return RedirectToPage("/Index", new
        {
            CarBrand,
            Model,
            ProductionYear,
            Engine,
            Transmission,
            FuelType,
            Photos,
            Options
        });
    }


        public IActionResult OnPostReturn()
        {
            return RedirectToPage("/Index");
        }
    }
}
