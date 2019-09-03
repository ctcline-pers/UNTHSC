using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SampleApp.Data;
using SampleApp.Models;

namespace SampleApp.Pages_Customer
{
    [Authorize(Roles="Admin")]
    public class CreateModel : PageModel
    {
        private readonly SampleApp.Data.SampleAppDbContext _context;

        public CreateModel(SampleApp.Data.SampleAppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            PopulateRoles();
            return Page();
        }

        [BindProperty]
        public Customer Customer { get; set; }

        public SelectList RolesList{get; set;}

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Customer.Add(Customer);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private void PopulateRoles()
        {
            var roles = ((WindowsBuiltInRole[])Enum.GetValues(typeof(WindowsBuiltInRole))).ToList().Select(c => c.ToString()).ToList();

            RolesList = new SelectList(roles);
        }
    }
}