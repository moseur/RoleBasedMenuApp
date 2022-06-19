using Microsoft.AspNetCore.Mvc;
using RoleBasedMenuApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedMenuApp.ViewComponents
{
	public class NavigationMenuViewComponent : ViewComponent
	{
		private readonly IDataAccessService _dataAccessService;

		public NavigationMenuViewComponent(IDataAccessService dataAccessService)
		{
			_dataAccessService = dataAccessService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var items = await _dataAccessService.GetMenuItemsAsync(HttpContext.User);

			return View(items);
		}
	}
}
