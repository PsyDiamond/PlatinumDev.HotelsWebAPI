public class HotelAPI : IAPI
{
    public void Register(WebApplication app)
    {
        app.MapGet("/hotels", Get)
            .Produces<List<Hotel>>(StatusCodes.Status200OK)
            .WithName("GetAllHotels")
            .WithTags("Getters");

        app.MapGet("/hotels/{id}", GetById)
            .Produces<Hotel>(StatusCodes.Status200OK)
            .WithName("GetHotel")
            .WithTags("Getters");

        app.MapPost("/hotels", Post)
            .Accepts<Hotel>("application/json")
            .Produces<Hotel>(StatusCodes.Status201Created)
            .WithName("CreateHotel")
            .WithTags("Setters");

        app.MapPut("/hotels", Put)
            .Accepts<Hotel>("application/json")
            .WithName("UpdateHotel")
            .WithTags("Updaters");

        app.MapDelete("/hotels/{id}", Delete)
            .WithName("DeleteHotel")
            .WithTags("Deleters");

        app.MapGet("/hotels/search/name/{query}",SearchByName)
            .Produces<List<Hotel>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("SearchHotels")
            .WithTags("Getters")
            .ExcludeFromDescription();

        app.MapGet("/hotels/search/location/{coordinate}", SearchByCoordinate)
            .Produces<List<Hotel>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("SearchHotelsCoordinate")
            .WithTags("Getters")
            .ExcludeFromDescription();
    }

    [Authorize]
    async Task<IResult> Get(IHotelRepository repository) => 
        Results.Extensions.Xml(await repository.GetHotelsAsync());

    [Authorize]
    async Task<IResult> GetById(int id, IHotelRepository repository) => 
        await repository.GetHotelAsync(id) is Hotel hotel
        ? Results.Ok(hotel)
        : Results.NotFound();

    [Authorize]
    async Task<IResult> Post(
        [FromBody]
        Hotel hotel, 
        IHotelRepository repository) 
    {
        await repository.InsertHotelAsync(hotel);
        await repository.SaveAsync();
        return Results.Created($"/hotels/{hotel.Id}", hotel);
    }

    [Authorize]
    async Task<IResult> Put(
        [FromBody]
        Hotel hotel, 
        IHotelRepository repository)
    {
        await repository.UpdateHotelAsync(hotel);
        await repository.SaveAsync();
        return Results.NoContent();
    }

    [Authorize]
    async Task<IResult> Delete(int id, IHotelRepository repository) 
    {
        await repository.DeleteHotelAsync(id);
        await repository.SaveAsync();

        return Results.NoContent();
    }

    [Authorize]
    async Task<IResult> SearchByName(string query, IHotelRepository repository) =>
        await repository.GetHotelsAsync(query) is IEnumerable<Hotel> hotels
            ? Results.Ok(hotels)
            : Results.NotFound(Array.Empty<Hotel>());

    [Authorize]
    async Task<IResult> SearchByCoordinate(Coordinate coordinate, IHotelRepository repository) => 
        await repository.GetHotelsAsync(coordinate) is IEnumerable<Hotel> hotels
            ? Results.Ok(hotels)
            : Results.NotFound(Array.Empty<Hotel>());
}