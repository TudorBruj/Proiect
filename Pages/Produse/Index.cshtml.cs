using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Proiect.Data;
using Proiect.Models;

namespace Proiect.Pages.Produse
{
    public class IndexModel : PageModel
    {
        private readonly Proiect.Data.ProiectContext _context;

        public IndexModel(Proiect.Data.ProiectContext context)
        {
            _context = context;
        }

        public IList<Produs> Produs { get; set; } = new List<Produs>();

        public int Id { get; set; }
        public string Nume { get; set; }
        public string Descriere { get; set; }

        public string NumeSort { get; set; }
        public string CurrentFilter { get; set; }

        public async Task OnGetAsync(int? id, string sortOrder, string searchString)
        {
            NumeSort = String.IsNullOrEmpty(sortOrder) ? "nume_desc" : "";

            CurrentFilter = searchString;
            IQueryable<Produs> produseQuery = _context.Produs
                .AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                produseQuery = produseQuery.Where(s => s.Nume.Contains(searchString) || s.Descriere.Contains(searchString));
            }

            Produs = await produseQuery
                .OrderBy(b => b.Nume)
                .AsNoTracking()
                .ToListAsync();

            if (id != null)
            {
                Id = id.Value;
                Produs produs = Produs.FirstOrDefault(i => i.Id == id.Value);

                if (produs != null)
                {
                    Nume = produs.Nume;
                    Descriere = produs.Descriere;
                }
            }
        }
    }
}
