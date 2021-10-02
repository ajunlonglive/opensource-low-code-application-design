using Domain.FormEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ISDAOC_Web.Controllers
{
    [Authorize]
    public class JsonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Builder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Builder([FromBody] object collection)
        {
            Components someType = JsonConvert.DeserializeObject<Components>(collection.ToString());
            HttpContext.Session.SetString("dForm", JsonConvert.SerializeObject(someType));
            return Json(new
            {
                newUrl = Url.Action("Renderer", "Json") //Payment as Action; Process as Controller
            });
        }

        public IActionResult Renderer()
        {
            var jsonmodel = HttpContext.Session.GetString("dForm");
            var model = JsonConvert.DeserializeObject<Components>(jsonmodel.ToString());
            return View(model);
        }

        [HttpPost]
        public JsonResult Renderer([FromBody] object collection)
        {
            var someType = JsonConvert.DeserializeObject<Dictionary<string, string>>(collection.ToString());
            HttpContext.Session.SetString("dFormValues", JsonConvert.SerializeObject(someType));
            return Json(new
            {
                newUrl = Url.Action("Presenter", "Json") //Payment as Action; Process as Controller
            });
        }

        public IActionResult Presenter()
        {
            var json = HttpContext.Session.GetString("dFormValues");
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.ToString());

            var jsonmodel = HttpContext.Session.GetString("dForm");
            var model = JsonConvert.DeserializeObject<Components>(jsonmodel.ToString());
            DataGridData data = new DataGridData();
            Dictionary<string, string> valueList = new Dictionary<string, string>();
            foreach (BaseComponent component in model.ComponentList)
            {
                valueList.Add(component.key, values[component.key]);
            }
            data.DataValueList = new DataGridDataChildren();
            data.DataValueList.DataValueList.Add(valueList);

            DataGridComponent dataGridComponent = new DataGridComponent();
            dataGridComponent.DataGridContainer.Add(new DataGridContainer { ComponentList = model.ComponentList });

            DataGridComponentWithData dataGrid = new DataGridComponentWithData { data = data, components = dataGridComponent };
            return View(dataGrid);
        }
    }
}