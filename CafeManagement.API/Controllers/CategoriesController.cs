using AutoMapper;
using CafeManagement.Data.DataModels.DTOs;
using CafeManagement.Data.DataModels.Entities;
using CafeManagement.Data.DataModels.Models;
using CafeManagement.Data.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CafeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        private readonly IProductService productService;
        private readonly IMapper mapper;
        private readonly int maxPageSize = 20;

        public CategoriesController(ICategoryService categoryService, IProductService productService, IMapper mapper)
        {
            this.categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories
            (string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if(pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }

            var(categoryEntities, paginationMetadata) = 
                await categoryService.GetAllCategoriesAsync(searchQuery, pageNumber, pageSize);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
            return Ok(mapper.Map<IEnumerable<Category>>(categoryEntities));
        }

        [HttpGet("{id}", Name = "GetCategory")]
        public async Task<ActionResult<Category>> GetCategoryById(int id, bool includeProducts)
        {
            CategoryEntity? categoryEntity = await categoryService.GetCategoryByIdAsync(id);
            if(categoryEntity == null)
            {
                return NotFound("A category with this id was not found");
            }

            if (includeProducts)
            {
                return Ok(mapper.Map<Category>(categoryEntity));
            }
            return Ok(mapper.Map<CategoryWithoutProductsDto>(categoryEntity));
        }

        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory(CategoryDto categoryDto)
        {
            if(categoryDto == null)
            {
                return BadRequest("Please provide a valid category to add");
            }
            CategoryEntity categoryEntity = mapper.Map<CategoryEntity>(categoryDto);
            await categoryService.AddCategoryAsync(categoryEntity);
            await categoryService.SaveChangesAsync();

            Category toReturn = mapper.Map<Category>(categoryEntity);

            return CreatedAtRoute("GetCategory",
                new
                {
                    id = toReturn.Id
                }, toReturn);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdateCategory(int id, CategoryDto updatedCategory)
        {
            CategoryEntity? categoryEntity = await categoryService.GetCategoryByIdAsync(id);
            if(categoryEntity == null)
            {
                return BadRequest("Category with provided id was not found");
            }
            mapper.Map(updatedCategory, categoryEntity);
            return Ok(await categoryService.SaveChangesAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteCategory(int id)
        {
            CategoryEntity? categoryEntity = await categoryService.GetCategoryByIdAsync(id);
            if (categoryEntity == null)
            {
                return BadRequest("Category with provided id was not found");
            }

            var (productEntitiesInCategory, paginationMetaData) = 
                await productService.GetAllProductsAsync(id, string.Empty, 1, 10);
            if(productEntitiesInCategory.Count() > 0)
            {
                return BadRequest("It is not possible to delete a category that includes products");
            }

            await categoryService.DeleteCategoryAsync(id);
            return Ok(await categoryService.SaveChangesAsync());
        }
    }
}
