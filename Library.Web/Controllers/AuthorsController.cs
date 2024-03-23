using Library.Web.Data;
using Library.Web.Data.Entities;
using Library.Web.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Library.Web.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly DataContext _context;

        public AuthorsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> Index()
        {
            IEnumerable<Author> list = await _context.Authors.ToListAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(AuthorDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(dto);
                }
                Author author = new Author()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                };

                await _context.Authors.AddAsync(author);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute]int id)
        {
            try
            {
                Author autor = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);

                if (autor is null)
                {
                    return RedirectToAction(nameof(Index));
                }


                AuthorDTO dto = new AuthorDTO()
                {
                    Id = id,
                    FirstName = autor.FirstName,
                    LastName = autor.LastName,
                };

                return View(dto);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AuthorDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(dto);
                }
                Author author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == dto.Id);

                if (author is null)
                {
                    return NotFound();
                }
                author.FirstName = dto.FirstName;
                author.LastName = dto.LastName;

                _context.Authors.Update(author);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }

        }


        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                Author autor = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);

                if (autor is null)
                {
                    return RedirectToAction(nameof(Index));
                }


                _context.Authors.Remove(autor);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
