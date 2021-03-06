﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using PhoneDirectoryLibrary;
using System.Web.Http.Cors;
using System.Web.Http.Results;

namespace RoboDexApi.Controllers
{
    [EnableCors("*", "*", "*")]
    public class ContactController : ApiController
    {
        PhoneDirectory phoneDirectory = new PhoneDirectory();

        [HttpGet]
        public IHttpActionResult Get(string id = "")
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();

            if (id.Length == 0)
            {
                return Json<IEnumerable<Contact>>(phoneDirectory.GetAll());
            }
            else
            {
                Guid contactGuid;

                if (Guid.TryParse(id, out contactGuid))
                {
                    List<Contact> contacts = new List<Contact>();

                    try
                    {
                        contacts.Add(phoneDirectory.GetContactFromDB(contactGuid));
                    }
                    catch(ArgumentNullException e)
                    {
                        logger.Error(e.Message);
                        return BadRequest("No contact with that ID.");
                    }

                    return Json<IEnumerable<Contact>>(contacts);
                }

                else
                {
                    //HttpResponseMessage message = new HttpResponseMessage();
                    //message.Content = new StringContent("<html><body><div>No contacts with that ID.</div></body></html>");
                    //message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
                    //message.StatusCode = HttpStatusCode.InternalServerError;
                    //return ResponseMessage(message);
                    return BadRequest("No contact with that ID");
                }
            }
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]Contact contact)
        {
            if(contact == null)
            {
                return BadRequest("No data supplied.");
            }
            else
            {
                return Json(phoneDirectory.Add(contact));
            }
        }
    }
}
