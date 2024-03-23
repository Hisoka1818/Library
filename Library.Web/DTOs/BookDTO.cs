using Library.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Title { get; set; }
        [MaxLength(128, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Description { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public DateTime PublishDate { get; set; }

        public IEnumerable<SelectListItem>? Authors { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Autor.")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int AuthorId { get; set; }

        [MaxLength(32, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Editorial { get; set; }

    }
}
