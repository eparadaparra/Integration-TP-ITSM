using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tasksAction.Conn;
using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace tasksAction.Controllers
{

    [Authorize]
    [ApiController]
    public class CadaArchivoController : ControllerBase
    {
        [HttpGet]
        [Route("ChangeName")]
        public async Task<IActionResult> ChangeNameFile()
        {

            string folderPath = new CustomException().LogPath();
            //string folderPath = "\\\\192.168.22.46\\App_Data";
            string searchString = "05 Programada";
            string changeTo = "05 Vencida";
            int countChanges = 0;

            //Console.WriteLine(folderPath + "\n");
            string[] files = Directory.GetFiles(folderPath);

            foreach (string filePath in files)
            {
                string fileName = Path.GetFileName(filePath);

                if (fileName.Contains(searchString))
                {
                    Console.WriteLine(fileName);
                    
                    string newFileName = fileName.Replace(searchString, changeTo);
                    string newFilePath = Path.Combine(folderPath, newFileName);
                    
                    Console.WriteLine(newFileName);
                    System.IO.File.Move(filePath, newFilePath); //RENOMBRA ARCHIVO

                    countChanges++;
                    Console.WriteLine("\n");
                    //break;
                }    
            }

            if (countChanges > 0) { Console.WriteLine($"{countChanges} Archivos renombrados"); }
                            
            return StatusCode(StatusCodes.Status200OK, new { files } );
        }
    }
}
