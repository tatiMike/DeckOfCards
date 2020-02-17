using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DeckOfCards.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeckOfCards.Controllers
{
    public class CardController : Controller
    {
        // this Json document object will eventually pull our cards out after we draw from the deck
        private JsonDocument jDoc;

        //1st) make a method that draws a new deck -- this will give us access to a deck id
        public async Task<string> GetDeck()
        {
            // make using block to hold HttpClient object
            using (var httpClient = new HttpClient())
            {
                // make another using block to make an async call to the API
                using (var response = await httpClient.GetAsync("https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count=1"))
                {
                    // read our API response as a string
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    // parse the string and return a JsonDocument
                    jDoc = JsonDocument.Parse(stringResponse);
                }
            }

            // we return the "deck_id" as a string that we will get from a property of our JsonDocument
            return jDoc.RootElement.GetProperty("deck_id").GetString();
        }

        // (1) make an async method that will call the Deck of Cards API 
        // -- and return to use a number of cards --
        // (2) put those in a List of Cards and return them
        public async Task<List<Card>> GetCards(string deckId)
        {
            // make a blank List of Cards to add cards to later
            List<Card> cardList = new List<Card>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.
                    GetAsync($"https://deckofcardsapi.com/api/deck/{deckId}/draw/?count=6"))
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    jDoc = JsonDocument.Parse(stringResponse);

                    // this grabbed the data that formed our card array in Json format
                    var jsonList = jDoc.RootElement.GetProperty("cards");

                    // we will use a for loop to iterate through that card array
                    // and build our List of cards to return
                    for (int i = 0; i < jsonList.GetArrayLength(); i++)
                    {
                        // we parse through the data whie building a new Card and then add that Card to a our List of Cards
                        cardList.Add(new Card()
                        {
                            image = jsonList[i].GetProperty("image").GetString(),
                            value = jsonList[i].GetProperty("value").GetString(),
                            suit = jsonList[i].GetProperty("suit").GetString(),
                            code = jsonList[i].GetProperty("code").GetString()
                        });
                    }
                }
            }

            return cardList;
        }

        //public async Task<string> ShuffleDeck(string deckId)
        //{
        //    Card shuffleCards = new Card();
        //    using (var httpClient = new HttpClient())
        //    {
        //        using (var response = await httpClient.
        //            GetAsync($"https://deckofcardsapi.com/api/deck/{deckId}/shuffle/"))
        //        {
        //            var stringResponse = await response.Content.ReadAsStringAsync();
        //            jDoc = JsonDocument.Parse(stringResponse);

        //            var jsonShuffle = jDoc.RootElement.GetProperty("shuffled");

        //            if (jsonShuffle = )
        //            {

        //            }
        //        }
        //    }
        //    return jsonSh
        //}
    }
}