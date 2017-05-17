using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPIDemo.Controllers
{
    public class EmployeesController : ApiController
    {
        public IEnumerable<Employee> Get()
        {
            using (WebApiDemoEntities db = new WebApiDemoEntities())
            {
                return db.Employees.ToList();
            }

        }

        public HttpResponseMessage Get(int id)
        {
            using (WebApiDemoEntities db = new WebApiDemoEntities())
            {
                var entity = db.Employees.FirstOrDefault(e => e.ID == id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);

                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id: " + id.ToString() + " not found");
                }

            }

        }

        public HttpResponseMessage Post([FromBody]Employee employee)
        {

            try
            {
            using (WebApiDemoEntities db = new WebApiDemoEntities())
            {
                db.Employees.Add(employee);
                db.SaveChanges();


                var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                return message;
            }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
            using (WebApiDemoEntities db = new WebApiDemoEntities())
            {
                var entity = db.Employees.FirstOrDefault(e => e.ID == id);
                if(entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id: " + id.ToString() + " not found to delete");
                }
                else
                {
                    db.Employees.Remove(entity);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }


            }
            }

            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }





    }
}
