using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Indeavor.Client.Models;
using Indeavor.Client.Data;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace Indeavor.Client.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; }

        public HomeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Skills()
        {
            SkillResults results;

            System.Net.ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.DefaultConnectionLimit = 9999;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Configuration.GetSection("AppSettings")["RootUrl"] + Configuration.GetSection("AppSettings")["SkillsAction"]);
            request.Method = WebRequestMethods.Http.Get;
            request.Timeout = 30000;
            request.ContentType = "text/json";

            try
            {
                WebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                {
                    string response = streamReader.ReadToEnd();
                    results = JsonConvert.DeserializeObject<SkillResults>(response);
                    streamReader.Close();
                }
                return View(results);
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    {
                        string text = new StreamReader(data).ReadToEnd();
                    }
                }
            }

            return View();
        }

        public IActionResult SkillDetails(int? id, string get)
        {
            Skill skill = new Skill();
            if (id.HasValue)
            {
                System.Net.ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.DefaultConnectionLimit = 9999;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Configuration.GetSection("AppSettings")["RootUrl"] + Configuration.GetSection("AppSettings")["SkillDetailsAction"] + "?id=" + id.Value.ToString());
                request.Method = WebRequestMethods.Http.Get;
                request.Timeout = 30000;
                request.ContentType = "text/json";

                try
                {
                    WebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        string response = streamReader.ReadToEnd();
                        skill = JsonConvert.DeserializeObject<Skill>(response);
                        streamReader.Close();
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        {
                            string text = new StreamReader(data).ReadToEnd();
                        }
                    }
                }
                if(get == "post")
                {
                    ViewBag.Updated = "1";
                }
            }
            return View(skill);
        }

        [HttpPost]
        public ActionResult SkillDetails(Skill obj)
        {
            Response resp = new Response();
            long id = obj.SkillId;
            try
            {
                System.Net.ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.DefaultConnectionLimit = 9999;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                HttpWebRequest request;
                if (obj.SkillId != 0)
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(Configuration.GetSection("AppSettings")["RootUrl"] + Configuration.GetSection("AppSettings")["UpdateSkillAction"]);
                }
                else
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(Configuration.GetSection("AppSettings")["RootUrl"] + Configuration.GetSection("AppSettings")["AddSkillAction"]);
                    obj.CreationDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                }
                request.Method = WebRequestMethods.Http.Post;
                request.Timeout = 30000;
                request.ContentType = "text/json";

                using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(obj));
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                try
                {
                    WebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        string response = streamReader.ReadToEnd();
                        resp = JsonConvert.DeserializeObject<Response>(response);
                        streamReader.Close();
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        {
                            string text = new StreamReader(data).ReadToEnd();
                        }
                    }
                }

                if (resp != null)
                {
                    if (resp.ErrorCode != "0")
                    {
                        ModelState.AddModelError("", resp.ErrorLabel);
                    }
                    else if (resp.ErrorCode == "0" && !string.IsNullOrEmpty(resp.Id))
                    {
                        id = long.Parse(resp.Id);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "General Error");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction("SkillDetails", new { @id = id, @get = "post" });
        }

        public JsonResult DeleteSkill(string ID)
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.DefaultConnectionLimit = 9999;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Configuration.GetSection("AppSettings")["RootUrl"] + Configuration.GetSection("AppSettings")["DeleteSkillAction"] + "?id=" + ID);
                request.Method = WebRequestMethods.Http.Post;
                request.Timeout = 30000;
                request.ContentType = "text/json";

                try
                {
                    WebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        string response = streamReader.ReadToEnd();
                        //skill = JsonConvert.DeserializeObject<Skill>(response);
                        streamReader.Close();
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        {
                            string text = new StreamReader(data).ReadToEnd();
                        }
                    }
                }
                return Json(new
                {
                    Result = true
                });
            }
            catch
            {
                return Json(new
                {
                    Result = false
                });
            }
        }

        public IActionResult Employees(int? id, string name, string surname)
        {
            Employees employees;

            System.Net.ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.DefaultConnectionLimit = 9999;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Configuration.GetSection("AppSettings")["RootUrl"] + Configuration.GetSection("AppSettings")["EmployeesAction"]);
            request.Method = WebRequestMethods.Http.Get;
            request.Timeout = 30000;
            request.ContentType = "text/json";

            try
            {
                WebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                {
                    string response = streamReader.ReadToEnd();
                    employees = JsonConvert.DeserializeObject<Employees>(response);
                    streamReader.Close();
                }

                if(id.HasValue)
                {
                    employees.SortMode = id.Value;
                    if(id.Value.ToString() == "1")
                    {
                        employees.employees = employees.employees.OrderBy(x => x.Surname).ToList();
                    }
                    else if(id.Value.ToString() == "-1")
                    {
                        employees.employees = employees.employees.OrderByDescending(x => x.Surname).ToList();
                    }
                    else if(id.Value.ToString() == "2")
                    {
                        employees.employees = employees.employees.OrderBy(x => x.HiringDate).ToList();
                    }
                    else if (id.Value.ToString() == "-2")
                    {
                        employees.employees = employees.employees.OrderByDescending(x => x.HiringDate).ToList();
                    }
                }
                else
                {
                    employees.SortMode = 1;
                }

                if(!string.IsNullOrEmpty(name))
                {
                    employees.employees = employees.employees.Where(x => x.Name.ToLower().Contains(name.ToLower()) || x.Name.ToLower().StartsWith(name.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(surname))
                {
                    employees.employees = employees.employees.Where(x => x.Surname.ToLower().Contains(surname.ToLower()) || x.Surname.ToLower().StartsWith(surname.ToLower())).ToList();
                }
                return View(employees);
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    {
                        string text = new StreamReader(data).ReadToEnd();
                    }
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult Employees(Employees obj)
        {
            return RedirectToAction("Employees", new { @id = obj.SortMode, @name = obj.Name, @surname = obj.Surname });
        }

        public IActionResult EmployeeDetails(int? id, string get)
        {
            Employee employee = new Employee();
            if (id.HasValue)
            {
                System.Net.ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.DefaultConnectionLimit = 9999;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Configuration.GetSection("AppSettings")["RootUrl"] + Configuration.GetSection("AppSettings")["EmployeeDetailsAction"] + "?id=" + id.Value.ToString());
                request.Method = WebRequestMethods.Http.Get;
                request.Timeout = 30000;
                request.ContentType = "text/json";

                try
                {
                    WebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        string response = streamReader.ReadToEnd();
                        employee = JsonConvert.DeserializeObject<Employee>(response);
                        string defaultDate = DateTime.ParseExact(employee.HiringDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                        employee.HiringDate = defaultDate;
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        {
                            string text = new StreamReader(data).ReadToEnd();
                        }
                    }
                }
                if (get == "post")
                {
                    ViewBag.Updated = "1";
                }
            }
            return View(employee);
        }

        [HttpPost]
        public ActionResult EmployeeDetails(Employee obj)
        {
            Response resp = new Response();
            long id = obj.EmployeeId;
            try
            {
                System.Net.ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.DefaultConnectionLimit = 9999;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                HttpWebRequest request;
                if (obj.EmployeeId != 0)
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(Configuration.GetSection("AppSettings")["RootUrl"] + Configuration.GetSection("AppSettings")["UpdateEmployeeAction"]);
                }
                else
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(Configuration.GetSection("AppSettings")["RootUrl"] + Configuration.GetSection("AppSettings")["AddEmployeeAction"]);
                }
                request.Method = WebRequestMethods.Http.Post;
                request.Timeout = 30000;
                request.ContentType = "text/json";

                using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(obj));
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                try
                {
                    WebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        string response = streamReader.ReadToEnd();
                        resp = JsonConvert.DeserializeObject<Response>(response);
                        streamReader.Close();
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        {
                            string text = new StreamReader(data).ReadToEnd();
                        }
                    }
                }

                if (resp != null)
                {
                    if (resp.ErrorCode != "0")
                    {
                        ModelState.AddModelError("", resp.ErrorLabel);
                    }
                    else if (resp.ErrorCode == "0" && !string.IsNullOrEmpty(resp.Id))
                    {
                        id = long.Parse(resp.Id);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "General Error");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction("EmployeeDetails", new { @id = id, @get = "post" });
        }

        public JsonResult DeleteEmployees(string IDs)
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.DefaultConnectionLimit = 9999;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Configuration.GetSection("AppSettings")["RootUrl"] + Configuration.GetSection("AppSettings")["DeleteEmployeesAction"]);
                request.Method = WebRequestMethods.Http.Post;
                request.Timeout = 30000;
                request.ContentType = "text/json";

                List<string> Ids = !string.IsNullOrEmpty(IDs) ? IDs.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();

                using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(Ids));
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                try
                {
                    WebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        string response = streamReader.ReadToEnd();
                        //skill = JsonConvert.DeserializeObject<Skill>(response);
                        streamReader.Close();
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        {
                            string text = new StreamReader(data).ReadToEnd();
                        }
                    }
                }
                return Json(new
                {
                    Result = true
                });
            }
            catch
            {
                return Json(new
                {
                    Result = false
                });
            }
        }


        public JsonResult AddSkillToEmployee(string employeeid, string skillid)
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.DefaultConnectionLimit = 9999;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Configuration.GetSection("AppSettings")["RootUrl"] + Configuration.GetSection("AppSettings")["AddEmployeeSkillAction"] + "?employeeid=" + employeeid + "&skillid=" + skillid);
                request.Method = WebRequestMethods.Http.Post;
                request.Timeout = 30000;
                request.ContentType = "text/json";

                try
                {
                    WebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        string response = streamReader.ReadToEnd();
                        //skill = JsonConvert.DeserializeObject<Skill>(response);
                        streamReader.Close();
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        {
                            string text = new StreamReader(data).ReadToEnd();
                        }
                    }
                }
                return Json(new
                {
                    Result = true
                });
            }
            catch
            {
                return Json(new
                {
                    Result = false
                });
            }
        }


        public JsonResult DeleteEmployeeSkill(string employeeid, string skillid)
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.DefaultConnectionLimit = 9999;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Configuration.GetSection("AppSettings")["RootUrl"] + Configuration.GetSection("AppSettings")["DeleteEmployeeSkillAction"] + "?employeeid=" + employeeid + "&skillid=" + skillid);
                request.Method = WebRequestMethods.Http.Post;
                request.Timeout = 30000;
                request.ContentType = "text/json";

                try
                {
                    WebResponse httpResponse = (HttpWebResponse)request.GetResponse();
                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        string response = streamReader.ReadToEnd();
                        //skill = JsonConvert.DeserializeObject<Skill>(response);
                        streamReader.Close();
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        {
                            string text = new StreamReader(data).ReadToEnd();
                        }
                    }
                }
                return Json(new
                {
                    Result = true
                });
            }
            catch
            {
                return Json(new
                {
                    Result = false
                });
            }
        }
    }
}
