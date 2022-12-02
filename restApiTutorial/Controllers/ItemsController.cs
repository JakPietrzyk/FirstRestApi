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
        public IEnumerable<ItemDto> GetItems()
        {
            var items = repository.GetItems().Select(item => item.AsDto());
            return items;
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var item = repository.GetItem(id);

            if(item==null)
            {
                return NotFound();
            }

            return item.AsDto();
        }
        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow,
            };

            repository.CreateItem(item);

            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item.AsDto());
        }
        //PUT /items/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = repository.GetItem(id);
            if(existingItem==null)
            {
                return NotFound();
            }

            Item updatedItem = existingItem with //creates copy of existing Item
            {
                Name = itemDto.Name,
                Price = itemDto.Price,
            };

            repository.UpdateItem(updatedItem);

            return NoContent();
        }
        [HttpDelete("{id}")]
        public ActionResult DelteItem(Guid id)
        {
            var existingItem = repository.GetItem(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            repository.DeleteItem(id);

            return NoContent();
        }
    }
}
