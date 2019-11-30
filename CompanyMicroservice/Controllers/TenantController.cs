using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CompanyMicroservice.Services;
using SharedMicroservice.DTO;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace CompanyMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {

        private readonly ITenantService _tenantService;
        private readonly ILogger _logger;


        public TenantController(ITenantService tenantService, ILogger<TenantController> logger)
        {
            _tenantService = tenantService;
            _logger = logger;
        }

        /// <summary>
        /// Retorna todos os tenants existentes
        /// </summary>
        /// <returns>List de objetos tenant contendo os dados dos tenants.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            
            var tenants = _tenantService.ReturnAll();
            return new OkObjectResult(tenants);
        }

        /// <summary>
        /// Retorna um tenant em específico (pelo Id)
        /// </summary>
        /// <param name="id">Id do Tenant</param>
        /// <returns>Objeto tenant contendo os dados do tenant solicitado.</returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var tenant = _tenantService.ReturnById(id);
            if (tenant == null)
                return new NotFoundResult();
            return new OkObjectResult(tenant);
        }

        /// <summary>
        /// Retorna um tenant em específico (pelo host name)
        /// </summary>
        /// <param name="hostName">Host name do Tenant</param>
        /// <returns>Objeto tenant contendo os dados do tenant solicitado.</returns>
        [Route("GetByHostName/{hostName}")]
        [HttpGet]
        public IActionResult GetByHostName(string hostName)
        {
            var tenant = _tenantService.ReturnByHostName(hostName);

            if (tenant == null)
                return new NoContentResult();
            else
                return new OkObjectResult(tenant);
        }


        /// <summary>
        /// Inclui um tenant
        /// </summary>
        /// <param name="tenantDTO">Objeto Tenant</param>
        /// <returns>Id do tenant cadastrado.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] TenantDTO tenantDTO)
        {
            int dto_id = _tenantService.Add(tenantDTO);
            return CreatedAtAction(nameof(Get), new { id = dto_id });
        }

        /// <summary>
        /// Altera um tenant
        /// </summary>
        /// <param name="tenantDTO">Objeto Tenant</param>
        /// <returns>Ok/NoOk result.</returns>
        [HttpPut]
        public IActionResult Put([FromBody] TenantDTO tenantDTO)
        {
            if (tenantDTO != null)
            {
                _tenantService.Alter(tenantDTO);
                return new OkResult();
            }
            return new NoContentResult();
        }

        /// <summary>
        /// Exclui uma empresa
        /// </summary>
        /// <param name="id">Id da empresa</param>
        /// <returns>Ok result ou exception.</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _tenantService.Remove(id);
            return new OkResult();
        }

    }
}
