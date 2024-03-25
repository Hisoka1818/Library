using AspNetCoreHero.ToastNotification.Abstractions;
using Library.Web.Core;
using Library.Web.Data;
using Library.Web.Data.Entities;
using Library.Web.DTOs;
using Library.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Library.Web.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly DataContext _context;
        private readonly IAuthorsService _authorsService;
        public readonly INotyfService _notifyService;

        public AuthorsController(DataContext context, IAuthorsService authorsService, INotyfService notifyService)
        {
            _context = context;
            _authorsService = authorsService;
            _notifyService = notifyService;
        }

        [HttpGet]

        public async Task<IActionResult> Index()
        {
          
            Response<List<Author>> response = await _authorsService.GetListAsyc();

            if (!response.IsSuccess)
            {
                _notifyService.Error((response.Message));
                return RedirectToAction("Index", "Home");
            }
            _notifyService.Success(response.Message);
            return View(response.Result);
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
