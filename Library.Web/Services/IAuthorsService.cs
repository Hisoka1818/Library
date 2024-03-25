using Library.Web.Core;
using Library.Web.Data;
using Library.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Services
{
    public interface IAuthorsService
    {
        public Task<Response<List<Author>>> GetListAsyc();
    }

    public class AuthorsService : IAuthorsService  
    {
        private readonly DataContext _context;

        public AuthorsService(DataContext context)
        {
            _context = context;
        }

        public async Task<Response<List<Author>>> GetListAsyc()
        {
            try
            {

                List<Author> list = await _context.Authors.ToListAsync();

                Response<List<Author>> response = new Response<List<Author>>
                {
                    IsSuccess = true,
                    Message = "Lista obtenida con exito",
                    Result = list

                };

                return response;

            }catch (Exception ex)
            {
                return new Response<List<Author>>
                {
                    IsSuccess= false,
                    Message = ex.Message,
                };
            }
        }
    }
}
