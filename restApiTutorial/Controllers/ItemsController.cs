using Microsoft.AspNetCore.Mvc;
using restApiTutorial.Dtos;
using restApiTutorial.Entities;
using restApiTutorial.Repositories;

namespace restApiTutorial.Controllers
{
    [ApiController]
    [Route ("items")]
    public class ItemsController: ControllerBase
    {
        private readonly IItemsRepository repository;

        //created additionall interface and the constructor down below provides us to find items via ID (doesnt create new repository each request) Its called "Dependency"
        public ItemsController(IItemsRepository repository)
        {
            this.repository = repository;
        }


        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            var items = (await repository.GetItemsAsync())
                .Select(item => item.AsDto());
            return items;
        }

        [HttpGet("{id}")]
        public async Task< ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await repository.GetItemAsync(id);

            if(item==null)
            {
                return NotFound();
            }

            return item.AsDto();
        }
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow,
            };

            await repository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
        }
        //PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = await repository.GetItemAsync(id);
            if(existingItem==null)
            {
                return NotFound();
            }

            Item updatedItem = existingItem with //creates copy of existing Item
            {
                Name = itemDto.Name,
                Price = itemDto.Price,
            };

            await repository.UpdateItemAsync(updatedItem);

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task< ActionResult> DelteItemAsync(Guid id)
        {
            var existingItem = await repository.GetItemAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            await repository.DeleteItemAsync(id);

            return NoContent();
        }
    }
}
