using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/categories")]
[Authorize]   // 🔒 THÊM DÒNG NÀY
public class CategoriesController : ControllerBase
{

}
