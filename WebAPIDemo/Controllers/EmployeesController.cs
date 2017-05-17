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

        public HttpResponseMessage Get(string gender ="All" )
        {
            using (WebApiDemoEntities db = new WebApiDemoEntities())
            {
                switch(gender.ToLower())
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK,
                            db.Employees.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK,
                            db.Employees.Where(e => e.Gender.ToLower() =="male").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK,
                            db.Employees.Where(e => e.Gender.ToLower() == "female").ToList());
                    default:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                            "Value for gender must be All, Male or Female." + gender + " is invalic.");




                }
            }

        }

        [HttpGet]
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


        public HttpResponseMessage Put (int id, [FromBody] Employee employee)
        {

            try
            {
            using (WebApiDemoEntities db = new WebApiDemoEntities())
            {
                var entity = db.Employees.FirstOrDefault(e => e.ID == id);

                if(entity==null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id: " + id.ToString() + " not found to update.");
                }
                else
                {
                entity.FirstName = employee.FirstName;
                entity.LastName = employee.LastName;
                entity.Gender = employee.Gender;
                entity.Salary = employee.Salary;

                db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }


            }

        
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }


        }





    }
}
