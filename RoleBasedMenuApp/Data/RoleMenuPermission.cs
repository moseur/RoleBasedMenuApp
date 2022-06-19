using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedMenuApp.Data
{
	[Table(name: "AspNetRoleMenuPermission")]
	public class RoleMenuPermission
	{
		public string RoleId { get; set; }

		public Guid NavigationMenuId { get; set; }

		public NavigationMenu NavigationMenu { get; set; }
	}
}
