using System.ComponentModel.DataAnnotations.Schema;
using AspNetCore.Identity.Mongo.Model;

namespace TeachingWebApi.Models.Identity
{
    public class TeachingBlazorUser : MongoUser
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CreatedBy { get; set; }

        [Column(TypeName = "text")]
        public string ProfilePictureDataUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsActive { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}

