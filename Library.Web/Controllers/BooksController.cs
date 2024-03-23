using Library.Web.Data.Entities;
using Library.Web.Data;
using Library.Web.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly DataContext _context;

        public BooksController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> Index()
        {
            IEnumerable<Book> list = await _context.Books.Include(b => b.Author).ToListAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            BookDTO bookDTO = new BookDTO
            {
                Authors = await _context.Authors.Select(a => new SelectListItem
                {
                    Text = a.FullName,
                    Value = a.Id.ToString(),
                }).ToArrayAsync(),
            };
            return View(bookDTO);

        }

        [HttpPost]
        public async Task<IActionResult> Create(BookDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    dto.Authors = await _context.Authors.Select(a => new SelectListItem
                    {
                        Text = a.FullName,
                        Value = a.Id.ToString(),
                    }).ToArrayAsync();
                    return View(dto);
                }
                Book book = new Book
                {
                    Editorial = dto.Editorial,
                    Description = dto.Description,
                    PublishDate = dto.PublishDate,
                    Title = dto.Title,
                    Author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == dto.AuthorId)
                };

                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            try
            {
                Book? book = await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);

                if (book is null)
                {
                    return RedirectToAction(nameof(Index));
                }


                BookDTO dto = new BookDTO
                {
                    Id = id,
                    AuthorId = book.Author.Id,
                    Description = book.Description,
                    Editorial = book.Editorial,
                    PublishDate = book.PublishDate,
                    Title = book.Title,
                    Authors = await _context.Authors.Select(a => new SelectListItem
                    {
                        Text = a.FullName,
                        Value = a.Id.ToString(),
                    }).ToArrayAsync(),
                };

                return View(dto);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BookDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    dto.Authors = await _context.Authors.Select(a => new SelectListItem
                    {
                        Text = a.FullName,
                        Value = a.Id.ToString(),
                    }).ToArrayAsync();
                    return View(dto);
                }
                Book book = await _context.Books.FirstOrDefaultAsync(a => a.Id == dto.Id);

                if (book is null)
                {
                    return NotFound();
                }

                book.PublishDate = dto.PublishDate;
                book.Title = dto.Title;
                book.Author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == dto.AuthorId);
                book.Description = dto.Description;
                book.Editorial = dto.Editorial;

                _context.Books.Update(book);
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
                Book book= await _context.Books.FirstOrDefaultAsync(a => a.Id == id);

                if (book is null)
                {
                    return RedirectToAction(nameof(Index));
                }


                _context.Books.Remove(book);
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
