using Microsoft.AspNetCore.Mvc.Rendering;

namespace RestoranASP.Models.ViewModels
{
    public class JeloFilterViewModel
    {
        public PagedResult<Jelo> PagedJela {  get; set; }

        public SelectList CategorySelectList { get; set; }

        public int? SelectedCategoryId { get; set; }

        public string SearchTerm {  get; set; }

        public string SortOrder { get; set; }

    }
}
