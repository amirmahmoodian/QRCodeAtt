using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace QRCodeAttendance.Models
{
    public class QRCodeModel
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        [NotMapped]
        public IFormFile FileUri { get; set; }
        public string ActtualFileUrl { get; set; }
        [NotMapped]
        public DateTime lastUserDate { get; set; }
    }
}