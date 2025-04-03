using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantService.Features.CreateRestaurant;
using RestaurantService.Features.GetRestaurantById;
using RestaurantService.Features.SearchRestaurants;

namespace RestaurantService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRestaurantCommand command)
        {
            var id = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));
            if (restaurant == null)
            {
                return NotFound();
            }
            return Ok(restaurant);
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string? name)
        {
            var restaurants = await mediator.Send(new SearchRestaurantsQuery(name));
            return Ok(restaurants);
        }
    }
}
