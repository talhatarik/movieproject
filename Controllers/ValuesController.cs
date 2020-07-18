using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using restapi.Models;
using restapi.Types;
using MailKit.Net.Smtp;
using MimeKit;

namespace restapi.Controllers
{

    [Route("api")]
    public class ValuesController : Controller
    {
        // GET api
        [HttpGet]
        public JsonResult API()
        {
            JsonResult jsonResult = new JsonResult("API Çalışıyor");
            return jsonResult;
        }

        // GET /sayfaNo/sayfadakiKayıtSayısı/accessToken
        [HttpGet("{pageNumber}/{pageSize}/{accessToken}")]
        public List<MovieContract> GetMovieList(int pageNumber, int pageSize, string accessToken)
        {
            if (restapi.Models.MongoDB.GetAuthControl(accessToken) == true)
            {
                return restapi.Models.MongoDB.GetMovieList(pageNumber, pageSize);
            }
            else
            {
                List<MovieContract> movie = new List<MovieContract>();
                return movie;
            }

        }


        // GET /film no/notlar/point/accessToken
        [HttpGet("{filmId}/{note}/{point}")]
        public JsonResult UpdateNoteAndPoint(int filmId, string notes, int point, string accessToken)
        {

            if (restapi.Models.MongoDB.GetAuthControl(accessToken) == true)
            {
                var repsonseFilmDetail = restapi.Models.MongoDB.GetMovieDetail(filmId);

                if(repsonseFilmDetail != null)
                {
                    repsonseFilmDetail.MyNotes = notes;
                    repsonseFilmDetail.MyRating = point;

                    var responseUpdate = restapi.Models.MongoDB.UpdateMovie(repsonseFilmDetail);

                    if(responseUpdate == true)
                    {
                        return new JsonResult("Güncelleme işlemi başarılı oldu");
                    }
                    else
                    {
                        return new JsonResult("Güncelleme işlemi başarısız oldu");
                    }

                }
                else
                {
                    return new JsonResult("Film Bulunamadı");
                }
            }

            return new JsonResult("Hatalı Token, işlem başarısız.");

        }


        // GET /filmId/accessToken
        [HttpGet("{filmId}/{accessToken}")]
        public MovieContract GetFilmDetail(int filmId,string accessToken)
        {

            if (restapi.Models.MongoDB.GetAuthControl(accessToken) == true)
            {
                var repsonseFilmDetail = restapi.Models.MongoDB.GetMovieDetail(filmId);

                return repsonseFilmDetail;
            }
            else
            {
                return new MovieContract { Description = "Token hatalı, işlem Başarısız" };
            }

        }

        // GET /filmId/accessToken
        [HttpGet("{toEmail}/{filmId}/{accessToken}")]
        public JsonResult SendEmail(string toEmail, int filmId, string accessToken)
        {

            if (restapi.Models.MongoDB.GetAuthControl(accessToken) == true)
            {

                var repsonseFilmDetail = restapi.Models.MongoDB.GetMovieDetail(filmId);


                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("Film Projesi",
                "info@filmprojesi.com");
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress("Arkadaşım",
                toEmail);
                message.To.Add(to);

                message.Subject = "Arkadaşın sana bir film önerdi.";

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = "FilmAdı: "+repsonseFilmDetail.Name;

                SmtpClient client = new SmtpClient();
                client.Connect("smtp_address_here", 2020, true);
                client.Authenticate("user_name_here", "pwd_here");

                client.Send(message);
                client.Disconnect(true);
                client.Dispose();


                return new JsonResult("Gönderildi");
            }
            else
            {
                return new JsonResult("Token hatalı, gönderilemedi");
            }
        }

    }
}
