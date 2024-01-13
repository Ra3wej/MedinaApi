using MedinaApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedinaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MobilePatientsController : ControllerBase
    {
        private readonly Medina_Api_DbContext _context;
        public MobilePatientsController(Medina_Api_DbContext context)
        {
            _context = context;
        }

    }
}
