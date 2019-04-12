using Microsoft.AspNetCore.Mvc;  
using Rotativa.AspNetCore;  
  
  
namespace DemoRotativa.Controllers  
{  
    public class DemoController : Controller  
    {  
        public IActionResult DemoViewAsPDF()  
        {  
            return new ViewAsPdf("DemoViewAsPDF");  
        }  
    }  
}