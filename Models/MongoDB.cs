using System;
using System.Collections.Generic;
using restapi.Types;
using MongoDB.Driver;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace restapi.Models
{
    public static class MongoDB
    {

        public static IMongoDatabase MovieService()
        {
            var _conn = string.Format(@"mongodb://{0}:{1}@{2}:{3}/{4}"
                    , "heroku_z5fgsdl2"
                    , "heroku_z5fgsdl2"
                    , "ds241977.mlab.com"
                    , 41977
                    , "heroku_z5fgsdl1");


            var _client = new MongoClient(_conn);
            var _database = _client.GetDatabase("heroku_z5fgsdl1");

            return _database;

        }



        public static IMongoDatabase AuhtService()
        {

            var _conn = string.Format(@"mongodb://{0}:{1}@{2}:{3}/{4}"
                    , "heroku_z5fgsdl2"
                    , "heroku_z5fgsdl2"
                    , "ds241977.mlab.com"
                    , 41977
                    , "heroku_z5fgsdl1");


            var _client = new MongoClient(_conn);
            var _database = _client.GetDatabase("heroku_z5fgsdl1");

            return _database;

        }

        public static List<MovieContract> GetMovieList(int page, int pageSize)
        {
            var movieList =  MovieService().GetCollection<MovieContract>("Movie").Find(null).ToList();

            return movieList.Skip(page * pageSize).Take(pageSize).ToList();
        }


        public static MovieContract GetMovieDetail(int id)
        {
            return MovieService().GetCollection<MovieContract>("Movie").Find(movie => movie.Id == id).FirstOrDefault();
        }


        public static bool CreateMovie(MovieContract movie)
        {
            MovieService().GetCollection<MovieContract>("Movie").InsertOne(movie);

            return true;
        }

        public static bool UpdateMovie(MovieContract movieUpdate)
        {

            MovieService().GetCollection<MovieContract>("Movie").ReplaceOne(movie => movie.Id == movieUpdate.Id, movieUpdate);

            return true;
        }


        public static bool GetAuthControl(string accessToken)
        {
            var response = AuhtService().GetCollection<AuthContract>("Auth").Find(auth => auth.AccessToken == accessToken).FirstOrDefault();

            if(response.Password != null)
            {
                return true;

            }

            return false;
        }

        public static bool AddNewAuth(string accessToken, string password, string userName)
        {
            var contract = new AuthContract();
            contract.UserName = userName;
            contract.Password = password;
            contract.StartTime = DateTime.Now;
            contract.AccessToken = Sha256("admin");
            var response = AuhtService().GetCollection<AuthContract>("Auth");

            try
            {
                response.InsertOne(contract);
                return true;
            }
            catch
            {
                return false;
            }

        }

        static string Sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

    }
}