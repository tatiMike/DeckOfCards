using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DeckOfCards.Models;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
//FIRST CLASS ON API's

namespace DeckOfCards.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        // make a global card controller object that allows me to call the Deck of Cards API
        CardController cardController = new CardController();
        

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var deckid = await cardController.GetDeck();

            // we use Sessions to hold data as a key value pair
            HttpContext.Session.SetString("deckId", deckid);

            //var testSession = HttpContext.Session.GetString("deckId");

            return View();
        }

        // we make an async method to call the CardController and return to us a number of cards
        public async Task<IActionResult> GetCards()
        {
            List<Card> cardList = await cardController.GetCards(HttpContext.Session.GetString("deckId"));
            return View(cardList);
        }

        //public async Task<IActionResult> ShuffleDeck()
        //{
        //    var deckid = await cardController.ShuffleDeck(HttpContext.Session.GetString("deckid"));
        //    return View();
        //}


    //(about async) we make the methd async, so that we can await async method calls
    //inside this method
    //(about Task<T>) Task keyword is saying that the return process will be an async process
    //(about Using blocks) using blocks in our code, assure that the objects resources/memory ar released to be used again

    //public async Task<IActionResult> Index()
    //{
    //    //using this string to hold response before it gets released
    //    string apiResponse = "";

    //    //1st) make "new" HttpClient object in a using block
    //    //1.5) call in using statement for HttpClient
    //    using (var httpClient = new HttpClient())
    //    {
    //        //2nd) make another using block for these internal processes
    //        //2.5) call the GetAsync method and await the response
    //        using (var response = await httpClient.GetAsync("https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count=1"))
    //        {
    //            //3rd) read the response with ReadAsStringAsync method which we will also await
    //            apiResponse = await response.Content.ReadAsStringAsync();

    //            //parses our Json String into a JsonDocument object
    //            var jsonDocument = JsonDocument.Parse(apiResponse);

    //            //GetProperty or "Index" through  JsonDocument object, then we call the GetString method to retrieve value from index
    //            //call correct type (Get"Type")
    //            var jsonProperty = jsonDocument.RootElement.GetProperty("deck_id").GetString();

    //            var cardModel = JsonSerializer.Deserialize<Card>(apiResponse);

    //        }//these curly brackets release the resources used to call the api

    //    }//these curly brackets release the memory used to hold the HttpClient
    //    return View();
    //}

    public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
