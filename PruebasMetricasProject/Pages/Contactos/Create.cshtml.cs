using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PruebasMetricasProject.Data;
using PruebasMetricasProject.Models;

namespace PruebasMetricasProject.Pages.Contactos
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Contacto Contacto { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Contactos.Add(Contacto);

            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
