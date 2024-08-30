using ClosedXML.Excel;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using PACAdminApp.Models;

namespace PACAdminApp.Controllers
{
    public class PetsController : Controller
    {

        public PetsController() {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        public IActionResult Index()
        {
            HttpClient client = new HttpClient();

            string URL = "https://petadoptioncenterapplication.azurewebsites.net/api/PacAdmin/ListAllPets";

            HttpResponseMessage response = client.GetAsync(URL).Result;

            var data = response.Content.ReadAsAsync<List<Pet>>().Result;

            return View(data);
        }

        public IActionResult Details(Guid id)
        {
            HttpClient client = new HttpClient();

            string URL = "https://petadoptioncenterapplication.azurewebsites.net/api/PacAdmin/GetPetDetails/" + id.ToString();

            HttpResponseMessage response = client.GetAsync(URL).Result;

            var data = response.Content.ReadAsAsync<Pet>().Result;

            return View(data);
        }

        public FileContentResult CreatePetDetailsDocument(Guid? id)
        {

            HttpClient client = new HttpClient();

            string URL = "https://petadoptioncenterapplication.azurewebsites.net/api/PacAdmin/GetPetDetails/" + id.ToString();

            HttpResponseMessage response = client.GetAsync(URL).Result;

            var pet = response.Content.ReadAsAsync<Pet>().Result;
            

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "PetDetails.docx");
            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{PetName}}", pet.Name);
            document.Content.Replace("{{Shelter}}", pet.Shelter.FirstName);
            document.Content.Replace("{{Type}}", pet.PetType.ToString());
            document.Content.Replace("{{Breed}}", pet.Breed);
            document.Content.Replace("{{Sex}}", pet.Sex);
            document.Content.Replace("{{Description}}", pet.Description);

            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportPetDetails.pdf");

        }

        public FileContentResult ExportAllPets()
        {
            string fileName = "MyPets.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Pets");
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Name";
                worksheet.Cell(1, 3).Value = "Type";
                worksheet.Cell(1, 4).Value = "Breed";
                worksheet.Cell(1, 5).Value = "Sex";
                worksheet.Cell(1, 6).Value = "Description";
                //worksheet.Cell(1, 7).Value = "Number of applications";
                worksheet.Cell(1, 8).Value = "Status";



                HttpClient client = new HttpClient();

                string URL = "https://petadoptioncenterapplication.azurewebsites.net/api/PacAdmin/ListAllPets";

                HttpResponseMessage response = client.GetAsync(URL).Result;

                var pets = response.Content.ReadAsAsync<List<Pet>>().Result;


                for (int i = 0; i < pets.Count(); i++)
                {
                    var pet = pets[i];

                    worksheet.Cell(i + 2, 1).Value = pet.Id.ToString();
                    worksheet.Cell(i + 2, 2).Value = pet.Name;
                    worksheet.Cell(i + 2, 3).Value = pet.PetType.ToString();
                    worksheet.Cell(i + 2, 4).Value = pet.Breed;
                    worksheet.Cell(i + 2, 5).Value = pet.Sex;
                    worksheet.Cell(i + 2, 6).Value = pet.Description;

                    //var num = _adoptionApplicationService.GetAdoptionApplicationsByPetId(pet.Id).Count();
                    //worksheet.Cell(i + 2, 7).Value = num.ToString();
                    worksheet.Cell(i + 2, 8).Value = pet.PetStatus.ToString();

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }
    }
}
