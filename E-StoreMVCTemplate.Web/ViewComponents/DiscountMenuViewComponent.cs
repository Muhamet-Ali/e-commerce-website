using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;



namespace E_StoreMVCTemplate.Web.ViewComponents
{
    public class DiscountMenuViewComponent:ViewComponent
    {
        private readonly IDiscountService _discountService;
        public DiscountMenuViewComponent(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
           var discounts=await _discountService.GetActiveDiscountsForUserAsync();

            if (discounts == null || !discounts.Any())
                return Content(string.Empty);

            return View(discounts);
        }




    }
}
