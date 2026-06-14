using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace E_StoreMVCTemplate.Web.ViewComponents
{
    public class RelatedProductsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public RelatedProductsViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int Id )
        {
            var relatedProducts = await _productService.GetRelatedProductAsync(Id);
            return View(relatedProducts);
        }

    }
}
