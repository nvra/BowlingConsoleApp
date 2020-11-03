using BowlingConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BowlingConsoleApp
{
    class Program
    {
        private static readonly HttpClient _client = new HttpClient();
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to 10 Pin Bowling!");

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            _client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            Console.WriteLine("New User/Existing User?  Press 'n' for New User or any key for Existing User.");
            var user = Console.ReadLine();
            if (string.Equals(user.ToUpper(), "N"))
            {
                user = await AddPlayer();
            }
            else
            {
                Console.WriteLine("Enter user name");
                user = Console.ReadLine();
            }
            await GetPlayer(user);

            var userId = await StartGame();
            var gameId = await GetGameId(userId);

            await PlayGame(gameId, userId);
           
            Console.WriteLine();
            Console.WriteLine("Final Score");
            await GetScores(gameId);
        }

        private static async Task<string> AddPlayer()
        {
            Console.WriteLine("Enter Player Name");
            var name = Console.ReadLine();

            using var httpResponse =
                await _client.PostAsync($"http://localhost:64454/api/player/{name}", null);

            httpResponse.EnsureSuccessStatusCode();

            return name;
        }

        private static async Task GetPlayer(string name)
        {
            var responseTask = _client.GetStreamAsync($"http://localhost:64454/api/player/{name}");
            var players = await JsonSerializer.DeserializeAsync<List<Player>>(await responseTask);

            foreach (var player in players)
            {
                Console.WriteLine($"Id : {player.id}    Name : {player.name}");
            }
        }

        private static async Task<string> StartGame()
        {
            Console.WriteLine("Enter Player Id to start the game");
            var id = Console.ReadLine();

            using var httpResponse =
                await _client.PostAsync($"http://localhost:64454/api/startgame/{id}", null);

            httpResponse.EnsureSuccessStatusCode();

            return id;
        }

        private static async Task<string> GetGameId(string playerId)
        {
            var responseTask = _client.GetStringAsync($"http://localhost:64454/api/getGameId/{playerId}");
            var gameId = await responseTask;

            Console.WriteLine($"Game Id : {gameId}");

            return gameId;
        }

        private static async Task PostAsync(FramethrowRequest request)
        {

            var requestJson = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            using var httpResponse =
                await _client.PostAsync("http://localhost:64454/api/FrameThrowScore", requestJson);

            httpResponse.EnsureSuccessStatusCode();
        }

        private static async Task GetScores(string gameId)
        {
            var responseTask = _client.GetStreamAsync($"http://localhost:64454/api/gameScores/{gameId}");
            var scores = await JsonSerializer.DeserializeAsync<BowlingResponse>(await responseTask);

            Console.WriteLine();
            Console.WriteLine($"Player Name : {scores.playerName}");
            Console.WriteLine($"Game Id : {scores.gameId}");
            Console.WriteLine($"Game Total Score : {scores.totalScore}");

            foreach (var frame in scores.frames)
            {
                Console.WriteLine();
                Console.WriteLine($"Frame {frame.frameNum} Total Score : {frame.totalScore}");
                foreach (var thrw in frame.throws)
                {
                    Console.WriteLine($"        Throw {thrw.throwNum} Score : {thrw.score}");
                }
            }
        }

        private static async Task PlayGame(string gameId, string playerId)
        {
            var score = 0;
            var totalScore = 0;
            var request = new FramethrowRequest();
            for (int frame = 1; frame < 11; frame++)
            {
                Console.WriteLine($"Frame {frame}");
                totalScore = 0;
                for (int thrw = 1; thrw <= 3; thrw++)
                {
                    Console.WriteLine($"Enter score for Throw {thrw}");
                    int.TryParse(Console.ReadLine(), out score);

                    request = new FramethrowRequest
                    {
                        GameId = Convert.ToInt32(gameId),
                        FrameNum = frame,
                        ThrowNum = thrw,
                        Score = score
                    };

                    await PostAsync(request);
                    totalScore = totalScore + score;

                    if (frame != 10)
                    {
                        if (thrw == 1 && score == 10) //strike
                        {
                            break;
                        }
                        else if (thrw == 2)
                        {
                            break;
                        }
                    }
                    else 
                    {
                        if (score == 10 && thrw == 1) // extra chance for frame10 if all the pins are cleared.
                        {
                            thrw++;
                        }
                        else if (totalScore != 10 && thrw == 2)
                        {
                            break;
                        }
                    }
                }

                await GetScores(gameId);
            }

            Console.WriteLine("GAME FINISHED!!");
            Console.WriteLine("Press any key to view the final score.");
            Console.ReadLine();
        }
    }
}
