using Microsoft.AspNetCore.Mvc;
using restApiTutorial.Entities;
using restApiTutorial.Repositories;

namespace restApiTutorial.Controllers
{
    [ApiController]
    [Route ("items")]
    public class ItemsController: ControllerBase
    {
        private readonly InMemItemsRepository repository;

        public ItemsController()
        {
            repository = new InMemItemsRepository();
        }


        [HttpGet]
        public IEnumerable<Entities.Item> GetItems()
        {
            var items = repository.GetItems();
            return items;
        }

        [HttpGet("{id}")]
        public ActionResult<Item> GetItem(Guid id)
        {
            var item = repository.GetItem(id);

            if(item==null)
            {
                return NotFound();
            }

            return item;
        }
    }
}
