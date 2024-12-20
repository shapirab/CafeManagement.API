using AutoMapper;
using CafeManagement.Data.DataModels.DTOs;
using CafeManagement.Data.DataModels.Entities;
using CafeManagement.Data.DataModels.Models;
using CafeManagement.Data.Services;
using CafeManagement.Data.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CafeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IPurchaseOrderService purchaseOrderService;
        private readonly IMapper mapper;
        private readonly int maxPageSize = 20;

        public PurchaseOrdersController(IPurchaseOrderService purchaseOrderService, IMapper mapper)
        {
            this.purchaseOrderService = purchaseOrderService ?? throw new ArgumentNullException(nameof(purchaseOrderService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseOrder>>> GetAllPurchaseOrders
            (int? productId, int? userId, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }
            var (POEntities, paginationMetadata) = 
                await purchaseOrderService.GetAllPurchaseOrdersAsync(productId, userId, searchQuery, pageNumber, pageSize);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
            return Ok(mapper.Map<IEnumerable<PurchaseOrder>>(POEntities));
        }

        [HttpGet("{id}", Name = "GetPurchaseOrder")]
        public async Task<ActionResult<PurchaseOrder>> GetPurchaseOrderById(int id)
        {
            PurchaseOrderEntity? purchaseOrderEntity = await purchaseOrderService.GetPurchaseOrderByIdAsync(id);
            if(purchaseOrderEntity == null)
            {
                return NotFound("Purchase Order with the provided id was not found");
            }
            return Ok(mapper.Map<PurchaseOrder>(purchaseOrderEntity));
        }

        [HttpPost]
        public async Task<ActionResult<PurchaseOrder>> AddPurchaseOrder(PurchaseOrderDto purchaseOrder)
        {
            if(purchaseOrder == null)
            {
                return BadRequest("A purchase order was not provided");
            }

            PurchaseOrderEntity purchaseOrderEntity = mapper.Map<PurchaseOrderEntity>(purchaseOrder);
            await purchaseOrderService.AddPurchaseOrderAsync(purchaseOrderEntity);
            await purchaseOrderService.SaveChangesAsync();

            PurchaseOrder toReturn = mapper.Map<PurchaseOrder>(purchaseOrderEntity);
            return CreatedAtRoute("GetPurchaseOrder",
                new
                {
                    id = toReturn.Id
                }, toReturn);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdatePurchaseOrder(int id, PurchaseOrderDto updatedPurchaseOrder)
        {
            PurchaseOrderEntity? purchaseOrderEntity = await purchaseOrderService.GetPurchaseOrderByIdAsync(id);
            if(purchaseOrderEntity == null)
            {
                return BadRequest("Purchase Order with provided id was not found");
            }
            mapper.Map(updatedPurchaseOrder, purchaseOrderEntity);
            return Ok(await purchaseOrderService.SaveChangesAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeletePurchaseOrder(int id)
        {
            PurchaseOrderEntity? purchaseOrderEntity = await purchaseOrderService.GetPurchaseOrderByIdAsync(id);
            if (purchaseOrderEntity == null)
            {
                return BadRequest("Purchase Order with provided id was not found");
            }
            await purchaseOrderService.DeletePurchaseOrderAsync(id);
            return Ok(await purchaseOrderService.SaveChangesAsync());
        }
    }
}
